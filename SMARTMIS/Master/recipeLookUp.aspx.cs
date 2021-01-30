using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Master
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string moduleName = "recipeLookupMaster";

        #region System Defined Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    fillGridView();
                    fillTBMRecipeNames();
                    fillCuringRecipeNames();
                }
                catch(Exception exp)
                {

                }

            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "recipeLookUpTbmRecipeNameDropDownList")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select iD from recipeMaster Where name= @name";
                    myConnection.comm.Parameters.AddWithValue("@name", recipeLookUpTbmRecipeNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        recipeLookUpTbmRecipeIDLabel.Text = myConnection.reader[0].ToString();
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

            else if (((DropDownList)sender).ID == "recipeLookUpCuringRecipeNameDropDownList")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select iD from recipeMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", recipeLookUpCuringRecipeNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        recipeLookUpCuringRecipeIDLabel.Text = myConnection.reader[0].ToString();


                    }
                }
                catch (Exception exc)
                {

                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }


            }

         
        }
        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "recipeLookUpSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(recipeLookUpCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
                else if (((Button)sender).ID == "recipeLookUpCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "recipeLookUpDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        recipeLookUpIDLabel.Text = recipeLookUpIDHidden.Value; //Passing value to recipeLookUpIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(recipeLookUpCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
            }
            catch(Exception exc)
            {

            }
        }
        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "recipeLookUpGridEditImageButton")
            {
                 if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                 {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                recipeLookUpIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeLookUpGridIDLabel")).Text);
                recipeLookUpTbmRecipeNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeLookUpGridTBMRecipeNameLabel")).Text);

                EventArgs recipelookupDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(recipeLookUpTbmRecipeNameDropDownList, recipelookupDropDownListEventArgs);
                recipeLookUpCuringRecipeIDLabel.Text=(((Label)gridViewRow.Cells[1].FindControl("recipeLookUpCuringRecipeGridIDLabel")).Text);
                recipeLookUpCuringRecipeNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeLookUpGridCuringRecipeNameLabel")).Text);

                   }
                 else
                 {
                     ScriptManager.RegisterClientScriptBlock(recipeLookUpCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                 }
            }
            else if (((ImageButton)sender).ID == "recipeLookUpGridDeleteImageButton")
            {
                //Code for deleting gridview row
                delete();
            }
            else
            {
            }
        }

        #endregion

        #region User Defined Function

        private void fillTBMRecipeNames()
        {
            recipeLookUpTbmRecipeNameDropDownList.Items.Clear();
            recipeLookUpTbmRecipeNameDropDownList.Items.Add("");
            try
            {
                recipeLookUpTbmRecipeNameDropDownList.DataSource = myWebService.FillDropDownList("recipemaster", "name", "where processId in(4,7)");
                recipeLookUpTbmRecipeNameDropDownList.DataBind();
            }
            catch(Exception exc)
            {

            }

        }
        private void fillCuringRecipeNames()
        {
        
            recipeLookUpCuringRecipeNameDropDownList.Items.Clear();
            recipeLookUpCuringRecipeNameDropDownList.Items.Add("");
            try
            {
                recipeLookUpCuringRecipeNameDropDownList.DataSource = myWebService.FillDropDownList("recipemaster", "name", "where processID in(5,8)");
                recipeLookUpCuringRecipeNameDropDownList.DataBind();
            }
            catch(Exception exc)
            {

            }

        }
        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            recipeLookUpNotifyMessageDiv.Visible = false;
            recipelookUpNotifyTimer.Enabled = false;
        }
        private void fillGridView()
        {
            try
            {
                recipeLookUpGridView.DataSource = myWebService.fillGridView("Select *  from vrecipelooKup", ConnectionOption.SQL);
                recipeLookUpGridView.DataBind();
            }
            catch(Exception exc)
            {

            }
        }
        private int save()
        {
            //Description   : Function for saving and updating record in wcMaster Table
            //Author        : Rohit Singh
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;

            if (recipeLookUpIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0))
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into recipelookup (tbmrecipeID, curingRecipeID) values (@tbmrecipeID, @curingRecipeID)";
                        myConnection.comm.Parameters.AddWithValue("@tbmrecipeID", Convert.ToInt32(recipeLookUpTbmRecipeIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@curingRecipeID", Convert.ToInt32(recipeLookUpCuringRecipeIDLabel.Text.Trim()));



                        flag = myConnection.comm.ExecuteNonQuery();
                        Notify(1, "RecipeSetting saved successfully");

                    }
                    catch (Exception exc)
                    {
                        Notify(1, "Recipe Setting could not save");

                    }
                    finally
                    {
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                }
            }

            else if (recipeLookUpIDLabel.Text.Trim() != "0")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update recipeLookUp SET tbmrecipeID = @tbmrecipeID, curingRecipeID = @curingRecipeID where (iD = @iD)";

                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(recipeLookUpIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@tbmrecipeID", Convert.ToInt32(recipeLookUpTbmRecipeIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@curingRecipeID", Convert.ToInt32(recipeLookUpCuringRecipeIDLabel.Text.Trim()));

                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "Recipe Setting updated successfully");

                }
                catch (Exception exc)
                {

                }
                finally
                {
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }


            }
            return flag;
        }
        private void clearPage()
        {
            recipeLookUpCuringRecipeIDLabel.Text = "0";
            recipeLookUpTbmRecipeIDLabel.Text = "0";


            fillTBMRecipeNames();
            fillCuringRecipeNames();


            //mheProductCodeDropDownList.SelectedIndex = 0;
            //mheUnitDropDownList.SelectedIndex = 0;
        }
        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in xrayMessageDiv
            //Author        : Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                recipeLookUpNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                recipeLookUpNotifyImage.Src = "../Images/tick.png";
            }
            recipeLookUpNotifyLabel.Text = notifyMessage;

            recipeLookUpNotifyMessageDiv.Visible = true;
            recipelookUpNotifyTimer.Enabled = true;
        }
        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in wcMaster Table
            //Author        : Rohit Singh
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            int flag = 0;

         

            if (recipeLookUpTbmRecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (recipeLookUpCuringRecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }


            return flag;
        }
        private int delete()
        {

            //Description   : Function for deleting record in vInspectionTBR Table
            //Author        : Rohit Singh
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (recipeLookUpIDLabel.Text.Trim() != "0")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From recipelookUp WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(recipeLookUpIDLabel.Text.Trim()));

                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "recipe Setting  deleted successfully");

                }
                catch (Exception exc)
                {

                }
                finally
                {
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

            }

            return flag;
        }

        #endregion

     

      

       
    }
}
