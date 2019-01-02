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
    /// Interaction logic for Close_Batch_Item_Complaint_Window.xaml
    /// </summary>
    public partial class Close_Batch_Item_Complaint_Window : Window
    {
        public Close_Batch_Item_Complaint_Window()
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

        public Close_Batch_Item_Complaint_Window(int compID1)
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                cmb_compID.SelectedItem = compID1.ToString();
                loadData(compID1);
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

        int compItemID = 0;

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , Delivery AS D , ComplaintItem AS CI WHERE D.destination_id = " + Login.LocID + " AND D.comp_item_id = CI.comp_item_id AND C.comp_id = CI.comp_id AND ( C.comp_status_id = 38 OR C.comp_status_id = 42 ) ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadData(int compID)
        {
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , CI.comp_item_id , CI.item_defect , CI.item_remarks , CI.item_decision FROM ItemType AS IT , ComplaintItem AS CI WHERE CI.comp_id  = '" + compID + "' AND CI.item_type_id = IT.item_type_id ";
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
                

                string itemDecision = dt.Rows[0]["item_decision"].ToString();
                if (itemDecision.Equals("Repair"))
                {
                    txt_itemStatus.Text = "Repaired";
                    string query1 = "SELECT R.repair_remarks FROM ItemType AS IT , ComplaintItem AS CI , Repair AS R WHERE CI.comp_id  = '" + compID + "' AND CI.item_type_id = IT.item_type_id AND CI.comp_item_id = R.comp_item_id";
                    System.Data.DataTable dt1 = db.GetData(query1);
                    itemRemarksVisibility(Visibility.Visible);
                    txt_repairRemarks.Text = dt.Rows[0]["repair_remarks"].ToString();
                }
                else if (itemDecision.Equals("Investigation"))
                {
                    itemRemarksVisibility(Visibility.Collapsed);
                    txt_itemStatus.Text = "New Item";
                }
            }
        }
        private void itemRemarksVisibility(Visibility visibility)
        {
            lbl_repairRemarks.Visibility = visibility;
            txt_repairRemarks.Visibility = visibility;
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

        ~Close_Batch_Item_Complaint_Window() { }

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
                    string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') ";
                    query += "UPDATE ComplaintItem SET returned_dt = GETDATE() WHERE comp_item_id = @COMPitemID ";
                    //query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 16 then 17 when comp_status_id = 21 then 22 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET closed_dt = GETDATE() WHERE comp_id = '" + compID + "' ";

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
    }
}
