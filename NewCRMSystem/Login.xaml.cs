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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        static BackButton B1 = new BackButton(30);
        internal static BackButton b1 { get { return B1; } }

        static int empID = 0; 
        internal static int EmpID { get { return empID; } }

        static string logindetailID = "";
        internal static string LogindetailID { get { return logindetailID; } }

        static string desID = "";
        internal static string DesID { get { return desID; } }

        static int locID = 0;
        internal static int LocID { get { return locID; } }

        static string uName = "";
        internal static string UName { get { return uName; } }

        public Login()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uName = uname_txt.Text;
                string upass = Password.sha256(upass_txt.Password);

                Database db = new Database();
                string query = "SELECT des_id,emp_id,login_id,emp_username,emp_pass from Login where emp_username = '" + uName + "' and emp_pass='" + upass + "' and account_status = 1 ";
                System.Data.DataTable dt = db.GetData(query);


                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["emp_username"].Equals(uName) && dt.Rows[0]["emp_pass"].Equals(upass))
                    {
                        string query2 = "insert into LoginDetails (login_id,login_dt) values ('"+ dt.Rows[0]["login_id"] + "',DEFAULT)  declare @ID int = SCOPE_IDENTITY() Select @ID as logindetail_id";
                        System.Data.DataTable dt1 = db.GetData(query2);
                        logindetailID = dt1.Rows[0]["logindetail_id"].ToString();

                        if (dt.Rows[0]["emp_id"].ToString().Length > 0)
                        {
                            empID = Int32.Parse(dt.Rows[0]["emp_id"].ToString());
                        }
                        
                        desID = dt.Rows[0]["des_id"].ToString();
                        //locID = dt.Rows[0]["emp_id"].ToString();
                        if (desID.Equals("H"))
                        {
                            B1.hideWindowAndOpenNextWindow(this, new HQ_Manager_Dashboard());
                        }
                        else if (desID.Equals("S"))
                        {
                            B1.hideWindowAndOpenNextWindow(this, new Showroom_Manager_Mainmenu());
                        }
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

        private void uname_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                upass_txt.Focus();
            }
        }

        private void upass_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login_btn_Click(this, new RoutedEventArgs());
            }
        }
    }
}
