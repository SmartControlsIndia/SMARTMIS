using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using OfficeOpenXml;

namespace SmartMIS.Report
{
    public partial class classificationReport2 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable dt = new DataTable();
                
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "TBRVisualInspectionClassifiationReport.xlsx";
        string filepath;
        public int total_checked = 0, ok = 0, rework = 0, downgrade = 0, waiting_for_disposal = 0, scrap = 0;
        
        public classificationReport2()
        {
            filepath = myWebService.getExcelPath();

            dt.Columns.Add("wcID", typeof(int));
            dt.Columns.Add("curingRecipeID", typeof(int));
            dt.Columns.Add("status", typeof(int));
            dt.Columns.Add("defectid", typeof(int));
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("wc_name", typeof(int));
            dt.Columns.Add("recipe_id", typeof(int));
            dt.Columns.Add("recipe_name", typeof(string));
            dt.Columns.Add("recipe_description", typeof(string));
        }

        public string rToDate, rFromDate, percent_sign=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tuoReportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
            {
                tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                tuoReportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy"); 
                
                /*string showToDate = "";
                int month = DateTime.Now.Month, year = DateTime.Now.Year;

                if (DateTime.Now.Month == 12 && DateTime.Now.Day == 31)
                    showToDate = "01-01" + "-" + (DateTime.Now.Year + 1);
                else if (DateTime.Now.Day == 31 && (DateTime.Now.Month == 1 || DateTime.Now.Month == 3 || DateTime.Now.Month == 5 || DateTime.Now.Month == 7 || DateTime.Now.Month == 8 || DateTime.Now.Month == 10))
                    showToDate = "01-" + checkDigit((DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else if (DateTime.Now.Day == 30 && (DateTime.Now.Month == 4 || DateTime.Now.Month == 6 || DateTime.Now.Month == 9 || DateTime.Now.Month == 11))
                    showToDate = "01-" + (checkDigit(DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else if (DateTime.Now.Month == 2)
                    showToDate = "01-" + checkDigit((DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else
                    showToDate = checkDigit((DateTime.Now.Day + 1)) + "-" + checkDigit(DateTime.Now.Month) + "-" + DateTime.Now.Year;
                
                tuoReportMasterToDateTextBox.Text = showToDate.ToString();
                */
            }
        }
        /*protected string checkDigit(int digit)
        {
            string str = "";
            if (digit.ToString().Length == 1)
                str = "0" + digit;
            else
                str = digit.ToString();
            return str;
        }*/
        public int totalcheckedcount = 0, okcount = 0, reworkcount = 0, downgradecount = 0, waitingfordisposal = 0, scrapereport = 0;
        public string getdisplaytype = null;
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            showdata();
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    
                    for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = MainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
                        for (int cellCount = 0; cellCount < 2 /*gvRow.Cells.Count*/; cellCount++)
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
        protected void displayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            showdata();
        }
        public string formatfromDate(String date)
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
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (Convert.ToInt32(month) == 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-01-" + (Convert.ToInt32(year) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    else if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }

        public void showdata()
        {
            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));

            loadData();

            DataTable gridviewdt = new DataTable();
            DataRow drt;
            gridviewdt.Columns.Add("Tyre_Type", typeof(string));
            gridviewdt.Columns.Add("Total_Checked", typeof(string));
            gridviewdt.Columns.Add("OK", typeof(string));
            gridviewdt.Columns.Add("Rework", typeof(string));
            gridviewdt.Columns.Add("Downgrade", typeof(string));
            gridviewdt.Columns.Add("Waiting_for_Disposal", typeof(string));
            gridviewdt.Columns.Add("Scrap", typeof(string));

            createGridView(gridviewdt, MainGridView);

            var query = (dt.AsEnumerable().GroupBy(l => l.Field<int?>("curingRecipeID"))
                    .Select(g => new
                    {
                        curing_id = g.Key
                    }));
            int num_of_curing = query.Count();
            var items = query.ToArray();

            switch(getdisplaytype)
            {
                case "Numbers":
                    foreach (var item in items)
                    {
                        var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                        {
                            description = l.Field<string>("recipe_description"),
                            status = l.Field<int?>("status")
                        }).ToArray();
                        if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();

                                total_checked += data.Count();
                                ok += data.Count(d => d.status == 31);
                                rework += data.Count(d => d.status == 32);
                                downgrade += data.Count(d => d.status == 33);
                                waiting_for_disposal += data.Count(d => d.status == 35);
                                scrap += data.Count(d => d.status == 34);


                                dr[0] = data[0].description;
                                dr[1] = data.Count();
                                dr[2] = data.Count(d => d.status == 31);
                                dr[3] = data.Count(d => d.status == 32);
                                dr[4] = data.Count(d => d.status == 33);
                                dr[5] = data.Count(d => d.status == 35);
                                dr[6] = data.Count(d => d.status == 34);
                                gridviewdt.Rows.Add(dr);
                            }
                       
                    }
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[1] = total_checked;
                    drt[2] = ok;
                    drt[3] = rework;
                    drt[4] = downgrade;
                    drt[5] = waiting_for_disposal;
                    drt[6] = scrap;
                    gridviewdt.Rows.Add(drt);
                    break;
                case "Percent":
                    foreach (var item in items)
                    {
                        var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                        {
                            description = l.Field<string>("recipe_description"),
                            status = l.Field<int?>("status")
                        }).ToArray();
                        if (data.Count() != 0)
                        {
                                DataRow dr = gridviewdt.NewRow();

                                total_checked += data.Count();
                                ok += data.Count(d => d.status == 31);
                                rework += data.Count(d => d.status == 32);
                                downgrade += data.Count(d => d.status == 33);
                                waiting_for_disposal += data.Count(d => d.status == 35);
                                scrap += data.Count(d => d.status == 34);

                                dr[0] = data[0].description;
                                dr[1] = data.Count();
                                dr[2] = (data.Count(d => d.status == 31) * 100 / Convert.ToInt32(dr[1])) + "%";
                                dr[3] = (data.Count(d => d.status == 32) * 100 / Convert.ToInt32(dr[1])) + "%";
                                dr[4] = (data.Count(d => d.status == 33) * 100 / Convert.ToInt32(dr[1])) + "%";
                                dr[5] = (data.Count(d => d.status == 35) * 100 / Convert.ToInt32(dr[1])) + "%";
                                dr[6] = (data.Count(d => d.status == 34) * 100 / Convert.ToInt32(dr[1])) + "%";

                                gridviewdt.Rows.Add(dr);
                            }
                    }
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[1] = total_checked;
                    drt[2] = (ok * 100 / total_checked) + "%";
                    drt[3] = (rework * 100 / total_checked) + "%";
                    drt[4] = (downgrade * 100 / total_checked) + "%";
                    drt[5] = (waiting_for_disposal * 100 / total_checked) + "%";
                    drt[6] = (scrap * 100 / total_checked) + "%";
                    gridviewdt.Rows.Add(drt);
                    break;
            }

            MainGridView.DataSource = gridviewdt;
            MainGridView.DataBind();

            // Select the rows where showing total & make them bold
            IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
.Where(row => row.Cells[0].Text == "Total");

            foreach (var row in rows)
                row.Font.Bold = true;

        }
        private void loadData()
        {
            DataTable dt_vi = new DataTable();
            DataTable dt_workcenter = new DataTable();
            DataTable dt_recipe = new DataTable();
            DataTable dt_defect = new DataTable();

            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

           // myConnection.comm.CommandText = "select distinct wcID, curingRecipeID, status, defectID from TBRVisualInspection where  dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcID IN ('244')"; //93
            myConnection.comm.CommandText = @"	SELECT  *
FROM (	select wcID, curingRecipeID, status, defectID, 
 ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY wcID) AS RowNumber
from TBRVisualInspection 
where  dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcID IN ('244','245')) As a Where a.RowNumber=1";
            
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_vi.Load(myConnection.reader);
            dt_vi.Columns.RemoveAt(4);


            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select Distinct iD, name AS wc_name from wcMaster where name IN ('7010','7011')";
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_workcenter.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select iD AS recipe_id, name as recipe_name, description AS recipe_description from recipeMaster";
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_recipe.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            var row = from r0w1 in dt_vi.AsEnumerable()
                      join r0w2 in dt_workcenter.AsEnumerable()
                        on r0w1.Field<int>("wcID") equals r0w2.Field<int>("iD")
                      join r0w3 in dt_recipe.AsEnumerable()
                        on r0w1.Field<int?>("curingRecipeID") equals r0w3.Field<int?>("recipe_id")
                      select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3.ItemArray)).ToArray();

            foreach (object[] values in row)
                dt.Rows.Add(values);
        }
        private void createGridView(DataTable gridviewdt, GridView gridview)
        {
            gridview.Columns.Clear();
            //Iterate through the columns of the datatable to set the data bound field dynamically.
            foreach (DataColumn col in gridviewdt.Columns)
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
        
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));

                DataTable dt = new DataTable();
                DataTable curdt = new DataTable();

                DataTable gridviewdt = new DataTable();

                string[] head_array = new string[] { "S. No.", "Press No.", "Mould No.", "Shift", "TyreSize", "VI WorkCenterName", "Building Date", "Building Time", "Builder Name", "Barcode", "Defect", "Disposal", "Remark", "Responsibility" };
                foreach (var arr in head_array)
                    gridviewdt.Columns.Add(arr, typeof(string));

                createGridView(gridviewdt, ExcelGridView);

              //  string query = @"select distinct
                  //  shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),
                //    description, wcName, CAST(dtandTime AS DATE) AS getdate, convert(char(8), dtandTime, 108) AS gettime, firstName + ' ' + lastName As builderName, gtbarcode, defectName, defectstatusName, remarks
                 //   from vTBRVisualInspectionReport where dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcName = '7010'";

                string query = @"	SELECT  *
FROM (	select   shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),
                    description, wcName, CAST(dtandTime AS DATE) AS getdate, convert(char(8), dtandTime, 108) AS gettime, firstName + ' ' + lastName As builderName,gtbarcode, defectName, defectstatusName, remarks,
					 ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber
                    from vTBRVisualInspectionReport  where dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND  wcName in('7010','7011') )As a Where a.RowNumber=1";


                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();


                foreach (DataRow dtRow in dt.Rows)
                {
                        if(dtRow["wcName"].ToString()=="7011")
                        {
                            dtRow["defectName"] = "";
                        }
                }

                if (dt.Rows.Count != 0)
                {
                    string InQuery = "('" + string.Join("','", (string[])dt.AsEnumerable().Select(x => x.Field<string>("gtbarcode").ToString()).ToArray()) + "')"; // InQuery.TrimEnd(',');

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT gtbarCode AS cur_gtbarcode, wcName AS pressno, mouldNo FROM vCuringtbr WHERE gtbarCode IN " + InQuery.ToString(), con);
                    var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    curdt.Load(dread);

                    con.Close();
                    cmd.Dispose();
                    dread.Close();

                    int serial_number = 1;
                    var row = from r0w1 in dt.AsEnumerable()
                              join r0w2 in curdt.AsEnumerable()
                                on r0w1.Field<string>("gtbarcode") equals r0w2.Field<string>("cur_gtbarcode") into ps
                              from r0w2 in ps.DefaultIfEmpty()
                              select new string[] { serial_number++.ToString() }.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "" }).Concat(r0w1.ItemArray).ToArray();

                    foreach (object[] values in row)
                        gridviewdt.Rows.Add(values);

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + filepath + fileName);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    ExcelPackage pck = new ExcelPackage();
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                    ws.Cells["A1"].LoadFromDataTable(gridviewdt, true, OfficeOpenXml.Table.TableStyles.Medium2);
                    ws.Cells.AutoFitColumns();
                    var ms = new MemoryStream();
                    pck.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);


                    Response.Flush();
                    Response.End();

                    /*ExcelGridView.DataSource = gridviewdt;
                    ExcelGridView.DataBind();

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + filepath + fileName);
                    Response.ContentType = "application/vnd.ms-excel";

                    StringWriter stringWrite = new StringWriter();
                    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    ExcelPanel.RenderControl(htmlWrite);

                    Response.Write(stringWrite.ToString());

                    Response.Flush();
                    Response.End();*/
                }                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }        
    }   
}
