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


        static string[] previousWindows;
        static int noOfWindows = 0;

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

        public void addWindowAndOpenNextWindow(Window Window1, Window Window2)
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
        string id;
        string fname;
        string nic;

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
            con.ConnectionString = @"Data Source=DESKTOP-99OKMBM;Initial Catalog=CRMdb;Integrated Security=True";
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
        internal static class Location
        {
            internal static class location_id
            {
                internal static int size = 5;
            }
            internal static class addr_no
            {
                internal static int size = 30;
            }
            internal static class addr_lane
            {
                internal static int size = 100;
            }
            internal static class addr_town
            {
                internal static int size = 30;
            }
            internal static class addr_city
            {
                internal static int size = 30;
            }
            internal static class location_type
            {
                internal static int size = 12;
            }
        }
        internal static class Designation
        {
            internal static class des_id
            {
                internal static int size = 1;
            }
            internal static class desName
            {
                internal static int size = 20;
            }
        }
        internal static class Manager
        {
            internal static class emp_id
            {
                internal static int size = 6;
            }
            internal static class emp_title
            {
                internal static int size = 5;
            }
            internal static class emp_fullname
            {
                internal static int size = 100;
            }
            internal static class emp_tp
            {
                internal static int size = 12;
            }
            internal static class emp_email
            {
                internal static int size = 100;
            }
        }
        internal static class Complaint
        {
            internal static class comp_id
            {
                internal static int size = 8;
            }
            internal static class comp_type
            {
                internal static int size = 8;
            }
        }
        internal static class Customer
        {
            internal static class cus_id
            {
                internal static int size = 7;
            }
            internal static class cus_name
            {
                internal static int size = 50;
            }
            internal static class cus_tp
            {
                internal static int size = 12;
            }
            internal static class cus_email
            {
                internal static int size = 100;
            }
        }
        internal static class CustomerComplaint
        {
            internal static class comp_method
            {
                internal static int size = 9;
            }
            internal static class cus_comp_type
            {
                internal static int size = 5;
            }
        }
        internal static class Reference
        {
            internal static class ref_id
            {
                internal static int minsize = 8;
            }
        }
        internal static class Login
        {
            internal static class login_id
            {
                internal static int size = 6;
            }
            internal static class emp_username
            {
                internal static int size = 30;
            }
            internal static class emp_pass
            {
                internal static int size = 64;
            }
        }

    }

}
