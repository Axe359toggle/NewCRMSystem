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

        private void btn_insert_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new HQ_insert_Dashboard());
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_managerDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Manager_Details_window());
        }
    }
}
