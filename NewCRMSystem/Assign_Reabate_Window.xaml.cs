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
        private int compItemID = 0;
        private int itemTypeID = 0;
        private string itemImageSource = "";
        private double rebatePercentage = 0;

        string ext = "";
        string filepath = "";

        public Assign_Reabate_Window()
        {
            InitializeComponent();
        }

        public Assign_Reabate_Window(int compItemID1)
        {
            InitializeComponent();
            txt_compItemID.Text = compItemID1.ToString();
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

            //Complaint Item ID
            if (CRMdbData.ComplaintItem.comp_item_id.validate(txt_compItemID.Text))
            {
                compItemID_Notify.Source = compItemID_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                compItemID_Notify.Source = compItemID_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //Rebate Percentage
            if (CRMdbData.Item.item_price.validate(txt_itemPrice.Text))
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyCorrectImage") as BitmapImage;
            }
            else
            {
                itemPrice_Notify.Source = itemPrice_Notify.TryFindResource("notifyErrorImage") as BitmapImage;
                check = false;
            }

            //HQ Manager ID
            if (CRMdbData.Manager.emp_id.validate(Login.EmpID.ToString()))
            { }
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
            Login.b1.goBack(this);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
