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

        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT D.delivery_id , CI.comp_id , D.source_id ,(SELECT location_name FROM Location as L WHERE D.source_id = L.location_id) as source_name , D.destination_id , (SELECT location_name FROM Location as L WHERE D.destination_id = L.location_id) as destination_name , D.source_dt , D.destination_dt from Delivery as D , ComplaintItem as CI WHERE D.comp_item_id = CI.comp_item_id ";
                int x = 1;

                void checkX()
                {
                    if (x > 0)
                    {
                        query = query + " AND";
                    }
                }

                //Delivery ID
                if (chk_deliveryID.IsChecked == true && txt_deliveryID.Text.Length > 0)
                {
                    checkX();
                    query = query + " D.delivery_id LIKE '%" + txt_deliveryID.Text + "%'";
                    x++;
                }

                //Complaint ID
                if (chk_compID.IsChecked == true && txt_compID.Text.Length > 0)
                {
                    checkX();
                    query = query + " CI.comp_id LIKE '%" + txt_compID.Text + "%'";
                    x++;
                }

                //Source ID
                if (chk_sourceID.IsChecked == true && txt_sourceID.Text.Length > 0)
                {
                    checkX();
                    query = query + " D.source_id  LIKE '%" + txt_sourceID.Text + "%'";
                    x++;
                }

                //Destination ID
                if (chk_destinationID.IsChecked == true && txt_destinationID.Text.Length > 0)
                {
                    checkX();
                    query = query + " D.destination_id  LIKE '%" + txt_destinationID.Text + "%'";
                    x++;
                }

                //Source Date
                if (chk_sourceSentDate.IsChecked == true && dt_sourceSentDate.Text.Length > 0)
                {
                    checkX();
                    query = query + " D.source_dt  LIKE '%" + dt_sourceSentDate.Text + "%'";
                    x++;
                }

                //Destination Date
                if (chk_destinationReceivedDate.IsChecked == true && dt_destinationReceivedDate.Text.Length > 0)
                {
                    checkX();
                    query = query + " D.destination_dt  LIKE '%" + dt_destinationReceivedDate.Text + "%'";
                    x++;
                }

                Database db = new Database();
                delivery_Datagrid.ItemsSource = db.GetData(query).AsDataView();


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

        private void Delivery_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)delivery_Datagrid.SelectedItem;
                if (dv != null)
                {
                    int deliveryID = Int32.Parse(dv.Row.ItemArray[0].ToString());//delivery_id
                    txt_deliveryID.Text = deliveryID.ToString();
                    
                    txt_compID.Text = dv.Row.ItemArray[1].ToString();//comp_id
                    txt_sourceID.Text = dv.Row.ItemArray[2].ToString();//source_id
                    txt_sourceName.Text = dv.Row.ItemArray[3].ToString();//source_name
                    dt_sourceSentDate.SelectedDate = DateTime.Parse(dv.Row.ItemArray[4].ToString());//source_dt
                    txt_destinationID.Text = dv.Row.ItemArray[5].ToString();//destination_dt
                    txt_destinationName.Text = dv.Row.ItemArray[6].ToString();//destination_name
                    dt_destinationReceivedDate.SelectedDate = DateTime.Parse(dv.Row.ItemArray[7].ToString());//destination_dt
                }
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
