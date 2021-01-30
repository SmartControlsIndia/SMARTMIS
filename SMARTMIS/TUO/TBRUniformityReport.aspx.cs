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
    public partial class TBRUniformityReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable maindt = new DataTable();
        DataTable reportdt = new DataTable();

        string processType, day, month, year, wcIDInQuery = "(", OIDInQuery = "(";
        string duration, getType, getOperator;
        DateTime fromDate, toDate;
       
        DataTable exldt;
        string processName;

        string getfromdate;
        string gettodate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



                reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                DropDownListMonth.SelectedValue = DateTime.Now.ToString("MM");
                DropDownListYear.SelectedValue = DateTime.Now.ToString("yyyy");
            }
        }

        protected void viewReport_Click(object sender, EventArgs e)
        {
            var datetimedt = "";
            string getMonth = DropDownListMonth.SelectedValue;
            string getYear = DropDownListYear.SelectedItem.Text;
            string duration = "";

            string getfromdate = reportMasterFromDateTextBox.Text;
           
            


            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                    toDate = fromDate.AddDays(1);

                    string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                    string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";

                    showReportDateMonthWise(nfromDate, ntoDate, getType);
                    //HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>Curing Production Report</strong></td><td width=12% align=right>Date : " + getDate + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";
                    break;

                //  fromDate = DateTime.Parse(formatDate(getfromdate));

                //  toDate = DateTime.Parse(formatDate(gettodate));

                // string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                //string  ntoDate = toDate.AddDays(1).ToString("dd/MMM/yyyy") + " 07:00:00";



                case "Month":

                    nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                    if (Convert.ToInt32(getMonth) < 12)
                    {
                        datetimedt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                    }
                    else
                    { datetimedt = getYear.ToString() + "-" + (getMonth) + "-31 07:00:00"; }

                    ntoDate = datetimedt;


                    showReportDateMonthWise(nfromDate, ntoDate, getType);
                    // HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=60%><strong>Curing Production Report</strong></td><td width=14% align=right>Month : " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(getMonth)) + " " + getYear + "</td><td width=16% align=right> Type : " + getType.ToString() + "</td></tr></table></div>";

                    break;

                //HeaderText.Text = " &nbsp;&nbsp; &nbsp; MachineType-" + ddlMachineType.SelectedItem.Text + " &nbsp; FromDate-" + txtfrom.Text + " &nbsp; To-" + txtto.Text;
            }
        }
                  
      

        protected void showReportDateMonthWise(string nfromDate, string ntoDate, string type)
        {
            if (validateInput("Month", type, nfromDate, ntoDate, 0, 0, 0))
            {

                showReportDateMonthWcWise(nfromDate, ntoDate);

            }
        }
        
      
        protected void showReportDateMonthWcWise(string fromDate, string toDate)
        {

            try
            {
                
                var datetimedt = "";
                DataTable dt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("WcName", typeof(string));
                gridviewdt.Columns.Add("Recipe Name", typeof(string));
                gridviewdt.Columns.Add("A", typeof(string));
                gridviewdt.Columns.Add("B", typeof(string));
                gridviewdt.Columns.Add("C", typeof(string));
                gridviewdt.Columns.Add("D", typeof(string));
                gridviewdt.Columns.Add("E", typeof(string));
                gridviewdt.Columns.Add("Total", typeof(int));
                gridviewdt.Columns.Add("Total Yield(%)", typeof(decimal));

                
                DataRow dr;
                int total = 0, runoutA = 0, runoutB = 0, runoutC = 0, runoutD = 0, runoutE = 0;

                decimal finalyield = 0;
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (ddlMachineType.SelectedValue == "1")
                {
                    if (ddlshift.SelectedValue == "ALL")
                    {

                        myConnection.comm.CommandText = @"SELECT shd.id, shd.wcId, shd.TBMWcID, dbo.wcmaster.name, dbo.wcMaster.processID,shd.dtandtime, 
shd.RecipeId,shd.barcode,shd.TotalRank,dbo.recipeMaster.name as recipecode,Shift=(CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND 
 convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN 
 convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN 
 ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999') 
 or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END) FROM dbo.tbruniformityData shd
INNER JOIN dbo.recipeMaster ON shd.RecipeId = dbo.recipeMaster.iD 
INNER JOIN dbo.wcMaster ON shd.tbmwcid = dbo.wcMaster.iD where shd.dtandTime >= '" + fromDate + "' AND shd.dtandTime < '" + toDate + "' and dbo.wcMaster.processID='4'  order by dbo.wcmaster.iD asc ";

                    }
                    else {
                        myConnection.comm.CommandText = @"SELECT shd.id, shd.wcId, shd.TBMWcID, dbo.wcmaster.name, dbo.wcMaster.processID,shd.dtandtime, 
shd.RecipeId,shd.barcode,shd.TotalRank,dbo.recipeMaster.name as recipecode,Shift=(CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND 
 convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN 
 convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN 
 ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999') 
 or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END) FROM dbo.tbruniformityData shd
INNER JOIN dbo.recipeMaster ON shd.RecipeId = dbo.recipeMaster.iD 
INNER JOIN dbo.wcMaster ON shd.tbmwcid = dbo.wcMaster.iD where shd.dtandTime >= '" + fromDate + "' AND shd.dtandTime < '" + toDate + "' and dbo.wcMaster.processID='4' and (CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND  convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN  convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN  ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999')  or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END)= '"+ ddlshift.SelectedItem.Text +"' order by dbo.wcmaster.id asc";
                    }
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    var wc_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("TBMWcID"))
                       .Select(g => new
                       {
                           wc_id = g.Key
                       }));
                    var wc_items = wc_query.ToArray();
                    foreach (var wc_item in wc_items)
                    {
                        //ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;
                        var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("recipeCode"))
                            .Select(g => new
                            {
                                tyreType_id = g.Key
                            }));
                        var tyreType_items = tyreType_query.ToArray();
                        foreach (var tyreType_item in tyreType_items)
                        {
                            dr = gridviewdt.NewRow();
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("TBMWcID") == wc_item.wc_id && l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                            {
                                wcname = l.Field<string>("name"),
                                tyreType = l.Field<string>("recipeCode"),
                                RoRank = l.Field<string>("TotalRank"),
                            }).ToArray();

                            if (data.Count() != 0)
                            {

                                total = data.Count();

                                runoutA = data.Count(d => d.RoRank == "A");
                                runoutB = data.Count(d => d.RoRank == "B");
                                runoutC = data.Count(d => d.RoRank == "C");
                                runoutD = data.Count(d => d.RoRank == "D");
                                runoutE = data.Count(d => d.RoRank == "E");
                                finalyield = Convert.ToDecimal(((runoutA) * 100) / total);


                                dr[0] = data[0].wcname;
                                dr[1] = data[0].tyreType;
                                dr[2] = runoutA;
                                dr[3] = runoutB;
                                dr[4] = runoutC;
                                dr[5] = runoutD;
                                dr[6] = runoutE;
                                dr[7] = total;
                                dr[8] = finalyield;

                                gridviewdt.Rows.Add(dr);

                            }
                            //gridviewdt.Rows.Add(dr);

                        }

                        


                    }
                    if (gridviewdt.Rows.Count - 1 <= 0)
                    {
                        lblText.Text = "No Matching Records Found!!";
                        lblText.Visible = true;


                    }
                    else
                    {
                        lblText.Visible = false;
                        dr = gridviewdt.NewRow();
                        dr[0] = "GrandTotal";
                        dr[1] = "Total";

                        int sum = 0, v = 0;
                        for (v = 2; v < gridviewdt.Columns.Count - 1; v++)
                        {
                            sum = 0;
                            for (int j = 0; j < gridviewdt.Rows.Count; j++)
                            {
                                int number = Convert.ToInt32(gridviewdt.Rows[j][v].ToString());
                                sum += number;
                                dr[v] = sum;
                            }
                        }


                        gridviewdt.Rows.Add(dr);


                        int x = gridviewdt.Rows.Count - 1;
                        string Aper = "0";
                        string Bper = "0";
                        string Total = "0";
                        Aper = gridviewdt.Rows[x]["A"].ToString();
                        Bper = gridviewdt.Rows[x]["B"].ToString();
                        Total = gridviewdt.Rows[x]["Total"].ToString();
                        decimal PerYield = (((Convert.ToDecimal(Aper) + Convert.ToDecimal(Bper)) * 100) / Convert.ToDecimal(Total));
                        if (v == 8)
                        {


                            dr[v] = Math.Round((PerYield), 2);
                        }
                        dr = gridviewdt.NewRow();
                        dr[0] = "";
                        dr[1] = "Percentage";

                        decimal per = 0;
                        decimal pernumber = Convert.ToDecimal(gridviewdt.Rows[gridviewdt.Rows.Count - 1][7].ToString());
                        int colValue = 0;
                        for (colValue = 2; colValue < gridviewdt.Columns.Count - 1; colValue++)
                        {

                            decimal colValue1 = Convert.ToDecimal(gridviewdt.Rows[gridviewdt.Rows.Count - 1][colValue].ToString());
                            per = Math.Round(((colValue1 * 100) / pernumber), 2);
                            dr[colValue] = per;


                        }
                        if (colValue == 8)
                        {
                            dr[colValue] = Math.Round((PerYield), 2);
                        }

                        gridviewdt.Rows.Add(dr);


                    }
                }
                else
                {
                    if (ddlshift.SelectedItem.Text == "ALL")
                    {
                        myConnection.comm.CommandText = @"SELECT shd.id, shd.wcId, shd.wcid as TBMWcID, dbo.wcmaster.name, shd.dtandtime, shd.TireId as recipecode,
shd.RecipeId,shd.barcode,shd.TotalRank,Shift=(CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND 
 convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN 
 convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN 
 ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999') 
 or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END)  FROM dbo.tbruniformityData shd 
INNER JOIN dbo.wcMaster ON shd.wcid = dbo.wcMaster.iD where shd.dtandTime >= '" + fromDate + "' AND shd.dtandTime < '" + toDate + "' order by dbo.wcmaster.id asc";
                    }
                    else
                    {
                        // myConnection.comm.CommandText = "select wcId,dtandtime,shift,TotalRank,barcode,name,TBMWcid,recipeCode from vTBRUniformityData where dtandTime >= '" + fromDate + "' AND dtandTime < '" + toDate + "' order by wcid asc";
                        myConnection.comm.CommandText = @"SELECT shd.id, shd.wcId, shd.wcid as TBMWcID, dbo.wcmaster.name, shd.dtandtime, shd.TireId as recipecode,
shd.RecipeId,shd.barcode,shd.TotalRank,Shift=(CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND 
 convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN 
 convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN 
 ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999') 
 or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END)  FROM dbo.tbruniformityData shd 
INNER JOIN dbo.wcMaster ON shd.wcid = dbo.wcMaster.iD where shd.dtandTime >= '" + fromDate + "' AND shd.dtandTime < '" + toDate + "'  and (CASE WHEN convert(char(8), shd.dtandtime, 108) >= '07:00:00 AM' AND  convert(char(8), shd.dtandtime, 108) <= '14:59:59.999' THEN 'A' WHEN  convert(char(8), shd.dtandtime, 108) >= '15:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '22:59:59.999' THEN 'B' WHEN  ((convert(char(8), shd.dtandtime, 108) >= '23:00:00.000' AND convert(char(8), shd.dtandtime, 108) <= '23:59:59.999')  or (convert(char(8), shd.dtandtime, 108) >= '00:00:01.000' AND convert(char(8), shd.dtandtime, 108) <= '06:59:59.999')) THEN 'C' END)= '" + ddlshift.SelectedItem.Text + "' order by dbo.wcmaster.id asc";
                    }
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    var wc_query = (dt.AsEnumerable().GroupBy(l => l.Field<int>("TBMWcID"))
                       .Select(g => new
                       {
                           wc_id = g.Key
                       }));
                    var wc_items = wc_query.ToArray();
                    foreach (var wc_item in wc_items)
                    {
                        //ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;
                        var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("recipeCode"))
                            .Select(g => new
                            {
                                tyreType_id = g.Key
                            }));
                        var tyreType_items = tyreType_query.ToArray();
                        foreach (var tyreType_item in tyreType_items)
                        {
                            dr = gridviewdt.NewRow();
                            var data = dt.AsEnumerable().Where(l => l.Field<int>("TBMWcID") == wc_item.wc_id && l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                            {
                                wcname = l.Field<string>("name"),
                                tyreType = l.Field<string>("recipeCode"),
                                RoRank = l.Field<string>("TotalRank"),
                            }).ToArray();

                            if (data.Count() != 0)
                            {

                                total = data.Count();

                                runoutA = data.Count(d => d.RoRank == "A");
                                runoutB = data.Count(d => d.RoRank == "B");
                                runoutC = data.Count(d => d.RoRank == "C");
                                runoutD = data.Count(d => d.RoRank == "D");
                                runoutE = data.Count(d => d.RoRank == "E");
                                finalyield = Convert.ToDecimal(((runoutA) * 100) / total);


                                dr[0] = data[0].wcname;
                                dr[1] = data[0].tyreType;
                                dr[2] = runoutA;
                                dr[3] = runoutB;
                                dr[4] = runoutC;
                                dr[5] = runoutD;
                                dr[6] = runoutE;
                                dr[7] = total;
                                dr[8] = finalyield;

                                gridviewdt.Rows.Add(dr);

                            }
                            //gridviewdt.Rows.Add(dr);


                        }

                       





                    }
                    if (gridviewdt.Rows.Count - 1 <= 0)
                    {
                        lblText.Text = "No Matching Records Found!!";
                        lblText.Visible = true;


                    }
                    else
                    {
                        lblText.Visible = false;
                        dr = gridviewdt.NewRow();
                        dr[0] = "GrandTotal";
                        dr[1] = "Total";

                        int sum = 0, v = 0;
                        for (v = 2; v < gridviewdt.Columns.Count - 1; v++)
                        {
                            sum = 0;
                            for (int j = 0; j < gridviewdt.Rows.Count; j++)
                            {
                                int number = Convert.ToInt32(gridviewdt.Rows[j][v].ToString());
                                sum += number;
                                dr[v] = sum;
                            }
                        }


                        gridviewdt.Rows.Add(dr);

                        int x = gridviewdt.Rows.Count - 1;
                        string Aper = "0";
                        string Bper = "0";
                        string Total = "0";
                        Aper = gridviewdt.Rows[x]["A"].ToString();
                        Bper = gridviewdt.Rows[x]["B"].ToString();
                        Total = gridviewdt.Rows[x]["Total"].ToString();
                        decimal PerYield = (((Convert.ToDecimal(Aper) + Convert.ToDecimal(Bper)) * 100) / Convert.ToDecimal(Total));
                        if (v == 8)
                        {


                            dr[v] = Math.Round((PerYield), 2);
                        }
                        dr = gridviewdt.NewRow();
                        dr[0] = "";
                        dr[1] = "Percentage";

                        decimal per = 0;
                        decimal pernumber = Convert.ToDecimal(gridviewdt.Rows[gridviewdt.Rows.Count - 1][7].ToString());
                        int colValue = 0;
                        for (colValue = 2; colValue < gridviewdt.Columns.Count - 1; colValue++)
                        {

                            decimal colValue1 = Convert.ToDecimal(gridviewdt.Rows[gridviewdt.Rows.Count - 1][colValue].ToString());
                            per = Math.Round(((colValue1 * 100) / pernumber), 2);
                            dr[colValue] = per;


                        }
                        if (colValue == 8)
                        {
                            dr[colValue] = Math.Round((PerYield), 2);
                        }

                        gridviewdt.Rows.Add(dr);

                    }
                }
                //myConnection.comm.CommandText = "Select wcName, recipeCode, dtandTime from vTbmTBR WHERE wcID IN " + wcIDInQuery.ToString() + " AND dtandTime >= '" + fromDate + "' AND dtandTime < '" + toDate + "' and recipeCode is not null ORDER BY wcID, recipeCode, dtandTime asc";

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                MainGridView.Visible = true;
                //gvpanel.Visible = true;
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "GrandTotal");
                IEnumerable<GridViewRow> rows1 = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[1].Text == "Percentage");
                IEnumerable<GridViewRow> rows2 = MainGridView.Rows.Cast<GridViewRow>()
        .Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;
                foreach (var row2 in rows2)
                    row2.Font.Bold = true;
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
                //exldt.Columns[3].DataType = typeof(Double);
                //exldt.Columns[4].DataType = typeof(Double);
                //exldt.Columns[5].DataType = typeof(Double);
                //exldt.Columns[6].DataType = typeof(Double);
                //exldt.Columns[7].DataType = typeof(Double);
                //exldt.Columns[8].DataType = typeof(Double);

                //exldt.Columns[13].DataType = typeof(Double);
                //exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);




                ViewState["dt"] = exldt;
            }
            catch (Exception exp)
            {
               // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
       
        //public string formatDate(String date)
        //{
        //    string flag = "";

        //    string day, month, year;
        //    if (date != null)
        //    {
        //        string[] tempDate = date.Split(new char[] { '/' });
        //        try
        //        {
        //            if (tempDate.Length > 2)
        //            {
        //            }
        //            else
        //            { tempDate = date.Split(new char[] { '-' }); }
        //            day = tempDate[0].ToString().Trim();
        //            month = tempDate[1].ToString().Trim();
        //            year = tempDate[2].ToString().Trim();
        //            flag = day + "/" + month + "/" + year + " " + "07" + ":" + "00" + ":" + "00";

        //        }
        //        catch (Exception exp)
        //        {
        //            myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //        }
        //    }
        //    return flag;
        //}
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


            // getdisplaytype = optionDropDownList.SelectedItem.Text;

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=UniformityReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Uniformityreport");


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
        //protected void DropDownListDuration_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DropDownListDuration.SelectedValue == "1")
        //    {
        //        pnldate1.Visible = true;
        //        pnlDateRange1.Visible = false;
        //        pnlmonth1.Visible = false;
        //        pnlyearly1.Visible = false;
        //    }
        //    else if (DropDownListDuration.SelectedValue == "2")
        //    {
        //        pnldate1.Visible = false;
        //        pnlDateRange1.Visible = true;
        //        pnlmonth1.Visible = false;
        //        pnlyearly1.Visible = false;
        //    }
        //    else if (DropDownListDuration.SelectedValue == "3")
        //    {
        //        pnldate1.Visible = false;
        //        pnlDateRange1.Visible = false;
        //        pnlmonth1.Visible = true;
        //        pnlyearly1.Visible = false;
        //    }
        //    else if (DropDownListDuration.SelectedValue == "4")
        //    {
        //        pnldate1.Visible = false;
        //        pnlDateRange1.Visible = false;
        //        pnlmonth1.Visible = false;
        //        pnlyearly1.Visible = true;
        //        gvpanel.Visible = true;
        //    }
        //}

        //protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlMachineType.SelectedValue == "1")
        //    {
        //        Divmachine.Visible = true;
        //        DivUniformity.Visible = false;
        //    }
        //    else if (ddlMachineType.SelectedValue == "2")
        //    {
        //        Divmachine.Visible = false;
        //        DivUniformity.Visible = true;

        //    }
        //}

       
    }
}
