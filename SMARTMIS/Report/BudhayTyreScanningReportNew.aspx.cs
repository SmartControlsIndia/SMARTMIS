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
using OfficeOpenXml;

namespace SmartMIS.Report
{
    public partial class BudhayTyreScanningReportNew : System.Web.UI.Page
    {
        #region Global Variables

        int flag = 0;
        string[] xvalues;
        string queryString = null;
        string durationQuery = "";

        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        DataTable reportdt = new DataTable();
        string day, month, year, duration;
        DateTime fromDate, toDate;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            ErrorMsg.Visible = false;
            HeaderText.Text = "";
        }


        protected void viewReport_Click(object sender, EventArgs e)
        {
            try
            {
                string getMonth = DropDownListMonth.SelectedValue;
                string getYear = DropDownListYear.SelectedItem.Text;
           
                var datetimebt = "";
                string getfromdate = reportMasterFromDateTextBox.Text; ;

                switch (DropDownListDuration.SelectedItem.Value)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = formatDate(reportMasterFromDateTextBox.Text);
                        toDate = DateTime.Parse(nfromDate);
                        string ntoDate = toDate.AddDays(1).ToString("MM - dd - yyyy");
                        showReportDateWise(nfromDate, ntoDate);
                        break;


                    case "Month":
                        nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                        if (Convert.ToInt32(getMonth) < 12)
                        {
                            datetimebt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                        }
                        else
                        { datetimebt = getYear.ToString() + "-" + (getMonth) + "-31 07:00:00"; }

                        ntoDate = datetimebt;
                        showReportMonthWise(getMonth, getYear);

                        break;

                }


            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void showReportDateWise(string fromDate, string toDate)
        {

            if (validateInput("date", "", fromDate, toDate, 0, 0, 0))
            {
                showReportDayRecipeWise(fromDate, toDate);     
            }
        }

        protected void showReportMonthWise(string getMonth, string getYear)
        {
           int getMonthh = Convert.ToInt32(getMonth);
           int getYearr = Convert.ToInt32(getYear);
            if (validateInput("month", "", "", "", getMonthh, getYearr, 0))
            {
                showReportMonthRecipeWise(getMonthh, getYearr);        
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
                    case "month":
                        string rtoDate = "";
                        string rfromDate = year.ToString() + "-" + month + "-01 07:00:00";
                        if (month == 12)
                            rtoDate = (year + 1).ToString() + "-01-01 07:00:00";
                        else
                            rtoDate = year.ToString() + "-" + (month + 1) + "-01 07:00:00"; durationQuery += "(dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
                        break;
                   
                }

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
        }

        protected void showReportDayRecipeWise(string fromDate, string toDate)
        {
            try
            {            
                
                DataTable wcdt = new DataTable();
                DataTable dtExport = new DataTable();
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("RecipeCode", typeof(string));
                gridviewdt.Columns.Add("Product Name", typeof(string));
                gridviewdt.Columns.Add("Destination", typeof(string));
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

               // myConnection.comm.CommandText = @" select distinct BTD.gtbarcode ,BTD.destination,isnull(BTD.recipeCode,'Unknown')as recipeCode,RM.SAPMaterialCode,RM.description,BTD.dtandTime from [BuddeScannedTyreDetail]  BTD inner join recipeMaster RM on BTD.recipeCode = RM.name where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and BTD.stationNo='3' and BTD.destination not in('00','01')  order by BTD.destination asc ";
               

                if (ddlshift.SelectedItem.Text == "Normal Barcode")
                {
                    myConnection.comm.CommandText = @" SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime FROM( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime,
                        ROW_NUMBER() OVER(PARTITION BY gtbarcode ORDER BY destination asc) rn
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination not in('00')  ) a WHERE rn = 1) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name  where recipeCode !='Unknown' ORDER BY destination asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    // myConnection.comm.CommandText = @"select distinct BTD.gtbarcode As BARCODE ,isnull(BTD.recipeCode,'Unknown')as  'RECIPE CODE',RM.description AS 'TIRE SIZE',RM.SAPMaterialCode AS SEPCODE,BTD.destination AS DESTINATION,convert(char(10),  BTD.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) ,  BTD.dtandTime , 108) AS [TIME],BTD.dtandtime from [BuddeScannedTyreDetail]  BTD inner join recipeMaster RM on BTD.recipeCode = RM.name where BTD.dtandtime>='" + fromDate + " 07:00:00' and BTD.dtandtime<'" + toDate + " 06:59:59' and BTD.stationNo='3' and BTD.destination not in('00','01')  order by BTD.dtandtime asc";
                    myConnection.comm.CommandText = @"SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime FROM ( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime,
                        ROW_NUMBER() OVER(PARTITION BY gtbarcode ORDER BY dtandtime DESC) rn
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination not in('00') ) a WHERE rn = 1) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name where recipeCode !='Unknown' ORDER BY dtandTime";   
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtExport.Load(myConnection.reader);
                    myConnection.reader.Close();
                    ViewState["dt"] = dtExport;
                }
                else if (ddlshift.SelectedItem.Text == "Technical Barcode")
                {
                    myConnection.comm.CommandText = @"  SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime FROM ( SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime  
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination in('11')  and gtbarcode !='??????????') DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name  ORDER BY destination asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    // myConnection.comm.CommandText = @"select distinct BTD.gtbarcode As BARCODE ,isnull(BTD.recipeCode,'Unknown')as  'RECIPE CODE',RM.description AS 'TIRE SIZE',RM.SAPMaterialCode AS SEPCODE,BTD.destination AS DESTINATION,convert(char(10),  BTD.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) ,  BTD.dtandTime , 108) AS [TIME],BTD.dtandtime from [BuddeScannedTyreDetail]  BTD inner join recipeMaster RM on BTD.recipeCode = RM.name where BTD.dtandtime>='" + fromDate + " 07:00:00' and BTD.dtandtime<'" + toDate + " 06:59:59' and BTD.stationNo='3' and BTD.destination not in('00','01')  order by BTD.dtandtime asc";
                    myConnection.comm.CommandText = @"	SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime FROM ( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination in('11')  and gtbarcode !='??????????') a) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY dtandTime";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtExport.Load(myConnection.reader);
                    myConnection.reader.Close();
                    ViewState["dt"] = dtExport;
                
                }
                else if (ddlshift.SelectedItem.Text == "Unknown Barcode")
                {
                    myConnection.comm.CommandText = @"  SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime FROM ( SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime  
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination in('11') and gtbarcode ='??????????') DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name  ORDER BY destination asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    // myConnection.comm.CommandText = @"select distinct BTD.gtbarcode As BARCODE ,isnull(BTD.recipeCode,'Unknown')as  'RECIPE CODE',RM.description AS 'TIRE SIZE',RM.SAPMaterialCode AS SEPCODE,BTD.destination AS DESTINATION,convert(char(10),  BTD.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) ,  BTD.dtandTime , 108) AS [TIME],BTD.dtandtime from [BuddeScannedTyreDetail]  BTD inner join recipeMaster RM on BTD.recipeCode = RM.name where BTD.dtandtime>='" + fromDate + " 07:00:00' and BTD.dtandtime<'" + toDate + " 06:59:59' and BTD.stationNo='3' and BTD.destination not in('00','01')  order by BTD.dtandtime asc";
                    myConnection.comm.CommandText = @"	SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime FROM ( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 06:59:59' and stationNo='3' and destination in('11')  and gtbarcode ='??????????') a) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY dtandTime";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtExport.Load(myConnection.reader);
                    myConnection.reader.Close();
                    ViewState["dt"] = dtExport;

                }         
              
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                var query = wcdt.AsEnumerable()
                                .GroupBy(row => new
                                {
                                    Hour = row.Field<DateTime>("dtandTime").Hour,
                                    recipeCode = row.Field<string>("recipeCode"),
                                    description = row.Field<string>("description"),
                                    destination = row.Field<string>("destination"),
                                    SAPMaterialCode = row.Field<string>("SAPMaterialCode")
                                })
                                .Select(g => new
                                {
                                    Hour = g.Key.Hour,
                                    recipeCode = g.Key.recipeCode,
                                    description = g.Key.description,
                                    destination = g.Key.destination,
                                    SAPMaterialCode = g.Key.SAPMaterialCode,
                                    quantity = g.Count()
                                }
                             );

                myWebService.writeLogs(query.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        
                DataRow dr = gridviewdt.NewRow();
                var items = query.ToArray();
                int total = 0;
                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
                {
                    if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
                    {
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].description == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].description.ToString();

                        if (items[i].destination == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].destination.ToString();

                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();


                        // Insert rows in all the hours places by default
                        for (int h = 4; h <= 31; h++)
                        {
                            dr[h] = 0;
                        }
                    }
                    else if (items[i - 1].recipeCode.ToString().Trim() != items[i].recipeCode.ToString().Trim()) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].description == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].description.ToString();

                        if (items[i].destination == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].destination.ToString();

                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the hours places by default
                        for (int h = 4; h <= 31; h++)
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
                    else if (items[i].recipeCode.ToString().Trim() != items[i + 1].recipeCode.ToString().Trim()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
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

                 //Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                for (int v = 4; v <= 31; v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);


                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // ViewState["dt"] = gridviewdt;

                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
                        .Where(row => row.Cells[0].Text == "Total" || row.Cells[0].Text == "GrandTotal");

                foreach (var row in rows)
                    row.Font.Bold = true;

                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                DataTable newdt = new DataTable();
                newdt.Columns.Add("columnName", typeof(string));
                newdt.Columns.Add("hours", typeof(string));
                for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
                {
                    if (gridviewdt.Columns[3 + i].ColumnName != "TotalA" && gridviewdt.Columns[3 + i].ColumnName != "TotalB" && gridviewdt.Columns[3 + i].ColumnName != "TotalC" && gridviewdt.Columns[3 + i].ColumnName != "DayTotal" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
                    {
                        DataRow drow = newdt.NewRow();
                        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
                        drow[1] = gridviewdt.Rows[0][3 + i];
                        newdt.Rows.Add(drow);
                    }
                }

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
      
        protected void showReportMonthRecipeWise(int getMonth, int getYear)
        {
            try
            {
                
                DataTable dt = new DataTable();
                DataTable dtExport = new DataTable();
                
                DataTable gridviewdt = new DataTable();
                int daysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                gridviewdt.Columns.Add("RecipeCode", typeof(string));
                gridviewdt.Columns.Add("Product Name", typeof(string));
                gridviewdt.Columns.Add("Destination", typeof(string));
                gridviewdt.Columns.Add("SapCode", typeof(string));
                //Generate datatable columns dynamically
                for (int i = 1; i <= daysinMonth; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                string toDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 06:59:59";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (ddlshift.SelectedItem.Text == "Normal Barcode") 
                {
                    myConnection.comm.CommandText = @" SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime FROM( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime,
                        ROW_NUMBER() OVER(PARTITION BY gtbarcode ORDER BY destination asc) rn
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination not in('00','11')  ) a WHERE rn = 1) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name  where recipeCode !='Unknown' ORDER BY destination,recipeCode asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    myConnection.comm.CommandText = @"SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime FROM ( SELECT * FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime,
                        ROW_NUMBER() OVER(PARTITION BY gtbarcode ORDER BY dtandtime asc) rn
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination not in('00','11') ) a WHERE rn = 1) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name  where recipeCode !='Unknown' ORDER BY dtandTime";
             
                
                }
                else if (ddlshift.SelectedItem.Text == "Technical Barcode")
                {
                    myConnection.comm.CommandText = @" SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime  FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination in('11') and gtbarcode !='??????????') DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY destination,recipeCode asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    myConnection.comm.CommandText = @"SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime  FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination in('11') and gtbarcode !='??????????' ) DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY dtandTime";
                           
                }

                else if (ddlshift.SelectedItem.Text == "Unknown Barcode")
                {
                    myConnection.comm.CommandText = @" SELECT gtbarcode,destination,recipeCode,recipeMaster.SAPMaterialCode,recipeMaster.description,dtandTime  FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination in('11') and gtbarcode ='??????????') DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY destination,recipeCode asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));


                    myConnection.comm.CommandText = @"SELECT gtbarcode As BARCODE,recipeCode as  'RECIPE CODE',recipeMaster.description AS 'TIRE SIZE',SAPMaterialCode AS SEPCODE,destination AS DESTINATION ,convert(char(10),  dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],dtandtime  FROM (  SELECT gtbarcode ,destination,isnull(recipeCode,'Unknown')as recipeCode,dtandTime
                    FROM [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and stationNo='3' and destination in('11') and gtbarcode ='??????????') DBNew  inner join recipeMaster on DBNew.recipeCode = recipeMaster.name ORDER BY dtandTime";

                }
                
                myConnection.reader = myConnection.comm.ExecuteReader();
                dtExport.Load(myConnection.reader);
                myConnection.reader.Close();
                ViewState["dt"] = dtExport;

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        day = row.Field<DateTime>("dtandTime").AddHours(-7).Day,
        recipeCode = row.Field<string>("recipeCode"),
        description = row.Field<string>("description"),
        destination = row.Field<string>("destination"),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode")
    })
    .Select(g => new
    {
        day = g.Key.day,
        recipeCode = g.Key.recipeCode,
        description = g.Key.description,
        destination = g.Key.destination,
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
                        if (items[i].description == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].description.ToString();

                        if (items[i].destination == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].destination.ToString();

                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 4; h <= (daysinMonth + 3); h++)
                            dr[h] = 0;
                    }
                    else if (items[i - 1].recipeCode.ToString().Trim() != items[i].recipeCode.ToString().Trim()) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].recipeCode.ToString();
                        if (items[i].description == null)
                            dr[1] = "";
                        else
                            dr[1] = items[i].description.ToString();


                        if (items[i].destination == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].destination.ToString();

                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();

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
                    else if (items[i].recipeCode.ToString().Trim() != items[i + 1].recipeCode.ToString().Trim()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
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

                // Calculate total of all workstations of every hour
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                for (int v = 4; v <= (daysinMonth + 4); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total" || row.Cells[0].Text == "GrandTotal");

                foreach (var row in rows)
                    row.Font.Bold = true;

                //ViewState["dt"] = gridviewdt;
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

               
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
      
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
                //case "Date":
                //    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                //    TableHeaderCell cell = new TableHeaderCell();

                //        case "RecipeWise":
                //            cell.Text = "";
                //            cell.ColumnSpan = 2;
                //            row.Controls.Add(cell);

                //            cell = new TableHeaderCell();
                //            cell.Text = "A";
                //            cell.ColumnSpan = 9;
                //            row.Controls.Add(cell);

                //            cell = new TableHeaderCell();
                //            cell.ColumnSpan = 9;
                //            cell.Text = "B";
                //            row.Controls.Add(cell);
                //            cell = new TableHeaderCell();
                //            cell.ColumnSpan = 9;
                //            cell.Text = "C";
                //            row.Controls.Add(cell);
                //            cell = new TableHeaderCell();
                //            cell.ColumnSpan = 1;
                //            cell.Text = "";
                //            row.Controls.Add(cell);
                //            break;
                        

                    
                //    MainGridView.HeaderRow.Parent.Controls.AddAt(0, row);

                //    break;
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

        protected void Export_click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            DataTable dtNew = dt.AsEnumerable().CopyToDataTable();
            dtNew.Columns.Remove("dtandtime");

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BuddheExit.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TBRbuddheExitStationReportNew");
            ws.Cells["A1"].LoadFromDataTable((DataTable)dtNew, true, OfficeOpenXml.Table.TableStyles.Medium2);
            ws.Cells.AutoFitColumns();
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();
        }

    }
}
