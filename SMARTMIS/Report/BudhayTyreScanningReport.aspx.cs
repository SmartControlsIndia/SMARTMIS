using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using OfficeOpenXml;

namespace SmartMIS.Report
{
    public partial class BudhayTyreScanningReport : System.Web.UI.Page
    {
        myConnection myconnection = new myConnection();
       String fromdate, todate;
       string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "BuddhayTyreScanningReport.xlsx";
       string filepath;

       smartMISWebService myWebService = new smartMISWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ShowReport();
                //fromdate = DateTime.Parse(formatDate(fromdatecalendertextbox.Text));

                //todate = DateTime.Parse(formatDate(TodateCalendertextbox.Text)).AddDays(1);
               
            }
        }
        protected void ddlmonthselection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmonthselection.SelectedValue == "Daily")
            {
                pnlshift.Visible = true;
                pnlto.Visible = false;

            }
            else
            {
                pnlshift.Visible = false;
                pnlto.Visible = true;

            }
        }
        private void ShowReport()
        {
            DataTable dt = new DataTable();

            var fromDate =  Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).ToString("yyyy-MM-dd");
            var fromDateto = fromDate;
            var ToDate = "";
            if (ddlmonthselection.SelectedValue == "Daily")
            {

                if (TodateCalendertextbox.Text == "")
                { }
                else
                { ToDate = Convert.ToDateTime(myWebService.formatDate(TodateCalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd"); }

                if (ddlshift.SelectedValue == "1")
                {

                    fromDate = fromDate + " 07:00:00";
                    ToDate = fromDateto + " 15:00:00 ";

                }
                else if (ddlshift.SelectedValue == "2")
                {
                    fromDate = fromDate + " 15:00:00";
                    ToDate = fromDateto + " 23:00:00 ";

                }
                else if (ddlshift.SelectedValue == "3")
                {
                    fromDate = fromDate + " 23:00:00";
                    ToDate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd");
                    ToDate = ToDate + " 07:00:00 ";
                }
                else
                {
                    ToDate = Convert.ToDateTime(myWebService.formatDate(fromDateto)).AddDays(1).ToString("yyyy-MM-dd");
                    fromDate = fromDate + " 07:00:00 ";
                    ToDate = ToDate + " 07:00:00 ";
                }
            }
            else
            {
                ToDate = Convert.ToDateTime(myWebService.formatDate(TodateCalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd");
                fromDate = fromDate + " 07:00:00 ";
                ToDate = ToDate + " 07:00:00 ";
            }

            //todate = Convert.ToDateTime(myWebService.formatDate(TodateCalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd" + " 07:00:00");
            //fromdate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).ToString("yyyy-MM-dd" + " 07:00:00");
           
            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                //myconnection.comm.CommandText = "select count(distinct gtbarcode) as TyreCount, destination,recipeCode  FROM BuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime) FROM BuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and  dtandtime>'" + fromDate + "' AND  dtandtime<'" + ToDate + "'  and stationNo='3'  and destination !='00') and dtandtime>'" + fromDate + "' AND dtandtime<'" + ToDate + "' and destination !='01' group by destination,recipeCode order by destination asc ";
                myconnection.comm.CommandText = "select count(distinct gtbarcode) as TyreCount,destination,isnull(recipeCode,'Unknown')as recipeCode from [BuddeScannedTyreDetail] where dtandtime>='" + fromDate + "' and dtandtime<'" + ToDate + "' and stationNo='3' and destination !='00' and destination !='01' group by destination,recipeCode order by destination asc ";
                myconnection.reader = myconnection.comm.ExecuteReader();
                dt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);
                ViewState["dt"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception exc)
            {
                //mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }

           

        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            ShowReport();

           
            if (GridView1.Rows.Count > 0)
            {
                // Bind your grid here
                lblNoRecord.Visible = false;
            }
            else
            {
                lblNoRecord.Visible = true; // label for "Record Not Found"
            }


        }
        public string formatDate(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    flag = day + "-" + month + "-" + year + " " + "07" + ":" + "00" + ":" + "00";

                }
                catch (Exception exp)
                {
                    //myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }

        protected void excelButton_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BuddheExit.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TBRbuddheExitStationReport");
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
