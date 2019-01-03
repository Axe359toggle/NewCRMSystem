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
    /// Interaction logic for CustomerComplaintWindow.xaml
    /// </summary>
    public partial class Customer_Complaint_Window : Window
    {
        public Customer_Complaint_Window()
        {
            InitializeComponent();
        }
        
        private int refID;
        private int cusID;
        private string compMethod;
        private string compType1 = "Customer";
        private string compType2;
        private int relShrmID;

        private bool validateRefID()
        {
            bool check = false;

            if (txt_refID.Text.Length > 0)
            {
                refID = Int32.Parse(txt_refID.Text);

                string query = " SELECT refID from Reference WHERE refID = " + refID + " ";
                Database db = new Database();
                System.Data.DataTable dt = db.GetData(query);

                if (dt.Rows.Count == 1)
                {
                    check = true;
                }
            }
            else
            {
                check = true;
            }
            
            return check;
        }

        private bool validate()
        {
            bool check = true;

            //Reference ID
            if (Validation.validate(refID_Notify, CRMdbData.Reference.ref_id.validate(txt_refID.Text) && validateRefID(), CRMdbData.Reference.ref_id.Error)) { }
            else { check = false; }

            //Customer ID
            if (Validation.validate(cusID_Notify, CRMdbData.Customer.cus_id.validate(txt_cusID.Text), CRMdbData.Customer.cus_id.Error)) { }
            else { check = false; }

            //Complaint Method
            if (Validation.validate(compMethod_Notify, rbn_byCall.IsChecked == true || rbn_inPerson.IsChecked == true, "Choose an Option")) { }
            else { check = false; }

            //Complaint Type
            if (Validation.validate(compType_Notify, rbn_staffComp.IsChecked == true || rbn_itemComp.IsChecked == true, "Choose an Option")) { }
            else { check = false; }

            //Related Showroom ID
            if (Validation.validate(relShrmID_Notify, CRMdbData.Location.location_id.validate(txt_relShrmID.Text), CRMdbData.Location.location_id.Error)) { }
            else { check = false; }


            return check;
        }

        ~Customer_Complaint_Window() { }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    cusID = Int32.Parse(txt_cusID.Text);
                    int compStatusID = 0;

                    if (rbn_byCall.IsChecked == true) { compMethod = "By Call"; compStatusID = 23; }
                    else if (rbn_inPerson.IsChecked == true) { compMethod = "In Person"; }

                    if (rbn_staffComp.IsChecked == true) { compType2 = "Staff"; }
                    else if (rbn_itemComp.IsChecked == true) { compType2 = "Item"; compStatusID = 1; }
                    
                    Database db = new Database();

                    if (txt_refID.Text.Trim().Length == 0)
                    {
                        string query1 = "INSERT INTO Reference DEFAULT VALUES DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as ref_id";
                        txt_refID.Text = db.GetData(query1).Rows[0]["ref_id"].ToString();
                    }

                    refID = Int32.Parse(txt_refID.Text);

                    relShrmID = Int32.Parse(txt_relShrmID.Text);
                    string query = "INSERT INTO Complaint (comp_type , ref_id , relatedLocation_id , comp_status_id , recordedEmp_id , recordedLocation_id) VALUES ('" + compType1 + "','" + refID + "','" + relShrmID + "' , " + compStatusID + " , " + Login.EmpID + " , " + Login.LocID + ") DECLARE @ID int = SCOPE_IDENTITY() INSERT INTO CustomerComplaint (comp_id,cus_id,comp_method,cus_comp_type) values(@ID,'" + cusID + "','" + compMethod + "','" + compType2 + "') SELECT @ID as comp_id";

                    int compID = 0;
                    compID = Int32.Parse(db.GetData(query).Rows[0]["comp_id"].ToString());

                    if (compID > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();

                        if (rbn_staffComp.IsChecked == true)
                        {
                            Login.b1.hideWindowAndOpenNextWindow(this, new Staff_Complaint(compID));
                        }
                        else if (rbn_itemComp.IsChecked == true)
                        {
                            Login.b1.hideWindowAndOpenNextWindow(this, new ReceivedItem_Details(compID));
                        }
                        //open next window
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

        private void btn_cusSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Customer_Details(true);
                if (w.ShowDialog() == true)
                {
                    txt_cusID.Text = w.txt_cusID.Text;
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void btn_shrmSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true,"Showroom");
                if (w.ShowDialog() == true)
                {
                    txt_relShrmID.Text = w.txt_LocationID.Text;
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
