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
    /// Interaction logic for Rebate_Payment.xaml
    /// </summary>
    public partial class Rebate_Payment : Window
    {
        double itemPrice = 0;

        public Rebate_Payment()
        {
            InitializeComponent();
            bindCompItemIDList();
        }

        public Rebate_Payment(int compItemID1)
        {
            InitializeComponent();
            bindCompItemIDList();
            foreach (ComboBoxItem item in cmb_compItemID.Items)
            {
                if (item.Content.ToString() == compItemID1.ToString())
                {
                    cmb_compItemID.SelectedValue = item;
                    break;
                }
            }
            loadData(compItemID1.ToString());
        }

        private void bindCompItemIDList()
        {
            try
            {
                string query = "SELECT comp_item_id from Rebate";
                Database db = new Database();
                DataTable dt = db.GetData(query);

                cmb_compItemID.Items.Clear();

                foreach (DataRow dr in dt.Rows)
                    cmb_compItemID.Items.Add(dr["comp_item_id"].ToString());

                cmb_compItemID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void loadData(string compItemID1)
        {
            string query = "select I.item_price , RB.rebate_percentage , CC.cus_id from Rebate as RB , ComplaintItem as CI , CustomerComplaint as CC , Item as I WHERE RB.comp_item_id = '" + compItemID1 + "' and CI.comp_item_id = RB.comp_item_id and CI.comp_id = CC.comp_id and CI.item_id = I.item_id";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            double rebatePercentage = Double.Parse(dt.Rows[0]["rebate_percentage"].ToString());
            if(rebatePercentage == 0.25)
            {
                txt_rebatePercentage.Text = "25%";
            }
            else if (rebatePercentage == 0.50)
            {
                txt_rebatePercentage.Text = "50%";
            }
            else if (rebatePercentage == 0.75)
            {
                txt_rebatePercentage.Text = "75%";
            }
            else if (rebatePercentage == 1.00)
            {
                txt_rebatePercentage.Text = "100%";
            }
            
            itemPrice = Double.Parse(dt.Rows[0]["item_price"].ToString());
            txt_rebateAmount.Text = (itemPrice * rebatePercentage).ToString();
            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
            
        }

        ~Rebate_Payment() { }


        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void cmb_compItemID_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_compItemID.Text.Length > 0)
                {
                    loadData(cmb_compItemID.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
