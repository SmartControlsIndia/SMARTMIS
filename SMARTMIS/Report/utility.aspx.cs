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

namespace SmartMIS
{
    public partial class utilityReport : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void bindGridView()
        {
            //SqlCommand cmd = new SqlCommand("Select * from vroleMaster", new SqlConnection(conString));
            //cmd.Connection.Open();

            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);

            //roleGridView.DataSource = ds;
            //roleGridView.DataBind();

            //cmd.Connection.Close();

        }

        public void generatechart()
        {
           
            Chart1.Width = 700;
            Chart1.Height = 557;
            Chart1.Titles.Add("Utility Report");
            Chart1.Series.Add("Series1");
            Chart1.Series.Add("Series2");
            Chart1.Series.Add("Series3");
            Chart1.Series.Add("Series4");
            Chart1.Series.Add("Series5");

            Chart1.Series[0].ChartType = SeriesChartType.Bar;
            Chart1.Series[1].ChartType = SeriesChartType.Bar;
            Chart1.Series[2].ChartType = SeriesChartType.Bar;
            Chart1.Series[3].ChartType = SeriesChartType.Bar;
            Chart1.Series[4].ChartType = SeriesChartType.Bar;
            Chart1.BorderlineWidth = 2;
            //Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
            //Chart1.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
           // Chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 10;

            Chart1.Series["Series1"].YValueMembers = "20";
            Chart1.Series["Series2"].YValueMembers = "30";
            Chart1.Series["Series3"].YValueMembers = "40";
            Chart1.Series["Series4"].YValueMembers = "60";
            Chart1.Series["Series5"].YValueMembers = "10";
         
            Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;           
            Chart1.Series["Series1"].IsValueShownAsLabel = true;
            Chart1.Series["Series1"].BorderWidth = 2;
            Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;          
            Chart1.Series["Series1"].MarkerSize = 7;
            Chart1.Series["Series1"].LegendText = "Good";
            Chart1.Series["Series1"].IsVisibleInLegend = true;

           
            //Chart1.Series["Series2"].Points.AddY(90);
            //Chart1.Series["Series2"].Points.AddY(97);
            //Chart1.Series["Series2"].Points.AddY(45);
            //Chart1.Series["Series2"].Points.AddY(87);
            //Chart1.Series["Series2"].Points.AddY(33);
            //Chart1.Series["Series2"].Points.AddY(71);
          
            Chart1.Series["Series2"].MarkerStyle = MarkerStyle.Circle;            
            Chart1.Series["Series2"].IsValueShownAsLabel = true;
            Chart1.Series["Series2"].BorderWidth = 2;
            Chart1.Series["Series2"].MarkerStyle = MarkerStyle.Circle;
            Chart1.Series["Series2"].LabelBackColor = Chart1.Series["Series2"].Color;
            Chart1.Series["Series2"].MarkerSize = 7;
            Chart1.Series["Series2"].LegendText = "Rejected";
            Chart1.Series["Series2"].IsVisibleInLegend = true;
        }
    }
}
