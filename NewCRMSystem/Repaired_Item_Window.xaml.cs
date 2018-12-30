using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for Repaired_Item_WIndow.xaml
    /// </summary>
    public partial class Repaired_Item_Window : Window
    {
        public Repaired_Item_Window()
        {
            try
            {
                InitializeComponent();
                bindDeliveryIDList();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void bindDeliveryIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , Delivery AS D , ComplaintItem AS CI WHERE D.destination_id = " + Login.LocID + " AND D.comp_item_id = CI.comp_item_id AND C.comp_id = CI.comp_id AND ( C.comp_status_id = 11 OR C.comp_status_id = 33 )  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadData(int compID)
        {
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size FROM ItemType as IT , ComplaintItem as CI WHERE CI.comp_id  = '" + compID + "' AND CI.item_type_id = IT.item_type_id  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
                txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
                txt_category.Text = dt.Rows[0]["item_category"].ToString();
                txt_name.Text = dt.Rows[0]["item_name"].ToString();
                txt_size.Text = dt.Rows[0]["item_size"].ToString();
            }
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Repair Remarks
            if (Validation.validate(repairRemarks_Notify, CRMdbData.Repair.repair_remarks.validate(cmb_compID.Text), CRMdbData.Repair.repair_remarks.Error)) { }
            else { check = false; }

            //Repaired Date
            if (Validation.validate(repairedDate_Notify, CRMdbData.Repair.repair_dt.validate(cmb_compID.Text), CRMdbData.Repair.repair_dt.Error)) { }
            else { check = false; }
            
            return check;
        }

        ~Repaired_Item_Window() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void cmb_compID_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_compID.Text.Length > 0)
                {
                    loadData(Int32.Parse(cmb_compID.Text));
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    string repairRemarks = txt_repairRemarks.Text;
                    string repairedDate = dt_repairedDate.Text;

                    string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') Update Repair SET repair_remarks = '" + repairRemarks + "' , repair_dt = '" + repairedDate + "' WHERE comp_item_id = @COMPitemID  ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 11 then 12 when comp_status_id = 33 then 34 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";

                    Database db = new Database();
                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        string query1 = "SELECT L.location_id , L.location_name FROM Location AS L WHERE location_type = 'HQ' ";
                        System.Data.DataTable dt = db.GetData(query1);

                        if (dt.Rows.Count == 1)
                        {
                            DatabaseBased_Objects.Location loc = new DatabaseBased_Objects.Location();
                            loc.locID = Int32.Parse(dt.Rows[0]["location_id"].ToString());
                            loc.locName = dt.Rows[0]["location_name"].ToString();
                            Login.b1.hideWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID , loc));
                        }
                        else
                        {
                            Login.b1.hideWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID));
                        }
                    }
                    else
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Failed();
                    }

                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
