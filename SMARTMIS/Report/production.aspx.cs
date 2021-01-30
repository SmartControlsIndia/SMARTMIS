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

namespace SmartMIS
{
    public partial class productionReport : System.Web.UI.Page
    {
        #region Global Variables
        myConnection myConnection = new myConnection();
        int flag = 0;
        string[] xvalues;
        string queryString = null;
        
        smartMISWebService myWebService = new smartMISWebService();
        
        double[] TBRSeriesPlanned, TBRSeriesActual, TBRSeriesDiff, PCRSeriesPlan, PCRSeriesActual, PCRSeriesDiff;
        string reportType, reportChoice, filteredDate, toDate, toMonth, toYear, Year, query, month, day, year; 
    
        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            reportHeader.Visible = false;
            
            productionReportWCDateWise.Visiblity = "None";
            PlantWideDiv.Visible = false;
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            try
            {
                queryString = magicHidden.Value;
                string[] tempString = queryString.Split(new char[] { '#' });
                reportType = tempString[0].Substring(0, 1);

                if (reportType.Equals("0"))
                {

                    string[] tempString2 = queryString.Split(new char[] { '?' });
                    reportChoice = tempString2[2].ToString();
                    if (reportChoice.Equals("0"))
                    {
                        filteredDate = formatDate(tempString2[3].ToString());
                    }
                    toMonth = tempString2[5].ToString();
                    toYear = tempString2[6].ToString();
                    Year = tempString2[7].ToString();
                    fillGridView();
                    PlantWideDiv.Visible = true;
                }

                else if (reportType.Equals("1"))
                {
                    string[] tempString2 = queryString.Split(new char[] { '?' });
                    reportHeader.Visible = true;
                    reportHeader.ReportDate = tempString2[3].ToString();
                    productionReportWCDateWise.Visiblity = "Block";
                    PlantWideDiv.Visible = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        #endregion

        #region User Defined Function

        private void fillGridView()
        {          
            // Calling PlantWide Selected Date Records
            if (reportChoice.Equals("0") && reportType.Equals("0")) 
            {                
                MonthlyReportDiv.Visible = false;
                YearlyReportDiv.Visible = false;
                productionReportDateLabel.Text = day + "-" + myWebService.monthName[Convert.ToInt32(month) - 1] + "-" + year;
                dtWiseProductionGridView.DataSource = myWebService.fillGridView("SELECT substring(processName,0,4) as processName, SUM(CASE WHEN Shift = 'A' THEN PlannedQty ELSE 0 END) AS PlanA, SUM(CASE WHEN Shift = 'A' THEN ActualQty ELSE 0 END) AS ActualA, SUM(CASE WHEN Shift = 'A' THEN Difference ELSE 0 END) AS DifferenceA, SUM(CASE WHEN Shift = 'B' THEN PlannedQty ELSE 0 END) AS PlanB,SUM(CASE WHEN Shift = 'B' THEN actualQty ELSE 0 END) AS ActualB,SUM(CASE WHEN Shift = 'B' THEN Difference ELSE 0 END) AS DifferenceB,SUM(CASE WHEN Shift = 'C' THEN plannedQty ELSE 0 END) AS PlanC, SUM(CASE WHEN Shift = 'C' THEN actualQty ELSE 0 END) AS ActualC,SUM(CASE WHEN Shift = 'C' THEN Difference ELSE 0 END) AS DifferenceC,SUM(CASE WHEN Shift in ('A','B','C') THEN plannedQty ELSE 0 END) AS PlanALL,SUM(CASE WHEN Shift in ('A','B','C') THEN actualQty ELSE 0 END) AS ActualALL,SUM(CASE WHEN Shift in ('A','B','C') THEN Difference ELSE 0 END) AS DifferenceALL FROM vProduction2 where processID in (32,39) and planDtandTime >= '" + filteredDate + "' and planDtandTime < '" + Convert.ToDateTime(filteredDate).AddDays(1) + "' and actualDtandTime >= '" + filteredDate + "' and actualDtandTime < '" + Convert.ToDateTime(filteredDate).AddDays(1) + "' GROUP BY processID, processName;", ConnectionOption.SQL);
                dtWiseProductionGridView.DataBind();
                if ((dtWiseProductionGridView.Rows.Count != 0))
                {
                    reportHeader.Visible = true;
                    productionReportWCDateWise.Visiblity = "None";
                    DateWiseReportDiv.Visible = true;
                    flag = 1;
                    bindDateWiseChartData();
                }
                else {
                    DateWiseReportDiv.Visible = false;
                    PlantWideDiv.Visible = false;
                }
            }
            //Calling PlantWide Selected Month Records
            else if (reportChoice.Equals("1") && reportType.Equals("0")) 
            {
                if (toMonth == null || toMonth == "0") { DateWiseReportDiv.Visible = false; }// Checking if Month is not selected by User
                else
                {
                    MonthlyReportDiv.Visible = true;
                    DateWiseReportDiv.Visible = false;
                    YearlyReportDiv.Visible = false;
                    productionReportMonthLabel.Text = myWebService.monthName[Convert.ToInt32(toMonth) - 1] + " " + toYear;
                    monthWiseProdGridView.DataSource = MonthWise(toMonth, toYear);
                    monthWiseProdGridView.DataBind();
                }
            }
            //Calling PlantWide Selected Year Records
            else if (reportChoice.Equals("2") && reportType.Equals("0"))
            {
                YearlyReportDiv.Visible = true;
                DateWiseReportDiv.Visible = false;
                MonthlyReportDiv.Visible = false;
                productionReportYearLabel.Text = Year;
                yearWiseProdGridView.DataSource = YearWise();
                yearWiseProdGridView.DataBind();
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

        private DataTable MonthWise(string toMonth, string toYear)
        {
            TBRSeriesPlanned = new double[31];
            TBRSeriesActual = new double[31];
            TBRSeriesDiff = new double[31];

            PCRSeriesPlan = new double[31];
            PCRSeriesActual = new double[31];
            PCRSeriesDiff = new double[31];

            int flg = 0;
            DataTable mytbl = new DataTable();
            mytbl.Columns.Add("Date");
            mytbl.Columns.Add("PlannedTBR");
            mytbl.Columns.Add("ActualTBR");
            mytbl.Columns.Add("DifferenceTBR");
            mytbl.Columns.Add("PlannedPCR");
            mytbl.Columns.Add("ActualPCR");
            mytbl.Columns.Add("DifferencePCR");

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            for (int i = 1; i <= 31; i++)
            {
                string query = "select  processName,  dtandTime, sum(planningQuantity)  as Planned , sum(actualProdQuantity) as Actual, (sum(planningQuantity)-sum(actualProdQuantity)) as Difference from vProductionReport where Month(dtandTime)='" + toMonth + "' and Year(dtandTime)='" + toYear + "'  and Day(dtandTime)='" + i + "' group by  dtandTime, processName";
                myConnection.comm.CommandText = query;
                myConnection.reader = myConnection.comm.ExecuteReader();

                DataRow row = mytbl.NewRow();
                row["Date"] = i.ToString();
                int count = 0;

                if (myConnection.reader.HasRows == true)
                {
                    flg = 1;  //If Any Data Found
                    while (myConnection.reader.Read())
                    {
                        count++;
                    }
                    if (count == 1)
                    {
                        myConnection.reader.Close();
                        myConnection.comm.CommandText = query;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myConnection.reader.Read();

                        if (myConnection.reader[0].ToString().Contains("TBR") == true)
                        {
                            if (myConnection.reader[2].ToString() == "") row["PlannedTBR"] = "-"; else row["PlannedTBR"] = myConnection.reader[2].ToString();
                            if (myConnection.reader[3].ToString() == "") row["ActualTBR"] = "-"; else row["ActualTBR"] = myConnection.reader[3].ToString();
                            if (myConnection.reader[4].ToString() == "") row["DifferenceTBR"] = "-"; else row["DifferenceTBR"] = myConnection.reader[4].ToString();

                            row["PlannedPCR"] = "-";
                            row["ActualPCR"] = "-";
                            row["DifferencePCR"] = "-";
                            mytbl.Rows.Add(row);
                        }
                        else
                        {
                            if (myConnection.reader[2].ToString() == "") row["PlannedPCR"] = "-"; else row["PlannedPCR"] = myConnection.reader[2].ToString();
                            if (myConnection.reader[3].ToString() == "") row["ActualPCR"] = "-"; else row["ActualPCR"] = myConnection.reader[3].ToString();
                            if (myConnection.reader[4].ToString() == "") row["DifferencePCR"] = "-"; else row["DifferencePCR"] = myConnection.reader[4].ToString();

                            row["PlannedTBR"] = "-";
                            row["ActualTBR"] = "-";
                            row["DifferenceTBR"] = "-";
                            mytbl.Rows.Add(row);
                        }
                    }
                    else
                    {
                        myConnection.reader.Close();
                        myConnection.comm.CommandText = query;
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        myConnection.reader.Read();

                        if (myConnection.reader[2].ToString() == "") row["PlannedTBR"] = "-"; else row["PlannedTBR"] = myConnection.reader[2].ToString();
                        if (myConnection.reader[3].ToString() == "") row["ActualTBR"] = "-"; else row["ActualTBR"] = myConnection.reader[3].ToString();
                        if (myConnection.reader[4].ToString() == "") row["DifferenceTBR"] = "-"; else row["DifferenceTBR"] = myConnection.reader[4].ToString();

                        myConnection.reader.Read();

                        if (myConnection.reader[2].ToString() == "") row["PlannedPCR"] = "-"; else row["PlannedPCR"] = myConnection.reader[2].ToString();
                        if (myConnection.reader[3].ToString() == "") row["ActualPCR"] = "-"; else row["ActualPCR"] = myConnection.reader[3].ToString();
                        if (myConnection.reader[4].ToString() == "") row["DifferencePCR"] = "-"; else row["DifferencePCR"] = myConnection.reader[4].ToString();

                        mytbl.Rows.Add(row);
                    }
                }
                else
                {

                    row["PlannedTBR"] = "-";
                    row["ActualTBR"] = "-";
                    row["DifferenceTBR"] = "-";
                    row["PlannedPCR"] = "-";
                    row["ActualPCR"] = "-";
                    row["DifferencePCR"] = "-";
                    mytbl.Rows.Add(row);

                }

                if (row["PlannedTBR"].ToString() == "-") TBRSeriesPlanned[i - 1] = 0; else TBRSeriesPlanned[i - 1] = Convert.ToDouble(row["PlannedTBR"]);
                if (row["ActualTBR"].ToString() == "-") TBRSeriesActual[i - 1] = 0; else TBRSeriesActual[i - 1] = Convert.ToDouble(row["ActualTBR"]);
                if (row["DifferenceTBR"].ToString() == "-") TBRSeriesDiff[i - 1] = 0; else TBRSeriesDiff[i - 1] = Convert.ToDouble(row["DifferenceTBR"]);
                if (row["PlannedPCR"].ToString() == "-") PCRSeriesPlan[i - 1] = 0; else PCRSeriesPlan[i - 1] = Convert.ToDouble(row["PlannedPCR"]);
                if (row["ActualPCR"].ToString() == "-") PCRSeriesActual[i - 1] = 0; else PCRSeriesActual[i - 1] = Convert.ToDouble(row["ActualPCR"]);
                if (row["DifferencePCR"].ToString() == "-") PCRSeriesDiff[i - 1] = 0; else PCRSeriesDiff[i - 1] = Convert.ToDouble(row["DifferencePCR"]);


                myConnection.reader.Close();
                //flag = 0;

            }
            xvalues = xvaluesFunc("M");

            if (flg == 1)
            {
                flag = 2;
                createTBRChart(xvalues, TBRSeriesPlanned, TBRSeriesActual, TBRSeriesDiff);
                createPCRChart(xvalues, PCRSeriesPlan, PCRSeriesActual, PCRSeriesDiff);
                PlantWideDiv.Visible = true;
                TBRChart.Visible = true;
                PCRChart.Visible = true;
                MonthlyReportDiv.Visible = true;
               
                reportHeader.Visible = true;
            }
            else
            {
                MonthlyReportDiv.Visible = false;
                TBRChart.Visible = false;
                PCRChart.Visible = false;
                productionReportWCDateWise.Visiblity = "None";
                reportHeader.Visible = false;
            }
            return mytbl;

        }

        private DataTable YearWise()
        { 
            
            TBRSeriesPlanned = new double[12];
            TBRSeriesActual = new double[12];
            TBRSeriesDiff = new double[12];

            PCRSeriesPlan = new double[12];
            PCRSeriesActual = new double[12];
            PCRSeriesDiff = new double[12];
            int flg = 0;

            DataTable mytbl = new DataTable();
            mytbl.Columns.Add("Month");
            mytbl.Columns.Add("PlannedTBR");
            mytbl.Columns.Add("ActualTBR");
            mytbl.Columns.Add("DifferenceTBR");

            mytbl.Columns.Add("PlannedPCR");
            mytbl.Columns.Add("ActualPCR");
            mytbl.Columns.Add("DifferencePCR");

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            for (int i = 1; i <= 12; i++)
            {
                string query = "select  processName,Month(dtandTime) as Month,  sum(planningQuantity)  as Planned , sum(actualProdQuantity) as Actual, (sum(planningQuantity)-sum(actualProdQuantity)) as Difference from vProductionReport where Month(dtandTime)='" + i + "' and year(dtandTime)='" + Year + "' group by  processName,Month(dtandTime)";
                myConnection.comm.CommandText = query;
                myConnection.reader = myConnection.comm.ExecuteReader();
                DataRow row = mytbl.NewRow();

                row["Month"] = myWebService.monthName[i - 1].ToString(); ;
                int count = 0;

                if (myConnection.reader.HasRows == true)
                {
                    flg = 1;//If Data Found
                    while (myConnection.reader.Read())
                    {
                        count++;
                    }

                    if (count == 1)
                    {
                        myConnection.reader.Close();
                        myConnection.comm.CommandText = query;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myConnection.reader.Read();

                        if (myConnection.reader[0].ToString().Contains("TBR") == true)
                        {
                            if (myConnection.reader[2].ToString() == "") row["PlannedTBR"] = "-"; else row["PlannedTBR"] = myConnection.reader[2].ToString();
                            if (myConnection.reader[3].ToString() == "") row["ActualTBR"] = "-"; else row["ActualTBR"] = myConnection.reader[3].ToString();
                            if (myConnection.reader[4].ToString() == "") row["DifferenceTBR"] = "-"; else row["DifferenceTBR"] = myConnection.reader[4].ToString();

                            row["PlannedPCR"] = "-";
                            row["ActualPCR"] = "-";
                            row["DifferencePCR"] = "-";
                            mytbl.Rows.Add(row);
                        }
                        else
                        {
                            if (myConnection.reader[2].ToString() == "") row["PlannedPCR"] = "-"; else row["PlannedPCR"] = myConnection.reader[2].ToString();
                            if (myConnection.reader[3].ToString() == "") row["ActualPCR"] = "-"; else row["ActualPCR"] = myConnection.reader[3].ToString();
                            if (myConnection.reader[4].ToString() == "") row["DifferencePCR"] = "-"; else row["DifferencePCR"] = myConnection.reader[4].ToString();

                            row["PlannedTBR"] = "-";
                            row["ActualTBR"] = "-";
                            row["DifferenceTBR"] = "-";
                            mytbl.Rows.Add(row);
                        }
                    }
                    else
                    {
                        myConnection.reader.Close();
                        myConnection.comm.CommandText = query;
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        myConnection.reader.Read();

                        if (myConnection.reader[2].ToString() == "") row["PlannedTBR"] = "-"; else row["PlannedTBR"] = myConnection.reader[2].ToString();
                        if (myConnection.reader[3].ToString() == "") row["ActualTBR"] = "-"; else row["ActualTBR"] = myConnection.reader[3].ToString();
                        if (myConnection.reader[4].ToString() == "") row["DifferenceTBR"] = "-"; else row["DifferenceTBR"] = myConnection.reader[4].ToString();

                        myConnection.reader.Read();

                        if (myConnection.reader[2].ToString() == "") row["PlannedPCR"] = "-"; else row["PlannedPCR"] = myConnection.reader[2].ToString();
                        if (myConnection.reader[3].ToString() == "") row["ActualPCR"] = "-"; else row["ActualPCR"] = myConnection.reader[3].ToString();
                        if (myConnection.reader[4].ToString() == "") row["DifferencePCR"] = "-"; else row["DifferencePCR"] = myConnection.reader[4].ToString();

                        mytbl.Rows.Add(row);
                    }
                }
                else
                {
                    row["PlannedTBR"] = "-";
                    row["ActualTBR"] = "-";
                    row["DifferenceTBR"] = "-";
                    row["PlannedPCR"] = "-";
                    row["ActualPCR"] = "-";
                    row["DifferencePCR"] = "-";
                    mytbl.Rows.Add(row);
                }


                myConnection.reader.Close();
                //flag = 0;

                if (row["PlannedTBR"].ToString() == "-") TBRSeriesPlanned[i - 1] = 0; else TBRSeriesPlanned[i - 1] = Convert.ToDouble(row["PlannedTBR"]);
                if (row["ActualTBR"].ToString() == "-") TBRSeriesActual[i - 1] = 0; else TBRSeriesActual[i - 1] = Convert.ToDouble(row["ActualTBR"]);
                if (row["DifferenceTBR"].ToString() == "-") TBRSeriesDiff[i - 1] = 0; else TBRSeriesDiff[i - 1] = Convert.ToDouble(row["DifferenceTBR"]);
                if (row["PlannedPCR"].ToString() == "-") PCRSeriesPlan[i - 1] = 0; else PCRSeriesPlan[i - 1] = Convert.ToDouble(row["PlannedPCR"]);
                if (row["ActualPCR"].ToString() == "-") PCRSeriesActual[i - 1] = 0; else PCRSeriesActual[i - 1] = Convert.ToDouble(row["ActualPCR"]);
                if (row["DifferencePCR"].ToString() == "-") PCRSeriesDiff[i - 1] = 0; else PCRSeriesDiff[i - 1] = Convert.ToDouble(row["DifferencePCR"]);
            }
            xvalues = xvaluesFunc("Y");
            if (flg == 1)
            {
                flag = 3;
                createTBRChart(xvalues, TBRSeriesPlanned, TBRSeriesActual, TBRSeriesDiff);
                createPCRChart(xvalues, PCRSeriesPlan, PCRSeriesActual, PCRSeriesDiff);
                TBRChart.Visible = true;
                PCRChart.Visible = true;
                YearlyReportDiv.Visible = true;
               
                reportHeader.Visible = true;
            }
            else
            {
                YearlyReportDiv.Visible = false;
                TBRChart.Visible = false;
                PCRChart.Visible = false;
                reportHeader.Visible = false;
                productionReportWCDateWise.Visiblity = "None";
            }
            return mytbl;
        } 


        private string[] xvaluesFunc(string choice)
        {
            string[] xvalues;
            //Monthly
            if (choice.ToString().Trim() == "M")
            {
                xvalues = new string[31];
                for (int i = 1; i <= 31; i++)
                {
                    xvalues[i - 1] = i.ToString();
                }
            }
            else
            {
                //Yearly
                xvalues = new string[12];
                for (int i = 1; i <= 12; i++)
                {
                    xvalues[i - 1] = myWebService.monthName[i - 1].ToString();
                }
            }
            return xvalues;
        }

        private void bindDateWiseChartData()
        {
            string[] xValues = { "ShiftA", "ShiftB", "ShiftC", "All" };
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT substring(processName,0,4) as processName, SUM(CASE WHEN Shift = 'A' THEN PlannedQty ELSE 0 END) AS PlanA, SUM(CASE WHEN Shift = 'A' THEN ActualQty ELSE 0 END) AS ActualA, SUM(CASE WHEN Shift = 'A' THEN Difference ELSE 0 END) AS DifferenceA, SUM(CASE WHEN Shift = 'B' THEN PlannedQty ELSE 0 END) AS PlanB,SUM(CASE WHEN Shift = 'B' THEN actualQty ELSE 0 END) AS ActualB,SUM(CASE WHEN Shift = 'B' THEN Difference ELSE 0 END) AS DifferenceB,SUM(CASE WHEN Shift = 'C' THEN plannedQty ELSE 0 END) AS PlanC, SUM(CASE WHEN Shift = 'C' THEN actualQty ELSE 0 END) AS ActualC,SUM(CASE WHEN Shift = 'C' THEN Difference ELSE 0 END) AS DifferenceC,SUM(CASE WHEN Shift in ('A','B','C') THEN plannedQty ELSE 0 END) AS PlanALL,SUM(CASE WHEN Shift in ('A','B','C') THEN actualQty ELSE 0 END) AS ActualALL,SUM(CASE WHEN Shift in ('A','B','C') THEN Difference ELSE 0 END) AS DifferenceALL FROM vProduction2 where processID in (32,39) and planDtandTime >= '" + filteredDate + "' and planDtandTime < '" + Convert.ToDateTime(filteredDate).AddDays(1) + "' and actualDtandTime >= '" + filteredDate + "' and actualDtandTime < '" + Convert.ToDateTime(filteredDate).AddDays(1) + "' GROUP BY processID, processName";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 2)
            {
                string val = (ds.Tables[0].Rows[0][0].ToString());
                if (val.Contains("TBR"))
                {
                    double[] PlanPCR = { Convert.ToDouble(ds.Tables[0].Rows[1]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanAll"]) };
                    double[] ActualPCR = { Convert.ToDouble(ds.Tables[0].Rows[1]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualAll"]) };
                    double[] DifferencePCR = { Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceAll"]) };
                    createPCRChart(xValues, PlanPCR, ActualPCR, DifferencePCR);

                    double[] PlanTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanAll"]) };
                    double[] ActualTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualAll"]) };
                    double[] DifferenceTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceAll"]) };
                    createTBRChart(xValues, PlanTBR, ActualTBR, DifferenceTBR);
                }
                else if (val.Contains("PCR"))
                {
                    double[] PlanPCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanAll"]) };
                    double[] ActualPCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualAll"]) };
                    double[] DifferencePCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceAll"]) };
                    createPCRChart(xValues, PlanPCR, ActualPCR, DifferencePCR);

                    double[] PlanTBR = { Convert.ToDouble(ds.Tables[0].Rows[1]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["PlanAll"]) };
                    double[] ActualTBR = { Convert.ToDouble(ds.Tables[0].Rows[1]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["ActualAll"]) };
                    double[] DifferenceTBR = { Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[1]["DifferenceAll"]) };
                    createTBRChart(xValues, PlanTBR, ActualTBR, DifferenceTBR);
                }
                TBRChart.Visible = true;
                PCRChart.Visible = true;

            }

            else if (ds.Tables[0].Rows.Count == 1)
            {
                string val = (ds.Tables[0].Rows[0][0].ToString());
                if (val.Contains("TBR"))
                {
                    double[] PlanTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanAll"]) };
                    double[] ActualTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualAll"]) };
                    double[] DifferenceTBR = { Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceAll"]) };
                    createTBRChart(xValues, PlanTBR, ActualTBR, DifferenceTBR);
                    TBRChart.Visible = true;
                }
                else
                {
                    double[] PlanPCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["PlanA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["PlanAll"]) };
                    double[] ActualPCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["ActualA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["ActualAll"]) };
                    double[] DifferencePCR = { Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceA"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceB"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceC"]), Convert.ToDouble(ds.Tables[0].Rows[0]["DifferenceAll"]) };
                    createPCRChart(xValues, PlanPCR, ActualPCR, DifferencePCR);
                    PCRChart.Visible = true;
                }
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

        }

        private void createTBRChart(object[] XValues, double[] YSeries1Values, double[] YSeries2Values, double[] YSeries3Values)
        { 
            TBRChart.Series.Add("Plan");
            TBRChart.Series.Add("Actual");
            TBRChart.Series.Add("Difference");
            TBRChart.Titles.Add("Title");
            TBRChart.ChartAreas[0].AxisX.Interval = 1;

            for (int i = 0; i < TBRChart.Series.Count; i++)
            {
                if (flag == 2) // Flag 1: DateWise, 2: Monthly, 3:Yearly
                {
                    TBRChart.Series[i]["DrawingStyle"] = "Default";
                    TBRChart.Series[i].IsValueShownAsLabel = false;
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["PointWidth"] = "0.6";
                    TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }

                else if (flag == 1)
                {
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    TBRChart.Series[i]["PointWidth"] = "0.6";
                    TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                    TBRChart.Series[i].IsValueShownAsLabel = true;
                }

                else if (flag == 3)
                {
                    TBRChart.Series[i].IsValueShownAsLabel = false;
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    TBRChart.Series[i]["PointWidth"] = "0.6";
                    TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }
            }

            TBRChart.ChartAreas[0].AxisY.Title = "Production";
            TBRChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
            TBRChart.Titles[0].Text = "TBR Production Trend (Plan/Actual)";
            TBRChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
            TBRChart.Legends[0].Title = "Legend(Plan/Actual)";
            TBRChart.Series[0].Points.DataBindXY(XValues, YSeries1Values);
            TBRChart.Series[1].Points.DataBindXY(XValues, YSeries2Values);
            TBRChart.Series[2].Points.DataBindXY(XValues, YSeries3Values);
        }

        private void createPCRChart(object[] XValues, double[] YSeries1Values, double[] YSeries2Values, double[] YSeries3Values)
        {
            PCRChart.Series.Add("Plan");
            PCRChart.Series.Add("Actual");
            PCRChart.Series.Add("Difference");
            PCRChart.ChartAreas[0].AxisX.Interval = 1;

            PCRChart.Titles.Add("Title");

            for (int i = 0; i < PCRChart.Series.Count; i++)
            {
                if (flag == 2) // Flag 1: DateWise, 2: Monthly, 3:Yearly
                {
                    PCRChart.Series[i]["DrawingStyle"] = "Default";
                    PCRChart.Series[i].IsValueShownAsLabel = false;
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["PointWidth"] = "0.6";
                    PCRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }

                else if (flag == 1)
                {
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    PCRChart.Series[i]["PointWidth"] = "0.6";
                    PCRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                    PCRChart.Series[i].IsValueShownAsLabel = true;
                }

                else if (flag == 3)
                {
                    PCRChart.Series[i].IsValueShownAsLabel = false;
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    PCRChart.Series[i]["PointWidth"] = "0.6";
                    PCRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }
                PCRChart.ChartAreas[0].AxisY.Title = "Production";
                PCRChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
                PCRChart.Titles[0].Text = "PCR Production Trend (Plan/Actual)";
                PCRChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
                PCRChart.Legends[0].Title = "Legend(Plan/Actual)";

                PCRChart.Series[0].Points.DataBindXY(XValues, YSeries1Values);
                PCRChart.Series[1].Points.DataBindXY(XValues, YSeries2Values);
                PCRChart.Series[2].Points.DataBindXY(XValues, YSeries3Values);
            }
        }

        #endregion
    }
   }