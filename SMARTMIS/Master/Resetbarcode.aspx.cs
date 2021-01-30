using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.SqlClient;
using SmartMIS.SmartWebReference;
using System.Web.SessionState;

namespace SmartMIS.Report
{
    public partial class Resetbarcode : System.Web.UI.Page
    {
        string moduleName = "Resetbarcode";
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {

                }
                catch (Exception ex)
                { }
            }
        }
        //public HttpSessionState Session { get; }
        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserId"] != null)
                { 
                int user_id = Convert.ToInt32( Session["UserId"].ToString());
                int flag = 0;
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                {
                  
                    DataTable dt = new DataTable();
                    string CheckCode = barcodeTextBox.Text.Trim();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select * from curingpcr where gtbarcode='" + CheckCode + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader();

                      while (myConnection.reader.Read())
                      {
                          flag = 1;
                         
                      }

                      myConnection.reader.Close();
                     // myConnection.comm.Dispose();
                      if (flag == 1)
                      {
                          myConnection.comm = myConnection.conn.CreateCommand();
                          myConnection.comm.CommandText = "insert into UpdateBarcode([wcID],[gtbarCode],[pressbarCode],[serialNo],[recipeID],[mouldNo],[manningID],[dtandTime],[updatestatus] ,[cycleUpdate],Currentdate,Userid) (SELECT [wcID],[gtbarCode],[pressbarCode],[serialNo],[recipeID],[mouldNo],[manningID],[dtandTime],[updatestatus] ,[cycleUpdate],getDate(),'" + user_id + "' FROM curingpcr  WHERE gtbarcode='" + CheckCode + "')Delete From curingPCR WHERE (gtbarcode = '" + CheckCode + "')";
                          flag = myConnection.comm.ExecuteNonQuery();
                          Notify(1, "Record deleted successfully");
                          //rNotifyLabel.Visible = true;
                          myConnection.comm.Dispose();
                          myConnection.close(ConnectionOption.SQL);
                      }
                      else
                      {
                          Notify(1, "Record not found");
                      }
                  
                }
                else 
                {
                    Notify(0, "Reset Not Done.Please Login With Admin User");
                    //rNotifyLabel.Text = "Reset Not Done.!!Please Login With Admin User";
                    //rNotifyLabel.Visible = true;
                }

                }
            }
            catch (Exception exp)
            {
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
          
            }
            finally
            {
               
               
            }
        }
        private int validation()
        {

            int flag = 0;

            if (barcodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }
        private void Notify(int notifyIcon, string notifyMessage)
        {

            if (notifyIcon == 0)
            {
                rNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                rNotifyImage.Src = "../Images/tick.png";
            }
            rNotifyLabel.Text = notifyMessage;

            oemNotifyMessageDiv.Visible = true;
            oemNotifyTimer.Enabled = true;
           
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            oemNotifyMessageDiv.Visible = false;
            
        }
       
    }
}
