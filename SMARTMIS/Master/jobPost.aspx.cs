using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Master
{
    public partial class jobPost : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillWorkCenter();
                fillGridView();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
            myConnection.comm.Parameters.AddWithValue("@name", jobPostWCNameDropDownList.SelectedItem.ToString().Trim());

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                jobPostWockCenterIDLabel.Text = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "jobPostSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "jobPostCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "jobPostDialogOKButton")
            {
                jobPostIDLabel.Text = jobPostIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "jobPostGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                jobPostIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("jobPostGridIDLabel")).Text);
                jobPostNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("jobPostGridJobNameLabel")).Text);

                jobPostWockCenterIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("jobPostGridWorkCenterIDLabel")).Text);
                jobPostWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("jobPostGridWorkCenterNameLabel")).Text);

                jobPostDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("jobPostGridDescriptionLabel")).Text);



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
            jobPostNotifyMessageDiv.Visible = false;
            jobPostNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillWorkCenter()
        {

            //Description   : Function for filling jobPostWCNameDropDownList with WorkCenter Name
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            jobPostWCNameDropDownList.Items.Clear();
            jobPostWCNameDropDownList.Items.Add("");

            jobPostWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "");
            jobPostWCNameDropDownList.DataBind();
        }

        private void fillGridView()
        {

            //Description   : Function for filling jobPostGridView
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            jobPostGridView.DataSource = myWebService.fillGridView("Select * from vJobPost", "iD", smartMISWebService.order.Desc);
            jobPostGridView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in jobPostMaster Table
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            int flag = 0;
            int notifyIcon = 0;

            if (jobPostIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("jobPostMaster", "name", "WHERE name = '" + jobPostNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into jobPostMaster (wcID, name, description) values (@wcID, @name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(jobPostWockCenterIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", jobPostNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", jobPostDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(notifyIcon, "Job Post Name saved successfully");
                }
                else
                {
                    Notify(notifyIcon, "Job Post Name already exists");
                }
            }
            else if (jobPostIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update jobPostMaster SET wcID = @wcID, name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(jobPostWockCenterIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", jobPostNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", jobPostDescriptionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@iD", jobPostIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Job Post updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in jobPostMaster Table
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            int flag = 0;

            if (jobPostIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From jobPostMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", jobPostIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Job Post deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in jobPostMaster Table
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              
            int flag = 0;

            if (jobPostWockCenterIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (jobPostNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            jobPostIDLabel.Text = "0";
            jobPostNameTextBox.Text = "";
            fillWorkCenter();
            jobPostWockCenterIDLabel.Text = "0";
            jobPostDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in jobPostMessageDiv
            //Author        : Brajesh Kumar  
            //Date Created  : 02 july 2011 
            //Date Updated  : 
            //Revision No.  : 01           
            //Revision Desc :              

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                jobPostNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                jobPostNotifyImage.Src = "../Images/tick.png";
            }
            jobPostNotifyLabel.Text = notifyMessage;

            jobPostNotifyMessageDiv.Visible = true;
            jobPostNotifyTimer.Enabled = true;
        }

        #endregion
    }
}
