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
    /// Interaction logic for View_Item_Type_Details.xaml
    /// </summary>
    public partial class View_Item_Type_Details : Window
    {
        public View_Item_Type_Details()
        {
            InitializeComponent();
        }

        public View_Item_Type_Details(int itemTypeID1)
        {
            try
            {
                InitializeComponent();
                loadData(itemTypeID1);
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

        private void loadItemImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_itemImage.Source = imageSource;
        }

        private void loadData(int itemTypeID1)
        {
            string query = "SELECT item_brand , item_category , item_name , item_size , item_pic from Customer WHERE item_type_id = '" + itemTypeID1 + "' ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = itemTypeID1.ToString();
            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();
            string imagePath = dt.Rows[0]["item_pic"].ToString();
            if (imagePath.Length > 0)
            {
                loadItemImageFromLocal(imagePath);
            }
            else
            {
                img_itemImage.Source = null;
            }
        }

        ~View_Item_Type_Details() { }
    }
}
