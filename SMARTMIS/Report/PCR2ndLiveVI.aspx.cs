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
    public partial class PCR2ndLiveVI : System.Web.UI.Page
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
        public string getdisplaytype = "Numbers";

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
                string getYearwise = DropDownListYearWise.SelectedItem.Text;

                var datetimebt = "";
                string getfromdate = reportMasterFromDateTextBox.Text; ;

                switch (DropDownListDuration.SelectedItem.Value)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);
                        string nfromDate = fromDate.ToString("MM-dd-yyyy");
                        string ntoDate = toDate.ToString("MM-dd-yyyy");
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
                    case "Year":
                        showReportYearWcWise(Convert.ToInt32(getYearwise));
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

                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("MACHINENAME", typeof(string));

                gridviewdt.Columns.Add("TIRETYPE", typeof(string));

                gridviewdt.Columns.Add("PRODUCT NAME", typeof(string));

                gridviewdt.Columns.Add("SAPCODE", typeof(string));

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

                //if (ddldecision.SelectedValue == "ALL")
                //{    
                //    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='')  and " + durationQuery + " and curingRecipeName!=''  order by wcname asc";//@"select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]    where " + durationQuery + " and curingRecipeName!=''  order by wcname asc ";
                //}
                //else
                //{
                //    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and (" + durationQuery + " and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  order by wcname asc";   
                //}

                if (ddldecision.SelectedValue == "ALL")
                {
                    myConnection.comm.CommandText = @" select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='')  and " + durationQuery + " and curingRecipeName!=''   order by wcname,curingRecipeName,dtandtime desc";
                }
                else if (ddldecision.SelectedValue == "333")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and " + durationQuery + " and curingRecipeName!='' and status=1 and TUOGrade=1 or TUOGrade=2   order by wcname,curingRecipeName,dtandtime desc";
                }
                else if (ddldecision.SelectedValue == "444")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and " + durationQuery + " and curingRecipeName!='' and status=1 and TUOGrade=4 order by wcname,curingRecipeName,dtandtime desc";
                }
                else
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and " + durationQuery + " and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and " + durationQuery + " and curingRecipeName!='' and  status=" + ddldecision.SelectedValue + "  order by wcname,curingRecipeName,dtandtime desc";
                }
   
                myConnection.reader = myConnection.comm.ExecuteReader();

                wcdt.Load(myConnection.reader);

                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                myConnection.reader.Close();

                myConnection.comm.Dispose();

                myConnection.close(ConnectionOption.SQL);

                var query = wcdt.AsEnumerable()

                                .GroupBy(row => new

                                {

                                    Hour = row.Field<DateTime>("dtandTime").Hour,

                                    recipeCode = row.Field<string>("curingRecipeName"),

                                    wcname = row.Field<string>("wcname"),

                                    description = row.Field<string>("description"),

                                    SAPMaterialCode = row.Field<string>("SAPMaterialCode")

                                })

                                .Select(g => new

                                {

                                    Hour = g.Key.Hour,

                                    recipeCode = g.Key.recipeCode,

                                    wcname = g.Key.wcname,

                                    description = g.Key.description,

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

                        dr[0] = items[i].wcname.ToString();

                        dr[1] = items[i].recipeCode.ToString();



                        if (items[i].description == null)

                            dr[2] = "";

                        else

                            dr[2] = items[i].description.ToString();



                        if (items[i].SAPMaterialCode == null)

                            dr[3] = "";

                        else

                            dr[3] = items[i].SAPMaterialCode.ToString();





                        // Insert rows in all the hours places by default

                        for (int h = 4; h <= 30; h++)
                        {

                            dr[h] = 0;

                        }

                    }

                    else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].wcname.ToString() != items[i - 1].wcname.ToString())) //If Recipe or Workcenter changes, then create new data row
                    {

                        dr = gridviewdt.NewRow();

                        dr[0] = items[i].wcname.ToString();

                        dr[1] = items[i].recipeCode.ToString();



                        if (items[i].description == null)

                            dr[2] = "";

                        else

                            dr[2] = items[i].description.ToString();



                        if (items[i].SAPMaterialCode == null)

                            dr[3] = "";

                        else

                            dr[3] = items[i].SAPMaterialCode.ToString();



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

                    else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].wcname.ToString() != items[i + 1].wcname.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
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



                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {

                        DataRow ndr = gridviewdt.NewRow();

                        ndr[0] = "";

                        ndr[1] = "Total";

                        for (int v = 4; v <= 31; v++)
                        {

                            ndr[v] = gridviewdt.AsEnumerable()

            .Where(r => r.Field<string>(0) == items[i].wcname)

            .Sum(r => r.Field<int>(v));



                        }

                        gridviewdt.Rows.Add(ndr);

                    }

                    else if (items[i].wcname.ToString() != items[i + 1].wcname.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
                    {

                        DataRow ndr = gridviewdt.NewRow();

                        ndr[0] = "";

                        ndr[1] = "Total";

                        for (int v = 4; v <= 31; v++)
                        {

                            ndr[v] = gridviewdt.AsEnumerable()

            .Where(r => r.Field<string>(0) == items[i].wcname)

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



                //Calculate total of all workstations of every hour

                dr = gridviewdt.NewRow();

                gridviewdt.Rows.Add(dr);

                dr = gridviewdt.NewRow();

                dr[0] = "Total";

                dr[1] = "";

                dr[2] = "";

                for (int v = 4; v <= 31; v++)
                {

                    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));



                }

                gridviewdt.Rows.Add(dr);



                dr = gridviewdt.NewRow();

                gridviewdt.Rows.Add(dr);



                MainGridView.DataSource = gridviewdt;

                //MainGridView.DataBind();

                //dr = gridviewdt.NewRow();

                //dr[0] = "Total";

                //for (int v = 4; v <= 31; v++)

                //{

                //    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));



                //}

                //gridviewdt.Rows.Add(dr);





                //MainGridView.DataSource = gridviewdt;

                MainGridView.DataBind();

                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()

   .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)

                    row.Font.Bold = true;



                //gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();



                //DataTable newdt = new DataTable();

                //newdt.Columns.Add("columnName", typeof(string));

                //newdt.Columns.Add("hours", typeof(string));

                //for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)

                //{

                //    if (gridviewdt.Columns[3 + i].ColumnName != "TotalA" && gridviewdt.Columns[3 + i].ColumnName != "TotalB" && gridviewdt.Columns[3 + i].ColumnName != "TotalC" && gridviewdt.Columns[3 + i].ColumnName != "DayTotal" && gridviewdt.Columns[3 + i].ColumnName != "tireType")

                //    {

                //        DataRow drow = newdt.NewRow();

                //        drow[0] = gridviewdt.Columns[3 + i].ColumnName;

                //        drow[1] = gridviewdt.Rows[0][3 + i];

                //        newdt.Rows.Add(drow);

                //    }

                //}



                ViewState["dt"] = gridviewdt;

            }

            catch (Exception exp)
            {

                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }

        }

   //     protected void showReportDayRecipeWise(string fromDate, string toDate)
   //     {
   //         try
   //         {
   //             DataTable wcdt = new DataTable();
   //             DataTable gridviewdt = new DataTable();
   //             gridviewdt.Columns.Add("MACHINENAME", typeof(string));
   //             gridviewdt.Columns.Add("TIRETYPE", typeof(string));
   //             gridviewdt.Columns.Add("PRODUCT NAME", typeof(string));
   //             gridviewdt.Columns.Add("SAPCODE", typeof(string));
   //             for (int i = 7; i <= 23; i++)
   //             {
   //                 gridviewdt.Columns.Add(i.ToString(), typeof(int));
   //                 if (i == 14)
   //                     gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
   //                 else if (i == 22)
   //                     gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
   //             }
   //             for (int i = 0; i <= 6; i++)
   //                 gridviewdt.Columns.Add(i.ToString(), typeof(int));
   //             gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
   //             gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));

   //             // Get the Data based on WCName
   //             myConnection.open(ConnectionOption.SQL);
   //             myConnection.comm = myConnection.conn.CreateCommand();

   //             if (ddldecision.SelectedValue == "ALL")
   //             {
   //                 myConnection.comm.CommandText = @" select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='')  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!=''   order by wcname asc";
   //             }
   //             else if (ddldecision.SelectedValue == "333")
   //             {
   //                 myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=1 or TUOGrade=2   order by wcname asc";
   //             }
   //             else if (ddldecision.SelectedValue == "444")
   //             {
   //                 myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=4 order by wcname asc";
   //             }
   //             else
   //             {
   //                 myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and  status=" + ddldecision.SelectedValue + "  order by wcname asc";
   //             }
   
   //             myConnection.reader = myConnection.comm.ExecuteReader();
   //             wcdt.Load(myConnection.reader);
   //             myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

   //             myConnection.reader.Close();
   //             myConnection.comm.Dispose();
   //             myConnection.close(ConnectionOption.SQL);

   //             var query = wcdt.AsEnumerable()
   //                             .GroupBy(row => new
   //                             {
   //                                 Hour = row.Field<DateTime>("dtandTime").Hour,
   //                                 recipeCode = row.Field<string>("curingRecipeName"),
   //                                 wcname = row.Field<string>("wcname"),
   //                                 description = row.Field<string>("description"),
   //                                 SAPMaterialCode = row.Field<string>("SAPMaterialCode")
   //                             })
   //                             .Select(g => new
   //                             {
   //                                 Hour = g.Key.Hour,
   //                                 recipeCode = g.Key.recipeCode,
   //                                 wcname = g.Key.wcname,
   //                                 description = g.Key.description,
   //                                 SAPMaterialCode = g.Key.SAPMaterialCode,
   //                                 quantity = g.Count()
   //                             }
   //                          );

   //                      // myWebService.writeLogs(query.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

   //             DataRow dr = gridviewdt.NewRow();
   //             var items = query.ToArray();
   //             int total = 0;
   //             for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
   //             {
   //                 if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
   //                 {
   //                     dr[0] = items[i].wcname.ToString();
   //                     dr[1] = items[i].recipeCode.ToString();

   //                     if (items[i].description == null)
   //                         dr[2] = "";
   //                     else
   //                         dr[2] = items[i].description.ToString();

   //                     if (items[i].SAPMaterialCode == null)
   //                         dr[3] = "";
   //                     else
   //                         dr[3] = items[i].SAPMaterialCode.ToString();


   //                     // Insert rows in all the hours places by default
   //                     for (int h = 4; h <= 31; h++)
   //                     {
   //                         dr[h] = 0;
   //                     }
   //                 }
   //                 //else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable

   //                 else if (items[i - 1].recipeCode.ToString().Trim() != items[i].recipeCode.ToString().Trim()) // || (items[i].wcname.ToString().Trim() != items[i - 1].wcname.ToString().Trim())) //If Recipe or Workcenter changes, then create new data row
   //                 {
   //                     dr = gridviewdt.NewRow();
   //                     dr[0] = items[i].wcname.ToString(); 
   //                     dr[1] = items[i].recipeCode.ToString();

   //                     if (items[i].description == null)
   //                         dr[2] = "";
   //                     else
   //                         dr[2] = items[i].description.ToString();

   //                     if (items[i].SAPMaterialCode == null)
   //                         dr[3] = "";
   //                     else
   //                         dr[3] = items[i].SAPMaterialCode.ToString();


   //                     // Insert rows in all the hours places by default
   //                     for (int h = 4; h <= 31; h++)
   //                     {
   //                         dr[h] = 0;
   //                     }
   //                 }
   //                 int getHour = items[i].Hour; //Store current array hour
   //                 int getDifference = 0; //Store time difference of current to previous day
   //                 if (i > 0)
   //                 {
   //                     if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
   //                         getDifference = items[i].Hour - items[i - 1].Hour;
   //                     else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
   //                         getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
   //                 }

   //                 dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

   //                 if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
   //                 {
   //                     total = 0;
   //                     // Display Shift Total
   //                     for (int v = 4; v <= 30; v++)
   //                     {
   //                         if (v >= 4 && v <= 11) //Calculate A shift values
   //                             total += Convert.ToInt32(dr[v]);
   //                         else if (v > 12 && v < 21) //Calculate B shift values
   //                             total += Convert.ToInt32(dr[v]);
   //                         else if (v > 21 && v < 30) //Calculate C shift values
   //                             total += Convert.ToInt32(dr[v]);

   //                         if (v == 12) //if all A shift values processed, then add total of A shift
   //                         {
   //                             dr[12] = total;
   //                             total = 0;
   //                         }
   //                         else if (v == 21) //if all B shift values processed, then add total of A shift
   //                         {
   //                             dr[21] = total;
   //                             total = 0;
   //                         }
   //                         else if (v == 30) //if all C shift values processed, then add total of A shift
   //                         {
   //                             dr[31] = total;
   //                             total = 0;

   //                             //Set Day Total
   //                             dr[31] = Convert.ToInt32(dr[12]) + Convert.ToInt32(dr[21]) + Convert.ToInt32(dr[30]);
   //                         }

   //                     }
   //                     gridviewdt.Rows.Add(dr);
   //                 }
   //                 else if (items[i].recipeCode.ToString().Trim() != items[i + 1].recipeCode.ToString().Trim())// || (items[i].wcname.ToString().Trim() != items[i + 1].wcname.ToString().Trim())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
   //                 {
   //                     total = 0;
   //                     // Display Shift Total
   //                     for (int v = 4; v <= 30; v++)
   //                     {
   //                         if (v >= 4 && v <= 11) //Calculate A shift values
   //                             total += Convert.ToInt32(dr[v]);
   //                         else if (v > 12 && v < 21) //Calculate B shift values
   //                             total += Convert.ToInt32(dr[v]);
   //                         else if (v > 21 && v < 30) //Calculate C shift values
   //                             total += Convert.ToInt32(dr[v]);

   //                         if (v == 12) //if all A shift values processed, then add total of A shift
   //                         {
   //                             dr[12] = total;
   //                             total = 0;
   //                         }
   //                         else if (v == 21) //if all B shift values processed, then add total of A shift
   //                         {
   //                             dr[21] = total;
   //                             total = 0;
   //                         }
   //                         else if (v == 30) //if all C shift values processed, then add total of A shift
   //                         {
   //                             dr[30] = total;
   //                             total = 0;

   //                             //Set Day Total
   //                             dr[31] = Convert.ToInt32(dr[12]) + Convert.ToInt32(dr[21]) + Convert.ToInt32(dr[30]);
   //                         }
   //                     }
   //                     gridviewdt.Rows.Add(dr);
   //                 }
   //                 //for wcnme row wise total added on 27/11/2020 by sarita
   //                 if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
   //                 {
   //                     DataRow ndr = gridviewdt.NewRow();
   //                     ndr[0] = "";
   //                     ndr[1] = "Total";
   //                     for (int v = 4; v <= 31; v++)
   //                     {
   //                         ndr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(0) == items[i].wcname) .Sum(r => r.Field<int>(v));
   //                     }
   //                     gridviewdt.Rows.Add(ndr);
   //                 }
   //                 else if (items[i].wcname.ToString() != items[i + 1].wcname.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
   //                 {
   //                     DataRow ndr = gridviewdt.NewRow();
   //                     ndr[0] = "";
   //                     ndr[1] = "Total";
   //                     for (int v = 4; v <= 31; v++)
   //                     {
   //                         ndr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(0) == items[i].wcname).Sum(r => r.Field<int>(v));
   //                     }
   //                     gridviewdt.Rows.Add(ndr);
   //                 }
   //             }

   //             MainGridView.Columns.Clear();
   //             //Iterate through the columns of the datatable to set the data bound field dynamically.
   //             foreach (DataColumn col in gridviewdt.Columns)
   //             {
   //                 //Declare the bound field and allocate memory for the bound field.
   //                 BoundField bfield = new BoundField();

   //                 //Initalize the DataField value.
   //                 bfield.DataField = col.ColumnName;

   //                 //Initialize the HeaderText field value.
   //                 bfield.HeaderText = col.ColumnName;

   //                 //Add the newly created bound field to the GridView.
   //                 MainGridView.Columns.Add(bfield);

   //             }

   //             //Calculate total of all workstations of every hour
   //             dr = gridviewdt.NewRow();
   //             gridviewdt.Rows.Add(dr);
   //             dr = gridviewdt.NewRow();
   //             dr[0] = "Total";
   //             dr[1] = "";
   //             dr[2] = "";
   //             for (int v = 4; v <= 31; v++)
   //             {
   //                 dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

   //             }
   //             gridviewdt.Rows.Add(dr);

   //             dr = gridviewdt.NewRow();
   //             gridviewdt.Rows.Add(dr);

   //             MainGridView.DataSource = gridviewdt;
   //             //MainGridView.DataBind();
   //             //dr = gridviewdt.NewRow();
   //             //dr[0] = "Total";
   //             //for (int v = 4; v <= 31; v++)
   //             //{
   //             //    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

   //             //}
   //             //gridviewdt.Rows.Add(dr);


   //             //MainGridView.DataSource = gridviewdt;
   //             MainGridView.DataBind();
   //             IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
   //.Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");
   //             foreach (var row in rows)
   //                 row.Font.Bold = true;

   //             //gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

   //             //DataTable newdt = new DataTable();
   //             //newdt.Columns.Add("columnName", typeof(string));
   //             //newdt.Columns.Add("hours", typeof(string));
   //             //for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
   //             //{
   //             //    if (gridviewdt.Columns[3 + i].ColumnName != "TotalA" && gridviewdt.Columns[3 + i].ColumnName != "TotalB" && gridviewdt.Columns[3 + i].ColumnName != "TotalC" && gridviewdt.Columns[3 + i].ColumnName != "DayTotal" && gridviewdt.Columns[3 + i].ColumnName != "tireType")
   //             //    {
   //             //        DataRow drow = newdt.NewRow();
   //             //        drow[0] = gridviewdt.Columns[3 + i].ColumnName;
   //             //        drow[1] = gridviewdt.Rows[0][3 + i];
   //             //        newdt.Rows.Add(drow);
   //             //    }
   //             //}

   //             ViewState["dt"] = gridviewdt;
   //         }
   //         catch (Exception exp)
   //         {
   //             myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
   //         }
   //     }

        protected void showReportMonthRecipeWise(int getMonth, int getYear)
        {
            try
            {

                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();
                int daysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                gridviewdt.Columns.Add("MACHINENAME", typeof(string));
                gridviewdt.Columns.Add("TIRETYPE", typeof(string));
                gridviewdt.Columns.Add("PRODUCT NAME", typeof(string));
                gridviewdt.Columns.Add("SAPCODE", typeof(string));
                //Generate datatable columns dynamically
                for (int i = 1; i <= daysinMonth; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                string toDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 07:00:00";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (ddldecision.SelectedValue == "ALL")
                {
                    myConnection.comm.CommandText = @" select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='')  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!=''   order by wcname asc";
                }
                else if (ddldecision.SelectedValue == "333")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=1 or TUOGrade=2   order by wcname asc";
                }
                else if (ddldecision.SelectedValue == "444")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=4 order by wcname asc";
                }
                else
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and  status=" + ddldecision.SelectedValue + "  order by wcname asc";
                }
                
                
                
                
                //@"select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]    where " + durationQuery + " and curingRecipeName!=''  order by wcname asc ";
                    //myConnection.comm.CommandText = @" select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]   where dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + " order by wcname asc "; }

                
//                myConnection.comm.CommandText = @" SELECT wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime
//FROM( SELECT * FROM (  SELECT wcname,gtbarcode ,curingRecipeName,SAPMaterialCode,description,dtandTime,ROW_NUMBER() OVER(PARTITION BY gtbarcode ORDER BY curingRecipeName asc) rn FROM vInspectionpCR2ndview where dtandtime>='" + fromDate + " 07:00:00'  and dtandtime<'" + toDate + " 06:59:59' ) a WHERE rn = 1) DBNew ORDER BY curingRecipeName asc";
                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        day = row.Field<DateTime>("dtandTime").AddHours(-7).Day,
        recipeCode = row.Field<string>("curingRecipeName"),
        wcname = row.Field<string>("wcname"),
        description = row.Field<string>("description"),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode")
    })
    .Select(g => new
    {
        day = g.Key.day,
        recipeCode = g.Key.recipeCode,
        wcname = g.Key.wcname,
        description = g.Key.description,
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
                        dr[0] = items[i].wcname.ToString();
                        dr[1] = items[i].recipeCode.ToString();

                        if (items[i].description == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].description.ToString();
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
                        dr[0] = items[i].wcname.ToString();
                        dr[1] = items[i].recipeCode.ToString();

                        if (items[i].description == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].description.ToString();
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
                    // Wcwise Total
                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 4; v <= (daysinMonth + 4); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].wcname)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    else if (items[i].wcname.ToString() != items[i + 1].wcname.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 4; v <= (daysinMonth + 4); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].wcname)
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
                dr[0] = "Total";
                dr[1] = "";
                dr[2] = "";
                for (int v = 4; v <= (daysinMonth + 4); v++)
                {
                    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

                }
                gridviewdt.Rows.Add(dr);
                //dr[0] = "Total";
                //for (int v = 4; v <= (daysinMonth + 4); v++)
                //{
                //    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

                //}
                //gridviewdt.Rows.Add(dr);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;

                //gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

                //DataTable newdt = new DataTable();
                //newdt.Columns.Add("columnName", typeof(string));
                //newdt.Columns.Add("days", typeof(string));
                //for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
                //{
                //    if (gridviewdt.Columns[2 + i].ColumnName != "Total" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
                //    {
                //        DataRow drow = newdt.NewRow();
                //        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
                //        drow[1] = gridviewdt.Rows[0][2 + i];
                //        newdt.Rows.Add(drow);
                //    }
                //}

                ViewState["dt"] = gridviewdt;
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        #region YearWise Report
        protected void showReportYearWcWise(int getYear)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("MACHINENAME", typeof(string));
                gridviewdt.Columns.Add("TIRETYPE", typeof(string));
                gridviewdt.Columns.Add("PRODUCT NAME", typeof(string));
                gridviewdt.Columns.Add("SAPCODE", typeof(string));


                //Generate datatable columns dynamically
                for (int i = 1; i <= 12; i++)
                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
                gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

                string fromDate = getYear.ToString() + "-01-01 07:00:00";
                string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                //if (ddldecision.SelectedValue!="ALL")
                //{
                //    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + "  order by wcname asc";
                //}
                //else
                //{
                //    myConnection.comm.CommandText = @"select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='')  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!=''  order by wcname asc";
                //}

                if (ddldecision.SelectedValue == "ALL")
                {
                    myConnection.comm.CommandText = @" select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='')  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!=''   order by wcname asc";
                }
                else if (ddldecision.SelectedValue == "333")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=1 or TUOGrade=2   order by wcname asc";
                }
                else if (ddldecision.SelectedValue == "444")
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=1 and TUOGrade=4 order by wcname asc";
                }
                else
                {
                    myConnection.comm.CommandText = "select distinct  wcname,gtbarcode,curingRecipeName,SAPMaterialCode,description,dtandTime from [vInspectionpCR2ndview]  t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vInspectionpCR2ndview t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and status=" + ddldecision.SelectedValue + ")  and dtandtime>='" + fromDate + "' and dtandtime<'" + toDate + "' and curingRecipeName!='' and  status=" + ddldecision.SelectedValue + "  order by wcname asc";
                }



                
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                var query = dt.AsEnumerable()
    .GroupBy(row => new
    {
        month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
        recipeCode = row.Field<string>("curingRecipeName"),
        description = row.Field<string>("description"),
        SAPMaterialCode = row.Field<string>("SAPMaterialCode"),
        wcName = row.Field<string>("wcName")
    })
    .Select(g => new
    {
        WcName = g.Key.wcName,
        month = g.Key.month,
        recipeCode = g.Key.recipeCode,
        description = g.Key.description,
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
                        if (items[i].description == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].description.ToString();


                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the days places by default
                        for (int h = 4; h <= (13 + 2); h++)
                            dr[h] = 0;
                    }
                    else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i - 1].WcName.ToString())) //If Recipe or Workcenter changes, then create new data row
                    {
                        dr = gridviewdt.NewRow();
                        dr[0] = items[i].WcName.ToString();
                        dr[1] = items[i].recipeCode.ToString();
                        if (items[i].description == null)
                            dr[2] = "";
                        else
                            dr[2] = items[i].description.ToString();

                        if (items[i].SAPMaterialCode == null)
                            dr[3] = "";
                        else
                            dr[3] = items[i].SAPMaterialCode.ToString();

                        // Insert rows in all the hours places by default
                        for (int h = 4; h <= (13 + 2); h++)
                            dr[h] = 0;
                    }

                    dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 4; v <= (13 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[13 + 3] = total;

                        gridviewdt.Rows.Add(dr);
                    }
                    else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
                    {
                        total = 0;
                        // Display Whole Month Total
                        for (int v = 4; v <= (13 + 2); v++)
                            total += Convert.ToInt32(dr[v]);

                        dr[13 + 3] = total;

                        gridviewdt.Rows.Add(dr);
                    }

                    // Wcwise Total

                    // Wcwise Total
                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
                    {
                        DataRow ndr = gridviewdt.NewRow();
                        ndr[0] = "";
                        ndr[1] = "Total";
                        for (int v = 4; v <= (12 + 4); v++)
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
                        for (int v = 4; v <= (12 + 4); v++)
                        {
                            ndr[v] = gridviewdt.AsEnumerable()
            .Where(r => r.Field<string>(0) == items[i].WcName)
            .Sum(r => r.Field<int>(v));

                        }
                        gridviewdt.Rows.Add(ndr);
                    }
                    //End WCwise Total
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

                dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);
                dr = gridviewdt.NewRow();
                dr[0] = "Total";
                dr[1] = "";
                for (int v = 4; v <= (12 + 4); v++)
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
                ViewState["dt"] = gridviewdt;
                
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

        protected void displayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            try
            {
                string getMonth = DropDownListMonth.SelectedValue;
                string getYear = DropDownListYear.SelectedItem.Text;
                string getYearwise = DropDownListYearWise.SelectedItem.Text;

                var datetimebt = "";
                string getfromdate = reportMasterFromDateTextBox.Text; ;

                switch (DropDownListDuration.SelectedItem.Value)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);
                        string nfromDate = fromDate.ToString("MM-dd-yyyy");
                        string ntoDate = toDate.ToString("MM-dd-yyyy");
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
                    case "Year":
                        showReportYearWcWise(Convert.ToInt32(getYearwise));
                        break;

                }


            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }


        protected void Export_click(object sender, EventArgs e)
        {

            if (ViewState["dt"] != null)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=PCR2LINE.xls");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PCR2ndLINEReport");
                ws.Cells["A1"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Medium2);
                ws.Cells.AutoFitColumns();
                var ms = new MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
}
