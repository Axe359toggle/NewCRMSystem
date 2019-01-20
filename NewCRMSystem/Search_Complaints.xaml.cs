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
    /// Interaction logic for Search_Complaints.xaml
    /// </summary>
    public partial class Search_Complaints : Window
    {
        public Search_Complaints()
        {
            InitializeComponent();
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            Login.b1.goBack(this);
        }

        ~Search_Complaints() { }

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT C.comp_id , C.comp_dt , C.ref_id , C.comp_type , CS.current_status , CS.description , C.recordedEmp_id , C.relatedLocation_id , C.recordedLocation_id , C.closed_dt FROM Complaint as C , ComplaintStatus as CS WHERE C.comp_status_id = CS.comp_status_id";
                int x = 1;
                
                void checkX()
                {
                    if (x > 0)
                    {
                        query = query + " AND";
                    }
                }

                if (chk_compID.IsChecked == true || chk_compDate.IsChecked == true || chk_refID.IsChecked == true || chk_compType.IsChecked == true || chk_recEmpID.IsChecked == true || chk_relLocID.IsChecked == true || chk_recLocID.IsChecked == true || chk_closedDate.IsChecked == true)
                {
                    //Complaint ID
                    if (chk_compID.IsChecked == true && txt_compID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.comp_id LIKE '%" + txt_compID.Text + "%'";
                        x++;
                    }

                    //Complaint Date
                    if (chk_compDate.IsChecked == true && dt_compDate.SelectedDate.Value.Date.ToShortDateString().Length > 0)
                    {
                        checkX();
                        query = query + " C.comp_dt LIKE '%" + dt_compDate.SelectedDate.Value.Date.ToShortDateString() + "%'";
                        x++;
                    }

                    //Reference ID
                    if (chk_refID.IsChecked == true && txt_refID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.ref_id LIKE '%" + txt_refID.Text + "%'";
                        x++;
                    }

                    //Complaint Type
                    if (chk_compType.IsChecked == true && cmb_compType.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.comp_type LIKE '%" + cmb_compType.Text + "%'";
                        x++;
                    }

                    //Recorded Employee ID
                    if (chk_recEmpID.IsChecked == true && txt_recEmpID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.recordedEmp_id LIKE '%" + txt_recEmpID.Text + "%'";
                        x++;
                    }

                    //Related Location ID
                    if (chk_relLocID.IsChecked == true && txt_relLocID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.relatedLocation_id LIKE '%" + txt_relLocID.Text + "%'";
                        x++;
                    }

                    //Recorded Location ID
                    if (chk_recLocID.IsChecked == true && txt_recLocID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.recordedLocation_id LIKE '%" + txt_recLocID.Text + "%'";
                        x++;
                    }

                    //Closed Date
                    if (chk_closedDate.IsChecked == true && dt_closedDate.Text.Length > 0)
                    {
                        checkX();
                        query = query + " C.closed_dt LIKE '%" + dt_closedDate.Text + "%'";
                        x++;
                    }
                }
                else
                {
                    query = query + " AND ( C.comp_id LIKE '%" + txt_search.Text + "%' OR C.comp_dt LIKE '%" + txt_search.Text + "%' OR C.ref_id LIKE '%" + txt_search.Text + "%' OR C.comp_type LIKE '%" + txt_search.Text + "%' OR C.recordedEmp_id LIKE '%" + txt_search.Text + "%' OR C.relatedLocation_id LIKE '%" + txt_search.Text + "%' OR C.recordedLocation_id LIKE '%" + txt_search.Text + "%' OR C.closed_dt LIKE '%" + txt_search.Text + "%' OR CS.current_status LIKE '%" + txt_search.Text + "%' OR CS.description LIKE '%" + txt_search.Text + "%' )  ";
                }
                




                Database db = new Database();
                complaint_datagrid.ItemsSource = db.GetData(query).AsDataView();
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

        private void Btn_recEmpDetails_Click(object sender, RoutedEventArgs e)
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

        private void Btn_relLocDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                View_Location_Details w = new View_Location_Details(Int32.Parse(txt_relLocID.Text));
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

        private void Btn_recLocDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                View_Location_Details w = new View_Location_Details(Int32.Parse(txt_recLocID.Text));
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

        private void Complaint_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)complaint_datagrid.SelectedItem;
                if (dv != null)
                {
                    txt_compID.Text = dv.Row.ItemArray[0].ToString();//comp_id
                    dt_compDate.Text = dv.Row.ItemArray[1].ToString();//comp_dt
                    txt_refID.Text = dv.Row.ItemArray[2].ToString();//ref_id 
                    string compType = dv.Row.ItemArray[3].ToString();//comp_type
                    
                    foreach (ComboBoxItem item in cmb_compType.Items)
                    {
                        if (item.Content.ToString() == compType)
                        {
                            cmb_compType.SelectedValue = item;
                            break;
                        }
                    }

                    txt_compStatus.Text = dv.Row.ItemArray[4].ToString();//current_status
                    txt_compStatusDes.Text = dv.Row.ItemArray[5].ToString();//description

                    txt_recEmpID.Text = dv.Row.ItemArray[6].ToString();//recordedEmp_id
                    txt_relLocID.Text = dv.Row.ItemArray[7].ToString();//relatedLocation_id
                    txt_recLocID.Text = dv.Row.ItemArray[8].ToString();//recordedLocation_id
                    dt_closedDate.Text = dv.Row.ItemArray[9].ToString();//closed_dt
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
    }
}
