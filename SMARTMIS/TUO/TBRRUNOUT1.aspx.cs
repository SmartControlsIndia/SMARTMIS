using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Drawing;


namespace SmartMIS.TUO
{
    public partial class TBRRUNOUT1 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable mainGVdt;
        DataTable dynamicDB = new DataTable();
        DataTable weightdt;

        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign = null;
        public int totalcheckedcount = 0, okcount = 0, NotOkCount = 0, reworkcount = 0, scrapcount = 0, majorokcount = 0, minorbuffcount = 0, majorholdcount = 0, minorCount = 0, majorCount = 0, majorscrapcount = 0, totalokcount = 0, totalbuffcount = 0, totalscrapcount = 0, totalholdcount = 0, totalminorcount = 0, min_count = 0;
        string sqlquery = "";
        int status;
        DateTime fromDate, toDate;
        string wherequery = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }

        }

        private void createGridView(GridView gridview)
        {
            gridview.Columns.Clear();
            //Iterate through the columns of the datatable to set the data bound field dynamically.
            foreach (DataColumn col in gridview.Columns)
            {
                //Declare the bound field and allocate memory for the bound field.
                BoundField bfield = new BoundField();

                //Initalize the DataField value.
                bfield.DataField = col.ColumnName;

                //Initialize the HeaderText field value.
                bfield.HeaderText = col.ColumnName;

                //Add the newly created bound field to the GridView.
                gridview.Columns.Add(bfield);

            }

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
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                string getMonth = DropDownListMonth.SelectedValue;
                string getYear = DropDownListYear.SelectedItem.Text;
                string getyearwise = DropDownList2.SelectedItem.Text;
                string recipe = "";
                string tyredesign = "";
                string duration = "";
                var datetimebt = "";
                string getfromdate = reportMasterFromDateTextBox.Text; ;

                switch (DropDownListDuration.SelectedItem.Value)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = formatDate(reportMasterFromDateTextBox.Text);//fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                        toDate = DateTime.Parse(nfromDate);
                        string ntoDate = toDate.AddDays(1).ToString();
                        //string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);
                        break;


                    case "Month":
                        nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                        if (Convert.ToInt32(getMonth) < 12)
                        {
                            datetimebt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                        }
                        else
                        { datetimebt = getYear.ToString() + "-" + (getMonth) + "-31 07:00:00"; }

                        ntoDate = datetimebt;
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);

                        break;

                    case "DateFrom":

                        nfromDate = formatDate(tuoReportMasterFromDateTextBox.Text);

                        toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                        ntoDate = toDate.AddDays(1).ToString();
                        TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                        int result = (int)ts.TotalDays;
                        if ((int)ts.TotalDays > 7)
                        {
                            ShowWarning.Visible = true;
                            ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 7 days!!!</font></strong></td></tr></table>";
                        }

                        else
                        {
                            showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign); ;
                        }
                        break;

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        protected void showReportDateMonthWise(string nfromDate, string ntoDate, string recipe, string tyredesign)
        {

            try
            {
                DataTable dtTUO = new DataTable();
                DataTable curdt = new DataTable();
                DataTable tbmdt = new DataTable();
                DataTable manndt = new DataTable();
                DataTable wcdt = new DataTable();
                DataTable maindt = new DataTable();
                DataTable tro1dt = new DataTable();
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = "Select name as MachineName ,dtandTime, recipecode AS TyreType,BARCODE,TotalRank, RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where (dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " ";
                        myConnection.comm.CommandText = "SELECT WCID,(select name from wcmaster where id='261') as WCNAME,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],RECIPENO,RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT ,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE ,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE ,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLERANK ,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE ,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT ,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT ,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT ,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT ,LROTDENTANGLE ,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK ,ROTOTALRANK ,MEASPRESSURE  FROM tbrrunoutData where dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "' and wcID='261'  order by DATE,TIME asc";
                        myConnection.comm.CommandTimeout = 60;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dtTUO.Load(myConnection.reader);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    catch (Exception exc)
                    {
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }
                    

                    MainGridView.DataSource = dtTUO;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    ViewState["xmldt"] = null;
                    ViewState.Remove("xmldt");
                    ViewState["xmldt"] = dtTUO;
                   
                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["xmldt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=tbrRUNOUTReportRAW.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("RUNOUTReport");
            ws.Cells["A1"].Value = "RUNOUTReport ";

            using (ExcelRange r = ws.Cells["A1:AG1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }


            ws.Cells["A3"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();


            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();

           

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
