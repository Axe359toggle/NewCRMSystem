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
    /// Interaction logic for Deliver_Item_Window.xaml
    /// </summary>
    public partial class Deliver_Item_Window : Window
    {
        public Deliver_Item_Window()
        {
            InitializeComponent();
        }

        public Deliver_Item_Window(int compID)
        {
            try
            {
                InitializeComponent();
                txt_compID.Text = compID.ToString();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private bool validate1(Image image , bool condition ,string ErrorMessage)
        {
            bool check = true;

            if (condition)
            {
                image.Source = image.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                image.Source = image.TryFindResource("notifyErrorImage") as BitmapImage;
                image.ToolTip = ErrorMessage;
                check = false;
            }

            return check;
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(txt_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Source ID
            if (Validation.validate(sourceID_Notify, CRMdbData.Location.location_id.validate(txt_sourceID.Text), CRMdbData.Location.location_id.Error)) { }
            else { check = false; }

            //Source Sent Date
            if (Validation.validate(sourceSentDate_Notify, CRMdbData.ComplaintItem.received_dt.validate(dt_sourceSentDate.Text), CRMdbData.ComplaintItem.received_dt.Error)) { }
            else { check = false; }

            //Destination ID
            if (Validation.validate(destinationID_Notify, CRMdbData.Location.location_id.validate(txt_destinationID.Text), CRMdbData.Location.location_id.Error)) { }
            else { check = false; }

            return check;
        }

        ~Deliver_Item_Window() { }

        private void btnSourceLocationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_sourceID.Text = w.txt_LocationID.Text;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void btnDestinationLocationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_destinationID.Text = w.txt_LocationID.Text;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
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

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {

                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
