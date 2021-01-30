using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Data;
using SmartMIS.SmartWebReference;
using System.IO;

namespace SmartMIS
{
    public partial class manning : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string moduleName = "ManningMaster";
        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillDepartmentName();
                
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

                myConnection.comm.CommandText = "Select * from deptMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", manningDepartmentNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    manningDeptIDLabel.Text = myConnection.reader[0].ToString();
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
                if (((Button)sender).ID == "manningSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(manningCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
                else if (((Button)sender).ID == "manningCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "manningDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        manningIDLabel.Text = manningIDHidden.Value; //Passing value to manningIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(manningCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "manningGridEditImageButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    manningIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridIDLabel")).Text);
                    manningDeptIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("manningDeptGridIDLabel")).Text);

                    manningFirstNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridFirstNameLabel")).Text);

                    manningLastNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridLastNameLabel")).Text);

                    manningGenderDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("manningGridGenderLabel")).Text);
                    manningDOBCalenderTextBox.Text = formatDate((((Label)gridViewRow.Cells[1].FindControl("manningGridDOBLabel")).Text));
                    manningAddressTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridAddressLabel")).Text);

                    manningContactNoTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridContactNoLabel")).Text);

                    manningEmailTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridEmailLabel")).Text);
                    manningDepartmentNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("manningGridDepartmentLabel")).Text);

                    manningSAPCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("manningGridSAPCodeLabel")).Text);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(manningCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }



            }
            else if (((ImageButton)sender).ID == "manningGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            manningNotifyMessageDiv.Visible = false;
            manningNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillProcess()
        {
            try
            {
                processTypeDropDownList.Items.Clear();
                processTypeDropDownList.Items.Add("");

                processTypeDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name");
                processTypeDropDownList.DataBind();
            }
            catch (Exception exp)
            {

            }
        }
        private void fillDepartmentName()
        {

            //Description   : Function for filling manningDepartmentNameDropDownList with department Name
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01
            try
            {
                manningDepartmentNameDropDownList.Items.Clear();
                manningDepartmentNameDropDownList.Items.Add("");

                manningDepartmentNameDropDownList.DataSource = myWebService.FillDropDownList("deptMaster", "name");
                manningDepartmentNameDropDownList.DataBind();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void fillGridView()
        {

            //Description   : Function for filling manningGridView
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            try
            {
                manningGridView.DataSource = myWebService.fillGridView("Select * from vManning", "iD", smartMISWebService.order.Desc);
                manningGridView.DataBind();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private int save()
        {
            //Description   : Function for saving and updating record in manningMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011     ||  29 September 2011
            //Revision No.  : 01                ||  02
            //Description   :                   ||  Change the logic for entering the DOB in SQL

            int flag = 0;
            int notifyIcon = 0;
            try
            {
                if (manningIDLabel.Text.Trim() == "0")
                {
                    if ((validation() <= 0))
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into manningMaster (deptID, firstName,lastName,gender,dob,address,contactNo,email,sapCode,areaName) values (@deptID, @firstName, @lastName,@gender,@dob,@address,@contactNo,@email,@sapCode,@areaName)";
                        myConnection.comm.Parameters.AddWithValue("deptID", Convert.ToInt32(manningDeptIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@firstName", manningFirstNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@lastName", manningLastNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@gender", manningGenderDropDownList.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dob", Convert.ToDateTime(myWebService.formatDate(manningDOBCalenderTextBox.Text.Trim())));
                        myConnection.comm.Parameters.AddWithValue("@address", manningAddressTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@contactNo", manningContactNoTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@email", manningEmailTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@sapCode", manningSAPCodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@areaName", processTypeDropDownList.SelectedValue.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "User detail saved successfully");
                    }
                    else
                    {
                        Notify(1, "FaultName already exists");
                    }

                }
                else if (manningIDLabel.Text.Trim() != "0")
                {
                    if (validation() <= 0)
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Update manningMaster SET deptID = @deptID, firstName = @firstName, lastName=@lastName, gender=@gender, dob=@dob, address=@address, contactNo=@contactNo, email=@email, sapCode=@sapCode, areaName=@areaName WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("deptID", Convert.ToInt32(manningDeptIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@firstName", manningFirstNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@lastName", manningLastNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@gender", manningGenderDropDownList.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dob", Convert.ToDateTime(myWebService.formatDate(manningDOBCalenderTextBox.Text.Trim())));
                        myConnection.comm.Parameters.AddWithValue("@address", manningAddressTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@contactNo", manningContactNoTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@email", manningEmailTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@sapCode", manningSAPCodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@areaName", processTypeDropDownList.SelectedValue.Trim());

                        myConnection.comm.Parameters.AddWithValue("@iD", manningIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "User detail updated successfully");
                    }
                }
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in manningMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            int flag = 0;
            try
            {
                if (manningIDLabel.Text.Trim() != "0")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From manningMaster WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", manningIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "User detail deleted successfully");
                }
            }
            catch (Exception ex)
            {
               // ScriptManager.RegisterClientScriptBlock(manningCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForDeleteException');", true);

            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in manningMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (manningDeptIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (manningFirstNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01


            manningIDLabel.Text = "0";
            manningDeptIDLabel.Text = "0";

            manningFirstNameTextBox.Text = "";

            manningLastNameTextBox.Text = "";

            manningDOBCalenderTextBox.Text = "";
            manningAddressTextBox.Text = "";

            manningContactNoTextBox.Text = "";

            manningEmailTextBox.Text = "";
            fillDepartmentName();

            manningSAPCodeTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in manningMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                manningNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                manningNotifyImage.Src = "../Images/tick.png";
            }
            manningNotifyLabel.Text = notifyMessage;

            manningNotifyMessageDiv.Visible = true;
            manningNotifyTimer.Enabled = true;
        }
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.ToString("dd-MM-yyyy");

            return flag;

        #endregion

        }
    }

}