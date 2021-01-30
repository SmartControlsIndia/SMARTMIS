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
    public partial class unit : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function
        string moduleName = "unitMaster";

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
                    if (((Button)sender).ID == "unitSaveButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                        {
                            save();
                            fillGridView();
                            clearPage();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(unitCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }
                    }
                    else if (((Button)sender).ID == "unitCancelButton")
                    {
                        clearPage();
                    }
                    else if (((Button)sender).ID == "unitDialogOKButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                        {
                            unitIDLabel.Text = unitIDHidden.Value; //Passing value to unitIDLabel because on postback hidden field retains its value
                            delete();
                            fillGridView();
                            clearPage();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(unitCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (((ImageButton)sender).ID == "unitGridEditImageButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                    {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    unitIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("unitGridIDLabel")).Text);
                    unitNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("unitGridNameLabel")).Text);
                    unitDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("unitGridDescriptionLabel")).Text);
                     }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(unitCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }


                }
                else if (((ImageButton)sender).ID == "unitGridDeleteImageButton")
                {
                    //Code for deleting gridview row

                }
                else
                {
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
            {
                unitNotifyMessageDiv.Visible = false;
                unitNotifyTimer.Enabled = false;
            }

        #endregion

        #region User Defined Function

            private void fillGridView()
            {

                //Description   : Function for filling unitGridView
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                try
                {
                    unitGridView.DataSource = myWebService.fillGridView("Select * from vUnit", "iD", smartMISWebService.order.Desc);
                    unitGridView.DataBind();
                }
                catch(Exception ex)
                {

                }
            }

            private int save()
            {
                //Description   : Function for saving and updating record in unitMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;
                int notifyIcon = 0;
                try
                {
                    if (unitIDLabel.Text.Trim() == "0")
                    {
                        if ((validation() <= 0) && (myWebService.IsRecordExist("unitMaster", "name", "where name='" + unitNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                        {

                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into unitMaster (name, description) values (@name, @description)";
                            myConnection.comm.Parameters.AddWithValue("@name", unitNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", unitDescriptionTextBox.Text.Trim());

                            flag = myConnection.comm.ExecuteNonQuery();
                            Notify(1, "Work Center saved successfully");



                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                        }
                    }
                    else if (unitIDLabel.Text.Trim() != "0")
                    {
                        if (validation() <= 0)
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Update unitMaster SET name = @name, description = @description WHERE (iD = @iD)";
                            myConnection.comm.Parameters.AddWithValue("@name", unitNameTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@description", unitDescriptionTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@iD", unitIDLabel.Text.Trim());

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Unit updated successfully");
                        }


                        else
                        {
                            Notify(notifyIcon, "Unit already exists");
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                return flag;
            }

            private int delete()
            {

                //Description   : Function for deleting record in unitMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;
                try
                {

                    if (unitIDLabel.Text.Trim() != "0")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Delete From unitMaster WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@iD", unitIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Unit deleted successfully");
                    }
                }
                catch(Exception ex)
                {

                }
                return flag;
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in unitMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (unitNameTextBox.Text.Trim() == "")
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

                unitIDLabel.Text = "0";
                unitNameTextBox.Text = "";
                unitDescriptionTextBox.Text = "";
            }

            private void Notify(int notifyIcon, string notifyMessage)
            {

                //Description   : Function for showing notify information in unitMessageDiv
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                //Condition 0   : Nothing
                //Condition 1   : Insertion
                //Condition 2   : Updation
                //Condition 3   : Deletion
                //Condition 3   : Error

                if (notifyIcon == 0)
                {
                    unitNotifyImage.Src = "../Images/notifyCircle.png";
                }
                else if (notifyIcon == 1)
                {
                    unitNotifyImage.Src = "../Images/tick.png";
                }
                unitNotifyLabel.Text = notifyMessage;

                unitNotifyMessageDiv.Visible = true;
                unitNotifyTimer.Enabled = true;
            }

        #endregion
    }
}
