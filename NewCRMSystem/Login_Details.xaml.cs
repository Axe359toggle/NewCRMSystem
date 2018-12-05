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
    /// Interaction logic for Log_Details.xaml
    /// </summary>
    public partial class Login_Details : Window
    {
        string loginID;
        string empID;
        string desID;
        string uName;
        string newPass;


        public Login_Details()
        {
            InitializeComponent();
        }

        public Login_Details(string empID1)
        {
            InitializeComponent();
            empID_txt.Text = empID1;
            insert_rbn.IsChecked = true;
        }

        public Login_Details(string empID1 , string loginID1)
        {
            InitializeComponent();

            empID_txt.Text = empID1;
            loginID_txt.Text = loginID1;

            update_rbn.IsChecked = true;
            insert_rbn.IsEnabled = false;
        }

        private void setInsert(bool value)
        {
            currPass_txt.IsEnabled = !value;
            accStatus_cmb.IsEnabled = !value;
        }

        private void insert_rbn_Checked(object sender, RoutedEventArgs e)
        {
            if (insert_rbn.IsChecked == true)
            {
                setLoginID();
                setInsert(true);
            }
        }

        private void update_rbn_Checked(object sender, RoutedEventArgs e)
        {
            if (update_rbn.IsChecked == true)
            { setInsert(false); }
        }

        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                

                if(insert_rbn.IsChecked == true || update_rbn.IsChecked == true)
                {
                    if (insert_rbn.IsChecked == true)
                    {
                        if (validate(true))
                        {
                            loginID = loginID_txt.Text;
                            uName = uName_txt.Text;
                            desID = desID_txt.Text.Trim();
                            newPass = Password.sha256(newPass_txt.Password);


                            if (!(empID_txt.Text.Length == 0))
                            { empID = empID_txt.Text; }


                            string query = "insert into login (login_id,emp_username,emp_pass,des_id) values ('" + loginID + "','" + uName + "','" + newPass + "','" + desID + "')   ";

                            string query2 = "update Manager set login_id ='" + loginID + "' where emp_id='" + empID + "'";
                            query = query + query2;

                            Database db = new Database();
                            int rows = db.Save_Del_Update(query);

                            if (rows > 0)
                            {
                                MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                
                                //open next window
                            }
                            else
                            {
                                MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                        }
                    }
                    else if (update_rbn.IsChecked == true)
                    {
                        if (validate(false))
                        {
                            string currPass;

                        }
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

        private void setLoginID()
        {
            try
            {
                Database db = new Database();
                string query = "select case when MAX(login_id) is null then '100000' else MAX(login_id) END as login_id from Login";
                loginID = db.ReadData(query, "login_id");
                loginID_txt.Text = (Int32.Parse(loginID) + 1).ToString();
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

        private bool validate(bool type)
        {
            //type = true for insert
            //type = false for update
            bool check = true;
            if(type==true)
            {
                if (!(loginID_txt.Text.Trim().Length >= CRMdbData.Login.login_id.size || loginID_txt.Text.Trim().Length == 0))
                {
                    check = false;
                    loginIDNotify.Source = loginIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    loginIDNotify.Source = loginIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (!(empID_txt.Text.Trim().Length >= CRMdbData.Manager.emp_id.size || empID_txt.Text.Trim().Length == 0))
                {
                    check = false;
                    empIDNotify.Source = empIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    empIDNotify.Source = empIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (uName_txt.Text.Trim().Length > CRMdbData.Login.emp_username.size|| uName_txt.Text.Trim().Length == 0)
                {
                    check = false;
                    uNameNotify.Source = uNameNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    uNameNotify.Source = uNameNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (desID_txt.Text.Trim().Length != CRMdbData.Designation.des_id.size)
                {
                    check = false;
                    desIDNotify.Source = desIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    desIDNotify.Source = desIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }


                if (newPass_txt.Password.Length > CRMdbData.Login.emp_pass.size || newPass_txt.Password.Trim().Length == 0)
                {
                    check = false;
                    newPassNotify.Source = newPassNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    newPassNotify.Source = newPassNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (!rePass_txt.Password.Equals(newPass_txt.Password))
                {
                    check = false;
                    rePassNotify.Source = rePassNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    rePassNotify.Source = rePassNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

            }
            else
            {
                if (!(loginID_txt.Text.Trim().Length >= CRMdbData.Login.login_id.size))
                {
                    check = false;
                    loginIDNotify.Source = loginIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    loginIDNotify.Source = loginIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (!(empID_txt.Text.Trim().Length >= CRMdbData.Manager.emp_id.size || empID_txt.Text.Trim().Length == 0))
                {
                    check = false;
                    empIDNotify.Source = empIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    empIDNotify.Source = empIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (uName_txt.Text.Trim().Length > CRMdbData.Login.emp_username.size || uName_txt.Text.Trim().Length == 0)
                {
                    check = false;
                    uNameNotify.Source = uNameNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    uNameNotify.Source = uNameNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (desID_txt.Text.Trim().Length != CRMdbData.Designation.des_id.size)
                {
                    check = false;
                    desIDNotify.Source = desIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    desIDNotify.Source = desIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (currPass_txt.Password.Length > CRMdbData.Login.emp_pass.size || currPass_txt.Password.Trim().Length == 0)
                {
                    check = false;
                    currPassNotify.Source = currPassNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    currPassNotify.Source = currPassNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (newPass_txt.Password.Length > CRMdbData.Login.emp_pass.size || newPass_txt.Password.Trim().Length == 0)
                {
                    check = false;
                    newPassNotify.Source = newPassNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    newPassNotify.Source = newPassNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }

                if (!rePass_txt.Password.Equals(newPass_txt.Password))
                {
                    check = false;
                    rePassNotify.Source = rePassNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                }
                else
                {
                    rePassNotify.Source = rePassNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
            }
            

            return check;
        }

        private void clearText()
        {

        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
