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
        private int refID = 0;
        private DateTime receivedDt;
        private int itemTypeID = 0;
        private int itemID = 0;
        private string itemDefect = "";
        private string itemRemarks = "";
        private string defectImageSource = "";

        public ReceivedItem_Details()
        {
            InitializeComponent();
           
        }

        public ReceivedItem_Details(int compID1)
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
    }
}
