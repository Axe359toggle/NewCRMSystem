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
    /// Interaction logic for Staff_Complaint.xaml
    /// </summary>
    public partial class Staff_Complaint : Window
    {
        int compID;

        public Staff_Complaint()
        {
            InitializeComponent();
        }

        public Staff_Complaint(int compID1)
        {
            InitializeComponent();
            compID = compID1;
            txt_compID.Text = compID.ToString();
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
