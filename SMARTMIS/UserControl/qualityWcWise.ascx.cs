using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;
using Microsoft.VisualBasic;


namespace SmartMIS.UserControl
{
     
    public partial class qualityWcWise : System.Web.UI.UserControl
    {
        myConnection myConnection = new myConnection();

        #region Public Variables
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, month, day, year;
        ArrayList xValues = new ArrayList();     
        smartMISWebService myWebService = new smartMISWebService();
        string connString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
        #endregion

        #region System Defined functions

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] tempString = magicHidden.Value.Split(new char[] { '?' });
            productionReportDateWiseGridView.DataSource = null;
            productionReportDateWiseGridView.DataBind();

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

                //  Compare choice of report user had selected//
                //
                //  Daily = 0sss
                //  Monthly = 1
                //  Yearly  = 2                

                if (rType == "0")
                { }

                else if (rType == "1")
                {                    
                    if (rChoice == "0")
                    {                        
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
                    Label wcIDLabel = ((Label)e.Row.FindControl("qualityReportDateWiseWCIDLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("productionReportDateWiseChildGridView"));

                    System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)childGridView.FindControl("qualityReportDailyChart");

                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()));
                }

                else if (((GridView)sender).ID == "productionReportDateWiseChildGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildWCIDLabel"));
                    Label matTypeNameLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildMatTypeNameLabel"));
                    Label rawMatNameLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildRawMaterialNameLabel"));
                    System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("qualityReportDailyChart");
                    getYValues();           
                }
            }
        }

        #endregion

        #region User Defined Function

        private void fillChildGridView(GridView childGridView, int wcID)
        {
            if (childGridView.ID == "productionReportDateWiseChildGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("select distinct productTypeName,wcID,recipeName from vQualityWcWise where  dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId in(" + wcID + ")", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        public string plannedQuantity(Object wcID, Object productTypeName, Object recipeName, Object shift,Object status)
        {
            string flag = "";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "select quantity from vQualityWcWise where dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId=" + Convert.ToInt32(wcID) + "and productTypeName='" + productTypeName.ToString() + "' and recipeName='" + recipeName.ToString() + "' and ShiftName=@shift and status=@status";
            
            myConnection.comm.Parameters.AddWithValue("@shift", shift);
            myConnection.comm.Parameters.AddWithValue("@status", status);

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {               
                flag = myConnection.reader[0].ToString();               
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            if (flag == "") flag = "0"; 
            return flag;
        }

        public string formatDate(String date)
        {
            //string flag = "";
            //DateTime tempDate = Convert.ToDateTime(date);
            //flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");
            //return flag;

            string flag = "";
            string[] tempDate = date.Split(new char[] { '-' });
            month = tempDate[1].ToString();
            day = tempDate[0].ToString();
            year = tempDate[2].ToString();
            flag = year + "-" + day + "-" + month;
            return flag;
        }

        protected void showDailyReport()
        {      
            fillGridView("Select DISTINCT wcID, wcName from vQualityWcWise WHERE dtandTime>='" + myWebService.formatDate(rToDate) + "' and dtandTime<'" + Convert.ToDateTime(myWebService.formatDate(rToDate)).AddDays(1) + "' and wcId in(" + createQuery(rWCID) + ")");
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
            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011 ||  6 May 2011
            //Revision No.  : 01            ||  02
            //Updated By    :               ||  Shashank Jain
            //Upadte Description:           ||  For Getting WcID in "71,72,74" Format
           
            string query = "";
          
            string[] tempWCID = wcID.Split(new char[] { '#' });
            
            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + items + ",";                
                }
            }
            
            query = query.Substring(0, query.Length - 1);           
            return query;
        }

        #region Chart Functions

        private ArrayList getXValues()
        {           
            ArrayList xValues = new ArrayList();
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select distinct productTypeName from vQualityWCWise where  dtandtime >= '" + rFromDate + "' and  dtandtime< '" + Convert.ToDateTime(rFromDate).AddDays(1).ToString("yyyy-MM-dd") + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            foreach (DataRow dRow in ds.Tables[0].Rows)
            {
                xValues.Add(dRow[0].ToString());
            }
            cmd.Dispose();
            ds.Dispose();
            con.Close();

            return xValues;
        }

        private void getYValues()
        {
            ArrayList xVal = new ArrayList();
            xVal = getXValues();
            SqlConnection con = new SqlConnection(connString);
            con.Open();

            double[] shiftAGood = new double[xVal.Count];
            double[] shiftARej = new double[xVal.Count];
            double[] shiftBGood = new double[xVal.Count];
            double[] shiftBRej = new double[xVal.Count];
            double[] shiftCGood = new double[xVal.Count];
            double[] shiftCRej = new double[xVal.Count];

            for (int i = 0; i < xVal.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("select productTypeName, shiftName, status, sum(quantity) as Total from vQualityWCWise group by shiftName,  productTypeName, status having productTypeName='" + xVal[i].ToString() + "' order by productTypeName", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);


                foreach (DataRow dRow in ds.Tables[0].Rows)
                {
                    if (dRow[1].ToString().Trim().Equals("A") && dRow[2].ToString().Trim().Equals("0"))
                    {
                        shiftAGood.SetValue(Convert.ToDouble(dRow[3]), i);
                    }

                    else if (dRow[1].ToString().Trim().Equals("A") && dRow[2].ToString().Trim().Equals("1"))
                    {
                        shiftARej.SetValue(Convert.ToDouble(dRow[3]), i);
                    }

                    else if (dRow[1].ToString().Trim().Equals("B") && dRow[2].ToString().Trim().Equals("0"))
                    {
                        shiftBGood.SetValue(Convert.ToDouble(dRow[3]), i);
                    }

                    else if (dRow[1].ToString().Trim().Equals("B") && dRow[2].ToString().Trim().Equals("1"))
                    {
                        shiftBRej.SetValue(Convert.ToDouble(dRow[3]), i);
                    }

                    else if (dRow[1].ToString().Trim().Equals("C") && dRow[2].ToString().Trim().Equals("0"))
                    {
                        shiftCGood.SetValue(Convert.ToDouble(dRow[3]), i);
                    }

                    else if (dRow[1].ToString().Trim().Equals("C") && dRow[2].ToString().Trim().Equals("1"))
                    {
                        shiftCRej.SetValue(Convert.ToDouble(dRow[3]), i);
                    }
                }
                cmd.Dispose();
                ds.Dispose();
                con.Close();
            }
            createChart(xVal, shiftAGood, shiftARej, shiftBGood, shiftBRej, shiftCGood, shiftCRej);


        }

        private void createChart(ArrayList xVal, double[] shAGood, double[] shARej, double[] shBGood, double[] shBRej, double[] shCGood, double[] shCRej)
        {
            qualityWCWiseChart.Titles.Add("");
            qualityWCWiseChart.Titles[0].Text = "Quality Rejection Trend(Good/Rejected)";
            qualityWCWiseChart.Titles[0].Font = new System.Drawing.Font("Verdana", 14);

            for (int i = 0; i < qualityWCWiseChart.Series.Count; i++)
            {
                qualityWCWiseChart.Series[i].IsValueShownAsLabel = true;
                qualityWCWiseChart.Series[i].ChartType = SeriesChartType.Column;
                qualityWCWiseChart.Series[i]["DrawingStyle"] = "Cylinder";
            }

            qualityWCWiseChart.Visible = true;
           
            qualityWCWiseChart.Series[0].Points.DataBindXY(xVal, shAGood);
            qualityWCWiseChart.Series[1].Points.DataBindXY(xVal, shARej);
            qualityWCWiseChart.Series[2].Points.DataBindXY(xVal, shBGood);
            qualityWCWiseChart.Series[3].Points.DataBindXY(xVal, shBRej);
            qualityWCWiseChart.Series[4].Points.DataBindXY(xVal, shCGood);
            qualityWCWiseChart.Series[5].Points.DataBindXY(xVal, shCRej);
        } 

        #endregion

        #endregion

    }
}