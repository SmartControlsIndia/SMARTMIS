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
using System.IO;


namespace SmartMIS.Input
{
    public partial class CycleTime : System.Web.UI.Page
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
                myConnection.comm.CommandText = "select CASE WHEN processID='4' THEN 'TBMTBR' WHEN processID='7' THEN 'TBMPCR' END AS processName , recipeCode,cycleTime1,cycleTime2,updateDate from RecipeMasterCycleTime where processID='" + processType + "'order by id desc";
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
               // recipeGridView.Rows[0].Cells[4].Width = new Unit(7, UnitType.Percentage);
                
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
            myConnection.comm.CommandText = " select CASE WHEN processID='4' THEN 'TBMTBR' WHEN processID='7' THEN 'TBMPCR' END AS processName , recipeCode,cycleTime1,cycleTime2,updateDate from RecipeMasterCycleTime order by iD desc";
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

            }
            ViewState["dt"] = dt;
        }




        protected void Button_Click(object sender, EventArgs e)
        {

            if (((Button)sender).ID == "bdSaveButton")
            {

                //if (Session["userID"].ToString() == "tbmuser") //tbmuser
               //// {
                    save();
              //  }
               // else
               // {
                //    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You are not Authenticate User');", true);
                    //int temp = Convert.ToInt32(bdBarcodeTextBox.Text);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please provide number only');", true);
                    //MessageBox.Show("Please provide number only");
                    //Response.Write("alert('You are not authenticate User');");
              //  }
            }
            else if (((Button)sender).ID == "bdCancelButton")
            {
                clearPage();
            }
        }

        private int save()
        {
            int flag = 0;
            int notifyIcon = 0;

            try
            {
                if (processDropDownList.SelectedItem.Text == "TBMTBR")
                {
                    if ((myWebService.IsRecordExist("RecipeMasterCycleTime", "recipeCode", "where( recipeCode='" + bdrecipeCodeDropDownList.SelectedItem.Text.Trim() + "')", out notifyIcon) == false))
                    {

                                int processID = 4;
                                myConnection.open(ConnectionOption.SQL);
                                myConnection.comm = myConnection.conn.CreateCommand();
                                myConnection.comm.CommandText = "Insert into RecipeMasterCycleTime (processID,recipeID,recipeCode,cycleTime1,cycleTime2,insertDate,updateDate) values (@processID,@recipeID, @recipeCode,@cycleTime1,@cycleTime2,@dtandtime,@dtandtime1)";
                                myConnection.comm.Parameters.AddWithValue("@processID", processID);
                                myConnection.comm.Parameters.AddWithValue("@recipeID", 1);
                                myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@cycleTime1", bdBarcodeTextBox.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@cycleTime2", bdBarcodeTextBox1.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                                myConnection.comm.Parameters.AddWithValue("@dtandtime1", DateTime.Now);
                                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                flag = myConnection.comm.ExecuteNonQuery();

                                myConnection.comm.Dispose();
                                myConnection.close(ConnectionOption.SQL);
                                Notify(1, "Record saved successfully");
                                fillGridviewProcessWise("4");

                    }
                    else
                    {
                        int processID = 4;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "update RecipeMasterCycleTime set cycleTime1=@cycleTime,updateDate=@dtandtime where recipeCode=@recipeCode";
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@cycleTime", bdBarcodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record updated successfully");
                        fillGridviewProcessWise("4");
                        //Notify(notifyIcon, "Record already exists for " + bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                    }
                }
                else if (processDropDownList.SelectedItem.Text == "TBMPCR")
                {
                    if ((myWebService.IsRecordExist("RecipeMasterCycleTime", "recipeCode", "where( recipeCode='" + bdrecipeCodeDropDownList.SelectedItem.Text.Trim() + "')", out notifyIcon) == false))
                    {

                        int processID = 7;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Insert into RecipeMasterCycleTime (processID,recipeID,recipeCode,cycleTime1,cycleTime2,insertDate,updateDate) values (@processID,@recipeID, @recipeCode,@cycleTime1,@cycleTime2,@dtandtime,@dtandtime1)";
                        myConnection.comm.Parameters.AddWithValue("@processID", processID);
                        myConnection.comm.Parameters.AddWithValue("@recipeID", 1);
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@cycleTime1", bdBarcodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@cycleTime2", bdBarcodeTextBox1.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myConnection.comm.Parameters.AddWithValue("@dtandtime1", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        flag = myConnection.comm.ExecuteNonQuery();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record saved successfully");
                        fillGridviewProcessWise("7");
                    }
                    else
                    {
                        int processID = 7;
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "update RecipeMasterCycleTime set cycleTime1=@cycleTime1,cycleTime2=@cycleTime2,updateDate=@dtandtime where recipeCode=@recipeCode";
                        myConnection.comm.Parameters.AddWithValue("@recipeCode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@cycleTime1", bdBarcodeTextBox.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@cycleTime2", bdBarcodeTextBox1.Text.Trim());
                        myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                        Notify(1, "Record updated successfully");
                        fillGridviewProcessWise("7");
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
           

        }
        
        protected void processDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            fillRecipeCodes();
            
            bdBarcodeTextBox.Text = "0";
           

        }

    }
}
