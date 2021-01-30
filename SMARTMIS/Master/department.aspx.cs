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
    public partial class department : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "DepartmentMaster";
        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGridView();
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {

                if (((Button)sender).ID == "deptSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(deptCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
                else if (((Button)sender).ID == "deptCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "deptDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        deptIDLabel.Text = deptIDHidden.Value; //Passing value to roleIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(deptCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (((ImageButton)sender).ID == "deptGridEditImageButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                    {
                        //Code for editing gridview row
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        deptIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("deptGridIDLabel")).Text);
                        deptNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("deptGridNameLabel")).Text);
                        deptDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("deptGridDescriptionLabel")).Text);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(deptCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
                else if (((ImageButton)sender).ID == "deptGridDeleteImageButton")
                {
                    //Code for deleting gridview row

                }
                else
                {
                }
            }
            catch(Exception exp)
            {

            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            deptNotifyMessageDiv.Visible = false;
            deptNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling depapartmentGridView
            //Author        : Brajesh Kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01
            try
            {
                deptGridView.DataSource = myWebService.fillGridView("Select * from vDepartmentMaster", "iD", smartMISWebService.order.Desc);
                deptGridView.DataBind();
            }
            catch(Exception exp)
            {

            }
        }    

        private int save()
        {
            //Description   : Function for saving and updating record in departmentMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;
            if (deptIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("deptMaster", "name", "where name='" + deptNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into deptMaster (name, description) values (@name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@name", deptNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", deptDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Department saved successfully");
                }

               else

                {
                    Notify(notifyIcon, "department already exists");
                }
            }
            else if (deptIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update deptMaster SET name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@name", deptNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", deptDescriptionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@iD", deptIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Department updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in departmentMaster Table
            //Author        : Brajesh KUmar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (deptIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From deptMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", deptIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Department deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in departmentMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (deptNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            deptIDLabel.Text = "0";
            deptNameTextBox.Text = "";
            deptDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in departmentMessageDiv
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
                deptNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                deptNotifyImage.Src = "../Images/tick.png";
            }
            deptNotifyLabel.Text = notifyMessage;

            deptNotifyMessageDiv.Visible = true;
            deptNotifyTimer.Enabled = true;
        }

        #endregion
        
    }
}
