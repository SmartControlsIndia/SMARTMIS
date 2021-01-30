using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;

namespace SmartMIS.Master
{
    public partial class oem : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "OemMaster";
        public String FileName = "";
        public String imageurl="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearPage();
                fillGridView();
            }
            
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "oemSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();

                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(oemCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
                else if (((Button)sender).ID == "oemCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "oemDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        oemIDLabel.Text = oemIDHidden.Value; //Passing value to oemIDLabel because on postback hidden field retains its value
                        delete();
                        clearPage();
                        fillGridView();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(oemCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }

                }
            }
            catch(Exception exp)
            {

            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "oemGridEditImageButton")
            {
                try
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                    {
                        //Code for editing gridview row
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;
                        oemIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("oemGridIDLabel")).Text);
                        oemNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("oemGridNameLabel")).Text);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(oemCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
                catch(Exception exp)
                {
                }
            }
            else if (((ImageButton)sender).ID == "oemGridDeleteImageButton")
            {
                //Code for deleting gridview row
            }

            else
            {
            }
        }


        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling oemGridView
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01
            try
            {

                oemGridView.DataSource = myWebService.fillGridView("Select * from oemMaster", "iD", smartMISWebService.order.Desc);
                oemGridView.DataBind();
            }
            catch(Exception exp)
            {

            }
        }

        private int save()
        {
            //Description   : Function for saving and updating record in oemMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01
            fileUpload();
            int flag = 0;
            int notifyIcon = 0;
            if (oemIDLabel.Text.Trim() == "0")
            {     
            
                if ((validation() <= 0) && (myWebService.IsRecordExist("oemMaster", "name", "where name='" + oemNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into oemMaster (name,logoName) values (@name,@logoName)";
                        myConnection.comm.Parameters.AddWithValue("@name", oemNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@logoName", imageurl.ToString());


                        flag = myConnection.comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        clearPage();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                    Notify(1, "OEM Record saved successfully");
                }
                else
                {
                    Notify(notifyIcon, "OEM Record already exists");
                }
            }
            else if (oemIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Update oemMaster SET name = @name,logoName=@logoName WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@name", oemNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@iD", oemIDLabel.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@logoName", imageurl.ToString());

                        flag = myConnection.comm.ExecuteNonQuery();
                    }
                    catch (Exception exp)
                    {

                    }
                    finally
                    {
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                    Notify(1, "OEM Record updated successfully");
                }
            }

            return flag;
        }

      

        private int delete()
        {
            int flag = 0;

            if (oemIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                /**/
                myConnection.comm.CommandText = "select LogoName From oemMaster WHERE (iD = @logoID)";
                myConnection.comm.Parameters.AddWithValue("@logoID", oemIDLabel.Text.Trim());
                SqlDataReader rdr = myConnection.comm.ExecuteReader();
                if (rdr.HasRows == true)
                {
                    rdr.Read();
                    string logoName = rdr["logoName"].ToString();
                    if (logoName != "")
                    {
                        logoName = logoName.Substring(7, logoName.Length - 7);
                        logoName = logoName.Trim();
                        try
                        {
                            String path = Server.MapPath("~/Logo/");
                            FileInfo TheFile = new FileInfo(path + logoName);
                            string fn = TheFile.FullName;
                            if (TheFile.Exists)
                            {
                                File.Delete(fn);

                            }
                        }
                        catch (Exception e) { }
                        finally
                        {
                            rdr.Close();
                            myConnection.comm.Dispose();
                        }
                    }
                    else
                    {
                        rdr.Close();
                        myConnection.comm.Dispose();
                    }
                }

                /**/
                try
                {
                    myConnection.comm.CommandText = "Delete From oemMaster WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", oemIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "Record deleted successfully");
                    
                }
                catch (Exception exp)
                {
                    Notify(1, "You canot delete this Record");
                }
                finally
                {
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
               
               
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

            if (oemNameTextBox.Text.Trim() == "")
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

            oemIDLabel.Text = "0";
            oemNameTextBox.Text = "";
             FileName = "";
            imageurl="";

           
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in oemMessageDiv
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
                oemNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                oemNotifyImage.Src = "../Images/tick.png";
            }
            oemNotifyLabel.Text = notifyMessage;

            oemNotifyMessageDiv.Visible = true;
            oemNotifyTimer.Enabled = true;
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            oemNotifyMessageDiv.Visible = false;
            oemNotifyTimer.Enabled = false;
        }


        private void fileUpload()
        {
            String path = Server.MapPath("~/Logo/");
            if (FileUpload1.HasFile)
            {
                String extention = System.IO.Path.GetExtension(FileUpload1.FileName.ToLower());
                String[] ext = { ".jpg", ".jpeg", ".png", ".gif" };
                bool filextOK = false;
                for (int i = 0; i < ext.Length; i++)
                {
                    if (extention == ext[i])
                    {
                        filextOK = true;
                        break;
                    }

                }
                if (filextOK)
                {
                    try
                    {
                        FileName = FileUpload1.FileName;
                        imageurl = "~/Logo/" + FileName;
                        FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);
                        


                    }
                    catch (Exception exp)
                    {
                        
                    }

                }
            }

        }
        
        

        #endregion

      

       

        
    }
}
