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
    /// Interaction logic for ReceivedItem_Details.xaml
    /// </summary>
    public partial class ReceivedItem_Details : Window
    {
        public ReceivedItem_Details()
        {
            InitializeComponent();
           
        }

        public ReceivedItem_Details(string compID)
        {
            InitializeComponent();

        }
        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
