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

namespace SmartMIS
{
    public partial class newproductionReport : System.Web.UI.Page
    {
        #region Global Variables
    
        int flag = 0;
        string[] xvalues;
        string queryString = null;
        string durationQuery = "";
        
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        DataTable reportdt = new DataTable();

        string OldSQLTable,SQLTable, day, month, year, wcIDInQuery = "(";
        string duration, getType, getOperator;
        DateTime fromDate, toDate; 
    
        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            reportHeader.Visible = false;

            ErrorMsg.Visible = false;
            HeaderText.Text = "";
            GridViewoperator.Visible = false;
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Create an event handler for the master page's contentCallEvent event
            Master.contentCallEvent += new EventHandler(Master_ButtonClick);
        }      
        private void Master_ButtonClick(object sender, EventArgs e)
        {
            // Get all the inputs from the user
            string getProcess = ((smartMISTBMproductionReport)this.Master).reportMasterWCProcessDropDownListValue;
            
            getType = ((smartMISTBMproductionReport)this.Master).DropDownListTypeValue;
            duration = ((smartMISTBMproductionReport)this.Master).DropDownListDurationValue;

            getOperator = ((smartMISTBMproductionReport)this.Master).DropDownListOperatorsValue;
            
            string getDate = ((smartMISTBMproductionReport)this.Master).reportMasterFromDateTextBoxValue;
            string getMonth = ((smartMISTBMproductionReport)this.Master).DropDownListMonthValue;
            string getYear = ((smartMISTBMproductionReport)this.Master).DropDownListYearValue;
            string getYearwise = ((smartMISTBMproductionReport)this.Master).DropDownListYearWiseValue;

            ArrayList wcIDList = ((smartMISTBMproductionReport)this.Master).reportMasterWCGridViewValue;
                        
            //Create Query with wcID for IN Clause
            wcIDInQuery = "(";
            foreach (string wcID in wcIDList)
            {
                wcIDInQuery += "'" + wcID.ToString() + "',";
            }
            wcIDInQuery = wcIDInQuery.TrimEnd(',');
            wcIDInQuery += ")";
            //End//

            // Set Table to take data from i.e. TBR/PCR
            SQLTable = (getProcess.ToString() == "Tyre Building PCR") ? "vTbmPCR" : (getProcess == "Tyre Building TBR" ? "vTbmTBR" : "");
            OldSQLTable = (getProcess.ToString() == "Tyre Building PCR") ? "vTbmPCR16Jan2021" : (getProcess == "Tyre Building TBR" ? "vTbmTBR" : "");
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
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>TBM Production Report</strong></td><td width=12% align=right>Date : " + getDate + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
                        break;
                    case "Month":
                        showReportMonthWise(Convert.ToInt32(getMonth), Convert.ToInt32(getYear), getType);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=60%><strong>TBM Production Report</strong></td><td width=14% align=right>Month : " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(getMonth)) + " " + getYear + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
                        
                        break;
                    case "Year":
                        showReportYearWise(Convert.ToInt32(getYearwise), getType);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>TBM Production Report</strong></td><td width=12% align=right>Year : " + getYearwise + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
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

                    case "RecipeWise":
                        showReportDayRecipeWise(fromDate, toDate);
                        break;

                    case "OperatorWise":
                        showReportDayOperatorWise(fromDate, toDate);
                        break;
                }                
            }
        }
        protected void showReportMonthWise(int getMonth, int getYear, string type)
        {
            if (validateInput("month", type, "", "", getMonth, getYear, 0))
            { 
               switch (type)
                {
                    case "WcWise":
                        showReportMonthWcWise(getMonth, getYear);
                        break;

                    case "RecipeWise":
                        showReportMonthRecipeWise(getMonth, getYear);
                        break;

                    case "OperatorWise":
                        showReportMonthOperatorWise(getMonth, getYear);
                        break;
                }        
            }
        }
        protected void showReportYearWise(int getYearwise, string type)
        {
            if(validateInput("year", type, "", "", 0, 0, getYearwise))
            {
                switch (type)
                {
                    case "WcWise":
                        showReportYearWcWise(getYearwise);
                        break;

                    case "RecipeWise":
                        showReportYearRecipeWise(getYearwise);
                        break;

                    case "OperatorWise":
                        showReportYearOperatorWise(getYearwise);
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
                        durationQuery += "(dtandTime > '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
                        break;
                    case "month":
                         string rtoDate="";
                        string rfromDate = year.ToString() + "-" + month + "-01 07:00:00";
                       if(month==12)
                       rtoDate = (year+1).ToString() + "-01-01 07:00:00";
                        else
                       rtoDate = year.ToString() + "-" + (month + 1) + "-01 07:00:00";

                       durationQuery += "(dtandTime > '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
                        break;
                    case "year":
                        string nfromDate = yearwise.ToString() + "-01-01 07:00:00";
                        string ntoDate = (yearwise + 1).ToString() + "-01-01 07:00:00";
                        durationQuery += "(dtandTime > '" + nfromDate + "' AND dtandTime < '" + ntoDate + "')";
                        break;
                }
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "if exists(select * from " + SQLTable + " where WCID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + ") select 1 as DataExists else select 0 as DataExists"; 
                myConnection.comm.CommandTimeout = 0;
                myConnection.reader = myConnection.comm.ExecuteReader();
                myConnection.reader.Read();
                bool flag = Convert.ToBoolean(myConnection.reader[0]);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);


               
                if(!flag)
                {
                    if (SQLTable == "vTbmPCR")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "if exists(select * from " + OldSQLTable + " where WCID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + ") select 1 as DataExists else select 0 as DataExists";
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myConnection.reader.Read();
                        flag = Convert.ToBoolean(myConnection.reader[0]);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                    if (!flag)
                    {
                        ErrorMsg.Visible = true;
                        ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>No data available for this " + duration + "!!!</font></strong></td></tr></table>";
                        ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);
                        return false;
                    }  
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
                DataTable wcdt = new DataTable();
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                for (int i = 7; i <= 23; i++)
                {
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                    if(i == 14)
                        gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
                    else if (i == 22)
                        gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
                }
                for (int i = 0; i <= 6; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                    gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
                    gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));
                
                // Get the Data based on WCName
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                  myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcID,wcName, recipeCode,SAPMaterialCode, dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode) rn
                    FROM " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcID, recipeCode, dtandTime";
               
                myConnection.reader = myConnection.comm.ExecuteReader();
                wcdt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);


                if (!(wcdt.Rows.Count > 0))
                {
                    // Get the Data based on WCName
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcID,wcName, recipeCode,SAPMaterialCode, dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode) rn
                    FROM " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcID, recipeCode, dtandTime";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                
                }

                wcdt.Columns.Remove("wcID");
                wcdt.Columns.Remove("rn");


                var query = wcdt.AsEnumerable()
    .GroupBy(row => new
    {
        Hour = row.Field<DateTime>("dtandTime").Hour,
        recipeCode = row.Field<string>("recipeCode"),
        sapcode = row.Field<string>("SAPMaterialCode"),
        wcName = row.Field<string>("wcName")
    })
    .Select(g => new
    {
        WcName=g.Key.wcName,
        Hour = g.Key.Hour,
        recipeCode = g.Key.recipeCode,
        sapcode = g.Key.sapcode,
        quantity = g.Count()
    }
        );
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {   
                    if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                    {
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        dr[2] = items[i].sapcode.ToString();


                        // Insert rows in all the hours places by default
                        for (int h = 3; h <= 29; h++)
                        {
                            dr[h] = 0;
                        }
                    }
                    else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i - 1].WcName.ToString())) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        dr[2] = items[i].sapcode.ToString();
                        // Insert rows in all the hours places by default
                        for (int h = 3; h <= 29;h++ )
                        {
                            dr[h] = 0;
                        }
                    }
                    int getHour = items[i].Hour; //Store current array hour
                    int getDifference = 0; //Store time difference of current to previous day
                    if (i > 0)
                    {
                        if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
                            getDifference = items[i].Hour - items[i - 1].Hour;
                        else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
                            getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
                    }
                    
                    dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Shift Total
                        for (int v = 3; v <= 29; v++)
                        {
                            if (v >= 3 && v <= 10) //Calculate A shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 11 && v < 20) //Calculate B shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 20 && v < 29) //Calculate C shift values
                                total += Convert.ToInt32(dr[v]);

                            if (v == 11) //if all A shift values processed, then add total of A shift
                            {
                                dr[11] = total;
                                total = 0;
                            }
                            else if (v == 20) //if all B shift values processed, then add total of A shift
                            {
                                dr[20] = total;
                                total = 0;
                            }
                            else if (v == 29) //if all C shift values processed, then add total of A shift
                            {
                                dr[29] = total;
                                total = 0;

                                //Set Day Total
                                dr[30] = Convert.ToInt32(dr[11]) + Convert.ToInt32(dr[20]) + Convert.ToInt32(dr[29]);
                            }

                        }
                        gridviewdt.Rows.Add(dr);
                    }
                    else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Shift Total
                        for (int v = 3; v <= 29; v++)
                        {
                            if (v >= 3 && v <= 10) //Calculate A shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 11 && v < 20) //Calculate B shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 20 && v < 29) //Calculate C shift values
                                total += Convert.ToInt32(dr[v]);

                            if (v == 11) //if all A shift values processed, then add total of A shift
                            {
                                dr[11] = total;
                                total = 0;
                            }
                            else if (v == 20) //if all B shift values processed, then add total of A shift
                            {
                                dr[20] = total;
                                total = 0;
                            }
                            else if (v == 28) //if all C shift values processed, then add total of A shift
                            {
                                dr[29] = total;
                                total = 0;

                                //Set Day Total
                                dr[30] = Convert.ToInt32(dr[11]) + Convert.ToInt32(dr[20]) + Convert.ToInt32(dr[29]);
                            }

                        }
                        gridviewdt.Rows.Add(dr);
                    }

                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= 30; v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    else if (items[i].WcName.ToString() != items[i + 1].WcName.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= 30; v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                }

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

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                dr[1] = "";
                dr[2] = "";
                for (int v = 3; v <= 30; v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);
                
                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // Select the rows where showing total & make them bold
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 2).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("hours", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                {
                    if (gridviewdt.Columns[3 + i].ColumnName != "TotalA" && gridviewdt.Columns[3 + i].ColumnName != "TotalB" && gridviewdt.Columns[3 + i].ColumnName != "TotalC" && gridviewdt.Columns[3 + i].ColumnName != "DayTotal" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][3 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "hours";
                TBMChart.DataBind();
                
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportDayRecipeWise(string fromDate, string toDate)
        {
            try
            {
                DataTable wcdt = new DataTable();
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                for (int i = 7; i <= 23; i++)
                {
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                    if (i == 14)
                        gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
                    else if (i == 22)
                        gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
                }
                for (int i = 0; i <= 6; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
                gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));

                // Get the Data based on WCName
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                 myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName DESC) rn
                    FROM " + SQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY recipeCode, dtandTime";
                
                myConnection.reader = myConnection.comm.ExecuteReader();
                wcdt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(wcdt.Rows.Count > 0))
                {
                    // Get the Data based on WCName
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName DESC) rn
                    FROM " + OldSQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY recipeCode, dtandTime";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                
                }
               
                                
                var query = wcdt.AsEnumerable()
    .GroupBy(row => new
    {
        Hour = row.Field<DateTime>("dtandTime").Hour,
        recipeCode = row.Field<string>("recipeCode").Trim(),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode")
    })
    .Select(g => new
    {
        Hour = g.Key.Hour,
        recipeCode = g.Key.recipeCode,
        SAPMaterialCode = g.Key.SAPMaterialCode,
        quantity = g.Count()
    }
        );
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {
                    if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                    {
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the hours places by default
                        for (int h = 2; h <= 29; h++)
                        {
                            dr[h] = 0;
                        }
                    }
                    else if (items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();
                        // Insert rows in all the hours places by default
                        for (int h = 2; h <= 29; h++)
                        {
                            dr[h] = 0;
                        }
                    }
                    int getHour = items[i].Hour; //Store current array hour
                    int getDifference = 0; //Store time difference of current to previous day
                    if (i > 0)
                    {
                        if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
                            getDifference = items[i].Hour - items[i - 1].Hour;
                        else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
                            getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
                    }

                    dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Shift Total
                        for (int v = 2; v <= 29; v++)
                        {
                            if (v >= 2 && v <= 9) //Calculate A shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 10 && v < 19) //Calculate B shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 19 && v < 28) //Calculate C shift values
                                total += Convert.ToInt32(dr[v]);

                            if (v == 10) //if all A shift values processed, then add total of A shift
                            {
                                dr[10] = total;
                                total = 0;
                            }
                            else if (v == 19) //if all B shift values processed, then add total of A shift
                            {
                                dr[19] = total;
                                total = 0;
                            }
                            else if (v == 28) //if all C shift values processed, then add total of A shift
                            {
                                dr[28] = total;
                                total = 0;

                                //Set Day Total
                                dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
                            }

                        }
                        gridviewdt.Rows.Add(dr);
                    }
                    else if (items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Shift Total
                        for (int v = 2; v <= 28; v++)
                        {
                            if (v >= 2 && v <= 9) //Calculate A shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 10 && v < 19) //Calculate B shift values
                                total += Convert.ToInt32(dr[v]);
                            else if (v > 19 && v < 28) //Calculate C shift values
                                total += Convert.ToInt32(dr[v]);

                            if (v == 10) //if all A shift values processed, then add total of A shift
                            {
                                dr[10] = total;
                                total = 0;
                            }
                            else if (v == 19) //if all B shift values processed, then add total of A shift
                            {
                                dr[19] = total;
                                total = 0;
                            }
                            else if (v == 28) //if all C shift values processed, then add total of A shift
                            {
                                dr[28] = total;
                                total = 0;

                                //Set Day Total
                                dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
                            }

                        }
                        gridviewdt.Rows.Add(dr);
                    }                    
                }

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

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                for (int v = 2; v <= 29; v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 2).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("hours", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                {
                    if (gridviewdt.Columns[2 + i].ColumnName != "TotalA" && gridviewdt.Columns[2 + i].ColumnName != "TotalB" && gridviewdt.Columns[2 + i].ColumnName != "TotalC" && gridviewdt.Columns[2 + i].ColumnName != "DayTotal" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][2 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column; //SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "hours";
                TBMChart.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportDayOperatorWise(string fromDate, string toDate)
        {
            try
            {
                string sqlquery = "";
                if (getOperator != "All")
                    sqlquery = " AND (manningID=" + getOperator + " OR manningID2=" + getOperator + " OR manningID3=" + getOperator + ")";
                DataTable wcdt = new DataTable();
                DataTable manningdt = new DataTable();
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                if (getOperator != "All")
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator1", typeof(string));
                    dt.Columns.Add("dtandTime", typeof(DateTime));

                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));

                }
                else
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator1", typeof(string));
                    dt.Columns.Add("operator2", typeof(string));
                    dt.Columns.Add("operator3", typeof(string));
                    dt.Columns.Add("dtandTime", typeof(DateTime));

                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));
                    gridviewdt.Columns.Add("operator2", typeof(string));
                    gridviewdt.Columns.Add("operator3", typeof(string));
                }
                for (int i = 7; i <= 23; i++)
                {
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                    if (i == 14)
                        gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
                    else if (i == 22)
                        gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
                }
                for (int i = 0; i <= 6; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
                gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));
                
                // Get the Data based on WCName
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (getOperator != "All")
                {

                   // myConnection.comm.CommandText = "select " + SQLTable + ".wcName,dtandtime,mm.sapCode from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "'  ORDER BY wcName ,dtandTime asc";
                    myConnection.comm.CommandText = @"SELECT * FROM ( select " + SQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                }
                else
                {
                    //myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";
                    myConnection.comm.CommandText = @" SELECT * FROM (  Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName,
                    ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc ";
                }
                myConnection.reader = myConnection.comm.ExecuteReader();
                wcdt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(wcdt.Rows.Count > 0))
                {
                    // Get the Data based on WCName
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (getOperator != "All")
                    {
                        // myConnection.comm.CommandText = "select " + SQLTable + ".wcName,dtandtime,mm.sapCode from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "'  ORDER BY wcName ,dtandTime asc";
                        myConnection.comm.CommandText = @"SELECT * FROM ( select " + OldSQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + OldSQLTable + "  inner join manningmaster mm on " + OldSQLTable + ".manningID2 = mm.iD  or " + OldSQLTable + ".manningID3 = mm.iD or " + OldSQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                    }
                    else
                    {
                        //myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";
                        myConnection.comm.CommandText = @" SELECT * FROM (  Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName,
                        ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc ";
                    }
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                
                
                }



                // Get the Data based on ManningID
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select iD, sapcode from manningMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                manningdt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                if (getOperator != "All")
                {
                    

                var query = wcdt.AsEnumerable()
            .GroupBy(row => new
            {
                wcName = row.Field<string>("wcName"),
                Hour = row.Field<DateTime>("dtandTime").Hour,
                manningID1 = row.Field<string>("sapcode"),

            })
            .Select(g => new
            {
                Hour = g.Key.Hour,
                wcName = g.Key.wcName,
                manningID1 = g.Key.manningID1,

                quantity = g.Count()
            }
                );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= 28; h++)
                            {
                                dr[h] = 0;
                            }
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].wcName.ToString() != items[i].wcName.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= 28; h++)
                            {
                                dr[h] = 0;
                            }
                        }
                        int getHour = items[i].Hour; //Store current array hour
                        int getDifference = 0; //Store time difference of current to previous day
                        if (i > 0)
                        {
                            if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
                                getDifference = items[i].Hour - items[i - 1].Hour;
                            else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
                                getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
                        }

                        dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Shift Total
                            for (int v = 2; v <= 28; v++)
                            {
                                if (v >= 2 && v <= 9) //Calculate A shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 10 && v < 19) //Calculate B shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 19 && v < 28) //Calculate C shift values
                                    total += Convert.ToInt32(dr[v]);

                                if (v == 10) //if all A shift values processed, then add total of A shift
                                {
                                    dr[10] = total;
                                    total = 0;
                                }
                                else if (v == 19) //if all B shift values processed, then add total of A shift
                                {
                                    dr[19] = total;
                                    total = 0;
                                }
                                else if (v == 28) //if all C shift values processed, then add total of A shift
                                {
                                    dr[28] = total;
                                    total = 0;

                                    //Set Day Total
                                    dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
                                }

                            }
                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].wcName.ToString() != items[i + 1].wcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                        {
                            total = 0;
                            // Display Shift Total
                            for (int v = 2; v <= 28; v++)
                            {
                                if (v >= 2 && v <= 9) //Calculate A shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 10 && v < 19) //Calculate B shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 19 && v < 28) //Calculate C shift values
                                    total += Convert.ToInt32(dr[v]);

                                if (v == 10) //if all A shift values processed, then add total of A shift
                                {
                                    dr[10] = total;
                                    total = 0;
                                }
                                else if (v == 19) //if all B shift values processed, then add total of A shift
                                {
                                    dr[19] = total;
                                    total = 0;
                                }
                                else if (v == 28) //if all C shift values processed, then add total of A shift
                                {
                                    dr[28] = total;
                                    total = 0;

                                    //Set Day Total
                                    dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
                                }

                            }
                            gridviewdt.Rows.Add(dr);
                        }
                    }

                    GridViewoperator.Columns.Clear();
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
                        GridViewoperator.Columns.Add(bfield);

                    }

                    // Calculate total of all workstations of every hour

                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";
                    for (int v = 2; v <= 29; v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);

                }
                else
                {
                  var rows = wcdt.AsEnumerable().Join(
                    manningdt.AsEnumerable(),
                    wdtRows => (string.IsNullOrEmpty(wdtRows["manningID"].ToString()) ? 0 : wdtRows["manningID"]),
                    mdtRows => mdtRows.Field<string>("sapCode"),
                    (wdtRows, mdtRows1) => new { wdtRows, mdtRows1 }
                    ) //New anon<datarow,datarow> - I will call anons 'n' (for new) below
                    .Join(
                    manningdt.AsEnumerable(),
                    n => (string.IsNullOrEmpty(n.wdtRows["manningID2"].ToString()) ? 0 : n.wdtRows["manningID2"]),
                    mdtRows2 => mdtRows2.Field<string>("sapCode"),
                    (n, mdtRows2) => new { n.wdtRows, n.mdtRows1, mdtRows2 }
                    )//New anon<datarow,datarow,datarow>
                    .Join(
                    manningdt.AsEnumerable(),
                    n => (string.IsNullOrEmpty(n.wdtRows["manningID3"].ToString()) ? 0 : n.wdtRows["manningID3"]),
                    mdtRows3 => mdtRows3.Field<string>("sapCode"),
                    (n, mdtRows3) => new { n.wdtRows, n.mdtRows1, n.mdtRows2, mdtRows3 }
                    ) //New anon<datarow,datarow,datarow,datarow> - all joined up now
                    .ToList();
                rows.ForEach(r =>
                {
                    var datarow = dt.NewRow();
                    datarow[0] = r.wdtRows[4].ToString();
                    //datarow[1] = r.mdtRows1.Field<string>("firstName") + " " + r.mdtRows1.Field<string>("lastName");
                    datarow[1] = r.mdtRows1.Field<string>("sapCode") ;
                    datarow[2] = r.mdtRows2.Field<string>("sapCode") ;
                    datarow[3] = r.mdtRows3.Field<string>("sapCode") ;
                    datarow[4] = r.wdtRows[3].ToString();

                    //If you can find a way to add a range (or turn a List<datarow> into a table)
                    //this you can change this section to a select and return the datarow instead
                    dt.Rows.Add(datarow);
               
                    });
                    var query = dt.AsEnumerable()
            .GroupBy(row => new
            {
                wcName = row.Field<string>("MachineName"),
                Hour = row.Field<DateTime>("dtandTime").Hour,
                manningID1 = row.Field<string>("operator1"),
                manningID2 = row.Field<string>("operator2"),
                manningID3 = row.Field<string>("operator3")
            })
            .Select(g => new
            {
                wcName = g.Key.wcName,
                Hour = g.Key.Hour,
                manningID1 = g.Key.manningID1,
                manningID2 = g.Key.manningID2,
                manningID3 = g.Key.manningID3,
                quantity = g.Count()
            }
                );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= 30; h++)
                            {
                                dr[h] = 0;
                            }
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].manningID2.ToString() != items[i].manningID2.ToString())
                            || (items[i - 1].manningID3.ToString() != items[i].manningID3.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= 30; h++)
                            {
                                dr[h] = 0;
                            }
                        }
                        int getHour = items[i].Hour; //Store current array hour
                        int getDifference = 0; //Store time difference of current to previous day
                        if (i > 0)
                        {
                            if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
                                getDifference = items[i].Hour - items[i - 1].Hour;
                            else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
                                getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
                        }

                        dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Shift Total
                            for (int v = 4; v <= 30; v++)
                            {
                                if (v >= 4 && v <= 11) //Calculate A shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 12 && v < 21) //Calculate B shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 21 && v < 30) //Calculate C shift values
                                    total += Convert.ToInt32(dr[v]);

                                if (v == 12) //if all A shift values processed, then add total of A shift
                                {
                                    dr[12] = total;
                                    total = 0;
                                }
                                else if (v == 21) //if all B shift values processed, then add total of A shift
                                {
                                    dr[21] = total;
                                    total = 0;
                                }
                                else if (v == 30) //if all C shift values processed, then add total of A shift
                                {
                                    dr[30] = total;
                                    total = 0;

                                    //Set Day Total
                                    dr[31] = Convert.ToInt32(dr[12]) + Convert.ToInt32(dr[21]) + Convert.ToInt32(dr[30]);
                                }

                            }
                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].manningID2.ToString() != items[i + 1].manningID2.ToString()) || (items[i].manningID3.ToString() != items[i + 1].manningID3.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                        {
                            total = 0;
                            // Display Shift Total
                            for (int v = 4; v <= 30; v++)
                            {
                                if (v >= 4 && v <= 11) //Calculate A shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 12 && v < 21) //Calculate B shift values
                                    total += Convert.ToInt32(dr[v]);
                                else if (v > 21 && v < 30) //Calculate C shift values
                                    total += Convert.ToInt32(dr[v]);

                                if (v == 12) //if all A shift values processed, then add total of A shift
                                {
                                    dr[12] = total;
                                    total = 0;
                                }
                                else if (v == 21) //if all B shift values processed, then add total of A shift
                                {
                                    dr[21] = total;
                                    total = 0;
                                }
                                else if (v == 30) //if all C shift values processed, then add total of A shift
                                {
                                    dr[30] = total;
                                    total = 0;

                                    //Set Day Total
                                    dr[31] = Convert.ToInt32(dr[12]) + Convert.ToInt32(dr[21]) + Convert.ToInt32(dr[30]);
                                }

                            }
                            gridviewdt.Rows.Add(dr);
                        }
                    }

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

                    // Calculate total of all workstations of every hour

                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";
                    for (int v = 4; v <= 31; v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);

                    dr = gridviewdt.NewRow();
                    gridviewdt.Rows.Add(dr);
                }
                if (getOperator != "All")
                {
                    GridViewoperator.DataSource = gridviewdt;
                    GridViewoperator.DataBind();
                    GridViewoperator.Visible = true;
                    MainGridView.Visible = false;
                }
                else
                {
                    MainGridView.DataSource = gridviewdt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    GridViewoperator.Visible = false;
                }
                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 2).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("hours", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                {
                    if (gridviewdt.Columns[2 + i].ColumnName != "TotalA" && gridviewdt.Columns[2 + i].ColumnName != "TotalB" && gridviewdt.Columns[2 + i].ColumnName != "TotalC" && gridviewdt.Columns[2 + i].ColumnName != "DayTotal" && gridviewdt.Columns[2 + i].ColumnName != "wcName" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][2 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "hours";
                TBMChart.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        #endregion

        #region MonthWise Report
        protected void showReportMonthWcWise(int getMonth, int getYear)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                int daysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                string fromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                string toDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 07:00:00";

                //Generate datatable columns dynamically
                for (int i = 1; i <= daysinMonth; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

               // myConnection.comm.CommandText = "Select wcName, recipeCode,SAPMaterialCode, dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcID,wcName, recipeCode,SAPMaterialCode, dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode) rn
                    FROM " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcID, recipeCode, dtandTime asc";
               
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);


                if (!(dt.Rows.Count > 0))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    // myConnection.comm.CommandText = "Select wcName, recipeCode,SAPMaterialCode, dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                    myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcID,wcName, recipeCode,SAPMaterialCode, dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode) rn
                    FROM " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcID, recipeCode, dtandTime asc";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
               
                }

                dt.Columns.Remove("wcID");
                dt.Columns.Remove("rn");


                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        day = row.Field<DateTime>("dtandTime").AddHours(-7).Day,
        recipeCode = row.Field<string>("recipeCode"),
        wcName = row.Field<string>("wcName"),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode")
    })
    .Select(g => new
    {
        WcName = g.Key.wcName,
        day = g.Key.day,
        recipeCode = g.Key.recipeCode,
         SAPMaterialCode = g.Key.SAPMaterialCode,
        quantity = g.Count()
    }
        );
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {
                    
                    if(i == 0)
                    {
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 3; h <= (daysinMonth + 3); h++)
                            dr[h] = 0;
                    }
                    else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i - 1].WcName.ToString())) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].SAPMaterialCode.ToString();
                        // Insert rows in all the hours places by default
                        for (int h = 3; h <= (daysinMonth + 3); h++)
                            dr[h] = 0;
                    }

                    dr[items[i].day.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)
                    

                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 3; v <= (daysinMonth + 3); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[daysinMonth + 3] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                    else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 3; v <= (daysinMonth + 3); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[daysinMonth + 3] = total;
                        
                        gridviewdt.Rows.Add(dr);
                    }

                    // Wcwise Total
                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= (daysinMonth + 3); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    else if (items[i].WcName.ToString() != items[i + 1].WcName.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= (daysinMonth + 3); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    //End WCwise Total
                }


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


                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                dr[1] = "";
                dr[2] = "";
                for (int v = 3; v <= (daysinMonth + 3); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // Select the rows where showing total & make them bold
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("days", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
                {
                    if (gridviewdt.Columns[3 + i].ColumnName != "Total" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][3 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "days";
                TBMChart.DataBind();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportMonthRecipeWise(int getMonth, int getYear)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                int daysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                //Generate datatable columns dynamically
                for (int i = 1; i <= daysinMonth; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                string toDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 07:00:00";
                                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                ///myConnection.comm.CommandText = "Select wcName, recipeCode, SAPMaterialCode,dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName asc) rn
                  FROM " + SQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY wcID,recipeCode, dtandTime asc";
                myConnection.comm.CommandTimeout = 60;
                
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(dt.Rows.Count > 0))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    ///myConnection.comm.CommandText = "Select wcName, recipeCode, SAPMaterialCode,dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                    myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName asc) rn
                  FROM " + OldSQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY wcID,recipeCode, dtandTime asc";
                    myConnection.comm.CommandTimeout = 60;

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);        
                
                }



                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        day = row.Field<DateTime>("dtandTime").AddHours(-7).Day,
        recipeCode = row.Field<string>("recipeCode").Trim(),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode")
        
    })
    .Select(g => new
    {
        day = g.Key.day,
        recipeCode = g.Key.recipeCode,
        SAPMaterialCode = g.Key.SAPMaterialCode,
        quantity = g.Count()
      
    }
        ).OrderBy(item => item.recipeCode); 

                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {

                    if (i == 0)
                    {
                        dr[0] = items[i].recipeCode.ToString().Trim();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 2; h <= (daysinMonth + 2); h++)
                            dr[h] = 0;
                    }
                    else if (items[i - 1].recipeCode.ToString().Trim() != items[i].recipeCode.ToString().Trim()) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].recipeCode.ToString().Trim();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();
                        // Insert rows in all the hours places by default
                        for (int h = 2; h <= (daysinMonth + 2); h++)
                            dr[h] = 0;
                    }

                    dr[items[i].day.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 2; v <= (daysinMonth + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[daysinMonth + 2] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                    else if (items[i].recipeCode.ToString().Trim() != items[i + 1].recipeCode.ToString().Trim()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 2; v <= (daysinMonth + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[daysinMonth + 2] = total;

                        gridviewdt.Rows.Add(dr);
                    }                    
                }

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

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                for (int v = 2; v <= (daysinMonth + 2); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("days", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                {
                    if (gridviewdt.Columns[2 + i].ColumnName != "Total" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][2 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "days";
                TBMChart.DataBind();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportMonthOperatorWise(int getMonth, int getYear)
        {
            try
            {
                string sqlquery = "";
                if (getOperator != "All")
                    //sqlquery = " AND (manningID=" + getOperator + ")";
                    sqlquery = " AND (manningID=" + getOperator + " OR manningID2=" + getOperator + " OR manningID3=" + getOperator + ")";

                DataTable wcdt = new DataTable();
                DataTable manningdt = new DataTable();
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                if (getOperator != "All")
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator1", typeof(string));

                    dt.Columns.Add("dtandTime", typeof(DateTime));


                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));
                }

                else
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator1", typeof(string));
                    dt.Columns.Add("operator2", typeof(string));
                    dt.Columns.Add("operator3", typeof(string));
                    dt.Columns.Add("dtandTime", typeof(DateTime));

                   // DataTable gridviewdt = new DataTable();
                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));
                    gridviewdt.Columns.Add("operator2", typeof(string));
                    gridviewdt.Columns.Add("operator3", typeof(string));
                }

                //Generate datatable columns dynamically
                int daysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                for (int i = 1; i <= daysinMonth; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                string toDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 07:00:00";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (getOperator != "All")
                {
                  myConnection.comm.CommandText = @"SELECT * FROM ( select " + SQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                }
                else
                {
                   myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";  
                }
                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        
                myConnection.comm.CommandTimeout = 60;
                myConnection.reader = myConnection.comm.ExecuteReader();
                wcdt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(wcdt.Rows.Count > 0))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (getOperator != "All")
                    {
                        myConnection.comm.CommandText = @"SELECT * FROM ( select " + OldSQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + OldSQLTable + "  inner join manningmaster mm on " + OldSQLTable + ".manningID2 = mm.iD  or " + OldSQLTable + ".manningID3 = mm.iD or " + OldSQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                    }
                    else
                    {
                        myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";
                    }
                    
                    myConnection.comm.CommandTimeout = 60;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                
                }
                // Get the Data based on ManningID
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select iD,sapcode  from manningMaster";
                myConnection.comm.CommandTimeout = 60;
                myConnection.reader = myConnection.comm.ExecuteReader();
                manningdt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (getOperator != "All")
                {
                    var query = wcdt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            wcName = row.Field<string>("wcName"),
                            manningID1 = row.Field<string>("sapCode"),
                            day = row.Field<DateTime>("dtandTime").AddHours(-7).Day
                        })
                        .Select(g => new
                        {
                            wcName = g.Key.wcName,
                            day = g.Key.day,
                            manningID1 = g.Key.manningID1,
                            quantity = g.Count()
                        }
                        );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= (daysinMonth + 1); h++)
                                dr[h] = 0;
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].wcName.ToString() != items[i].wcName.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= (daysinMonth + 1); h++)
                                dr[h] = 0;
                        }

                        dr[items[i].day.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 2; v <= (daysinMonth + 1); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[daysinMonth + 2] = total;

                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].wcName.ToString() != items[i + 1].wcName.ToString()))//
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 2; v <= (daysinMonth + 1); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[daysinMonth + 2] = total;

                            gridviewdt.Rows.Add(dr);
                        }

                    }

                    GridViewoperator.Columns.Clear();
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
                        GridViewoperator.Columns.Add(bfield);

                    }

                    // Calculate total of all workstations of every day
                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";

                    for (int v = 2; v <= (daysinMonth + 2); v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);
                }
                else
                {
                    var rows = wcdt.AsEnumerable().Join(
                     manningdt.AsEnumerable(),
                     wdtRows => (string.IsNullOrEmpty(wdtRows["manningID"].ToString()) ? 0 : wdtRows["manningID"]),
                     mdtRows => mdtRows.Field<string>("sapCode"),
                     (wdtRows, mdtRows1) => new { wdtRows, mdtRows1 }
                     ) //New anon<datarow,datarow> - I will call anons 'n' (for new) below
                     .Join(
                     manningdt.AsEnumerable(),
                     n => (string.IsNullOrEmpty(n.wdtRows["manningID2"].ToString()) ? 0 : n.wdtRows["manningID2"]),
                     mdtRows2 => mdtRows2.Field<string>("sapCode"),
                     (n, mdtRows2) => new { n.wdtRows, n.mdtRows1, mdtRows2 }
                     )//New anon<datarow,datarow,datarow>
                     .Join(
                     manningdt.AsEnumerable(),
                     n => (string.IsNullOrEmpty(n.wdtRows["manningID3"].ToString()) ? 0 : n.wdtRows["manningID3"]),
                     mdtRows3 => mdtRows3.Field<string>("sapCode"),
                     (n, mdtRows3) => new { n.wdtRows, n.mdtRows1, n.mdtRows2, mdtRows3 }
                     ) //New anon<datarow,datarow,datarow,datarow> - all joined up now
                     .ToList();
                    rows.ForEach(r =>
                    {
                        var datarow = dt.NewRow();
                        datarow[0] = r.wdtRows[4].ToString();
                        //datarow[1] = r.mdtRows1.Field<string>("firstName") + " " + r.mdtRows1.Field<string>("lastName");
                        datarow[1] = r.mdtRows1.Field<string>("sapCode");
                        datarow[2] = r.mdtRows2.Field<string>("sapCode");
                        datarow[3] = r.mdtRows3.Field<string>("sapCode");
                        datarow[4] = r.wdtRows[3].ToString();

                        //If you can find a way to add a range (or turn a List<datarow> into a table)
                        //this you can change this section to a select and return the datarow instead
                        dt.Rows.Add(datarow);
                    });
                     var query = dt.AsEnumerable()
        .GroupBy(row => new
        {
            wcName = row.Field<string>("MachineName"),
            manningID1 = row.Field<string>("operator1"),
            manningID2 = row.Field<string>("operator2"),
            manningID3 = row.Field<string>("operator3"),
            day = row.Field<DateTime>("dtandTime").AddHours(-7).Day
        })
        .Select(g => new
        {
            wcName = g.Key.wcName,
            day = g.Key.day,
            manningID1 = g.Key.manningID1,
            manningID2 = g.Key.manningID2,
            manningID3 = g.Key.manningID3,
            quantity = g.Count()
        }
            );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= (daysinMonth + 3); h++)
                                dr[h] = 0;
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].manningID2.ToString() != items[i].manningID2.ToString())
                            || (items[i - 1].manningID3.ToString() != items[i].manningID3.ToString()) || (items[i - 1].wcName.ToString() != items[i].wcName.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= (daysinMonth + 3); h++)
                                dr[h] = 0;
                        }

                        dr[items[i].day.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 4; v <= (daysinMonth + 3); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[daysinMonth + 4] = total;

                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].manningID2.ToString() != items[i + 1].manningID2.ToString())
                            || (items[i].manningID3.ToString() != items[i + 1].manningID3.ToString()) || (items[i].wcName.ToString() != items[i + 1].wcName.ToString()))//
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 4; v <= (daysinMonth + 3); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[daysinMonth + 4] = total;

                            gridviewdt.Rows.Add(dr);
                        }

                    }

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

                    // Calculate total of all workstations of every day
                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    for (int v = 4; v <= (daysinMonth + 4); v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);
                }
                if (getOperator != "All")
                {
                    GridViewoperator.DataSource = gridviewdt;
                    GridViewoperator.DataBind();
                    GridViewoperator.Visible = true;
                    MainGridView.Visible = false;
                }
                else
                {
                    MainGridView.DataSource = gridviewdt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    GridViewoperator.Visible = false;
                }

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("days", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
                {
                    if (gridviewdt.Columns[3 + i].ColumnName != "Total" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][3 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "days";
                TBMChart.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        #endregion

        #region YearWise Report
        protected void showReportYearWcWise(int getYear)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                //Generate datatable columns dynamically
                for (int i = 1; i <= 12; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-01-01 07:00:00";
                string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

               // myConnection.comm.CommandText = "Select wcName, recipeCode,SAPMaterialCode, dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcName, recipeCode,SAPMaterialCode, dtandTime,gtbarCode, ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY wcID, recipeCode, dtandTime asc) rn
                    FROM " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcName, recipeCode, dtandTime asc";
            
                myConnection.reader = myConnection.comm.ExecuteReader();
                myConnection.comm.CommandTimeout = 60;
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(dt.Rows.Count > 0))
                {
                    
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    // myConnection.comm.CommandText = "Select wcName, recipeCode,SAPMaterialCode, dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
                    myConnection.comm.CommandText = @" SELECT * FROM ( SELECT  wcName, recipeCode,SAPMaterialCode, dtandTime,gtbarCode, ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY wcID, recipeCode, dtandTime asc) rn
                    FROM " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + "AND " + durationQuery + "   ) a WHERE rn = 1 ORDER BY wcName, recipeCode, dtandTime asc";


                    myConnection.reader = myConnection.comm.ExecuteReader();
                    myConnection.comm.CommandTimeout = 60;
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                   
                }



                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
        recipeCode = row.Field<string>("recipeCode").Trim(),
        wcName = row.Field<string>("wcName").Trim(),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode"),
    })
    .Select(g => new
    {
        WcName = g.Key.wcName,
        month = g.Key.month,
        recipeCode = g.Key.recipeCode,
        SAPMaterialCode = g.Key.SAPMaterialCode,
        quantity = g.Count()
    }
        );
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {

                    if (i == 0)
                    {
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 3; h <= (12 + 2); h++)
                            dr[h] = 0;
                    }
                    else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i - 1].WcName.ToString())) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 3; h <= (12 + 2); h++)
                            dr[h] = 0;
                    }

                    dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 3; v <= (12 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[12 + 3] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                    else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 3; v <= (12 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[12 + 3] = total;

                        gridviewdt.Rows.Add(dr);
                    }

                    // Wcwise Total
                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= (12 + 3); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    else if (items[i].WcName.ToString() != items[i + 1].WcName.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 3; v <= (12 + 3); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    //End WCwise Total
                }

                MainGridView.Columns.Clear();
                //Iterate through the columns of the datatable to set the data bound field dynamically.
                foreach (DataColumn col in gridviewdt.Columns)
                {
                    //Declare the bound field and allocate memory for the bound field.
                    BoundField bfield = new BoundField();

                    //Initalize the DataField value.
                    bfield.DataField = col.ColumnName;

                    //Initialize the HeaderText field value.
                    if (col.ColumnName == "wcName" || col.ColumnName == "tireType" || col.ColumnName == "Total" || col.ColumnName == "SapCode")
                        bfield.HeaderText = col.ColumnName;
                    else
                        bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);


                    //Add the newly created bound field to the GridView.
                    MainGridView.Columns.Add(bfield);

                }

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                dr[1] = "";
                for (int v = 3; v <= (12 + 3); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // Select the rows where showing total & make them bold
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("months", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
                {
                    if (gridviewdt.Columns[3 + i].ColumnName != "Total" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][3 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "months";
                TBMChart.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportYearRecipeWise(int getYear)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                //Generate datatable columns dynamically
                for (int i = 1; i <= 12; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-01-01 07:00:00";
                string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

              //  myConnection.comm.CommandText = "Select wcName, recipeCode, SAPMaterialCode,dtandTime from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";

               // myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,wcName,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName asc) rn
                 //   FROM " + SQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY wcID,recipeCode, dtandTime asc";

                myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,wcName,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode asc) rn
                    FROM " + SQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY recipeCode, dtandTime asc";


                myConnection.comm.CommandTimeout = 60;
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(dt.Rows.Count > 0))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                   // myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,wcName,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY wcName asc) rn
                   // FROM " + OldSQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY wcID,recipeCode, dtandTime asc";

                    myConnection.comm.CommandText = @"SELECT * FROM ( SELECT  wcID,wcName,recipeCode, SAPMaterialCode,dtandTime, ROW_NUMBER() OVER(PARTITION BY gtbarCode ORDER BY recipeCode asc) rn
                    FROM " + OldSQLTable + " WHERE  wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ) a WHERE rn = 1 ORDER BY recipeCode, dtandTime asc";

                    myConnection.comm.CommandTimeout = 60;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
        recipeCode = row.Field<string>("recipeCode").Trim(),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode"),
    })
    .Select(g => new
    {
        month = g.Key.month,
        recipeCode = g.Key.recipeCode,
        SAPMaterialCode = g.Key.SAPMaterialCode,
        quantity = g.Count()
    }
        );
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {

                    if (i == 0)
                    {
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();
                        // Insert rows in all the days places by default
                        for (int h = 2; h <= (12 + 2); h++)
                            dr[h] = 0;
                    }
                    else if (items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].SAPMaterialCode == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].SAPMaterialCode.ToString();
                        // Insert rows in all the days places by default
                        for (int h = 2; h <= (12 + 2); h++)
                            dr[h] = 0;
                    }

                    dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 2; v <= (12 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[12 + 2] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                    else if (items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 2; v <= (12 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[12 + 2] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                }

                MainGridView.Columns.Clear();
                //Iterate through the columns of the datatable to set the data bound field dynamically.
                foreach (DataColumn col in gridviewdt.Columns)
                {
                    //Declare the bound field and allocate memory for the bound field.
                    BoundField bfield = new BoundField();

                    //Initalize the DataField value.
                    bfield.DataField = col.ColumnName;

                    //Initialize the HeaderText field value.
                    if (col.ColumnName == "tireType" || col.ColumnName == "Total" || col.ColumnName == "SapCode")
                        bfield.HeaderText = col.ColumnName;
                    else
                        bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);

                    //Add the newly created bound field to the GridView.
                    MainGridView.Columns.Add(bfield);

                }

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                for (int v = 2; v <= (12 + 2); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("months", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                {
                    if (gridviewdt.Columns[2 + i].ColumnName != "Total" && gridviewdt.Columns[2 + i].ColumnName != "wcName" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][2 + i];
                        newdt.Rows.Add(drow);
                    }
                }

                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                TBMChart.DataSource = newdt;
                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                TBMChart.Series["TBMSeries"].YValueMembers = "months";
                TBMChart.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void showReportYearOperatorWise(int getYear)
        {
            try
            {
              
                DataTable wcdt = new DataTable();
                DataTable manningdt = new DataTable();
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                DataTable mdt = new DataTable();

                if (getOperator != "All")
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator", typeof(string));
                    dt.Columns.Add("dtandTime", typeof(DateTime));
                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));

                }
                else
                {
                    dt.Columns.Add("MachineName", typeof(string));
                    dt.Columns.Add("operator1", typeof(string));
                    dt.Columns.Add("operator2", typeof(string));
                    dt.Columns.Add("operator3", typeof(string));
                    dt.Columns.Add("dtandTime", typeof(DateTime));

                    gridviewdt.Columns.Add("MachineName", typeof(string));
                    gridviewdt.Columns.Add("operator1", typeof(string));
                    gridviewdt.Columns.Add("operator2", typeof(string));
                    gridviewdt.Columns.Add("operator3", typeof(string));
                }

                //Generate datatable columns dynamically
                for (int i = 1; i <= 12; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                // Get the Data based on WCName
                string fromDate = getYear.ToString() + "-01-01 07:00:00";
                string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();
  
                if (getOperator != "All")
                {
                  //  myConnection.comm.CommandText = "select " + SQLTable + ".wcName,dtandtime,mm.sapCode from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "'  ORDER BY wcName ,dtandTime asc";
                    myConnection.comm.CommandText = @"SELECT * FROM ( select " + SQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                }
                else
                {
                    myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + SQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";
                }
                myConnection.comm.CommandTimeout = 60;
                myConnection.reader = myConnection.comm.ExecuteReader();
                wcdt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (!(wcdt.Rows.Count > 0))
                {


                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    if (getOperator != "All")
                    {
                        //  myConnection.comm.CommandText = "select " + SQLTable + ".wcName,dtandtime,mm.sapCode from " + SQLTable + "  inner join manningmaster mm on " + SQLTable + ".manningID2 = mm.iD  or " + SQLTable + ".manningID3 = mm.iD or " + SQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "'  ORDER BY wcName ,dtandTime asc";
                        myConnection.comm.CommandText = @"SELECT * FROM ( select " + OldSQLTable + ".wcName,dtandtime,mm.sapCode,   ROW_NUMBER() OVER(PARTITION BY gtbarCode  ORDER BY recipeCode) rn from " + OldSQLTable + "  inner join manningmaster mm on " + OldSQLTable + ".manningID2 = mm.iD  or " + OldSQLTable + ".manningID3 = mm.iD or " + OldSQLTable + ".manningID = mm.iD WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " and mm.sapcode = '" + getOperator + "' )a where rn=1  ORDER BY wcName ,dtandTime asc";
                    }
                    else
                    {
                        myConnection.comm.CommandText = "Select (select sapCode from manningMaster where iD= manningID) as manningID, isnull( (select sapCode from manningMaster where iD= manningID2),'unknown') as manningID2,  isnull( (select sapCode from manningMaster where iD= manningID3),'unknown') as manningID3,   dtandTime,wcName from " + OldSQLTable + " WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + "  ORDER BY wcName,manningID, manningID2, manningID3, dtandTime asc";
                    }
                    myConnection.comm.CommandTimeout = 60;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                
                
                }



                // Get the Data based on ManningID
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select id,sapcode from manningMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                manningdt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (getOperator != "All")
                {
                    var query = wcdt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            wcName = row.Field<string>("wcName"),
                            manningID1 = row.Field<string>("sapcode"),
                            month = row.Field<DateTime>("dtandTime").AddHours(-7).Month
                        })
                        .Select(g => new
                        {
                            wcName = g.Key.wcName,
                            month = g.Key.month,
                            manningID1 = g.Key.manningID1,
                            quantity = g.Count()
                        }
                        );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= (12 + 2); h++)
                                dr[h] = 0;
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].wcName.ToString() != items[i].wcName.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 2; h <= (12 + 2); h++)
                                dr[h] = 0;
                        }

                        dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 2; v <= (12 + 2); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[12 + 2] = total;

                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].wcName.ToString() != items[i + 1].wcName.ToString()))// //Check if next recipe or workcenter is different from current one, then insert row in datatable
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 2; v <= (12 + 2); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[12 + 2] = total;

                            gridviewdt.Rows.Add(dr);
                        }

                    }

                    GridViewoperator.Columns.Clear();
                    //Iterate through the columns of the datatable to set the data bound field dynamically.
                    foreach (DataColumn col in gridviewdt.Columns)
                    {
                        //Declare the bound field and allocate memory for the bound field.
                        BoundField bfield = new BoundField();

                        //Initalize the DataField value.
                        bfield.DataField = col.ColumnName;

                        //Initialize the HeaderText field value.
                        //Initialize the HeaderText field value.
                        if (col.ColumnName == "operator1" || col.ColumnName == "Total" || col.ColumnName == "MachineName")
                            bfield.HeaderText = col.ColumnName;
                        else
                            bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);


                        //Add the newly created bound field to the GridView.
                        GridViewoperator.Columns.Add(bfield);

                    }

                    // Calculate total of all workstations of every day
                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";

                    for (int v = 2; v <= (12 + 2); v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);
                }
                else
                {
                var rows = wcdt.AsEnumerable().Join(
                    manningdt.AsEnumerable(),
                    wdtRows => (string.IsNullOrEmpty(wdtRows["manningID"].ToString()) ? 0 : wdtRows["manningID"]),
                    mdtRows => mdtRows.Field<string>("sapCode"),
                    (wdtRows, mdtRows1) => new { wdtRows, mdtRows1 }
                    ) //New anon<datarow,datarow> - I will call anons 'n' (for new) below
                    .Join(
                    manningdt.AsEnumerable(),
                    n => (string.IsNullOrEmpty(n.wdtRows["manningID2"].ToString()) ? 0 : n.wdtRows["manningID2"]),
                    mdtRows2 => mdtRows2.Field<string>("sapCode"),
                    (n, mdtRows2) => new { n.wdtRows, n.mdtRows1, mdtRows2 }
                    )//New anon<datarow,datarow,datarow>
                    .Join(
                    manningdt.AsEnumerable(),
                    n => (string.IsNullOrEmpty(n.wdtRows["manningID3"].ToString()) ? 0 : n.wdtRows["manningID3"]),
                    mdtRows3 => mdtRows3.Field<string>("sapCode"),
                    (n, mdtRows3) => new { n.wdtRows, n.mdtRows1, n.mdtRows2, mdtRows3 }
                    ) //New anon<datarow,datarow,datarow,datarow> - all joined up now
                    .ToList();
                rows.ForEach(r =>
                {
                    var datarow = dt.NewRow();
                    datarow[0] = r.wdtRows[4].ToString();
                    //datarow[1] = r.mdtRows1.Field<string>("firstName") + " " + r.mdtRows1.Field<string>("lastName");
                    datarow[1] = r.mdtRows1.Field<string>("sapCode") ;
                    datarow[2] = r.mdtRows2.Field<string>("sapCode") ;
                    datarow[3] = r.mdtRows3.Field<string>("sapCode") ;
                    datarow[4] = r.wdtRows[3].ToString();

                    //If you can find a way to add a range (or turn a List<datarow> into a table)
                    //this you can change this section to a select and return the datarow instead
                    dt.Rows.Add(datarow);
                });
                
               
                    var query = dt.AsEnumerable()
        .GroupBy(row => new
        {
            wcName = row.Field<string>("MachineName"),
            manningID1 = row.Field<string>("operator1"),
            manningID2 = row.Field<string>("operator2"),
            manningID3 = row.Field<string>("operator3"),
            month = row.Field<DateTime>("dtandTime").AddHours(-7).Month
        })
        .Select(g => new
        {
            wcName = g.Key.wcName,
            month = g.Key.month,
            manningID1 = g.Key.manningID1,
            manningID2 = g.Key.manningID2,
            manningID3 = g.Key.manningID3,

            quantity = g.Count()
        }
            );
                    DataRow dr = gridviewdt.NewRow();
                    var items = query.ToArray();
                    int total = 0;
                    for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                    {
                        if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                        {
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();

                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= (12 + 4); h++)
                                dr[h] = 0;
                        }
                        else if ((items[i - 1].manningID1.ToString() != items[i].manningID1.ToString()) || (items[i - 1].manningID2.ToString() != items[i].manningID2.ToString())
                            || (items[i - 1].manningID3.ToString() != items[i].manningID3.ToString()) || (items[i - 1].wcName.ToString() != items[i].wcName.ToString()))//If Recipe or Workcenter changes, then create new data row
                        {
                            dr = gridviewdt.NewRow();
                            dr[0] = items[i].wcName.ToString();
                            dr[1] = items[i].manningID1.ToString();
                            dr[2] = items[i].manningID2.ToString();
                            dr[3] = items[i].manningID3.ToString();
                            // Insert rows in all the hours places by default
                            for (int h = 4; h <= (12 + 4); h++)
                                dr[h] = 0;
                        }

                        dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

                        if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 4; v <= (12 + 4); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[12 + 4] = total;

                            gridviewdt.Rows.Add(dr);
                        }
                        else if ((items[i].manningID1.ToString() != items[i + 1].manningID1.ToString()) || (items[i].manningID2.ToString() != items[i + 1].manningID2.ToString())
                            || (items[i].manningID3.ToString() != items[i + 1].manningID3.ToString()) || (items[i].wcName.ToString() != items[i + 1].wcName.ToString()))// //Check if next recipe or workcenter is different from current one, then insert row in datatable
                        {
                            total = 0;
                            // Display Whole Month Total
                            for (int v = 4; v <= (12 + 4); v++)
                                total += Convert.ToInt32(dr[v]);

                            dr[12 + 4] = total;

                            gridviewdt.Rows.Add(dr);
                        }

                    }

                    MainGridView.Columns.Clear();
                    //Iterate through the columns of the datatable to set the data bound field dynamically.
                    foreach (DataColumn col in gridviewdt.Columns)
                    {
                        //Declare the bound field and allocate memory for the bound field.
                        BoundField bfield = new BoundField();

                        //Initalize the DataField value.
                        bfield.DataField = col.ColumnName;

                        //Initialize the HeaderText field value.
                        //Initialize the HeaderText field value.
                        if (col.ColumnName == "operator1" || col.ColumnName == "operator2" || col.ColumnName == "operator3" || col.ColumnName == "Total" || col.ColumnName == "MachineName")
                            bfield.HeaderText = col.ColumnName;
                        else
                            bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);


                        //Add the newly created bound field to the GridView.
                        MainGridView.Columns.Add(bfield);

                    }

                    // Calculate total of all workstations of every day
                    dr = gridviewdt.NewRow();
                    dr[0] = "Total";
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    for (int v = 4; v <= (12 + 4); v++)
                    {
                        dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                    }
                    gridviewdt.Rows.Add(dr);
                    }

                if (getOperator != "All")
                {
                    GridViewoperator.DataSource = gridviewdt;
                    GridViewoperator.DataBind();
                    GridViewoperator.Visible = true;
                    MainGridView.Visible = false;
                }
                else
                {
                    MainGridView.DataSource = gridviewdt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    GridViewoperator.Visible = false;
                }

                    gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                    DataTable newdt = new DataTable();
                    newdt.Columns.Add("columnName", typeof(string));
                    newdt.Columns.Add("months", typeof(string));
                    for (int i = 0; i < (gridviewdt.Columns.Count - 4); i++)
                    {
                        if (gridviewdt.Columns[3 + i].ColumnName != "Total" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                        {
                            DataRow drow = newdt.NewRow();
                            drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                            drow[1] = gridviewdt.Rows[0][3 + i];
                            newdt.Rows.Add(drow);
                        }
                    }

                    TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
                    TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                    TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
                    TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

                    TBMChart.DataSource = newdt;
                    TBMChart.Series["TBMSeries"].XValueMember = "columnName";
                    TBMChart.Series["TBMSeries"].YValueMembers = "months";

                    TBMChart.DataBind();
                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
          
        
        #endregion

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
                            cell.Text = "";
                            cell.ColumnSpan = 2;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.Text = "A";
                            cell.ColumnSpan = 9;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "B";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "C";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 1;
                            cell.Text = "";
                            row.Controls.Add(cell);
                            break;
                        case "RecipeWise":
                            cell.Text = "";
                            cell.ColumnSpan = 1;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.Text = "A";
                            cell.ColumnSpan = 9;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "B";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "C";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 1;
                            cell.Text = "";
                            row.Controls.Add(cell);
                            break;
                        case "OperatorWise":
                           
                            cell.Text = "Operator";
                            cell.ColumnSpan = 4;
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.Text = "A";
                            cell.ColumnSpan = 9;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "B";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "C";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 1;
                            cell.Text = "";
                            row.Controls.Add(cell);
                            break;

                    }
                    MainGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    
                    break;
            }

        }
        protected void opOnDataBound(object sender, EventArgs e)
        {
            switch (duration)
            {
                case "Date":
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell cell = new TableHeaderCell();

                    switch (getType)
                    {
                        
                        case "OperatorWise":

                            cell.Text = "";
                            cell.ColumnSpan = 2;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.Text = "A";
                            cell.ColumnSpan = 9;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "B";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 9;
                            cell.Text = "C";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 1;
                            cell.Text = "";
                            row.Controls.Add(cell);
                            break;

                    }
                    GridViewoperator.HeaderRow.Parent.Controls.AddAt(0, row);

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