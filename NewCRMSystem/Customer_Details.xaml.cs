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
        Database db = new Database();

        public Customer_Details()
        {
            InitializeComponent();
            setcusID();
        }

        private String cusID;

        private void setcusID()
        {
            try
            {
                string query = "select case when MAX(cus_id ) is null then 'CUS0001' else MAX(cus_id ) END as cus_id  from Customer";
                cusID = db.ReadData(query, "cus_id");
                txt_cusId.Text = cusID;
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
        private void btn_process_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (rb_Insert.IsChecked == true)
                {
                    cusID = txt_cusId.Text;
                    if ((txt_cusId.Text.Length <= CRMdbData.Customer.cus_id.size) || !(String.IsNullOrEmpty(txt_cusId.Text)))
                    {
                        if ((txt_cusName.Text.Length <= CRMdbData.Customer.cus_name.size) || !(String.IsNullOrEmpty(txt_cusName.Text)))
                        {
                            if ((txt_cusMail.Text.Length <= CRMdbData.Customer.cus_email.size) || !(String.IsNullOrEmpty(txt_cusContactNo.Text)))
                            {
                                if ((txt_cusContactNo.Text.Length <= CRMdbData.Customer.cus_tp.size) || !(String.IsNullOrEmpty(txt_cusMail.Text)))
                                {
                                    int x = db.Save_Del_Update("INSERT INTO Customer(cus_id,cus_name ,cus_tp , cus_email ) VALUES('" + txt_cusId.Text + "','" + txt_cusName.Text + "','" + txt_cusMail.Text + "','" + txt_cusContactNo.Text + "')  ");
                                    if (x == 1) { MessageBox.Show("Data Inserted Succesfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information); }
                                    else { MessageBox.Show("Data not Inserted", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                                }
                                else { txt_cusMail.BorderBrush = Brushes.Red; }
                            }
                            else { txt_cusContactNo.BorderBrush = Brushes.Red; }
                        }
                        else { txt_cusName.BorderBrush = Brushes.Red; }
                    }
                    else { txt_cusId.BorderBrush = Brushes.Red; }
                }


                else if (rb_update.IsChecked == true)
                {
                    if (txt_cusId.Text.Length < CRMdbData.Customer.cus_id.size || txt_cusName.Text.Length < CRMdbData.Customer.cus_name.size || txt_cusMail.Text.Length < CRMdbData.Customer.cus_email.size || txt_cusContactNo.Text.Length < CRMdbData.Customer.cus_tp.size)
                    {
                        int x = db.Save_Del_Update("UPDATE Customer SET cus_name = '" + txt_cusName.Text + "' ,cus_tp = '" + txt_cusContactNo.Text + "' , cus_email = '" + txt_cusMail.Text + "'  WHERE cusId = '" + txt_cusId.Text + "')  ");
                        if (x == 1) { MessageBox.Show("Data Updated Succesfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information); }
                        else { MessageBox.Show("Data not Updated", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Data ", "Error", MessageBoxButton.OK);
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
               
}

     