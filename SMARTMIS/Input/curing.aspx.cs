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

namespace SmartMIS.Input
{
    public partial class curing : System.Web.UI.Page
    {
        string[] pressDirection = new string[] { "Left", "Right" };

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                inputDataSet.createDataSet();
                fillGridView();
                fillWorkcenterName();
                fillPressDirection();
            }
        }

        protected void curingWCNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());

            if (((DropDownList)sender).ID == "curingWCNameDropDownList")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", curingWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    curingWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close();
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "curingSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "curingCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "curingDialogOKButton")
            {
                curingIDLabel.Text = curingIDHidden.Value; //Passing value to viIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "curingGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;
                curingIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridIDLabel")).Text);
                curingWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridWCIDLabel")).Text);
                curingGTBarcodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridGTBarcodeLabel")).Text);
                curingPressBarCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridPressBarCodeIDLabel")).Text);
                curingRecipecodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridPressBarCodeIDLabel")).Text);
                curingMouldNoTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridMouldNoLabel")).Text);
                curingRecipecodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("curingGridRecipeCodeLabel")).Text);
                curingWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("curingGridWCNameLabel")).Text);
                curingPressDirDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("curingGridPressDirectionLabel")).Text);

            }
            else if (((ImageButton)sender).ID == "visGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }


        #endregion


        #region User Defined Function

        private void fillGridView()
        {

            //Description   : Function for filling viGridView
            //Author        : Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01
            //Revision Desc :

            inputDataSet.clearCuringTable();

            myConnection.open();
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select iD, wcID, wcName, gtbarCode, pressbarCode, recipeCode, mouldNo, pressDirection, manningID from vCuringtbr";

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                DataRow row = inputDataSet.inputDataset.Tables["tCuringTBR"].NewRow();

                row[0] = myConnection.reader[0].ToString();
                row[1] = myConnection.reader[1].ToString();
                row[2] = myConnection.reader[2].ToString();
                row[3] = myConnection.reader[3].ToString();
                row[4] = myConnection.reader[4].ToString();
                row[5] = myConnection.reader[5].ToString();
                row[6] = myConnection.reader[6].ToString();
                row[7] = myConnection.reader[7].ToString();
                row[8] = myConnection.reader[8].ToString();

                inputDataSet.inputDataset.Tables["tCuringTBR"].Rows.Add(row);
            }

            curingGridView.DataSource = inputDataSet.curing;
            curingGridView.DataBind();

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close();
        }

        private void fillWorkcenterName()
        {

            //Description   : Function for filling viWCNameDropDownList with Workcenter Name
            //Author        : Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01

            curingWCNameDropDownList.Items.Clear();
            curingWCNameDropDownList.Items.Add("");

            myConnection.open();
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select name from wcMaster";

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                curingWCNameDropDownList.Items.Add(myConnection.reader[0].ToString());
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close();
        }

        private int delete()
        {

            //Description   : Function for deleting record in vInspectionTBR Table
            //Author        : Rohit Singh
            //Date Created  : 31 March 2011
            //Date Updated  : 31 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (curingIDLabel.Text.Trim() != "0")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From curingtbr WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", curingIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close();

                Notify(1, "Curing record deleted successfully");
            }

            return flag;
        }


        private void fillPressDirection()
        {

            //Description   : Function for filling viStatusDropDownList with Status Name
            //Author        : Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01

            curingPressDirDropDownList.Items.Clear();
            curingPressDirDropDownList.Items.Add("");

            foreach (string item in pressDirection)
            {
                curingPressDirDropDownList.Items.Add(item);
            }
        }

        private int save()
        {
            //Description   : Function for saving and updating record in vInspectiontbr Table
            //Author        : Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01
            //Revision Desc :

            int flag = 0;

            if (curingIDLabel.Text.Trim() == "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open();
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into curingtbr (gtbarCode, pressbarCode, recipeCode, manningID, mouldNo, pressDirection, wcID, dtandTime) values (@gtbarCode,@pressbarCode, @recipeCode, @manningID, @mouldNo, @pressDirection, @wcID, @dtandTime)";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", curingGTBarcodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@pressbarCode", curingPressBarCodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", curingRecipecodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                    myConnection.comm.Parameters.AddWithValue("@mouldNo", curingMouldNoTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@pressDirection", curingPressDirDropDownList.SelectedValue.ToString());
                    myConnection.comm.Parameters.AddWithValue("@wcID", curingWCIDLabel.Text.Trim());                          
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now.ToLongDateString()));

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close();

                    Notify(1, "Curing record saved successfully");
                }
            }
            else if (curingIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open();
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update curingtbr SET wcID = @wcID, gtbarCode = @gtbarCode,pressbarCode = @pressbarCode, recipeCode = @recipeCode, manningID = @manningID, mouldNo = @mouldNo, pressDirection = @pressDirection, dtandTime=@dtandTime  WHERE (iD = @iD)";

                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", curingGTBarcodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@pressbarCode", curingPressBarCodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", curingRecipecodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                    myConnection.comm.Parameters.AddWithValue("@mouldNo", curingMouldNoTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@pressDirection", curingPressDirDropDownList.SelectedValue.ToString());
                    myConnection.comm.Parameters.AddWithValue("@wcID", curingWCIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now.ToLongDateString()));
                    myConnection.comm.Parameters.AddWithValue("@iD", curingIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close();

                    Notify(1, "Curing record updated successfully");
                }
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in vInspectionTBR Table
            //Author        : Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (curingWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (curingGTBarcodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            if (curingPressBarCodeTextBox.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (curingRecipecodeTextBox.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (curingMouldNoTextBox.Text.Trim() == "0")
            {
                flag = 1;
            }        

            return flag;
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
                curingNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                curingNotifyImage.Src = "../Images/tick.png";
            }
            curingNotifyLabel.Text = notifyMessage;

            curingNotifyMessageDiv.Visible = true;
            curingNotifyTimer.Enabled = true;
        }


        private void clearPage()
        {
            //Description   : Function for clearing controls and variables of Page
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011 || 31 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || Add the logic to clear the viFaultDirectionDropDownList

            curingIDLabel.Text = "0";
            curingWCIDLabel.Text = "0";
            curingGTBarcodeTextBox.Text = "";
            curingPressBarCodeTextBox.Text = "";
            curingMouldNoTextBox.Text = "";
            curingRecipecodeTextBox.Text = "";
            fillWorkcenterName();
            fillPressDirection();
          
        }



        #endregion

  
    }
}
