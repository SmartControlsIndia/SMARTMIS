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

namespace SmartMIS
{
    public partial class _Default : System.Web.UI.Page
    {

        #region System Defined Function
       // smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();


            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    if (Session["ID"].ToString() == "0")
                    {
                        loginUserNameTextBox.Focus();
                    }
                    else
                    {
                        if (Request.QueryString.Count == 0)
                        {
                            Response.Redirect("/SMARTMIS/Home/home.aspx", true);
                        }
                        else
                        {
                            Response.Redirect(Server.UrlDecode(Request.QueryString["page"].ToString()), true);
                        }
                    }
                }
                //System.Threading.Thread.Sleep(5000);
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                try
                {
                    if (authenticateUser(loginUserNameTextBox.Text.Trim(), loginPasswordTextBox.Text.Trim()))
                    {

                        if (Request.QueryString.Count == 0)
                        {
                            Response.Redirect("/SMARTMIS/Home/home.aspx", true);
                        }
                        else
                        {
                            Response.Redirect(Server.UrlDecode(Request.QueryString["page"].ToString()), true);
                        }
                    }
                    else
                    {
                        loginErrorLabel.Visible = true;
                        loginErrorLabel.Text = "Invalid Usename or Password.";
                    }
                }
                catch (Exception exp)
                {
                    //myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }

        #endregion

        #region User Defined Function

            private bool authenticateUser(string uID, string pass)
            {
                bool flag = false;
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from [vUserDetails] where [userID]= @userID and [password] = @password and accountActivate = 1";
                    myConnection.comm.Parameters.AddWithValue("@userID", uID);
                    myConnection.comm.Parameters.AddWithValue("@password", pass);
                   // myConnection.comm.Parameters.AddWithValue("@accounActivate", accounActivate);

                    myConnection.reader = myConnection.comm.ExecuteReader();

                    while (myConnection.reader.Read())
                    {
                        Session["ID"] = myConnection.reader[5].ToString();
                        Session["userID"] = uID;
                        Session["userName"] = myConnection.reader[3].ToString() + " " + myConnection.reader[4].ToString();
                        flag = true;
                    }
                }
                catch (Exception exp)
                {
                    //myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); 
                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }


                return flag;
            }
            private void getroles(string userID)
            {
                string userRoles="";

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select roleID from [vUserRoles] where [userID]=@userID";
                    myConnection.comm.Parameters.AddWithValue("@userID", userID);


                    myConnection.reader = myConnection.comm.ExecuteReader();

                    while (myConnection.reader.Read())
                    {
                        userRoles = userRoles + myConnection.reader["roleID"];
                        userRoles = userRoles + "#";

                    }
                    Session["userRoles"] = userRoles;
                }
                catch (Exception exp)
                {
                    //myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

 
            }

        #endregion

        

    }
}
