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

using System.Data.SqlClient;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        internal static BackButton b1;
        internal static string empID;
        internal static string logindetailID;
        internal static string desID;
        internal static string locID;

        public Login()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string uname = uname_txt.Text;
                string upass = Password.sha256(upass_txt.Password);

                Database db = new Database();
                string query = "SELECT des_id,emp_id,login_id,emp_username,emp_pass from Login where emp_username = '" + uname + "' and emp_pass='" + upass + "' ";
                System.Data.DataTable dt = db.GetData(query);


                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["emp_username"].Equals(uname) && dt.Rows[0]["emp_pass"].Equals(upass))
                    {
                        string query2 = "insert into LoginDetails (login_id,login_dt) values ('"+ dt.Rows[0]["login_id"] + "',DEFAULT)  declare @ID int = SCOPE_IDENTITY() Select @ID as logindetail_id";
                        System.Data.DataTable dt1 = db.GetData(query2);
                        logindetailID = dt1.Rows[0]["logindetail_id"].ToString();

                        empID = dt.Rows[0]["emp_id"].ToString();
                        desID = dt.Rows[0]["des_id"].ToString();
                        locID = dt.Rows[0]["emp_id"].ToString();
                        if (desID.Equals("H"))
                        {
                            b1 = new BackButton();
                            b1.addWindowAndOpenNextWindow(this, new HQ_Manager_Dashboard());
                        }
                        else if (desID.Equals("S"))
                        {
                            b1 = new BackButton();
                            b1.addWindowAndOpenNextWindow(this, new Showroom_Manager_Mainmenu());
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
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }
    }
}
