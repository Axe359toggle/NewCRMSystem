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
    /// Interaction logic for Factory_Item_Decision_Window.xaml
    /// </summary>
    public partial class Factory_Item_Decision_Window : Window
    {
        public Factory_Item_Decision_Window()
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                hide_investigationDT(Visibility.Collapsed);
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

        public Factory_Item_Decision_Window(int compID1)
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                hide_investigationDT(Visibility.Collapsed);
                cmb_compID.SelectedItem = compID1.ToString();
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

        //Set investigation date limit
        private void setInvDtLimit(int compID1)
        {
            dt_investigationDate.DisplayDateEnd = DateTime.Today.Date;
            string query = "SELECT D.destination_dt FROM Complaint AS C , Delivery AS D , ComplaintItem AS CI WHERE D.destination_id = " + Login.LocID + " AND D.comp_item_id = CI.comp_item_id AND C.comp_id = CI.comp_id AND C.comp_id = '" + compID1 + "'   ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);
            dt_investigationDate.DisplayDateStart = DateTime.Parse(dt.Rows[0]["destination_dt"].ToString());
        }

        private void bindCompIDList()
        {
            string query = "SELECT C.comp_id FROM Complaint AS C , Delivery AS D , ComplaintItem AS CI WHERE D.destination_id = " + Login.LocID + " AND D.comp_item_id = CI.comp_item_id AND C.comp_id = CI.comp_id AND ( C.comp_status_id = 10 OR C.comp_status_id = 32 )  ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            cmb_compID.Items.Clear();

            foreach (System.Data.DataRow dr in dt.Rows)
                cmb_compID.Items.Add(dr["comp_id"].ToString());

            cmb_compID.SelectedIndex = 0;
        }

        private void hide_investigationDT(Visibility visibility1)
        {
            lbl_investigationDt.Visibility = visibility1;
            dt_investigationDate.Visibility = visibility1;
        }

        private void setRepair()
        {
            hide_investigationDT(Visibility.Collapsed);
        }

        private void setInvestigation()
        {
            hide_investigationDT(Visibility.Visible);
        }

        private void loadDefectImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_defectImage.Source = imageSource;
        }

        private void loadItemImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_itemImage.Source = imageSource;
        }

        private void loadData(int compID)
        {
            string query = "SELECT IT.item_type_id , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , IT.item_pic , CI.item_defect , CI.item_defect_img , CI.item_remarks FROM ItemType as IT , ComplaintItem as CI  WHERE CI.comp_id  = '" + compID + "' and CI.item_type_id = IT.item_type_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);
            setInvDtLimit(compID);
            if (dt.Rows.Count == 1)
            {
                txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
                txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
                txt_category.Text = dt.Rows[0]["item_category"].ToString();
                txt_name.Text = dt.Rows[0]["item_name"].ToString();
                txt_size.Text = dt.Rows[0]["item_size"].ToString();
                txt_itemDefect.Text = dt.Rows[0]["item_defect"].ToString();
                txt_itemRemarks.Text = dt.Rows[0]["item_remarks"].ToString();

                string imagePath1 = dt.Rows[0]["item_defect_img"].ToString();
                loadDefectImageFromLocal(imagePath1);
                string imagePath2 = dt.Rows[0]["item_pic"].ToString();
                loadItemImageFromLocal(imagePath2);
            }
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Item Decision
            if (Validation.validate(itemDecision_Notify, rbn_investigation.IsChecked== true || rbn_repair.IsChecked==true, "Choose an Option")) { }
            else { check = false; }

            if(rbn_investigation.IsChecked == true)
            {
                //Investigation Date
                if (Validation.validate(investigationDate_Notify, CRMdbData.Investigation.investigation_dt.validate(dt_investigationDate.Text), CRMdbData.Investigation.investigation_dt.Error)) { }
                else { check = false; }
            }

            //Factory Manager ID
            if (CRMdbData.Manager.emp_id.validate(Login.EmpID.ToString())) { }
            else
            {
                MessageBox.Show("Logged Employee ID Error ", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                check = false;
            }


            return check;
        }

        ~Factory_Item_Decision_Window() { }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (validate())
                {
                    int compID = Int32.Parse(cmb_compID.Text);
                    int facManagerID = Login.EmpID;
                    string query = "";

                    if(rbn_investigation.IsChecked == true)
                    {
                        DateTime invDate = dt_investigationDate.SelectedDate.Value.Date;
                        query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Investigation (comp_item_id , factoryManager , investigation_dt ) VALUES ( @COMPitemID , " + facManagerID + " , '" + invDate + "' ) UPDATE ComplaintItem SET item_decision = 'Investigation' WHERE comp_item_id = @COMPitemID ";
                        query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 10 then 18 when comp_status_id = 32 then 39 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                        query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";
                    }
                    else if (rbn_repair.IsChecked == true)
                    {
                        query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "') INSERT INTO Repair (comp_item_id , factoryManager ) VALUES ( @COMPitemID , " + facManagerID + " ) UPDATE ComplaintItem SET item_decision = 'Repair' WHERE comp_item_id = @COMPitemID ";
                        query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 10 then 11 when comp_status_id = 32 then 33 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                        query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = '" + compID + "' ";
                    }
                    
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

        private void cmb_compID_DropDownClosed(object sender, EventArgs e)
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

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void rbn_repair_Checked(object sender, RoutedEventArgs e)
        {
            if(rbn_repair.IsChecked == true)
            {
                setRepair();
            }
        }

        private void rbn_investigation_Checked(object sender, RoutedEventArgs e)
        {
            if (rbn_investigation.IsChecked == true)
            {
                setInvestigation();
            }
        }
    }
}
