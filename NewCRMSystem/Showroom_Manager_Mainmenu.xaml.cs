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

        private void btn_cusComplaint_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.addWindowAndOpenNextWindow(this, new Customer_Complaint_Window());
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
