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
    /// Interaction logic for Customer_Complaint.xaml
    /// </summary>
    public partial class Customer_Complaint : Window
    {
        public Customer_Complaint()
        {
            InitializeComponent();
        }

        private void btn_item_Click(object sender, RoutedEventArgs e)
        {
            //Item_Complaint ic = new Item_Complaint();
            //ic.Show();
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
