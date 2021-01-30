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
    public partial class AreaCenter : System.Web.UI.Page
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
            if (((Button)sender).ID == "areaSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "areaCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "areaDialogOKButton")
            {
                areaIDLabel.Text = areaIDHidden.Value; //Passing value to areaIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "areaGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;
                areaIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("areaGridIDLabel")).Text);
                areaNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("areaGridNameLabel")).Text);
                areaDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("areaGridDescriptionLabel")).Text);
            }
            else if (((ImageButton)sender).ID == "areaGridDeleteImageButton")
            {
                //Code for deleting gridview row
            }

            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
           areaNotifyMessageDiv.Visible = false;
           areaNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling areaGridView
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01


            areaGridView.DataSource = myWebService.fillGridView("Select * from vArea", "iD", smartMISWebService.order.Desc);
            areaGridView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in areaMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;
            if (areaIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("areaMaster", "name", "where name='" + areaNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into areaMaster (name, description) values (@name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@name", areaNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", areaDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Area saved successfully");
                }
                else
                {
                    Notify(notifyIcon, "Area already exists");
                }
            }
            else if (areaIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update areaMaster SET name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@name", areaNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", areaDescriptionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@iD", areaIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Area updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in areaMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (areaIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From areaMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", areaIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Area deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in AreaMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (areaNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            areaIDLabel.Text = "0";
            areaNameTextBox.Text = "";
            areaDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in areaMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                areaNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                areaNotifyImage.Src = "../Images/tick.png";
            }
            areaNotifyLabel.Text = notifyMessage;

            areaNotifyMessageDiv.Visible = true;
            areaNotifyTimer.Enabled = true;
        }

        #endregion
    }
}
