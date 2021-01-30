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
    public partial class MHE : System.Web.UI.Page
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
            if (((Button)sender).ID == "mheSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "mheCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "mheDialogOKButton")
            {
                mheIDLabel.Text = mheIDHidden.Value; //Passing value to mheIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "mheGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                mheIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("mheGridIDLabel")).Text);
                mheCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("mheGridNameLabel")).Text);
                mheDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("mheGridDescriptionLabel")).Text);



            }
            else if (((ImageButton)sender).ID == "mheGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            mheNotifyMessageDiv.Visible = false;
            mheNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling mheGridView
            //Author        : Brajesh Kumar
            //Date Created  : 1 April 2011
            //Date Updated  : 1 April 2011
            //Revision No.  : 01

            mheGridView.DataSource = myWebService.fillGridView("Select * from vMhe", "iD", smartMISWebService.order.Desc);
            mheGridView.DataBind();


        }

        private int save()
        {
            //Description   : Function for saving and updating record in unitMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 1 April 2011
            //Date Updated  :1 April 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;
            if (mheIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("mheMaster", "name", "where name='" + mheCodeTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into mheMaster (name, description) values (@name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@name", mheCodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", mheDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "MHE saved successfully");
                }
                else
                {
                    Notify(notifyIcon, "Mhe already exists");
                }

            }
            else if (mheIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update mheMaster SET name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@name", mheCodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", mheDescriptionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@iD", mheIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "MHE updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in mheMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 1 April 2011
            //Date Updated  : 1 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (mheIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From mheMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", mheIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "MHE deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in unitMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 1 April 2011
            //Date Updated  : 1 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (mheCodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 1 April 2011
            //Date Updated  :1 April 2011
            //Revision No.  : 01

            mheIDLabel.Text = "0";
            mheCodeTextBox.Text = "";
            mheDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in mheMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 1 April 20111 April 20111 April 20111 April 20111 April 20111 April 2011
            //Date Updated  : 1 April 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                mheNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                mheNotifyImage.Src = "../Images/tick.png";
            }
            mheNotifyLabel.Text = notifyMessage;

            mheNotifyMessageDiv.Visible = true;
            mheNotifyTimer.Enabled = true;
        }

        #endregion
    }
}
