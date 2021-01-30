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
using System.Data.SqlClient;


namespace SmartMIS.UserControl
{
    public partial class matConsumptionControl : System.Web.UI.UserControl
    {
        myConnection myConnection = new myConnection();
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;
        ArrayList xValues = new ArrayList();
        ArrayList xValuesMatTypeName = new ArrayList();
        double[] seriesA = null;
        double[] seriesB = null;
        double[] seriesC = null;
        double[] seriesAWC = null;
        double[] seriesBWC = null;
        double[] seriesCWC = null;
        smartMISWebService myWebService = new smartMISWebService();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] tempString = magicHidden.Value.Split(new char[] { '?' });
            productionReportDateWiseGridView.DataSource = null;
            productionReportDateWiseGridView.DataBind();
            matConsumptionChart.Visible = false;

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
                        string dt = formatChartDate(rToDate);
                        getXValues(dt);
                        createChart(dt);
                        createChartMatTypeWise(dt);
                        rFromDate = formatDate(myWebService.formatDate(rFromDate));
                        showDailyReport();
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
                if (((GridView)sender).ID == "productionReportDateWiseGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseWCIDLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("productionReportDateWiseChildGridView"));
                    //System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)childGridView.FindControl("qualityReportDailyChart");                    
                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()));
                }

                else if (((GridView)sender).ID == "productionReportDateWiseChildGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildWCIDLabel"));
                    Label matTypeNameLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildMatTypeNameLabel"));
                    Label rawMatNameLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildRawMaterialNameLabel"));
                    //System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("qualityReportDailyChart");
                    //fillChart(childChart, wcIDLabel.Text.Trim(), matTypeNameLabel.Text, rawMatNameLabel.Text, rToDate);


                }
            }
        }

        #endregion

        #region UserDefined Function

        public void getXValues(string dt)
        {
            // Values for RawMaterial Name Wise for Chart Binding
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select distinct rawMaterialName from vMaterialConsumption where dtandtime >= '" + dt.ToString() + "' and dtandTime < '" + (Convert.ToDateTime(dt).AddDays(1).ToString()) + "' and wcID in (" + createQuery(rWCID.ToString()) + ")  group by rawMaterialName, shiftName";
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                xValues.Add(drow[0].ToString());
            }
            seriesA = new double[ds.Tables[0].Rows.Count];
            seriesB = new double[ds.Tables[0].Rows.Count];
            seriesC = new double[ds.Tables[0].Rows.Count];
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            // Values for RawMaterial Type Name Wise for Chart Binding
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select distinct materialTypeName from vMaterialConsumption where dtandtime >= '" + dt.ToString() + "' and dtandTime < '" + (Convert.ToDateTime(dt).AddDays(1).ToString()) + "' and wcID in (" + createQuery(rWCID.ToString()) + ")  group by materialTypeName, shiftName";
            SqlDataAdapter ad = new SqlDataAdapter(myConnection.comm);
            DataSet dataset = new DataSet();
            ad.Fill(dataset);

            foreach (DataRow drow in dataset.Tables[0].Rows)
            {
                xValuesMatTypeName.Add(drow[0].ToString());
            }
            seriesAWC = new double[dataset.Tables[0].Rows.Count];
            seriesBWC = new double[dataset.Tables[0].Rows.Count];
            seriesCWC = new double[dataset.Tables[0].Rows.Count];
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

        }

        public string formatChartDate(string date)
        {
            string flag = "";
            string month, day, year;
            string[] tempDate = date.Split(new char[] { '-' });
            month = tempDate[1].ToString();
            day = tempDate[0].ToString();
            year = tempDate[2].ToString();
            flag = month + "-" + day + "-" + year;
            return flag;
        }

        private void fillChildGridView(GridView childGridView, int wcID)
        {
            if (childGridView.ID == "productionReportDateWiseChildGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("select distinct materialTypeName,wcID,rawMaterialName from vMaterialConsumption where  dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId in(" + wcID + ")", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        public string plannedQuantity(Object wcID, Object materialTypeName, Object rawMaterialName, Object shift)
        {
            string flag = "";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select sum(quantity) As TotQty from vMaterialConsumption where dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId=" + Convert.ToInt32(wcID) + "and materialTypeName='" + materialTypeName.ToString() + "' and rawMaterialName='" + rawMaterialName.ToString() + "' and ShiftName=@shift group by materialTypeName,rawMaterialName,ShiftName,wcID,wcName order by materialTypeName,ShiftName";
            myConnection.comm.Parameters.AddWithValue("@shift", shift);
            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                flag = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        public string formatDate(String date)
        {
            string flag = "";
            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");
            return flag;
        }

        private void fillChart(System.Web.UI.DataVisualization.Charting.Chart objChart, Object wcID, Object materialTypeName, Object rawMaterialName, Object rDate)
        {
            double[] planSeries = new double[3];

            for (int i = 0; i < 3; i++)
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select sum(quantity) As TotQty from vMaterialConsumption where dtandTime>='" + rDate.ToString() + "' and dtandTime<'" + Convert.ToDateTime(rDate).AddDays(1) + "' and wcId='" + wcID.ToString() + "' and materialTypeName='" + materialTypeName + "' and rawMaterialName='" + rawMaterialName + "' and shiftName='" + myWebService.shift[i].ToString() + "' and wcID in " + createQuery(rWCID.ToString()) + " group by materialTypeName,rawMaterialName,shiftName,wcID,wcName order by materialTypeName,shiftName";
                myConnection.reader = myConnection.comm.ExecuteReader();

                if (myConnection.reader.HasRows)
                {
                    while (myConnection.reader.Read())
                    {
                        planSeries.SetValue(Convert.ToInt32(myConnection.reader[0].ToString()), i);
                    }
                }

                else
                {
                    planSeries.SetValue(0, i);
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            objChart.Series[0]["PointWidth"] = "0.6";
            objChart.Series[0]["DrawingStyle"] = "Cylinder";
            objChart.Series[0].Points.DataBindXY(myWebService.shift, planSeries);
        }

        protected void showDailyReport()
        {
            //fillGridView("select materialTypeName,rawMatNAme,sum(quantity) As TotQty,ShiftName from vMatarialConsumption where dtandTime='" + myWebService.formatDate(rToDate) + "' and wcId in(" + createQuery(rWCID) + ") group by materialTypeName,rawMatNAme,ShiftName order by materialTypeName,ShiftName");
            fillGridView("Select DISTINCT wcID, wcName from vMaterialConsumption WHERE dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId in(" + createQuery(rWCID) + ")");
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011
            //Revision No.  : 01

            productionReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            productionReportDateWiseGridView.DataBind();
        }

        public string createQuery(String wcID)
        {
            string query = "";
            //string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });
            //   string items = "";
            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + items + ",";
                    //or = " Or ";
                }
            }
            //query = items;

            query = query.Substring(0, query.Length - 1);
            //  query = "(" + query + ") And PlanDtandTime > '" + myWebService.formatDate(toDate) + "' And PlanDtandTime < '" + myWebService.formatDate(fromDate) + "'";

            return query;
        }

        public void createChart(string dt)
        {
            matConsumptionChart.Series.Add("ShiftA Consumption");
            matConsumptionChart.Series.Add("ShiftB Consumption");
            matConsumptionChart.Series.Add("ShiftC Consumption");
          

            for (int i = 0; i < xValues.Count; i++)
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select rawMaterialName,shiftName,sum(quantity) as Total from vMaterialConsumption where dtandtime >= '" + dt.ToString() + "' and dtandTime < '" + (Convert.ToDateTime(dt).AddDays(1).ToString()) + "' and  rawMaterialName='" + xValues[i].ToString() + "' and wcID in (" + createQuery(rWCID) + ")   group by rawMaterialName, shiftName";
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);         

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow[1].Equals("A"))
                    {
                        seriesA[i] = Convert.ToDouble(drow[2]);
                    }
                    if (drow[1].Equals("B"))
                    {
                        seriesB[i] = Convert.ToDouble(drow[2]);
                    }
                    if (drow[1].Equals("C"))
                    {
                        seriesC[i] = Convert.ToDouble(drow[2]);
                    }
                }                
                if (ds.Tables[0].Rows.Count != 0)
                {
                    matConsumptionChart.Visible = true;
                }
            }

            for (int i = 0; i < matConsumptionChart.Series.Count; i++)
            {
                matConsumptionChart.Series[i].IsValueShownAsLabel = true;
                matConsumptionChart.Series[i]["PointWidth"] = "0.6";
                matConsumptionChart.Series[i]["DrawingStyle"] = "Cylinder";
            }

            matConsumptionChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
            matConsumptionChart.ChartAreas[0].AxisY.Title = "Consumption";
            matConsumptionChart.Titles.Add("Title");
            matConsumptionChart.Titles[0].Text = "Material Consumption Trend";
            matConsumptionChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
            matConsumptionChart.Series[0].Points.DataBindXY(xValues, seriesA);
            matConsumptionChart.Series[1].Points.DataBindXY(xValues, seriesB);
            matConsumptionChart.Series[2].Points.DataBindXY(xValues, seriesC);
            matConsumptionChart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Verdana", 12);
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            Array.Clear(seriesA, 0, seriesA.Length);
            Array.Clear(seriesB, 0, seriesA.Length);
            Array.Clear(seriesC, 0, seriesA.Length);
        }

        public void createChartMatTypeWise(string dt)
        {
            matConsumpRawMatTypeWiseChart.Series.Add("ShiftA Consumption");
            matConsumpRawMatTypeWiseChart.Series.Add("ShiftB Consumption");
            matConsumpRawMatTypeWiseChart.Series.Add("ShiftC Consumption");
            for (int i = 0; i < xValuesMatTypeName.Count; i++)
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select wcID,materialTypeName,shiftName, sum(quantity) as Total, dtandTime from vMaterialConsumption where materialTypeName='" + xValuesMatTypeName[i] + "' and dtandTime>='" + dt + "' and dtandTime<'" + (Convert.ToDateTime(dt).AddDays(1).ToString()) + "' and wcID in (" + createQuery(rWCID) + ") group by materialTypeName, shiftname, dtandTime, wcID";
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow[2].Equals("A"))
                    {
                        seriesAWC[i] =seriesAWC[i]+ Convert.ToDouble(drow[3]);
                    }
                    if (drow[2].Equals("B"))
                    {
                        seriesBWC[i] =seriesBWC[i]+ Convert.ToDouble(drow[3]);
                    }
                    if (drow[2].Equals("C"))
                    {
                        seriesCWC[i] =seriesCWC[i]+ Convert.ToDouble(drow[3]);
                    }
                }
                if (ds.Tables[0].Rows.Count != 0)
                {
                    matConsumpRawMatTypeWiseChart.Visible = true;
                }
            }

            for (int i = 0; i < matConsumpRawMatTypeWiseChart.Series.Count; i++)
            {
                matConsumpRawMatTypeWiseChart.Series[i].IsValueShownAsLabel = true;
                matConsumpRawMatTypeWiseChart.Series[i]["PointWidth"] = "0.6";
                matConsumpRawMatTypeWiseChart.Series[i]["DrawingStyle"] = "Cylinder";
            }

            matConsumpRawMatTypeWiseChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Verdana", 12);
            matConsumpRawMatTypeWiseChart.ChartAreas[0].AxisY.Title = "Consumption";
            matConsumpRawMatTypeWiseChart.Titles.Add("Title");
            matConsumpRawMatTypeWiseChart.Titles[0].Text = "Raw Material Type Consumption Trend";
            matConsumpRawMatTypeWiseChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);
            matConsumpRawMatTypeWiseChart.Series[0].Points.DataBindXY(xValuesMatTypeName, seriesAWC);
            matConsumpRawMatTypeWiseChart.Series[1].Points.DataBindXY(xValuesMatTypeName, seriesBWC);
            matConsumpRawMatTypeWiseChart.Series[2].Points.DataBindXY(xValuesMatTypeName, seriesCWC);
            matConsumpRawMatTypeWiseChart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Verdana", 12);
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        #endregion       

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               