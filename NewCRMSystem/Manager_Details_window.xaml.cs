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

using System.Data.SqlClient;
using System.Data;

namespace NewCRMSystem
{
    /// <summary>
    /// Interaction logic for Manager_Details_window.xaml
    /// </summary>
    public partial class Manager_Details_window : Window
    {
        private string managerID;
        private string title;
        private string fname;
        private string lname;
        private string tp;
        private string accStatus;
        private string desID;
        private string loginID;
        private string locationID;


        public Manager_Details_window()
        {
            InitializeComponent();
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
            txtFullName.Text = "";
            txtTp.Text = "";
            txtLoginStatus.Text = "";
            cmbDes.Text = "";
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

        //load Insert option
        private void setInsert()
        {
            setManagerID();
            txtManagerID.IsReadOnly = true;
            txtlocationID.IsReadOnly = true;
            btnProcess.Content = "Insert";
            rbnUpdate.IsEnabled = false;
            btnSetLogin.IsEnabled = false;
            setErrorImagesNull();
        }

        //load Update option
        private void setUpdate()
        {
            txtManagerID.IsReadOnly = true;
            txtlocationID.IsReadOnly = true;
            btnSetLogin.IsEnabled = true;
            txtlocationID.Text = "";
            rbnUpdate.IsEnabled = true;
            btnProcess.Content = "Update";
            setErrorImagesNull();
        }

        //load Search option
        private void setSearch()
        {
            txtManagerID.IsReadOnly = false;
            txtlocationID.IsReadOnly = false;
            rbnUpdate.IsEnabled = false;
            btnSetLogin.IsEnabled = false;
            btnProcess.Content = "Search";
            setErrorImagesNull();
        }
        

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbnInsert.IsChecked == true)
                {
                    if (validate())
                    {

                        title = cmbTitle.Text.Trim();
                        fname = txtFname.Text.Trim();
                        lname = txtLname.Text.Trim();
                        tp = txtTp.Text.Trim();
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
                        string query = "INSERT INTO Manager ( emp_title, emp_fname, emp_lname, emp_tp, des_id, location_id) values ('" + title + "','" + fname + "','" + lname + "','" + tp + "','" + desID + "','"+locationID+"')";
                        int rows = db.Save_Del_Update(query);

                        if (rows > 0)
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
                else if (rbnUpdate.IsChecked == true)
                {
                    if (validate())
                    {




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

                    if (chkManagerID.IsChecked == true || chkTitle.IsChecked == true || chkFullName.IsChecked==true || chkTp.IsChecked==true || chkAccStatus.IsChecked==true || chkDes.IsChecked==true || chkLocation.IsChecked==true)
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
                    if (chkFullName.IsChecked == true && txtFullName.Text.Length > 0)
                    {
                        checkX();
                        query = query + " emp_fullname LIKE '%" + txtFullName.Text + "%'";
                        x++;
                    }
                    if (chkTp.IsChecked == true && txtTp.Text.Length > 0)
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
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool validate()
        {
            bool check = true;

            if (!(txtManagerID.Text.Trim().Length >= CRMdbData.Manager.emp_id.size || txtManagerID.Text.Trim().Length == 0))
            {
                check = false;
                managerIDNotify.Source = managerIDNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                managerIDNotify.Source = managerIDNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (cmbTitle.Text.Trim().Length > CRMdbData.Manager.emp_title.size || cmbTitle.Text.Trim().Length == 0)
            {
                check = false;
                titleNotify.Source = titleNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                titleNotify.Source = titleNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (txtFullName.Text.Trim().Length > CRMdbData.Manager.emp_fullname.size || txtFullName.Text.Trim().Length == 0)
            {
                check = false;
                fullNameNotify.Source = fullNameNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                fullNameNotify.Source = fullNameNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (txtTp.Text.Trim().Length > CRMdbData.Manager.emp_tp.size || txtTp.Text.Trim().Length == 0)
            {
                check = false;
                tpNotify.Source = tpNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                tpNotify.Source = tpNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            if (cmbDes.Text.Trim().Length > CRMdbData.Designation.desName.size || cmbDes.Text.Trim().Length == 0)
            {
                check = false;
                desNotify.Source = desNotify.TryFindResource("notifyErrorImage") as BitmapImage;
            }
            else
            {
                desNotify.Source = desNotify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }

            return check;
        }

        private void setErrorImagesNull()
        {
            managerIDNotify.Source = null;
            titleNotify.Source = null;
            fullNameNotify.Source = null;
            tpNotify.Source = null;
            desNotify.Source = null;
        }

        private void managerDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)managerDatagrid.SelectedItem;
                if (dv != null)
                {
                    txtManagerID.Text = managerID = dv.Row.ItemArray[0].ToString();//manager_id
                    cmbTitle.Text = dv.Row.ItemArray[1].ToString();//emp_title
                    txtFullName.Text = dv.Row.ItemArray[2].ToString();//emp_fullname
                    txtTp.Text = dv.Row.ItemArray[3].ToString();//emp_tp

                    loginID = dv.Row.ItemArray[6].ToString();//login_id
                    string query = "Select login_dt,logout_dt from LoginDetails where login_id='" + loginID + "' ";
                    Database db = new Database();
                    loginDatagrid.ItemsSource = db.GetData(query).AsDataView();
                    
                    desID = dv.Row.ItemArray[5].ToString();//emp_title
                    if (desID.Equals("S"))
                    {
                        cmbDes.Text = "Showroom Manager";
                    }
                    else if (desID.Equals("F"))
                    {
                        cmbDes.Text = "Factory Manager";
                    }
                    else if (desID.Equals("H"))
                    {
                        cmbDes.Text = "Headquarters Manager";
                    }
                    else if (desID.Equals("T"))
                    {
                        cmbDes.Text = "Top Manager";
                    }


                    rbnUpdate.IsChecked = true;
                }
            }
            catch (SqlException ex)
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
            if (txtManagerID.Text.Trim().Length>0)
            {
                Login_Details ld;
                if(loginID.Trim().Length > 0)
                {
                    ld = new Login_Details(txtManagerID.Text.Trim(), loginID.Trim());
                    ld.Show();
                }
                else if(loginID.Trim().Length == 0)
                {
                    ld = new Login_Details(txtManagerID.Text.Trim());
                    ld.Show();
                }
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
                if (desID.Length > 0 && managerID.Length > 0)
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
                    int rows = db.Save_Del_Update(query2);
                    if (rows > 0)
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
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            
            
        }
    }
}
