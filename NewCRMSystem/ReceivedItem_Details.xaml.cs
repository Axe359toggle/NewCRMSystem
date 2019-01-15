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
    /// Interaction logic for ReceivedItem_Details.xaml
    /// </summary>
    public partial class ReceivedItem_Details : Window
    {
        private int compID = 0;
        private DateTime receivedDt;
        private double itemPrice = 0;
        private string shoeSide = "";
        private int itemTypeID = 0;
        private string itemID = "";
        private string itemDefect = "";
        private string itemRemarks = "";
        private string defectImageSource = "";
        
        public ReceivedItem_Details()
        {
            InitializeComponent();
        }

        public ReceivedItem_Details(int compID1)
        {
            try
            {
                InitializeComponent();
                txt_compID.Text = compID1.ToString();
                string query = "SELECT ref_id FROM Complaint WHERE comp_id = '" + compID1 + "' ";
                Database db = new Database();
                txt_refID.Text = db.GetData(query).Rows[0]["ref_id"].ToString();
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

        //Save to Local required varibles
        string filepath = "";
        string ext = "";

        private void browseImageToLocal()
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Multiselect = false;
            open.DefaultExt = ".png";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
            //open.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            bool? result = open.ShowDialog();


            if (result == true)
            {
                filepath = open.FileName; // Stores Original Path in Textbox
                defectImageSource = filepath;
                ext = System.IO.Path.GetExtension(open.FileName);
                ImageSource imgsource = new BitmapImage(new Uri(filepath)); // Just show The File In Image when we browse It
                img_defect.Source = imgsource;
            }

            
        }

        private string saveImageToLocal(int compItemID1)
        {
            string imageSource = "";

            if(filepath.Length>0)
            {
                var imageFile = new System.IO.FileInfo(filepath);
                if (imageFile.Exists)// check image file exist
                {
                    // get your application folder
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    // get your 'Uploaded' folder
                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "Defect Images"));
                    if (!dir.Exists)
                        dir.Create();

                    System.IO.FileInfo destFile = new System.IO.FileInfo(System.IO.Path.Combine(dir.FullName, compItemID1 + ext));
                    if (destFile.Exists)
                    {
                        //delete
                        System.IO.File.Delete(System.IO.Path.Combine(dir.FullName, compItemID1 + ext));
                    }
                    // Copy file to your folder
                    string imageName = imageFile.CopyTo(System.IO.Path.Combine(dir.FullName, compItemID1 + ext)).ToString();

                    imageSource = dir.FullName + @"\" + imageName;
                }
            }

            return imageSource;
        }

        //Save to DB required varibles
        System.Data.DataSet ds;
        string strName = "", imageName = "";

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
                img_defect.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
            }
            fldlg = null;
        }

        private bool saveImageToDB(int compItemID1)
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
                string sql = "UPDATE ComplaintItem SET item_defect_img = @img WHERE comp_item_id = '" + compItemID1 + "' ";
                
                Database db = new Database();
                //Pass byte array into database

                if (db.Save_Del_Update(sql,imgByteArr) == 1)
                {
                    value = true;
                }
            }

            return value;
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
            
            //Received Date
            if (CRMdbData.ComplaintItem.received_dt.validate(dt_receivedDt.Text))
            {
                receivedDt_Notify.Source = receivedDt_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                receivedDt_Notify.Source = receivedDt_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                receivedDt_Notify.ToolTip = CRMdbData.ComplaintItem.received_dt.Error;
                check = false;
            }
            
            //Item Price
            if (CRMdbData.Item.item_price.validate(txt_itemPrice.Text))
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                itemPrice_Notify.ToolTip = CRMdbData.Item.item_price.Error;
                check = false;
            }

            //Shoe Side
            if (CRMdbData.ComplaintItem.shoe_side.validate(cmb_shoeSide.Text))
            {
                shoeSide_Notify.Source = shoeSide_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                shoeSide_Notify.Source = shoeSide_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                shoeSide_Notify.ToolTip = CRMdbData.ComplaintItem.shoe_side.Error;
                check = false;
            }


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

            //Item ID
            if (CRMdbData.Item.item_id.validate(txt_itemID.Text))
            {
                itemID_Notify.Source = itemID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                itemID_Notify.Source = itemID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                itemID_Notify.ToolTip = CRMdbData.Item.item_id.Error;
                check = false;
            }

            //Item Defect
            if (CRMdbData.ComplaintItem.item_defect.validate(txt_defect.Text))
            {
                defect_Notify.Source = defect_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                defect_Notify.Source = defect_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                defect_Notify.ToolTip = CRMdbData.ComplaintItem.item_defect.Error;
                check = false;
            }

            //Item Defect Image
            if (CRMdbData.ComplaintItem.item_defect_img.validate(defectImageSource))
            {
                defectImage_Notify.Source = defectImage_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                defectImage_Notify.Source = defectImage_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                defectImage_Notify.ToolTip = CRMdbData.ComplaintItem.item_defect_img.Error;
                check = false;
            }

            //Item Remarks
            if (CRMdbData.ComplaintItem.item_remarks.validate(txt_remarks.Text))
            {
                remarks_Notify.Source = remarks_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                remarks_Notify.Source = remarks_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                remarks_Notify.ToolTip = CRMdbData.ComplaintItem.item_remarks.Error;
                check = false;
            }

            return check;
        }



        ~ReceivedItem_Details() { }



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
                    string query2 = "SELECT location_id , location_name from Location WHERE location_type = 'HQ' ";
                    Database db1 = new Database();
                    System.Data.DataTable dt1 = db1.GetData(query2);

                    DatabaseBased_Objects.Location HQ = new DatabaseBased_Objects.Location();
                    if (dt1.Rows.Count == 1)
                    {
                        HQ.locID = Int32.Parse(dt1.Rows[0]["location_id"].ToString());
                        HQ.locName = dt1.Rows[0]["location_name"].ToString();
                    }

                    compID = Int32.Parse(txt_compID.Text);
                    receivedDt = DateTime.Parse(dt_receivedDt.Text);
                    itemTypeID = Int32.Parse(txt_itemTypeID.Text);
                    itemID = txt_itemID.Text;
                    itemDefect = txt_defect.Text;
                    itemRemarks = txt_remarks.Text;
                    itemPrice = Double.Parse(txt_itemPrice.Text);
                    shoeSide = cmb_shoeSide.Text;

                    string query = "INSERT INTO Item (item_id ,item_price , item_type_id ) VALUES ( '" + itemID + "' , " + itemPrice + " , '" + itemTypeID + "' ) ";
                    query += "DECLARE @COMPstatusID int SET @COMPstatusID = (select case when comp_status_id = 1 then 2 when comp_status_id = 26 then 27 END as comp_status_id from Complaint WHERE comp_id = '" + compID + "') ";
                    query += "UPDATE Complaint SET comp_status_id = @COMPstatusID WHERE comp_id = " + compID + " ";
                    query += "INSERT INTO ComplaintItem ( shoe_side , received_dt , item_defect , item_remarks , item_id , item_type_id ,comp_id ) VALUES ( '"+shoeSide+"' , '"+receivedDt+"' , '"+itemDefect+"' , '"+itemRemarks+"' , '"+itemID+"' , '"+itemTypeID+"' , '"+compID+ "' ) DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as comp_item_id , @COMPstatusID as comp_status_id ";
                    
                    Database db = new Database();
                    System.Data.DataTable dt = db.GetData(query);
                    string compStatusID = dt.Rows[0]["comp_status_id"].ToString();
                    int compItemID = Int32.Parse(dt.Rows[0]["comp_item_id"].ToString());

                    string imagePath = saveImageToLocal(compItemID);
                    string query1 = "Update ComplaintItem SET item_defect_img = '" + imagePath + "' WHERE comp_item_id = " + compItemID + " ";
                    if (db.Save_Del_Update(query1)>0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();
                        if (compStatusID.Equals("2"))
                        {
                            LoadMainMenu.LoadFor(this);
                        }
                        else if (compStatusID.Equals("27"))
                        {
                            Login.b1.closeWindowAndOpenNextWindow(this, new Deliver_Item_Window(compID , HQ , Login.Loc));
                        }
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

        private void btn_defectUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                browseImageToLocal();
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }

        private void btn_itemTypeSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Item_Type_Window(true);
                if (w.ShowDialog() == true)
                {
                    txt_itemTypeID.Text = w.txt_itemTypeID.Text;
                }
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
