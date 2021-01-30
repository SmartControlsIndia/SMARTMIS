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

namespace SmartMIS
{
    public partial class QualityReport : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        int totalRej,flag, cntTBRShiftA, cntTBRShiftB, cntTBRShiftC,cntPCRShiftA, cntPCRShiftB, cntPCRShiftC, actualTBRA, actualTBRB, actualTBRC, actualPCRA, actualPCRB, actualPCRC;
        string reportType, queryString, reportChoice, filteredDate, toDate, toMonth, toYear, Year, query, month, day, year;
        DateTime shftAStTime, shftBStTime, shftCStTime, shftAEndTime, shftBEndTime, shftCEndTime;
        DataTable dateWiseDT = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            DateWiseReportDiv.Visible = false;
            MonthlyReportDiv.Visible = false;
            YearlyReportDiv.Visible = false;
            
           //qualityUC.Visible = false;
            createDataCol();
            getShiftTime();
        }

        private void getShiftTime()
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select * from shiftMaster";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            shftAStTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["startTime"]);
            shftAEndTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["endTime"]);
            shftBStTime = Convert.ToDateTime(ds.Tables[0].Rows[1]["startTime"]);
            shftBEndTime = Convert.ToDateTime(ds.Tables[0].Rows[1]["endTime"]);
            shftCStTime = Convert.ToDateTime(ds.Tables[0].Rows[2]["startTime"]);
            shftCEndTime = Convert.ToDateTime(ds.Tables[0].Rows[2]["endTime"]);

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }    
      
        private void fillGridView()
        {
            DateWiseReportDiv.Visible = true;
            getTotalRejectedTBR();
            getTotalRejectedPCR();
            getActual();
            populateTBRDataTable();
            populatePCRDataTable();       
            flag = 1;
            populateTBRChartData();
            populatePCRChartData();
            productionReportDateLabel.Text = filteredDate.ToString();
        }

        private void populateTBRChartData()
        {
            string[] xValues = { "ShiftA", "ShiftB", "ShiftC", "All" };
            int[] goodVal = { actualTBRA - cntTBRShiftA, actualTBRB - cntTBRShiftB, actualTBRC - cntTBRShiftC, (actualTBRA - cntTBRShiftA)+ (actualTBRB - cntTBRShiftB)+ (actualTBRC - cntTBRShiftC)};
            int[] rejectedVal = { cntTBRShiftA, cntTBRShiftB, cntTBRShiftC, (cntTBRShiftA+ cntTBRShiftB+ cntTBRShiftC) };
            TBRChart.Visible = true;
            createTBRChart(xValues, goodVal, rejectedVal);
        }

        private void populatePCRChartData()
        {
            string[] xValues = { "ShiftA", "ShiftB", "ShiftC", "All" };
            int[] goodVal = { actualPCRA - cntPCRShiftA, actualPCRB - cntPCRShiftB, actualPCRC - cntPCRShiftC, (actualPCRA - cntPCRShiftA) + (actualPCRB - cntPCRShiftB) + (actualPCRC - cntPCRShiftC) };
            int[] rejectedVal = { cntPCRShiftA, cntPCRShiftB, cntPCRShiftC, (cntPCRShiftA + cntPCRShiftB + cntPCRShiftC) };
            PCRChart.Visible = true;
            createPCRChart(xValues, goodVal, rejectedVal);
        }

        private void getTotalRejectedTBR()
        {
            cntTBRShiftA = 0;
            cntTBRShiftB = 0;
            cntTBRShiftC = 0;
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select  id, dtandTime, count(*) as totalRej from tyrexray where status=2 and dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "'  group by id , dtandTime union  all select id,dtandTime, count(*) as totalRej from unibalrunoutTBR where status=2 and dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "' group by id, dtandTime union all select id, dtandTime, count(*) as totalRej from vinspectiontbr where status=2 and dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "'  group by id, dtandTime";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            totalRej = ds.Tables[0].Rows.Count;

            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftAStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftAEndTime.TimeOfDay))
                {
                    cntTBRShiftA++;
                }

                else if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftBStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftBEndTime.TimeOfDay))
                {
                    cntTBRShiftB++;
                }

                else if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftCStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftCEndTime.TimeOfDay))
                {
                    cntTBRShiftC++;
                }
            }

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        private void getTotalRejectedPCR()
        {
            cntPCRShiftA = 0;
            cntPCRShiftB = 0;
            cntPCRShiftC = 0;
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select id,dtandTime, count(*) as totalRej from unibalrunoutPCR where status=2 and dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "' group by id, dtandTime union all select id, dtandTime, count(*) as totalRej from vinspectionpcr where status=2 and dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "'  group by id, dtandTime";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftAStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftAEndTime.TimeOfDay))
                {
                    cntPCRShiftA++;
                }

                else if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftBStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftBEndTime.TimeOfDay))
                {
                    cntPCRShiftB++;
                }

                else if ((Convert.ToDateTime(drow[1]).TimeOfDay > shftCStTime.TimeOfDay) && (Convert.ToDateTime(drow[1]).TimeOfDay < shftCEndTime.TimeOfDay))
                {
                    cntPCRShiftC++;
                }
            }

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        private int getActual()
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select substring(processName,0,4) as processName,dtandTime, quantity, shift from vproductionActual1 where dtandtime between '" + filteredDate + "' and '" + (Convert.ToDateTime(filteredDate).AddDays(1)) + "' and shift in ('A','B','C') ";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if (drow["processName"].Equals("TBR"))
                {
                    if (drow["shift"].Equals("A"))
                    { actualTBRA = Convert.ToInt32(drow["quantity"]); }

                    else if (drow["shift"].Equals("B"))
                    { actualTBRB = Convert.ToInt32(drow["quantity"]); }

                    else if (drow["shift"].Equals("C"))
                    { actualTBRC = Convert.ToInt32(drow["quantity"]); }
                }

                else if (drow["processName"].Equals("PCR"))
                {
                    if (drow["shift"].Equals("A"))
                    { actualPCRA = Convert.ToInt32(drow["quantity"]); }

                    else if (drow["shift"].Equals("B"))
                    { actualPCRB = Convert.ToInt32(drow["quantity"]); }

                    else if (drow["shift"].Equals("C"))
                    { actualPCRC = Convert.ToInt32(drow["quantity"]); }
                }
            }

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return totalRej;
        }

        private void createDataCol()
        {
            dateWiseDT = new DataTable();
            dateWiseDT.Columns.Add("processName");
            dateWiseDT.Columns.Add("GoodA");
            dateWiseDT.Columns.Add("RejectedA");
            dateWiseDT.Columns.Add("GoodB");
            dateWiseDT.Columns.Add("RejectedB");
            dateWiseDT.Columns.Add("GoodC");
            dateWiseDT.Columns.Add("RejectedC");
            dateWiseDT.Columns.Add("GoodAll");
            dateWiseDT.Columns.Add("RejectedAll");
        }

        private void populateTBRDataTable()
        { 
            DataRow row = dateWiseDT.NewRow();
            row["processName"] = "TBR";
            row["RejectedA"] = cntTBRShiftA;
            row["RejectedB"] = cntTBRShiftB;
            row["RejectedC"] = cntTBRShiftC;

            row["GoodA"] = actualTBRA - cntTBRShiftA;
            row["GoodB"] = actualTBRB - cntTBRShiftB;
            row["GoodC"] = actualTBRC - cntTBRShiftC;

            row["GoodAll"] = Convert.ToInt32(row["GoodA"]) + Convert.ToInt32(row["GoodB"]) + Convert.ToInt32(row["GoodC"]);
            row["RejectedAll"] = Convert.ToInt32(row["RejectedA"]) + Convert.ToInt32(row["RejectedB"]) + Convert.ToInt32(row["RejectedC"]);

            dateWiseDT.Rows.Add(row);

            dtWiseQualityGridView.DataSource = dateWiseDT;
            dtWiseQualityGridView.DataBind();
        }

        private void populatePCRDataTable()
        {
            DataRow row = dateWiseDT.NewRow();
            row["processName"] = "PCR";
            row["RejectedA"] = cntPCRShiftA;
            row["RejectedB"] = cntPCRShiftB;
            row["RejectedC"] = cntPCRShiftC;

            row["GoodA"] = actualPCRA - cntPCRShiftA;
            row["GoodB"] = actualPCRB - cntPCRShiftB;
            row["GoodC"] = actualPCRC - cntPCRShiftC;

            row["GoodAll"] = Convert.ToInt32(row["GoodA"]) + Convert.ToInt32(row["GoodB"]) + Convert.ToInt32(row["GoodC"]);
            row["RejectedAll"] = Convert.ToInt32(row["RejectedA"]) + Convert.ToInt32(row["RejectedB"]) + Convert.ToInt32(row["RejectedC"]);

            dateWiseDT.Rows.Add(row);

            dtWiseQualityGridView.DataSource = dateWiseDT;
            dtWiseQualityGridView.DataBind();
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            string[] tempString = queryString.Split(new char[] { '#' });
            reportType = tempString[0].Substring(0, 1);
           
            if (reportType.Equals("0"))
            {
                //qualityUC.Visible = false;
                string[] tempString2 = queryString.Split(new char[] { '?' });
                reportChoice = tempString2[2].ToString();
                if (reportChoice.Equals("0"))
                {
                    filteredDate = formatDate(tempString2[3].ToString());
                    fillGridView();
                }
                toMonth = tempString2[5].ToString();
                toYear = tempString2[6].ToString();
                Year = tempString2[7].ToString();                
            }

            else if (reportType.Equals("1"))
            {
                qualityUC.Visible = true;
                string[] tempString2 = queryString.Split(new char[] { '?' });
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

        private void createTBRChart(object[] XValues, int[] YSeries1Values, int[] YSeries2Values)
        {
            TBRChart.Series.Add("Good");
            TBRChart.Series.Add("Rejected");           

            TBRChart.Titles.Add("Title");

            for (int i = 0; i < TBRChart.Series.Count; i++)
            {
                if (flag == 2) // Flag 1: DateWise, 2: Monthly, 3:Yearly
                {
                    TBRChart.Series[i]["DrawingStyle"] = "Default";
                    TBRChart.Series[i].IsValueShownAsLabel = false;
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["PointWidth"] = "0.6";
                    //TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }

                else if (flag == 1)
                {
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    TBRChart.Series[i]["PointWidth"] = "0.4";
                    //TBRChart.Series[i].ToolTip = ":  \t = #VALY";
                    TBRChart.Series[i].IsValueShownAsLabel = true;
                }

                else if (flag == 3)
                {
                    TBRChart.Series[i].IsValueShownAsLabel = false;
                    TBRChart.Series[i].ChartType = SeriesChartType.Column;
                    TBRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    TBRChart.Series[i]["PointWidth"] = "0.6";
                    //TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }
            }
            TBRChart.ChartAreas[0].AxisY.Title = "Production";
            TBRChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
            TBRChart.Titles[0].Text = "TBR Quality Rejection Trend (Good/Rejected)";
            TBRChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
            TBRChart.Legends[0].Title = "Legend(Good/Rejected)";
            TBRChart.Series[0].Points.DataBindXY(XValues, YSeries1Values);
            TBRChart.Series[1].Points.DataBindXY(XValues, YSeries2Values);
        }

        private void createPCRChart(object[] XValues, int[] YSeries1Values, int[] YSeries2Values)
        {
            PCRChart.Series.Add("Good");
            PCRChart.Series.Add("Rejected");

            PCRChart.Titles.Add("Title");

            for (int i = 0; i < TBRChart.Series.Count; i++)
            {
                if (flag == 2) // Flag 1: DateWise, 2: Monthly, 3:Yearly
                {
                    PCRChart.Series[i]["DrawingStyle"] = "Default";
                    PCRChart.Series[i].IsValueShownAsLabel = false;
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["PointWidth"] = "0.6";
                    //TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }

                else if (flag == 1)
                {
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    PCRChart.Series[i]["PointWidth"] = "0.4";
                    //TBRChart.Series[i].ToolTip = ":  \t = #VALY";
                    PCRChart.Series[i].IsValueShownAsLabel = true;
                }

                else if (flag == 3)
                {
                    PCRChart.Series[i].IsValueShownAsLabel = false;
                    PCRChart.Series[i].ChartType = SeriesChartType.Column;
                    PCRChart.Series[i]["DrawingStyle"] = "Cylinder";
                    PCRChart.Series[i]["PointWidth"] = "0.6";
                    //TBRChart.Series[i].ToolTip = "Production:  \t = #VALY";
                }
            }
            PCRChart.ChartAreas[0].AxisY.Title = "Production";
            PCRChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
            PCRChart.Titles[0].Text = "PCR Quality Rejection Trend (Good/Rejected)";
            PCRChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
            PCRChart.Legends[0].Title = "Legend(Good/Rejected)";
            PCRChart.Series[0].Points.DataBindXY(XValues, YSeries1Values);
            PCRChart.Series[1].Points.DataBindXY(XValues, YSeries2Values);
        }
    }
}

