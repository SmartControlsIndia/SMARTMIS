using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Data;
using SmartMIS.SmartWebReference;

namespace SmartMIS
{
    public partial class _event : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillCategoryName();
                fillGridView();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select * from categoryMaster Where name = @name";
            myConnection.comm.Parameters.AddWithValue("@name", eventCategoryNameDropDownList.SelectedItem.ToString().Trim());

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                eventCategoryIDLabel.Text = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "eventSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "eventCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "eventDialogOKButton")
            {
                eventIDLabel.Text = eventIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "eventGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                eventIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("eventGridIDLabel")).Text);
                eventIDTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("eventIDGridIDLabel")).Text);

                eventCategoryNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("eventCategoryGridNameLabel")).Text);

                eventCategoryIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("eventCategoryIDGridIDLabel")).Text);
                eventNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("eventGridNameLabel")).Text);

                eventDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("eventGridDescriptionLabel")).Text);



            }
            else if (((ImageButton)sender).ID == "eventGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            eventNotifyMessageDiv.Visible = false;
           eventNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillCategoryName()
        {

            //Description   : Function for filling wcProcessNameDropDownList with Process Name
            //Author        : Brajesh kumar
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            eventCategoryNameDropDownList.Items.Clear();
            eventCategoryNameDropDownList.Items.Add("");

            eventCategoryNameDropDownList.DataSource = myWebService.FillDropDownList("categoryMaster", "name");
            eventCategoryNameDropDownList.DataBind();
        }

        private void fillGridView()
        {

            //Description   : Function for filling wcGridView
            //Author        : Brajesh kumar
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            eventGridView.DataSource = myWebService.fillGridView("Select * from vEventMaster", "iD", smartMISWebService.order.Desc);
            eventGridView.DataBind();
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
            if (eventIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("eventMaster", "name", "where name='" + eventNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into eventMaster (eventID, categoryID, name, description) values (@eventID, @categoryID, @name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@eventID", eventIDTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@categoryID", Convert.ToInt32(eventCategoryIDLabel.Text.Trim()));

                    myConnection.comm.Parameters.AddWithValue("@name", eventNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", eventDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Event saved successfully");
                }

                else
                {
                    Notify(notifyIcon, "Event already exists");
                }
            }
            else if (eventIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update eventMaster SET eventID = @eventID,categoryID=@categoryID, name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@eventID",eventIDTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@categoryID", Convert.ToInt32(eventCategoryIDLabel.Text.Trim()));

                    myConnection.comm.Parameters.AddWithValue("@name", eventNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", eventDescriptionTextBox.Text.Trim());

                    myConnection.comm.Parameters.AddWithValue("@iD", eventIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Event updated successfully");
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

            if (eventIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From eventMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", eventIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Event deleted successfully");
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

            if (eventCategoryIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (eventNameTextBox.Text.Trim() == "")
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

            eventIDLabel.Text = "0";
            eventIDTextBox.Text = "";
            eventNameTextBox.Text = "";
            fillCategoryName();
            eventCategoryIDLabel.Text = "0";
            eventDescriptionTextBox.Text = "";
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
                eventNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                eventNotifyImage.Src = "../Images/tick.png";
            }
            eventNotifyLabel.Text = notifyMessage;

            eventNotifyMessageDiv.Visible = true;
            eventNotifyTimer.Enabled = true;
        }

        #endregion

    }
}
