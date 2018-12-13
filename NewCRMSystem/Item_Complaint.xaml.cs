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
    /// Interaction logic for Item_Complaint.xaml
    /// </summary>
    public partial class Item_Complaint : Window
    {
        string compID;
        public Item_Complaint()
        {
            InitializeComponent();
        }

        public Item_Complaint(string compID1)
        {
            InitializeComponent();
            txt_compId.Text = compID1;
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_insert_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Delivery());
        }
    }
}
