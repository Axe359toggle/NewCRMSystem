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
    /// Interaction logic for Showroom_Manager_Dashboard.xaml
    /// </summary>
    public partial class Showroom_Manager_Mainmenu : Window
    {
        public Showroom_Manager_Mainmenu()
        {
            InitializeComponent();
        }

        ~Showroom_Manager_Mainmenu() { }

        private void btn_cusComplaint_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Customer_Complaint_Window());
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_assignRebate_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Rebate_Payment());
        }

        private void btn_deliverToCustomer_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Deliver_To_Customer());
        }

        private void btn_closeBatchItemComplaint_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Close_Batch_Item_Complaint_Window());
        }

        private void Btn_batchComplaint_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Batch_Item_Complaint_Window());
        }

        private void Btn_recordReceivedCustomerItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new ReceivedItem_Details());
        }

        private void btn_recordReceivedItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Record_Delivered_Item_Window(Login.LocID));
        }
    }
}
