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
        private int itemID = 0;
        private string itemDefect = "";
        private string itemRemarks = "";
        private string defectImageSource = "";

        string ext = "";
        string filepath = "";


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
                MessageBox.Show(ex.ToString(), "SQL Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private string uploadImage(string filepath1, string ext1, string imageName)
        {
            string imgSource = "";
            if (filepath1.Length > 0)
            {
                var imageFile = new System.IO.FileInfo(filepath1);
                if (imageFile.Exists)// check image file exist
                {
                    // get your application folder
                    var applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    // get your 'Uploaded' folder
                    var dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(applicationPath, "Defect Images"));
                    if (!dir.Exists)
                        dir.Create();
                    // Copy file to your folder
                    string fileName = imageFile.CopyTo(System.IO.Path.Combine(dir.FullName, imageName + ext1)).ToString();
                    
                    imgSource = dir.FullName + @"\" + fileName;
                }
            }
            else
            {
                MessageBox.Show("Please Choose a image to Upload", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
            return imgSource;
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
                check = false;
            }

            /*
            //Received Date
            if (CRMdbData.Location.location_id.validate(txt_relShrmID.Text))
            {
                relShrmID_Notify.Source = relShrmID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                relShrmID_Notify.Source = relShrmID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }
            */
            
            //Item Price
            if (CRMdbData.Item.item_price.validate(txt_itemPrice.Text))
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
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
                    compID = Int32.Parse(txt_compID.Text);
                    receivedDt = DateTime.Parse(dt_receivedDt.Text);
                    itemTypeID = Int32.Parse(txt_itemTypeID.Text);
                    itemID = Int32.Parse(txt_itemID.Text);
                    itemDefect = txt_defect.Text;
                    itemRemarks = txt_remarks.Text;

                    string query = "INSERT INTO Item (item_id ,item_price ) VALUES ('"+itemID+"',"+itemPrice+") ";
                    query += "INSERT INTO ComplaintItem (shoe_side ,received_dt ,item_defect ,item_remarks ,item_id  ,item_type_id ,comp_id ) VALUES ('"+shoeSide+"','"+receivedDt+"','"+itemDefect+"','"+itemRemarks+"','"+itemID+"','"+itemTypeID+"','"+compID+"') DECLARE @ID int = SCOPE_IDENTITY() SELECT @ID as comp_item_id";
                    
                    Database db = new Database();

                    int compItemID = Int32.Parse(db.GetData(query).Rows[0]["comp_item_id"].ToString());

                    defectImageSource = uploadImage(filepath, ext,compItemID.ToString());

                    string queryUpdate = "Update ComplaintItem set item_defect_img = '"+defectImageSource+ "' WHERE comp_item_id = "+compItemID+" ";

                    if (compItemID > 0)
                    {
                        MessageBox.Show("Data inserted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (Login.DesID.Equals("H"))
                            Login.b1.hideWindowAndOpenNextWindow(this, new HQ_Manager_Dashboard());
                        else if (Login.DesID.Equals("S"))
                            Login.b1.hideWindowAndOpenNextWindow(this, new Showroom_Manager_Mainmenu());
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

        private void btn_defectUpload_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    ext = System.IO.Path.GetExtension(open.FileName);
                    ImageSource imgsource = new BitmapImage(new Uri(filepath)); // Just show The File In Image when we browse It
                    img_defect.Source = imgsource;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_itemTypeSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var w = new Item_Type_Window(true);
                Login.b1.addCurrentWindow(this);
                if (w.ShowDialog() == true)
                {
                    txt_itemID.Text = w.txt_itemTypeID.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
