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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace SmartMIS.Report
{
    public partial class ShreoDefectReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable maindt = new DataTable();
        DataTable reportdt = new DataTable();

        string processType, day, month, year, wcIDInQuery = "(", OIDInQuery = "(";
        string duration, getType, getOperator;
        DateTime fromDate, toDate;
        ArrayList tempwcname;
        DataTable exldt;
        string processName;

        string getfromdate;
        string gettodate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



                fromdatecalendertextbox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                TodateCalendertextbox.Text = DateTime.Now.AddDays(1).ToString("dd-MMM-yyyy");

                // viewReport_Click(sender, e);

            }
        }
        protected void viewReport_Click(object sender, EventArgs e)
        {
            
            string getfromdate = fromdatecalendertextbox.Text;
            string gettodate = TodateCalendertextbox.Text;
            string duration = "";

            fromDate = DateTime.Parse(formatDate(getfromdate));

            toDate = DateTime.Parse(formatDate(gettodate));

            string nfromDate = fromDate.ToString("dd-MMM-yyyy") + " 07:00:00";
            string ntoDate = toDate.AddDays(1).ToString("dd-MMM-yyyy") + " 07:00:00";


            showReportDateMonthWise(nfromDate, ntoDate);
            //HeaderText.Text = " &nbsp;&nbsp; &nbsp; MachineType-" + ddlMachineType.SelectedItem.Text + " &nbsp; FromDate-" + txtfrom.Text + " &nbsp; To-" + txtto.Text;
        }



       

        protected void showReportDateMonthWise(string fromDate, string toDate)
        {

            try
            {

                var datetimedt = "";
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("Date", typeof(string));
                gridviewdt.Columns.Add("Shift", typeof(string));
                gridviewdt.Columns.Add("Recipe Name", typeof(string));
                gridviewdt.Columns.Add("Barcode", typeof(int));
                gridviewdt.Columns.Add("Pressno", typeof(string));
                gridviewdt.Columns.Add("Cured date/shift", typeof(decimal));
                gridviewdt.Columns.Add("TBM", typeof(string));
                gridviewdt.Columns.Add("Builder no.", typeof(decimal));
                gridviewdt.Columns.Add("Build date/shift", typeof(string));
                gridviewdt.Columns.Add("Grade", typeof(string));
              


                DataRow dr;
                int total = 0, runoutA = 0, runoutB = 0, runoutC = 0, runoutD = 0, runoutE = 0;

               
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();


                myConnection.comm.CommandText = @"select CONVERT(VARCHAR(15),shd.dtandtime,106) as  Date,
 Shift=(CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND 
 convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN 
 convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN 
 ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999') 
 or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END) 
 ,RM.name as TyreSize,shd.barcode as Barcode,ctbr.mouldNo as MouldNo,WM.name as PressName,ctbr.dtandTime as CureDate,WM1.name as TBM,MRM.firstName as BuilderName,ttbr.dtandtime as TBMDate,shd.Grade
 from dbo.ShearographyData shd
  
  join recipeMaster RM on RM.id=shd.RecipeID 
  left outer join curingtbr ctbr on ctbr.gtbarCode=shd.barCode
  inner join wcmaster WM on WM.id=ctbr.wcID

   left outer join tbmtbr ttbr on ttbr.gtbarCode=shd.barCode
     inner join wcmaster WM1 on WM1.id=ttbr.wcID
   inner join manningMaster MRM on MRM.id=ttbr.manningID  where shd.dtandTime >= '" + fromDate + "' AND shd.dtandTime < '" + toDate + "'  ";

                    myConnection.comm.CommandTimeout = 180;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    MainGridView.DataSource = dt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    //gvpanel.Visible = true;
                    IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "GrandTotal");
                   
                    IEnumerable<GridViewRow> rows2 = MainGridView.Rows.Cast<GridViewRow>()
            .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;
                foreach (var row2 in rows2)
                    row2.Font.Bold = true;
                

                if (gridviewdt.Rows.Count > 0)
                {
                    MainGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                MainGridView.Rows[MainGridView.Rows.Count - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#E0E0E0");




              
                exldt = dt.Copy();
              
                exldt.Columns[0].DataType = typeof(string);
                exldt.Columns[1].DataType = typeof(string);
                exldt.Columns[2].DataType = typeof(string);
               



                ViewState["dt"] = exldt;
            }
            catch (Exception exp)
            {
                // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex == 0)
                        e.Row.Style.Add("height", "50px");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = MainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
                        for (int cellCount = 0; cellCount < 1 /*gvRow.Cells.Count*/; cellCount++)
                        {
                            if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                            {
                                if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                                {
                                    gvRow.Cells[cellCount].RowSpan = 2;
                                }
                                else
                                {
                                    gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                                }
                                gvPreviousRow.Cells[cellCount].Visible = false;
                            }
                        }
                    }
                    //MainGridView.Rows[MainGridView.Rows.Count - 1].BackColor = Color.Aqua;
                }
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
                string durationQuery = "";
                var tdqte = "";
                durationQuery += "(dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";

            }
            catch (Exception exp)
            {
                // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=shreography.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("shreography");

            ws.Cells["A1"].Value = "ShreographyDefect Report";
           
            using (ExcelRange r = ws.Cells["A1:I1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }


            ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();

        }
    }
}
