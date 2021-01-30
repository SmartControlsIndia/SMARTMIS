using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.IO;

namespace SmartMIS
{
    /// <summary>
    /// Summary description for smartMISWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class smartMISWebService : System.Web.Services.WebService
    {
        myConnection myConnection = new myConnection();


        public string[] status = new string[] { "OK", "Not OK" };
        public string[] userDecision = new string[] { "OK", "Rework", "Scrap" };
        public string[] faultDirection = new string[] { "-", "Serial", "Non Serial" };
        public string[] statusState = new string[] { "OK", "Not OK", "Rework", "Scrap" };


        public string[] reportType = new string[] { "  ", "Workcenter wise" };
        public string[] weekDayName = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public string[] monthName = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

        public string[] yearName = new string[] { "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023","2024", "2025", "2026"};
        public string[] shift = new string[] { "A", "B", "C" };
        public string[] duration = new string[] { "Minute(s)", "Hour(s)", "Day(s)" };
        public string[] csvHeaders = new string[] { "No", "Date", "Time", "Shift", "cycleTime", "Model No.", "Size code", "Barcode", "Low-Speed", "Spindle Rpm", "Air press.", "Top Temp.", "Center Temp.", "Bottom temp.", "Around temp.", "Load", "CW RFV-OA", "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "LFV-OA", "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "LFD", "", "CCW RFV-OA", "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "LFV-OA", "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "LFD", "", "CON", "", "PLY", "", "Ufm", "LT-OA", "1H", "LB-OA", "1H", "RT-OA", "1H", "RC-OA", "1H", "RB-OA", "1H", "LTbulg", "dent", "LBbulg", "dent", "RoRank", "Bal-Speed", "Spindle Rpm", "Air press.", "Top Temp.", "Center Temp.", "Bottom temp.", "Around temp.", "Upper", "Lower", "Static", "Couple", "Up+Low", "", "Bal", "Total" };

        public string[] attendanceKeyWord = new string[] { "A", "P", "L", "WO", "POW" };
        public string[] attendanceImagePath = new string[] { "../Images/absent.png", "../Images/present.png", "../Images/leave.png", "../Images/leave.png", "../Images/leave.png" };


        public enum order {Asc, Desc};

        /// <summary>
        /// Function for filling DropDownList
        /// </summary>
        /// <param name="tableName">Name of database table</param>
        /// <param name="coloumnName">Name of coloum of table</param>
        /// <returns></returns>

        public string getExcelPath()
        {
            //return @"O:\SMARTMIS\Excel\";//FOR SERVER 53 REPORTING
            return @"D:\CTP_CT_1148\SMARTMIS\Excel";
            //return @"O:\SMARTMIS\Excel\";//FOR SERVER 52 TBR BUDDHE STATION
           // return @" E:\Projects\BacupCTWise\JKTyreChennai_CT424\HarshitSmartMIS\SMARTMIS\SMARTMIS\Excel";
        }
        public void writeLogs(string message, string methodName, string fileName)
        {
            try
            {
                //using (StreamWriter w = new StreamWriter(Server.MapPath("ErrorLogs\\data.txt"), true))
                using (StreamWriter w = new StreamWriter(("C:\\IMSMART\\SMARTMIS\\ErrorLogs\\data.txt"), true))
                {
                    w.WriteLine(DateTime.Now.ToString() + "\t" + message + "\t" + methodName + "()\t" + fileName); // Write the text
                }
            }
            catch (Exception ex)
            {
               
             }
        }
        public void writeBuddheLogs(string message, string methodName, string fileName)
        {
            try
            {
                using (StreamWriter w = new StreamWriter(Server.MapPath("ErrorLogs\\Boddhedata.txt"), true))
                {
                    w.WriteLine(DateTime.Now.ToString() + "\t" + message + "\t" + methodName + "()\t" + fileName); // Write the text
                }
            }
            catch (Exception ex)
            { }
        }
        [WebMethod]
        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 01 April 2011
            //Date Updated  : 01 April 2011
            //Revision No.  : 01

            flag.Add("");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                //sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + "";
                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " where " + coloumnName + " != ''"; // Updated Date  : 05 Aug 2020
                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch( Exception exp)
            {

            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);


            return flag;
        }

        /// <summary>
        /// Function for filling DropDownList
        /// </summary>
        /// <param name="tableName">Name of database table</param>
        /// <param name="coloumnName">Name of coloum of table</param>
        /// <param name="whereClause">Where Condition for Query</param>
        /// <returns></returns>

        public ArrayList FillDropDownList(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            flag.Add("");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }
        public ArrayList FillDropDownListXray(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            flag.Add("ALL");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }

        /// <summary>
        /// Function for checking records in Table
        /// </summary>
        /// <param name="tableName">Name of database table</param>
        /// <param name="coloumnName">Name of coloum of table</param>
        /// <param name="whereClause">Where Condition for Query</param>
        /// /// <param name="notifyIcon">Image icon for notification</param>
        /// <returns></returns>

        public bool IsRecordExist(string tableName, string coloumnName, string whereClause, out int notifyIcon)
        {
            bool flag = false;
            notifyIcon = 1;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = true;
                    notifyIcon = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }
        public bool IsRecordExist1(string tableName, string coloumnName,string coloumnName1, string whereClause, out int notifyIcon)
        {
            bool flag = false;
            notifyIcon = 1;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + " ," + coloumnName1 + "  from " + tableName + " " + whereClause + "";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = true;
                    notifyIcon = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }

        public bool IsRecordExistUnload(string tableName, string coloumnName,  string whereClause)
        {
            bool flag = false;
          
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + "   from " + tableName + " " + whereClause + "";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = true;
                   
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }


        /// <summary>
        /// Function for returning Workcenter Name on the basis of wcID
        /// </summary>
        /// <param name="tableName">Name of database table</param>
        /// <param name="wcID">Value of ID coloum of table</param>
        /// <returns>Work center name</returns>

        public string setWorkCenterName(string tableName, int wcID)
        {
            string flag = "";
            string sqlQuery = "";

            //Description   : Function for returning Workcenter Name on the basis of wcID
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select name from " + tableName + " WHERE (iD = " + wcID + ")";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }

        /// <summary>
        /// Function for Counting Records in a query
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="coloumnName"></param>
        /// <param name="whereClause"></param>
        /// <returns>Number of Records</returns>

        public int RecordCount(string tableName, string coloumnName, string whereClause)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = flag + 1;
                }
              }
            catch (Exception exp)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
           

            return flag;
        }

        /// <summary>
        /// Function for returning ID on the basis of passed coloum name
        /// </summary>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="coloumName">Name of the coloum on the basis of which id will be returned</param>
        /// <param name="coloumValue">Name of the coloum </param>
        /// <returns></returns>

        public string getID(string tableName, string coloumName, string coloumValue)
        {
            string flag = "";
            string sqlQuery = "";

            //Description   : Function for returning id of on the basis of coloum name
            //Author        : Brajesh kumar
            //Date Created  : 20 May 2011
            //Date Updated  : 20 May 2011
            //Revision No.  : 01
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select id from " + tableName + " WHERE (" + coloumName + " = '" + coloumValue + "')";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }
        public string getRecipeCode(string tableName, string coloumName, string coloumValue)
        {
            string flag = "";
            string sqlQuery = "";

            //Description   : Function for returning id of on the basis of coloum name
            //Author        : Brajesh kumar
            //Date Created  : 20 May 2011
            //Date Updated  : 20 May 2011
            //Revision No.  : 01
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select description from " + tableName + " WHERE (" + coloumName + " = '" + coloumValue + "')";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }
        /// <summary>
        /// Function for returning ManningID on the basis of passed coloum name
        /// </summary>
        /// <param name="coloumnName">Name of the coloum</param>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="sapCode">SAP code of employee</param>
        /// <returns>Int</returns>
        public int GetManningID(String coloumnName, String tableName, String sapCode)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + " from " + tableName + " WHERE (sapCode = '" + sapCode + "')";

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = Convert.ToInt32(myConnection.reader[0]);
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            //myConnection.close();

            return flag;
        }
        /// <summary>
        /// Function for returning Datatable on the basis of SQL Query
        /// </summary>
        /// <param name="sqlQuery">Sql Query</param>
        /// <returns></returns>
        public DataTable fillGridView(string sqlQuery, ConnectionOption option)
        {
            DataTable flag = new DataTable();

            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01

            if (option == ConnectionOption.SQL)
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = sqlQuery;

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);
                }
                catch (Exception exp)
                {

                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }
            //else if (option == ConnectionOption.MySQL)
            //{
            //    myConnection.open(ConnectionOption.MySQL);
            //    myConnection.msComm = myConnection.msConn.CreateCommand();

            //    myConnection.msComm.CommandText = sqlQuery;

            //    myConnection.msReader = myConnection.msComm.ExecuteReader(CommandBehavior.CloseConnection);
            //    flag.Load(myConnection.msReader);

            //    myConnection.msReader.Close();
            //    myConnection.msComm.Dispose();
            //    myConnection.close(ConnectionOption.MySQL);
            //}


            return flag;
        }
        /// <summary>
        /// Function for returning Datatable on the basis of SQL Query
        /// </summary>
        /// <param name="sqlQuery">Sql Query</param>
        /// <param name="coloumnName">Name of the coloum on the basis you want to order</param>
        /// <param name="order">Order for sorting the sql query</param>
        /// <returns></returns>
        public DataTable fillGridView(string sqlQuery, string coloumnName, order order)
        {
            DataTable flag = new DataTable();

            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 06 July 2011
            //Date Updated  : 06 July 2011
            //Revision No.  : 01
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = sqlQuery + " ORDER BY " + coloumnName + " " + order;

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                flag.Load(myConnection.reader);
            }
            catch (Exception exp)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }
        /// <summary>
        /// Function for making a decision status
        /// </summary>
        /// <param name="obj">Name of object</param>
        /// <returns></returns>

        public string displayUserDecision(Object obj)
        {

            //Description   : Function for making a decision status
            //Author        : Brajesh kumar
            //Date Created  : 12 April 2011
            //Date Updated  : 11 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                flag = userDecision[Convert.ToInt32(obj)];
            }
            return flag;
        }

        /// <summary>
        /// Function for returning a decision statusState
        /// </summary>
        /// <param name="value">Value of status</param>
        /// <returns></returns>

        public int getStatusState(string value)
        {
            //Description   : Function for getting the status index from webservice
            //Author        : Brajesh kumar
            //Date Created  : 18 April 2011
            //Date Updated  : 18 April 2011
            //Revision No.  : 01           
            //Revision Desc : 

            int i = 0;
            int flag = 0;

            if (!string.IsNullOrEmpty(value.ToString()))
            {
                foreach (string status in statusState)
                {
                    if (status == value)
                    {
                        flag = i;
                    }

                    i++;
                }
            }

            return flag;
        }

        /// <summary>
        /// Function for returning authentication on the basis of role
        /// </summary>
        /// <param name="userID">UserID of Logged user</param>
        /// <param name="role">roleID  of Logged user</param>
        /// <returns>True or False</returns>

        public bool authenticate(string userID, string role)
        {
            //Description   : Function for returning authentication on the basis of role
            //Author        : Brajesh kumar
            //Date Created  : 20 April 2011
            //Date Updated  : 20 April 2011
            //Revision No.  : 01

            string query = "";
            string or = "";
            string[] tempRoles = role.Split(new char[] { '#' });

            foreach (string items in tempRoles)
            {
                query = query + or +"roleID = '" + items + "'";
                or = " Or ";
            }

            bool flag = false;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT userRoles.* FROM userRoles WHERE (userID = '" + userID + "') AND (" + query + ")";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                while (myConnection.reader.Read())
                {
                    flag = true;
                }
            }
            catch(Exception xp)
            {

            }
            finally
            {

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }
        public bool uservalidation(string userID, string moduleName, int validationType)
        {

            string rights = "";

        
            bool flag = false;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT rights FROM vUserRoles WHERE (userID = '" + userID + "') AND (moduleName='" + moduleName + "')";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                while (myConnection.reader.Read())
                {
                    rights = myConnection.reader[0].ToString();
                    string[] tempRoles = rights.Split(new char[] { '#' });

                    for (int i = 0; i <= tempRoles.Length - 1; i++)
                    {
                        if (tempRoles[i] != "")
                        {
                            if (validationType == Convert.ToInt32(tempRoles[i]))
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                        }

                    }
                    if (flag == true)
                        break;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

           
                return flag;
           
        }

        /// <summary>
        /// Function for returning date format on the basis of date
        /// </summary>
        /// <param name="date">date to be formatted</param>
        /// <returns>String in the form of date</returns>

        public string formatDate(string date)
        {

            //Description   : Function for returning date format on the basis of date
            //Author        : Brajesh kumar
            //Date Created  : 01 May 2011
            //Date Updated  : 01 May 2011
            //Revision No.  : 01

            string flag = "";
            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });
            try
            {
                day = tempDate[0].ToString().Trim();
                month = tempDate[1].ToString().Trim();
                year = tempDate[2].ToString().Trim();

                flag = month + "-" + day + "-" + year;
            }
            catch(Exception exp)
            {
            }
            return flag;
        }

        /// <summary>
        /// Function for making query on the basis of WCID and dates
        /// </summary>
        /// <param name="wcID">Workcenter ID</param>
        /// <param name="fromDate">From Date</param>
        /// <param name="toDate">To Date</param>
        /// <returns>String</returns>

        public string createQuery(String wcID, String fromDate, String toDate, String fromDateColoum, String toDateColoum)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "iD = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }

        /// <summary>
        /// Function for Getting ShiftID on the basis of timestamp
        /// </summary>
        /// <param name="dtandTime">Date Time</param>
        /// <returns>Int</returns>
        /// 

        public string createQuery(String wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "ID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
        public string createwcIDQuery(String wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "wcID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
        public string wcquery(string query,string option)
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name from wcmaster where " + query + " ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        if (option == "1")
                            flag = flag + "or" + " " + "machineName = '" + myConnection.reader[0] + "'";
                        else
                            flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        if (option == "1")
                            flag = "machineName = '" + myConnection.reader[0] + "'";
                        else

                            flag = "wcname = '" + myConnection.reader[0] + "'";

                    }

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int GetShiftID(DateTime dtandTime)
        {
            int flag = 0;
            int hour = dtandTime.Hour;
            
            int i = 0;
            
            int[] shiftID = new int[3];
            int[] shiftValue = new int[3];
            String[] shiftName = new String[3];
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select iD, shiftName, CAST(DATEPART(HH, startTime) As Integer) AS [Time] from shiftMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    shiftID[i] = Convert.ToInt32(myConnection.reader[0]);
                    shiftName[i] = myConnection.reader[1].ToString();
                    shiftValue[i] = Convert.ToInt32(myConnection.reader[2]);

                    i++;
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();

            }
            if ((hour >= shiftValue[0]) && (hour < shiftValue[1]))
                flag = shiftID[0];
            else if ((hour >= shiftValue[1]) && (hour < shiftValue[2]))
                flag = shiftID[1];
            else
                flag = shiftID[2];

            return flag;
        }
        public DateTime GetTimeStamp(DateTime dtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select TOP 1  CAST(CONVERT(varchar(2), startTime) as integer) as [Time] from shiftMaster";

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = Convert.ToInt32(myConnection.reader[0].ToString());
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();
            }
            if (dtandTime.Hour < flag)
                dtandTime = dtandTime.AddDays(-1);

            return dtandTime;
        }
        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(date);
                    flag = tempDate.ToString("MM-dd-yyyy");
                    flag = flag + " " + "07:00:00";
                }
                catch (Exception exp)
                {

                }
            }
            return flag;
        }
        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {

                }
            }
            return flag;
        }
        public string TotalformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";
            if (date != null)
            {
                string day, month, year;

                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();

                    flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }

                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' " + ")OR" + " " + "(dtandTime>'" + flag3 + "'and" + " " + "dtandTime<" + "'" + flag4 + "'))";

                }

                catch (Exception exp)
                {

                }
            }
            return flag;
        }
        public string TotalprodataformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";
            if (date != null)
            {
                string day, month, year;

                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();

                    flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }

                    flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' " + ")OR" + " " + "(testTime>'" + flag3 + "'and" + " " + "testTime<" + "'" + flag4 + "'))";
                }
                catch (Exception exp)
                {

                }

            }
            return flag;
        }
        public void saveBuddedata(string barcode,string stationNo, string finaldestination, string weight)
        {

             try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "sp_BuddeDataSave";
                myConnection.comm.CommandType = CommandType.StoredProcedure;
                System.Data.SqlClient.SqlParameter GTBarcodeParameter = new System.Data.SqlClient.SqlParameter("@GTBarcode", System.Data.SqlDbType.VarChar);
                GTBarcodeParameter.Direction = System.Data.ParameterDirection.Input;
                GTBarcodeParameter.Value = barcode;
                System.Data.SqlClient.SqlParameter stationNameParameter = new System.Data.SqlClient.SqlParameter("@stationNo", System.Data.SqlDbType.VarChar);
                stationNameParameter.Direction = System.Data.ParameterDirection.Input;
                stationNameParameter.Value = stationNo;
                System.Data.SqlClient.SqlParameter destinationParameter = new System.Data.SqlClient.SqlParameter("@destination", System.Data.SqlDbType.VarChar);
                destinationParameter.Direction = System.Data.ParameterDirection.Input;
                destinationParameter.Value = finaldestination;
                System.Data.SqlClient.SqlParameter weightParameter = new System.Data.SqlClient.SqlParameter("@weight", System.Data.SqlDbType.VarChar);
                destinationParameter.Direction = System.Data.ParameterDirection.Input;
                weightParameter.Value = weight;
                myConnection.comm.Parameters.Add(GTBarcodeParameter);
                myConnection.comm.Parameters.Add(stationNameParameter);
                myConnection.comm.Parameters.Add(destinationParameter);
                myConnection.comm.Parameters.Add(weightParameter);
                myConnection.comm.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                //mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
           



 
        }
        
    }
}
