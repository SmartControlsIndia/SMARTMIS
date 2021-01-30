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
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Data.SqlClient;
using Microsoft.VisualBasic;


namespace SmartMIS.Report
{
    public partial class qualityAnalysis : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        #region Global Variables
        string reportType, reportChoice, filteredDate, toDate, toMonth, toYear, Year, query, month, day, year;
        string queryString = null;
        ArrayList xValDay = new ArrayList();
        DateTime shftAStTime, shftBStTime, shftCStTime, shftAEndTime, shftBEndTime, shftCEndTime;
        #endregion

        #region System Defined Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            chartDiv.Visible = false;
            splitQueryString(magicHidden.Value);
        }
        
        #endregion

        #region User Defined Function

        public void splitQueryString(string queryString)
        {
            string[] tempString = queryString.Split(new char[] { '#' });
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

        protected void magicButton_Click(object sender, EventArgs e)
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
                    genChart(Convert.ToDateTime(filteredDate));
                    chartDiv.Visible = true;
                    genLineChart(Convert.ToDateTime(filteredDate));
                   // getShiftWiseScrapData(Convert.ToDateTime(filteredDate));
                }
                toMonth = tempString2[5].ToString();
                toYear = tempString2[6].ToString();
                Year = tempString2[7].ToString();
            }

            else if (reportType.Equals("1"))
            {
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

        private double getTargetValue(string qCode)
        {
            double targetVal = 0.0;
            string query = "select value from qualityTarget where qualityCode='" + qCode + "'";
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = query;
            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                targetVal = Convert.ToDouble(myConnection.reader[0]);
            }
            return targetVal;
        }

        private void formatChart()
        {
            qualityAnalysisUniChart.Width = 800;
            qualityAnalysisUniChart.Series.Add("MonthSeries");
            qualityAnalysisUniChart.Series.Add("DaySeries");
            qualityAnalysisUniChart.Series.Add("MTDSeries");
            qualityAnalysisUniChart.Series["MonthSeries"]["PointWidth"] = "0.8";
            qualityAnalysisUniChart.Series["MonthSeries"]["DrawingStyle"] = "Default";
            qualityAnalysisUniChart.Series["MonthSeries"].XValueType = ChartValueType.Auto;
            qualityAnalysisUniChart.Series["MonthSeries"].ChartType = SeriesChartType.Column;
            qualityAnalysisUniChart.Series["DaySeries"].ChartType = SeriesChartType.Line;
            qualityAnalysisUniChart.Series["DaySeries"].Color = Color.SteelBlue;
            qualityAnalysisUniChart.Series["DaySeries"].BorderWidth = 2;
            qualityAnalysisUniChart.Series["MTDSeries"].ChartType = SeriesChartType.Column;
            qualityAnalysisUniChart.Series["MTDSeries"].IsValueShownAsLabel = true;
            //qualityAnalysisUniChart.Series["MTDSeries"].Color = Color.Chocolate;
            qualityAnalysisUniChart.Titles.Add("Title");
            qualityAnalysisUniChart.Titles[0].Text = "OE Yield(%) Uniformity Trend TBR";
            qualityAnalysisUniChart.Titles[0].Font = new System.Drawing.Font("Verdana", 12);
            qualityAnalysisUniChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            qualityAnalysisUniChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            qualityAnalysisUniChart.Series[0].IsValueShownAsLabel = true;
            qualityAnalysisUniChart.Series[1].IsValueShownAsLabel = true;
            qualityAnalysisUniChart.ChartAreas[0].AxisX.Interval = 1;
            qualityAnalysisUniChart.Series["DaySeries"].MarkerSize = 5;
            qualityAnalysisUniChart.Series["DaySeries"].MarkerStyle = MarkerStyle.Square;
        }

        private void genChart(DateTime dt)
        {
            formatChart();
            int noOfDays = DateTime.DaysInMonth(dt.Year, dt.Month);
            double mtdVal = 0.0;
            double targetVal = getTargetValue("Uniformity");
            StripLine spLine = new StripLine();
            // MonthWiseBinding
            myConnection.open(ConnectionOption.SQL);
            for (int i = 1; i <= 4; i++)
            {
                double total, scrap, diff, resPer;
                DateAndTime.DateAdd(DateInterval.Month, -i, dt);
                string mthName = DateAndTime.DateAdd(DateInterval.Month, -i, dt).ToString("MMM");
                string query = "select count(*) from vUniBalRunoutTbr where status in (0,1) and month(dtandTime)= '" + DateAndTime.DateAdd(DateInterval.Month, -i, dt).Month + "' and Year(dtandTime)='" + DateAndTime.DateAdd(DateInterval.Month, -i, dt).Year + "' and wcName like '%unifor%' union all select count(*) from vUniBalRunoutTbr where status in (2) and month(dtandTime)= '" + DateAndTime.DateAdd(DateInterval.Month, -i, dt).Month + "' and Year(dtandTime)='" + DateAndTime.DateAdd(DateInterval.Month, -i, dt).Year + "' and wcName like '%unifor%'";
          
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();

                da.Fill(ds);
                total = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                scrap = Convert.ToDouble(ds.Tables[0].Rows[1][0]);
                diff = total - scrap;
                resPer = (diff / total) * 100;
                resPer = Convert.ToDouble(String.Format("{0:0.00}", resPer));
                qualityAnalysisUniChart.Series["MonthSeries"].Points.AddXY(i, Convert.ToDouble(resPer));
                qualityAnalysisUniChart.Series["MonthSeries"].Points[Convert.ToInt32(i - 1)].AxisLabel = mthName + " " + "'" + DateAndTime.DateAdd(DateInterval.Month, -i, dt).ToString("yy");

                myConnection.comm.Dispose();               
            }
            myConnection.close(ConnectionOption.SQL);

            DateTime date = new DateTime(dt.Year, dt.Month, 1);
            int count = 0;
            myConnection.open(ConnectionOption.SQL);
            for (int i = 5; i <= (Convert.ToInt32(dt.Day) + 4); i++)
            {
                double total, scrap, diff, resPer;
                string query = "select count(*) as Value  from vUnibalrunoutTBR where dtandTime >= '" + date.ToString("MM-dd-yyyy") + "'and dtandTime < '" + date.AddDays(1).ToString("MM-dd-yyyy") + "'  and status in (0,1) and wcName like '%unifor%' union all select count(*) as Value  from vUnibalrunoutTBR where dtandTime >= '" + date.ToString("MM-dd-yyyy") + "'and dtandTime < '" + date.AddDays(1).ToString("MM-dd-yyyy") + "'  and status in (2) and wcName like '%unifor%'";               
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                total = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                scrap = Convert.ToDouble(ds.Tables[0].Rows[1][0]);
                diff = total - scrap;
                resPer = (diff / total) * 100;
                resPer = Convert.ToDouble(String.Format("{0:0.00}", resPer));

                if (!(Double.IsNaN(resPer)))
                {
                    mtdVal = mtdVal + Convert.ToDouble(resPer);
                }
                else if (Double.IsNaN(resPer))
                {
                    resPer = 0.0;
                }
                count++;

                qualityAnalysisUniChart.Series["DaySeries"].Points.AddXY(i, resPer);
                qualityAnalysisUniChart.Series["DaySeries"].Points[i - 5].AxisLabel = (i - 4).ToString();
                myConnection.comm.Dispose();             
                date = DateAndTime.DateAdd(DateInterval.Day, 1, date);
            }
            myConnection.close(ConnectionOption.SQL);

            mtdVal = (mtdVal / count);
            mtdVal = Convert.ToDouble(string.Format("{0:0.00}", mtdVal));
            qualityAnalysisUniChart.Series["MTDSeries"].Points.AddXY(dt.Day + 5, mtdVal);
            qualityAnalysisUniChart.ChartAreas[0].AxisY.Maximum = targetVal + 5;
            qualityAnalysisUniChart.Series["MTDSeries"].Points[0].AxisLabel = "MTD";

            spLine.IntervalOffset = targetVal;
            spLine.StripWidth = 0.2;
            spLine.BorderColor = Color.Red;
            spLine.BorderDashStyle = ChartDashStyle.Solid;
            spLine.BorderWidth = 1;
            spLine.BackColor = Color.Red;
            qualityAnalysisUniChart.ChartAreas[0].AxisY.StripLines.Add(spLine);
            spLine.Text = "Target:" + targetVal;
            spLine.Font = new Font("Verdana", 10);
            spLine.TextOrientation = TextOrientation.Horizontal;
            spLine.TextAlignment = StringAlignment.Center;
            spLine.TextLineAlignment = StringAlignment.Far;
        }

        private void genLineChart(DateTime dt)
        {

            double[] yValTBR = new double[31];
            double[] yValPCR = new double[31];
            for (int i = 1; i <= 31; i++)
            {
                xValDay.Add(i);
            }
            formatLineChart();

            //TBR----------------------------------------------

            DateTime date = new DateTime(dt.Year, dt.Month, 1);
            for (int i = 1; i <= 31; i++)
            {
                curedTireScrapChart.Series[0].Points.Add(i);
            }
            myConnection.open(ConnectionOption.SQL);
            for (date = new DateTime(dt.Year, dt.Month, 1); date <= dt; date = date.AddDays(1))
            {
                double actualProd = 0, inspectScrap, xrayScrap, uniBalScrap, totalScrap, valuePercent;
                string query = @"select sum(quantity) as totalProd from vproductionActual1 where dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "' and processName like ('%tbr%') union all " +
                               "select count(*) from vInspectionTBR where status=2 and dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "' union all " +
                               "select count(*) from tyrexray where status=2 and dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "' union all " +
                               "select count(*) from unibalrunouttbr where status=2 and dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "'";

           
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!(ds.Tables[0].Rows[0].IsNull("totalProd")))
                {
                    actualProd = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    inspectScrap = Convert.ToInt32(ds.Tables[0].Rows[1][0]);
                    xrayScrap = Convert.ToInt32(ds.Tables[0].Rows[2][0]);
                    uniBalScrap = Convert.ToInt32(ds.Tables[0].Rows[3][0]);
                    totalScrap = inspectScrap + xrayScrap + uniBalScrap;
                    valuePercent = (totalScrap / actualProd) * 100;
                    curedTireScrapChart.Series["TBRScrap"].Points.AddXY(Convert.ToDouble(date.Day), valuePercent);
                    yValTBR.SetValue(Convert.ToDouble(String.Format("{0:0.00}", valuePercent)), (Convert.ToInt32(date.Day) - 1));
                }
                else
                {
                    yValTBR.SetValue(0, Convert.ToInt32(date.Day) - 1);
                }
            }
            curedTireScrapChart.Series[0].Points.DataBindXY(xValDay, yValTBR);
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            // PCR------------------------------------------------------------------
            myConnection.open(ConnectionOption.SQL);
            for (date = new DateTime(dt.Year, dt.Month, 1); date <= dt; date = date.AddDays(1))
            {
                double actualProd = 0, inspectScrap, uniBalScrap, totalScrap, valuePercent;
                string query = @"select sum(quantity) as totalProd from vproductionActual1 where dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "' and processName like ('%pcr%') union all " +
                               "select count(*) from vInspectionPCR where status=2 and dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "' union all " +
                               "select count(*) from unibalrunoutpcr where status=2 and dtandTime >= '" + date + "' and dtandTime<'" + date.AddDays(1) + "'";

       
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (!(ds.Tables[0].Rows[0].IsNull("totalProd")))
                {
                    actualProd = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    inspectScrap = Convert.ToInt32(ds.Tables[0].Rows[1][0]);
                    uniBalScrap = Convert.ToInt32(ds.Tables[0].Rows[2][0]);
                    totalScrap = inspectScrap + uniBalScrap;
                    valuePercent = (totalScrap / actualProd) * 100;
                    curedTireScrapChart.Series["PCRScrap"].Points.AddXY(Convert.ToDouble(date.Day), valuePercent);
                    yValPCR.SetValue(Convert.ToDouble(String.Format("{0:0.00}", valuePercent)), (Convert.ToInt32(date.Day)) - 1);
                }
                else
                {
                    yValPCR.SetValue(0, Convert.ToInt32(date.Day) - 1);
                }
            }
            curedTireScrapChart.Series[1].Points.DataBindXY(xValDay, yValPCR);
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        private void formatLineChart()
        {
            curedTireScrapChart.Width = 800;
            curedTireScrapChart.Series.Add("TBRScrap");
            curedTireScrapChart.Series.Add("PCRScrap");
            curedTireScrapChart.Titles.Add("Title");
            curedTireScrapChart.Titles[0].Text = "Cured Tire Scrap Trend(TBR/PCR)";
            curedTireScrapChart.Titles[0].Font = new System.Drawing.Font("Verdana", 12);

            curedTireScrapChart.ChartAreas[0].AxisX.Interval = 1;
            curedTireScrapChart.ChartAreas[0].AxisY.Maximum = 20;
            curedTireScrapChart.Series[0].Color = Color.Maroon;
            curedTireScrapChart.Series[0].MarkerStyle = MarkerStyle.Square;

            curedTireScrapChart.Series[1].Color = Color.MediumSeaGreen;
            curedTireScrapChart.Series[1].MarkerStyle = MarkerStyle.Diamond;

            for (int i = 0; i < curedTireScrapChart.Series.Count; i++)
            {
                curedTireScrapChart.Series[i].ChartType = SeriesChartType.Line;
                curedTireScrapChart.Series[i].BorderWidth = 2;
                curedTireScrapChart.Series[1].IsValueShownAsLabel = true;
                curedTireScrapChart.Series[i].MarkerSize = 7;
                //curedTireScrapChart.Series[i].IsValueShownAsLabel = true;
                curedTireScrapChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            }
        }

        private void getShiftWiseScrapData(DateTime dt)
        {
            ArrayList xVals = new ArrayList();
            getShiftTime();
            for (int i = 0; i < 32; i++)
            {
                xVals.Add(i);
            }

            //TBR-----------------------------------------------

            int noOfDays = DateTime.DaysInMonth(dt.Year, dt.Month);
            int[] insTBRA = new int[32];
            int[] insTBRB = new int[32];
            int[] insTBRC = new int[32];
            int[] xrayTBRA = new int[32];
            int[] xrayTBRB = new int[32];
            int[] xrayTBRC = new int[32];
            int[] uniTBRA = new int[32];
            int[] uniTBRB = new int[32];
            int[] uniTBRC = new int[32];
            int[] finalTBRA = new int[32];
            int[] finalTBRB = new int[32];
            int[] finalTBRC = new int[32];


            //VI--------------------------------------------------------------

            for (DateTime date = new DateTime(dt.Year, dt.Month, 1); date.Day <= DateTime.DaysInMonth(dt.Year, dt.Month); date = DateAndTime.DateAdd(DateInterval.Day, 1, date))
            {
                string query = "select * from vVisualization where status= 2 and dtandTime >= '" + date + "' and dtandTime<'" + DateAndTime.DateAdd(DateInterval.Day, 1, date) + "' ";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if ((Convert.ToDateTime(drow[14]).TimeOfDay > shftAStTime.TimeOfDay) && (Convert.ToDateTime(drow[14]).TimeOfDay < shftAEndTime.TimeOfDay))
                    {
                        insTBRA.SetValue((insTBRA[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[14]).TimeOfDay > shftBStTime.TimeOfDay) && (Convert.ToDateTime(drow[14]).TimeOfDay < shftBEndTime.TimeOfDay))
                    {
                        insTBRB.SetValue((insTBRB[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[14]).TimeOfDay >= shftCStTime.TimeOfDay) || (Convert.ToDateTime(drow[14]).TimeOfDay <= shftCEndTime.TimeOfDay))
                    {
                        insTBRC.SetValue((insTBRC[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }
                }

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            for (DateTime date = new DateTime(dt.Year, dt.Month, 1); date.Day <= DateTime.DaysInMonth(dt.Year, dt.Month); date = DateAndTime.DateAdd(DateInterval.Day, 1, date))
            {
                string query = "select * from vTyreXray where status= 2 and dtandTime >= '" + date + "' and dtandTime<'" + DateAndTime.DateAdd(DateInterval.Day, 1, date) + "' ";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if ((Convert.ToDateTime(drow[14]).TimeOfDay > shftAStTime.TimeOfDay) && (Convert.ToDateTime(drow[14]).TimeOfDay < shftAEndTime.TimeOfDay))
                    {
                        xrayTBRA.SetValue((xrayTBRA[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[14]).TimeOfDay > shftBStTime.TimeOfDay) && (Convert.ToDateTime(drow[14]).TimeOfDay < shftBEndTime.TimeOfDay))
                    {
                        xrayTBRB.SetValue((xrayTBRB[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[14]).TimeOfDay >= shftCStTime.TimeOfDay) || (Convert.ToDateTime(drow[14]).TimeOfDay <= shftCEndTime.TimeOfDay))
                    {
                        xrayTBRC.SetValue((xrayTBRC[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }
                }

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            for (DateTime date = new DateTime(dt.Year, dt.Month, 1); date.Day <= DateTime.DaysInMonth(dt.Year, dt.Month); date = DateAndTime.DateAdd(DateInterval.Day, 1, date))
            {
                string query = "select * from vuniBalRunOutTBR where status= 2 and dtandTime >= '" + date + "' and dtandTime<'" + DateAndTime.DateAdd(DateInterval.Day, 1, date) + "' ";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if ((Convert.ToDateTime(drow[7]).TimeOfDay > shftAStTime.TimeOfDay) && (Convert.ToDateTime(drow[7]).TimeOfDay < shftAEndTime.TimeOfDay))
                    {
                        uniTBRA.SetValue((uniTBRA[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[7]).TimeOfDay > shftBStTime.TimeOfDay) && (Convert.ToDateTime(drow[7]).TimeOfDay < shftBEndTime.TimeOfDay))
                    {
                        uniTBRB.SetValue((uniTBRB[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }

                    else if ((Convert.ToDateTime(drow[7]).TimeOfDay >= shftCStTime.TimeOfDay) || (Convert.ToDateTime(drow[7]).TimeOfDay <= shftCEndTime.TimeOfDay))
                    {
                        uniTBRC.SetValue((uniTBRC[Convert.ToInt32(date.Day)]) + 1, Convert.ToInt32(date.Day));
                    }
                }

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            for (int i = 0; i < 32; i++)
            {
                finalTBRA[i] = insTBRA[i] + xrayTBRA[i] + uniTBRA[i];
                finalTBRB[i] = insTBRB[i] + xrayTBRB[i] + uniTBRB[i];
                finalTBRC[i] = insTBRC[i] + xrayTBRC[i] + uniTBRC[i];
            }

            curedTireScrapChart.Series.Add("ShiftA Scrap");
            curedTireScrapChart.Series.Add("ShiftB Scrap");
            curedTireScrapChart.Series.Add("ShiftC Scrap");

            for (int i = 0; i <= 2; i++)
            {
                curedTireScrapChart.Series[i].ChartType = SeriesChartType.Column;
            }
            curedTireScrapChart.Width = 800;
            curedTireScrapChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            curedTireScrapChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            curedTireScrapChart.ChartAreas[0].AxisX.Interval = 1;
            curedTireScrapChart.Series[0].Points.DataBindXY(xVals, finalTBRA);
            curedTireScrapChart.Series[1].Points.DataBindXY(xVals, finalTBRB);
            curedTireScrapChart.Series[2].Points.DataBindXY(xVals, finalTBRC);
        }

        #endregion     

    }
        
    }

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   