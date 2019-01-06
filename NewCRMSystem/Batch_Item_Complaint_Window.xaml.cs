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
    /// Interaction logic for Batch_Item_Complaint_Window.xaml
    /// </summary>
    public partial class Batch_Item_Complaint_Window : Window
    {
        public Batch_Item_Complaint_Window()
        {
            InitializeComponent();
        }

        private int refID;
        private string compType1 = "Manager";
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

            //Related Showroom ID
            if (Validation.validate(relShrmID_Notify, CRMdbData.Location.location_id.validate(txt_relShrmID.Text), CRMdbData.Location.location_id.Error)) { }
            else { check = false; }

            return check;
        }

        ~Batch_Item_Complaint_Window() { }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compStatusID = 26;

                    Database db = new Database();

                    if (txt_refID.Text.Trim().Length == 0)
                    {
                        string query1 = "INSERT INTO Reference DEFAULT VALUES DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as ref_id";
                        txt_refID.Text = db.GetData(query1).Rows[0]["ref_id"].ToString();
                    }

                    refID = Int32.Parse(txt_refID.Text);

                    relShrmID = Int32.Parse(txt_relShrmID.Text);
                    string query = "INSERT INTO Complaint (comp_type , ref_id , relatedLocation_id , comp_status_id , recordedEmp_id , recordedLocation_id) VALUES ('" + compType1 + "','" + refID + "','" + relShrmID + "' , " + compStatusID + " , " + Login.EmpID + " , " + Login.LocID + ") DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as comp_id";

                    int compID = 0;
                    compID = Int32.Parse(db.GetData(query).Rows[0]["comp_id"].ToString());

                    if (compID > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        Login.b1.closeWindowAndOpenNextWindow(this, new ReceivedItem_Details(compID));
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

        private void btn_shrmSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true, "Showroom");
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
