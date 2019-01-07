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

using System.Data;

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
        
        Visibility col = Visibility.Collapsed;
        Visibility vis = Visibility.Visible;

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

        private void reset()
        {
            gb_cusComp.Visibility = col;
            gb_staffComp.Visibility = col;
            gb_compItem.Visibility = col;
            gb_itemType.Visibility = col;
            gb_rebate.Visibility = col;
            gb_delivery.Visibility = col;
            gb_repair.Visibility = col;
            gb_inv.Visibility = col;
        }

        private void setCusComp(int compID1)
        {
            gb_cusComp.Visibility = vis;
            string query = "SELECT CUS.cus_id , CUS.cus_name , CUS.cus_tp , CUS.cus_email , CC.cus_comp_type , CC.comp_method FROM CustomerComplaint as CC , Customer as CUS WHERE CC.comp_id = '" + compID1 + "' AND CC.cus_id = CUS.cus_id  ";
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
            gb_staffComp.Visibility = vis;
            string query = "SELECT SC.staff_id , SC.staff_name , SC.description , SC.remarks , SC.closed_manager FROM StaffComplaint as SC WHERE SC.comp_id = '" + compID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_staffID.Text = dt.Rows[0]["staff_id"].ToString();
            txt_staffName.Text = dt.Rows[0]["staff_name"].ToString();
            txt_description.Text = dt.Rows[0]["description"].ToString();
            txt_staffRemarks.Text = dt.Rows[0]["remarks"].ToString();

            if (dt.Rows[0]["closed_manager"].ToString().Length > 0)
            {
                txt_staffCompClosedManagerID.Text = dt.Rows[0]["closed_manager"].ToString();
                txt_staffCompClosedManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["closed_manager"].ToString()));
            }
        }

        private void setCompItem(int compID1)
        {
            gb_compItem.Visibility = vis;
            string query = "SELECT CI.received_dt , CI.shoe_side , CI.item_id , CI.item_defect , CI.item_defect_img , CI.item_remarks , CI.item_decision FROM ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_receivedDt.Text = dt.Rows[0]["received_dt"].ToString();
            txt_shoeSide.Text = dt.Rows[0]["shoe_side"].ToString();
            txt_itemID.Text = dt.Rows[0]["item_id"].ToString();
            txt_defect.Text = dt.Rows[0]["item_defect"].ToString();
            loadDefectImageFromLocal(dt.Rows[0]["item_defect_img"].ToString());
            txt_itemRemarks.Text = dt.Rows[0]["item_remarks"].ToString();
            txt_itemDecision.Text = dt.Rows[0]["item_decision"].ToString();
        }

        private void setItemType(int compID1)
        {
            gb_itemType.Visibility = vis;
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , IT.item_pic FROM ItemType as IT , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.item_type_id = IT.item_type_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();
            loadItemImageFromLocal(dt.Rows[0]["item_pic"].ToString());

        }

        private void setRebate(int compID1)
        {
            gb_rebate.Visibility = vis;
            string query = "SELECT I.item_price , R.rebate_percentage , R.customer_choice , R.hQManager , R.shrmManager FROM Rebate as R , ComplaintItem as CI , Item as I WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id AND CI.item_id = I.item_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemPrice.Text = dt.Rows[0]["item_price"].ToString();
            txt_rebatePercentage.Text = dt.Rows[0]["rebate_percentage"].ToString();
            txt_cusChoice.Text = dt.Rows[0]["customer_choice"].ToString();
            
            txt_rebHQManagerID.Text = dt.Rows[0]["hQManager"].ToString();
            txt_rebHQManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["hQManager"].ToString()));
            
            txt_rebShrmManagerID.Text = dt.Rows[0]["shrmManager"].ToString();
            txt_rebShrmManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["shrmManager"].ToString()));
        }

        private void setDelivery(int compID1)
        {
            gb_delivery.Visibility = vis;
            string query = "SELECT D.delivery_id , D.source_id , D.source_dt , D.destination_id , D.destination_dt FROM Delivery as D , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = D.comp_item_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            deliveryDatagrid.ItemsSource = dt.AsDataView();
        }

        private void setRepair(int compID1)
        {
            gb_repair.Visibility = vis;
            string query = "SELECT R.repair_remarks , R.repair_dt , R.factoryManager FROM Repair as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_repairRemarks.Text = dt.Rows[0]["repair_remarks"].ToString();
            txt_repairedDt.Text = dt.Rows[0]["repair_dt"].ToString();

            txt_repFacManagerID.Text = dt.Rows[0]["factoryManager"].ToString();
            txt_repFacManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["factoryManager"].ToString()));
        }

        private void setInvestigation(int compID1)
        {
            gb_inv.Visibility = vis;
            string query = "SELECT INV.investigation_dt , INV.newItem_id , INV.factoryManager , INV.hQManager FROM Investigation as INV , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_invDt.Text = dt.Rows[0]["investigation_dt"].ToString();
            txt_newItemID.Text = dt.Rows[0]["newItem_id"].ToString();

            txt_invFacManagerID.Text = dt.Rows[0]["factoryManager"].ToString();
            txt_invFacManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["factoryManager"].ToString()));

            txt_invHQManagerID.Text = dt.Rows[0]["hQManager"].ToString();
            txt_invHQManagerName.Text = getManagerName(Int32.Parse(dt.Rows[0]["hQManager"].ToString()));
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

        //Load Item Image from local file
        private void loadItemImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_itemImage.Source = imageSource;
        }

        //Load Defect Image from local file
        private void loadDefectImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_defect.Source = imageSource;
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

        private void checkConditionsAndLoad(int compID1)
        {
            reset();
            //Checking if complaint type is Customer or Staff
            string query = "SELECT C.comp_type FROM Complaint as C WHERE comp_id = '" + compID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            if (dt.Rows[0]["comp_type"].ToString().Equals("Customer"))
            {
                setCusComp(compID1);
                
                //Checking if Customer Complaint Type is Staff or Item
                query = "SELECT cus_comp_type FROM CustomerComplaint WHERE comp_id = '" + compID1 + "' ";
                dt = db.GetData(query);
                if (dt.Rows[0]["cus_comp_type"].ToString().Equals("Staff"))
                {
                    //Checking if Staff Complaint is set
                    query = "SELECT comp_id FROM StaffComplaint WHERE comp_id = '" + compID1 + "' ";
                    dt = db.GetData(query);

                    if(dt.Rows.Count == 1)
                    {
                        setStaffComp(compID1);
                    }
                }
                else if (dt.Rows[0]["cus_comp_type"].ToString().Equals("Item"))
                {
                    //Checking if Complaint Item is set
                    query = "SELECT comp_id FROM ComplaintItem WHERE comp_id = '" + compID1 + "' ";
                    dt = db.GetData(query);

                    if (dt.Rows.Count == 1)
                    {
                        setCompItem(compID1);
                        setItemType(compID1);

                        //Checking if Rebate is set
                        query = "SELECT R.comp_item_id FROM Rebate as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
                        dt = db.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            setRebate(compID1);

                            //Check if customer choice is set
                            query = "SELECT R.customer_choice FROM Rebate as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
                            dt = db.GetData(query);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["customer_choice"].ToString().Equals("Rejected"))
                                {
                                    //Checking if delivery is set
                                    query = "SELECT D.delivery_id FROM ComplaintItem as CI , Delivery as D WHERE CI.comp_id = '" + compID1 + "' AND D.comp_item_id = CI.comp_item_id ";
                                    dt = db.GetData(query);
                                    if (dt.Rows.Count > 0)
                                    {
                                        setDelivery(compID1);

                                        //Checking if Item Decision is set
                                        query = "SELECT CI.item_decision FROM ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' ";
                                        dt = db.GetData(query);

                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["item_decision"].ToString().Equals("Repair"))
                                            {
                                                //Checking if Repair is set
                                                query = "SELECT R.comp_item_id FROM Repair as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
                                                dt = db.GetData(query);

                                                if (dt.Rows.Count > 0)
                                                {
                                                    setRepair(compID1);
                                                }
                                            }
                                            else if (dt.Rows[0]["item_decision"].ToString().Equals("Investigation"))
                                            {
                                                //Checking if Investigation is set
                                                query = "SELECT I.comp_item_id FROM Investigation as I , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = I.comp_item_id ";
                                                dt = db.GetData(query);

                                                if (dt.Rows.Count > 0)
                                                {
                                                    setInvestigation(compID1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (dt.Rows[0]["comp_type"].ToString().Equals("Manager"))
            {
                //Checking if Complaint Item is set
                query = "SELECT comp_id FROM ComplaintItem WHERE comp_id = '" + compID1 + "' ";
                dt = db.GetData(query);

                if (dt.Rows.Count == 1)
                {
                    setCompItem(compID1);
                    setItemType(compID1);

                    //Checking if delivery is set
                    query = "SELECT D.delivery_id FROM ComplaintItem as CI , Delivery as D WHERE CI.comp_id = '" + compID1 + "' AND D.comp_item_id = CI.comp_item_id ";
                    dt = db.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        setDelivery(compID1);

                        //Checking if Item Decision is set
                        query = "SELECT CI.item_decision FROM ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' ";
                        dt = db.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["item_decision"].ToString().Equals("Repair"))
                            {
                                //Checking if Repair is set
                                query = "SELECT R.comp_item_id FROM Repair as R , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = R.comp_item_id ";
                                dt = db.GetData(query);

                                if (dt.Rows.Count > 0)
                                {
                                    setRepair(compID1);
                                }
                            }
                            else if (dt.Rows[0]["item_decision"].ToString().Equals("Investigation"))
                            {
                                //Checking if Investigation is set
                                query = "SELECT I.comp_item_id FROM Investigation as I , ComplaintItem as CI WHERE CI.comp_id = '" + compID1 + "' AND CI.comp_item_id = I.comp_item_id ";
                                dt = db.GetData(query);

                                if (dt.Rows.Count > 0)
                                {
                                    setInvestigation(compID1);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void loadData(int compID1)
        {
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

            if (dt.Rows[0]["closed_dt"].ToString().Length > 0)
            {
                txt_closedDate.Text = dt.Rows[0]["closed_dt"].ToString();
            }
            txt_compType.Text = dt.Rows[0]["comp_type"].ToString();


            checkConditionsAndLoad(compID1);//Load remaining data with only avaliable data as visible
        }

        ~Complaint_Details_Window() { }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

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

        private void Btn_recEmpSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                View_Manager_Details w = new View_Manager_Details(Int32.Parse(txt_recEmpID.Text));
                w.Show();
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
