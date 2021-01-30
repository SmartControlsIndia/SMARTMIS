using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace SmartMIS.Master
{
    public partial class TrimmingFileName : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string moduleName = "TrimLookUpMaster";

        #region System Defined Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    fillGridView();
                    fillTBMRecipeNames();
                  
                }
                catch (Exception exp)
                {

                }

            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "TrimLookUpTbmRecipeNameDropDownList")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select iD from recipeMaster Where name= @name";
                    myConnection.comm.Parameters.AddWithValue("@name", TrimLookUpTbmRecipeNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        TrimLookUpTbmRecipeIDLabel.Text = myConnection.reader[0].ToString();
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

            //else if (((DropDownList)sender).ID == "TrimLookUpCuringRecipeNameDropDownList")
            //{
            //    try
            //    {
            //        myConnection.open(ConnectionOption.SQL);
            //        myConnection.comm = myConnection.conn.CreateCommand();

            //        myConnection.comm.CommandText = "Select iD from recipeMaster Where name = @name";
            //        //myConnection.comm.Parameters.AddWithValue("@name", TrimLookUpCuringRecipeNameDropDownList.SelectedItem.ToString().Trim());

            //        myConnection.reader = myConnection.comm.ExecuteReader();
            //        while (myConnection.reader.Read())
            //        {
            //            TrimLookUpCuringRecipeIDLabel.Text = myConnection.reader[0].ToString();


            //        }
            //    }
            //    catch (Exception exc)
            //    {

            //    }
            //    finally
            //    {
            //        myConnection.reader.Close();
            //        myConnection.comm.Dispose();
            //        myConnection.close(ConnectionOption.SQL);
            //    }


            //}


        }
        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "TrimLookUpSaveButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        Notify(1, "Please login with Admin.");
                    }
                }
                else if (((Button)sender).ID == "TrimLookUpCancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "TrimLookUpDialogOKButton")
                {
                    if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                    {
                        TrimLookUpIDLabel.Text = TrimLookUpIDHidden.Value; //Passing value to TrimLookUpIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(TrimLookUpCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                    }
                }
                else {
                    Notify(1, "Please login with Admin.");
                }
            }
            catch (Exception exc)
            {

            }
        }
        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "TrimLookUpGridEditImageButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    TrimLookUpIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("TrimLookUpGridIDLabel")).Text);
                    TrimLookUpTbmRecipeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("TrimLookUptbmRecipeGridIDLabel")).Text);
                    TrimLookUpTbmRecipeNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("TrimLookUpGridTBMRecipeNameLabel")).Text);

                    EventArgs recipelookupDropDownListEventArgs = new EventArgs();
                    DropDownList_SelectedIndexChanged(TrimLookUpTbmRecipeNameDropDownList, recipelookupDropDownListEventArgs);
                   
                    txtmouldname.Text = (((Label)gridViewRow.Cells[1].FindControl("TrimLookUpGridMouldNameLabel")).Text);
                    txtTrim.Text = (((Label)gridViewRow.Cells[1].FindControl("TrimLookUpGridTrimNOLabel")).Text);

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(TrimLookUpCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }
            }
            else if (((ImageButton)sender).ID == "TrimLookUpGridDeleteImageButton")
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
            TrimLookUpTbmRecipeNameDropDownList.Items.Clear();
            TrimLookUpTbmRecipeNameDropDownList.Items.Add("");
            try
            {
                TrimLookUpTbmRecipeNameDropDownList.DataSource = myWebService.FillDropDownList("recipemaster", "name", "where processId in(7)");
                TrimLookUpTbmRecipeNameDropDownList.DataBind();
            }
            catch (Exception exc)
            {

            }

        }
        
        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            TrimLookUpNotifyMessageDiv.Visible = false;
            recipelookUpNotifyTimer.Enabled = false;
        }
        private void fillGridView()
        {
            try
            {
                TrimLookUpGridView.DataSource = myWebService.fillGridView("Select *  from vTrimLookUp", ConnectionOption.SQL);
                TrimLookUpGridView.DataBind();
            }
            catch (Exception exc)
            {

            }
        }
        public bool IsRecordExist(string tableName, string coloumnName, string whereClause, out int notifyIcon)
        {
            bool flag = false;
            notifyIcon = 1;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = true;
                    notifyIcon = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                //myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }
        private int save()
        {
            
            int flag = 0;
            int notifyIcon = 0;

            if (TrimLookUpIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist1("TrimLookUp", "tbmrecipeID", "mouldName", "WHERE tbmRecipeID = '" + TrimLookUpTbmRecipeIDLabel.Text.Trim() + "' and mouldName= '" + txtmouldname.Text.Trim() + "'", out notifyIcon) == false))
                    //if ((validation() <= 0))
                    //{
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into TrimLookUp (tbmrecipeID, mouldName,TrimNo) values (@tbmrecipeID, @mouldName,@TrimNo)";

                        if (txtmouldname.Text.Contains(','))//checking for you are entered single value or multiple values  
                        {
                            string[] arryval = txtmouldname.Text.Split(',');//split values with ‘,’  
                            int j = arryval.Length;
                            int i = 0;
                            for (i = 0; i < j; i++)
                            {
                                myConnection.comm.Parameters.Clear();
                                myConnection.comm.Parameters.AddWithValue("@tbmrecipeID", Convert.ToInt32(TrimLookUpTbmRecipeIDLabel.Text.Trim()));
                                myConnection.comm.Parameters.AddWithValue("@mouldName", txtmouldname.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@TrimNo", txtTrim.Text.Trim());

                            }

                            flag = myConnection.comm.ExecuteNonQuery();
                            Notify(1, "Trim Setting saved successfully");

                        }
                        else
                        {
                            myConnection.comm.Parameters.Clear();
                            myConnection.comm.Parameters.AddWithValue("@tbmrecipeID", Convert.ToInt32(TrimLookUpTbmRecipeIDLabel.Text.Trim()));
                            myConnection.comm.Parameters.AddWithValue("@mouldName", txtmouldname.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@TrimNo", txtTrim.Text.Trim());
                            flag = myConnection.comm.ExecuteNonQuery();
                            Notify(1, "Trim Setting saved successfully");
                        }
                    }
                    catch (Exception exc)
                    {
                        Notify(1, "Trim Setting could not save");

                    }
                    finally
                    {
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }

                else
                {
                    Notify(notifyIcon, "Record already exists");
                }
            }
            else if (TrimLookUpIDLabel.Text.Trim() != "0")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update TrimLookUp SET tbmrecipeID = @tbmrecipeID, mouldName = @mouldName,TrimNo= @TrimNo where (id = @iD)";

                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(TrimLookUpIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@tbmrecipeID", Convert.ToInt32(TrimLookUpTbmRecipeIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@mouldName", txtmouldname.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@TrimNo", txtTrim.Text.Trim());


                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "Trim Setting updated successfully");

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
            //TrimLookUpGridTrimNOLabel.Text = "0";
            TrimLookUpTbmRecipeIDLabel.Text = "0";
            //TrimLookUpGridMouldNameLabel = "0";

            txtmouldname.Text = "0";
            txtTrim.Text = "0";
            fillTBMRecipeNames();
            //fillCuringRecipeNames();


            //mheProductCodeDropDownList.SelectedIndex = 0;
            //mheUnitDropDownList.SelectedIndex = 0;
        }
        private void Notify(int notifyIcon, string notifyMessage)
        {

            if (notifyIcon == 0)
            {
                TrimLookUpNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                TrimLookUpNotifyImage.Src = "../Images/tick.png";
            }
            TrimLookUpNotifyLabel.Text = notifyMessage;

            TrimLookUpNotifyMessageDiv.Visible = true;
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



            if (TrimLookUpTbmRecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            //if (TrimLookUpGridMouldNameLabel.Text.Trim() == "0")
            //{
            //    flag = 1;
            //}


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

            if (TrimLookUpIDLabel.Text.Trim() != "0")
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From TrimLookUp WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(TrimLookUpIDLabel.Text.Trim()));

                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "TrimLookUp Setting  deleted successfully");

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
