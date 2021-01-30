using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SmartMIS.SmartWebReference;


namespace SmartMIS
{
    public partial class processCheckParametersInput : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillWorkCenterName();
                fillGridView();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());



            if (((DropDownList)sender).ID == "inputProcessCheckParameterWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", inputProcessCheckParameterWCNameDropDownList.SelectedValue.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    inputProcessCheckParameterWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                fillProductTypeName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else if (((DropDownList)sender).ID == "inputProcessCheckParameterProductTypeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from producttypeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", inputProcessCheckParameterProductTypeDropDownList.SelectedValue.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    inputProcessCheckParameterProductTypeIDLabel.Text = myConnection.reader[0].ToString();
                }

                fillRecipeName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }




            else if (((DropDownList)sender).ID == "inputProcessCheckParameterRecipeNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from recipeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", inputProcessCheckParameterRecipeNameDropDownList.SelectedValue.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    inputProcessCheckParameterRecipeIDLabel.Text = myConnection.reader[0].ToString();
                }
                fillParameterName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }


            else if (((DropDownList)sender).ID == "inputProcessCheckParameterNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from processCheckParameterMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", inputProcessCheckParameterNameDropDownList.SelectedValue.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    inputProcessCheckParameterMasterIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }



        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "inputProcessCheckParameterSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "inputProcessCheckParameterCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "inputProcessCheckParameterDialogOKButton")
            {
                inputProcessCheckParameterIDLabel.Text = inputProcessCheckParameterIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "inputProcessCheckParameterGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                inputProcessCheckParameterIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridIDLabel")).Text);
                inputProcessCheckParameterWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridWCIDLabel")).Text);
                inputProcessCheckParameterWCNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridWCNameLabel")).Text); 
                EventArgs inputProcessCheckParameterDropDownListEventArgs = new EventArgs();                
                DropDownList_SelectedIndexChanged(inputProcessCheckParameterWCNameDropDownList, inputProcessCheckParameterDropDownListEventArgs);

                inputProcessCheckParameterProductTypeDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridProductTypeLabel")).Text);
                inputProcessCheckParameterProductTypeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridProductTypeIDLabel")).Text);

                DropDownList_SelectedIndexChanged(inputProcessCheckParameterProductTypeDropDownList, inputProcessCheckParameterDropDownListEventArgs);

                inputProcessCheckParameterRecipeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridRecipeIDLabel")).Text);
                inputProcessCheckParameterRecipeNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridRecipeNameLabel")).Text);
                DropDownList_SelectedIndexChanged(inputProcessCheckParameterRecipeNameDropDownList, inputProcessCheckParameterDropDownListEventArgs);

                inputProcessCheckParameterMasterIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParametermMasterGridIDLabel")).Text);
                inputProcessCheckParameterNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridNameLabel")).Text);

                DropDownList_SelectedIndexChanged(inputProcessCheckParameterNameDropDownList, inputProcessCheckParameterDropDownListEventArgs);                              
                
                inputProcessCheckParameterValueTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridValueLabel")).Text);
                inputProcessCheckParameterDateTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("inputProcessCheckParameterGridDateLabel")).Text);

            }
            else if (((ImageButton)sender).ID == " processCheckParameterGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            inputProcessCheckParameterNotifyMessageDiv.Visible = false;
            inputProcessCheckParameterNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillWorkCenterName()
        {

            //Description   : Function for filling wcNameDropDownList 
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01



            inputProcessCheckParameterWCNameDropDownList.Items.Clear();
            inputProcessCheckParameterWCNameDropDownList.Items.Add("");
            inputProcessCheckParameterWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            inputProcessCheckParameterWCNameDropDownList.DataBind();
        }
        private void fillProductTypeName()
        {

            //Description   : Function for filling productTypeNameDropDownList 
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22April 2011
            //Revision No.  : 01

            inputProcessCheckParameterProductTypeDropDownList.Items.Clear();
            inputProcessCheckParameterProductTypeDropDownList.Items.Add("");

            inputProcessCheckParameterProductTypeDropDownList.DataSource = myWebService.FillDropDownList("producttypeMaster", "name", "where wcID='" + inputProcessCheckParameterWCIDLabel.Text.ToString().Trim() + "'");
           
            inputProcessCheckParameterProductTypeDropDownList.DataBind();

        }

        private void fillRecipeName()
        {

            //Description   : Function for filling productTypeNameDropDownList with productType
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            inputProcessCheckParameterRecipeNameDropDownList.Items.Clear();
            inputProcessCheckParameterRecipeNameDropDownList.Items.Add("");

            inputProcessCheckParameterRecipeNameDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "Name", "where productTypeID='" + inputProcessCheckParameterProductTypeIDLabel.Text.ToString().Trim() + "'");
           // myConnection.comm.Parameters.AddWithValue("@productTypeID", inputProcessCheckParameterProductTypeDropDownList.SelectedItem.ToString().Trim());

            inputProcessCheckParameterRecipeNameDropDownList.DataBind();
        }




        private void fillParameterName()
        {

            //Description   : Function for filling productTypeNameDropDownList with productType
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            inputProcessCheckParameterNameDropDownList.Items.Clear();
            inputProcessCheckParameterNameDropDownList.Items.Add("");

            inputProcessCheckParameterNameDropDownList.DataSource = myWebService.FillDropDownList("processCheckParameterMaster", "name", "where recipeID='" + inputProcessCheckParameterRecipeIDLabel.Text.ToString().Trim() + "'");
           // myConnection.comm.Parameters.AddWithValue("@recipeID", inputProcessCheckParameterRecipeNameDropDownList.SelectedItem.ToString().Trim());

            inputProcessCheckParameterNameDropDownList.DataBind();
        }


        private void fillGridView()
        {

            //Description   : Function for filling processCheckParameterGridView
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            inputProcessCheckParameterGridView.DataSource = myWebService.fillGridView("Select * from vProcessCheckParameterInput", ConnectionOption.SQL);
            inputProcessCheckParameterGridView.DataBind();


            
        }

        
        

        private int save()
        {
            //Description   : Function for saving and updating record in processCheckParameterMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            int flag = 0;
           int notifyIcon=0;


           if (inputProcessCheckParameterIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) &&(myWebService.IsRecordExist("processCheckParameterInput","parameterValue","where parameterValue='"+inputProcessCheckParameterValueTextBox.Text.Trim()+"'",out notifyIcon)==false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into processCheckParameterInput (processCheckParameterID, parameterValue, dtandTime) values (@processCheckParameterID, @parameterValue, @dtandTime)";
                    myConnection.comm.Parameters.AddWithValue("@processCheckParameterID", Convert.ToInt32(inputProcessCheckParameterMasterIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@parameterValue", (inputProcessCheckParameterValueTextBox.Text.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", (inputProcessCheckParameterDateTextBox.Text.ToString().Trim()));
                   
                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "processCheckParameter saved successfully");
                }

                else
                {
                    Notify(notifyIcon, "Parametervalue already exists");
                }

            }
           else if (inputProcessCheckParameterIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Update processCheckParameterInput SET processCheckParameterID = @processCheckParameterID, parameterValue = @parameterValue, dtandTime = @dtandTime WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@processCheckParameterID", Convert.ToInt32(inputProcessCheckParameterMasterIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@parameterValue", (inputProcessCheckParameterValueTextBox.Text.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", (inputProcessCheckParameterDateTextBox.Text.ToString().Trim()));
                   

                    myConnection.comm.Parameters.AddWithValue("@iD", inputProcessCheckParameterIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "processCheckParameterValue updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in processCheckParameterMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (inputProcessCheckParameterIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From processCheckParameterInput WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", inputProcessCheckParameterIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "processCheckParameterValue deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in processCheckParameterMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (inputProcessCheckParameterWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (inputProcessCheckParameterProductTypeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (inputProcessCheckParameterRecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (inputProcessCheckParameterMasterIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (inputProcessCheckParameterValueTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            if (inputProcessCheckParameterDateTextBox.Text.Trim() == "")
            {
                flag = 1;
            }




            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01

            inputProcessCheckParameterWCIDLabel.Text = "0";
            inputProcessCheckParameterProductTypeIDLabel.Text = "0";
            inputProcessCheckParameterRecipeIDLabel.Text = "0";
            inputProcessCheckParameterMasterIDLabel.Text = "0";
            fillWorkCenterName();
            fillProductTypeName();
            fillRecipeName();
            fillParameterName();
            inputProcessCheckParameterIDLabel.Text = "0";
            inputProcessCheckParameterValueTextBox.Text = "";
            inputProcessCheckParameterDateTextBox.Text = "";


        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in processCheckParameterMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                inputProcessCheckParameterNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                inputProcessCheckParameterNotifyImage.Src = "../Images/tick.png";
            }
            inputProcessCheckParameterNotifyLabel.Text = notifyMessage;

            inputProcessCheckParameterNotifyMessageDiv.Visible = true;
            inputProcessCheckParameterNotifyTimer.Enabled = true;
        }

        #endregion

    }
}
