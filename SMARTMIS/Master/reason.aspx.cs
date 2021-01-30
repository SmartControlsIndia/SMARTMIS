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
    public partial class reason : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "ReasonMaster";
        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                  

                    fillWorkcenterName();
                    fillGridView();
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                ((DropDownList)sender).Items.Remove("".Trim());
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", reasonWCNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        reasonWCIDLabel.Text = myConnection.reader[0].ToString();
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

            }

            protected void Button_Click(object sender, EventArgs e)
            {
                try
                {
                    if (((Button)sender).ID == "reasonSaveButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                        {
                            save();
                            fillGridView();
                            clearPage();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(reasonCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }
                    }
                    else if (((Button)sender).ID == "reasonCancelButton")
                    {
                        clearPage();
                    }
                    else if (((Button)sender).ID == "reasonDialogOKButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                        {
                            reasonIDLabel.Text = reasonIDHidden.Value; //Passing value to reasonIDLabel because on postback hidden field retains its value
                            delete();
                            fillGridView();
                            clearPage();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(reasonCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (((ImageButton)sender).ID == "reasonGridEditImageButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                    {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    reasonIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("reasonGridIDLabel")).Text);
                    reasonNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("reasonGridNameLabel")).Text);

                    reasonWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("reasonGridWCIDLabel")).Text);
                    reasonWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("reasonGridWCNameLabel")).Text);

                    reasonDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("reasonGridDescriptionLabel")).Text);

                    }
                      else
                    {
                        ScriptManager.RegisterClientScriptBlock(reasonCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
                else if (((ImageButton)sender).ID == "reasonGridDeleteImageButton")
                {
                    //Code for deleting gridview row

                }
                else
                {
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            reasonNotifyMessageDiv.Visible = false;
            reasonNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

            private void fillWorkcenterName()
            {

                //Description   : Function for filling reasonWCNameDropDownList with Workcenter Name
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 27 March 2011 
                //Date Updated  : 27 March 2011 || 01 April 2011
                //Revision No.  : 01            || 02
                //Revision Desc :               || Change the Logic for filling the reasonWCNameDropDownList
                try
                {
                    reasonWCNameDropDownList.Items.Clear();
                    reasonWCNameDropDownList.Items.Add("");

                    reasonWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "");
                    reasonWCNameDropDownList.DataBind();
                }
                catch(Exception exp)
                {

                }
            }

            private void fillGridView()
            {

                //Description   : Function for filling reasonGridView
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                reasonGridView.DataSource = myWebService.fillGridView("Select * from vReason", "iD", smartMISWebService.order.Desc);
                reasonGridView.DataBind();
            }

            private int save()
            {
                //Description   : Function for saving and updating record in reasonMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;
                int notifyIcon = 0;
                try
                {
                    if (reasonIDLabel.Text.Trim() == "0")
                    {
                        if ((validation() <= 0) && (myWebService.IsRecordExist("reasonMaster", "name", "where( name='" + reasonNameTextBox.Text.Trim() + "' AND wcID='" + reasonWCIDLabel.Text.Trim() + "')", out notifyIcon) == false))
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into reasonMaster (wcID, name, description) values (@wcID, @name, @description)";
                            myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(reasonWCIDLabel.Text.Trim()));
                            myConnection.comm.Parameters.AddWithValue("@name", reasonNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", reasonDescriptionTextBox.Text.Trim());

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Reason saved successfully");
                        }

                        else
                        {

                            Notify(notifyIcon, "Reason already exists for " + reasonWCNameDropDownList.SelectedValue.ToString().Trim());
                        }
                    }
                    else if (reasonIDLabel.Text.Trim() != "0")
                    {
                        if (validation() <= 0)
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Update reasonMaster SET wcID = @wcID, name = @name, description = @description WHERE (iD = @iD)";
                            myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(reasonWCIDLabel.Text.Trim()));
                            myConnection.comm.Parameters.AddWithValue("@name", reasonNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", reasonDescriptionTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@iD", reasonIDLabel.Text.Trim());

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Reason updated successfully");
                        }
                    }
                }
                catch(Exception exp)
                {

                }
                return flag;
            }

            private int delete()
            {

                //Description   : Function for deleting record in reasonMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (reasonIDLabel.Text.Trim() != "0")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From reasonMaster WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", reasonIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Reason deleted successfully");
                }

                return flag;
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in reasonMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (reasonWCIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (reasonNameTextBox.Text.Trim() == "")
                {
                    flag = 1;
                }

                return flag;
            }

            private void clearPage()
            {

                //Description   : Function for clearing controls and variables of Page
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                reasonIDLabel.Text = "0";
                reasonNameTextBox.Text = "";
                fillWorkcenterName();
                reasonWCIDLabel.Text = "0";
                reasonDescriptionTextBox.Text = "";
            }

            private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in reasonMessageDiv
            //Author        : Brajesh kumar
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                reasonNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                reasonNotifyImage.Src = "../Images/tick.png";
            }
            reasonNotifyLabel.Text = notifyMessage;

            reasonNotifyMessageDiv.Visible = true;
            reasonNotifyTimer.Enabled = true;
        }

        #endregion

    }
}
