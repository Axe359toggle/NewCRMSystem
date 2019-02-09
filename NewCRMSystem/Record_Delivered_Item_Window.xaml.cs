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
    /// Interaction logic for Record_Delivered_Item_Window.xaml
    /// </summary>
    public partial class Record_Delivered_Item_Window : Window
    {
        public Record_Delivered_Item_Window()
        {
            try
            {
                InitializeComponent();
                bindDeliveryIDList();
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

        public Record_Delivered_Item_Window(int locID1)
        {
            try
            {
                InitializeComponent();
                bindDeliveryIDList(locID1);
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

        private void bindDeliveryIDList()
        {
            string query = "SELECT delivery_id FROM Delivery WHERE destination_dt IS NULL";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_deliveryID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_deliveryID.Items.Add(dr["delivery_id"].ToString());

            cmb_deliveryID.SelectedIndex = 0;
        }

        private void bindDeliveryIDList(int locID1)
        {
            string query = "SELECT delivery_id FROM Delivery WHERE destination_id = '" + locID1 + "' AND destination_dt IS NULL ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_deliveryID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_deliveryID.Items.Add(dr["delivery_id"].ToString());

            cmb_deliveryID.SelectedIndex = 0;
        }

        //Set destination date limits
        private void setDestinationDtLimit(int deliveryID1)
        {
            dt_destinationReceivedDate.DisplayDateEnd = DateTime.Today.Date;

            string query = "SELECT D.source_dt FROM Delivery AS D WHERE D.delivery_id = '" + deliveryID1 + "'  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);
            dt_destinationReceivedDate.DisplayDateStart = DateTime.Parse(dt.Rows[0]["source_dt"].ToString());
        }

        private void loadData(int deliveryID1)
        {
            string query = "SELECT CI.comp_id , D.source_id , D.destination_id , D.source_dt FROM Delivery as D , ComplaintItem as CI WHERE delivery_id = '" + deliveryID1 + "' and D.comp_item_id = CI.comp_item_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_compID.Text = dt.Rows[0]["comp_id"].ToString();
                setSourceDetails(Int32.Parse(dt.Rows[0]["source_id"].ToString()));
                setDestinationDetails(Int32.Parse(dt.Rows[0]["destination_id"].ToString()));
                txt_sourceSentDate.Text = dt.Rows[0]["source_dt"].ToString();
            }
            setDestinationDtLimit(deliveryID1);
        }

        private void setDestinationDetails(int locID1)
        {
            string query = "SELECT location_name from Location WHERE location_id = '" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if(dt.Rows.Count == 1)
            {
                txt_destinationID.Text = locID1.ToString();
                txt_destinationName.Text = dt.Rows[0]["location_name"].ToString();
            }

        }

        private void setSourceDetails(int locID1)
        {
            string query = "SELECT location_name from Location WHERE location_id = '" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if(dt.Rows.Count == 1)
            {
                txt_sourceID.Text = locID1.ToString();
                txt_sourceName.Text = dt.Rows[0]["location_name"].ToString();
            }
        }

        private bool validate()
        {
            bool check = true;

            //Delivery ID
            if (Validation.validate(deliveryID_Notify, CRMdbData.Delivery.delivery_id.validate(cmb_deliveryID.Text), CRMdbData.Delivery.delivery_id.Error)) { }
            else { check = false; }

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(txt_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Destination Received Date
            if (Validation.validate(destinationReceivedDate_Notify, CRMdbData.Delivery.destination_dt.validate(dt_destinationReceivedDate.Text), CRMdbData.Delivery.destination_dt.Error)) { }
            else { check = false; }

            return check;
        }

        ~Record_Delivered_Item_Window() { }

        private void cmb_deliveryID_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_deliveryID.Text.Length > 0)
                {
                    loadData(Int32.Parse(cmb_deliveryID.Text));
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

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(txt_compID.Text);
                    int deliveryID = Int32.Parse(cmb_deliveryID.Text);
                    DateTime desDt = dt_destinationReceivedDate.SelectedDate.Value.Date;
                    string query = "UPDATE Delivery SET destination_dt = '" + desDt + "' WHERE delivery_id = '" + deliveryID + "' ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 6 then 7 when comp_status_id = 28 then 29 when comp_status_id = 9 then 10 when comp_status_id = 31 then 32 when comp_status_id = 13 then 14 when comp_status_id = 35 then 36 when comp_status_id = 15 then 16 when comp_status_id = 37 then 38 when comp_status_id = 20 then 21 when comp_status_id = 41 then 42 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' SELECT @COMPstatusID as comp_status_id , L.location_id , L.location_name FROM Location as L , Complaint as C WHERE C.comp_id = '" + compID + "' AND C.recordedLocation_id = L.location_id ";
                    
                    Database db = new Database();
                    System.Data.DataTable dt = db.GetData(query);
                    int compStatusID = 0;
                    compStatusID = Int32.Parse(dt.Rows[0]["comp_status_id"].ToString());
                    DatabaseBased_Objects.Location loc;

                    if (compStatusID > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        if(compStatusID == 7 || compStatusID == 29)
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Assign_Factory_Window(compID));
                        }
                        else if (compStatusID == 10 || compStatusID == 32)
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Factory_Item_Decision_Window(compID));
                        }
                        else if (compStatusID == 14 || compStatusID == 36)
                        {
                            loc = new DatabaseBased_Objects.Location();
                            loc.locID = Int32.Parse(dt.Rows[0]["location_id"].ToString());
                            loc.locName = dt.Rows[0]["location_name"].ToString();
                            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID , loc ));
                        }
                        else if (compStatusID == 16)
                        {
                            SMSMessages.sendMessage(SMSMessages.getCusTp(compID), "Repaired Item is ready for you to pick up at the Showroom for Complaint ID '" + compID + "' ");
                            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_To_Customer(compID));
                        }
                        else if (compStatusID == 38)
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Close_Batch_Item_Complaint_Window(compID));
                        }
                        else if (compStatusID == 21)
                        {
                            SMSMessages.sendMessage(SMSMessages.getCusTp(compID), "New Item is ready for you to pick up at the Showroom for Complaint ID '" + compID + "' ");
                            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_To_Customer(compID));
                        }
                        else if (compStatusID == 42)
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Close_Batch_Item_Complaint_Window(compID));
                        }
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
