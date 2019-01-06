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
    /// Interaction logic for View_Customer_Details.xaml
    /// </summary>
    public partial class View_Customer_Details : Window
    {
        public View_Customer_Details()
        {
            InitializeComponent();
        }

        public View_Customer_Details(int cusID1)
        {
            try
            {
                InitializeComponent();
                loadData(cusID1);
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

        private void loadData(int cusID1)
        {
            string query = "SELECT cus_name , cus_tp , cus_email from Customer WHERE cus_id = '" + cusID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_cusID.Text = cusID1.ToString();
            txt_cusName.Text = dt.Rows[0]["cus_name"].ToString();
            txt_cusTp.Text = dt.Rows[0]["cus_tp"].ToString();
            txt_cusEmail.Text = dt.Rows[0]["cus_email"].ToString();
        }

        ~View_Customer_Details() { }
    }
}
