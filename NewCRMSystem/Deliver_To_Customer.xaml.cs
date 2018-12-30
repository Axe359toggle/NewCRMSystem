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
    /// Interaction logic for Deliver_To_Customer.xaml
    /// </summary>
    public partial class Deliver_To_Customer : Window
    {
        public Deliver_To_Customer()
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
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

        public Deliver_To_Customer(int compID)
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                foreach (ComboBoxItem item in cmb_compID.Items)
                {
                    if (item.Content.ToString() == compID.ToString())
                    {
                        cmb_compID.SelectedValue = item;
                        break;
                    }
                }
                loadData(compID);
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

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , Delivery AS D , CustomerComplaint AS CC , ComplaintItem AS CI WHERE D.destination_id = " + Login.LocID + " AND D.comp_item_id = CI.comp_item_id AND CI.comp_id = C.comp_id AND ( C.comp_status_id = 16 OR C.comp_status_id = 21 ) AND C.comp_id = CC.comp_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadTxt_itemStatus(string itemDecision)
        {
            if (itemDecision.Equals("Repair"))
            {
                txt_itemStatus.Text = "Repaired";
            }
            else if (itemDecision.Equals("Investigation"))
            {
                txt_itemStatus.Text = "New Item";
            }
        }

        private void loadData(int compID)
        {
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , CI.item_defect , CI.item_remarks , CI.item_decision , R.repair_remarks , CC.cus_id FROM ItemType AS IT , ComplaintItem AS CI , Repair AS R , CustomerComplaint AS CC WHERE CI.comp_id  = '" + compID + "' AND CI.item_type_id = IT.item_type_id AND CI.comp_item_id = R.comp_item_id AND CI.comp_id = CC.comp_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
                txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
                txt_category.Text = dt.Rows[0]["item_category"].ToString();
                txt_name.Text = dt.Rows[0]["item_name"].ToString();
                txt_size.Text = dt.Rows[0]["item_size"].ToString();
                txt_itemDefect.Text = dt.Rows[0]["item_defect"].ToString();
                txt_itemRemarks.Text = dt.Rows[0]["item_remarks"].ToString();
                txt_repairRemarks.Text = dt.Rows[0]["repair_remarks"].ToString();
                txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();

                loadTxt_itemStatus(dt.Rows[0]["item_decision"].ToString());

            }
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Close Complaint
            if (Validation.validate(closeComplaint_Notify, chk_closeComplaint.IsChecked == true, "This must be checked")) { }
            else { check = false; }
            
            return check;
        }

        ~Deliver_To_Customer() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') ";
                    query += "UPDATE ComplaintItem SET returned_dt = GETDATE() WHERE comp_item_id = @COMPitemID ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 16 then 17 when comp_status_id = 21 then 22 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID , closed_dt = GETDATE() WHERE comp_id = '" + compID + "' ";

                    Database db = new Database();

                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        LoadMainMenu.LoadFor(this);
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
    }
}
