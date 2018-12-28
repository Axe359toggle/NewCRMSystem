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
    /// Interaction logic for Assign_Reabate_Window.xaml
    /// </summary>
    public partial class Assign_Reabate_Window : Window
    {
        private int compID = 0;
        private string itemImageSource = "";
        private string rebatePercentage = "";
        
        public Assign_Reabate_Window()
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

        public Assign_Reabate_Window(int compID1)
        {
            try
            {
                InitializeComponent();
                bindCompIDList();
                foreach (ComboBoxItem item in cmb_compID.Items)
                {
                    if (item.Content.ToString() == compID1.ToString())
                    {
                        cmb_compID.SelectedValue = item;
                        break;
                    }
                }
                loadData(compID1.ToString());
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

        private void bindCompIDList()
        {
                string query = "SELECT comp_id FROM ComplaintItem WHERE NOT EXISTS (SELECT comp_item_id FROM Rebate WHERE  Rebate.comp_item_id = ComplaintItem.comp_item_id)";
                Database db = new Database();
                System.Data.DataTable dt = db.GetData(query);

                cmb_compID.Items.Clear();

                foreach (System.Data.DataRow dr in dt.Rows)
                    cmb_compID.Items.Add(dr["comp_id"].ToString());

                cmb_compID.SelectedIndex = 0;
        }

        private double getRebatePercentage()
        {
            double percentage = 0;
                if (cmb_rebatePercentage.Text.Equals("25%"))
                {
                    percentage = 0.25;
                }
                else if (cmb_rebatePercentage.Text.Equals("50%"))
                {
                    percentage = 0.50;
                }
                else if (cmb_rebatePercentage.Text.Equals("75%"))
                {
                    percentage = 0.75;
                }
                else if (cmb_rebatePercentage.Text.Equals("100%"))
                {
                    percentage = 1.00;
                }
            return percentage;
        }

        private void setRebateAmountTxt(double itemPrice1)
        {
            double percentage = getRebatePercentage();
            
            txt_rebateAmount.Text = (itemPrice1 * percentage).ToString();
        }
        
        System.Data.DataSet ds;
        string strName, imageName;

        private void browseImageToDB()
        {
            Microsoft.Win32.FileDialog fldlg = new Microsoft.Win32.OpenFileDialog();
            fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
            fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            fldlg.ShowDialog();
            {
                strName = fldlg.SafeFileName;
                imageName = fldlg.FileName;
                ImageSourceConverter isc = new ImageSourceConverter();
                img_itemImage.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
            }
            fldlg = null;
        }

        private bool saveImageToDB(int itemID1)
        {
            bool value = false;
            if (imageName != "")
            {
                //Initialize a file stream to read the image file
                System.IO.FileStream fs = new System.IO.FileStream(imageName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                //Initialize a byte array with size of stream
                byte[] imgByteArr = new byte[fs.Length];

                //Read data from the file stream and put into the byte array
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                //Close a file stream
                fs.Close();
                string sql = "UPDATE Item SET item_pic = @img WHERE item_id = " + itemID1 + " ";

                Database db = new Database();
                if (db.Save_Del_Update(sql , imgByteArr) == 1)
                {
                    value = true;
                }
            }

            return value;
        }

        private void loadDefectImageFromDB(byte[] blob)
        {
            //Store binary data read from the database in a byte array
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            stream.Write(blob, 0, blob.Length);
            stream.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            img_defectImage.Source = bi;
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

        private void loadDefectImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_defectImage.Source = imageSource;
        }

        private void loadData(string compID1)
        {
            string query = "SELECT CI.item_type_id , CI.item_id , CI.item_defect , CI.item_defect_img , CI.item_remarks , CC.cus_id , I.item_price , IT.item_brand , IT.item_category , IT.item_name , IT.item_size from ComplaintItem as CI , CustomerComplaint as CC , Item as I , ItemType as IT where CI.comp_id = '" + compID1 + "' and CC.comp_id = CI.comp_id and I.item_id = CI.item_id and CI.item_type_id = IT.item_type_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
            txt_itemID.Text = dt.Rows[0]["item_id"].ToString();
            txt_itemDefect.Text = dt.Rows[0]["item_defect"].ToString();
            txt_itemRemarks.Text = dt.Rows[0]["item_remarks"].ToString();
            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
            txt_itemPrice.Text = dt.Rows[0]["item_price"].ToString();

            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();

            string imagePath = dt.Rows[0]["item_defect_img"].ToString();
            loadDefectImageFromLocal(imagePath);
        }

        private bool validateItemImageSource()
        {
            bool check = true;

            //Item Image
            if (Validation.validate(itemImage_Notify, CRMdbData.Item.item_pic.validate(itemImageSource), CRMdbData.Item.item_pic.Error)) { }
            else { check = false; }

            return check;
        }

        private bool validate()
        {
            bool check = true;

            //Complaint ID
            if (Validation.validate(compID_Notify, CRMdbData.Complaint.comp_id.validate(cmb_compID.Text), CRMdbData.Complaint.comp_id.Error)) { }
            else { check = false; }

            //Rebate Percentage
            if (Validation.validate(rebatePercentage_Notify, CRMdbData.Rebate.rebate_percentage.validate(cmb_rebatePercentage.Text), CRMdbData.Rebate.rebate_percentage.Error)) { }
            else { check = false; }

            //Item Image
            if (validateItemImageSource()) { }
            else
            { check = false; }

            //HQ Manager ID
            if (CRMdbData.Manager.emp_id.validate(Login.EmpID.ToString())) { }
            else
            {
                MessageBox.Show("Logged Employee ID Error ", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                check = false;
            }


            return check;
        }

        ~Assign_Reabate_Window() { }



        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login.b1.goBack(this);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
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

        private void cmb_rebatePercentage_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmb_rebatePercentage.Text.Length > 0)
                {
                    setRebateAmountTxt(Double.Parse(txt_itemPrice.Text));
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
                    loadData(cmb_compID.Text);
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

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    compID = Int32.Parse(cmb_compID.Text);
                    rebatePercentage = cmb_rebatePercentage.Text;
                    string itemID = txt_itemID.Text;
                    string itemImageSource = saveItemImageToLocal(txt_itemID.Text);
                    int HQManagerID = Login.EmpID;

                    if (CRMdbData.Item.item_pic.validate(itemImageSource))
                    {
                        string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "')  INSERT INTO Rebate (comp_item_id ,hQManager ,rebate_percentage ) VALUES (@COMPitemID," + HQManagerID + ",'" + rebatePercentage + "') ";
                        query += "Update Item SET item_pic = '" + itemImageSource + "' WHERE item_id = '" + itemID + "' ";
                        query += "UPDATE Complaint SET comp_status_id = 3 WHERE comp_id = " + compID + " ";

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
                    else
                    {
                        MessageBox.Show("Image Source Length Error");
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
    }
}
