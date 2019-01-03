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
    /// Interaction logic for Staff_Complaint_Window.xaml
    /// </summary>
    public partial class Staff_Complaint_Window : Window
    {
        public Staff_Complaint_Window()
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
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

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , StaffComplaint AS SC WHERE C.comp_id = SC.comp_id AND C.comp_status_id = 24 ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void setRelatedLocationDetails(int locID1)
        {
            string query = "SELECT location_name from Location WHERE location_id = '" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_relLocID.Text = locID1.ToString();
                txt_relLocName.Text = dt.Rows[0]["location_name"].ToString();
            }

        }

        private void setRecordedLocationDetails(int locID1)
        {
            string query = "SELECT location_name from Location WHERE location_id = '" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_recLocID.Text = locID1.ToString();
                txt_recLocName.Text = dt.Rows[0]["location_name"].ToString();
            }

        }

        private void loadData(int compID)
        {
            string query = "SELECT C.ref_id , CC.comp_method , Cus.cus_id , Cus.cus_name , Cus.cus_tp , Cus.cus_email , C.relatedLocation_id , C.recordedLocation_id , SC.staff_id , SC.staff_name , SC.description  FROM Complaint as C , StaffComplaint as SC , CustomerComplaint as CC , Customer as Cus  WHERE C.comp_id  = '" + compID + "' and C.comp_id = SC.comp_id AND C.comp_id = CC.comp_id AND CC.cus_id = Cus.cus_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows.Count == 1)
            {
                txt_refID.Text = dt.Rows[0]["ref_id"].ToString();
                txt_compMethod.Text = dt.Rows[0]["comp_method"].ToString();
                txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
                txt_cusName.Text = dt.Rows[0]["cus_name"].ToString();
                txt_cusTp.Text = dt.Rows[0]["cus_tp"].ToString();
                txt_cusEmail.Text = dt.Rows[0]["cus_email"].ToString();
                txt_staffID.Text = dt.Rows[0]["staff_id"].ToString();
                txt_staffName.Text = dt.Rows[0]["staff_name"].ToString();
                txt_description.Text = dt.Rows[0]["description"].ToString();

                setRelatedLocationDetails(Int32.Parse(dt.Rows[0]["relatedLocation_id"].ToString()));
                setRecordedLocationDetails(Int32.Parse(dt.Rows[0]["recordedLocation_id"].ToString()));
            }
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Remarks
            if (Validation.validate(remarks_Notify, CRMdbData.StaffComplaint.remarks.validate(txt_remarks.Text), CRMdbData.StaffComplaint.remarks.Error)) { }
            else { check = false; }

            //Close Complaint
            if (Validation.validate(closeComplaint_Notify, chk_closeComplaint.IsChecked == true, "This must be checked")) { }
            else { check = false; }

            return check;
        }

        ~Staff_Complaint_Window() { }

        private void Cmb_compID_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_compID.Text.Length > 0)
                {
                    loadData(Int32.Parse(cmb_compID.Text));
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

        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    string query = "DECLARE @COMPstatusID int SET @COMPstatusID = 25 ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID , closed_dt = GETDATE() WHERE comp_id = '" + compID + "' ";

                    Database db = new Database();

                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
