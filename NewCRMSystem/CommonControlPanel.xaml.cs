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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for CommonControlPanel.xaml
    /// </summary>
    public partial class CommonControlPanel : UserControl
    {
        Window parentWindow;
        public CommonControlPanel()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Loaded_Panel);
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            string query = "Update LoginDetails set logout_dt = GETDATE() WHERE logindetail_id ='"+ Login.LogindetailID +"' ";
            db.Save_Del_Update(query);

            Login lg = new Login();
            lg.Show();
            parentWindow.Hide();

            MessageBox.Show("Logged out","Information",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void Loaded_Panel(object sender, RoutedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
        }
    }
}
