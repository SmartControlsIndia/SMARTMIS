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
    public partial class category : System.Web.UI.Page
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
            if (((Button)sender).ID == "categorySaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "categoryCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "categoryDialogOKButton")
            {
                categoryIDLabel.Text = categoryIDHidden.Value; //Passing value to categoryIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "categoryGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                categoryIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("categoryGridIDLabel")).Text);
                categoryNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("categoryGridNameLabel")).Text);
                categoryDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("categoryGridDescriptionLabel")).Text);



            }
            else if (((ImageButton)sender).ID == "categoryGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            categoryNotifyMessageDiv.Visible = false;
            categoryNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling categoryGridView
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            categoryGridView.DataSource = myWebService.fillGridView("Select * from vCategoryMaster", "iD", smartMISWebService.order.Desc);
            categoryGridView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in categoryMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;
            if (categoryIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("categoryMaster", "name", "where name='" + categoryNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into categoryMaster (name, description) values (@name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@name", categoryNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", categoryDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Category saved successfully");
                }


                else
                {
                    Notify(notifyIcon, "Category already exists");
                }
            }
            else if (categoryIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update categoryMaster SET name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@name", categoryNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", categoryDescriptionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@iD", categoryIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Category updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in categoryMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (categoryIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From categoryMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", categoryIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Category deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in categoryMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (categoryNameTextBox.Text.Trim() == "")
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

            categoryIDLabel.Text = "0";
            categoryNameTextBox.Text = "";
            categoryDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in categoryMessageDiv
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
                categoryNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                categoryNotifyImage.Src = "../Images/tick.png";
            }
            categoryNotifyLabel.Text = notifyMessage;

            categoryNotifyMessageDiv.Visible = true;
            categoryNotifyTimer.Enabled = true;
        }

        #endregion

    

    }
}
