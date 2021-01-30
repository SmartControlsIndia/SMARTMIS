using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Data;

namespace SmartMIS
{
    public partial class userManagement : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        string dbOldPassword, textPassword;
        SQL.SQLQueryClass obj = null;
        string moduleName = "UserManagement";
        ArrayList colValues = new ArrayList();
        ArrayList colNames = new ArrayList();
        string connString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
        smartMISWebService myWebService = new smartMISWebService();
      

        protected void Page_Load(object sender, EventArgs e)
        {
            obj = new SQL.SQLQueryClass();
            if (!Page.IsPostBack)
            {
                try
                {
                    fillGridView();
                }
                catch(Exception ex)
                {
                }
            }    
        }

        private void fillGridView()
        {
            try
            {
                umGridView.DataSource = myWebService.fillGridView("Select iD, userID, manningID from userDetails where accountStatus=1", ConnectionOption.SQL);
                umGridView.DataBind();
            }
            catch(Exception ex)
            {

            }
                
        }

        protected void Gridview_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label userID = ((Label)e.Row.FindControl("umUserIDGridNameLabel"));
                    BulletedList bl = (BulletedList)e.Row.FindControl("bulletedListRoles");

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select distinct roleID from vUserRoles where userID='" + userID.Text + "'";
                    SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    bl.DataSource = ds;
                    bl.DataBind();
                }
                catch(Exception ex)
                {

                }
            }                
        }       

        protected void editButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SmartMIS/UserManagement/userRegistration.aspx");
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "userManagemetSavePasswordButton")
                {
                    if (getDBPassword())
                    {
                        colNames.Add("password");
                        colValues.Add(userManagemetNewPasswordTextBox.Text);
                        string condition = "userID='" + Session["userID"] + "'";
                        obj.getConnectionString(ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString());
                        string result = obj.updateQuery("userDetails", colNames, colValues, condition);
                        if (result.Equals("UPD100"))
                        {
                            Notify(1, "Password Changed Successfully");
                        }
                    }
                    else
                    {
                        Notify(0, "Your old password does not match! Please try again.");
                    }
                }
                else if (((Button)sender).ID == "umDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        umIDLabel.Text = umIDHidden.Value;
                        delete();
                        fillGridView();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(umDialogCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);

                    }
                }
            }
            catch(Exception epx)
            {

            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (((ImageButton)sender).ID == "usermgmtEditButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;
                        mannIDHidden.Value = (((Label)gridViewRow.Cells[1].FindControl("umUserMannIDGridNameLabel")).Text);

                        Response.Redirect("/SMARTMIS/UserManagement/userRegistration.aspx?u9873518=" + mannIDHidden.Value);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(umDialogCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);

                    }
                }
            }
            catch (Exception exep)
            { 
            }
        }

        private void delete()
        {
            string flag = "";

            if (umIDLabel.Text.Trim() != "0")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select userID from userDetails WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", umIDLabel.Text.Trim());
                   myConnection.reader= myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        flag = myConnection.reader[0].ToString();
                    }
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();

                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "delete userRoles WHERE (userID = @flag)";
                    myConnection.comm.Parameters.AddWithValue("@flag", flag);
                    myConnection.comm.ExecuteNonQuery();
                    myConnection.comm.Dispose();
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "delete userDetails WHERE (userID = @flag)";
                    myConnection.comm.Parameters.AddWithValue("@flag", flag);
                    myConnection.comm.ExecuteNonQuery();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    Notify(1, "User deleted successfully!");

                }
                catch (Exception ex)
                {
 
                }
            }

        }
       

        private bool getDBPassword()
        {
            textPassword = userManagemetOldPasswordTextBox.Text;
            SqlCommand cmd = new SqlCommand("select password from userDetails where userID='" +Session["userID"] + "'", new SqlConnection(connString));
            cmd.Connection.Open();
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                dbOldPassword = rd[0].ToString();
            }
            else
                dbOldPassword = "";

            if (dbOldPassword.Equals(textPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {
            if (notifyIcon == 0)
            {
                passChangeNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                passChangeNotifyImage.Src = "../Images/tick.png";
            }
            passChangeNotifyLabel.Text = notifyMessage;

            passChangeNotifyMessageDiv.Visible = true;
            passChangeNotifyTimer.Enabled = true;
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            passChangeNotifyMessageDiv.Visible = false;
            passChangeNotifyTimer.Enabled = false;
        }



    }
}
