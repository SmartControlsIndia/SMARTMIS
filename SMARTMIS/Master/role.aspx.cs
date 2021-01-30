using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using SmartMIS.SmartWebReference;

namespace SmartMIS
{
    public partial class role : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "UserRolesmaseter";
        string rolepermissions = "";
        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    View.Checked = true;
                    fillmodulename();
                    fillGridView();
                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                if (((Button)sender).ID == "roleSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(roleCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
                else if (((Button)sender).ID == "roleCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "roleDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                    roleIDLabel.Text = roleIDHidden.Value; //Passing value to roleIDLabel because on postback hidden field retains its value
                    delete();
                    fillGridView();
                    clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(roleCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                   
                }
            }
            protected void Gridview_RowBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        if (((GridView)sender).ID == "roleGridView")
                        {
                            Label roleNameLabel = ((Label)e.Row.FindControl("roleGridNameLabel"));
                            GridView childGridView = ((GridView)e.Row.FindControl("rolechildGridView"));

                            fillChildGridView(childGridView,roleNameLabel.Text.Trim());
                        }
                        if (((GridView)sender).ID == "rolechildGridView")
                        {
                            string temprights = "";
                            string tempquery = "";
                            Label moduleID = ((Label)e.Row.FindControl("roleGridmoduleIDLabel"));
                            BulletedList bl = (BulletedList)e.Row.FindControl("bulletedListRoles");
                            Label roleiD=((Label)e.Row.FindControl("roleGridIDLabel"));

                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();
                            myConnection.comm.CommandText = "select rights from vRoleMaster where moduleID='" + Convert.ToInt32(moduleID.Text) + "'and iD='" + Convert.ToInt32(roleiD.Text) + "' ";
                            myConnection.reader = myConnection.comm.ExecuteReader();
                            if (myConnection.reader.Read())
                            {
                                temprights = myConnection.reader[0].ToString();
                            }
                            myConnection.comm.Dispose();
                            myConnection.reader.Close();

                          tempquery=createQuery(temprights);

                          myConnection.comm = myConnection.conn.CreateCommand();
                          myConnection.comm.CommandText = "select name  from rightsMaster where "+ tempquery + "";
                          SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                          DataSet ds = new DataSet();
                          da.Fill(ds);

                          bl.DataSource = ds;
                          bl.DataBind();
                          myConnection.comm.Dispose();
                          myConnection.conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }       

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (((ImageButton)sender).ID == "roleGridEditImageButton")
                {
                    try
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                        {
                            clearPage();
                            string temprights = "";

                            //Code for editing gridview row

                            GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                            roleIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("roleGridIDLabel")).Text);
                            roleNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("rolechildGridNameLabel")).Text);
                            roleModuleIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("roleGridmoduleIDLabel")).Text);
                            roleModuleNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("roleGridModuleNameLabel")).Text);
                            roleDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("roleGridModuledescriptionLabel")).Text);
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();
                            myConnection.comm.CommandText = "select rights from vRoleMaster where moduleID='" + Convert.ToInt32(roleModuleIDLabel.Text) + "'and id='" + Convert.ToInt32(roleIDLabel.Text) + "'";
                            myConnection.reader = myConnection.comm.ExecuteReader();
                            if (myConnection.reader.Read())
                            {
                                temprights = myConnection.reader[0].ToString();
                            }
                            myConnection.comm.Dispose();
                            myConnection.reader.Close();
                            myConnection.conn.Close();
                            String[] temppermittion = temprights.Split(new char[] { '#' });
                            for (int i = 0; i <= temppermittion.Length - 1; i++)
                            {
                                if (temppermittion[i] == "1")
                                    View.Checked = true;
                                if (temppermittion[i] == "2")
                                    Add.Checked = true;
                                if (temppermittion[i] == "3")
                                    Edit.Checked = true;
                                if (temppermittion[i] == "4")
                                    Delete.Checked = true;
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(roleCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }
                    }
                    catch(Exception exp)
                    {

                    }

                }
                else if (((ImageButton)sender).ID == "roleGridDeleteImageButton")
                {
                    //Code for deleting gridview row

                }
                else
                {
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
            {
                roleNotifyMessageDiv.Visible = false;
                roleNotifyTimer.Enabled = false;
            }
            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    ((DropDownList)sender).Items.Remove("".Trim());

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from moduleMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", roleModuleNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        roleModuleIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
                catch(Exception ex)
                {
                }
            }


        #endregion

        #region User Defined Function

            public bool isAuthenticate(int validationtype)
            {
                return myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, validationtype);
            }
            private void fillChildGridView(GridView childGridView, string rolename)
            {
                if (childGridView.ID == "rolechildGridView")
                {
                    try
                    {
                        childGridView.DataSource = myWebService.fillGridView("Select iD,name, moduleID,moduleName,description from vRoleMaster WHERE name = '" + rolename + "' ", ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    catch(Exception xe)
                    {

                        
                    }
                }
            }

            private void fillGridView()
            {

                //Description   : Function for filling roleGridView 
                //Author        : Brajesh Kumar
                //Date Created  : 28 March 2011
                //Date Updated  : 28 March 2011
                //Revision No.  : 01

                roleGridView.DataSource = myWebService.fillGridView("Select distinct name from RoleMaster", "name", smartMISWebService.order.Asc);
                roleGridView.DataBind();
            }
            private void fillmodulename()
            {

                roleModuleNameDropDownList.Items.Clear();
                roleModuleNameDropDownList.Items.Add("");

                roleModuleNameDropDownList.DataSource = myWebService.FillDropDownList("moduleMaster", "name");
                roleModuleNameDropDownList.DataBind();
            }
            private string getpermissions()
            {
                rolepermissions = "";
                if (View.Checked)
                    rolepermissions = "1";
                if (Add.Checked)
                    if (rolepermissions != "")
                        rolepermissions = rolepermissions + "#" + 2;
                    else
                        rolepermissions = "2";
                if (Edit.Checked)
                    if (rolepermissions != "")
                        rolepermissions = rolepermissions + "#" + 3;
                    else
                        rolepermissions = "3";

                if (Delete.Checked)
                    if (rolepermissions != "")
                        rolepermissions = rolepermissions + "#" + 4;
                    else
                        rolepermissions = "4";

                return rolepermissions;
 
            }

            private int save()
            {
                //Description   : Function for saving and updating record in roleMaster Table
                //Author        : Brajesh Kumar
                //Date Created  : 28 March 2011
                //Date Updated  : 28 March 2011
                //Revision No.  : 01

                int flag = 0;
                int notifyIcon = 0;
                if (roleIDLabel.Text.Trim() == "0")
                {
                    try
                    {
                        if ((validation() <= 0) && (myWebService.IsRecordExist("roleMaster", "name", "where name='" + roleNameTextBox.Text.Trim() + "' and moduleID='" + roleModuleIDLabel.Text.Trim() + "'", out notifyIcon) == false))
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into roleMaster (name,moduleID, description,rights) values (@name,@moduleID, @description,@rights)";
                            myConnection.comm.Parameters.AddWithValue("@name", roleNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@moduleID", roleModuleIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", roleDescriptionTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@rights", getpermissions());


                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, " Role saved successfully");
                        }

                        else
                        {
                            Notify(notifyIcon, "Role already exists");
                        }
                    }
                    catch(Exception exp)
                    {

                    }
                }
                else if (roleIDLabel.Text.Trim() != "0")
                {
                    try
                    {
                        if (validation() <= 0)
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Update roleMaster SET name = @name, description = @description,moduleID=@moduleID,rights=@rights WHERE (iD = @iD)";
                            myConnection.comm.Parameters.AddWithValue("@name", roleNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", roleDescriptionTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@moduleID", roleModuleIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@rights", getpermissions());
                            myConnection.comm.Parameters.AddWithValue("@iD", roleIDLabel.Text.Trim());

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Role updated successfully");
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }

                return flag;
            }
            public string createQuery(string temprights)
            {
                string query = "";
                string or = "";
                string[] tempID = temprights.Split(new char[] { '#' });

                foreach (string items in tempID)
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

            private int delete()
            {

                //Description   : Function for deleting record in roleMaster Table
                //Author        : Brajesh Kumar
                //Date Created  : 28 March 2011
                //Date Updated  : 28 March 2011
                //Revision No.  : 01
                
                int flag = 0;
                try
                {

                    if (roleIDLabel.Text.Trim() != "0")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Delete From roleMaster WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@iD", roleIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Role deleted successfully");
                    }
                }
                catch (Exception ex)
                {
 
                }

                return flag;
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in roleMaster Table
                //Author        : Brajesh Kumar
                //Date Created  : 28 March 2011
                //Date Updated  : 28 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (roleNameTextBox.Text.Trim() == "")
                {
                    flag = 1;
                }

                return flag;
            }

            private void clearPage()
            {

                //Description   : Function for clearing controls and variables of Page
                //Author        : Brajesh Kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                roleIDLabel.Text = "0";
                roleNameTextBox.Text = "";
                roleDescriptionTextBox.Text = "";
                roleModuleIDLabel.Text = "0";
                fillmodulename();
                Add.Checked = false;
                Edit.Checked = false;
                Delete.Checked = false;
            }

            private void Notify(int notifyIcon, string notifyMessage)
            {

                //Description   : Function for showing notify information in roleMessageDiv
                //Author        : Brajesh Kumar
                //Date Created  : 28 March 2011
                //Date Updated  : 28 March 2011
                //Revision No.  : 01

                //Condition 0   : Nothing
                //Condition 1   : Insertion
                //Condition 2   : Updation
                //Condition 3   : Deletion
                //Condition 3   : Error

                if (notifyIcon == 0)
                {
                    roleNotifyImage.Src = "../Images/notifyCircle.png";
                }
                else if (notifyIcon == 1)
                {
                    roleNotifyImage.Src = "../Images/tick.png";
                }
                roleNotifyLabel.Text = notifyMessage;

                roleNotifyMessageDiv.Visible = true;
                roleNotifyTimer.Enabled = true;
            }

        #endregion

            protected void roleGridView_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

           
    }
}








      