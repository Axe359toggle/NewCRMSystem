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

using System.Data;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for Rebate_Payment.xaml
    /// </summary>
    public partial class Rebate_Payment : Window
    {
        private int compID = 0;
        private int empID = 0;
        private string cusChoice = "";
        private double itemPrice = 0;

        public Rebate_Payment()
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

        public Rebate_Payment(int compID1)
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

        private void bindCompIDList()
        {
            try
            {
                string query = "SELECT CI.comp_id from ComplaintItem as CI , Rebate as R , Complaint as C WHERE CI.comp_item_id = R.comp_item_id AND CI.comp_id = C.comp_id AND C.comp_status_id = 3 ";
                Database db = new Database();
                DataTable dt = db.GetData(query);

                cmb_compID.Items.Clear();

                foreach (DataRow dr in dt.Rows)
                    cmb_compID.Items.Add(dr["comp_id"].ToString());

                cmb_compID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
        
        private void loadData(string compID1)
        {
            string query = "SELECT I.item_price , RB.rebate_percentage , CC.cus_id , C.cus_name , C.cus_tp , C.cus_email from Rebate as RB , ComplaintItem as CI , CustomerComplaint as CC , Item as I , Customer as C WHERE CI.comp_id = '" + compID1 + "' and CI.comp_item_id = RB.comp_item_id and CI.comp_id = CC.comp_id and CI.item_id = I.item_id and CC.cus_id = C.cus_id";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_rebatePercentage.Text = dt.Rows[0]["rebate_percentage"].ToString();
            double rebatePercentage = 0;
            if (dt.Rows[0]["rebate_percentage"].ToString().Equals("25%"))
            {
                rebatePercentage = 0.25 ;
            }
            else if(dt.Rows[0]["rebate_percentage"].ToString().Equals("50%"))
            {
                rebatePercentage = 0.50;
            }
            else if (dt.Rows[0]["rebate_percentage"].ToString().Equals("75%"))
            {
                rebatePercentage = 0.75;
            }
            else if (dt.Rows[0]["rebate_percentage"].ToString().Equals("100%"))
            {
                rebatePercentage = 1.00;
            }

            itemPrice = Double.Parse(dt.Rows[0]["item_price"].ToString());
            txt_rebateAmount.Text = (itemPrice * rebatePercentage).ToString();

            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
            txt_cusName.Text = dt.Rows[0]["cus_name"].ToString();
            txt_cusTp.Text = dt.Rows[0]["cus_tp"].ToString();
            txt_cusEmail.Text = dt.Rows[0]["cus_email"].ToString();

        }

        private bool validate()
        {
            bool check = true;

            //Complaint Item ID
            if (CRMdbData.Complaint.comp_id.validate(cmb_compID.Text))
            {
                compID_Notify.Source = compID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                compID_Notify.Source = compID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                compID_Notify.ToolTip = CRMdbData.ComplaintItem.comp_item_id.Error;
                check = false;
            }

            //Customer Choice
            if (CRMdbData.Rebate.customer_choice.validate(cmb_cusChoice.Text))
            {
                cusChoice_Notify.Source = cusChoice_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                cusChoice_Notify.Source = cusChoice_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                cusChoice_Notify.ToolTip = CRMdbData.Item.item_price.Error;
                check = false;
            }

            //Showroom Manager ID
            if (CRMdbData.Manager.emp_id.validate(Login.EmpID.ToString()))
            { }
            else
            {
                MessageBox.Show("Logged Employee ID Error ", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                check = false;
            }


            return check;
        }

        ~Rebate_Payment() { }


        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void cmb_compItemID_DropDownClosed(object sender, EventArgs e)
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

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    string query2 = "SELECT location_id , location_name from Location WHERE location_type = 'HQ' ";
                    Database db1 = new Database();
                    System.Data.DataTable dt1 = db1.GetData(query2);

                    DatabaseBased_Objects.Location HQ = new DatabaseBased_Objects.Location();
                    if (dt1.Rows.Count == 1)
                    {
                        HQ.locID = Int32.Parse(dt1.Rows[0]["location_id"].ToString());
                        HQ.locName = dt1.Rows[0]["location_name"].ToString();
                    }

                    compID = Int32.Parse(cmb_compID.Text);
                    cusChoice = cmb_cusChoice.Text;

                    int compStatusID = 0;
                    if (cmb_cusChoice.Text.Equals("Accepted")) { compStatusID = 4; }
                    else if(cmb_cusChoice.Text.Equals("Rejected")) { compStatusID = 5; }

                    int ShowroomManagerID = Login.EmpID;

                    string query = "DECLARE @ID int SET @ID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') ";
                    query += "Update Rebate SET shrmManager = " + ShowroomManagerID + " , customer_choice = '" + cusChoice + "' WHERE comp_item_id = @ID ";
                    query += "UPDATE Complaint SET comp_status_id = " + compStatusID + " WHERE comp_id = " + compID + " ";

                    Database db = new Database();

                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        if (cusChoice.Equals("Rejected"))
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID , HQ , Login.Loc ));
                        }
                        else if (cusChoice.Equals("Accepted"))
                        {
                            LoadMainMenu.LoadFor(this);
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
