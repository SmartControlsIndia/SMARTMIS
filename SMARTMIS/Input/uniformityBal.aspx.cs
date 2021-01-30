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
    public partial class uniformityBal : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        string[] status = new string[] { "Pending", "OK", "Not OK" };
        string[] testResult = new string[] {"OK", "Not OK" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                inputDataSet.createDataSet();
                fillWorkcenterName();
                fillGridView();
                fillTestResult();
                fillStatus();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());

            if (((DropDownList)sender).ID == "uniBalWCNameDropDownList")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", uniBalWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    uniBalWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close();

                fillReasonName(uniBalWCIDLabel.Text.Trim());
            }


            else if (((DropDownList)sender).ID == "uniBalReasonNameDropDownList")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from reasonMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", uniBalReasonNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    uniBalReasonIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close();
            }
            else if (((DropDownList)sender).ID == "xRayStatusDropDownList")
            {

            }

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "uniBalSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "uniBalCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "uniBalDialogOKButton")
            {
                uniBalIDLabel.Text = uniBalIDHidden.Value; //Passing value to xRayIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "uniBalGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                uniBalIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridIDLabel")).Text);
                uniBalWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridWCNameLabel")).Text);
                uniBalGTBarcodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridGTBarcodeLabel")).Text);
                uniBalRecipeCodeTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridRecipeCodeIDLabel")).Text);

                uniBalTestResultDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridTestResultLabel")).Text);
                uniBalReasonNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridReasonNameLabel")).Text);
                uniBalStatusDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridStatusLabel")).Text);

            }

            else if (((ImageButton)sender).ID == "uniBalGridDeleteImageButton")
            {
                //Code for deleting gridview row
            }

            else
            { }
        }

        private int delete()
        {

            //Description   : Function for deleting record in tyreXRay Table
            //Author        : Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (uniBalIDLabel.Text.Trim() != "0")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From unibalrunouttbr WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", uniBalIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close();

                Notify(1, "Uniformity record deleted successfully");
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
                uniBalNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                uniBalNotifyImage.Src = "../Images/tick.png";
            }
            uniBalNotifyLabel.Text = notifyMessage;

            uniBalNotifyMessageDiv.Visible = true;
            uniBalNotifyTimer.Enabled = true;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || Add logic for Clear xRayStatusDropDownList

            uniBalIDLabel.Text = "0";
            uniBalWCIDLabel.Text = "0";
            uniBalGTBarcodeTextBox.Text = "";
            uniBalRecipeCodeTextBox.Text = "";
            uniBalReasonIDLabel.Text = "0";

            fillWorkcenterName();
            fillStatus();
            fillTestResult();

        }

        private int save()
        {
            //Description   : Function for saving and updating record in tyreXRay Table
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || change the logic for row[9]

            int flag = 0;

            if (uniBalIDLabel.Text.Trim() == "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open();
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into unibalrunouttbr (gtbarCode, recipeCode, testResult,manningID, status,reasonID, wcID, dtandTime) values (@gtbarCode, @recipeCode, @testResult,@manningID, @status,@reasonID, @wcID, @dtandTime)";

                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", uniBalGTBarcodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", "R19C29");
                    myConnection.comm.Parameters.AddWithValue("@testResult", uniBalTestResultDropDownList.SelectedIndex);
                    myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                    myConnection.comm.Parameters.AddWithValue("@status", uniBalStatusDropDownList.SelectedIndex);
                    myConnection.comm.Parameters.AddWithValue("@reasonID", uniBalReasonIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@wcID", uniBalWCIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", "R19C29");
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now.ToLongDateString()));
                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close();

                    Notify(1, "X-Ray record saved successfully");

                }
            }

            else if (uniBalIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open();
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update unibalrunouttbr SET wcID = @wcID, gtbarCode = @gtbarCode, testResult=@testResult, reasonID = @reasonID, manningID = @manningID, status=@status, dtandTime=@dtandTime WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", uniBalGTBarcodeTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", "R19C29");
                    myConnection.comm.Parameters.AddWithValue("@testResult", uniBalTestResultDropDownList.SelectedIndex);
                    myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                    myConnection.comm.Parameters.AddWithValue("@status", uniBalStatusDropDownList.SelectedIndex);
                    myConnection.comm.Parameters.AddWithValue("@reasonID", uniBalReasonIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@wcID", uniBalWCIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeCode", "R19C29");
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now.ToLongDateString()));

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close();

                    Notify(1, "X-Ray record updated successfully");
                }
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in tyreXRay Table
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || Add a flag for xRayStatusDropDownList

            int flag = 0;

            if (uniBalWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (uniBalGTBarcodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            if (uniBalReasonIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (uniBalStatusDropDownList.SelectedValue.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void fillWorkcenterName()
        {

            //Description   : Function for filling xRayWCNameDropDownList with Workcenter Name
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 01 April 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || Change the Logic for filling the xRayWCNameDropDownList

            uniBalReasonNameDropDownList.Items.Clear();
            uniBalReasonNameDropDownList.Items.Add("");

            uniBalWCNameDropDownList.Items.Clear();
            uniBalWCNameDropDownList.Items.Add("");

            uniBalWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "");
            uniBalWCNameDropDownList.DataBind();


        }

        private void fillGridView()
        {

            //Description   : Function for filling xRayGridView
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || change the logic for row[9]

            inputDataSet.clearUniBalTable();

            myConnection.open();
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select iD, wcID, wcName, gtbarCode, recipeCode, csvFileData, testResult, reasonID, reasonName, status, manningID from vUniBalTBR";

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                DataRow row = inputDataSet.inputDataset.Tables["tUniBal"].NewRow();

                row[0] = myConnection.reader[0].ToString();
                row[1] = myConnection.reader[1].ToString();
                row[2] = myConnection.reader[2].ToString();
                row[3] = myConnection.reader[3].ToString();
                row[4] = myConnection.reader[4].ToString();
                row[5] = myConnection.reader[5].ToString();
                row[6] = myConnection.reader[6].ToString();
                row[7] = myConnection.reader[7].ToString();
                row[8] = myConnection.reader[8].ToString();
                row[9] = myConnection.reader[9].ToString();
                row[10] = myConnection.reader[10].ToString();

                inputDataSet.inputDataset.Tables["tUniBal"].Rows.Add(row);
            }

            uniBalGridView.DataSource = inputDataSet.uniBal;
            uniBalGridView.DataBind();

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close();
        }

        private void fillReasonName(string wcID)
        {
            //Description   : Function for filling xRayReasonNameDropDownList with Reason Name Workcenter wise
            //Author        : Rohit Singh    || Rohit Singh
            //Date Created  : 27 March 2011 
            //Date Updated  : 27 March 2011  || 01 April 2011
            //Revision No.  : 01             || 02
            //Revision Desc :                || Change the Logic for filling the xRayReasonNameDropDownList

            uniBalReasonNameDropDownList.Items.Clear();
            uniBalReasonNameDropDownList.Items.Add("");

            uniBalReasonNameDropDownList.DataSource = myWebService.FillDropDownList("reasonMaster", "name", "WHERE wcID = " + wcID + "");
            uniBalReasonNameDropDownList.DataBind();

        }

        private void fillStatus()
        {

            //Description   : Function for filling xRayStatusDropDownList with Status Name
            //Author        : Rohit Singh
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            uniBalStatusDropDownList.Items.Clear();
            uniBalStatusDropDownList.Items.Add("");

            foreach (string item in status)
            {
                uniBalStatusDropDownList.Items.Add(item);
            }
        }

        private void fillTestResult()
        {

            //Description   : Function for filling xRayStatusDropDownList with Status Name
            //Author        : Rohit Singh
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            uniBalTestResultDropDownList.Items.Clear();
            uniBalTestResultDropDownList.Items.Add("");

            foreach (string item in testResult)
            {
                uniBalTestResultDropDownList.Items.Add(item);
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        