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

        private bool validate()
        {
            bool check = true;

            if (txt_compID.Text.Length != CRMdbData.Complaint.comp_id.size)
            {
                check = false;
                compIDNotify.Source = compIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compIDNotify.Source = compIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(txt_refID.Text.Trim().Length >= CRMdbData.Reference.ref_id.minsize || txt_refID.Text.Trim().Length == 0))
            {
                check = false;
                refIDNotify.Source = refIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                refIDNotify.Source = refIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (txt_cusID.Text.Trim().Length != CRMdbData.Customer.cus_id.size)
            {
                check = false;
                cusIDNotify.Source = cusIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                cusIDNotify.Source = cusIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(rbn_byCall.IsChecked == true || rbn_inPerson.IsChecked == true))
            {
                check = false;
                compMethodNotify.Source = compMethodNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compMethodNotify.Source = compMethodNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(rbn_staffComp.IsChecked == true || rbn_itemComp.IsChecked == true))
            {
                check = false;
                compTypeNotify.Source = compTypeNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                compTypeNotify.Source = compTypeNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (!(txt_relShrmID.Text.Trim().Length == CRMdbData.Location.location_id.size || txt_relShrmID.Text.Trim().Length == 0))
            {
                check = false;
                relShrmIDNotify.Source = relShrmIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                relShrmIDNotify.Source = relShrmIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }


            return check;
        }
    }
}
