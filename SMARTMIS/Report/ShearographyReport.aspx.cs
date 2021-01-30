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
    public partial class ShearographyReport : System.Web.UI.Page
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
            if (!Page.IsPostBack)
            {
               

                reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                DropDownListMonth.SelectedValue = DateTime.Now.ToString("MM");
                DropDownListYear.SelectedValue = DateTime.Now.ToString("yyyy");
                
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
        public bool validateInput(string duration, string fromDate, string toDate, int month, int year, int yearwise)
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


            // getdisplaytype = optionDropDownList.SelectedItem.Text;

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=shreography.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("shreographyData");
            ws.Cells["A1"].Value = "ShreographyReport";

            //" &nbsp;&nbsp; Date- " + txtdate.Text + " &nbsp; Recipe-" + ddlsizewise.SelectedItem.Text + " &nbsp; Machine-" + ddlMachinewise.SelectedItem.Text;
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
        protected void viewReport_Click(object sender, EventArgs e)
        {
            string getMonth = DropDownListMonth.SelectedValue;
            string getYear = DropDownListYear.SelectedItem.Text;
            string duration = "";

            string getfromdate = reportMasterFromDateTextBox.Text;
            var datetimedt = "";
            switch (DropDownListDuration.SelectedItem.Value)
            {
                    case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = fromDate.ToString("dd-MMM-yyyy") + " 07:00:00";
                        string ntoDate = toDate.ToString("dd-MMM-yyyy") + " 07:00:00";

                        showReportMonthWise(nfromDate, ntoDate);
                        //HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>Curing Production Report</strong></td><td width=12% align=right>Date : " + getDate + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
                        break;
                    case "Month":

                         nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                        if ( Convert.ToInt32( getMonth) < 12)
                        {
                            datetimedt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                        }
                        else
                        { datetimedt = (Convert.ToInt32(getYear)+1).ToString() + "-" +"01"+"-01 07:00:00"; }

                         ntoDate = datetimedt;
                         

                        showReportMonthWise( nfromDate,  ntoDate);
                       // HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=60%><strong>Curing Production Report</strong></td><td width=14% align=right>Month : " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(getMonth)) + " " + getYear + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";

                        break;
                        //ScriptManager.RegisterClientScriptBlock(UpdatePanelProcess, this.GetType(), "myScript", "showDuration('" + duration + "','hideWCIDDiv')", true);
                   
                }
           
            //HeaderText.Text = " &nbsp;&nbsp; &nbsp; MachineType-" + ddlMachineType.SelectedItem.Text + " &nbsp; FromDate-" + txtfrom.Text + " &nbsp; To-" + txtto.Text;
        }



        protected void showReportMonthWise(string nfromDate, string ntoDate)
        {
            if (validateInput("Month",  nfromDate, ntoDate, 0, 0, 0))
            {

                showReportMonthWcWise(nfromDate, ntoDate);

            }
        }


        protected void showReportMonthWcWise(string fromDate, string toDate)
        {

            try
            {

                var datetimedt = "";
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();


                gridviewdt.Columns.Add("Date", typeof(string));
                gridviewdt.Columns.Add("Shift", typeof(string));
                gridviewdt.Columns.Add("Recipe Name", typeof(string));
                gridviewdt.Columns.Add("Total", typeof(int));
                gridviewdt.Columns.Add("A", typeof(string));
                gridviewdt.Columns.Add("A%", typeof(decimal));
                gridviewdt.Columns.Add("B", typeof(string));
                gridviewdt.Columns.Add("B%", typeof(decimal));
                gridviewdt.Columns.Add("C", typeof(string));
                gridviewdt.Columns.Add("C%", typeof(decimal));
                gridviewdt.Columns.Add("Overall Yield(%)", typeof(decimal));


                DataRow dr;
                int total = 0, runoutA = 0, runoutB = 0, runoutC = 0;

                decimal finalyield = 0, finalyieldA = 0, finalyieldB = 0, finalyieldC = 0;
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "SELECT id,CONVERT(VARCHAR(15),dtandtime,106) as  sDate,name,shift,barcode,Grade FROM vShearographyData where dtandTime >= '" + fromDate + "' AND dtandTime < '" + toDate + "' and Grade <>''   order by sDate asc ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                if (dt.Columns.Count > 0)
                {
                    var wc_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("sDate"))
                       .Select(g => new
                       {
                           wc_id = g.Key
                       }));
                    var wc_items = wc_query.ToArray();
                    foreach (var wc_item in wc_items)
                    {
                        //ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;


                        dr = gridviewdt.NewRow();
                        var data = dt.AsEnumerable().Where(l => l.Field<string>("sDate") == wc_item.wc_id).Select(l => new
                        {

                            tyreType = l.Field<string>("name"),
                            d_date = l.Field<string>("sDate"),
                            shift = l.Field<string>("shift"),
                            RoRank = l.Field<string>("Grade"),
                        }).ToArray();

                        if (data.Count() != 0)
                        {

                            total = data.Count();

                            runoutA = data.Count(d => d.RoRank == "A");
                            finalyieldA = Convert.ToDecimal(((runoutA) * 100) / total);
                            runoutB = data.Count(d => d.RoRank == "B");
                            finalyieldB = Convert.ToDecimal(((runoutB) * 100) / total);
                            runoutC = data.Count(d => d.RoRank == "C");
                            finalyieldC = Convert.ToDecimal(((runoutC) * 100) / total);
                            finalyield = Convert.ToDecimal(((runoutA) * 100) / total);



                            dr[0] = data[0].d_date; //
                            dr[1] = data[0].shift;
                            dr[2] = data[0].tyreType;
                            dr[3] = total;
                            dr[4] = runoutA;
                            dr[5] = finalyieldA;
                            dr[6] = runoutB;
                            dr[7] = finalyieldB;
                            dr[8] = runoutC;
                            dr[9] = finalyieldC;
                            dr[10] = finalyield;

                            gridviewdt.Rows.Add(dr);


                            //gridviewdt.Rows.Add(dr);

                        }



                    }

                    dr = gridviewdt.NewRow();
                    dr[0] = "GrandTotal";
                    dr[1] = "";
                    dr[2] = "";

                    int sum = 0, v = 0;
                    decimal sum1 = 0, sum2 = 0, sum01 = 0;
                    for (v = 3; v < gridviewdt.Columns.Count - 1; v++)
                    {
                        sum = 0;
                        for (int j = 0; j < gridviewdt.Rows.Count; j++)
                        {
                            int number = Convert.ToInt32(gridviewdt.Rows[j][v].ToString());
                            sum += number;
                            dr[v] = sum;

                        }
                        if (v == 5)
                        {

                            sum1 = ((Convert.ToDecimal(dr[4]) * 100) / Convert.ToDecimal(dr[3]));
                            dr[5] = Math.Round(sum1,2);
                            dr[10] = Math.Round(sum1, 2); 
                        }
                        if (v == 7)
                        {
                            sum2 = ((Convert.ToDecimal(dr[6]) * 100) / Convert.ToDecimal(dr[3]));
                            dr[7] = Math.Round(sum2,2);
                        }
                        if (v == 9)
                        {
                            sum01 = ((Convert.ToDecimal(dr[8]) * 100) / Convert.ToDecimal(dr[3]));
                            dr[9] = Math.Round(sum01,2);
                        }
                      

                    }


                    gridviewdt.Rows.Add(dr);
                    //dr = gridviewdt.NewRow();
                    //int x = gridviewdt.Rows.Count - 1;
                    //dr[0] = "Percentage";
                    //string Aper = "0";
                    //string Bper = "0";
                    //string Cper = "0";
                    //string Total = "0";
                    //Aper = gridviewdt.Rows[x]["A"].ToString();
                    //Bper = gridviewdt.Rows[x]["B"].ToString();
                    //Cper = gridviewdt.Rows[x]["C"].ToString();
                    //Total = gridviewdt.Rows[x]["Total"].ToString();
                    //decimal PerYield = (((Convert.ToDecimal(Aper)) * 100) / Convert.ToDecimal(Total));
                    //dr[5] = Math.Round((PerYield), 2);
                    //decimal PerYield7 = (((Convert.ToDecimal(Bper)) * 100) / Convert.ToDecimal(Total));
                    //dr[7] = Math.Round((PerYield7), 2);
                    //decimal PerYield9 = (((Convert.ToDecimal(Cper)) * 100) / Convert.ToDecimal(Total));
                    //dr[9] = Math.Round((PerYield9), 2);
                    //dr[10] = Math.Round((PerYield), 2);

                    //gridviewdt.Rows.Add(dr);


                    MainGridView.DataSource = gridviewdt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    //gvpanel.Visible = true;
                    IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "GrandTotal");
                    IEnumerable<GridViewRow> rows1 = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "Percentage");


                    foreach (var row in rows)
                        row.Font.Bold = true;

                    foreach (var row1 in rows1)
                        row1.Font.Bold = true;

                    if (gridviewdt.Rows.Count > 0)
                    {
                        MainGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    MainGridView.Rows[MainGridView.Rows.Count - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#E0E0E0");




                    //Excel Datatable
                    //Created:2-12-2016
                    //Sarita
                    exldt = gridviewdt.Copy();
                    // exldt = gridviewdt.Copy();
                    exldt.Columns[0].DataType = typeof(string);
                    exldt.Columns[1].DataType = typeof(string);
                    exldt.Columns[2].DataType = typeof(string);

                    ViewState["dt"] = exldt;
                }
            }
            catch (Exception exp)
            {
                // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
           
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell cell = new TableHeaderCell();

                    
                            cell.Text = "";
                            cell.ColumnSpan = 3;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.Text = "A";
                            cell.ColumnSpan = 2;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 2;
                            cell.Text = "B";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 2;
                            cell.Text = "C";
                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 1;
                            cell.Text = "";
                            row.Controls.Add(cell);
                           

                    
                    MainGridView.HeaderRow.Parent.Controls.AddAt(0, row);

                   
            }
      
        }
    }

