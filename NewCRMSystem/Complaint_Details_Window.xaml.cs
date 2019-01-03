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
    /// Interaction logic for Complaint_Details_Window.xaml
    /// </summary>
    public partial class Complaint_Details_Window : Window
    {
        public Complaint_Details_Window()
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
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

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void loadData(string compID1)
        {
            string query = "SELECT CI.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size from ComplaintItem as CI , ItemType as IT where CI.comp_id = '" + compID1 + "' and CI.item_type_id = IT.item_type_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();
        }

        ~Complaint_Details_Window() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
