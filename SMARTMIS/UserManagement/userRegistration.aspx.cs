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
using System.Text;
using System.Runtime.InteropServices;
using System.IO;



namespace SmartMIS
{
    public partial class userRegistration : System.Web.UI.Page
    {
        #region Global Variables
        smartMISWebService myWebService = new smartMISWebService();
        string month, day, year;
        string moduleName = "UserRegistration";
        string[] gender = new string[] { "Male", "Female" };
        string connString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
        string filepath;
        #endregion
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    clearPersonalDetails();
                    clearLoginDetails();
                    fillSapCode();
                    fillDepartment();
                    fillGender();

                    if (Request.QueryString["u9873518"] != null)
                    {
                        userRegPasswordFieldValidator.Enabled = false;
                        userRegRePasswordFieldValidator.Enabled = false;
                        CompareValidator.Enabled = false;
                        signInButton.Text = "Update";
                        string deptID = null;
                        mannIDHidden.Value = Request.QueryString["u9873518"];

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select * from manningMaster where iD = " + mannIDHidden.Value;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        while (myConnection.reader.Read())
                        {
                            firstNameTextBox.Text = myConnection.reader[2].ToString();
                            lastNameTextBox.Text = myConnection.reader[3].ToString();
                            userRegGenderDropDownList.SelectedValue = myConnection.reader[4].ToString();

                            txtDOB.Text = myConnection.reader[5].ToString();
                            addressTextBox.Text = myConnection.reader[6].ToString();
                            contactNoTextBox.Text = myConnection.reader[7].ToString();
                            emailTextBox.Text = myConnection.reader[8].ToString();
                            deptID = myConnection.reader[1].ToString();
                            userRegSAPCodeDropDown.SelectedValue = myConnection.reader[9].ToString();
                        }

                       
                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        getDepartmentName(deptID);

                        myConnection.open(ConnectionOption.SQL);

                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select * from userDetails where manningID = " + mannIDHidden.Value;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        while (myConnection.reader.Read())
                        {
                            userIDTextBox.Text = myConnection.reader[1].ToString();
                        }
                        passwordTextBox.Enabled = false;
                        rePasswordTextBox.Enabled = false;

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        disablePersonalDetails();
                        getRoles();

                    }
                }
                //if (Page.IsPostBack)
                //{
                //    passwordTextBox.Attributes["value"] = passwordTextBox.Text;
                //    rePasswordTextBox.Attributes["value"] = rePasswordTextBox.Text;
                //}
            }
            catch(Exception ex)
            {
                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }

        private void getUserDetails()
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from manningMaster where manningID=" + mannIDHidden.Value;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    userRegSAPCodeDropDown.SelectedValue = myConnection.reader[2].ToString();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

        }

        private void fillSapCode()
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select distinct sapCode from manningMaster where sapCode!='None'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                userRegSAPCodeDropDown.Items.Clear();
                userRegSAPCodeDropDown.Items.Add("");
                userRegSAPCodeDropDown.Items.Add("None");
                while (myConnection.reader.Read())
                {
                    userRegSAPCodeDropDown.Items.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
               
            }

            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        #endregion

        #region User Defined Function

        private void getRoles()
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select * from userRoles where userID='" + userIDTextBox.Text.Trim() + "'";
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (GridViewRow dRow in roleGridView.Rows)
                    {
                        Label val = ((Label)dRow.FindControl("roleNameLabel"));
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (val.Text.Equals((row[2]).ToString()))
                            {
                                CheckBox chk = (CheckBox)(dRow.FindControl("roleCheckBox"));
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.comm.Dispose();
               
            }
        }

        private void fillDepartment()
        {

            //Description   : Function for filling tyreXRay with Status Name
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01

            userRegDepartmentDropDownList.Items.Clear();
            userRegDepartmentDropDownList.Items.Add("");
            try
            {
                userRegDepartmentDropDownList.DataSource = myWebService.FillDropDownList("deptMaster", "name");
                userRegDepartmentDropDownList.DataBind();
            }
            catch(Exception ex)
            {

            }
        }

        private void fillGridView()
        {
            try
            {
                roleGridView.DataSource = myWebService.fillGridView("Select distinct iD,name,description from userRoles", ConnectionOption.SQL);
                roleGridView.DataBind();
            }
            catch(Exception ex)
            {
            }
        }

        private void fillGender()
        {
            userRegGenderDropDownList.Items.Clear();
            userRegGenderDropDownList.Items.Add("");

            foreach (string s in gender)
            {
                userRegGenderDropDownList.Items.Add(s);
            }
        }

        private void clearPersonalDetails()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            userRegGenderDropDownList.SelectedIndex = 0;
            txtDOB.Text = "";
            addressTextBox.Text = "";
            contactNoTextBox.Text = "";
            emailTextBox.Text = "";
            userRegDepartmentDropDownList.SelectedIndex = 0;
            userIDTextBox.Text = "";
        }

        private void clearLoginDetails()
        {
            userIDTextBox.Text = "";
            passwordTextBox.Text = "";
            rePasswordTextBox.Text = "";
            userRegSAPCodeDropDown.SelectedIndex =0;
        }

        private void disablePersonalDetails()
        {
            firstNameTextBox.Enabled = false;
            lastNameTextBox.Enabled = false;
            userRegGenderDropDownList.Enabled = false;
           // txtDOB.Enabled = false;
            txtDOB.Disabled = true;
            addressTextBox.Enabled = false;
            contactNoTextBox.Enabled = false;
            emailTextBox.Enabled = false;
            userRegDepartmentDropDownList.Enabled = false;
            
        }

        private void enablePersonalDetails()
        {
            firstNameTextBox.Enabled = true;
            lastNameTextBox.Enabled = true;
            userRegGenderDropDownList.Enabled = true;
            txtDOB.Disabled = false;
            addressTextBox.Enabled = true;
            contactNoTextBox.Enabled = true;
            emailTextBox.Enabled = true;
            userRegDepartmentDropDownList.Enabled = true;
         
        }

        private void populatePersonalDetails()
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select * from manningMaster where sapCode='" + userRegSAPCodeDropDown.SelectedItem + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    firstNameTextBox.Text = myConnection.reader[2].ToString();
                    lastNameTextBox.Text = myConnection.reader[3].ToString();
                    userRegGenderDropDownList.SelectedValue = myConnection.reader[4].ToString();
                    txtDOB.Text = myConnection.reader[5].ToString();
                    addressTextBox.Text = myConnection.reader[6].ToString();
                    contactNoTextBox.Text = myConnection.reader[7].ToString();
                    emailTextBox.Text = myConnection.reader[8].ToString();
                    userRegDeptIDLabel.Text = myConnection.reader[1].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            getDepartmentName(userRegDeptIDLabel.Text);
        }

        private void getDepartmentName(string deptID)
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from deptMaster Where iD = " + deptID;
                myConnection.reader = myConnection.comm.ExecuteReader();
                
                while (myConnection.reader.Read())
                {
                    userRegDepartmentDropDownList.SelectedValue = myConnection.reader[1].ToString();
                }
            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }


        }

        private void saveNone()
        {
            int flag = 0;
            string manningID = getManningID();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Insert into userDetails (userID, password, manningID, createdDate, modifiedDate, accountStatus) values (@userID, @password, @manningID, @createdDate,@modiefiedDate,@accountStatus)";
                myConnection.comm.Parameters.AddWithValue("@userID", userIDTextBox.Text.Trim());
                // myConnection.comm.Parameters.AddWithValue("@password",FormsAuthentication.HashPasswordForStoringInConfigFile(passwordTextBox.Text.Trim(),"sha1"));
                myConnection.comm.Parameters.AddWithValue("@password", passwordTextBox.Text.Trim());
                myConnection.comm.Parameters.AddWithValue("@manningID", manningID.Trim());
                myConnection.comm.Parameters.AddWithValue("@createdDate", DateTime.Now.ToString());
                myConnection.comm.Parameters.AddWithValue("@modiefiedDate", DateTime.Now.ToString());
                myConnection.comm.Parameters.AddWithValue("@accountStatus", "1");
                flag = myConnection.comm.ExecuteNonQuery();
                Notify(1, "User saved successfully");

            }
            catch (Exception ex)
            {

            }
            finally
            {
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        protected void save()
        {
            int flag = 0;
            try
            {
                if (!(checkAvail() == 1))
                {
                    if (!userRegSAPCodeDropDown.SelectedValue.Equals("None"))
                    {
                        if (!(chkSAPCode()))
                        {
                            saveNone();
                            addRoles();
                            Response.Redirect("/SmartMIS/UserManagement/userManagement.aspx");
                        }
                        else
                        {
                            Notify(0, "User with specified SAP Code already exist!");
                        }
                    }

                    else
                    {
                        //Inserting into manning table
                        string dob = formatDate(txtDOB.Text);
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into manningMaster(deptID,firstName,lastName,gender,dob,address,contactNo,email,sapCode) values(@deptID,@firstName,@lastName,@gender,@dob,@address,@contactNo,@email,@sapCode)";
                        myConnection.comm.Parameters.AddWithValue("@deptID", Convert.ToInt32(userRegDeptIDLabel.Text));
                        myConnection.comm.Parameters.AddWithValue("@firstName", firstNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@lastName", lastNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@gender", userRegGenderDropDownList.SelectedValue.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dob", dob);
                        myConnection.comm.Parameters.AddWithValue("@address", addressTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@contactNo", contactNoTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@email", emailTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@sapCode", userRegSAPCodeDropDown.SelectedValue.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        saveNone();
                        addRoles();
                        Response.Redirect("/SmartMIS/UserManagement/userManagement.aspx");
                    }
                }

                else
                    Notify(0, "User already exist. Please choose some other UserName.");
            }
            catch(Exception exp)
            {

            }
        }

        public string formatDate(string date)
        {
            string flag = "";
            string[] tempDate = date.Split(new char[] { '-' });
            day = tempDate[0].ToString();
            month = tempDate[1].ToString();            
            year = tempDate[2].ToString();
            flag = year + "-" + day + "-" + month;
            return flag;
        }

        private bool chkSAPCode()
        {   
            bool result = false;
            string mannID=null;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select iD from manningMaster where sapCode = '" + userRegSAPCodeDropDown.SelectedValue + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    mannID = myConnection.reader[0].ToString();
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();

                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select userID from userDetails where manningID = " + mannID;
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    mannID = myConnection.reader[0].ToString();
                    result = true;
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.close(ConnectionOption.SQL);
            }

            return result;
        }

        private void addRoles()
        {
            int flag = 0;
            foreach (GridViewRow d in roleGridView.Rows)
            {
                CheckBox chkbox = (CheckBox)(d.FindControl("roleCheckBox"));
                if (chkbox.Checked)
                {
                    Label userRoleID = (Label)(d.FindControl("roleNameLabel"));

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into userRoles(userID,roleID) values(@userID,@roleID)";
                    myConnection.comm.Parameters.AddWithValue("@userID", userIDTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@roleID", userRoleID.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();
                    myConnection.comm.Dispose();
                }
            }
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {
            if (notifyIcon == 0)
            {
                userRegNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                userRegNotifyImage.Src = "../Images/tick.png";
            }
            userRegNotifyLabel.Text = notifyMessage;

            userRegNotifyMessageDiv.Visible = true;
            userRegNotifyTimer.Enabled = true;
        }

        private string getManningID()
        {
            string manningID = null;
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select iD from manningMaster where sapCode = '" + userRegSAPCodeDropDown.SelectedValue + "'";
            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                manningID = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            return manningID;
        }

        private int checkAvail()
        {
            int flag = 0;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select userID from userDetails where userID = '" + userIDTextBox.Text.Trim() + "' and accountStatus=0";
            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                flag = 1;
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            return flag;
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "userRegSAPCodeDropDown")
            {
                if (userRegSAPCodeDropDown.SelectedItem.Value.Equals("None"))
                {
                    clearPersonalDetails();
                    enablePersonalDetails();
                }
                else
                {
                    populatePersonalDetails();
                    disablePersonalDetails();
                }
            }

            else if (((DropDownList)sender).ID == "userRegDepartmentDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select iD from deptMaster where name = '" + userRegDepartmentDropDownList.SelectedValue + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    userRegDeptIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "signInButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                {

                    if (((Button)sender).Text == "Save")
                    {
                        save();
                        clearPersonalDetails();
                    }

                    else if (((Button)sender).Text == "Update")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Delete from userRoles where userID='" + userIDTextBox.Text.Trim().ToString() + " '";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        addRoles();

                        Notify(1, "User has been updated successfully.");
                        clearPersonalDetails();
                        clearLoginDetails();


                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(cancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }

            }           
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            userRegNotifyMessageDiv.Visible = false;
            userRegNotifyTimer.Enabled = false;
        }

        #endregion

    }
}
