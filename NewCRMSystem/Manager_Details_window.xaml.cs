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
    /// Interaction logic for Manager_Details_window.xaml
    /// </summary>
    public partial class Manager_Details_window : Window
    {
        private int managerID = 0;
        private string title = "";
        private string fname = "";
        private string lname = "";
        private string tp = "";
        private string accStatus = "";
        private string desID = "";
        private int loginID = 0;
        private int locationID = 0;

        private bool showdialogstatus;


        public Manager_Details_window()
        {
            InitializeComponent();
            rbnInsert.IsChecked = true;
        }

        public Manager_Details_window(bool dialogstatus)
        {
            InitializeComponent();
            showdialogstatus = dialogstatus;
        }

        public Manager_Details_window(char option)
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
        public Manager_Details_window(char option,string empID)
        {
            if (option.Equals("u"))
            {
                txtManagerID.Text = empID;
                rbnUpdate.IsChecked = true;
            }
            else if (option.Equals("s"))
            {
                txtManagerID.Text = empID;
                rbnSearch.IsChecked = true;
            }
        }

        //clear allowed text
        private void clearText()
        {
            txtManagerID.Text = "";
            cmbTitle.Text = "";
            txtFname.Text = "";
            txtLname.Text = "";
            txtTp.Text = "";
            txtLoginStatus.Text = "";
            cmbDes.Text = "";
            txtlocationID.Text = "";
        }


        //load Insert option
        private void setInsert()
        {
            txtManagerID.IsReadOnly = true;
            txtlocationID.IsReadOnly = true;
            btnSetLogin.IsEnabled = false;
            rbnUpdate.IsEnabled = false;
            txtAssignedDt.IsEnabled = false;
            btn_ok.IsEnabled = false;
            btnProcess.Content = "Insert";
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Update option
        private void setUpdate()
        {
            txtManagerID.IsReadOnly = true;
            txtlocationID.IsReadOnly = true;
            btnSetLogin.IsEnabled = true;
            rbnUpdate.IsEnabled = true;
            txtAssignedDt.IsEnabled = false;
            btn_ok.IsEnabled = true;
            btnProcess.Content = "Update";
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Search option
        private void setSearch()
        {
            txtManagerID.IsReadOnly = false;
            txtlocationID.IsReadOnly = false;
            btnSetLogin.IsEnabled = false;
            rbnUpdate.IsEnabled = false;
            txtAssignedDt.IsEnabled = true;
            btn_ok.IsEnabled = false;
            btnProcess.Content = "Search";
            setErrorImagesNull();
            enable_chk(true);
        }


        private void setErrorImagesNull()
        {
            managerIDNotify.Source = null;
            titleNotify.Source = null;
            fnameNotify.Source = null;
            lnameNotify.Source = null;
            tpNotify.Source = null;
            desNotify.Source = null;
        }

        private void enable_chk(bool value)
        {
            chkManagerID.IsEnabled = value;
            chkTitle.IsEnabled = value;
            chkFname.IsEnabled = value;
            chkLname.IsEnabled = value;
            chkTp.IsEnabled = value;
            chkAccStatus.IsEnabled = value;
            chkDes.IsEnabled = value;
            chkLocationID.IsEnabled = value;
            chkAssignedDt.IsEnabled = value;
        }

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
                    if (validate(false))
                    {

                        title = cmbTitle.Text.Trim();
                        fname = txtFname.Text.Trim();
                        lname = txtLname.Text.Trim();
                        tp = txtTp.Text.Trim();
                        locationID = Int32.Parse(txtlocationID.Text);
                        //accStatus = 
                        //loginID;

                        if (cmbDes.Text == "Showroom Manager")
                        {
                            desID = "S";
                        }
                        else if (cmbDes.Text == "Factory Manager")
                        {
                            desID = "F";
                        }
                        else if (cmbDes.Text == "Headquarters Manager")
                        {
                            desID = "H";
                        }
                        else if (cmbDes.Text == "Top Manager")
                        {
                            desID = "T";
                        }


                        Database db = new Database();
                        string query = "INSERT INTO Manager ( emp_title, emp_fname, emp_lname, emp_tp, assigned_dt, des_id, location_id) values ('" + title + "','" + fname + "','" + lname + "','" + tp + "',GETDATE(),'" + desID + "',"+locationID+ " ) DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as emp_id";
                        System.Data.DataTable dt = db.GetData(query);
                        managerID = Int32.Parse(dt.Rows[0]["emp_id"].ToString());

                        if (managerID > 0)
                        {
                            txtManagerID.Text = managerID.ToString();
                            MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            rbnUpdate.IsChecked = true;

                        }
                        else
                        {
                            MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }

                    }
                }
                else if (rbnUpdate.IsChecked == true)
                {
                    if (validate(true))
                    {
                        managerID = Int32.Parse(txtManagerID.Text);
                        title = cmbTitle.Text;
                        fname = txtFname.Text;
                        lname = txtLname.Text;
                        tp = txtTp.Text;

                        if (cmbDes.Text == "Showroom Manager")
                        {
                            desID = "S";
                        }
                        else if (cmbDes.Text == "Factory Manager")
                        {
                            desID = "F";
                        }
                        else if (cmbDes.Text == "Headquarters Manager")
                        {
                            desID = "H";
                        }
                        else if (cmbDes.Text == "Top Manager")
                        {
                            desID = "T";
                        }

                        locationID = Int32.Parse(txtlocationID.Text);

                        string query = " UPDATE Manager SET emp_title = '" + title + "' , emp_fname = '" + fname + "' , emp_lname  = '" + lname + "' , emp_tp = '" + tp + "' , des_id = '" + desID + "' , location_id = '" + locationID + "' WHERE emp_id = " + managerID + " ";
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

                    string query = "SELECT emp_id,emp_title,emp_fname,emp_lname,emp_tp,des_id,login_id,location_id,assigned_dt from Manager";
                    int x = 0;

                    void checkX()
                    {
                        if (x > 0)
                        {
                            query = query + " AND";
                        }
                    }

                    if (chkManagerID.IsChecked == true || chkTitle.IsChecked == true || chkFname.IsChecked==true || chkLname.IsChecked == true || chkTp.IsChecked==true || chkAccStatus.IsChecked==true || chkDes.IsChecked==true || chkLocationID.IsChecked==true || chkAssignedDt.IsChecked == true)
                    {
                        query = query + " WHERE";
                    }


                    if (chkManagerID.IsChecked == true && txtManagerID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " emp_id LIKE '%" + txtManagerID.Text + "%'";
                        x++;
                    }
                    if (chkTitle.IsChecked == true && cmbTitle.Text.Length > 0)
                    {
                        checkX();
                        query = query + " emp_title LIKE '%" + cmbTitle.Text + "%'";
                        x++;
                    }
                    if (chkFname.IsChecked == true && txtFname.Text.Length > 0)
                    {
                        checkX();
                        query = query + " emp_fname LIKE '%" + txtFname.Text + "%'";
                        x++;
                    }
                    if (chkTp.IsChecked == true && txtTp.Text.Length > 0)
                    {
                        checkX();
                        query = query + " emp_tp LIKE '%" + txtTp.Text + "%'";
                        x++;
                    }
                    if (chkLocationID.IsChecked == true && txtlocationID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " location_id LIKE '%" + txtlocationID.Text + "%'";
                        x++;
                    }
                    if (chkAssignedDt.IsChecked == true)
                    {
                        checkX();
                        query = query + " emp_tp LIKE '%" + txtTp.Text + "%'";
                        x++;
                    }
                    //accStatus = 
                    //loginID;

                    if (chkDes.IsChecked == true)
                    {
                        checkX();
                        if (cmbDes.Text.Equals("Showroom Manager"))
                        {
                            desID = "S";
                        }
                        else if (cmbDes.Text.Equals("Factory Manager"))
                        {
                            desID = "F";
                        }
                        else if (cmbDes.Text.Equals("Headquarters Manager"))
                        {
                            desID = "H";
                        }
                        else if (cmbDes.Text.Equals("Top Manager"))
                        {
                            desID = "T";
                        }

                        query = query + " des_id = '" + desID + "'";
                        x++;
                    }

                    Database db = new Database();
                    managerDatagrid.ItemsSource = db.GetData(query).AsDataView();

                }
                else
                {
                    MessageBox.Show("Please select an option", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool validate(bool value)
        {
            bool check = true;

            if (value == true)
            {
                //Manager ID
                if (CRMdbData.Manager.emp_id.validate(txtManagerID.Text))
                {
                    managerIDNotify.Source = managerIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
                else
                {
                    managerIDNotify.Source = managerIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                    check = false;
                }
            }

            //Title
            if (CRMdbData.Manager.emp_title.validate(cmbTitle.Text))
            {
                titleNotify.Source = titleNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                titleNotify.Source = titleNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //First Name
            if (CRMdbData.Manager.emp_fname.validate(txtFname.Text))
            {
                fnameNotify.Source = fnameNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                fnameNotify.Source = fnameNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Last Name
            if (CRMdbData.Manager.emp_lname.validate(txtLname.Text))
            {
                lnameNotify.Source = lnameNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                lnameNotify.Source = lnameNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Telephone
            if (CRMdbData.Manager.emp_tp.validate(txtTp.Text))
            {
                tpNotify.Source = tpNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                tpNotify.Source = tpNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Designation
            if (CRMdbData.Designation.desName.validate(cmbDes.Text))
            {
                desNotify.Source = desNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                desNotify.Source = desNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Location ID
            if (CRMdbData.Location.location_id.validate(txtlocationID.Text))
            {
                locationIDNotify.Source = locationIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                locationIDNotify.Source = locationIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            return check;
        }


        private void managerDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)managerDatagrid.SelectedItem;
                if (dv != null)
                {
                    managerID = Int32.Parse(dv.Row.ItemArray[0].ToString());//manager_id
                    txtManagerID.Text = managerID.ToString();
                    
                    btn_ok.IsEnabled = true;

                    string title1 = dv.Row.ItemArray[1].ToString();//emp_title

                    foreach (ComboBoxItem item in cmbTitle.Items)
                    {
                        if (item.Content.ToString() == title1)
                        {
                            cmbTitle.SelectedValue = item;
                            break;
                        }
                    }

                    txtFname.Text = dv.Row.ItemArray[2].ToString();//emp_fname
                    txtLname.Text = dv.Row.ItemArray[3].ToString();//emp_fname
                    txtTp.Text = dv.Row.ItemArray[4].ToString();//emp_tp

                    if (dv.Row.ItemArray[6].ToString().Length > 0)
                    {
                        loginID = Int32.Parse(dv.Row.ItemArray[6].ToString());//login_id

                        btnSetLogin.Content = "Update Login";
                    }

                    txtlocationID.Text = dv.Row.ItemArray[7].ToString();//location_id
                    txtAssignedDt.Text = dv.Row.ItemArray[8].ToString();//assigned_dt
                    string query = "Select login_dt,logout_dt from LoginDetails where login_id='" + loginID + "' ";
                    Database db = new Database();
                    loginDatagrid.ItemsSource = db.GetData(query).AsDataView();
                    
                    desID = dv.Row.ItemArray[5].ToString();//emp_title
                    string cmbDes1 = "";
                    if (desID.Equals("S"))
                    {
                        cmbDes1 = "Showroom Manager";
                    }
                    else if (desID.Equals("F"))
                    {
                        cmbDes1 = "Factory Manager";
                    }
                    else if (desID.Equals("H"))
                    {
                        cmbDes1 = "Headquarters Manager";
                    }
                    else if (desID.Equals("T"))
                    {
                        cmbDes1 = "Top Manager";
                    }
                    foreach (ComboBoxItem item in cmbDes.Items)
                    {
                        if (item.Content.ToString() == cmbDes1)
                        {
                            cmbDes.SelectedValue = item;
                            break;
                        }
                    }

                    rbnUpdate.IsChecked = true;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnSetLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (managerID > 0)
                {
                    var w = new Login_Details();
                    if (loginID > 0)
                    {
                        w = new Login_Details(true, managerID, loginID);
                    }
                    else
                    {
                        w = new Login_Details(true, managerID);
                    }

                    Login.b1.addCurrentWindow(this);
                    if (w.ShowDialog() == true)
                    {
                        loginID = Int32.Parse(w.txt_loginID.Text);
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        private void btnLocationAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (desID.Length > 0 && txtManagerID.Text.Length > 0)
                {

                    string query2 = "";
                    if (desID.Equals("S"))
                    {
                        cmbDes.Text = "Showroom Manager";
                        query2 = "insert into ShowroomManager_Showroom (emp_id,assigned_dt,location_id) values('" + managerID + "',DEFAULT,'" + txtlocationID.Text + "') ";
                    }
                    else if (desID.Equals("F"))
                    {
                        cmbDes.Text = "Factory Manager";
                        query2 = "insert into FactoryManager_Factory (emp_id,assigned_dt,location_id) values('" + managerID + "',DEFAULT,'" + txtlocationID.Text + "') ";
                    }
                    else if (desID.Equals("H"))
                    {
                        cmbDes.Text = "Headquarters Manager";
                        query2 = "insert into HQ_Top_Manager (emp_id,assigned_dt) values('" + managerID + "',DEFAULT) ";
                    }
                    else if (desID.Equals("T"))
                    {
                        cmbDes.Text = "Top Manager";
                        query2 = "insert into HQ_Top_Manager (emp_id,assigned_dt) values('" + managerID + "',DEFAULT) ";
                    }

                    Database db = new Database();

                    if (db.Save_Del_Update(query2) > 0)
                    {
                        MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        rbnUpdate.IsChecked = true;

                    }
                    else
                    {
                        MessageBox.Show("Data insertion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            
            
        }

        ~Manager_Details_window() { }

        private void btnLocationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Location(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txtlocationID.Text = w.txt_LocationID.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back_btn_Click_1(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
    }
}
