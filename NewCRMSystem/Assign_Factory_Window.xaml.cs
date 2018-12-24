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
    /// Interaction logic for Assign_Factory_Window.xaml
    /// </summary>
    public partial class Assign_Factory_Window : Window
    {
        public Assign_Factory_Window()
        {
            InitializeComponent();
        }

        ~Assign_Factory_Window() { }
        private void btnFactoryLocationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location( true , "Factory" );
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_locationID.Text = w.txt_LocationID.Text;
                    txt_locationName.Text = w.txt_LocationName.Text;
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
