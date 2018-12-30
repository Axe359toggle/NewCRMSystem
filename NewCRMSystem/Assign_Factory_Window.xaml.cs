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
    /// Interaction logic for Assign_Factory_Window.xaml
    /// </summary>
    public partial class Assign_Factory_Window : Window
    {
        public Assign_Factory_Window()
        {
            InitializeComponent();
            bindCompIDList();
        }

        public Assign_Factory_Window(int compID1)
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                foreach (ComboBoxItem item in cmb_compID.Items)
                {
                    if (item.Content.ToString() == compID1.ToString())
                    {
                        cmb_compID.SelectedValue = item;
                        break;
                    }
                }
                loadData(compID1.ToString());
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

        DatabaseBased_Objects.Location loc;

        private void bindCompIDList()
        {
            string query = "SELECT comp_id FROM Complaint WHERE comp_status_id = 7 OR comp_status_id = 29";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadData(string compID1)
        {
            string query = "SELECT CI.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size from ComplaintItem as CI , ItemType as IT where CI.comp_id = '" + compID1 + "' and CI.item_type_id = IT.item_type_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Factory ID
            if (Validation.validate(locationID_Notify, CRMdbData.Location.location_id.validate(txt_locationID.Text), CRMdbData.Location.location_id.Error)) { }
            else { check = false; }
            
            return check;
        }

        ~Assign_Factory_Window() { }
        private void btnFactoryLocationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location( true , "Factory" );
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_locationID.Text = w.txt_LocationID.Text;
                    txt_locationName.Text = w.txt_LocationName.Text;

                    loc = new DatabaseBased_Objects.Location();
                    loc.locID = Int32.Parse(txt_locationID.Text);
                    loc.locName = txt_locationName.Text;
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    string query = "DECLARE @COMPstatusID int SET @COMPstatusID = (SELECT CASE WHEN C.comp_status_id = 7 THEN 8 WHEN C.comp_status_id = 29 THEN 30 END FROM Complaint C WHERE C.comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";

                    Database db = new Database();
                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        Login.b1.hideWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID, loc));
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

        private void Cmb_compID_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_compID.Text.Length > 0)
                {
                    loadData(cmb_compID.Text);
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
