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
using SmartMIS.SmartWebReference;

namespace SmartMIS.UserControl
{
    public partial class downTimeReportWCWise : System.Web.UI.UserControl
    {
        myConnection myConnection = new myConnection();
        smartMISWebService myWebService = new smartMISWebService();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] tempString = magicHidden.Value.Split(new char[] { '?' });

            downTimeReportWCWiseGridView.DataSource = null;
            downTimeReportWCWiseGridView.DataBind();

            //Compare the hidden field if it contains the query string or not

            if (tempString.Length > 1)
            {
                rType = tempString[0];
                rWCID = tempString[1];
                rChoice = tempString[2];
                rToDate = tempString[3];
                rFromDate = tempString[3];
                rToMonth = tempString[5];
                rToYear = tempString[6];
                rFromYear = tempString[7];

                //  Compare which type of report user had selected//
                //
                //  Plant wide = 0
                //  Workcenter wide = 1
                //


                if (rType == "0")
                {
                }
                else if (rType == "1")
                {
                    //  Compare choice of report user had selected//
                    //
                    //  Daily = 0
                    //  Monthly = 1
                    //  Yearly  = 2
                    //

                    if (rChoice == "0")
                    {
                        rFromDate = formatDate(myWebService.formatDate(rFromDate));
                        string query = myWebService.createQuery(rWCID, rToDate, rFromDate, "downdtandTime", "downdtandTime");
                        showDownTimeReport(query);
                        magicHidden.Value = "";
                    }
                    else if (rChoice == "1")
                    {
                    }
                    else if (rChoice == "2")
                    {
                    }

                }
            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "downTimeReportWCWiseGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("downTimeReportWCWiseWCIDLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("downTimeReportWCWiseChildGridView"));
                    System.Web.UI.DataVisualization.Charting.Chart childChartByDuration = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("downTimeReportByDurationChart");
                    System.Web.UI.DataVisualization.Charting.Chart childChartByOccurance = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("downTimeReportByOccuranceChart");

                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), rToDate, rFromDate);
                    fillChartByDuration(childChartByDuration, wcIDLabel.Text.Trim(), rToDate, rFromDate);
                    fillChartByOccurance(childChartByOccurance, wcIDLabel.Text.Trim(), rToDate, rFromDate);
                }
                else if (((GridView)sender).ID == "downTimeReportWCWiseChildGridView")
                {
                    Label iDLabel = ((Label)e.Row.FindControl("downTimeReportWCWiseChildIDLabel"));
                    Label wcIDLabel = ((Label)e.Row.FindControl("downTimeReportWCWiseChildWCIDLabel"));
                    Label downEventLabel = ((Label)e.Row.FindControl("downTimeReportWCWisedownEventLabel"));
                    Label upEventLabel = ((Label)e.Row.FindControl("downTimeReportWCWiseUpEventLabel"));

                    GridView childGridView = ((GridView)e.Row.FindControl("downTimeReportWCWiseChildReasonGridView"));
                    

                    DropDownList childDropDownList = ((DropDownList)e.Row.FindControl("downTimeReportWCWiseReasonIDDropDownList"));
                    DropDownList childDurationDropDownList = ((DropDownList)e.Row.FindControl("downTimeReportWCWiseDurationDropDownList"));

                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), downEventLabel.Text.Trim(), upEventLabel.Text.Trim(), iDLabel.Text.Trim(), rToDate, rFromDate);
                    
                }
            }
        }

        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }

        protected void showDownTimeReport(string query)
        {

            fillGridView("Select DISTINCT wcID, name from vDownTimeEvents WHERE " + query + "");
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling downTimeReportWCWiseGridView WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 12 May 2011
            //Date Updated  : 12 May 2011
            //Revision No.  : 01


            downTimeReportWCWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            downTimeReportWCWiseGridView.DataBind();
        }

        private void fillChildGridView(GridView childGridView, int wcID, String toDate, String fromDate)
        {

            if (childGridView.ID == "downTimeReportWCWiseChildGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("Select DISTINCT iD, wcID, downEvent, CONVERT(varchar(8), downdtandtime, 114) as downdtandTime, upEvent, CONVERT(varchar(8), updtandTime, 114) as updtandTime, duration from vDownTimeEvents WHERE (WCID = " + wcID + ") AND downdtandTime > '" + myWebService.formatDate(toDate) + "' AND updtandTime < '" + myWebService.formatDate(fromDate) + "'", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        private void fillChildGridView(GridView childGridView, int wcID, string downEvent, string upEvent, string iD, String toDate, String fromDate)
        {

            if (childGridView.ID == "downTimeReportWCWiseChildReasonGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("SELECT DISTINCT reasonID, downTimeReasonID, reasonName, description, downDuration FROM vDownTimeReason WHERE (WCID = " + wcID + ") AND downEvent = '" + downEvent + "' AND upEvent = '" + upEvent + "' AND iD = '" + iD + "' AND downdtandTime > '" + myWebService.formatDate(toDate) + "' AND updtandTime < '" + myWebService.formatDate(fromDate) + "'", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        private void fillChartByDuration(System.Web.UI.DataVisualization.Charting.Chart objChart, String wcID, String toDate, String fromDate)
        {

            //Description   : Function for creating downTimeReportByDurationChart chart
            //Author        : Brajesh kumar
            //Date Created  : 13 May 2011
            //Date Updated  : 13 May 2011
            //Revision No.  : 01

            int i = 0;
            double pointWidth = 0.2;
            int seriesCount = 0;
            
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select COUNT(DISTINCT(downEvent)) from vDownTimeEvents WHERE (WCID = " + Convert.ToInt32(wcID) + ")";
            seriesCount = (Int32)myConnection.comm.ExecuteScalar();

            myConnection.comm.CommandText = "Select MAX(downEvent), SUM(duration) as duration from vDownTimeEvents WHERE (WCID = " + Convert.ToInt32(wcID) + ")";

            double[] downTimeSeries = new double[seriesCount];
            string[] downTimeXAxis = new string[seriesCount];

            myConnection.reader = myConnection.comm.ExecuteReader();

            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    downTimeSeries.SetValue(Convert.ToInt32(myConnection.reader[1].ToString()), i);
                    downTimeXAxis.SetValue(myConnection.reader[0].ToString(), i);

                    i++;
                }
            }
            else
            {
                downTimeSeries.SetValue(0, 0);
            }

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            myConnection.reader.Close();

            TimeSpan timeSpan = TimeSpan.FromMinutes(5);

            objChart.ChartAreas[0].AxisX.TextOrientation = System.Web.UI.DataVisualization.Charting.TextOrientation.Horizontal;
            objChart.Series[0]["PointWidth"] = pointWidth.ToString();
            objChart.Series[0].Points.DataBindXY(downTimeXAxis, downTimeSeries);
        }

        private void fillChartByOccurance(System.Web.UI.DataVisualization.Charting.Chart objChart, String wcID, String toDate, String fromDate)
        {

            //Description   : Function for creating downTimeReportByOccuranceChart chart
            //Author        : Brajesh kumar
            //Date Created  : 13 May 2011
            //Date Updated  : 13 May 2011
            //Revision No.  : 01

            int i = 0;
            double pointWidth = 0.2;
            int seriesCount = 0;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select COUNT(DISTINCT downEvent) from vDownTimeEvents WHERE (WCID = " + Convert.ToInt32(wcID) + ")";
            seriesCount = (Int32)myConnection.comm.ExecuteScalar();

            myConnection.comm.CommandText = "SELECT wcID, COUNT(downEvent) AS downEventOccurance, downEvent FROM dbo.vDownTimeEvents GROUP BY wcID, downEvent HAVING (wcID = " + Convert.ToInt32(wcID) + ")";
          
            myConnection.reader = myConnection.comm.ExecuteReader();

            double[] downTimeSeries = new double[seriesCount];
            string[] downTimeXAxis = new string[seriesCount];


            if (myConnection.reader.HasRows)
            {
                while (myConnection.reader.Read())
                {
                    downTimeSeries.SetValue(Convert.ToInt32(myConnection.reader[1].ToString()), i);
                    downTimeXAxis.SetValue(myConnection.reader[2].ToString(), i);

                    i++;
                }
            }
            else
            {
                downTimeSeries.SetValue(0, 0);
            }

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            myConnection.reader.Close();

            objChart.Series[0].SmartLabelStyle.Enabled = false;
            objChart.Series[0].LabelAngle = 90;
            objChart.Series[0]["PointWidth"] = pointWidth.ToString();
            objChart.Series[0].Points.DataBindXY(downTimeXAxis, downTimeSeries);
        }
    }
}