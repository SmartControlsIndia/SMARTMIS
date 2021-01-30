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
using SmartMIS.SmartWebReference;

namespace SmartMIS
{
    public partial class _tbmDefault : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    if (Session["ID"].ToString() == "0")
                    {
                        loginUserNameOP1TextBox.Focus();
                    }
                    else
                    {
                        if (Request.QueryString.Count == 0)
                        {
                            Response.Redirect("/SmartMIS/Home/home.aspx", true);
                        }
                        else
                        {
                            Response.Redirect(Server.UrlDecode(Request.QueryString["page"].ToString()), true);
                        }
                    }
                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                if (authenticateUser(loginUserNameOP1TextBox.Text.Trim(), loginPasswordOP1TextBox.Text.Trim()))
                {
                    if (Request.QueryString.Count == 0)
                    {
                        Response.Redirect("/SmartMIS/Home/home.aspx", true);
                    }
                    else
                    {
                        Response.Redirect(Server.UrlDecode(Request.QueryString["page"].ToString()), true);
                    }
                }
                else
                {
                    loginErrorOP1Label.Visible = true;
                    loginErrorOP1Label.Text = "Invalid Usename or Password.";
                }
            }

        #endregion

        #region User Defined Function

            private bool authenticateUser(string uID, string pass)
            {
                bool flag = false;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from [vUserDetails] where [userID]= @userID and [password] = @password";
                myConnection.comm.Parameters.AddWithValue("@userID", uID);
                myConnection.comm.Parameters.AddWithValue("@password", pass);

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    Session["ID"] = myConnection.reader[5].ToString();
                    Session["userID"] = uID;
                    Session["userName"] = myConnection.reader[3].ToString() + " " + myConnection.reader[4].ToString();
                    flag = true;
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                return flag;
            }

        #endregion

        

    }
}
