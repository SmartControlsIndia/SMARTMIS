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
using System.IO;
using System;


namespace SmartMIS.Input
{
    public partial class ColorCode : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        DataTable dt = new DataTable();
        string Uid = "tbmuser";
        string upassword = "smart";// new pwd= jk@12345

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGridView();

            }


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
                //e.Row.Cells[3].Controls.Add(lb);
                // e.Row.Cells[12].Controls.Add(lbdelete);
            }
        }
        private void fillRecipeCodes()
        {
            bdrecipeCodeDropDownList.Items.Clear();
            bdrecipeCodeDropDownList.Items.Add("");
            if (processDropDownList.SelectedItem.Text == "TBMTBR")
            {
                bdrecipeCodeDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "name", "WHERE ProcessId = '4'");
                fillGridviewProcessWise("4");
                clearPage();
                bdBarcodeTextBox1.Enabled = false;
            }
            else
            {
                bdrecipeCodeDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "name", "WHERE ProcessId = '7'");
                fillGridviewProcessWise("7");
                clearPage();
                bdBarcodeTextBox1.Enabled = true;
            }
            bdrecipeCodeDropDownList.DataBind();

        }
        private void fillGridviewProcessWise(string processType)
        {
            try
            {
                DataTable pdt = new DataTable();

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select CASE WHEN processID='4' THEN 'TBMTBR' WHEN processID='7' THEN 'TBMPCR' END AS processName , recipeCode,colorCode,numberOfColor,colorName,updateDate from RecipeWiseColorCode where processID='" + processType + "'order by id desc";
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
                recipeGridView.Rows[0].Cells[4].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[5].Width = new Unit(7, UnitType.Percentage);

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
            //Author        : Saccchin S Gupta  ||
            //Date Created  : 31 march 2021 ||
            //Date Updated  : 31 march 2021 || 
            //Revision No.  : 01            || 02
            //Revision Desc :  new created  || logic for filling wcGridView by webservice
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = " select CASE WHEN processID='4' THEN 'TBMTBR' WHEN processID='7' THEN 'TBMPCR' END AS processName , recipeCode,colorCode,numberOfColor,colorName,updateDate from RecipeWiseColorCode order by iD desc";
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt.Load(myConnection.reader);

            if (!myConnection.reader.IsClosed)
                myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            recipeGridView.DataSource = dt;
            recipeGridView.DataBind();
            if (dt.Rows.Count > 0)
            {
                //recipeGridView.Columns[4].ItemStyle.Width = new Unit(50, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[0].Width = new Unit(8, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[1].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[2].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[3].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[4].Width = new Unit(5, UnitType.Percentage);
                recipeGridView.Rows[0].Cells[5].Width = new Unit(5, UnitType.Percentage);

            }
            ViewState["dt"] = dt;
        }




        protected void Button_Click(object sender, EventArgs e)
        {

            if (((Button)sender).ID == "bdSaveButton")
            {

                string k = "";

                string vColorC = DropDownList1.SelectedItem.Value.ToString();
                if (vColorC == "Yes")
                {
                    string vNoC = DropDownList2.SelectedItem.Value.ToString();
                 
                    if(vNoC !="Select"){
                        
                           string vRed = ddlRed.SelectedItem.Value.ToString();
                            if (vRed != "Select")
                            { k = k + vRed + ","; }
                            string vBlue = ddlBlue.SelectedItem.Value.ToString();
                            if (vBlue != "Select")
                            { k = k + vBlue + ","; }
                            string vYellow = ddlYellow.SelectedItem.Value.ToString();
                            if (vYellow != "Select")
                            { k = k + vYellow + ","; }
                            string vGreen = ddlGreen.SelectedItem.Value.ToString();
                            if (vGreen != "Select")
                            { k = k + vGreen + ","; }
                             if (vRed == "Select" && vBlue == "Select" && vYellow == "Select" && vGreen == "Select") {
                              Notify(0, "Please select colour name ?");
                                }
                                else
                                {
                                    lbmsg.Text = "Last Selected Color Code : " + k.TrimEnd(',');
                                    lbmsg.ForeColor = System.Drawing.Color.ForestGreen;
                                    save(k.TrimEnd(','));
                                }
                    }
                    else{
                     Notify(0, "Please select any no. of colour ?");
                    }
                }

            }
            else if (((Button)sender).ID == "bdCancelButton")
            {
                clearPage();
            }
        }

        private int save(string k)
        {
            int flag = 0;
            int notifyIcon = 0;

            try
            {
                if (processDropDownList.SelectedItem.Text == "TBMTBR")
                {
                    if ((myWebService.IsRecordExist("RecipeWiseColorCode", "recipeCode", "where( recipeCode='" + bdrecipeCodeDropDownList.SelectedItem.Text.Trim() + "')", out notifyIcon) == false))
                    {

                        int processID = 4;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into RecipeWiseColorCode (processID,recipeID,recipeCode,colorCode,numberOfColor,colorName,insertDate,updateDate) values (@processID,@recipeID, @recipeCode,@colorCode,@numberOfColor,@colorName,@dtandtime,@dtandtime1)";
                        myConnection.comm.Parameters.AddWithValue("@processID", processID);
                        myConnection.comm.Parameters.AddWithValue("@recipeID", 1);
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorCode", DropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@numberOfColor", DropDownList2.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorName", k.ToString());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myConnection.comm.Parameters.AddWithValue("@dtandtime1", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record saved successfully");
                        fillGridviewProcessWise("4");
                        clearPage();

                    }
                    else
                    {
                        int processID = 4;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "update RecipeWiseColorCode set colorCode=@colorCode,numberOfColor=@numberOfColor,colorName=@colorName,updateDate=@dtandtime where recipeCode=@recipeCode";
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorCode", DropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@numberOfColor", DropDownList2.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorName", k.ToString());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record updated successfully");
                        fillGridviewProcessWise("4");
                        clearPage();
                        //Notify(notifyIcon, "Record already exists for " + bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                    }
                }
                else if (processDropDownList.SelectedItem.Text == "TBMPCR")
                {
                    if ((myWebService.IsRecordExist("RecipeWiseColorCode", "recipeCode", "where( recipeCode='" + bdrecipeCodeDropDownList.SelectedItem.Text.Trim() + "')", out notifyIcon) == false))
                    {

                        int processID = 7;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into RecipeWiseColorCode (processID,recipeID,recipeCode,colorCode,numberOfColor,colorName,insertDate,updateDate) values (@processID,@recipeID, @recipeCode,@colorCode,@numberOfColor,@colorName,@dtandtime,@dtandtime1)";
                        myConnection.comm.Parameters.AddWithValue("@processID", processID);
                        myConnection.comm.Parameters.AddWithValue("@recipeID", 1);
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorCode", DropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@numberOfColor", DropDownList2.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorName", k.ToString());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myConnection.comm.Parameters.AddWithValue("@dtandtime1", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        flag = myConnection.comm.ExecuteNonQuery();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record saved successfully");
                        fillGridviewProcessWise("7");
                        clearPage();
                    }
                    else
                    {
                        int processID = 7;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "update RecipeWiseColorCode set colorCode=@colorCode,numberOfColor=@numberOfColor,colorName=@colorName,updateDate=@dtandtime where recipeCode=@recipeCode";
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorCode", DropDownList1.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@numberOfColor", DropDownList2.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@colorName", k.ToString());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record updated successfully");
                        fillGridviewProcessWise("7");
                        clearPage();
                        //Notify(notifyIcon, "Record already exists for "+bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }


            //}
            return flag;

        }
        private void Notify(int notifyIcon, string notifyMessage)
        {
            if (notifyIcon == 0)
            {
                bdNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                bdNotifyImage.Src = "../Images/tick.png";
            }
            bdNotifyLabel.Text = notifyMessage;

            bdNotifyMessageDiv.Visible = true;
            bdNotifyTimer.Enabled = true;
        }
        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            bdNotifyMessageDiv.Visible = false;
            bdNotifyTimer.Enabled = false;
        }
        private int validation()
        {

            int flag = 0;



            if (bdrecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }




            if (bdBarcodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }
        private void clearPage()
        {

            bdrecipeIDLabel.Text = "0";
            bdBarcodeTextBox.Text = "0";
            bdBarcodeTextBox1.Text = "0";

            bdrecipeCodeDropDownList.SelectedIndex = 0;
            ddlRed.SelectedIndex = 0;
            ddlBlue.SelectedIndex = 0;
            ddlGreen.SelectedIndex = 0;
            ddlYellow.SelectedIndex = 0;
            DropDownList1.SelectedIndex = 0;
            DropDownList2.SelectedIndex = 0;


        }

        protected void processDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {


            fillRecipeCodes();

            bdBarcodeTextBox.Text = "0";


        }

    }
}

