using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Drawing;

namespace SmartMIS.Master
{
    public partial class recipeNew : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable dt = new DataTable();
        string moduleName = "RecipeMaster";

        #region System Defined Function

        private void Page_Init(object sender, System.EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                fillProcessName();
                FillOEMName();
                fillProductTypeName();
                fillDesignName();
            }
            fillGridView();
            fillDesignName();
        }
        protected void GridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // CREATE A LinkButton AND IT TO EACH ROW.
                LinkButton lb = new LinkButton();
                LinkButton lbdelete = new LinkButton();

                lb.OnClientClick = "edit_Click(this);";
                //lbdelete.OnClientClick = "delete_Click(this);";

                lb.Text = "Edit";
                //lbdelete.Text = "Delete";
                e.Row.Cells[15].Controls.Add(lb);
                // e.Row.Cells[12].Controls.Add(lbdelete);
            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ((DropDownList)sender).Items.Remove("".Trim());


                if (((DropDownList)sender).ID == "recipeProcessNameDropDownList")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from processMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", recipeProcessNameDropDownList.SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        recipeProcessIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    // fillProductTypeName();

                }
                else if (((DropDownList)sender).ID == "recipeProductTypeNameDropDownList")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from productTypeMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", recipeProductTypeNameDropDownList.SelectedItem.ToString());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        recipeProductTypeIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                }

                else if (((DropDownList)sender).ID == "OEMDropDownList")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from oemMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", OEMDropDownList.SelectedItem.ToString());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        oemIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                }
                else if (((DropDownList)sender).ID == "processDropDownList")
                {
                    if (processDropDownList.SelectedValue != "All")
                        fillGridviewProcessWise();
                    else
                        fillGridView();

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "recipeSaveButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                {
                    save();
                    fillGridView();
                    clearPage();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(recipeCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }


            }
            else if (((Button)sender).ID == "recipeCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "recipeDialogOKButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
                {
                    recipeIDLabel.Text = recipeIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                    delete();
                    fillGridView();
                    clearPage();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(recipeCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }

            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {

            if (((ImageButton)sender).ID == "recipeGridEditImageButton")
            {
                if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 3) == true)
                {
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    recipeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridIDLabel")).Text);
                    recipeNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridRecipeNameLabel")).Text);

                    recipeProcessIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridProcessIDLabel")).Text);
                    recipeProcessNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeGridProcessNameLabel")).Text);
                    recipeProductTypeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridProductTypeIDLabel")).Text);
                    recipeProductTypeNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeGridProductTypeNameLabel")).Text);
                    oemIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridOemIDLabel")).Text);

                    OEMDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeGridOemNameLabel")).Text);

                    recipeNoTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridRecipeNoLabel")).Text);
                    SpecWeightTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridspecWeightLabel")).Text);
                    TBRPCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridTBRPcodeLabel")).Text);

                    recipeSizeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridRecipeSizeLabel")).Text);
                    recipeDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridDescriptionLabel")).Text);
                    txtsapmaterialcode.Text = (((Label)gridViewRow.Cells[1].FindControl("lblsapmatlcd")).Text);
                    txtsapmaterialcode.Text = (((Label)gridViewRow.Cells[1].FindControl("lblsapmatlcd")).Text);
                    txtsapmaterialcode.Text = (((Label)gridViewRow.Cells[1].FindControl("lblsapmatlcd")).Text);
                    txtsapmaterialcode.Text = (((Label)gridViewRow.Cells[1].FindControl("lblsapmatlcd")).Text);
                    TUOEnableDropDownList1.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("LabelTUOEnable")).Text);
                    //ddlRecipeEnable.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("LabelRecipeEnable")).Text);
                    //recipeProcessNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("recipeGridProcessNameLabel")).Text);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(recipeCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }

            }
            else if (((ImageButton)sender).ID == "recipeGridDeleteImageButton")
            {
                //Code for deleting gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                recipeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("recipeGridIDLabel")).Text);

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            recipeNotifyMessageDiv.Visible = false;
            recipeNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillProcessName()
        {

            //Description   : Function for filling wcProcessNameDropDownList with Process Name
            //Author        : Brajesh Kumar  
            //Date Created  : 10 feb 2012 
            //Date Updated  : 10 feb 2012
            //Revision No.  : 
            //Revision Desc : Change the Logic for filling the wcProcessNameDropDownList
            try
            {
                recipeProcessNameDropDownList.Items.Clear();
                recipeProcessNameDropDownList.Items.Add("");

                recipeProcessNameDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name");
                recipeProcessNameDropDownList.DataBind();

                processDropDownList.Items.Clear();
                processDropDownList.Items.Add("");

                processDropDownList.DataSource = FillDropDownList("processMaster", "name");
                processDropDownList.DataBind();
                processDropDownList.SelectedIndex = 1;
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }

        private void FillOEMName()
        {

            //Description   : Function for filling OEMLogoSelectionDropDownList with Recipe OEM Name
            //Author        : Sanjay Goyal  
            //Date Created  : 22 Nov 2012 
            //Date Updated  : 22 Nov 2012
            //Revision No.  : 
            //Revision Desc : 
            try
            {
                OEMDropDownList.Items.Clear();
                OEMDropDownList.Items.Add("");

                OEMDropDownList.DataSource = myWebService.FillDropDownList("oemMaster", "name");
                OEMDropDownList.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 01 April 2011
            //Date Updated  : 01 April 2011
            //Revision No.  : 01

            flag.Add("");
            flag.Add("All");
            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            return flag;
        }


        public bool isAuthenticate(int validationtype)
        {
            return myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, validationtype);
        }

        private void fillProductTypeName()
        {

            //Description   : Function for filling productTypeNameDropDownList 
            //Author        : Brajesh Kumar
            //Date Created  : 30 jan 2012
            //Date Updated  : 30 jan 2012
            //Revision No.  : 01


            recipeProductTypeNameDropDownList.Items.Clear();
            recipeProductTypeNameDropDownList.DataSource = myWebService.FillDropDownList("productTypeMaster", "name");
            recipeProductTypeNameDropDownList.DataBind();

        }
        private void fillDesignName()
        {

            //Description   : Function for filling productTypeNameDropDownList 
            //Author        : Brajesh Kumar
            //Date Created  : 30 jan 2012
            //Date Updated  : 30 jan 2012
            //Revision No.  : 01


            DesignDropDownList1.Items.Clear();
            DesignDropDownList1.DataSource = myWebService.FillDropDownList("recipeMaster", "tyreDesign");
            DesignDropDownList1.DataBind();

        }


        private void fillGridviewProcessWise()
        {
            try
            {
                DataTable pdt = new DataTable();

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select RecipeName,ProcessName,ProductTypeName, OemName,RecipeNo,SpecWeight,TBRPCode, TyreSize,TyreDesign,Description ,SAPMaterialCode,upperWeight,lowerWeight,TBRTUOCode,TUOEnable  ,id as rid  ,id from  vRecipeMaster where processName='" + processDropDownList.SelectedValue.ToString() + "'order by id desc";
                myConnection.reader = myConnection.comm.ExecuteReader();
                pdt.Load(myConnection.reader);


                if (!myConnection.reader.IsClosed)
                    myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                recipeGridView.DataSource = pdt;
                recipeGridView.DataBind();

                recipeGridView.Rows[0].Cells[0].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[1].Width = new Unit(6, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[2].Width = new Unit(6, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[3].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[4].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[5].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[6].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[7].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[8].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[9].Width = new Unit(10, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[10].Width = new Unit(7, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[11].Width = new Unit(6, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[12].Width = new Unit(6, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[13].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[14].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[15].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[16].Width = new Unit(5, UnitType.Percentage);
                //recipeGridView.DataSource = myWebService.fillGridView("select iD,processID,producttypeid,oemID,oemName,processName,productTypeName, recipeName,recipeNo,specWeight,TBRPCode, tyreSize,description  from vRecipeMaster where processName='" + processDropDownList.SelectedValue.ToString() + "'", "iD", smartMISWebService.order.Desc);
                //recipeGridView.DataBind();
                ViewState["dt"] = pdt;
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void fillGridView()
        {

            //Description   : Function for filling wcGridView
            //Author        : Brajesh Kumar  ||
            //Date Created  : 10 feb 2012 ||
            //Date Updated  : 10 feb 2012 || 11 April
            //Revision No.  : 01            || 02
            //Revision Desc :               || Change the logic for filling wcGridView by webservice
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select RecipeName,ProcessName,ProductTypeName, OemName,RecipeNo,SpecWeight,TBRPCode, TyreSize,TyreDesign,Description ,SAPMaterialCode,upperWeight,lowerWeight,TBRTUOCode,TUOEnable  ,id as rid  ,id from vRecipeMaster order by id desc";
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt.Load(myConnection.reader);


            if (!myConnection.reader.IsClosed)
                myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            recipeGridView.DataSource = dt;
            recipeGridView.DataBind();
            //recipeGridView.Columns[4].ItemStyle.Width = new Unit(50, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[0].Width = new Unit(8, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[1].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[2].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[3].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[4].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[5].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[6].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[7].Width = new Unit(4, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[8].Width = new Unit(7, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[9].Width = new Unit(9, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[10].Width = new Unit(16, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[11].Width = new Unit(4, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[12].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[13].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[14].Width = new Unit(5, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[15].Width = new Unit(2, UnitType.Percentage);
            recipeGridView.Rows[0].Cells[16].Width = new Unit(5, UnitType.Percentage);
            //recipeGridView.Rows[0].Cells[12].Width = new Unit(4, UnitType.Percentage);

            //recipeGridView.DataSource = myWebService.fillGridView("select iD,processID,producttypeid,oemID,oemName,processName,productTypeName, recipeName,recipeNo,specWeight,TBRPCode, tyreSize,description,SAPMaterialCode  from vRecipeMaster", "iD", smartMISWebService.order.Desc);
            //recipeGridView.DataBind();
            ViewState["dt"] = dt;
        }

        public int FindOEMID(string OEMName)
        {

            int id;
            id = 0;

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select ID from oemMaster where name='" + OEMName + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    id = Convert.ToInt32(myConnection.reader[0].ToString());
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();

                return id;
            }
            catch
            {
                return 0;
            }

        }

        public void edit_Click(Object sender, EventArgs e)
        {

            //Label2.Text = "You clicked the link button";
        }


        private int save()
        {
            //Description   : Function for saving and updating record in wcMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 10 feb 2012
            //Date Updated  : 10 feb 2012 By Brajesh Kumar With productType and barCode
            //Revision No.  : 03

            int flag = 0;
            int notifyIcon = 0;
            recipeIDLabel.Text = recipeIDHidden.Value;
            if (recipeIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("recipeMaster", "name", "WHERE name = '" + recipeNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    try
                    {
                        Console.WriteLine("Enter Insert");
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into recipeMaster (processID,productTypeID,oemID, name,recipeNo,specWeight,TBRPCode, tyreSize,tyreDesign, description,SAPMaterialCode,upperWeight,lowerWeight,TBRTUOCode,TUOEnable) values (@processID,@productTypeID,@oemID, @name,@recipeNo,@specWeight,@tbrPcode, @recipeSize, @design,@description,@SAPMaterialCode,@upperWeight,@lowerWeight,@TBRTUOCode,@TUOEnable)";
                        myConnection.comm.Parameters.AddWithValue("@processID", Convert.ToInt32(recipeProcessIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(recipeProductTypeIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@oemID", Convert.ToInt32(oemIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@name", recipeNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@recipeNO", recipeNoTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@specWeight", SpecWeightTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@tbrPcode", TBRPCodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@recipeSize", recipeSizeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@design", DesignDropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", recipeDescriptionTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@SAPMaterialCode", txtsapmaterialcode.Text);
                        myConnection.comm.Parameters.AddWithValue("@upperWeight", upperweighttxt.Text);
                        myConnection.comm.Parameters.AddWithValue("@lowerWeight", txtLowerWeight.Text);
                        myConnection.comm.Parameters.AddWithValue("@TBRTUOCode", tbrtuocodetxt.Text);
                        myConnection.comm.Parameters.AddWithValue("@TUOEnable", TUOEnableDropDownList1.SelectedValue.Trim());
                        //myConnection.comm.Parameters.AddWithValue("@RecipeEnable", ddlRecipeEnable.SelectedValue.Trim());
                        //myConnection.comm.Parameters.AddWithValue("@OEMID", Convert.ToInt32(recipeDescriptionTextBox.Text.Trim()));

                        //myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        flag = myConnection.comm.ExecuteNonQuery();
                        //myWebService.writeLogs("", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(notifyIcon, "Recipe saved successfully");
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    fillDesignName();
                }
                else
                {
                    Notify(notifyIcon, "Recipe already exists");
                }
            }
            else if (recipeIDLabel.Text.Trim() != "0")
            {
                //if ((validation() <= 0) && (myWebService.IsRecordExist("recipeMaster", "name", "WHERE name = '" + recipeNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                // {
                if (validation() <= 0)
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Update recipeMaster SET productTypeID=@productTypeID,oemID=@oemID,recipeNO=@recipeNo,specWeight=@specWeight,TBRPcode=@tbrPcode, tyreSize=@recipeSize, tyreDesign = @design,description = @description,SAPMaterialCode=@SAPMaterialCode,upperWeight=@upperWeight,lowerWeight=@lowerWeight,TBRTUOCode=@TBRTUOCode,TUOEnable = @TUOEnable WHERE (iD = @iD)";
                        // myConnection.comm.Parameters.AddWithValue("@processID", Convert.ToInt32(recipeProcessIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(recipeProductTypeIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@oemID", Convert.ToInt32(oemIDLabel.Text.Trim()));
                        //myConnection.comm.Parameters.AddWithValue("@name", recipeNameTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@recipeNo", recipeNoTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@specWeight", SpecWeightTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@tbrPcode", TBRPCodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@recipeSize", recipeSizeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@design", DesignDropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@description", recipeDescriptionTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@SAPMaterialCode", txtsapmaterialcode.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@upperWeight", upperweighttxt.Text);
                        myConnection.comm.Parameters.AddWithValue("@lowerWeight", txtLowerWeight.Text);
                        myConnection.comm.Parameters.AddWithValue("@TBRTUOCode", tbrtuocodetxt.Text);
                        myConnection.comm.Parameters.AddWithValue("@TUOEnable", TUOEnableDropDownList1.SelectedValue.Trim());
                        //myConnection.comm.Parameters.AddWithValue("@RecipeEnable", ddlRecipeEnable.SelectedValue.Trim());
                        myConnection.comm.Parameters.AddWithValue("@iD", recipeIDLabel.Text);
                        flag = myConnection.comm.ExecuteNonQuery();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        recipeIDHidden.Value = "0";
                        Notify(1, "Recipe updated successfully");
                        //Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in wcMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 28 novem 2012
            //Date Updated  : 10 novem 2012
            //Revision No.  : 01

            int flag = 0;
            try
            {
                recipeIDLabel.Text = recipeIDHidden.Value;
                if (recipeIDLabel.Text.Trim() != "0")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From recipeMaster WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@iD", recipeIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Recipe deleted successfully");
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in wcMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 10 feb 2012
            //Date Updated  : 10 feb 2012
            //Revision No.  : 01

            int flag = 0;

            if (recipeProcessIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (recipeNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 10 feb 2012
            //Date Updated  : 
            //Revision No.  : 01

            recipeIDLabel.Text = "0";
            recipeNameTextBox.Text = "";
            fillProcessName();
            FillOEMName();
            recipeProcessIDLabel.Text = "0";
            recipeDescriptionTextBox.Text = "";
            recipeProductTypeIDLabel.Text = "0";
            fillProductTypeName();
            recipeNoTextBox.Text = "";
            DesignDropDownList1.SelectedItem.Text = "";
            SpecWeightTextBox.Text = "";
            TBRPCodeTextBox.Text = "";
            recipeSizeTextBox.Text = "";
            txtsapmaterialcode.Text = "";
            upperweighttxt.Text = "";
            txtLowerWeight.Text = "";
            tbrtuocodetxt.Text = "";


            fillGridView();
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {
            //Description   : Function for showing notify information in wcMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 10 feb 2012
            //Date Updated  : 10 feb 2012
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                recipeNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                recipeNotifyImage.Src = "../Images/tick.png";
            }
            recipeNotifyLabel.Text = notifyMessage;

            recipeNotifyMessageDiv.Visible = true;
            recipeNotifyTimer.Enabled = true;
        }

        #endregion

        #region Export Data into Excel

        protected void expToExcel_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["dt"];
            DataTable dtNew = dt.AsEnumerable().OrderBy(row => row.Field<Int32>("id")).CopyToDataTable();
            dtNew.Columns.Remove("rid");
            dtNew.Columns["id"].SetOrdinal(0);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Recipe_Report.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Recipe_Report");
            ws.Cells["A1"].Value = "Recipe_Report ";

            using (ExcelRange r = ws.Cells["A1:AG1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }

            ws.Cells["A3"].LoadFromDataTable(dtNew, true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();

            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);
            System.Threading.Thread.Sleep(10);
            Response.Flush();
            Response.End();

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        #endregion

    }
}
