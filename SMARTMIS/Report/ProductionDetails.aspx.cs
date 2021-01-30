using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace SmartMIS.Report
{
    public partial class ProductionDetails : System.Web.UI.Page
    {

        #region Global Variables

        int flag = 0;
        string[] xvalues;
        string queryString = null;
        string durationQuery = "";
        string getProcessNew = "";
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        DataTable reportdt = new DataTable();

        string processType, day, month, year, wcIDInQuery = "";
        string duration, getType, getOperator, getRecipe;
        DateTime fromDate, toDate;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            ErrorMsg.Visible = false;
            HeaderText.Text = "";
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Create an event handler for the master page's contentCallEvent event
            Master.contentCallEvent += new EventHandler(Master1_ButtonClick);

        }
        private void Master1_ButtonClick(object sender, EventArgs e)
        {
            // Get all the inputs from the user
            string getProcess = Master.reportMasterWCProcessDropDownListValue;
            getProcessNew = Master.reportMasterWCProcessDropDownListValue;
            getType = Master.DropDownListTypeValue;
            duration = Master.DropDownListDurationValue;

            getOperator = Master.DropDownListOperatorsValue;
            getRecipe = Master.DropDownListRecipeValue;
            string getDate = Master.reportMasterFromDateTextBoxValue;
            string getMonth = Master.DropDownListMonthValue;
            string getYear = Master.DropDownListYearValue;
            string getYearwise = Master.DropDownListYearWiseValue;

            ArrayList wcIDList = Master.reportMasterWCGridViewValue;

            //Create Query with wcID for IN Clause
            wcIDInQuery = "(";
            foreach (string wcID in wcIDList)
            {
                wcIDInQuery += "'" + wcID.ToString() + "',";
            }
            wcIDInQuery = wcIDInQuery.TrimEnd(',');
            wcIDInQuery += ")";
            //End

            // Set Table to take data from i.e. TBR/PCR
            processType = (getProcess.ToString() == "Tyre Building PCR") ? "processID = 7" : (getProcess == "Tyre Building TBR" ? "processID = 4" : "");

            // Check if WorkCenters has been selected!!
            if (wcIDList.Count > 0)
            {
                // Redirect the flow according to the duration selected by the user
                switch (duration)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getDate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = fromDate.ToString("MM-dd-yyyy");
                        string ntoDate = toDate.ToString("MM-dd-yyyy");

                        showReportDateWise(nfromDate, ntoDate, getType);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>Curing Production Report</strong></td><td width=12% align=right>Date : " + getDate + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
                        break;
                   
                    case "Select":
                        ErrorMsg.Visible = true;
                        ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>Select the duration!!!</font></strong></td></tr></table>";
                        ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);
                        break;
                    default:
                        ErrorMsg.Visible = true;
                        ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>Not the valid selection!!!</font></strong></td></tr></table>";
                        ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);

                        break;
                }
            }
            else
            {
                ErrorMsg.Visible = true;
                ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>Please select the WorkCenters!!!</font></strong></td></tr></table>";
                ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);

            }
        }

        protected void showReportDateWise(string fromDate, string toDate, string type)
        {
            // Validate the user input with proper message
            if (validateInput("date", type, fromDate, toDate, 0, 0, 0))
            {
                switch (type)
                {
                    case "WcWise":
                        showReportDayWcWise(fromDate, toDate);
                        break;
                }
            }
        }


        public bool validateInput(string duration, string type, string fromDate, string toDate, int month, int year, int yearwise)
        {
            try
            {
                durationQuery = "";
                // Create query for particular duration
                switch (duration)
                {
                    case "date":
                        durationQuery += "(dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
                        break;
                }

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select top 1 * from LogsHistoryData where wcID IN " + wcIDInQuery + " AND " + durationQuery+"";
                myConnection.comm.CommandTimeout = 0;
                myConnection.reader = myConnection.comm.ExecuteReader();
                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                bool flag = myConnection.reader.HasRows;
                //flag = true;
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!flag)
                {
                    ErrorMsg.Visible = true;
                    ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>No data available for this " + duration + "!!!</font></strong></td></tr></table>";
                    ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);
                    return false;
                }

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
        }


        #region DayWise Report
        protected void showReportDayWcWise(string fromDate, string toDate)
        {
            try
            {
                if (getProcessNew.ToString() == "Tyre Building PCR")
                {
                    #region showReportDayWcWise
                    DataTable wcdt = new DataTable();
                    DataTable gridviewdt = new DataTable();
                    gridviewdt.Columns.Add("WcName", typeof(string));
                    gridviewdt.Columns.Add("Recipe Code", typeof(string));
                    gridviewdt.Columns.Add("GTBarcode", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit", typeof(string));
                    gridviewdt.Columns.Add("Ready To Scan", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit High Date", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit High Time", typeof(string));


                    // Get the Data based on WCName
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select WC.name AS WcName, recipeCode as [Recipe Code],gtbarcode As GTBarcode, ackBit AS [Acknowledge Bit] ,readyToScan As [Ready To Scan], convert(char(10),dtAndTime,105) as [Acknowledge Bit High Date],CONVERT(VARCHAR(8) ,dtAndTime,108) as [Acknowledge Bit High Time] from [LogsHistoryData]  LHD inner join  wcmaster WC on LHD.wcID = WC.iD WHERE LHD.wcID IN " + (wcIDInQuery) + " AND " + durationQuery + "";
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    MainGridView.Columns.Clear();
                    //Iterate through the columns of the datatable to set the data bound field dynamically.
                    foreach (DataColumn col in gridviewdt.Columns)
                    {
                        //Declare the bound field and allocate memory for the bound field.
                        BoundField bfield = new BoundField();

                        //Initalize the DataField value.
                        bfield.DataField = col.ColumnName;
                        //Initialize the HeaderText field value.
                        bfield.HeaderText = col.ColumnName;
                        //Add the newly created bound field to the GridView.
                        MainGridView.Columns.Add(bfield);

                    }

                    MainGridView.DataSource = wcdt;
                    MainGridView.DataBind();
                    #endregion
                
                }
                else
                {
                    #region showReportDayWcWise
                    DataTable wcdt = new DataTable();
                    DataTable gridviewdt = new DataTable();
                    gridviewdt.Columns.Add("WcName", typeof(string));
                    gridviewdt.Columns.Add("Recipe Code", typeof(string));
                    //gridviewdt.Columns.Add("GTBarcode", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit", typeof(string));
                    gridviewdt.Columns.Add("Ready To Scan", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit High Date", typeof(string));
                    gridviewdt.Columns.Add("Acknowledge Bit High Time", typeof(string));


                    // Get the Data based on WCName
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select WC.name AS WcName, recipeCode as [Recipe Code], ackBit AS [Acknowledge Bit] ,readyToScan As [Ready To Scan], convert(char(10),dtAndTime,105) as [Acknowledge Bit High Date],CONVERT(VARCHAR(8) ,dtAndTime,108) as [Acknowledge Bit High Time] from [LogsHistoryData]  LHD inner join  wcmaster WC on LHD.wcID = WC.iD WHERE LHD.wcID IN " + (wcIDInQuery) + " AND " + durationQuery + "";
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    MainGridView.Columns.Clear();
                    //Iterate through the columns of the datatable to set the data bound field dynamically.
                    foreach (DataColumn col in gridviewdt.Columns)
                    {
                        //Declare the bound field and allocate memory for the bound field.
                        BoundField bfield = new BoundField();

                        //Initalize the DataField value.
                        bfield.DataField = col.ColumnName;
                        //Initialize the HeaderText field value.
                        bfield.HeaderText = col.ColumnName;
                        //Add the newly created bound field to the GridView.
                        MainGridView.Columns.Add(bfield);

                    }

                    MainGridView.DataSource = wcdt;
                    MainGridView.DataBind();
                    #endregion
                }

                

                // Select the rows where showing total & make them bold
               // IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
   // .Where(row => row.Cells[0].Text == "Total" || row.Cells[0].Text == "GrandTotal");

              //  foreach (var row in rows)
                   // row.Font.Bold = true;

               // gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        #endregion

        #region User Defined Function

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = MainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
                        for (int cellCount = 0; cellCount < 1 /*gvRow.Cells.Count*/; cellCount++)
                        {
                            if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                            {
                                if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                                {
                                    gvRow.Cells[cellCount].RowSpan = 2;
                                }
                                else
                                {
                                    gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                                }
                                gvPreviousRow.Cells[cellCount].Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void OnDataBound(object sender, EventArgs e)
        {
            switch (duration)
            {
                case "Date":
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell cell = new TableHeaderCell();

                    switch (getType)
                    {
                        case "WcWise":
                            //cell.Text = "";
                            //cell.ColumnSpan = 3;
                            //row.Controls.Add(cell);

                            //cell = new TableHeaderCell();
                            //cell.Text = "A";
                            //cell.ColumnSpan = 9;
                            //row.Controls.Add(cell);

                            //cell = new TableHeaderCell();
                            //cell.ColumnSpan = 9;
                            //cell.Text = "B";
                            //row.Controls.Add(cell);
                            //cell = new TableHeaderCell();
                            //cell.ColumnSpan = 9;
                            //cell.Text = "C";
                            //row.Controls.Add(cell);
                            //cell = new TableHeaderCell();
                            //cell.ColumnSpan = 1;
                            //cell.Text = "";
                            //row.Controls.Add(cell);
                            break;
                        
                    }
                    MainGridView.HeaderRow.Parent.Controls.AddAt(0, row);

                    break;
            }

        }

        public string formatDate(string date)
        {
            string flag = "";
            string[] tempDate = date.Split(new char[] { '-' });
            month = tempDate[1].ToString();
            day = tempDate[0].ToString();
            year = tempDate[2].ToString();
            flag = month + "-" + day + "-" + year;
            return flag;
        }

        #endregion

    }
}

