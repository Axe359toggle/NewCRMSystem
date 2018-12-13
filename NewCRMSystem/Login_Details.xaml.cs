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
        int loginID = 0;
        int empID = 0;
        string desID = "";
        string uName = "";
        string newPass = "";

        private bool showdialogstatus;

        public Login_Details()
        {
            InitializeComponent();
        }

        public Login_Details(bool dialogstatus, int empID1)
        {
            InitializeComponent();
            txt_empID.Text = empID1.ToString();
            rbn_insert.IsChecked = true;
            showdialogstatus = true;
        }

        public Login_Details(bool dialogstatus, int empID1 , int loginID1)
        {
            InitializeComponent();

            txt_empID.Text = empID1.ToString();
            txt_loginID.Text = loginID1.ToString();

            rbn_update.IsChecked = true;
            showdialogstatus = true;
        }

        private void setInsert()
        {
            txt_loginID.IsEnabled = false;
            txt_currPass.IsEnabled = false;
            cmb_accStatus.IsEnabled = false;
        }

        private void setUpdate()
        {
            txt_loginID.IsEnabled = true;
            txt_currPass.IsEnabled = true;
            cmb_accStatus.IsEnabled = true;
            rbn_insert.IsEnabled = false;
        }

        private bool authenticate()
        {
            bool check = false;

            string uname = Login.UName;
            string upass = Password.sha256(txt_currPass.Password);

            Database db = new Database();
            string query = "SELECT emp_username,emp_pass from Login where emp_username = '" + uname + "' and emp_pass='" + upass + "' ";
            System.Data.DataTable dt = db.GetData(query);


            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["emp_username"].Equals(uname) && dt.Rows[0]["emp_pass"].Equals(upass))
                {
                    check = true;
                }
                else
                {
                    MessageBox.Show("Login Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Login Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            return check;
        }

        private void insert_rbn_Checked(object sender, RoutedEventArgs e)
        {
            if (rbn_insert.IsChecked == true)
            {
                setInsert();
            }
        }

        private void update_rbn_Checked(object sender, RoutedEventArgs e)
        {
            if (rbn_update.IsChecked == true)
            {
                setUpdate();
            }
        }

        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbn_insert.IsChecked == true)
                {
                    if (validate(false))
                    {
                        empID = Int32.Parse(txt_empID.Text);
                        uName = txt_uName.Text;
                        desID = txt_desID.Text.Trim();
                        newPass = Password.sha256(txt_newPass.Password);

                        string query = "insert into login (emp_username,emp_pass,des_id) values ('" + uName + "','" + newPass + "','" + desID + "')  DECLARE @ID int = SCOPE_IDENTITY() ";

                        string query2 = "update Manager set login_id = @ID where emp_id='" + empID + "' SELECT @ID AS login_id";
                        query = query + query2;

                        Database db = new Database();

                        System.Data.DataTable dt = db.GetData(query);
                        txt_loginID.Text = dt.Rows[0]["login_id"].ToString();

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (showdialogstatus == true)
                            {
                                Login.b1.removePreviousWindow();
                                DialogResult = true;
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else if (rbn_update.IsChecked == true)
                {
                    if (validate(true)) //validates with Location ID
                    {
                        loginID = Int32.Parse(txt_loginID.Text);
                        desID = txt_desID.Text;
                        uName = txt_uName.Text;

                        if (authenticate())
                        {
                            string query = " UPDATE Login SET emp_username = '" + uName + "' , emp_pass  = '" + txt_newPass.Password + "' WHERE login_id  = " + loginID + " ";
                            Database db = new Database();

                            if (db.Save_Del_Update(query) > 0)
                            {
                                MessageBox.Show("Data Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                if (showdialogstatus == true)
                                {
                                    Login.b1.removePreviousWindow();
                                    DialogResult = true;
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Data updation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Current Password Wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
        

        private bool validate(bool value)
        {
            //value = true to include loginID and currentPassword validation
            //value = false to exclude loginID and currentPassword validation
            bool check = true;

            if (value == true)
            {
                //Location ID
                if (CRMdbData.Login.login_id.validate(txt_loginID.Text))
                {
                    loginID_Notify.Source = loginID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
                else
                {
                    loginID_Notify.Source = loginID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                    check = false;
                }

                //Current Password
                if (CRMdbData.Login.emp_pass.validate(txt_currPass.Password))
                {
                    currPass_Notify.Source = currPass_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
                else
                {
                    currPass_Notify.Source = currPass_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                    check = false;
                }
            }

            //Manager ID
            if (CRMdbData.Manager.emp_id.validate(txt_empID.Text))
            {
                empID_Notify.Source = empID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                empID_Notify.Source = empID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Designation ID
            if (CRMdbData.Designation.desName.validate(txt_desID.Text))
            {
                desID_Notify.Source = desID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                desID_Notify.Source = desID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Username
            if (CRMdbData.Login.emp_username.validate(txt_uName.Text))
            {
                uName_Notify.Source = uName_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                uName_Notify.Source = uName_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }
            

            //New Password
            if (CRMdbData.Login.emp_pass.validate(txt_newPass.Password))
            {
                newPass_Notify.Source = newPass_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                newPass_Notify.Source = newPass_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Re-Enter Password
            if (txt_newPass.Password.Equals(txt_rePass.Password))
            {
                rePass_Notify.Source = rePass_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                rePass_Notify.Source = rePass_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
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
