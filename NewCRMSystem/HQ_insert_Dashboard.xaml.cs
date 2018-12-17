using System.Windows;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for HQ_insert_Dashboard.xaml
    /// </summary>
    public partial class HQ_insert_Dashboard : Window
    {
        public HQ_insert_Dashboard()
        {
            InitializeComponent();
        }

        private void btn_LoginDetails_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Login_Details());
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btn_AssignRebate_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.hideWindowAndOpenNextWindow(this, new Assign_Reabate_Window());
        }
    }
}

