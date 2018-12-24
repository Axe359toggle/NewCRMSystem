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
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Window
    {
        private int locID = 0;
        private string locName = "";
        private string locType = "";
        private string addrNo = "";
        private string addrLane = "";
        private string addrTown = "";
        private string addrCity = "";
        private string tp = "";

        private bool showdialogstatus;
        private bool onlyAllowSearch = false;

        public Location()
        {
            InitializeComponent();
            rbnInsert.IsChecked = true;
        }

        public Location(bool dialogstatus)
        {
            InitializeComponent();
            showdialogstatus = dialogstatus;
            rbnSearch.IsChecked = true;
        }

        public Location(bool dialogstatus , string LocationType)
        {
            try
            {
                InitializeComponent();
                showdialogstatus = dialogstatus;
                rbnSearch.IsChecked = true;
                rbnInsert.IsEnabled = false;
                onlyAllowSearch = true;

                foreach (ComboBoxItem item in cmb_LocationType.Items)
                {
                    if (item.Content.ToString() == LocationType)
                    {
                        cmb_LocationType.SelectedValue = item;
                        break;
                    }
                }

                chk_LocationType.IsChecked = true;
                chk_LocationType.IsEnabled = false;
                cmb_LocationType.IsHitTestVisible = false;
                cmb_LocationType.Focusable = false;
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

        public Location(char option)
        {
            InitializeComponent();
            if (option.Equals("i"))
            {
                rbnInsert.IsChecked = true;
            }
            else if (option.Equals("s"))
            {
                rbnSearch.IsChecked = true;
            }
        }

        public Location(char option, string locID1)
        {
            InitializeComponent();
            if (option.Equals("u"))
            {
                txt_LocationID.Text = locID1;
                rbnUpdate.IsChecked = true;
            }
            else if (option.Equals("s"))
            {
                txt_LocationID.Text = locID1;
                rbnSearch.IsChecked = true;
            }
        }

        private void clearText()
        {
            txt_LocationID.Text = "";
            cmb_LocationType.Text = "";
            txt_LocationName.Text = "";
            txt_AddrNo.Text = "";
            txt_AddrLane.Text = "";
            txt_AddrTown.Text = "";
            txt_AddrCity.Text = "";
            txt_Tp.Text = "";
        }

        private void setErrorImagesNull()
        {
            locationID_Notify.Source = null;
            locationType_Notify.Source = null;
            locationName_Notify.Source = null;
            addrNo_Notify.Source = null;
            addrLane_Notify.Source = null;
            addrTown_Notify.Source = null;
            addrCity_Notify.Source = null;
            tp_Notify.Source = null;
        }

        private void hide_chk(Visibility visibility)
        {
            chk_LocationID.Visibility = visibility;
            chk_LocationName.Visibility = visibility;
            chk_LocationType.Visibility = visibility;
            chk_AddrNo.Visibility = visibility;
            chk_AddrLane.Visibility = visibility;
            chk_AddrTown.Visibility = visibility;
            chk_AddrCity.Visibility = visibility;
            chk_Tp.Visibility = visibility;
        }
        
        //load Insert option
        private void setInsert()
        {
            btnProcess.Content = "Insert";
            rbnUpdate.IsEnabled = false;
            txt_LocationID.IsReadOnly = true;
            txt_LocationID.IsEnabled = false;
            btn_tpAdd.IsEnabled = false;
            btn_tpRemove.IsEnabled = false;
            btn_ok.IsEnabled = false;
            setErrorImagesNull();
            hide_chk(Visibility.Hidden);
        }

        //load Update option
        private void setUpdate()
        {

            btnProcess.Content = "Update";
            rbnUpdate.IsEnabled = true;
            txt_LocationID.IsReadOnly = true;
            txt_LocationID.IsEnabled = true;
            btn_tpAdd.IsEnabled = true;
            btn_tpRemove.IsEnabled = true;
            btn_ok.IsEnabled = true;
            setErrorImagesNull();
            hide_chk(Visibility.Hidden);
        }

        //load Search option
        private void setSearch()
        {
            btnProcess.Content = "Search";
            rbnUpdate.IsEnabled = false;
            txt_LocationID.IsReadOnly = false;
            txt_LocationID.IsEnabled = true;
            btn_tpAdd.IsEnabled = false;
            btn_tpRemove.IsEnabled = false;
            btn_ok.IsEnabled = false;
            setErrorImagesNull();
            hide_chk(Visibility.Visible);
        }

        private bool validate(bool value)
        {
            bool check = true;
            
            if (value == true)
            {
                //Location ID
                if (Validation.validate(locationID_Notify, CRMdbData.Location.location_id.validate(txt_LocationID.Text), CRMdbData.Location.location_id.Error)) { }
                else { check = false; }
            }

            if (value == false)
            {
                //Telephone
                if (Validation.validate(tp_Notify, CRMdbData.Location.location_tp.validate(txt_Tp.Text), CRMdbData.Location.location_tp.Error)) { }
                else { check = false; }
            }

            //Location Type
            if (Validation.validate(locationType_Notify, CRMdbData.Location.location_type.validate(cmb_LocationType.Text), CRMdbData.Location.location_type.Error)) { }
            else { check = false; }

            //Location Name
            if (Validation.validate(locationName_Notify, CRMdbData.Location.location_name.validate(txt_LocationName.Text), CRMdbData.Location.location_name.Error)) { }
            else { check = false; }

            //Address No.
            if (Validation.validate(addrNo_Notify, CRMdbData.Location.addr_no.validate(txt_AddrNo.Text), CRMdbData.Location.addr_no.Error)) { }
            else { check = false; }

            //Address Lane
            if (Validation.validate(addrLane_Notify, CRMdbData.Location.addr_lane.validate(txt_AddrLane.Text), CRMdbData.Location.addr_lane.Error)) { }
            else { check = false; }

            //Address Town
            if (Validation.validate(addrTown_Notify, CRMdbData.Location.addr_town.validate(txt_AddrTown.Text), CRMdbData.Location.addr_town.Error)) { }
            else { check = false; }

            //Address City
            if (Validation.validate(addrCity_Notify, CRMdbData.Location.addr_city.validate(txt_AddrCity.Text), CRMdbData.Location.addr_city.Error)) { }
            else { check = false; }
            
            return check;
        }

        private void validate_LocID_Tp()
        {
            //Location ID
            if (Validation.validate(locationID_Notify, CRMdbData.Location.location_id.validate(txt_LocationID.Text), CRMdbData.Location.location_id.Error)) { }

            //Telephone
            if (Validation.validate(tp_Notify, CRMdbData.Location.location_tp.validate(txt_Tp.Text), CRMdbData.Location.location_tp.Error)) { }
        }

        private void refreshTP_datagrid(int locID1)
        {
            string query = "Select location_tp from Location_tp where location_id = '" + locID1 + "' ";
            Database db = new Database();
            telephone_Datagrid.ItemsSource = db.GetData(query).AsDataView();
        }
        
        ~Location() { }

        private void rbnInsert_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnInsert.IsChecked == true)
            {
                clearText();
                setInsert();
            }

        }

        private void rbnUpdate_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnUpdate.IsChecked == true)
            {
                setUpdate();
            }
        }

        private void rbnSearch_Checked(object sender, RoutedEventArgs e)
        {
            if (rbnSearch.IsChecked == true)
            {
                setSearch();
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbnInsert.IsChecked == true)
                {
                    if (validate(false)) //validates without Location ID
                    {

                        locType = cmb_LocationType.Text;
                        locName = txt_LocationName.Text;
                        addrNo = txt_AddrNo.Text;
                        addrLane = txt_AddrLane.Text;
                        addrTown = txt_AddrTown.Text;
                        addrCity = txt_AddrCity.Text;
                        tp = txt_Tp.Text;
                        
                        Database db = new Database();
                        string query = "INSERT INTO Location (location_type ,location_name ,addr_no ,addr_lane ,addr_town ,addr_city ) VALUES ('" + locType + "','" + locName+"','"+addrNo+"','"+addrLane+"','"+addrTown+"','"+addrCity+"') DECLARE @ID int = SCOPE_IDENTITY() INSERT INTO Location_tp (location_id ,location_tp ) VALUES (@ID,'"+tp+"')  SELECT @ID as location_id";
                        
                        locID = Int32.Parse(db.GetData(query).Rows[0]["location_id"].ToString());

                        if (locID > 0)
                        {
                            txt_LocationID.Text = locID.ToString();
                            GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                            rbnUpdate.IsChecked = true;
                        }
                        else
                        {
                            GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Failed();
                        }

                    }
                }
                else if (rbnUpdate.IsChecked == true)
                {
                    if (validate(true)) //validates with Location ID
                    {
                        locID = Int32.Parse(txt_LocationID.Text);
                        locType = cmb_LocationType.Text;
                        locName = txt_LocationName.Text;
                        addrNo = txt_AddrNo.Text;
                        addrLane = txt_AddrLane.Text;
                        addrTown = txt_AddrTown.Text;
                        addrCity = txt_AddrCity.Text;

                        string query = " UPDATE Location SET location_type = '" + locType + "' , location_name = '" + locName + "' , addr_no = '" + addrNo + "' , addr_lane = '" + addrLane + "' , addr_town = '" + addrTown + "' , addr_city = '" + addrCity + "' WHERE location_id = " + locID + " ";
                        Database db = new Database();

                        if (db.Save_Del_Update(query) > 0)
                        {
                            MessageBox.Show("Data Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Data updation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
                else if (rbnSearch.IsChecked == true)
                {

                    string query = "SELECT location_id ,location_type ,location_name ,addr_no ,addr_lane ,addr_town ,addr_city from Location";
                    int x = 0;

                    void checkX()
                    {
                        if (x > 0)
                        {
                            query = query + " AND";
                        }
                    }

                    if (chk_LocationID.IsChecked == true || chk_LocationType.IsChecked == true || chk_LocationName.IsChecked == true || chk_AddrNo.IsChecked == true || chk_AddrLane.IsChecked == true || chk_AddrTown.IsChecked == true || chk_AddrCity.IsChecked == true || chk_Tp.IsChecked == true)
                    {
                        query = query + " WHERE";
                    }

                    //Location ID
                    if (chk_LocationID.IsChecked == true && txt_LocationID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " location_id LIKE '%" + txt_LocationID.Text + "%'";
                        x++;
                    }

                    //Location Type
                    if (chk_LocationType.IsChecked == true && cmb_LocationType.Text.Length > 0)
                    {
                        checkX();
                        query = query + " location_type LIKE '%" + cmb_LocationType.Text + "%'";
                        x++;
                    }

                    //Location Name
                    if (chk_LocationName.IsChecked == true && txt_LocationName.Text.Length > 0)
                    {
                        checkX();
                        query = query + " location_name LIKE '%" + txt_LocationName.Text + "%'";
                        x++;
                    }

                    //Address No
                    if (chk_AddrNo.IsChecked == true && txt_AddrNo.Text.Length > 0)
                    {
                        checkX();
                        query = query + " addr_no LIKE '%" + txt_AddrNo.Text + "%'";
                        x++;
                    }

                    //Address Lane
                    if (chk_AddrLane.IsChecked == true && txt_AddrLane.Text.Length > 0)
                    {
                        checkX();
                        query = query + " addr_lane LIKE '%" + txt_AddrLane.Text + "%'";
                        x++;
                    }

                    //Address Town
                    if (chk_AddrNo.IsChecked == true && txt_AddrNo.Text.Length > 0)
                    {
                        checkX();
                        query = query + " addr_town LIKE '%" + txt_AddrNo.Text + "%'";
                        x++;
                    }

                    //Address City
                    if (chk_AddrNo.IsChecked == true && txt_AddrNo.Text.Length > 0)
                    {
                        checkX();
                        query = query + " addr_city LIKE '%" + txt_AddrNo.Text + "%'";
                        x++;
                    }

                    //Telephone
                    if (chk_Tp.IsChecked == true && txt_Tp.Text.Length > 0)
                    {
                        checkX();
                        query = query + " location_id IN (SELECT location_id FROM Location_tp WHERE location_tp LIKE '%" + txt_Tp.Text + "%')";
                        x++;
                    }

                    

                    Database db = new Database();
                    location_Datagrid.ItemsSource = db.GetData(query).AsDataView();

                }
                else
                {
                    MessageBox.Show("Please select an option", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void btn_tpAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                validate_LocID_Tp();
                if (CRMdbData.Location.location_id.validate(txt_LocationID.Text) && CRMdbData.Location.location_tp.validate(txt_Tp.Text))
                {
                    locID = Int32.Parse(txt_LocationID.Text);
                    tp = txt_Tp.Text;

                    string query = "INSERT INTO Location_tp (location_id,location_tp) VALUES ("+locID+",'"+tp+"')";
                    Database db = new Database();

                    

                    if (db.Save_Del_Update(query) > 0)
                    {
                        refreshTP_datagrid(locID);
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();

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

        private void btn_tpRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validate_LocID_Tp();
                if (CRMdbData.Location.location_id.validate(txt_LocationID.Text))
                {
                    MessageBoxResult result = MessageBox.Show("This action is not reversible \n Do you wish to remove this item?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if(result == MessageBoxResult.Yes)
                    {
                        locID = Int32.Parse(txt_LocationID.Text);
                        tp = txt_Tp.Text;

                        string query = "DELETE FROM Location_tp Where location_id = " + locID + " AND location_tp = '" + tp + "' ";
                        Database db = new Database();

                        if (db.Save_Del_Update(query) > 0)
                        {
                            refreshTP_datagrid(locID);
                            MessageBox.Show("Data Deleted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Data Deletion Failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
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
            Login.b1.removePreviousWindow();
            this.Hide();
        }

        private void location_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)location_Datagrid.SelectedItem;
                if (dv != null)
                {
                    locID = Int32.Parse(dv.Row.ItemArray[0].ToString());//location_id
                    txt_LocationID.Text = locID.ToString();


                    btn_ok.IsEnabled = true;

                    string locType1 = dv.Row.ItemArray[1].ToString();//location_type

                    foreach (ComboBoxItem item in cmb_LocationType.Items)
                    {
                        if(item.Content.ToString() == locType1)
                        {
                            cmb_LocationType.SelectedValue = item;
                            break;
                        }
                    }
                    

                    txt_LocationName.Text = dv.Row.ItemArray[2].ToString();//location_name
                    txt_AddrNo.Text = dv.Row.ItemArray[3].ToString();//Address No
                    txt_AddrLane.Text = dv.Row.ItemArray[4].ToString();//Address Lane
                    txt_AddrTown.Text = dv.Row.ItemArray[5].ToString();//Address Town
                    txt_AddrCity.Text = dv.Row.ItemArray[6].ToString();//Address City

                    refreshTP_datagrid(locID);

                    if (!onlyAllowSearch)
                    {
                        rbnUpdate.IsChecked = true;
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

        private void telephone_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)telephone_Datagrid.SelectedItem;
                if (dv != null)
                {
                    txt_Tp.Text = dv.Row.ItemArray[0].ToString();//Telephone
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

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (showdialogstatus == true)
                {
                    Login.b1.removePreviousWindow();
                    DialogResult = true;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
