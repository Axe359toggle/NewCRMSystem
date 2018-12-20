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
    /// Interaction logic for Staff_Complaint.xaml
    /// </summary>
    public partial class Staff_Complaint : Window
    {
        public Staff_Complaint()
        {
            InitializeComponent();
        }

        private int compID;
        private string staffID = "";
        private string staffName = "";
        private string description = "";

        public Staff_Complaint(int compID1)
        {
            InitializeComponent();
            txt_compID.Text = compID1.ToString();
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (CRMdbData.Complaint.comp_id.validate(txt_compID.Text))
            {
                compID_Notify.Source = compID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                compID_Notify.Source = compID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                compID_Notify.ToolTip = CRMdbData.Complaint.comp_id.Error;
                check = false;
            }

            //Staff ID
            if (CRMdbData.StaffComplaint.staff_id.validate(txt_staffID.Text))
            {
                staffID_Notify.Source = staffID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                staffID_Notify.Source = staffID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                staffID_Notify.ToolTip = CRMdbData.StaffComplaint.staff_id.Error;
                check = false;
            }

            //Staff Name
            if (CRMdbData.StaffComplaint.staff_name.validate(txt_staffName.Text))
            {
                staffName_Notify.Source = staffName_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                staffName_Notify.Source = staffName_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                staffName_Notify.ToolTip = CRMdbData.StaffComplaint.staff_name.Error;
                check = false;
            }

            //Description
            if (CRMdbData.StaffComplaint.description.validate(txt_description.Text))
            {
                description_Notify.Source = description_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                description_Notify.Source = description_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                description_Notify.ToolTip = CRMdbData.StaffComplaint.description.Error;
                check = false;
            }

            return check;
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }
        
        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    
                    
                    Database db = new Database();

                    compID = Int32.Parse(txt_compID.Text);
                    staffID = txt_staffID.Text;
                    staffName = txt_staffName.Text;
                    description = txt_description.Text;

                    string query = "INSERT INTO StaffComplaint (comp_id ,staff_id ,staff_name ,description ) VALUES ("+compID+" , '"+staffID+"' , '"+staffName+"' , '"+description+"') ";
                    
                    if (db.Save_Del_Update(query) > 0)
                    {
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
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
