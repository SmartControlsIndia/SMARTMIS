using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;


namespace SmartMIS
{

    /// <summary>
    /// Standard Class for connecting to SQL Server
    /// </summary>
    public  class myConnection
    {
        public  SqlConnection conn = new SqlConnection();
        public  SqlCommand comm;
        public  SqlDataReader reader;
        public  SqlDataAdapter adapter;

        public  OleDbConnection oConn = new OleDbConnection();
        public  OleDbCommand oComm;
        public  OleDbDataReader oReader ;
        public  OleDbDataAdapter oAdapter;
       
        /// <summary>
        /// Function for opening connection with the database 
        /// </summary>

        public  void open(ConnectionOption option)
        {
            try
            {
                if (option == ConnectionOption.SQL)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                    }
                    else
                    {
                        conn = new SqlConnection();
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                        conn.Open();
                    }
                }
                
            }
            catch (Exception exc)
            {
               // myWebService.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName("MYCOnnection"));
            }
        }

        public  void open(ConnectionOption option, String connectionString)
        {
            try
            {
                if (option == ConnectionOption.SQL)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                    }
                    else
                    {
                        conn = new SqlConnection();
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ToString();
                        conn.Open();
                    }
                }
                else if (option == ConnectionOption.MSAccess)
                {

                    oConn = new OleDbConnection();
                    oConn.ConnectionString = connectionString;
                    oConn.Open();
                }
            }
            catch (Exception exc)
            {
                //myWebService.writeLogs("Not able to connect database", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.    GetFileName(Request.Url.AbsolutePath));
            }
        }

        public  void close(ConnectionOption option)
        {
            try
            {
                if (option == ConnectionOption.SQL)
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                    }
                    else
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                if (option == ConnectionOption.MSAccess)
                {
                    if (oConn.State == ConnectionState.Closed)
                    {
                    }
                    else
                    {
                        oConn.Close();
                        oConn.Dispose();
                    }
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}