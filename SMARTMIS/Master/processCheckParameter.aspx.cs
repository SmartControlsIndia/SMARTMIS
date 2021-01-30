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
    public partial class processCheckParameters : System.Web.UI.Page
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



            if (((DropDownList)sender).ID == "processCheckParameterWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", processCheckParameterWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    processCheckParameterWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                fillProductTypeName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else if (((DropDownList)sender).ID == "processCheckParameterProductTypeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from producttypeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", processCheckParameterProductTypeDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    processCheckParameterProductIDLabel.Text = myConnection.reader[0].ToString();
                }

                fillRecipeName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }




            else if (((DropDownList)sender).ID == "processCheckParameterRecipeNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from recipeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", processCheckParameterRecipeNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    processCheckParameterRecipeIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }






        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "processCheckParameterSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "processCheckParameterCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "processCheckParameterDialogOKButton")
            {
                processCheckParameterIDLabel.Text = processCheckParameterIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "processCheckParameterGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                processCheckParameterIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridIDLabel")).Text);
                processCheckParameterWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridWCIDLabel")).Text);
                processCheckParameterProductIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridProductTypeIDLabel")).Text);
                processCheckParameterWCNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridWCNameLabel")).Text);
                EventArgs processCheckParameterDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(processCheckParameterWCNameDropDownList, processCheckParameterDropDownListEventArgs);
                processCheckParameterProductTypeDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridProductTypeLabel")).Text);
                
                DropDownList_SelectedIndexChanged(processCheckParameterProductTypeDropDownList, processCheckParameterDropDownListEventArgs);
                processCheckParameterRecipeNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridRecipeNameLabel")).Text);
                processCheckParameterNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridNameLabel")).Text);
                processCheckParameterDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("processCheckParameterGridDescriptionLabel")).Text);

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
            processCheckParameterNotifyMessageDiv.Visible = false;
            processCheckParameterNotifyTimer.Enabled = false;
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

            processCheckParameterWCNameDropDownList.Items.Clear();
            processCheckParameterWCNameDropDownList.Items.Add("");

            processCheckParameterWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            processCheckParameterWCNameDropDownList.DataBind();
         
        }

        private void fillProductTypeName()
        {

            //Description   : Function for filling productTypeNameDropDownList 
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22April 2011
            //Revision No.  : 01


            processCheckParameterProductTypeDropDownList.Items.Clear();
            processCheckParameterProductTypeDropDownList.Items.Add("");

            processCheckParameterProductTypeDropDownList.DataSource = myWebService.FillDropDownList("producttypeMaster", "name", "where wcID='" + processCheckParameterWCIDLabel.Text.ToString().Trim() + "'");

            processCheckParameterProductTypeDropDownList.DataBind();

        }

        private void fillRecipeName()
        {

            //Description   : Function for filling productTypeNameDropDownList with productType
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            processCheckParameterRecipeNameDropDownList.Items.Clear();
            processCheckParameterRecipeNameDropDownList.Items.Add("");

            processCheckParameterRecipeNameDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "Name", "where productTypeID='" + processCheckParameterProductIDLabel.Text.ToString().Trim() + "'");

            processCheckParameterRecipeNameDropDownList.DataBind();
        }

        private void fillGridView()
        {

            //Description   : Function for filling processCheckParameterGridView
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            processCheckParameterGridView.DataSource = myWebService.fillGridView("Select * from vProcessCheckParameter", "iD", smartMISWebService.order.Desc);
            processCheckParameterGridView.DataBind();



       
        }

        private int save()
        {
            //Description   : Function for saving and updating record in processCheckParameterMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 22 April 2011
            //Date Updated  : 22 April 2011
            //Revision No.  : 01


            int flag = 0;
            int notifyIcon = 0;
            if (processCheckParameterIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("processCheckParameterMaster", "name", "where name='" + processCheckParameterNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into processCheckParameterMaster (wcID, productTypeID, recipeID, name, description) values (@wcID, @productTypeID, @recipeID, @name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(processCheckParameterWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(processCheckParameterProductIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(processCheckParameterRecipeIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", processCheckParameterNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", processCheckParameterDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "processCheckParameter saved successfully");
                }


                else
                {
                    Notify(notifyIcon, "Parameter already exists");
                }

            }
            else if (processCheckParameterIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update processCheckParameterMaster SET wcID = @wcID, productTypeID = @productTypeID, recipeID=@recipeID, name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(processCheckParameterWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(processCheckParameterProductIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(processCheckParameterRecipeIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", processCheckParameterNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", processCheckParameterDescriptionTextBox.Text.Trim());

                    myConnection.comm.Parameters.AddWithValue("@iD", processCheckParameterIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "processCheckParameter updated successfully");
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

            if (processCheckParameterIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From processCheckParameterMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", processCheckParameterIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "processCheckParameter deleted successfully");
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

            if (processCheckParameterWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (processCheckParameterProductIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (processCheckParameterNameTextBox.Text.Trim() == "")
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

            processCheckParameterWCIDLabel.Text = "0";
            processCheckParameterProductIDLabel.Text = "0";
            processCheckParameterNameTextBox.Text = "";
            fillWorkCenterName();
            fillProductTypeName();
            fillRecipeName();
            processCheckParameterIDLabel.Text = "0";
            processCheckParameterDescriptionTextBox.Text = "";
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
                processCheckParameterNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                processCheckParameterNotifyImage.Src = "../Images/tick.png";
            }
            processCheckParameterNotifyLabel.Text = notifyMessage;

            processCheckParameterNotifyMessageDiv.Visible = true;
            processCheckParameterNotifyTimer.Enabled = true;
        }

        #endregion

    }
}
