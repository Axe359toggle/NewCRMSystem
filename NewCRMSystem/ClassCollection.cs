using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace NewCRMSystem
{
    public class BackButton
    {


        static string[] previousWindows { get; set; }
        static int noOfWindows { get; set; }

        public BackButton()
        {
            previousWindows = new string[10];
        }

        public BackButton(int maxNoOfWindows)
        {
            previousWindows = new string[maxNoOfWindows];
        }

        public int getNoOfWindows()
        {
            return noOfWindows;
        }

        public string getPreviousWindow()
        {
            string value;
            if (noOfWindows > 0)
            {
                value = previousWindows[noOfWindows - 1];
            }
            else
            {
                value = "Error";
            }
            return value;
        }

        public void goBack(Window Window1)
        {

            Window currentWindow = Window1;
            if (noOfWindows > 0)
            {
                Window Window2 = (Window)Activator.CreateInstance(Type.GetType(previousWindows[noOfWindows - 1]));
                previousWindows[noOfWindows] = null;
                noOfWindows--;
                Window2.Show();
                currentWindow.Hide();
            }
            else
            {
                MessageBox.Show("No available previous Windows");
            }

        }

        public void goBack()
        {
            if (noOfWindows > 0)
            {
                Window Window2 = (Window)Activator.CreateInstance(Type.GetType(previousWindows[noOfWindows - 1]));
                previousWindows[noOfWindows] = null;
                noOfWindows--;
                Window2.Show();

            }
            else
            {
                MessageBox.Show("No available Windows");
            }

        }

        public void addCurrentWindow(Window Window1)
        {
            previousWindows[noOfWindows] = Window1.GetType().FullName;
            noOfWindows++;
        }

        public void hideWindowAndOpenNextWindow(Window Window1, Window Window2)
        {
            addCurrentWindow(Window1);
            Window2.Show();
            Window1.Hide();

        }

        public void removePreviousWindow()
        {
            previousWindows[noOfWindows] = null;
            noOfWindows--;
        }
        ~BackButton() { }
    }


    class Person
    {
        string id { get; set; }
        string fname { get; set; }
        string nic { get; set; }

        public Person(string x, string y, string z)
        {
            id = x;
            fname = y;
            nic = z;
        }

        public string getID()
        {
            return id;
        }
        public string getFname()
        {
            return fname;
        }
        public string getNic()
        {
            return nic;
        }
        ~Person() { }
    }
    class Database
    {
        private SqlConnection con;
        private SqlCommand cmd;
        public Database()
        {
            con = new SqlConnection();
            con.ConnectionString = @"Data Source=DESKTOP-99OKMBM;Initial Catalog=NewCRMdb;Integrated Security=True";
        }

        public void openCon()
        {
            con.Open();
        }
        public void closeCon()
        {
            con.Close();
        }

        public int Save_Del_Update(string query)
        {
            int rows;
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            cmd = new SqlCommand(query, con);
            rows = cmd.ExecuteNonQuery();
            cmd.Dispose();
            closeCon();
            return rows;
        }
        public DataTable GetData(string query)
        {
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            closeCon();
            return dt;
        }

        /*
        public string ReadData(string query, string column)
        {
            string tb = "555";
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                tb = Convert.ToString(dr[column]);
            }
            else
            {
                MessageBox.Show("Either returns multiple values or does not return a value");
            }
            closeCon();

            return tb;

        }
        */

        public Person ReadData1(string query)
        {
            string x = "";
            string y = "";
            string z = "";

            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                x = Convert.ToString(dr["emp_id"]);
                y = Convert.ToString(dr["password"]);
                z = Convert.ToString(dr["desID"]);
            }
            else
            {
                MessageBox.Show("Either returns multiple values or does not return a value");
            }
            closeCon();
            Person p = new Person(x, y, z);
            return p;

        }
        ~Database() { }
    }

    static class Notifications
    {
        internal static void setNotification()
        {
            throw new NotImplementedException();
        }

        internal static void setNotification(string value, string type1, string type2, string des)
        {
            try
            {
                string query = " insert into Notifications (des_id,notification_type) values('" + des + "','" + type1 + "') declare @ID int = SCOPE_IDENTITY() insert into " + type1 + "_Notification values(@ID,'" + value + "','" + type2 + "') ";

                Database db = new Database();
                int rows = db.Save_Del_Update(query);
                MessageBox.Show(rows + " rows inserted");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Exception \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

    }

    static class Password
    {
        internal static string sha256(string randomString)
        {
            randomString = " TheCRMSystem" + randomString;
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }

    static class CRMdbData
    {
        //Location table
        internal static class Location
        {
            internal static class location_id
            {
                static int size = 5;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class location_type
            {
                static int size = 12;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class location_name
            {
                static int size = 20;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class addr_no
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class addr_lane
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class addr_town
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class addr_city
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class location_tp
            {
                static int size = 12;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //Designation table
        internal static class Designation
        {
            internal static class des_id
            {
                static int size = 1;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class desName
            {
                static int size = 20;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //Manager table
        internal static class Manager
        {
            internal static class emp_id
            {
                static int size = 6;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_title
            {
                static int size = 5;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_fname
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_lname
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_tp
            {
                static int size = 12;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_email
            {
                static int size = 100;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //Complaint table
        internal static class Complaint
        {
            internal static class comp_id
            {
                static int size = 8;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class comp_type
            {
                static int size = 8;
                internal static int Size
                {
                    get { return size; }
                }
                static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //Customer table
        internal static class Customer
        {
            internal static class cus_id
            {
                static int size = 7;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class cus_name
            {
                static int size = 50;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class cus_tp
            {
                static int size = 12;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class cus_email
            {
                static int size = 100;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //CustomerComplaint table
        internal static class CustomerComplaint
        {
            internal static class comp_method
            {
                static int size = 9;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class cus_comp_type
            {
                static int size = 5;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
        //Reference table
        internal static class Reference
        {
            internal static class ref_id
            {
                static int size = 8;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length > 0 && value.Length < size)
                        check = false;
                    return check;
                }
            }
        }
        //Login table
        internal static class Login
        {
            internal static class login_id
            {
                static int size = 6;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_username
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class emp_pass
            {
                static int size = 64;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }

        //Staff Complaint table
        internal static class StaffComplaint
        {
            internal static class staff_id
            {
                static int size = 4;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class staff_name
            {
                static int size = 30;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class description
            {
                static int size = 250;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class remarks
            {
                static int size = 250;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }

        //Item Type table
        internal static class ItemType
        {
            internal static class item_type_id
            {
                static int size = 5;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_brand
            {
                static int size = 10;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_category
            {
                static int size = 10;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_name
            {
                static int size = 50;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_size
            {
                static int size = 2;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }

        //Item table
        internal static class Item
        {
            internal static class item_id
            {
                static int size = 15;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_price
            {
                static int size = 7;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class item_pic
            {
                static int size = 100;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }

        //Item table
        internal static class ComplaintItem
        {
            internal static class comp_item_id
            {
                static int size = 8;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            internal static class shoe_side
            {
                static int size = 5;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }

            /*
            internal static class received_dt
            {
                internal static int size = 100;
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
            */
            internal static class item_defect_img
            {
                static int size = 100;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }

            internal static class item_defect
            {
                static int size = 100;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }

            internal static class item_remarks
            {
                static int size = 250;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                        check = false;
                    return check;
                }
            }
        }
    }

}
