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
    /// Interaction logic for CustomerComplaintWindow.xaml
    /// </summary>
    public partial class Customer_Complaint_Window : Window
    {
        public Customer_Complaint_Window()
        {
            InitializeComponent();
        }
        
        private int refID;
        private int cusID;
        private string compMethod;
        private string compType1 = "Customer";
        private string compType2;
        private int relShrmID;

        private bool validateRefID()
        {
            bool check = false;

            if (txt_refID.Text.Length > 0)
            {
                refID = Int32.Parse(txt_refID.Text);

                string query = " SELECT refID from Reference WHERE refID = " + refID + " ";
                Database db = new Database();
                System.Data.DataTable dt = db.GetData(query);

                if (dt.Rows.Count == 1)
                {
                    check = true;
                }
            }
            else
            {
                check = true;
            }
            
            return check;
        }

        private bool validate()
        {
            bool check = true;
            
            //Reference ID
            if (CRMdbData.Reference.ref_id.validate(txt_refID.Text) && validateRefID())
            {
                refID_Notify.Source = refID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                refID_Notify.Source = refID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Customer ID
            if (CRMdbData.Customer.cus_id.validate(txt_cusID.Text))
            {
                cusID_Notify.Source = cusID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                cusID_Notify.Source = cusID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Complaint Method
            if (rbn_byCall.IsChecked == true || rbn_inPerson.IsChecked == true)
            {
                compMethod_Notify.Source = compMethod_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                compMethod_Notify.Source = compMethod_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Complaint Type
            if (rbn_staffComp.IsChecked == true || rbn_itemComp.IsChecked == true)
            {
                compType_Notify.Source = compType_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                compType_Notify.Source = compType_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Related Showroom ID
            if (CRMdbData.Location.location_id.validate(txt_relShrmID.Text))
            {
                relShrmID_Notify.Source = relShrmID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                relShrmID_Notify.Source = relShrmID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }


            return check;
        }

        ~Customer_Complaint_Window() { }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    cusID = Int32.Parse(txt_cusID.Text);

                    if (rbn_byCall.IsChecked == true) { compMethod = "By Call"; }
                    else if (rbn_inPerson.IsChecked == true) { compMethod = "In Person"; }

                    if (rbn_staffComp.IsChecked == true) { compType2 = "Staff"; }
                    else if (rbn_itemComp.IsChecked == true) { compType2 = "Item"; }
                    
                    Database db = new Database();

                    if (txt_refID.Text.Trim().Length == 0)
                    {
                        string query1 = "INSERT INTO Reference DEFAULT VALUES DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as ref_id";
                        txt_refID.Text = db.GetData(query1).Rows[0]["ref_id"].ToString();
                    }

                    refID = Int32.Parse(txt_refID.Text);

                    relShrmID = Int32.Parse(txt_relShrmID.Text);
                    string query = "INSERT INTO Complaint (comp_type,ref_id,relatedLocation_id ) VALUES ('" + compType1 + "','" + refID + "','" + relShrmID + "') DECLARE @ID int = SCOPE_IDENTITY() INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type) values(@ID,'" + cusID + "','" + compMethod + "','" + compType2 + "') SELECT @ID as comp_id";

                    int compID = 0;
                    compID = Int32.Parse(db.GetData(query).Rows[0]["comp_id"].ToString());

                    if (compID > 0)
                    {
                        MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (rbn_staffComp.IsChecked == true)
                        {
                            Login.b1.hideWindowAndOpenNextWindow(this, new Staff_Complaint(compID));
                        }
                        else if (rbn_itemComp.IsChecked == true)
                        {
                            Login.b1.hideWindowAndOpenNextWindow(this, new ReceivedItem_Details(compID));
                        }
                        //open next window
                    }
                    
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            


            
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_cusSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Customer_Details(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_cusID.Text = w.txt_cusID.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_shrmSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_relShrmID.Text = w.txt_LocationID.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
