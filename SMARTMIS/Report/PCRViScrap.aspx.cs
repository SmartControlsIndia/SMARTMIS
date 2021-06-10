using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Text;


namespace SmartMIS.Report
{
    public partial class PCRViScrap : System.Web.UI.Page
    {
          #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign=null;
        public int totalcheckedcount = 0, okcount = 0, NotOkCount = 0, reworkcount = 0, ncmrcount = 0, totalrework = 0, treadfault = 0, sidewallfault = 0, beadfault = 0, carcassfault = 0, othersfault = 0, oeCount = 0, replcementCount = 0;
        int status;

        DataTable dt = new DataTable();
        DataTable trimdt = new DataTable();
        DataTable weightdt = new DataTable();  
        DataTable dt_vi_Header = new DataTable();
        DataTable dt_viData = new DataTable();
                       
               
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "PCRVISCRAP_REPORT.xlsx";
        string filepath; 


        #endregion
        #region System Defined Function
        public PCRViScrap()
        {
            filepath = myWebService.getExcelPath();            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                backDiv.Visible = false;
                dialogPanel.Visible = false;
                ShowWarning.Visible = false;
                                       
                if (string.IsNullOrEmpty(tuoReportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
                {
                    tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    tuoReportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    
                }
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    
                    rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
                                        
                   

                    backDiv.Visible = false;
            
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void loadData()
        {
                      

                        rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select  distinct PCRDefectMaster.name As DefectName  from vInspectionPCR left join PCRDefectMaster on vInspectionPCR.defectstatusID = PCRDefectMaster.iD  inner join recipeMaster on vInspectionPCR.curingRecipeID =recipeMaster.iD  and vInspectionPCR.status='33' and vInspectionPCR.wcID=283 and vInspectionPCR.dtandTime >'" + rToDate + "' ";

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_vi_Header.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);




                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select  vInspectionPCR.gtbarCode,PCRDefectMaster.name As DefectName , recipeMaster.description as RecipeName from vInspectionPCR left join PCRDefectMaster on vInspectionPCR.defectstatusID = PCRDefectMaster.iD  inner join recipeMaster on vInspectionPCR.curingRecipeID =recipeMaster.iD  and vInspectionPCR.status='33' and vInspectionPCR.wcID=283 and vInspectionPCR.dtandTime >'" + rToDate + "'";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_viData.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                       
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
                    
                 
                    
                    //for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    //{
                    //    GridViewRow gvRow = MainGridView.Rows[rowIndex];
                    //    GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
                    //    for (int cellCount = 0; cellCount < 2 /*gvRow.Cells.Count*/; cellCount++)
                    //    {
                    //        if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    //        {
                    //            if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                    //            {
                    //                gvRow.Cells[cellCount].RowSpan = 2;
                    //            }
                    //            else
                    //            {
                    //                gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                    //            }
                    //            gvPreviousRow.Cells[cellCount].Visible = false;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        #endregion
        #region User Defined Function
                        
        public string formatfromDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(formatDate(date));
                    flag = tempDate.ToString("MM-dd-yyyy");
                    flag = flag + " " + "07:00:00";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message+ date, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(formatDate(date)).AddDays(1);
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
        public string formattoDate11(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    month = tempDate[1].ToString().Trim();
                    day = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (Convert.ToInt32(month) == 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-05-" + (Convert.ToInt32(year) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    else if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = month + "-" + (Convert.ToInt32(day)).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "05" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }



        #endregion
       
        public string getdisplaytype = "Numbers";
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
            int result = (int)ts.TotalDays;
            if ((int)ts.TotalDays < 0)
            {
                ShowWarning.Visible = true;
                ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>From Date cannot be greater than To Date!!!</font></strong></td></tr></table>";
            }
            else if ((int)ts.TotalDays > 2)
            {
                ShowWarning.Visible = true;
                ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>We can be shown only 2 day data only!!!</font></strong></td></tr></table>";
         
            }
            else
                showVIReport();   
        }
        private void showVIReport()
        {
            try
            {
                loadData();
                VIRecipeWiseGridView.Visible = true;
               
                DataTable gridviewdt = new DataTable();
                DataRow drt;

                if (dt_vi_Header.Rows.Count > 0)
                {
                    gridviewdt.Columns.Add("TYRE SIZE", typeof(string));

                    for (int i = 0; i < dt_vi_Header.Rows.Count; i++)
                    {
                        gridviewdt.Columns.Add("" + dt_vi_Header.Rows[i][0] + "", typeof(string));
                    }
                    gridviewdt.Columns.Add("GRAND TOTAL", typeof(string));
                }

                var query = (dt_viData.AsEnumerable().GroupBy(l => l.Field<string>("RecipeName"))
                    .Select(g => new
                    {
                        curing_id = g.Key
                    }));

                int num_of_curing = query.Count();
                var items = query.ToArray();

                        foreach (var item in items)
                        {
                            DataRow dr = gridviewdt.NewRow();

                            var data = dt_viData.AsEnumerable().Where(l => l.Field<string>("RecipeName") == item.curing_id).Select(l => new
                            {   
                                RecipeName = l.Field<string>("RecipeName"),
                                DefectName = l.Field<string>("DefectName")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                              
                                dr[0] = data[0].RecipeName;
                                for (int i = 1; i < dt_vi_Header.Rows.Count+1; i++)
                                {
                                  
                                    for (int j = 0; j < data.Count(); j++)
                                    {
                                        if (dt_vi_Header.Rows[i - 1][0].ToString() == data[j].DefectName.ToString())
                                        {
                                            dr[i] = "1";
                                            break;
                                        }
                                        else
                                        {
                                            dr[i] = "";
                                        }
                                    }
                                   
                                }

                                dr[dt_vi_Header.Rows.Count + 1] = data.Count();
                                gridviewdt.Rows.Add(dr);
                            }
                           

                        }


                          drt = gridviewdt.NewRow();
                        if (dt_vi_Header.Rows.Count > 0)
                        {
                            drt[0] = "TOTAL";
                            var dTotal = 0;
                            for (int i = 1; i < dt_vi_Header.Rows.Count+1; i++)
                            {
                                var data = dt_viData.AsEnumerable().Where(l => l.Field<string>("DefectName") == dt_vi_Header.Rows[i - 1][0].ToString()).Select(l => new
                                {
                                    DefectName = l.Field<string>("DefectName")
                                }).ToArray();

                                drt[i] = data.Count();
                                dTotal = dTotal + data.Count();
                            }
                            drt[dt_vi_Header.Rows.Count + 1] = dTotal;
                        }

                        gridviewdt.Rows.Add(drt);

                        VIRecipeWiseGridView.DataSource = gridviewdt;
                        VIRecipeWiseGridView.DataBind();

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
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
        private string TotaldtformatDate(String fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";

            if (fromDate != null)
            {
                string fday, fmonth, fyear;
                string tday, tmonth, tyear;

                string[] ftempDate = fromDate.Split(new char[] { '-' });
                string[] ttempDate = toDate.Split(new char[] { '-' });

                try
                {
                    fday = ftempDate[1].ToString().Trim();
                    fmonth = ftempDate[0].ToString().Trim();
                    fyear = ftempDate[2].ToString().Trim();
                    tday = ttempDate[1].ToString().Trim();
                    tmonth = ttempDate[0].ToString().Trim();
                    tyear = ttempDate[2].ToString().Trim();

                    flag1 = fday + "-" + fmonth + "-" + fyear + " " + "07" + ":" + "00" + ":" + "00";

                    if (Convert.ToInt32(tday) == 12 && Convert.ToInt32(tmonth) == 31)
                    {
                        flag2 = "01-01-" + (Convert.ToInt32(tyear) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    if (DateTime.DaysInMonth(Convert.ToInt32(tyear), Convert.ToInt32(tday)) != Convert.ToInt32(tmonth))
                    {
                        flag2 = tday + "-" + (Convert.ToInt32(tmonth) + 1).ToString() + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else if (Convert.ToInt32(tday) < 12)
                    {
                        flag2 = (Convert.ToInt32(tday) + 1).ToString() + "-" + "01" + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    
                    flag = "" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

            }
            return flag;
        }

        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
                int result = (int)ts.TotalDays;

                if ((int)ts.TotalDays < 0)
                {
                    ShowWarning.Visible = true;
                    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>From Date cannot be greater than To Date!!!</font></strong></td></tr></table>";
                }
                else
                {

                    DataTable gridviewdt = new DataTable();
                    DataTable dt = new DataTable();
                    DataTable curdt = new DataTable();
                    DataTable tbmdt = new DataTable();
                    string from_date = formatfromDate(tuoReportMasterFromDateTextBox.Text),
                           to_date = formattoDate(tuoReportMasterToDateTextBox.Text);


                    string[] head_array = new string[] { "S. No.", "visualWCName", "TyreSize", "Barcode", "Status", "DefectAreaName", "Defectname", "Remark", "shift", "InspectorName", "VIDate", "VITime", "PressNo", "cavityName", "MouldNo", "Cure_Date", "Cure_Time", "tbmWCName", "TBM_Date", "TBM_Time", "BuilderName", "BuilderName2", "BuilderName3", "TrimMachine", ""};//, "Tyre_Weight" };
                    foreach (var arr in head_array)
                        gridviewdt.Columns.Add(arr, typeof(string));

                    createGridView(gridviewdt, ExcelGridView);

                    rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = @"select wcname AS visualWCName, description AS TyreSize, gtbarCode AS BarCode,
                    StatusName AS Status, defectLocationName as defectAreaName, defectname, remarks, 
                    shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),firstname as InspectorName,convert(char(10), dtandTime, 105) AS VIDate, convert(char(8), dtandTime, 108) AS VITime
                    from vvisualInspectionPCR where  wcID in (select iD from wcmaster where processID=9 and iD=283) and status in (33) AND dtandTime>='" + from_date + "' AND dtandTime<'" + to_date + "' order by viTime asc";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(myConnection.reader);
                   // myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
          
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();
                    string tempfromdt = Convert.ToDateTime(from_date).AddDays(-10).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "SELECT gtbarCode AS cur_gtbarcode,wcName As PressNo,RIGHT(pressbarcode,8) as cavityNo, case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as  mouldNo, convert(char(10), dtandTime, 105) AS Cure_Date, convert(char(8), dtandTime, 108) AS Cure_Time FROM vCuringpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                   // myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
          
                    curdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    //myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName, convert(char(10), dtandTime, 105) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName FROM vTbmpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName, convert(char(10), dtandTime, 105) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName, isnull( (select firstName from manningMaster where iD= manningID2),'unknown') as BuilderName2, isnull( (select firstName from manningMaster where iD= manningID3),'unknown') as BuilderName3 FROM vTbmpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                   // myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
          
                    tbmdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    


                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    //myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName, convert(char(10), dtandTime, 105) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName FROM vTbmpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.comm.CommandText = "SELECT gtbarCode AS trim_gtbarcode, wcName AS trimWCName from  trimmingdata WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                   // myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
          
                    trimdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    //myConnection.open(ConnectionOption.SQL);
                    //myConnection.comm = myConnection.conn.CreateCommand();
                    ////myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName, convert(char(10), dtandTime, 105) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName FROM vTbmpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    //myConnection.comm.CommandText = "SELECT gtbarCode AS weight_gtbarcode, weight AS Tyre_Weight from vPCRBuddeScannedTyreDetail WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    //myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    //myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    //weightdt.Load(myConnection.reader);
                    //myConnection.conn.Close();

                    //myConnection.comm.Dispose();
                    //myConnection.reader.Close();

                    // if (dt.Rows.Count != 0)
                    //{
                    int serial_number = 1;
                    var row = from r0w1 in dt.AsEnumerable()
                              join r0w2 in curdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w2.Field<string>("cur_gtbarcode") into p
                              from r0w2 in p.DefaultIfEmpty()

                              join r0w3 in tbmdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w3.Field<string>("tbm_gtbarCode") into ps
                              from r0w3 in ps.DefaultIfEmpty()

                              join r0w4 in trimdt.AsEnumerable()
                                on r0w1.Field<string>("Barcode") equals r0w4.Field<string>("trim_gtbarcode") into t
                              from r0w4 in t.DefaultIfEmpty()
                              select new string[] { serial_number++.ToString() }
                              .Concat(r0w1.ItemArray.Concat
                              (r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "", "", "" })
                              .Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "", "", "", "", "" })).
                              Concat(r0w4 != null ? r0w4.ItemArray.Skip(1) : new object[] { "" }).ToArray();

                    foreach (object[] values in row)
                        gridviewdt.Rows.Add(values);

                      // DataTable tcopy = gridviewdt.Copy();
                      //tcopy.Columns.RemoveAt(24);


                      myWebService.writeLogs(gridviewdt.Rows.Count.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                      if (gridviewdt.Rows.Count > 0)
                      {
                          ExcelGridView.DataSource = gridviewdt;
                          ExcelGridView.DataBind();
                          ExcelPanel.Visible = true;

                          Response.Clear();
                          Response.ClearHeaders();
                          Response.ClearContent();
                          Response.Buffer = true;
                          Response.AddHeader("content-disposition", "attachment;filename=PCR_VI_SCRAP_Report.xls");
                          Response.ContentType = "application/vnd.ms-excel";
                          //Response.ContentType = "application/vnd.xls";
                          StringWriter stringWrite = new StringWriter();
                          HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                          ExcelPanel.RenderControl(htmlWrite);

                          Response.Write(stringWrite.ToString());

                          HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                          HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                          HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                          //Response.Flush();
                          //Response.End();

                          ExcelPanel.Visible = false;
                      }
                    
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message + exp.Source, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    
    
    
    }
}
