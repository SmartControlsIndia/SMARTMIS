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

    public partial class productionPlanning : System.Web.UI.Page
    {
        smartMISWebService myWebService=new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Events
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());
            
            if (((DropDownList)sender).ID == "productionPlanningWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", productionPlanningWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    ProdPlanningWCIDLabel.Text = myConnection.reader[0].ToString();
                    ProdPlanningProcessID.Text = myConnection.reader[1].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
               //For Enable Disable Recipe No Text Box
                if (ProdPlanningProcessID.Text.ToString().Trim() == "33".Trim())
                {
                    productionPlanningRecipeNoText.Enabled = true;
                }
                else
                {
                    productionPlanningRecipeNoText.Enabled = false;
                }
                //CheckForRecipeCode(ProdPlanningProcessID.Text.ToString().Trim());

                FillPrototype(ProdPlanningWCIDLabel.Text.Trim());
                productionPlanningUnitOfMeasureLabel.Text = "";

            }
            else if (((DropDownList)sender).ID == "productionPlanningProductTypeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select * from producttypeMaster where name=@pTypeName";
                myConnection.comm.Parameters.AddWithValue("@pTypeName", productionPlanningProductTypeDropDownList.SelectedItem.ToString().Trim());
                myConnection.reader = myConnection.comm.ExecuteReader();
                if (myConnection.reader.HasRows)
                {
                    while (myConnection.reader.Read())
                    {
                        productTypeIDLabel.Text = myConnection.reader[0].ToString().Trim();
                    }
                }

                //For Displaying Unit 
                DisplayUnit(productionPlanningProductTypeDropDownList.SelectedItem.ToString().Trim());
            }
            
           

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGridView();
                fillWorkcenterName();
                FillShift();
                productionPlanningDateTextBox.Text = System.DateTime.Now.ToString();
            }
        }
        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "productionPlanningSaveButton")
            {
                Save();
                fillGridView();
                clearPage();
            }

            else if (((Button)sender).ID == "prodPlanCancelButton")
            {
                clearPage();
            }

            else if (((Button)sender).ID == "prodPlanDialogOKButton")
            {
                prodPlanIDLabel.Text = prodPlanIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }

        }
        #endregion
        #region User Defined Functions

        private string CheckForRecipeCode(string RecipeCode, string RecipeNo, int EnableFlag)
        {
            
            string recipeID = "";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select * from recipeMaster where name=@name";
            myConnection.comm.Parameters.AddWithValue("@name", RecipeCode);
           
            myConnection.reader = myConnection.comm.ExecuteReader();


            if (myConnection.reader.HasRows)
            {//Recipe Code Exist in Recipe Master Table
                while (myConnection.reader.Read())
                {
                    recipeID = myConnection.reader[0].ToString();
                }

            }
            else
            {//Recipe Code Does Not Exist in Recipe Master Table
                //Recipe no inserted '0'
                insertNewRecipe(RecipeCode, RecipeNo);
                recipeID = getRecipeID(RecipeCode);

            }
            return recipeID;
        }
        private string getRecipeID(string RecipeCode)
        {
            string recipeID = "";
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select * from recipeMaster where name=@name";
            myConnection.comm.Parameters.AddWithValue("@name", RecipeCode);
            // myConnection.comm.Parameters.AddWithValue("@RecipeNo",RecipeNo);
            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {

                    recipeID = myConnection.reader[0].ToString().Trim();
                }
            }
            return recipeID;
        }
        private void insertNewRecipe(string RecipeCode, string RecipeNo)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "insert into recipeMaster(processID,name,recipeNo,productTypeID) values(@processId,@name,@recipieNo,@productTypeID)";
            myConnection.comm.Parameters.AddWithValue("@processId", ProdPlanningProcessID.Text.ToString().Trim());
            myConnection.comm.Parameters.AddWithValue("@name", RecipeCode);
            myConnection.comm.Parameters.AddWithValue("@recipieNo", Convert.ToInt32(RecipeNo));
            myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeIDLabel.Text.Trim()));
            myConnection.comm.ExecuteNonQuery();
            myConnection.close(ConnectionOption.SQL);
        }
        private int delete()
        {

            //Description   : Function for deleting record in vInspectionTBR Table
            //Author        : Brajesh kumar
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (prodPlanIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From productionPlanning WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", prodPlanIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Production Plan record deleted successfully");
            }

            return flag;
        }
        private void Save()
        {
            
          
            int flag = 0;
          

            if (validation() <= 0)
            {
                if (productionPlanningRecipeNoText.Enabled == true)
                {

                    recipeIDLabel.Text = CheckForRecipeCode(productionPlanningRecipeCodeTextBox.Text.ToString().Trim(), productionPlanningRecipeNoText.Text.ToString().Trim(), 1);
                }
                else
                {

                    recipeIDLabel.Text = CheckForRecipeCode(productionPlanningRecipeCodeTextBox.Text.ToString().Trim(), "0", 0);
                }

                if (prodPlanIDLabel.Text.Trim() != "0")
                {
                    

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update productionPlanning SET wcID = @wcID, productTypeID = @productTypeID, dtandTime = @dtandTime, recipeID = @recipeID, quantity = @quantity, shift = @shift WHERE (iD = @iD)";

                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(prodPlanIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(ProdPlanningWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", productTypeIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", productionPlanningDateTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeID", recipeIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@quantity", productionPlanningQuantityTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@shift", productionPlanningShiftDropDownList.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Production Planning updated successfully");

                }    

                if (prodPlanIDLabel.Text.Trim() == "0")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "insert into productionPlanning(wcID,productTypeID,recipeID,quantity,shift,dtandTime) values(@wcID,@productTypeID,@recipeID,@quantity,@shift,@dtandTime)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(ProdPlanningWCIDLabel.Text.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeIDLabel.Text.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@quantity", Convert.ToInt32(productionPlanningQuantityTextBox.Text.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@shift", productionPlanningShiftDropDownList.SelectedItem.Text.ToString().Trim());
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", productionPlanningDateTextBox.Text.ToString());
                    myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeIDLabel.Text.ToString().Trim()));

                    
                     myConnection.comm.ExecuteNonQuery();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    Notify(1, "Production Planning Inserted successfully");
                }
            }
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in xrayMessageDiv
            //Author        : Brajesh kumar
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
                prodPlanNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                prodPlanNotifyImage.Src = "../Images/tick.png";
            }
            prodPlanNotifyLabel.Text = notifyMessage;

            prodPlanNotifyMessageDiv.Visible = true;
            prodPlanNotifyTimer.Enabled = true;
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            prodPlanNotifyMessageDiv.Visible = false;
            prodPlanNotifyTimer.Enabled = false;
        }

        private void fillGridView()
        {
            prodPlanRoleGridView.DataSource = myWebService.fillGridView("Select iD, wcID,wcName,productTypeID,productType, recipeID, name, quantity, shift, dtandTime from vProductionPlanning ORDER BY id Desc", ConnectionOption.SQL);
            prodPlanRoleGridView.DataBind();
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "prodPlanGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                prodPlanIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridIDLabel")).Text);
                productionPlanningWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridWCNameLabel")).Text);

                EventArgs prodPlanDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(productionPlanningWCNameDropDownList, prodPlanDropDownListEventArgs);

                productionPlanningProductTypeDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridProdTypeNameLabel")).Text);
                DropDownList_SelectedIndexChanged(productionPlanningProductTypeDropDownList, prodPlanDropDownListEventArgs);

                productionPlanningRecipeCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridRecipeIDLabel")).Text);
                productionPlanningQuantityTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridQuantityLabel")).Text);
                productionPlanningDateTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridDateTimeLabel")).Text);
                productionPlanningShiftDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("productionPlanningGridShiftLabel")).Text);


            }
            else if (((ImageButton)sender).ID == "visGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Shashank Jain  ||
            //Date Created  : 19 April 2011
            //Date Updated  : 30 March 2011 || 
            //Revision No.  : 01            || 
            //Revision Desc :               || 

            ProdPlanningWCIDLabel.Text = "0";
            ProdPlanningProcessID.Text = "0";
            productTypeIDLabel.Text = "0";
            recipeIDLabel.Text = "0";
            productionPlanningQuantityTextBox.Text = "";

            productionPlanningRecipeCodeTextBox.Text = "";
            productionPlanningRecipeNoText.Text = "";
            productionPlanningUnitOfMeasureLabel.Text = "";
            ppRecipeNoRequiredFieldLabel.Text = "";

            productionPlanningProductTypeDropDownList.Items.Clear();
            productionPlanningWCNameDropDownList.Items.Clear();
            
            fillWorkcenterName();
            FillShift();
            
        }
        
        private void FillShift()
        {
            ArrayList Shifts = new ArrayList();
            Shifts.Add("A");
            Shifts.Add("B");
            Shifts.Add("C");

            productionPlanningShiftDropDownList.DataSource = Shifts;
            productionPlanningShiftDropDownList.DataBind();
        }
        private void DisplayUnit(string Protype)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select name from unitMaster where id=(Select UnitId from producttypeMaster where name=@pTypeName)";
            myConnection.comm.Parameters.AddWithValue("@pTypeName", productionPlanningProductTypeDropDownList.SelectedItem.ToString().Trim());
            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    productionPlanningUnitOfMeasureLabel.Text = myConnection.reader[0].ToString().Trim();
                }
            }
        }
        private void EnableDisableRecipeNoTextBox(string ProcessID)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "Select name from processMaster where id=@ProcessID";
            myConnection.comm.Parameters.AddWithValue("@ProcessID", ProcessID);
            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    if (myConnection.reader[0].ToString().Trim() == "Tyre Curing".Trim())
                    {
                      
                        productionPlanningRecipeNoText.Enabled = true;
                    }
                    else
                    {
                      
                        productionPlanningRecipeNoText.Enabled = false;
                    }
                }
            }
        }

        private void fillWorkcenterName()
        {
            //Description   : Function for filling viWCNameDropDownList with Workcenter Name
            //Author        : Shashank Jain 
            //Date Created  : 19 April 2011                                                             
            //Date Updated  : 19 March 2011 
            //Revision No.  : 01            
            //Revision Desc :  
            productionPlanningWCNameDropDownList.Items.Clear();
            productionPlanningWCNameDropDownList.Items.Add("");

            productionPlanningWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            productionPlanningWCNameDropDownList.DataBind();

            productionPlanningWCNameDropDownList.SelectedValue = myWebService.setWorkCenterName("wcMaster", 89);

           // EventArgs productionPlanningWCNameDropDownListEventArgs = new EventArgs();
          //  DropDownList_SelectedIndexChanged(productionPlanningWCNameDropDownList, productionPlanningWCNameDropDownListEventArgs);
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting in productionPlanning Table
            //Author        : Brajesh kumar
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (productTypeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (ProdPlanningProcessID.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (productionPlanningUnitOfMeasureLabel.Text.Trim() == "")
            {
                flag = 1;
            }

            if (ProdPlanningWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (productionPlanningProductTypeDropDownList.SelectedValue.Trim() == "")
            {
                flag = 1;
            }
            if (productionPlanningWCNameDropDownList.SelectedValue.Trim() == "")
            {
                flag = 1;
            }
            if (productionPlanningRecipeNoText.Enabled == true)
            {
                if (productionPlanningRecipeNoText.Text.ToString().Trim() == "")
                {
                    flag = 1;
                    ppRecipeNoRequiredFieldLabel.Text = "Recipe No is Required";

                }
                               
            }
            return flag;
        }
        private void FillPrototype(string wcID)
        {
            //Description   : Function for filling viWCNameDropDownList with Workcenter Name
            //Author        : Shashank Jain 
            //Date Created  : 30 March 2011                                                             
            //Date Updated  : 30 March 2011 
            //Revision No.  : 01            
            //Revision Desc :  
            productionPlanningProductTypeDropDownList.Items.Clear();
            productionPlanningProductTypeDropDownList.Items.Add("");

            productionPlanningProductTypeDropDownList.DataSource = myWebService.FillDropDownList("producttypeMaster", "name", "WHERE wcID = " + wcID + "");
            productionPlanningProductTypeDropDownList.DataBind();

            //EventArgs productionPlanningProductTypeDropDownListEventArgs = new EventArgs();
           // DropDownList_SelectedIndexChanged(productionPlanningProductTypeDropDownList, productionPlanningProductTypeDropDownListEventArgs);
        }
        #endregion

    

    }
}
