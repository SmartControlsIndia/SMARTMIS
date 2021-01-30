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
    public partial class DownTimeReport : System.Web.UI.Page
    {
        SmartMIS.smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
     static DataTable tbldt = new DataTable();
        protected void Page_PreInit(object sender, EventArgs e)
        {
         // Create an event handler for the master page's contentCallEvent event
            Master.contentCallEvent += new EventHandler(Master1_ButtonClick);

        }
        string getProcess = "";
        string duration = "";
        string getDate = "";
        string getMonth = "";
        string getYear = "";
        string getYearwise = "";
        string getShift = "";
    static  string getoperator = "";
        string getDownTimeLimit = "";
        string getfromdate;
        string gettodate;
        string month = "";
        string day = "";
        string year = "";
        DateTime fromDate;
        DateTime toDate;
        string wcIDInQuery;
        string durationQuery = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            backDiv.Visible = false;
            dialogPanel.Visible = false;
            Master.heading = "MACHINE DOWN TIME REPORT";

        }
        private void Master1_ButtonClick(object sender, EventArgs e)
        {
           

            getProcess = Master.reportMasterWCProcessDropDownListValue;
            duration = Master.DropDownListDurationValue;
          //getOperator = Master.DropDownListOperatorsValue;
            getDate = Master.reportMasterFromDateTextBoxValue;
            getMonth = Master.DropDownListMonthValue;
            getYear = Master.DropDownListYearValue;
            getYearwise = Master.DropDownListYearWiseValue;
            getShift = Master.DropDownListShiftValue;
            getoperator = Master.operatorDropdownListvalue;
            getDownTimeLimit = Master.DownTimeLimitTextBoxbalue;
            getfromdate = Master.CalenderTextBoxFromvalue;
            gettodate = Master.CalenderTextBoxValue;

            tbldt.Clear();
            ArrayList wcIDList = Master.reportMasterWCGridViewValue;

            ////Create Query with wcID for IN Clause
            wcIDInQuery = "(";
            foreach (string wcID in wcIDList)
            {
                wcIDInQuery += "'" + wcID.ToString() + "',";
            }
            wcIDInQuery = wcIDInQuery.TrimEnd(',');
            wcIDInQuery += ")";

            string nfromDate;
            string ntoDate;

            switch (duration)
            {

                case "Date":
                    {
                        fromDate = DateTime.Parse(formatDate(getDate));
                        toDate = fromDate.AddDays(1);

                        nfromDate = fromDate.ToString("MM-dd-yyyy");
                        ntoDate = toDate.ToString("MM-dd-yyyy");

                        showReportDateWise(nfromDate, ntoDate, getShift, getProcess);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%> Selected Process: " + getProcess.ToString() + "</td><td width=12%>DateWise :</td><td width=12% align=left>Date " + getDate + "</td><td width=16% align=right>Shift :" + getShift + " </td><td width=16% align=right>dataFilter :" + getDownTimeLimit + getoperator + " </td></tr></table></div>";
                        break;
                    }
                case "DateFromTO":
                    {
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = DateTime.Parse(formatDate(gettodate));

                        nfromDate = fromDate.ToString("MM-dd-yyyy");
                        ntoDate = toDate.ToString("MM-dd-yyyy");

                        showReportDateWise(nfromDate, ntoDate, getShift, getProcess);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%> Selected Process: " + getProcess.ToString() + "</td><td width=8%>DateFromTo :</td><td width=20% align=left> FromDate " + getDate +"  "+ "ToDate " + getDate + "</td><td width=8% align=right>Shift :" + getShift + " </td><td width=16% align=right>dataFilter :" + getDownTimeLimit + getoperator + " </td></tr></table></div>";
                        break;
                    }

                case "Month":
                    {
                        showReportMonthWise(Convert.ToInt32(getMonth), Convert.ToInt32(getYear), getShift, getProcess);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%> Selected Process: " + getProcess.ToString() + "</td><td width=8%>Month Wise :</td><td width=20% align=left> Month " + getMonth + "  " + "Year " + getYear + "</td><td width=8% align=right>Shift :" + getShift + " </td><td width=16% align=right>dataFilter :" + getDownTimeLimit + getoperator + " </td></tr></table></div>";

                        break;
                    }
                case "Year":
                    {
                        showReportYearWise(Convert.ToInt32(getYearwise), getShift);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%> Selected Process: " + getProcess.ToString() + "</td><td width=8%>Year Wise :</td><td width=20% align=left> Year " + getYear + "</td><td width=8% align=right>Shift :" + getShift + " </td><td width=16% align=right>dataFilter :" + getDownTimeLimit + getoperator + " </td></tr></table></div>";
                        break;
                    }


            }


        
        }
        protected void showReportDateWise(string fromDate, string toDate, string type, string process)
        {
            // Validate the user input with proper message
            if (validateInput("date", type, fromDate, toDate, 0, 0, 0))
            {

                showReportDayWcWise(fromDate, toDate, type, process);

            }
        }
        protected void showReportMonthWise(int getMonth, int getYear, string type, string process)
        {
            if (validateInput("month", type, "", "", getMonth, getYear, 0))
            {

                showReportDayWcWise(getfromdate, gettodate, getShift,getProcess);

            }
        }
        protected void showReportYearWise(int getYearwise, string type)
        {
            if (validateInput("year", type, "", "", 0, 0, getYearwise))
            {
                switch (type)
                {
                    case "WcWise":
                        showReportDayWcWise(getfromdate, gettodate, getShift, getProcess);
                        break;

                }
            }
        }
        protected void showReportDayWcWise(string fromDate, string toDate, string shift, string process)
        {
            try
            {
                string tablename = "";
                
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("WCName", typeof(string));
                gridviewdt.Columns.Add("TotalDownTime", typeof(string));
              //  gridviewdt.Columns.Add("ShowDetail", typeof(string));

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                tablename = "vDownTimeDetail";

                if (shift == "All")
                {
                    myConnection.comm.CommandText = "Select wcName, dtandtime,status,shift from vDownTimeDetail WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY  wcname asc";
                }
                else if(shift!="All")
                    myConnection.comm.CommandText = "Select wcName, dtandtime,status,shift from vDownTimeDetail WHERE wcID IN " + wcIDInQuery.ToString() + " and shift='"+shift+"' AND " + durationQuery + " ORDER BY  wcname asc";
               

                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);


                var distinctvalues = tbldt.AsEnumerable()
                  .Select(s => new
                  {
                      wcname = s.Field<string>("wcName"),
                  })
                  .Distinct().ToList();

                int minut = 0;
                for (int i = 0; i < distinctvalues.Count; i++)
                {
                    var aa = (from r in tbldt.AsEnumerable() where r.Field<string>("wcName").Equals(distinctvalues[i].wcname) select r ).CopyToDataTable();

                    DataRow dr = gridviewdt.NewRow();
                    minut = 0;

                    for (int j = 0; j < aa.Rows.Count; j++)
                    {

                        if (aa.Rows[j][2].ToString() == "stop" && aa.Rows[j + 1][2].ToString() == "start")
                        {
                            TimeSpan span =Convert.ToDateTime(aa.Rows[j+1][1]).Subtract(Convert.ToDateTime(aa.Rows[j][1]));
                            minut = minut + span.Minutes;
                        }
                    

                    }
                    if (getoperator == "All")
                    {

                        dr[0] = distinctvalues[i].wcname;
                        dr[1] = minut;
                        //dr[2] = "";
                        gridviewdt.Rows.Add(dr);
                    }
                    else if (getoperator != "All")
                    {
                        if(getDownTimeLimit=="")
                            getDownTimeLimit="0";
                        if (getoperator == "<")
                        {

                            if (Convert.ToInt32(minut) < Convert.ToInt32(getDownTimeLimit))
                            {
                                dr[0] = distinctvalues[i].wcname;
                                dr[1] = minut;
                                //dr[2] = "";
                                gridviewdt.Rows.Add(dr);
                            }
                        }

                        else if (getoperator == ">")
                        {
                            if (Convert.ToInt32(minut) > Convert.ToInt32(getDownTimeLimit))
                            {
                                dr[0] = distinctvalues[i].wcname;
                                dr[1] = minut;
                                //dr[2] = "";
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        else if (getoperator == ">=")
                        {
                            if (Convert.ToInt32(minut) >= Convert.ToInt32(getDownTimeLimit))
                            {
                                dr[0] = distinctvalues[i].wcname;
                                dr[1] = minut;
                                //dr[2] = "";
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        else if (getoperator == "<=")
                        {
                            if (Convert.ToInt32(minut) <= Convert.ToInt32(getDownTimeLimit))
                            {
                                dr[0] = distinctvalues[i].wcname;
                                dr[1] = minut;
                                //dr[2] = "";
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                    }


                }

                
                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Spline;
                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = false;
                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;
                TBMChart.DataSource = gridviewdt;
                TBMChart.Series["TBMSeries"].XValueMember = "wcName";
                TBMChart.Series["TBMSeries"].YValueMembers = "TotalDownTime";
                TBMChart.DataBind();


             

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                    case "year":
                        string nfromDate = yearwise.ToString() + "-01-01 07:00:00";
                        string ntoDate = (yearwise + 1).ToString() + "-01-01 07:00:00";
                        durationQuery += "(dtandTime >= '" + nfromDate + "' AND dtandTime < '" + ntoDate + "')";
                        break;
                }




            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
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


        protected void DownTimeDetaillabel_Click(object sender, EventArgs e)
        {
            
            if (((LinkButton)sender).ID == "DownTimeDetaillabel")
            {
           

                DataTable gridviewdt1 = new DataTable();
                gridviewdt1.Columns.Add("WCName", typeof(string));
                gridviewdt1.Columns.Add("startTime", typeof(string));
                gridviewdt1.Columns.Add("stopTime", typeof(string));
                gridviewdt1.Columns.Add("totalDownTime", typeof(string));
                gridviewdt1.Columns.Add("shift", typeof(string));
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((LinkButton)sender).Parent).Parent;
                string wcName = (((Label)gridViewRow.Cells[1].FindControl("DownTimeWCnamelabel")).Text);

                var aa = (from r in tbldt.AsEnumerable() where r.Field<string>("wcName").Equals(wcName) select r).CopyToDataTable();



                for (int j = 0; j < aa.Rows.Count; j++)
                {

                    if (aa.Rows[j][2].ToString() == "stop" && aa.Rows[j + 1][2].ToString() == "start")
                    {
                        TimeSpan span = Convert.ToDateTime(aa.Rows[j + 1][1]).Subtract(Convert.ToDateTime(aa.Rows[j][1]));
                        DataRow dr = gridviewdt1.NewRow();

                        if (getoperator == "All")
                        {

                            dr[0] = wcName;
                            dr[1] = aa.Rows[j][1].ToString();
                            dr[2] = aa.Rows[j + 1][1].ToString();
                            dr[3] = span.Minutes;
                            dr[4] = aa.Rows[j][3].ToString();

                            //dr[2] = "";
                            gridviewdt1.Rows.Add(dr);
                        }
                        else if (getoperator != "All")
                        {
                            if (getDownTimeLimit == "")
                                getDownTimeLimit = "0";
                            if (getoperator == "<")
                            {

                                if (Convert.ToInt32(span.Minutes) < Convert.ToInt32(getDownTimeLimit))
                                {
                                    dr[0] = wcName;
                                    dr[1] = aa.Rows[j][1].ToString();
                                    dr[2] = aa.Rows[j + 1][1].ToString();
                                    dr[3] = span.Minutes;
                                    dr[4] = aa.Rows[j][3].ToString();
                                }
                            }

                            else if (getoperator == ">")
                            {
                                if (Convert.ToInt32(span.Minutes) > Convert.ToInt32(getDownTimeLimit))
                                {
                                    dr[0] = wcName;
                                    dr[1] = aa.Rows[j][1].ToString();
                                    dr[2] = aa.Rows[j + 1][1].ToString();
                                    dr[3] = span.Minutes;
                                    dr[4] = aa.Rows[j][3].ToString();
                                }
                            }
                            else if (getoperator == ">=")
                            {
                                if (Convert.ToInt32(span.Minutes) >= Convert.ToInt32(getDownTimeLimit))
                                {
                                    dr[0] = wcName;
                                    dr[1] = aa.Rows[j][1].ToString();
                                    dr[2] = aa.Rows[j + 1][1].ToString();
                                    dr[3] = span.Minutes;
                                    dr[4] = aa.Rows[j][3].ToString();
                                }
                            }
                            else if (getoperator == "<=")
                            {
                                if (Convert.ToInt32(span.Minutes) <= Convert.ToInt32(getDownTimeLimit))
                                {
                                    dr[0] = wcName;
                                    dr[1] = aa.Rows[j][1].ToString();
                                    dr[2] = aa.Rows[j + 1][1].ToString();
                                    dr[3] = span.Minutes;
                                    dr[4] = aa.Rows[j][3].ToString();
                                }
                            }
                        }


                    }
                }
                DownTimeReportDetailGridView.DataSource = gridviewdt1;
                DownTimeReportDetailGridView.DataBind();

                DownTimeReportDetailGridView.Visible = true;
                backDiv.Visible = true;
                dialogPanel.Visible = true;
                emptyMsg.Visible = false;

            }

        }

      

    }
}
