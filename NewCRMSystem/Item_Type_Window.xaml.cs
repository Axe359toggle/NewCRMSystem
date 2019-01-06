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
    /// Interaction logic for Item_Type_Window.xaml
    /// </summary>
    public partial class Item_Type_Window : Window
    {
        private int itemTypeID = 0;
        private string brand = "";
        private string category = "";
        private string name = "";
        private string size = "";
        private string itemImageSource = "";

        private bool showdialogstatus;

        public Item_Type_Window()
        {
            InitializeComponent();
        }

        public Item_Type_Window(bool dialogstatus)
        {
            InitializeComponent();
            showdialogstatus = dialogstatus;
            grid_panel.Visibility = Visibility.Collapsed;
            rbnSearch.IsChecked = true;
        }

        public Item_Type_Window(char option)
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

        public Item_Type_Window(char option, string itemTypeID1)
        {
            InitializeComponent();
            if (option.Equals("u"))
            {
                txt_itemTypeID.Text = itemTypeID1;
                rbnUpdate.IsChecked = true;
            }
            else if (option.Equals("s"))
            {
                txt_itemTypeID.Text = itemTypeID1;
                rbnSearch.IsChecked = true;
            }
        }

        private void clearText()
        {
            txt_itemTypeID.Text = "";
            txt_brand.Text = "";
            txt_category.Text = "";
            txt_name.Text = "";
            txt_size.Text = "";
        }

        private void setErrorImagesNull()
        {
            itemTypeID_Notify.Source = null;
            brand_Notify.Source = null;
            category_Notify.Source = null;
            name_Notify.Source = null;
            size_Notify.Source = null;
            itemImage_Notify.Source = null;
        }

        private void hide_chk(Visibility visibility)
        {
            chk_itemTypeID.Visibility = visibility;
            chk_brand.Visibility = visibility;
            chk_category.Visibility = visibility;
            chk_name.Visibility = visibility;
            chk_size.Visibility = visibility;
        }

        private void enable_chk(bool value)
        {
            chk_itemTypeID.IsEnabled = value;
            chk_brand.IsEnabled = value;
            chk_category.IsEnabled = value;
            chk_name.IsEnabled = value;
            chk_size.IsEnabled = value;

            if (value)
            {
                hide_chk(Visibility.Visible);
            }
            else
            {
                hide_chk(Visibility.Hidden);
            }
        }

        //load Insert option
        private void setInsert()
        {
            btnProcess.Content = "Insert";
            rbnUpdate.IsEnabled = false;
            txt_itemTypeID.IsReadOnly = true;
            txt_itemTypeID.IsEnabled = false;
            btn_ok.IsEnabled = false;
            btn_itemImageUpload.IsEnabled = true;
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Update option
        private void setUpdate()
        {

            btnProcess.Content = "Update";
            rbnUpdate.IsEnabled = true;
            txt_itemTypeID.IsReadOnly = true;
            txt_itemTypeID.IsEnabled = true;
            btn_ok.IsEnabled = true;
            btn_itemImageUpload.IsEnabled = true;
            setErrorImagesNull();
            enable_chk(false);
        }

        //load Search option
        private void setSearch()
        {
            btnProcess.Content = "Search";
            rbnUpdate.IsEnabled = false;
            txt_itemTypeID.IsReadOnly = false;
            txt_itemTypeID.IsEnabled = true;
            btn_ok.IsEnabled = false;
            btn_itemImageUpload.IsEnabled = false;
            setErrorImagesNull();
            enable_chk(true);
        }

        private bool validate(bool value)
        {
            bool check = true;

            if (value == true)
            {
                //Item Type ID
                if (CRMdbData.ItemType.item_type_id.validate(txt_itemTypeID.Text))
                {
                    itemTypeID_Notify.Source = itemTypeID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
                }
                else
                {
                    itemTypeID_Notify.Source = itemTypeID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                    itemTypeID_Notify.ToolTip = CRMdbData.ItemType.item_type_id.Error;
                    check = false;
                }
            }

            //Brand
            if (CRMdbData.ItemType.item_brand.validate(txt_brand.Text))
            {
                brand_Notify.Source = brand_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                brand_Notify.Source = brand_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                brand_Notify.ToolTip = CRMdbData.ItemType.item_brand.Error;
                check = false;
            }

            //Category
            if (CRMdbData.ItemType.item_category.validate(txt_category.Text))
            {
                category_Notify.Source = category_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                category_Notify.Source = category_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                category_Notify.ToolTip = CRMdbData.ItemType.item_category.Error;
                check = false;
            }

            //Name
            if (CRMdbData.ItemType.item_name.validate(txt_name.Text))
            {
                name_Notify.Source = name_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                name_Notify.Source = name_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                name_Notify.ToolTip = CRMdbData.ItemType.item_name.Error;
                check = false;
            }

            //Size
            if (CRMdbData.ItemType.item_size.validate(txt_size.Text))
            {
                size_Notify.Source = size_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                size_Notify.Source = size_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                size_Notify.ToolTip = CRMdbData.ItemType.item_size.Error;
                check = false;
            }

            //Item Image
            if (Validation.validate(itemImage_Notify, CRMdbData.ItemType.item_pic.validate(itemImageSource), CRMdbData.ItemType.item_pic.Error)) { }
            else { check = false; }

            return check;
        }

        //Save to Local required varibles
        string filepath = "";
        string ext = "";

        private void browseItemImageToLocal()
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Multiselect = false;
            open.DefaultExt = ".png";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
            //open.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            bool? result = open.ShowDialog();


            if (result == true)
            {
                filepath = open.FileName; // Stores Original Path
                itemImageSource = filepath;
                ext = System.IO.Path.GetExtension(open.FileName);
                ImageSource imgsource = new BitmapImage(new Uri(filepath)); // Just show The File In Image when we browse It
                img_itemImage.Source = imgsource;
            }


        }

        private string saveItemImageToLocal(string itemID1)
        {
            string imageSource = "";

            if (filepath.Length > 0)
            {
                var imageFile = new System.IO.FileInfo(filepath);
                if (imageFile.Exists)// check image file exist
                {
                    // get your application folder
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    // get your 'Uploaded' folder
                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "Item Images"));
                    if (!dir.Exists)
                        dir.Create();

                    System.IO.FileInfo destFile = new System.IO.FileInfo(System.IO.Path.Combine(dir.FullName, itemID1 + ext));
                    if (destFile.Exists)
                    {
                        //delete
                        System.IO.File.Delete(System.IO.Path.Combine(dir.FullName, itemID1 + ext));
                    }
                    // Copy file to your folder
                    string imageName = imageFile.CopyTo(System.IO.Path.Combine(dir.FullName, itemID1 + ext)).ToString();

                    imageSource = dir.FullName + @"\" + imageName;
                }
            }

            return imageSource;
        }

        private void loadItemImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_itemImage.Source = imageSource;
        }

        ~Item_Type_Window() { }

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
                    if (validate(false)) //validates without Comp Item ID
                    {

                        brand = txt_brand.Text;
                        category = txt_category.Text;
                        name = txt_name.Text;
                        size = txt_size.Text.Trim(); ;
                        
                        string query = "INSERT INTO ItemType (item_brand ,item_category ,item_name ,item_size ) VALUES ('" + brand + "','" + category + "','" + name + "','" + size + "' ) DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as item_type_id";
                        Database db = new Database();
                        itemTypeID = Int32.Parse(db.GetData(query).Rows[0]["item_type_id"].ToString());
                        

                        if (itemTypeID > 0)
                        {
                            itemImageSource = saveItemImageToLocal(itemTypeID.ToString());
                            string query1 = "Update ItemType SET item_pic = '" + itemImageSource + "' WHERE item_type_id = '" + itemTypeID + "' ";
                            if (db.Save_Del_Update(query1) > 0)
                            {
                                GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                                txt_itemTypeID.Text = itemTypeID.ToString();
                                rbnUpdate.IsChecked = true;
                            }
                            else
                            {
                                GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Failed();
                            }
                            
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
                        itemTypeID = Int32.Parse(txt_itemTypeID.Text);
                        brand = txt_brand.Text;
                        category = txt_category.Text;
                        name = txt_name.Text;
                        size = txt_size.Text;
                        itemImageSource = saveItemImageToLocal(txt_itemTypeID.Text);

                        string query = " UPDATE ItemType SET item_brand = '" + brand + "' , item_category = '" + category + "' , item_name = '" + name + "' , item_size = '" + size + "' , item_pic = '" + itemImageSource + "' WHERE item_type_id = " + itemTypeID + " ";
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

                    string query = "SELECT item_type_id ,item_brand ,item_category ,item_name ,item_size , item_pic from ItemType";
                    int x = 0;

                    void checkX()
                    {
                        if (x > 0)
                        {
                            query = query + " AND";
                        }
                    }

                    if (chk_itemTypeID.IsChecked == true || chk_brand.IsChecked == true || chk_category.IsChecked == true || chk_name.IsChecked == true || chk_size.IsChecked == true)
                    {
                        query = query + " WHERE";
                    }

                    //Item Type
                    if (chk_itemTypeID.IsChecked == true && txt_itemTypeID.Text.Length > 0)
                    {
                        checkX();
                        query = query + " item_type_id LIKE '%" + txt_itemTypeID.Text + "%'";
                        x++;
                    }

                    //Brand
                    if (chk_brand.IsChecked == true && txt_brand.Text.Length > 0)
                    {
                        checkX();
                        query = query + " item_brand LIKE '%" + txt_brand.Text + "%'";
                        x++;
                    }

                    //Category
                    if (chk_category.IsChecked == true && txt_category.Text.Length > 0)
                    {
                        checkX();
                        query = query + " item_category LIKE '%" + txt_category.Text + "%'";
                        x++;
                    }

                    //Item Name
                    if (chk_name.IsChecked == true && txt_name.Text.Length > 0)
                    {
                        checkX();
                        query = query + " item_name LIKE '%" + txt_name.Text + "%'";
                        x++;
                    }

                    //Size
                    if (chk_size.IsChecked == true && txt_size.Text.Length > 0)
                    {
                        checkX();
                        query = query + " item_size LIKE '%" + txt_size.Text + "%'";
                        x++;
                    }

                    Database db = new Database();
                    itemType_Datagrid.ItemsSource = db.GetData(query).AsDataView();

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

        private void itemType_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dv = (DataRowView)itemType_Datagrid.SelectedItem;
                if (dv != null)
                {
                    itemTypeID = Int32.Parse(dv.Row.ItemArray[0].ToString());//Item Type ID
                    txt_itemTypeID.Text = itemTypeID.ToString();


                    btn_ok.IsEnabled = true;
                    
                    txt_brand.Text = dv.Row.ItemArray[1].ToString();//Brand
                    txt_category.Text = dv.Row.ItemArray[2].ToString();//Category
                    txt_name.Text = dv.Row.ItemArray[3].ToString();//Name
                    txt_size.Text = dv.Row.ItemArray[4].ToString();//Size

                    string imagePath = dv.Row.ItemArray[5].ToString().ToString();//Item Pic
                    if (imagePath.Length > 0)
                    {
                        loadItemImageFromLocal(imagePath);
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

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (showdialogstatus == true)
                {
                    DialogResult = true;
                    this.Close();
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

        private void btn_itemImageUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                browseItemImageToLocal();
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
