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
using Microsoft.VisualBasic; 

namespace SmartMIS
{  
    
    
    //Author      : Brajesh Kumar
    //Date Created  : 20 May 2011
    //Date Updated  : 20 May 2011
    //Revision No.  : 01
    //Revision Desc : 
    public partial class oeeReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        //All Globle Variables :


        string queryString = null;
        int selectedMonth, selectedDate;
        int code, dayOrShift,unscheduleDownTime, noOfDays;
        string reportType, reportChoice, filteredDate, toDate, toMonth, toYear, Year, query, month, day, year,wcName;
        string value;
        double idealRunrate, AvailabilityRate,scheduleTime, performanceRate, qualityRate, oee, tempVal1, tempVal2, totalAvailableTime, totalProduction,opratingTime,scrap,tempScheduleTime;
        DataTable dt = new DataTable();
        DataTable dt1;
        //double[] avlRateValues =null;
            
        public void splitQueryString(string queryString)// This Method to split Quary String To get value in diffrent variables 
        {
            string[] tempString = queryString.Split(new char[] { '#' });
        }

        private void datacolumn() //This Function is use to Add data columns in data table 
        {
            dt.Columns.Add("wcName");
            dt.Columns.Add("AvailabilityRate");
            dt.Columns.Add("performanceRate");
            dt.Columns.Add("qualityRate");
            dt.Columns.Add("oee");

        }

        private void dataRowBind()
        {
            DataRow row = dt.NewRow();

            row["wcName"] = wcName.ToString();
            row["AvailabilityRate"] = AvailabilityRate.ToString();
            row["performanceRate"] = performanceRate.ToString();
            row["qualityRate"] = qualityRate.ToString();
            row["oee"] = oee.ToString();
            dt.Rows.Add(row);
            oeeReportGridView.DataSource = dt;
            oeeReportGridView.DataBind();
        }// we use dataRowBind function to bind datarow in gridview


        private void childGridDatacolumn() //This Function is use to Add data columns in data table 
        {
            dt1.Columns.Add("rateName");
            dt1.Columns.Add("day1");
            dt1.Columns.Add("day2");
            dt1.Columns.Add("day3");
            dt1.Columns.Add("day4");
            dt1.Columns.Add("day5");
            dt1.Columns.Add("day6");
            dt1.Columns.Add("day7");
            dt1.Columns.Add("day8");
            dt1.Columns.Add("day9");
            dt1.Columns.Add("day10");
            dt1.Columns.Add("day11");
            dt1.Columns.Add("day12");
            dt1.Columns.Add("day13");
            dt1.Columns.Add("day14");
            dt1.Columns.Add("day15");
            dt1.Columns.Add("day16");
            dt1.Columns.Add("day17");
            dt1.Columns.Add("day18");
            dt1.Columns.Add("day19");
            dt1.Columns.Add("day20");
            dt1.Columns.Add("day21");
            dt1.Columns.Add("day22");
            dt1.Columns.Add("day23");
            dt1.Columns.Add("day24");
            dt1.Columns.Add("day25");
            dt1.Columns.Add("day26");
            dt1.Columns.Add("day27");
            dt1.Columns.Add("day28");
            dt1.Columns.Add("day29");
            dt1.Columns.Add("day30");
            dt1.Columns.Add("day31");

        }



        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "oeeReportMonthWiseGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("oeeReportMonthWiseWCIDLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("oeeReportInnerGridView"));


                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()));
                }
            }
        }

        private void fillChildGridView(GridView childGridView,int wcID)
        {
          
            string workCeID = wcID.ToString();

            if (childGridView.ID == "oeeReportInnerGridView")
            {
                string[] tempString2 = queryString.Split(new char[] { '?' });
                        string month = tempString2[5].ToString();
                    string year = tempString2[6].ToString();
                    noOfDays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                    wcWiseReportDiv.Visible = false;
                    monthWiseReportDiv.Visible = true;
                    double[] avlRateValues = new double[Convert.ToInt32(noOfDays)];
                    double[] perRateValues = new double[Convert.ToInt32(noOfDays)];
                    double[] quaRateValues = new double[Convert.ToInt32(noOfDays)];
                    double[] oeeRateValues = new double[Convert.ToInt32(noOfDays)];


                    dt1 = new DataTable();
                    childGridDatacolumn();

                    DataRow row1 = dt1.NewRow();
                    DataRow row2 = dt1.NewRow();
                    DataRow row3 = dt1.NewRow();
                    DataRow row4 = dt1.NewRow();


                    row1["rateName"] = "Availability Rate";

                    row2["rateName"] = "Performance Rate";
                    row3["rateName"] = "Quality Rate";
                    row4["rateName"] = "Oee Rate";

                    for (int i = 1; i <= noOfDays; i++)
                    {
                        string day = i.ToString();
                        filteredDate = month + "-" + day + "-" + year;
                        getValue(workCeID, Convert.ToDateTime(filteredDate));
                        calculateValue();
                        if (Double.IsNaN(AvailabilityRate))
                        {
                            AvailabilityRate = 0.0;
                        }
                        else
                        {
                            avlRateValues.SetValue(AvailabilityRate, i - 1);
                        }
                        if (Double.IsNaN(performanceRate))
                        {
                            performanceRate = 0.0;
                        }
                        else
                        {
                            perRateValues.SetValue(performanceRate, i - 1);
                        }
                        if (Double.IsNaN(qualityRate))
                        {
                            qualityRate = 0.0;
                        }
                        else
                        {
                            quaRateValues.SetValue(qualityRate, i - 1);
                        }
                        if (Double.IsNaN(oee))
                        {
                            oee = 0.0;
                        }
                        else
                        {
                            oeeRateValues.SetValue(oee, i - 1);
                        }
                        row1[i] = avlRateValues[i - 1];
                        row2[i] = perRateValues[i - 1];
                        row3[i] = quaRateValues[i - 1];
                        row4[i] = oeeRateValues[i - 1];

                    }


                    dt1.Rows.Add(row1);
                    dt1.Rows.Add(row2);
                    dt1.Rows.Add(row3);
                    dt1.Rows.Add(row4);

                childGridView.DataSource = dt1;
                childGridView.DataBind();
               
            }
        }


     
            
        // we use dataRowBind function to bind datarow in gridview

       private void showDailyReport(string wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "iD = '" + items + "'";
                    or = " Or ";
                }

            }

            fillGridView("Select DISTINCT iD, name from wcMaster where " + query);
            

        }

        private void fillGridView(string query)
        {
            oeeReportMonthWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            oeeReportMonthWiseGridView.DataBind();
        }

        private void clearVariable()//this method to use clear all variables 
        {
            selectedMonth=0; selectedDate=0;
            code=100; dayOrShift=100; unscheduleDownTime=0;
            wcName = ""; month = ""; day = ""; year = "";
            value="";
            idealRunrate = 0.0; AvailabilityRate = 0.0; scheduleTime = 0.0; performanceRate = 0.0; qualityRate = 0.0; oee = 0.0; tempVal1 = 0.0; tempVal2 = 0.0; totalAvailableTime = 0.0; totalProduction = 0.0; opratingTime = 0.0; scrap = 0.0; tempScheduleTime = 0.0; 

        }

      
        protected void Page_Load(object sender, EventArgs e)
        {
            splitQueryString(magicHidden.Value);       
            datacolumn();
            clearVariable();
        }


        protected void magicButton_Click(object sender, EventArgs e) //This is Magic Button to take the data of report master page 
        //On content page and according to filter of master page we show report
        {
            queryString = magicHidden.Value;
            string[] tempString = queryString.Split(new char[] { '#' });
            reportType = tempString[0].Substring(0, 1);
            string[] tempString2 = queryString.Split(new char[] { '?' });
            string[] workCenterID = tempString2[1].Split(new char[] { '#' });
            if (reportType.Equals("1"))
            {
               
                reportChoice = tempString2[2].ToString();
                if (reportChoice.Equals("0"))
                {
                  
                    filteredDate = formatDate(tempString2[3].ToString());
                    monthWiseReportDiv.Visible = false;

                    foreach (string wcID in workCenterID)
                    {
                        if (!(wcID.Equals("")))
                        {
                            getValue(wcID, Convert.ToDateTime(filteredDate));
                            calculateValue();
                            dataRowBind();
                            clearVariable();
                        }
                    }
                    getGridData();
                }

                else if (reportChoice.Equals("1"))
                {
                    showDailyReport(tempString2[1]);
                }
                toMonth = tempString2[5].ToString();
                toYear = tempString2[6].ToString();
                Year = tempString2[7].ToString();

                

            }
        }

        private void calculateValue()
        {
            opratingTime=((totalAvailableTime-unscheduleDownTime-scheduleTime));
            tempVal1 = totalAvailableTime - scheduleTime;
            AvailabilityRate = ((opratingTime )/ tempVal1)*100;
            tempVal2 = totalProduction / opratingTime;
           
            performanceRate = (tempVal2/ idealRunrate)*100;
            qualityRate = ((totalProduction - scrap) / totalProduction)*100;
            oee = ((AvailabilityRate/100) * (performanceRate/100) * (qualityRate/100))*100;
           
            AvailabilityRate = Convert.ToDouble(String.Format("{0:0.0}", AvailabilityRate));
            performanceRate = Convert.ToDouble(String.Format("{0:0.0}", performanceRate));
            qualityRate = Convert.ToDouble(String.Format("{0:0.0}", qualityRate));
            oee = Convert.ToDouble(String.Format("{0:0.0}", oee));
            
        }//We use calculate value function to calculate required values for report


        private double getTotalAvailTime(string wcID)
        {

     string workCenterID =wcID;

          myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select sum(totalAvailableTime) from vOeeSettingTables where wcID="+workCenterID;
      

            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    totalAvailableTime = Convert.ToInt32(myConnection.reader[0]);
                }


                return totalAvailableTime;
            }
            else
                return totalAvailableTime = 0.0;
        }  //By getTotalAvailTime function we get total available time daywise for selected work center from availtimeSetting table 

        private string findSettingDay(string value)
        {
            string dayofweek = "";
            int daynumber = Convert.ToInt32(value);
            dayofweek = myWebService.weekDayName[daynumber];

            return dayofweek;
        }  // By this function we get day of downtime for weekly  setting 

        private void getValue(string wcID,DateTime dt)
        {

           string workCenterID =wcID;
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select * from  vOeeSettingTables where wcID="+workCenterID;

         
            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {             
                    code = Convert.ToInt32(myConnection.reader[1]);
                    switch (code)
                    {
                        case 0:
                            value = (myConnection.reader[2].ToString());
                            int tmpvar = Convert.ToInt32(value);
                            if (tmpvar == 0)
                            {
                                scheduleTime = Convert.ToInt32(myConnection.reader[3].ToString());
                            }
                            else
                            {
                                scheduleTime = 0;
                            }
                            break;
                        case 1:
                            value = myConnection.reader[2].ToString();
                      string settingdayInWeek=findSettingDay(value);

                      if (settingdayInWeek.Equals(dt.DayOfWeek.ToString()))
                      {
                          scheduleTime = Convert.ToInt32(myConnection.reader[3].ToString());
                      }
                      else
                      {
                          scheduleTime = 0;
                      }
                        break;
                        case 2:

                          selectedDate = dt.Day;

                           value = (myConnection.reader[2].ToString());
                            int tmpvar1 = Convert.ToInt32(value);
                            if (tmpvar1 == selectedDate)
                            {
                                scheduleTime = Convert.ToInt32(myConnection.reader[3].ToString());
                            }
                            else
                            {
                                scheduleTime = 0;
                            }
                            break;
                        case 3:
                            value = myConnection.reader[2].ToString();
                            myQuaterlyFunction(value,dt);

                            break;
                        case 4:

                            selectedMonth = dt.Month;
                            selectedDate = dt.Day;

                            value = myConnection.reader[2].ToString();

                            string[] tempstring1 = value.Split(new char[] { '#' });

                            int month = Convert.ToInt32(tempstring1[0].ToString());
                            int date = Convert.ToInt32(tempstring1[1].ToString());
                            string yearlyScheduledDate = (month).ToString() + "/" + date.ToString();

                            string selecteddateString = (selectedMonth).ToString() + "/" + (selectedDate).ToString();

                            if(selecteddateString.Equals(yearlyScheduledDate))
                            {
                                scheduleTime = Convert.ToInt32(myConnection.reader[3]);
                            }
                            else
                            {

                                scheduleTime = 0;
                            }
                            break;
                    }
                    idealRunrate = Convert.ToDouble(myConnection.reader[4]);
                    dayOrShift = Convert.ToInt32(myConnection.reader[5]);

                    if (dayOrShift == 0)
                    {
                        totalAvailableTime = Convert.ToInt32(myConnection.reader[6]);

                    }
                    else
                    {
                        totalAvailableTime = getTotalAvailTime(workCenterID);
                    }
                }
            }
            else
            {
                clearVariable();
            }
            myConnection.reader.Close();
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select * from  vOeeTatalScrapAndDowntTimeView where wcID="+workCenterID+" and dtandTime>='"+dt+"' and dtandTime<'"+ DateAndTime.DateAdd(DateInterval.Day,1,dt) +"'";
            myConnection.reader = myConnection.comm.ExecuteReader();
            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    wcID = myConnection.reader[0].ToString();
                    wcName = myConnection.reader[1].ToString();
                    totalProduction = Convert.ToDouble(myConnection.reader[2]);
                    scrap = Convert.ToInt32(myConnection.reader[3]);
                    unscheduleDownTime = Convert.ToInt32(myConnection.reader[4]);
                }           
            }              

            else 
            {
                myConnection.reader.Close();
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select name from wcMaster where iD="+wcID;        
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    wcName = myConnection.reader[0].ToString();
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);            
            }
        }//by get value function we get all required values from database tables for selected workcenter and date       

        private void getGridData()
        {
            foreach (GridViewRow gridViewRow in oeeReportGridView.Rows)
            {
                Chart myChart=((Chart) gridViewRow.FindControl("oeeReportChart"));
                Label avlRate =((Label) gridViewRow.FindControl("oeeGridAvailibilityLabel"));
                Label perRate = ((Label)gridViewRow.FindControl("oeeGridperformanceLabel"));
                Label qualityRate = ((Label)gridViewRow.FindControl("oeeGridQualityLabel"));
                Label oeeRate = ((Label)gridViewRow.FindControl("oeeGridOeeReportLabel"));

                formatChart(myChart,avlRate,perRate,qualityRate,oeeRate);
            }
        
        } // we use this function to get daqta grid value for chart binding

        private void formatChart(Chart myChart,Label avlRate,Label perRate, Label qualityRate,Label oeeRate)
        {
            myChart.Series.Add("availRateSeries"); 
            myChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            myChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            myChart.Series[0].ChartType = SeriesChartType.Column;
            myChart.Series[0].IsValueShownAsLabel = true;
            myChart.Series[0]["PointWidth"] = "0.4";
            myChart.Series[0]["DrawingStyle"] = "Cylinder";
            myChart.Series[0].Points.AddXY(1, Convert.ToDouble(avlRate.Text));
            myChart.Series[0].Points.AddXY(2,Convert.ToDouble(perRate.Text));
            myChart.Series[0].Points.AddXY(3, Convert.ToDouble(qualityRate.Text));
            myChart.Series[0].Points.AddXY(4, Convert.ToDouble(oeeRate.Text));

            myChart.Series[0].Points[0].AxisLabel = "Availabilty";
            myChart.Series[0].Points[1].AxisLabel = "Performance";
            myChart.Series[0].Points[2].AxisLabel = "Quality";
            myChart.Series[0].Points[3].AxisLabel = "OEE";

            myChart.Series[0].Points[0].Color = Color.DarkOrange;
            myChart.Series[0].Points[1].Color = Color.DarkSlateBlue;
            myChart.Series[0].Points[2].Color = Color.Chocolate;
            myChart.Series[0].Points[3].Color = Color.DarkRed;

        }    //this function is used to bind values with in chart 

        private void myQuaterlyFunction(string quaterlySetting, DateTime dt)
        {
            string setting = quaterlySetting;

            int quaterdate;
            int quatermonth;

            string firstQuaterDate;
            string secondQuaterDate;
            string thirdQuaterDate;
            string fourthQuaterDate;

            selectedMonth =dt.Month;
            selectedDate = dt.Day;

            string[] tempsetting = setting.Split(new char[] { '#' });

            quatermonth = Convert.ToInt32(tempsetting[0].ToString());
            quaterdate = Convert.ToInt32(tempsetting[1].ToString());

            firstQuaterDate = (quatermonth).ToString() + "/" + quaterdate.ToString();
            secondQuaterDate = (quatermonth + 3).ToString() + "/" + quaterdate.ToString();
            thirdQuaterDate = (quatermonth + 6).ToString() + "/" + quaterdate.ToString();
            fourthQuaterDate = (quatermonth + 9).ToString() + "/" + quaterdate.ToString();

            string selecteddate = (selectedMonth).ToString() + "/" + (selectedDate).ToString();
           
            if ((selecteddate.Equals(firstQuaterDate)) || (selecteddate.Equals(secondQuaterDate)) || (selecteddate.Equals(thirdQuaterDate)) || (selecteddate.Equals(fourthQuaterDate)))
            {

                scheduleTime = tempScheduleTime;
            }

            else
            {

                scheduleTime = 0;
            }


        } // this function is used to compare the date value with quaterly setting date 

           public string formatDate(string date)
        {
            string flag = "";
            string[] tempDate = date.Split(new char[] { '-' });
            month = tempDate[1].ToString();
            day = tempDate[0].ToString();
            year = tempDate[2].ToString();
            flag = month + "-" + day + "-" + year;
            return flag;
        }    // this is formate date unction used to formate date according to requirment
    }
}
