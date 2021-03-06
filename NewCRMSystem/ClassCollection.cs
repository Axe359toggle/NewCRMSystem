﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Windows;

using System.Configuration;
using System.Text.RegularExpressions;

using Twilio;
using Twilio.Rest.Api.V2010.Account;

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
                currentWindow.Close();
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

        public void closeWindowAndOpenNextWindow(Window Window1, Window Window2)
        {
            addCurrentWindow(Window1);
            Window2.Show();
            Window1.Close();

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
        internal SqlCommand Cmd { get { return cmd; } }

        public Database()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseKey"].ConnectionString;
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
            int rows = 0;
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(con.State == System.Data.ConnectionState.Open)
            {
                cmd = new SqlCommand(query, con);
                rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            closeCon();
            return rows;
        }

        public int Save_Del_Update(string query , byte[] imgByteArr)
        {
            int rows = 0;
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
                rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            closeCon();
            return rows;
        }

        public DataTable GetData(string query)
        {
            DataTable dt = null;
            try
            {
                openCon();
            }
            catch (SqlException)
            {
                MessageBox.Show("Check the Database Connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                dt = new DataTable();
                da.Fill(dt);
            }
            closeCon();
            return dt;
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
                string query = " insert into Notifications (des_id,notification_type) values('" + des + "','" + type1 + "') declare @ID int = SCOPE_IDENTITY() insert into " + type1 + "_Notification values( @ID , '" + value + "' , '" + type2 + "' ) ";

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

    static class SMSMessages
    {
        internal static string getCusTp(int compID1)
        {
            string cusTP = "";

            string query1 = "SELECT C.cus_tp FROM CustomerComplaint as CC , Customer as C WHERE CC.comp_id = " + compID1 + " AND CC.cus_id = C.cus_id ";
            Database db = new Database();
            System.Data.DataTable dt = db.GetData(query1);

            if (dt.Rows.Count > 0)
            {
                cusTP = dt.Rows[0]["cus_tp"].ToString();
            }
            return cusTP;
        }

        internal static void sendMessage(string receiverNo , string content)
        {
            try
            {
                // Find your Account Sid and Token at twilio.com/console
                const string accountSid = "AC2b264433451ba21e2f9bd18827f15d18";
                const string authToken = "auth";//your_auth_token

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: content,
                    from: new Twilio.Types.PhoneNumber("+14143107178"),
                    to: new Twilio.Types.PhoneNumber(receiverNo)
                );

                // Console.WriteLine(message.Sid);
                MessageBox.Show("Message Sent Successfully","SMS", MessageBoxButton.OK, MessageBoxImage.Information);

                
            }
            catch (Twilio.Exceptions.TwilioException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    static class LoadMainMenu
    {
        internal static void LoadFor(Window window)
        {
            if (Login.DesID.Equals("H"))
                Login.b1.closeWindowAndOpenNextWindow(window, new HQ_Manager_Dashboard());
            else if (Login.DesID.Equals("S"))
                Login.b1.closeWindowAndOpenNextWindow(window, new Showroom_Manager_Mainmenu());
            else if (Login.DesID.Equals("F"))
                Login.b1.closeWindowAndOpenNextWindow(window, new Factory_Manager_Dashboard());
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

    static class Logout
    {
        internal static bool logoutInitiated = false;
        internal static void resetLogout()
        {
            logoutInitiated = false;
        }
        internal static void logout()
        {

            if (Login.LogindetailID > 0 && !logoutInitiated )
            {
                Database db = new Database();
                string query = "Update LoginDetails set logout_dt = GETDATE() WHERE logindetail_id ='" + Login.LogindetailID + "' ";
                db.Save_Del_Update(query);
                logoutInitiated = true;

                Login lg = new Login();
                lg.Show();

                MessageBox.Show( "Logged out" , "Information" , MessageBoxButton.OK , MessageBoxImage.Information );
            }
        }
    }

    static class GenericMessageBoxes
    {
        internal static class DatabaseMessages
        {
            internal static class DataInsertMessage
            {
                internal static void Successful()
                {
                    MessageBox.Show( "Data inserted Successfully" , "Success" , MessageBoxButton.OK , MessageBoxImage.Information );
                }

                internal static void Failed()
                {
                    MessageBox.Show( "Data insertion failed" , "Error" , MessageBoxButton.OK , MessageBoxImage.Exclamation );
                }
            }

            internal static class DataDeleteMessage
            {
                internal static void Successful()
                {
                    MessageBox.Show("Data deleted Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                internal static void Failed()
                {
                    MessageBox.Show("Data deletion failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            internal static class DataUpdateMessage
            {
                internal static void Successful()
                {
                    MessageBox.Show("Data updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                internal static void Failed()
                {
                    MessageBox.Show("Data updation failed", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        internal static class ExceptionMessages
        {
            internal static void ExceptionMessage(Exception ex)
            {
                MessageBox.Show( ex.ToString() , "Error" , MessageBoxButton.OK , MessageBoxImage.Error );
            }

            internal static void SQLExceptionMessage(SqlException ex)
            {
                MessageBox.Show( ex.ToString() , "SQL Error" , MessageBoxButton.OK , MessageBoxImage.Error );
            }
        }
    }

    static class Validation
    {
        internal static bool validate(System.Windows.Controls.Image image, bool condition, string ErrorMessage)
        {
            bool check = true;

            if (condition)
            {
                image.Source = image.TryFindResource("notifyCorrectImage") as System.Windows.Media.Imaging.BitmapImage;
            }
            else
            {
                image.Source = image.TryFindResource("notifyErrorImage") as System.Windows.Media.Imaging.BitmapImage;
                image.ToolTip = ErrorMessage;
                check = false;
            }

            return check;
        }

        internal static DateTime getCompDate(int compID)
        {
            string query = "SELECT comp_dt FROM Complaint WHERE comp_id = '" + compID + "'  ";
            Database db = new Database();
            DataTable dt = db.GetData(query);
            DateTime compDt = DateTime.Parse(dt.Rows[0]["comp_dt"].ToString());

            return compDt;
        }
        internal static DateTime getSourceDate(int deliveryID)
        {
            string query = "SELECT source_dt FROM Complaint WHERE delivery_id = '" + deliveryID + "'  ";
            Database db = new Database();
            DataTable dt = db.GetData(query);
            DateTime sourceDt = DateTime.Parse(dt.Rows[0]["source_dt"].ToString());

            return sourceDt;
        }
    }

    public class DatabaseBased_Objects
    {
        public class Location
        {
            internal int locID = 0;
            //internal int locID { get { return location_id; } set { location_id = locID; } }
            internal string locName = "";
            internal string locType = "";
            internal string addrNo = "";
            internal string addrLane = "";
            internal string addrTown = "";
            internal string addrCity = "";

            /*
            internal void setLocID(int locID1) { locID = locID1; }
            internal void setLocName(string locName1) { locName = locName1; }
            internal void setLocType(string locType1) { locType = locType1; }
            internal void setAddrNo(string addrNo1) { addrNo = addrNo1; }
            internal void setAddrLane(string addrLane1) { addrLane = addrLane1; }
            internal void setAddrTown(string addrTown1) { addrTown = addrTown1; }
            internal void setAddrCity(string addrCity1) { addrCity = addrCity1; }

            internal int getLocID() { return locID; }
            internal string getLocName() { return locName; }
            internal string getLocType() { return locType; }
            internal string getAddrNo() { return addrNo; }
            internal string getAddrLane() { return addrLane; }
            internal string getAddrTown() { return addrTown; }
            internal string getAddrCity() { return addrCity; }
            */
        }
    }

    internal static class CommonMethods
    {
        internal static bool checkInvertedComma(string value)
        {
            bool check = false;
            if (Regex.IsMatch(value, @"^[']$"))
            {
                check = true;
            }
            return check;
        }
    }

    internal static class CRMdbData
    {
        
        //Location table
        internal static class Location
        {
            internal static class location_id 
            {
                static string error = "" ;
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class location_type 
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class location_name 
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class addr_no
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class addr_lane
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class addr_town
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class addr_city
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class location_tp
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (!Regex.IsMatch(value, @"^[+]{1}[0-9]{11}$"))
                    {
                        check = false;
                        error = "Invalid telephone number";
                    }
                    return check;
                }
            }
        }
        //Designation table
        internal static class Designation
        {
            internal static class des_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class desName
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
        }
        //Manager table
        internal static class Manager
        {
            internal static class emp_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class emp_title
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class emp_fname
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class emp_lname
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class emp_tp
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (!Regex.IsMatch(value, @"^[+]{1}[0-9]{11}$"))
                    {
                        check = false;
                        error = "Invalid telephone number";
                    }
                    return check;
                }
            }
            /*
            internal static class emp_email
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }*/
        }
        //Complaint table
        internal static class Complaint
        {
            internal static class comp_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class comp_type
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }
        //Customer table
        internal static class Customer
        {
            internal static class cus_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class cus_name
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class cus_tp
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (!Regex.IsMatch(value, @"^[+]{1}[0-9]{11}$"))
                    {
                        check = false;
                        error = "Invalid telephone number";
                    }
                    return check;
                }
            }
            internal static class cus_email
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (!Regex.IsMatch(value, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                    {
                        check = false;
                        error = "Enter a valid email.";
                    }
                    return check;
                }
            }
        }
        //CustomerComplaint table
        internal static class CustomerComplaint
        {

            internal static class comp_method
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class cus_comp_type
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }
        //Reference table
        internal static class Reference
        {
            internal static class ref_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }
        //Login table
        internal static class Login
        {
            internal static class login_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class emp_username
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class emp_pass
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 64;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    bool check = true;

                    var input = value;
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        check = false;
                        error = "Password should not be empty";
                    }

                    var hasNumber = new Regex(@"[0-9]+");
                    var hasUpperChar = new Regex(@"[A-Z]+");
                    var hasMiniMaxChars = new Regex(@".{8,15}");
                    var hasLowerChar = new Regex(@"[a-z]+");
                    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                    if (!hasLowerChar.IsMatch(input))
                    {
                        check = false;
                        error = "Password should contain At least one lower case letter";

                    }
                    else if (!hasUpperChar.IsMatch(input))
                    {
                        check = false;
                        error = "Password should contain At least one upper case letter";
                    }
                    else if (!hasMiniMaxChars.IsMatch(input))
                    {
                        check = false;
                        error = "Password should not be less than or greater than 12 characters";
                    }
                    else if (!hasNumber.IsMatch(input))
                    {
                        check = false;
                        error = "Password should contain At least one numeric value";
                    }
                    else if (!hasSymbols.IsMatch(input))
                    {
                        check = false;
                        error = "Password should contain at least one special case characters";
                    }
                    return check;
                }
            }
        }

        //Staff Complaint table
        internal static class StaffComplaint
        {
            internal static class staff_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 6;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class staff_name
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class description
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class remarks
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
        }

        //Item Type table
        internal static class ItemType
        {
            internal static class item_type_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class item_brand
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class item_category
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class item_name
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class item_size
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class item_pic
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 120;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }

        //Item table
        internal static class Item
        {
            internal static class item_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 15;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length != size)
                    {
                        check = false;
                        error = "Length should be " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            internal static class item_price
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 7;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;

                    var moneyR = new Regex(@"^(?!^0\.00$)(([1-9][\d]{0,6})|([0]))\.[\d]{2}$");

                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (!moneyR.IsMatch(value))
                    {
                        check = false;
                        error = "Invalid price. (Valid)Eg:- 200.00";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
        }

        //Complaint Item table
        internal static class ComplaintItem
        {
            internal static class comp_item_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class shoe_side
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
            
            internal static class received_dt
            {
                static string error = "";
                internal static string Error { get { return error; } }
                internal static int size = 30;
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
            internal static class item_defect_img
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 120;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }

            internal static class item_defect
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }

            internal static class item_remarks
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot cannot include single inverted comma (')";
                    }
                    return check;
                }
            }
        }
        //Rebate table
        internal static class Rebate
        {
            internal static class rebate_percentage
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 4;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }

            internal static class customer_choice
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }

        //Delivery table
        internal static class Delivery
        {
            internal static class delivery_id
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }

            internal static class source_dt
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }

            internal static class destination_dt
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }

        //Investigation table
        internal static class Investigation
        {
            internal static class investigation_dt
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }

        //Repair table
        internal static class Repair
        {
            internal static class repair_remarks
            {
                static string error = "";
                internal static string Error { get { return error; } }
                static int size = 200;
                internal static int Size
                {
                    get { return size; }
                }
                internal static bool validate(string value)
                {
                    value = value.Trim();
                    bool check = true;
                    if (value.Length == 0 || value.Length > size)
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    else if (CommonMethods.checkInvertedComma(value))
                    {
                        check = false;
                        error = "Cannot include single inverted comma (')";
                    }
                    return check;
                }
            }

            internal static class repair_dt
            {
                static string error = "";
                internal static string Error { get { return error; } }
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
                    {
                        check = false;
                        error = "Cannot be Empty or Greater than " + size + " characters";
                    }
                    return check;
                }
            }
        }
    }

}
