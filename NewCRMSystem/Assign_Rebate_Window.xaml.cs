﻿using System;
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
    public partial class Assign_Rebate_Window : Window
    {
        private int compID = 0;
        private string rebatePercentage = "";
        
        public Assign_Rebate_Window()
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

        public Assign_Rebate_Window(int compID1)
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
                string query = "SELECT CI.comp_id FROM ComplaintItem as CI , Complaint as C WHERE C.comp_id = CI.comp_id AND C.comp_status_id = 2 AND NOT EXISTS (SELECT comp_item_id FROM Rebate as R WHERE R.comp_item_id = CI.comp_item_id)";
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
            string rebPerc = cmb_rebatePercentage.Text;
            if (rebPerc.Equals("5%"))
            {
                percentage = 0.05;
            }
            else if (rebPerc.Equals("10%"))
            {
                percentage = 0.10;
            }
            else if (rebPerc.Equals("15%"))
            {
                percentage = 0.15;
            }
            else if (rebPerc.Equals("20%"))
            {
                percentage = 0.20;
            }
            else if (rebPerc.Equals("25%"))
            {
                percentage = 0.25;
            }
            else if (rebPerc.Equals("50%"))
            {
                percentage = 0.50;
            }
            else if (rebPerc.Equals("55%"))
            {
                percentage = 0.55;
            }
            else if (rebPerc.Equals("60%"))
            {
                percentage = 0.60;
            }
            else if (rebPerc.Equals("65%"))
            {
                percentage = 0.65;
            }
            else if (rebPerc.Equals("70%"))
            {
                percentage = 0.70;
            }
            else if (rebPerc.Equals("75%"))
            {
                percentage = 0.75;
            }
            else if (rebPerc.Equals("80%"))
            {
                percentage = 0.80;
            }
            else if (rebPerc.Equals("85%"))
            {
                percentage = 0.85;
            }
            else if (rebPerc.Equals("90%"))
            {
                percentage = 0.90;
            }
            else if (rebPerc.Equals("95%"))
            {
                percentage = 0.95;
            }
            else if (rebPerc.Equals("100%"))
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

        private void loadItemImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_itemImage.Source = imageSource;
        }

        private void loadDefectImageFromLocal(string path)
        {
            ImageSource imageSource = new BitmapImage(new Uri(path));
            img_defectImage.Source = imageSource;
        }

        private void loadData(string compID1)
        {
            string query = "SELECT CI.item_type_id , CI.item_id , CI.item_defect , CI.item_defect_img , CI.item_remarks , CUS.cus_id , CUS.cus_name , CUS.cus_tp , CUS.cus_email , I.item_price , IT.item_brand , IT.item_category , IT.item_name , IT.item_size , IT.item_pic from ComplaintItem as CI , CustomerComplaint as CC , Item as I , ItemType as IT , Customer as CUS WHERE CI.comp_id = '" + compID1 + "' and CC.comp_id = CI.comp_id and I.item_id = CI.item_id and CI.item_type_id = IT.item_type_id AND CC.cus_id = CUS.cus_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query);

            txt_itemTypeID.Text = dt.Rows[0]["item_type_id"].ToString();
            txt_itemID.Text = dt.Rows[0]["item_id"].ToString();
            txt_itemDefect.Text = dt.Rows[0]["item_defect"].ToString();
            txt_itemRemarks.Text = dt.Rows[0]["item_remarks"].ToString();
            txt_cusID.Text = dt.Rows[0]["cus_id"].ToString();
            txt_cusName.Text = dt.Rows[0]["cus_name"].ToString();
            txt_cusTp.Text = dt.Rows[0]["cus_tp"].ToString();
            txt_cusEmail.Text = dt.Rows[0]["cus_email"].ToString();
            txt_itemPrice.Text = dt.Rows[0]["item_price"].ToString();

            txt_brand.Text = dt.Rows[0]["item_brand"].ToString();
            txt_category.Text = dt.Rows[0]["item_category"].ToString();
            txt_name.Text = dt.Rows[0]["item_name"].ToString();
            txt_size.Text = dt.Rows[0]["item_size"].ToString();

            string imagePath1 = dt.Rows[0]["item_defect_img"].ToString();
            if (imagePath1.Length > 0)
            {
                loadDefectImageFromLocal(imagePath1);
            }
            else
            {
                img_defectImage.Source = null;
            }
            

            string imagePath2 = dt.Rows[0]["item_pic"].ToString();
            if (imagePath2.Length > 0)
            {
                loadItemImageFromLocal(imagePath2);
            }
            else
            {
                img_itemImage.Source = null;
            }
            
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
            
            //HQ Manager ID
            if (CRMdbData.Manager.emp_id.validate(Login.EmpID.ToString())) { }
            else
            {
                MessageBox.Show("Logged Employee ID Error ", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                check = false;
            }


            return check;
        }

        ~Assign_Rebate_Window() { }



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
                    int HQManagerID = Login.EmpID;

                    string query = "DECLARE @COMPitemID int SET @COMPitemID = (SELECT CI.comp_item_id FROM ComplaintItem CI WHERE CI.comp_id = '" + compID + "')  INSERT INTO Rebate (comp_item_id ,hQManager ,rebate_percentage ) VALUES (@COMPitemID," + HQManagerID + ",'" + rebatePercentage + "') ";
                    query += "UPDATE Complaint SET comp_status_id = 3 WHERE comp_id = " + compID + " ";

                    Database db = new Database();

                    if (db.Save_Del_Update(query) > 0)
                    {
                        GenericMessageBoxes.DatabaseMessages.DataInsertMessage.Successful();

                        SMSMessages.sendMessage( SMSMessages.getCusTp(compID) , "Rebate has been assigned for your Item with Complaint ID '" + compID + "' \n Item Price : Rs." + txt_itemPrice.Text + " \n Rebate Percentage : " + rebatePercentage + " \n Rebate Amount : Rs." + txt_rebateAmount.Text + " ");
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
                GenericMessageBoxes.ExceptionMessages.SQLExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                GenericMessageBoxes.ExceptionMessages.ExceptionMessage(ex);
            }
        }
    }
}
