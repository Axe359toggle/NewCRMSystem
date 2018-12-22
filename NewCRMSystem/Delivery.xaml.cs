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
    /// Interaction logic for Delivery.xaml
    /// </summary>
    public partial class Delivery : Window
    {
        int deliveryID;
        int compItemID;
        int sourceID;
        int destinationID;


        public Delivery()
        {
            InitializeComponent();
        }

        public Delivery(int compID1)
        {
            InitializeComponent();
            txt_compID.Text = compID1.ToString();
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_process_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbnInsert.IsChecked == true)
                {
                    compItemID = Int32.Parse(txt_compID.Text.Trim());
                    sourceID = Int32.Parse(txt_sourceID.Text.Trim());
                    destinationID = Int32.Parse(txt_destinationID.Text.Trim());

                    string query = "INSERT INTO Delivery (comp_item_id ,source_id ,destination_id ,source_dt ) values (" + compItemID + ", " + sourceID + ", " + destinationID + ", DEFAULT)";
                    Database db = new Database();

                    if (db.Save_Del_Update(query) > 0)
                    {
                        MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
    }
}
