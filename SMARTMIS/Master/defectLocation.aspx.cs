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
    public partial class defectLocationMaster : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

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
                if (((Button)sender).ID == "defectSaveButton")
                {
                    save();
                    fillGridView();
                    clearPage();
                }
                else if (((Button)sender).ID == "defectCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "defectDialogOKButton")
                {
                    defectIDLabel.Text = defectIDHidden.Value; //Passing value to defectIDLabel because on postback hidden field retains its value
                    delete();
                    fillGridView();
                    clearPage();
                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (((ImageButton)sender).ID == "defectGridEditImageButton")
                {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    defectIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("defectGridIDLabel")).Text);
                    defectNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("defectGridNameLabel")).Text);
                    defectDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("defectGridDescriptionLabel")).Text);



                }
                else if (((ImageButton)sender).ID == "defectGridDeleteImageButton")
                {
                    //Code for deleting gridview row

                }
                else
                {
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
            {
                defectNotifyMessageDiv.Visible = false;
                defectNotifyTimer.Enabled = false;
            }

        #endregion

        #region User Defined Function

            private void fillGridView()
            {

                //Description   : Function for filling defectLocationGridView
                //Author        : Shashank Jain
                //Date Created  : 27 June 2011
                //Date Updated  : 27 June 2011
                //Revision No.  : 01

                defectGridView.DataSource = myWebService.fillGridView("Select * from defectLocationMaster", "iD", smartMISWebService.order.Desc);
                defectGridView.DataBind();
            }

            private int save()
            {
                //Description   : Function for saving and updating record in defectMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;
                int notifyIcon = 0;
                if (defectIDLabel.Text.Trim() == "0")
                {
                    if ((validation() <= 0) && (myWebService.IsRecordExist("defectLocationMaster", "name", "where name='" + defectNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into defectLocationMaster (name, description) values (@name, @description)";
                        myConnection.comm.Parameters.AddWithValue("@name", defectNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", defectDescriptionTextBox.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Defect Location saved successfully");
                    }
                }
                else if (defectIDLabel.Text.Trim() != "0")
                {
                    if (validation() <= 0)
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Update defectLocationMaster SET name = @name, description = @description WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@name", defectNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", defectDescriptionTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@iD", defectIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Defect Location updated successfully");
                    }


                    else
                    {
                        Notify(notifyIcon, "Defect Location already exists");
                    }
                }

                return flag;
            }

            private int delete()
            {

                //Description   : Function for deleting record in defectMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (defectIDLabel.Text.Trim() != "0")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From defectLocationMaster WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", defectIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Defect Location deleted successfully");
                }

                return flag;
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in defectMaster Table
                //Author        : Brajesh kumar
                //Date Created  : 27 March 2011
                //Date Updated  : 27 March 2011
                //Revision No.  : 01

                int flag = 0;

                if (defectNameTextBox.Text.Trim() == "")
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

                defectIDLabel.Text = "0";
                defectNameTextBox.Text = "";
                defectDescriptionTextBox.Text = "";
            }

            private void Notify(int notifyIcon, string notifyMessage)
            {

                //Description   : Function for showing notify information in defectMessageDiv
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
                    defectNotifyImage.Src = "../Images/notifyCircle.png";
                }
                else if (notifyIcon == 1)
                {
                    defectNotifyImage.Src = "../Images/tick.png";
                }
                defectNotifyLabel.Text = notifyMessage;

                defectNotifyMessageDiv.Visible = true;
                defectNotifyTimer.Enabled = true;
            }

        #endregion
    }
}
