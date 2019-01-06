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
    /// Interaction logic for Assign_New_Item_Window.xaml
    /// </summary>
    public partial class Assign_New_Item_Window : Window
    {
        public Assign_New_Item_Window()
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

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , Investigation AS Inv , ComplaintItem AS CI WHERE  Inv.comp_item_id = CI.comp_item_id  AND C.comp_id = CI.comp_id AND ( C.comp_status_id = 18 OR C.comp_status_id = 39 ) ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadData(int compID)
        {
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , CI.item_id , Inv.factoryManager , M.emp_fname , M.emp_lname FROM ItemType AS IT , ComplaintItem AS CI , Investigation AS Inv , Manager AS M WHERE CI.comp_id  = '" + compID + "' AND CI.item_type_id = IT.item_type_id AND CI.comp_item_id = Inv.comp_item_id AND Inv.factoryManager = M.emp_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
                txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
                txt_category.Text = dt.Rows[0]["item_category"].ToString();
                txt_name.Text = dt.Rows[0]["item_name"].ToString();
                txt_size.Text = dt.Rows[0]["item_size"].ToString();
                txt_currItemID.Text = dt.Rows[0]["item_id"].ToString();
                txt_facManagerID.Text = dt.Rows[0]["factoryManager"].ToString();
                txt_facManagerName.Text = dt.Rows[0]["emp_fname"].ToString() + dt.Rows[0]["emp_lname"].ToString();

            }
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //New Item ID
            if (Validation.validate(newItemID_Notify, CRMdbData.Item.item_id.validate(txt_newItemID.Text), CRMdbData.Item.item_id.Error)) { }
            else { check = false; }
            
            return check;
        }

        ~Assign_New_Item_Window() { }

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

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    string newItemID = txt_newItemID.Text.Trim();
                    string itemTypeID = txt_itemTypeID.Text;

                    string query = "INSERT INTO Item (item_id , item_type_id ) VALUES ( '" + newItemID + "' , '" + itemTypeID + "' ) ";
                    query += "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') UPDATE Investigation SET hQManager = " + Login.EmpID + " , newItem_id = '" + newItemID + "' WHERE comp_item_id = @COMPitemID ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 18 then 19 when comp_status_id = 39 then 40 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";
                    query += "SELECT L.location_id , L.location_name FROM Location AS L , Complaint AS C WHERE C.comp_id = '" + compID + "' AND C.recordedLocation_id = L.location_id ";

                    Database db = new Database();

                    System.Data.DataTable dt = db.GetData(query);

                    if (dt.Rows.Count == 1)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        DatabaseBased_Objects.Location loc = new DatabaseBased_Objects.Location();
                        loc.locID = Int32.Parse(dt.Rows[0]["location_id"].ToString());
                        loc.locName = dt.Rows[0]["location_name"].ToString();
                        
                        Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID, loc ));
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
