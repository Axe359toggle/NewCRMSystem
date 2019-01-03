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
using System.Data.SqlClient;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for Customer_Details.xaml
    /// </summary>
    public partial class Customer_Details : Window
    {
        private int cusID = 0;
        private string cusName = "";
        private string cusEmail = "";
        private string cusTp = "";

        private bool showdialogstatus;
        private bool onlyAllowSearch = false;

        public Customer_Details()
        {
            InitializeComponent();
            rbnInsert.IsChecked = true;
        }

        public Customer_Details(bool dialogstatus)
        {
            InitializeComponent();
            showdialogstatus = dialogstatus;
            back_btn.Visibility = Visibility.Collapsed;
        }
        public Customer_Details(char option)
        {
            InitializeComponent();
            if (option.Equals("i"))
            {
                rbnInsert.IsChecked = true;
            }
            else if (option.Equals("s"))
            {
                rbnSearch.IsChecked = true;
            }
        }

        public Customer_Details(char option, string cusID1)
        {
            InitializeComponent();
            if (option.Equals("u"))
            {
                txt_cusID.Text = cusID1;
                rbnUpdate.IsChecked = true;
                rbnInsert.IsChecked = false;
                showdialogstatus = true;
                back_btn.Visibility = Visibility.Collapsed;
            }
            else if (option.Equals("s"))
            {
                txt_cusID.Text = cusID1;
                chk_cusID.IsChecked = true;
                chk_cusID.IsEnabled = false;
                rbnSearch.IsChecked = true;
                rbnInsert.IsEnabled = false;
                showdialogstatus = true;
                onlyAllowSearch = true;
                back_btn.Visibility = Visibility.Collapsed;
            }
        }

        private void clearText()
        {
            txt_cusID.Text = "";
            txt_cusName.Text = "";
            txt_cusEmail.Text = "";
            txt_cusTp.Text = "";
        }

        private void setErrorImagesNull()
        {
            cusID_Notify.Source = null;
            cusName_Notify.Source = null;
            cusEmail_Notify.Source = null;
            cusTp_Notify.Source = null;
        }

        private void hide_chk(Visibility visibility)
        {
            chk_cusID.Visibility = visibility;
            chk_cusName.Visibility = visibility;
            chk_cusEmail.Visibility = visibility;
            chk_cusTp.Visibility = visibility;
        }

        private void enable_chk(bool value)
        {
            chk_cusID.IsEnabled = value;
            chk_cusName.IsEnabled = value;
            chk_cusEmail.IsEnabled = value;
            chk_cusTp.IsEnabled = value;

            if (value)
            {
                hide_chk(Visibility.Visible);
            }
            else
            {
                hide_chk(Visibility.Hidden);
            }
        }

        //load Insert option
        private void setInsert()
        {
            btn_process.Content = "Insert";
            rbnUpdate.IsEnabled = false;
            txt_cusID.IsReadOnly = true;
            txt_cusID.IsEnabled = false;
            btn_ok.IsEnabled = false;
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Update option
        private void setUpdate()
        {

            btn_process.Content = "Update";
            rbnUpdate.IsEnabled = true;
            txt_cusID.IsReadOnly = true;
            txt_cusID.IsEnabled = true;
            btn_ok.IsEnabled = true;
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Search option
        private void setSearch()
        {
            btn_process.Content = "Search";
            rbnUpdate.IsEnabled = false;
            txt_cusID.IsReadOnly = false;
            txt_cusID.IsEnabled = true;
            btn_ok.IsEnabled = false;
            setErrorImagesNull();
            enable_chk(true);
        }

        private bool validate(bool value)
        {
            bool check = true;

            if (value == true)
            {
                //Customer ID
                if (CRMdbData.Customer.cus_id.validate(txt_cusID.Text))
                {
                    cusID_Notify.Source = cusID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
                else
                {
                    cusID_Notify.Source = cusID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                    cusID_Notify.ToolTip = CRMdbData.Customer.cus_id.Error;
                    check = false;
                }
            }

            //Customer Name
            if (CRMdbData.Customer.cus_name.validate(txt_cusName.Text))
            {
                cusName_Notify.Source = cusName_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                cusName_Notify.Source = cusName_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                cusName_Notify.ToolTip = CRMdbData.Customer.cus_name.Error;
                check = false;
            }

            //Customer Email
            if (CRMdbData.Customer.cus_email.validate(txt_cusEmail.Text))
            {
                cusEmail_Notify.Source = cusEmail_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                cusEmail_Notify.Source = cusEmail_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                cusEmail_Notify.ToolTip = CRMdbData.Customer.cus_email.Error;
                check = false;
            }

            //Customer Telephone
            if (CRMdbData.Customer.cus_tp.validate(txt_cusTp.Text))
            {
                cusTp_Notify.Source = cusTp_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                cusTp_Notify.Source = cusTp_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                cusTp_Notify.ToolTip = CRMdbData.Customer.cus_tp.Error;
                check = false;
            }

            return check;
        }

        ~Customer_Details() { }

        private void rbnInsert_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnInsert.IsChecked == true)
            {
                clearText();
                setInsert();
            }
        }

        private void rbnUpdate_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnUpdate.IsChecked == true)
            {
                setUpdate();
            }
        }

        private void rbnSearch_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnSearch.IsChecked == true)
            {
                setSearch();
            }
        }

        private void btn_process_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbnInsert.IsChecked == true)
                {
                    if (validate(false)) //validates without Customer ID
                    {
                        cusName = txt_cusName.Text;
                        cusEmail = txt_cusEmail.Text;
                        cusTp = txt_cusTp.Text;

                        Database db = new Database();
                        string query = "INSERT INTO Customer (cus_name ,cus_email ,cus_tp ) VALUES ('" + cusName + "','" + cusEmail + "','" + cusTp + "') DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as cus_id ";

                        cusID = Int32.Parse(db.GetData(query).Rows[0]["cus_id"].ToString());

                        if (cusID > 0)
                        {
                            txt_cusID.Text = cusID.ToString();
                            MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            rbnUpdate.IsChecked = true;
                        }
                        else
                        {
                            MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }

                    }
                }
                else if (rbnUpdate.IsChecked == true)
                {
                    if (validate(true))//Validates with Customer ID
                    {
                        cusID = Int32.Parse(txt_cusID.Text);
                        cusName = txt_cusName.Text;
                        cusEmail = txt_cusEmail.Text;
                        cusTp = txt_cusTp.Text;

                        string query = " UPDATE Customer SET cus_name = '" + cusName + "' , cus_email = '" + cusEmail + "' , cus_tp = '" + cusTp + "' WHERE cus_id = " + cusID + " ";
                        Database db = new Database();

                        if (db.Save_Del_Update(query) > 0)
                        {
                            MessageBox.Show("Data Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Data updation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else if (rbnSearch.IsChecked == true)
                {

                    string query = "SELECT cus_id ,cus_name ,cus_tp ,cus_email from Customer";
                    int x = 0;

                    void checkX()
                    {
                        if (x > 0)
                        {
                            query = query + " AND";
                        }
                    }

                    if (chk_cusID.IsChecked == true || chk_cusName.IsChecked == true || chk_cusEmail.IsChecked == true || chk_cusTp.IsChecked == true)
                    {
                        query = query + " WHERE";
                    }

                    //Customer ID
                    if (chk_cusID.IsChecked == true && txt_cusID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " cus_id LIKE '%" + txt_cusID.Text + "%'";
                        x++;
                    }

                    //Customer Name
                    if (chk_cusName.IsChecked == true && txt_cusName.Text.Length > 0)
                    {
                        checkX();
                        query = query + " cus_name LIKE '%" + txt_cusName.Text + "%'";
                        x++;
                    }

                    //Customer Email
                    if (chk_cusEmail.IsChecked == true && txt_cusEmail.Text.Length > 0)
                    {
                        checkX();
                        query = query + " cus_email LIKE '%" + txt_cusEmail.Text + "%'";
                        x++;
                    }

                    //Customer Telephone
                    if (chk_cusTp.IsChecked == true && txt_cusTp.Text.Length > 0)
                    {
                        checkX();
                        query = query + " cus_tp LIKE '%" + txt_cusTp.Text + "%'";
                        x++;
                    }
                    
                    Database db = new Database();
                    cus_Datagrid.ItemsSource = db.GetData(query).AsDataView();

                }
                else
                {
                    MessageBox.Show("Please select an option", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void cus_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)cus_Datagrid.SelectedItem;
                if (dv != null)
                {
                    cusID = Int32.Parse(dv.Row.ItemArray[0].ToString());//cus_id
                    txt_cusID.Text = cusID.ToString();
                    
                    btn_ok.IsEnabled = true;

                    txt_cusName.Text = dv.Row.ItemArray[1].ToString();//cus_name
                    txt_cusEmail.Text = dv.Row.ItemArray[2].ToString();//cus_email
                    txt_cusTp.Text = dv.Row.ItemArray[3].ToString();//cus_tp

                    if (!onlyAllowSearch)
                    {
                        rbnUpdate.IsChecked = true;
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

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (showdialogstatus == true)
                {
                    DialogResult = true;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }      
}

     