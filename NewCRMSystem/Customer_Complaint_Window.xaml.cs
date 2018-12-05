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

using System.Data.SqlClient;

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

        private int compID;
        private int refID;
        private int cusID;
        private string compMethod;
        private string compType1 = "Customer";
        private string compType2;
        private int relShrmID;

        string query;
        

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    compID = Int32.Parse(txt_compID.Text);
                    cusID = Int32.Parse(txt_cusID.Text.Trim());

                    if (rbn_byCall.IsChecked == true) { compMethod = "By Call"; }
                    else if (rbn_inPerson.IsChecked == true) { compMethod = "In Person"; }

                    if (rbn_staffComp.IsChecked == true) { compType2 = "Staff"; }
                    else if (rbn_itemComp.IsChecked == true) { compType2 = "Item"; }


                    Database db = new Database();

                    if (txt_refID.Text.Trim().Length == 0)
                    {
                        try
                        {
                            
                            string query1 = "INSERT INTO Reference DEFAULT VALUES DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as ref_id";
                            txt_refID.Text = db.ReadData(query1, "ref_id");
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        refID = Int32.Parse(txt_refID.Text.Trim());
                    }


                    if(txt_relShrmID.Text.Trim().Length==0)
                    {
                        query = "INSERT INTO Complaint (comp_id,comp_type,ref_id) values('" + compID + "','" + compType1 + "','" + refID + "') INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type) values('" + compID + "','" + cusID + "','"+compMethod+"','" + compType2 + "')";
                    }
                    else
                    {
                        relShrmID = Int32.Parse(txt_relShrmID.Text.Trim());
                        query = "INSERT INTO Complaint (comp_id,comp_type,ref_id) values('" + compID + "','" + compType1 + "','" + refID + "') INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type,related_showroom) values('" + compID + "','" + cusID + "','"+compMethod+"','" + compType2 + "','"+ relShrmID +"')";
                    }

                    int rows = db.Save_Del_Update(query);

                    if ( rows > 0)
                    {
                        MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (rbn_staffComp.IsChecked == true)
                        {
                            Login.b1.addWindowAndOpenNextWindow(this, new Staff_Complaint(compID));
                        }
                        else if (rbn_itemComp.IsChecked == true)
                        {
                            Login.b1.addWindowAndOpenNextWindow(this, new ReceivedItem_Details(compID));
                        }
                        //open next window
                    }
                    
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            


            
        }

        private bool validate()
        {
            bool check = true;

            if(CRMdbData.Complaint.comp_id.validate(txt_compID.Text))
            {
                check = false;
                compIDNotify.Source = compIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compIDNotify.Source = compIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (CRMdbData.Reference.ref_id.validate(txt_refID.Text))
            {
                check = false;
                refIDNotify.Source = refIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                refIDNotify.Source = refIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (CRMdbData.Customer.cus_id.validate(txt_cusID.Text))
            {
                check = false;
                cusIDNotify.Source = cusIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                cusIDNotify.Source = cusIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(rbn_byCall.IsChecked==true||rbn_inPerson.IsChecked==true))
            {
                check = false;
                compMethodNotify.Source = compMethodNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compMethodNotify.Source = compMethodNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(rbn_staffComp.IsChecked == true || rbn_itemComp.IsChecked == true))
            {
                check = false;
                compTypeNotify.Source = compTypeNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compTypeNotify.Source = compTypeNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (CRMdbData.Location.location_id.validate(txt_relShrmID.Text)
            {
                check = false;
                relShrmIDNotify.Source = relShrmIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                relShrmIDNotify.Source = relShrmIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
                

            return check;
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
