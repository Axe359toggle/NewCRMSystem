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
    /// Interaction logic for View_Location_Details.xaml
    /// </summary>
    public partial class View_Location_Details : Window
    {
        public View_Location_Details()
        {
            InitializeComponent();
        }

        public View_Location_Details(int locID1)
        {
            try
            {
                InitializeComponent();
                loadData(locID1);
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

        private void loadData(int locID1)
        {
            string query = "SELECT location_type ,location_name ,addr_no ,addr_lane ,addr_town ,addr_city from Location WHERE location_id ='" + locID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_LocationType.Text = dt.Rows[0]["location_type"].ToString();
            txt_LocationName.Text = dt.Rows[0]["location_name"].ToString();
            txt_AddrNo.Text = dt.Rows[0]["addr_no"].ToString();
            txt_AddrLane.Text = dt.Rows[0]["addr_lane"].ToString();
            txt_AddrTown.Text = dt.Rows[0]["addr_town"].ToString();
            txt_AddrCity.Text = dt.Rows[0]["addr_city"].ToString();

            query = "Select location_tp from Location_tp where location_id = '" + locID1 + "' ";
            telephone_Datagrid.ItemsSource = db.GetData(query).AsDataView();

        }

        ~View_Location_Details() { }
    }
}
