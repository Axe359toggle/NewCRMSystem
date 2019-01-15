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
        int compID = 0;
        int deliveyID = 0;

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
                txt_compID.IsReadOnly = true;
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

        public Deliver_Item_Window(int compID , DatabaseBased_Objects.Location destinationLocation)
        {
            try
            {
                InitializeComponent();
                txt_compID.Text = compID.ToString();
                txt_compID.IsReadOnly = true;

                if (destinationLocation.locID > 0)
                {
                    txt_destinationID.Text = destinationLocation.locID.ToString();
                    btnDestinationLocationSearch.IsEnabled = false;
                }
                if (destinationLocation.locName.Length > 0)
                {
                    txt_destinationName.Text = destinationLocation.locName;
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

        public Deliver_Item_Window(int compID, DatabaseBased_Objects.Location destinationLocation, DatabaseBased_Objects.Location sourceLocation)
        {
            try
            {
                InitializeComponent();
                txt_compID.Text = compID.ToString();
                txt_compID.IsReadOnly = true;

                if (destinationLocation.locID > 0)
                {
                    txt_destinationID.Text = destinationLocation.locID.ToString();
                }
                if (destinationLocation.locName.Length > 0)
                {
                    txt_destinationName.Text = destinationLocation.locName;
                }

                if (sourceLocation.locID > 0)
                {
                    txt_sourceID.Text = destinationLocation.locID.ToString();
                    btnSourceLocationSearch.IsEnabled = false;
                }
                if (sourceLocation.locName.Length > 0)
                {
                    txt_sourceName.Text = sourceLocation.locName;
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
                if (w.ShowDialog() == true)
                {
                    txt_sourceID.Text = w.txt_LocationID.Text;
                    txt_sourceName.Text = w.txt_LocationName.Text;
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
                if (w.ShowDialog() == true)
                {
                    txt_destinationID.Text = w.txt_LocationID.Text;
                    txt_destinationName.Text = w.txt_LocationName.Text;
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
                int sourceID = Int32.Parse(txt_sourceID.Text);
                int destinationID = Int32.Parse(txt_destinationID.Text);
                DateTime sourceDt= dt_sourceSentDate.DisplayDate;
                if (validate())
                {
                    compID = Int32.Parse(txt_compID.Text);
                    string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Delivery ( comp_item_id , source_id , destination_id , source_dt) VALUES ( @COMPitemID , '" + sourceID + "' , '" + destinationID + "' , '" + sourceDt + "' )  DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as delivery_id ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 5 then 6 when comp_status_id = 27 then 28 when comp_status_id = 8 then 9 when comp_status_id = 30 then 31 when comp_status_id = 12 then 13 when comp_status_id = 34 then 35 when comp_status_id = 14 then 15 when comp_status_id = 36 then 37 when comp_status_id = 19 then 20 when comp_status_id = 40 then 41 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";
                    Database db = new Database();
                    System.Data.DataTable dt = db.GetData(query);
                    deliveyID = Int32.Parse(dt.Rows[0]["delivery_id"].ToString());

                    if (deliveyID > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        MessageBox.Show("Delivery ID is " + deliveyID + " .", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMainMenu.LoadFor(this);
                    }
                    else
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Failed();
                    }
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
