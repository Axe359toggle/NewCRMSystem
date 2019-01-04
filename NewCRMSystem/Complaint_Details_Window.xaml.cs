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
    /// Interaction logic for Complaint_Details_Window.xaml
    /// </summary>
    public partial class Complaint_Details_Window : Window
    {
        public Complaint_Details_Window()
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

        int type = 0;

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void setType(int compID1)
        {
            string query = "SELECT comp_type FROM Complaint WHERE comp_id = '" + compID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows[0]["comp_type"].ToString().Equals("Customer"))
            {
                query = "SELECT cus_comp_type FROM CustomerComplaint WHERE comp_id = '" + compID1 + "' ";
                dt = db.GetData(query);
                if (dt.Rows[0]["cus_comp_type"].ToString().Equals("Staff"))
                {
                    type = 1;
                }
                else if (dt.Rows[0]["cus_comp_type"].ToString().Equals("Item"))
                {
                    query = "SELECT R.customer_choice FROM Rebate as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
                    dt = db.GetData(query);

                    if (dt.Rows[0]["customer_choice"].ToString().Equals("Accepted"))
                    {
                        type = 2;
                    }
                    else if (dt.Rows[0]["customer_choice"].ToString().Equals("Rejected"))
                    {
                        type = 3;

                        query = "SELECT CI.item_decision FROM ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' ";
                        dt = db.GetData(query);
                        if (dt.Rows[0]["item_decision"].ToString().Equals("Repair"))
                        {
                            type = 4;
                        }
                        else if (dt.Rows[0]["item_decision"].ToString().Equals("Investigation"))
                        {
                            type = 5;
                        }
                    }
                }
            }
            else if (dt.Rows[0]["comp_type"].ToString().Equals("Manager"))
            {
                type = 6;

                query = "SELECT CI.item_decision FROM ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' ";
                dt = db.GetData(query);
                if (dt.Rows[0]["item_decision"].ToString().Equals("Repair"))
                {
                    type = 7;
                }
                else if (dt.Rows[0]["item_decision"].ToString().Equals("Investigation"))
                {
                    type = 8;
                }
            }
            
                
        }

        private void setType1Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = vis;
            gb_staffComp.Visibility = vis;
            gb_compItem.Visibility = col;
            gb_itemType.Visibility = col;
            gb_rebate.Visibility = col;
            gb_delivery.Visibility = col;
            gb_repair.Visibility = col;
            gb_inv.Visibility = col;

        }

        private void setType2Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = vis;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = vis;
            gb_delivery.Visibility = col;
            gb_repair.Visibility = col;
            gb_inv.Visibility = col;

        }

        private void setType3Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = vis;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = vis;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = col;
            gb_inv.Visibility = col;

        }

        private void setType4Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = vis;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = vis;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = vis;
            gb_inv.Visibility = col;

        }

        private void setType5Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = vis;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = vis;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = col;
            gb_inv.Visibility = vis;

        }

        private void setType6Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = col;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = col;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = col;
            gb_inv.Visibility = col;

        }

        private void setType7Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = col;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = col;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = vis;
            gb_inv.Visibility = col;

        }

        private void setType8Visibility()
        {
            Visibility col = Visibility.Collapsed;
            Visibility vis = Visibility.Visible;
            gb_cusComp.Visibility = col;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = vis;
            gb_itemType.Visibility = vis;
            gb_rebate.Visibility = col;
            gb_delivery.Visibility = vis;
            gb_repair.Visibility = col;
            gb_inv.Visibility = vis;

        }

        private void setCusComp(int compID1)
        {
            string query = "SELECT CUS.cus_id , CUS.cus_name , CUS.cus_tp , CUS.cus_email , CC.cus_comp_type , CC.comp_method FROM CustomerComplaint as CC , Cusotmer as CUS WHERE CC.comp_id = '" + compID1 + "' AND CC.cus_id = CUS.cus_id  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
            txt_cusName.Text = dt.Rows[0]["cus_name"].ToString();
            txt_cusTp.Text = dt.Rows[0]["cus_tp"].ToString();
            txt_cusEmail.Text = dt.Rows[0]["cus_email"].ToString();
            txt_cusCompType.Text = dt.Rows[0]["cus_comp_type"].ToString();
            txt_compMethod.Text = dt.Rows[0]["comp_method"].ToString();
        }

        private void setStaffComp(int compID1)
        {
            string query = "SELECT SC.staff_id , SC.staff_name , SC.description , SC.remarks , SC.closed_manager FROM StaffComplaint as SC WHERE SC.comp_id = '" + compID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
        }

        private string getManagerName(int empID1)
        {
            string query = "SELECT emp_fname , emp_lname from Manager WHERE emp_id = '" + empID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            string empName = "";
            if (dt.Rows.Count == 1)
            {
                empName = dt.Rows[0]["emp_fname"].ToString() + " " + dt.Rows[0]["emp_lname"].ToString();
            }

            return empName;
        }

        private string getLocationName(int locID1)
        {
            string query = "SELECT location_name from Location WHERE location_id = '" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            string LocName = "";
            if (dt.Rows.Count == 1)
            {
                LocName = dt.Rows[0]["location_name"].ToString();
            }

            return LocName;
        }

        //set Related Location Details
        private void setRelatedLocationDetails(int locID1)
        {
            txt_relLocID.Text = locID1.ToString();
            txt_relLocName.Text = getLocationName(locID1);
        }

        //set Recorded Location Details
        private void setRecordedLocationDetails(int locID1)
        {
            txt_recLocID.Text = locID1.ToString();
            txt_recLocName.Text = getLocationName(locID1);
        }

        private void loadData(int compID1)
        {
            setType(compID1);

            string query = "SELECT C.comp_dt , C.ref_id , CS.current_status , CS.description , C.recordedEmp_id , C.relatedLocation_id , C.recordedLocation_id , C.closed_dt , C.comp_type FROM Complaint as C , ComplaintStatus as CS WHERE C.comp_id = '" + compID1 + "' AND C.comp_status_id = CS.comp_status_id  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_compDate.Text = dt.Rows[0]["comp_dt"].ToString();
            txt_refID.Text = dt.Rows[0]["ref_id"].ToString();
            txt_compStatus.Text = dt.Rows[0]["current_status"].ToString();
            txt_compStatusDes.Text = dt.Rows[0]["description"].ToString();
            txt_recEmpID.Text = dt.Rows[0]["recordedEmp_id"].ToString();

            setRelatedLocationDetails(Int32.Parse(dt.Rows[0]["relatedLocation_id"].ToString()));
            setRecordedLocationDetails(Int32.Parse(dt.Rows[0]["recordedLocation_id"].ToString()));

            txt_closedDate.Text = dt.Rows[0]["closed_dt"].ToString();
            txt_compType.Text = dt.Rows[0]["comp_type"].ToString();

            if (type == 1)
            {
                setType1Visibility();
                query = "SELECT  FROM Complaint as C , ComplaintStatus as CS WHERE C.comp_id = '" + compID1 + "' AND C.comp_status_id = CS.comp_status_id  ";
                dt = db.GetData(query);

            }
            

            
        }

        ~Complaint_Details_Window() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
