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
    /// Interaction logic for HQ_Manager_Dashboard.xaml
    /// </summary>
    public partial class HQ_Manager_Dashboard : Window
    {
        public HQ_Manager_Dashboard()
        {
            InitializeComponent();
        }

        ~HQ_Manager_Dashboard() { }

        
        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_managerDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Manager_Details_window());
        }

        private void btn_assignRebate_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Assign_Rebate_Window());
        }

        private void btn_recordReceivedItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Record_Delivered_Item_Window(Login.LocID));
        }

        private void btn_assignFactory_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Assign_Factory_Window());
        }

        private void btn_locationDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Location());
        }

        private void btn_itemTypeDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Item_Type_Window());
        }

        private void btn_assignNewItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Assign_New_Item_Window());
        }

        private void Btn_deliverItem_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_Item_Window());
        }

        private void btn_closeStaffComplaints_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Staff_Complaint_Window());
        }

        private void Btn_profileDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Profile_Details_Window(Login.EmpID));
        }

        private void Btn_complaintDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Complaint_Details_Window());
        }

        private void Btn_searchComplaints_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Search_Complaints());
        }

        private void btn_cusDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Customer_Details());
        }

        private void Btn_deliveryDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.closeWindowAndOpenNextWindow(this, new Delivery());
        }
    }
}
