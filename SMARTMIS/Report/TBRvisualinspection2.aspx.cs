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
    public partial class TBRvisualinspection2 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery;
        public string recipeCode, globalrecipecode,percent_sign = null;
        public int total_checked, buff = 0, rework = 0, ncmr = 0, camoflauge = 0;
        myConnection myConnection = new myConnection();
        DataTable dt = new DataTable();               
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "SecondTBRVisualInspectionReport.xls";
        string filepath;
        public TBRvisualinspection2()
        {
            filepath = myWebService.getExcelPath();

            dt.Columns.Add("wcID", typeof(int));
            dt.Columns.Add("curingRecipeID", typeof(int));
            dt.Columns.Add("status", typeof(int));
            dt.Columns.Add("defectid", typeof(int));
            dt.Columns.Add("ssORnss", typeof(string));
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("wc_name", typeof(int));
            dt.Columns.Add("recipe_id", typeof(int));
            dt.Columns.Add("recipe_name", typeof(string));
            dt.Columns.Add("recipe_description", typeof(string));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(date);
                    flag = tempDate.ToString("MM-dd-yyyy");
                    flag = flag + " " + "07:00:00";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
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
        public string getdisplaytype = "Numbers";
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            showdata();
        }
        protected void displayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            showdata();
        }
        public void showdata()
        {
            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));
           
            loadData();
            DataRow drt;
            DataTable gridviewdt = new DataTable();

            gridviewdt.Columns.Add("Tyre_Type", typeof(string));
            gridviewdt.Columns.Add("SS_OR_NSS", typeof(string));
            gridviewdt.Columns.Add("Total_Checked", typeof(string));
            gridviewdt.Columns.Add("Buff", typeof(string));
            gridviewdt.Columns.Add("Repair", typeof(string));
            gridviewdt.Columns.Add("NCMR", typeof(string));
            gridviewdt.Columns.Add("Camouflage", typeof(string));

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
                        var ss_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("ssORnss"))
                        .Select(g => new
                        {
                            ssORnss = g.Key
                        }));
                        int num_of_ss_nss = ss_query.Count();
                        var ss_nss_items = ss_query.ToArray();
                        foreach (var ss_nss_item in ss_nss_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id && l.Field<string>("ssORnss") == ss_nss_item.ssORnss).Select(l => new
                            {
                                description = l.Field<string>("recipe_description"),
                                status = l.Field<int?>("status"),
                                defectid =l.Field<int?>("defectid"),
                                ssORnss = l.Field<string>("ssORnss")
                            }).ToArray();
                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();

                                total_checked += data.Count();
                                //buff += data.Count(d => d.status == 21);
                                //rework += data.Count(d => d.status == 22);
                                //ncmr += data.Count(d => d.status == 23);
                                //camoflauge += data.Count(d => d.status == 27);

                                buff += data.Count(d => d.defectid == 21);
                                rework += data.Count(d => d.defectid == 22);
                                ncmr += data.Count(d => d.status == 23);
                                camoflauge += data.Count(d => d.defectid == 27);

                                dr[0] = data[0].description;
                                dr[1] = data[0].ssORnss;
                                dr[2] = data.Count();
                                dr[3] = data.Count(d => d.defectid == 21);
                                dr[4] = data.Count(d => d.defectid == 22);
                                dr[5] = data.Count(d => d.status == 23);
                                dr[6] = data.Count(d => d.defectid == 27);
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                    }
                    
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[2] = total_checked;
                    drt[3] = buff;
                    drt[4] = rework;
                    drt[5] = ncmr;
                    drt[6] = camoflauge;
                    gridviewdt.Rows.Add(drt);
                    break;
                case "Percent":

                    foreach (var item in items)
                    {
                        var ss_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("ssORnss"))
                        .Select(g => new
                        {
                            ssORnss = g.Key
                        }));
                        int num_of_ss_nss = ss_query.Count();
                        var ss_nss_items = ss_query.ToArray();
                        foreach (var ss_nss_item in ss_nss_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id && l.Field<string>("ssORnss") == ss_nss_item.ssORnss).Select(l => new
                            {
                                description = l.Field<string>("recipe_description"),
                                status = l.Field<int?>("status"),
                                defectid = l.Field<int?>("defectid"),
                                ssORnss = l.Field<string>("ssORnss")
                            }).ToArray();
                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();

                                total_checked += data.Count();
                                //buff += data.Count(d => d.status == 21);
                                //rework += data.Count(d => d.status == 22);
                                //ncmr += data.Count(d => d.status == 23);
                                //camoflauge += data.Count(d => d.status == 27);

                                buff += data.Count(d => d.defectid == 21);
                                rework += data.Count(d => d.defectid == 22);
                                ncmr += data.Count(d => d.status == 23);
                                camoflauge += data.Count(d => d.defectid == 27);

                                dr[0] = data[0].description;
                                dr[1] = data[0].ssORnss;
                                dr[2] = data.Count();
                                dr[3] = (data.Count(d => d.defectid == 21) * 100 / Convert.ToInt32(dr[2])) + "%";
                                dr[4] = (data.Count(d => d.defectid == 22) * 100 / Convert.ToInt32(dr[2])) + "%";
                                dr[5] = (data.Count(d => d.status == 23) * 100 / Convert.ToInt32(dr[2])) + "%";
                                dr[6] = (data.Count(d => d.defectid == 27) * 100 / Convert.ToInt32(dr[2])) + "%";

                                gridviewdt.Rows.Add(dr);
                            }
                        }
                    }
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[2] = total_checked;
                    drt[3] = (buff * 100 / total_checked) + "%";
                    drt[4] = (rework * 100 / total_checked) + "%";
                    drt[5] = (ncmr * 100 / total_checked) + "%";
                     drt[6] = (camoflauge * 100 / total_checked) + "%";
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
        private void loadData()
        {
            try
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

                myConnection.comm.CommandText = "select wcID, curingRecipeID, status, defectID, ssORnss=(CASE WHEN ssORnss='1' THEN 'ss' WHEN ssORnss='2' THEN 'nss' WHEN ssORnss='0' THEN '' END) from TBRVisualInspection where  dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcID IN (select ID from wcmaster where vistage=2 and processID=6)";
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt_vi.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select Distinct iD, name AS wc_name from wcMaster where iD IN (select ID from wcmaster where vistage=2 and processID=6)";
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt_workcenter.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select iD AS recipe_id, name as recipe_name, description AS recipe_description from recipeMaster where processID = 5";
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
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                DataTable tbmdt = new DataTable();
                DataTable fdt = new DataTable();

                DataTable gridviewdt = new DataTable();
                string[] head_array = new string[] { "S. No.", "visualWCName", "TyreSize", "Barcode", "Status", "Remark", "shift", "InspectorName", "SSORNSS","VIDate","VITime", "PressNo", "MouldNo", "Cure_Date", "Cure_Time", "tbmWCName", "TBM_Date", "TBM_Time", "BuilderName", "FirstvisualWCName", "FirstTyreSize", "FirstStatus", "FirstDefectAreaName", "FirstDefectname", "FirstRemark", "FirstShift", "FirstInspectorName", "FirstBarCode" };

               // string[] head_array = new string[] { "S. No.", "Press No.", "Mould No.", "Shift", "TyreSize", "VI WorkCenterName", "Building Date", "Building Time", "Builder Name", "Barcode", "Defect", "Disposal", "Remark", "Responsibility" };
                foreach (var arr in head_array)
                gridviewdt.Columns.Add(arr, typeof(string));
                createGridView(gridviewdt, ExcelGridView);
                string query = @"select wcname AS visualWCName, description AS TyreSize, gtbarCode AS BarCode,Status =(CASE WHEN DefectStatusN='OTHER' THEN defectStatusName WHEN DefectStatusN='BUFF' THEN DefectStatusN+'-'+defectStatusName WHEN DefectStatusN='REPAIR' THEN DefectStatusN+'-'+defectStatusName WHEN DefectStatusN='CAMOFLAUGE' THEN DefectStatusN+'-'+defectStatusName  END), remarks,shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),firstname as InspectorName, ssORnss=(CASE WHEN ssORnss='1' THEN 'ss' WHEN ssORnss='2' THEN 'nss' WHEN ssORnss='0' THEN '' END),convert(char(10), dtandTime, 103) AS VIDate, convert(char(8), dtandTime, 108) AS VITime from vTBRVisualInspectionReportNeww where  wcID in (select iD from wcmaster where vistage=2 and processID=6) AND dtandTime>='" + rToDate + "' AND dtandTime<'" + rFromDate + "'  order by dtandtime asc";


                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();

                if (dt.Rows.Count != 0)
                {
                    //string InQuery = "('" + string.Join("','", (string[])dt.AsEnumerable().Select(x => x.Field<string>("gtbarcode").ToString()).ToArray()) + "')"; // InQuery.TrimEnd(',');
                    string tempdtandtime = Convert.ToDateTime(rToDate).AddDays(-3).ToString();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT gtbarCode AS cur_gtbarcode, wcName As PressNo, mouldNo, convert(char(10), dtandTime, 110) AS Cure_Date, convert(char(8), dtandTime, 108) AS Cure_Time FROM vCuringTBR WHERE dtandTime>='" + tempdtandtime + "' AND dtandTime<'" + rFromDate + "'", con);
                    var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    curdt.Load(dread);

                    con.Close();
                    cmd.Dispose();
                    dread.Close();


                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName, CAST(dtandTime AS DATE) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName FROM vTbmTBR WHERE dtandTime>='" + tempdtandtime + "' AND dtandTime<'" + rFromDate + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    tbmdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();


                    string query1 = @"select  gtbarCode AS FirstBarCode, wcname AS FirstvisualWCName, description AS FirstTyreSize,
                    DefectStatusName AS FirstStatus, defectAreaName as FirstdefectAreaName, defectname as Firstdefectname, remarks as Firstremarks, 
                    FirstShift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),firstname as FirstInspectorName
                    from vTBRVisualInspectionReport where WCID IN (select ID from wcmaster where vistage=1 and processID=6) AND dtandTime>='" + tempdtandtime + "' AND dtandTime<'" + rFromDate + "' AND status<>'1'";


                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = query1;

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    fdt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.conn.Close();

                    int serial_number = 1;
                    var row = from r0w1 in dt.AsEnumerable()
                              join r0w2 in curdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w2.Field<string>("cur_gtbarcode") into p
                              from r0w2 in p.DefaultIfEmpty()
                              join r0w3 in tbmdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w3.Field<string>("tbm_gtbarCode") into ps
                              from r0w3 in ps.DefaultIfEmpty()
                              join r0w4 in fdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w4.Field<string>("FirstBarCode") into ps1
                              from r0w4 in ps1.DefaultIfEmpty()
                              select new string[] { serial_number++.ToString() }
                              .Concat(r0w1.ItemArray.Concat
                              (r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "", "" })
                              .Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "", "" }).Concat(r0w4 != null ? r0w4.ItemArray.Skip(1) : new object[] {"", "", "", "", "", "", "", "", "" })).ToArray();


                    foreach (object[] values in row)
                        gridviewdt.Rows.Add(values);

                 
                    ExcelGridView.DataSource = gridviewdt;
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
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    //Response.Flush();
                    //Response.End();
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
