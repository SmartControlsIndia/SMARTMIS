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
    public partial class workCenter : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "WorkcenterMaster";

        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    fillProcessName();
                    fillGridView();
                }
            }


            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    ((DropDownList)sender).Items.Remove("".Trim());

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from processMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", wcProcessNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        wcProcessIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
                catch(Exception ex)
                {

                }


            }

            protected void Button_Click(object sender, EventArgs e)
            {
                if (((Button)sender).ID == "wcSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(wcCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
                else if (((Button)sender).ID == "wcCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "wcDialogOKButton")
                {
                    wcIDLabel.Text = wcIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                    delete();
                    fillGridView();
                    clearPage();
                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
               
                    if (((ImageButton)sender).ID == "wcGridEditImageButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                        {
                        //Code for editing gridview row
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        wcIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("wcGridIDLabel")).Text);
                        wcNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("wcGridWCNameLabel")).Text);

                        wcProcessIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("wcGridProcessIDLabel")).Text);
                        wcProcessNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("wcGridProcessNameLabel")).Text);

                        wcDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("wcGridDescriptionLabel")).Text);

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(wcCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }

                    }
                    else if (((ImageButton)sender).ID == "wcGridDeleteImageButton")
                    {
                        //Code for deleting gridview row

                    }
                    else
                    {
                    }
                }
               
            
         

            protected void NotifyTimer_Tick(object sender, EventArgs e)
            {
                wcNotifyMessageDiv.Visible = false;
                wcNotifyTimer.Enabled = false;
            }

        #endregion

        #region User Defined Function
            
            private void fillProcessName()
            {

                //Description   : Function for filling wcProcessNameDropDownList with Process Name
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 26 March 2011 
                //Date Updated  : 26 March 2011 || 01 April 2011
                //Revision No.  : 01            || 02
                //Revision Desc :               || Change the Logic for filling the wcProcessNameDropDownList
                try
                {

                    wcProcessNameDropDownList.Items.Clear();
                    wcProcessNameDropDownList.Items.Add("");

                    wcProcessNameDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name");
                    wcProcessNameDropDownList.DataBind();
                }
                catch(Exception ex)
                {

                }
            }
            public bool isAuthenticate(int validationtype)
            {
                return myWebService.uservalidation(Session["userID"].ToString(),this.moduleName,validationtype);
            }


            private void fillGridView()
            {

                //Description   : Function for filling wcGridView
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 26 March 2011 ||
                //Date Updated  : 26 March 2011 || 11 April
                //Revision No.  : 01            || 02
                //Revision Desc :               || Change the logic for filling wcGridView by webservice
                try
                {
                    wcGridView.DataSource = myWebService.fillGridView("Select * from vWorkCenter", "iD", smartMISWebService.order.Desc);
                    wcGridView.DataBind();
                }
                catch (Exception exp)

                {
 
                }
            }
            
            private int save()
            {
                //Description   : Function for saving and updating record in wcMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 26 March 2011
                //Date Updated  : 26 March 2011
                //Revision No.  : 01

                int flag = 0;
                int notifyIcon = 0;

                if (wcIDLabel.Text.Trim() == "0")
                {
                    if ((validation() <= 0) && (myWebService.IsRecordExist("wcMaster", "name", "WHERE name = '" + wcNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into wcMaster (processID, name, description) values (@processID, @name, @description)";
                        myConnection.comm.Parameters.AddWithValue("@processID", Convert.ToInt32(wcProcessIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@name", wcNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", wcDescriptionTextBox.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(notifyIcon, "Work Center saved successfully");
                    }
                    else
                    {
                        Notify(notifyIcon, "Work Center already exists");
                    }
                }
                else if (wcIDLabel.Text.Trim() != "0")
                {
                    if (validation() <= 0)
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Update wcMaster SET processID = @processID, name = @name, description = @description WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@processID", Convert.ToInt32(wcProcessIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@name", wcNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", wcDescriptionTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@iD", wcIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Work Center updated successfully");
                    }
                }

                return flag;
            }

            private int delete()
            {

                //Description   : Function for deleting record in wcMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 26 March 2011
                //Date Updated  : 26 March 2011
                //Revision No.  : 01
                int flag = 0;
                try
                {
                    

                    if (wcIDLabel.Text.Trim() != "0")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Delete From wcMaster WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@iD", wcIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Work Center deleted successfully");
                    }
                }
                catch(Exception ex)
                {
                }

                return flag;
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in wcMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 26 March 2011
                //Date Updated  : 26 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (wcProcessIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (wcNameTextBox.Text.Trim() == "")
                {
                    flag = 1;
                }

                return flag;
            }

            private void clearPage()
            {

                //Description   : Function for clearing controls and variables of Page
                //Author        : Brajesh kumar
                //Date Created  : 26 March 2011
                //Date Updated  : 26 March 2011
                //Revision No.  : 01

                wcIDLabel.Text = "0";
                wcNameTextBox.Text = "";
                fillProcessName();
                wcProcessIDLabel.Text = "0";
                wcDescriptionTextBox.Text = "";
            }

            private void Notify(int notifyIcon, string notifyMessage)
            {

                //Description   : Function for showing notify information in wcMessageDiv
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
                    wcNotifyImage.Src = "../Images/notifyCircle.png";
                }
                else if (notifyIcon == 1)
                {
                    wcNotifyImage.Src = "../Images/tick.png";
                }
                wcNotifyLabel.Text = notifyMessage;

                wcNotifyMessageDiv.Visible = true;
                wcNotifyTimer.Enabled = true;
            }

        #endregion
    }
}
