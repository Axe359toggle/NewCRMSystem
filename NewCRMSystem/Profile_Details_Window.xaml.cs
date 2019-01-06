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
    /// Interaction logic for Profile_Details_Window.xaml
    /// </summary>
    public partial class Profile_Details_Window : Window
    {
        public Profile_Details_Window()
        {
            InitializeComponent();
        }

        public Profile_Details_Window(int empID1)
        {
            try
            {
                InitializeComponent();
                loadData(empID1);
                empID = empID1;
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

        int empID = 0;
        int loginID = 0;

        private void loadData(int empID1)
        {
            string query = "SELECT emp_title,emp_fname,emp_lname,emp_tp,des_id,login_id,location_id,assigned_dt from Manager WHERE emp_id = '" + empID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_title.Text = dt.Rows[0]["emp_title"].ToString();
            txtFname.Text = dt.Rows[0]["emp_fname"].ToString();
            txtLname.Text = dt.Rows[0]["emp_lname"].ToString();
            txtTp.Text = dt.Rows[0]["emp_tp"].ToString();
            string des = dt.Rows[0]["des_id"].ToString();

            if (des.Equals("S"))
            {
                txt_des.Text = "Showroom Manager";
            }
            else if (des.Equals("H"))
            {
                txt_des.Text = "Headquarters Manager";
            }
            else if (des.Equals("F"))
            {
                txt_des.Text = "Factory Manager";
            }

            loginID = Int32.Parse(dt.Rows[0]["login_id"].ToString());

            query = "SELECT login_dt,logout_dt FROM LoginDetails WHERE login_id='" + loginID + "' ";
            loginDatagrid.ItemsSource = db.GetData(query).AsDataView();

            txtlocationID.Text = dt.Rows[0]["location_id"].ToString();
            txtAssignedDt.Text = dt.Rows[0]["assigned_dt"].ToString();
        }

        ~Profile_Details_Window() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void BtnUpdateLogin_Click(object sender, RoutedEventArgs e)
        {
            if (empID > 0)
            {
                Login_Details w = new Login_Details(true, empID, loginID);

                if (w.ShowDialog() == true)
                {

                }
            }
        }

        private void BtnLocationView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                View_Location_Details w = new View_Location_Details(Int32.Parse(txtlocationID.Text));
                w.Show();
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
