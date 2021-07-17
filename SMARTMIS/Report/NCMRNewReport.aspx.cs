using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using OfficeOpenXml;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace SmartMIS.Report
{
    public partial class NCMRNewReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
       
        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign = null;
        public int totalcheckedcount = 0, okcount = 0, NotOkCount = 0, reworkcount = 0, scrapcount = 0, majorokcount = 0, minorbuffcount = 0, majorholdcount = 0, minorCount = 0, majorCount = 0, majorscrapcount = 0, erraticHold = 0, Pass1Count = 0, Pass2Count = 0, Pass3Count = 0, Pass4Count = 0, totalokcount = 0, totalbuffcount = 0, totalscrapcount = 0, totalholdcount = 0, totalminorcount = 0, min_count = 0, min_pass1count = 0, min_pass2count = 0;
        string sqlquery = "";
        int status;
        DateTime fromDate, toDate;
        DataTable wc_name_dt = new DataTable();
        DataTable dt = new DataTable();

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "TBRVisualInspectionReport.xlsx";
        string filepath;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
  
            if (!IsPostBack)
            {
                fillSizedropdownlist();

                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }
         
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {
          
            string getMonth = DropDownListMonth.SelectedValue;
            string getYear = DropDownListYear.SelectedItem.Text;
            string getyearwise = DropDownList2.SelectedItem.Text;
            string recipe = ddlRecipe.SelectedItem.Text;
            string tyredesign = "";
            string duration = "";
            var datetimebt = "";
            string getfromdate = reportMasterFromDateTextBox.Text;

            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                    if (formatDate(getfromdate) == "")
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>Please select the date!!!</font></strong></td></tr></table>";
                        break;
                    }
                    ShowWarning.Visible = false;
                    toDate = fromDate.AddDays(1);

                    string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                    string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";

                    DateTime date1 = Convert.ToDateTime(nfromDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    DateTime date2 = Convert.ToDateTime(ntoDate, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                   
                    showReportDateMonthWise(date1.ToString(), date2.ToString(), recipe, tyredesign);
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
                    if (nfromDate == "") {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>Please select the from date!!!</font></strong></td></tr></table>";
                        break;
                    }
                   
                    if (formatDate(tuoReportMasterToDateTextBox.Text) == "")
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>Please select the to date!!!</font></strong></td></tr></table>";
                        break;
                    }
                    toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                    ntoDate = toDate.AddDays(1).ToString();
                    TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                    int result = (int)ts.TotalDays;
                    if ((int)ts.TotalDays > 30)
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 30 days!!!</font></strong></td></tr></table>";
                    }

                    else {
                        ShowWarning.Visible = false;
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign); ; }
                    break;

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
             
                DataTable minor_gdt = new DataTable();
                DataTable dtTUO = new DataTable();
                DataTable rdt = new DataTable();
                DataTable wcdt = new DataTable();
                DataTable mdt = new DataTable();

                DataTable gridviewdt = new DataTable();
                DataTable majordt = new DataTable();
                DataRow dr;

                DataTable ncmrDT = new DataTable();
                DataTable vi2DT = new DataTable();
                DataTable vi1DT = new DataTable();
                DataTable curDT = new DataTable();
                DataTable tbmDT = new DataTable();
                gridviewdt.Columns.Add("S. No.", typeof(string));
                gridviewdt.Columns.Add("NCMR Work Center", typeof(string));
                gridviewdt.Columns.Add("Tyre Size", typeof(string));
                gridviewdt.Columns.Add("Barcode", typeof(string));
                gridviewdt.Columns.Add("Classifier Name", typeof(string));
                gridviewdt.Columns.Add("Classification Date", typeof(string));
                gridviewdt.Columns.Add("Classification Time", typeof(string));
                gridviewdt.Columns.Add("Disposal", typeof(string));
                gridviewdt.Columns.Add("VI-2 Station", typeof(string));
                gridviewdt.Columns.Add("VI-2 Status", typeof(string));
                gridviewdt.Columns.Add("VI-2 Date & Time", typeof(string));
                gridviewdt.Columns.Add("VI-1 Station", typeof(string));
                gridviewdt.Columns.Add("VI-1 Status", typeof(string));
                gridviewdt.Columns.Add("Defect Area Name", typeof(string));
                gridviewdt.Columns.Add("Defect Name", typeof(string));
                gridviewdt.Columns.Add("Remark", typeof(string));
                gridviewdt.Columns.Add("Shift", typeof(string));
                gridviewdt.Columns.Add("Inspector Name", typeof(string));
                gridviewdt.Columns.Add("VI Date ", typeof(string));
                gridviewdt.Columns.Add("VI Time", typeof(string));
                gridviewdt.Columns.Add("Press No", typeof(string));
                gridviewdt.Columns.Add("Mould No", typeof(string));
                gridviewdt.Columns.Add("Cure Date", typeof(string));
                gridviewdt.Columns.Add("Cure Time", typeof(string));
                gridviewdt.Columns.Add("TBM", typeof(string));
                gridviewdt.Columns.Add("TBM Date", typeof(string));
                gridviewdt.Columns.Add("TBM Time", typeof(string));
                gridviewdt.Columns.Add("TBM Builder Name", typeof(string));
           
                //Get NCMR details

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    if (ddlRecipe.SelectedValue != "ALL" && pcrDDldesign.SelectedValue !="ALL")
                    {
                        myConnection.comm.CommandText = @"WITH Ranked
AS (SELECT wcName,TyreSize,gtbarcode as gtbarcodeNCMR,builderName,NCMRdate,NCMRtime,defectstatusName,shift  FROM (select wcName,description As TyreSize,gtbarcode, convert(char(10), dtandTime, 105) AS NCMRdate, convert(char(8), dtandTime, 108) AS NCMRtime, firstName + ' ' + lastName As builderName, defectstatusName=UPPER(LEFT(defectstatusName,1))+LOWER(SUBSTRING(defectstatusName,2,LEN(defectstatusName))), shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END), ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVisualInspectionReport where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and description='" + ddlRecipe.SelectedItem.Text + "' and wcName in('7010') )As a Where a.RowNumber=1) select Ranked.wcName,Ranked.TyreSize,Ranked.gtbarcodeNCMR,Ranked.builderName,Ranked.NCMRdate,Ranked.NCMRtime,Ranked.defectstatusName from Ranked where Ranked.shift='" + pcrDDldesign.SelectedValue + "'";
                    }
                    else if (ddlRecipe.SelectedValue != "ALL" && pcrDDldesign.SelectedValue == "ALL")
                    {
                        myConnection.comm.CommandText = @"SELECT wcName,TyreSize,gtbarcode as gtbarcodeNCMR,builderName,NCMRdate,NCMRtime,defectstatusName  FROM (select wcName,description As TyreSize,gtbarcode, convert(char(10), dtandTime, 105) AS NCMRdate, convert(char(8), dtandTime, 108) AS NCMRtime, firstName + ' ' + lastName As builderName, defectstatusName=UPPER(LEFT(defectstatusName,1))+LOWER(SUBSTRING(defectstatusName,2,LEN(defectstatusName))), ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVisualInspectionReport where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and description='" + ddlRecipe.SelectedItem.Text + "'  and wcName in('7010') )As a Where a.RowNumber=1";
                
                    }
                    else if (ddlRecipe.SelectedValue == "ALL" && pcrDDldesign.SelectedValue != "ALL")
                    {
                        myConnection.comm.CommandText = @"WITH Ranked
AS (SELECT wcName,TyreSize,gtbarcode as gtbarcodeNCMR,builderName,NCMRdate,NCMRtime,defectstatusName,shift  FROM (select wcName,description As TyreSize,gtbarcode, convert(char(10), dtandTime, 105) AS NCMRdate, convert(char(8), dtandTime, 108) AS NCMRtime, firstName + ' ' + lastName As builderName, defectstatusName=UPPER(LEFT(defectstatusName,1))+LOWER(SUBSTRING(defectstatusName,2,LEN(defectstatusName))), shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END), ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVisualInspectionReport where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcName in('7010') )As a Where a.RowNumber=1) select Ranked.wcName,Ranked.TyreSize,Ranked.gtbarcodeNCMR,Ranked.builderName,Ranked.NCMRdate,Ranked.NCMRtime,Ranked.defectstatusName from Ranked where Ranked.shift='" + pcrDDldesign.SelectedValue + "'";
                    }
                    else
                    {
                        myConnection.comm.CommandText = @"SELECT wcName,TyreSize,gtbarcode as gtbarcodeNCMR,builderName,NCMRdate,NCMRtime,defectstatusName  FROM (select wcName,description As TyreSize,gtbarcode, convert(char(10), dtandTime, 105) AS NCMRdate, convert(char(8), dtandTime, 108) AS NCMRtime, firstName + ' ' + lastName As builderName, defectstatusName=UPPER(LEFT(defectstatusName,1))+LOWER(SUBSTRING(defectstatusName,2,LEN(defectstatusName))), ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVisualInspectionReport where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcName in('7010') )As a Where a.RowNumber=1";
                  
                    }
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    ncmrDT.Load(myConnection.reader);
                }
                catch (Exception exc)
                { }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                }


                //Get VI2 details
                try
                {
                    string tempfromdt = Convert.ToDateTime(nfromDate).AddDays(-20).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = @"SELECT gtbarcode as gtbarcodevi2,wcName,defectstatusName,NCMRdate  FROM (select wcName,gtbarcode, convert(char(10), dtandTime, 105) AS NCMRdate, convert(char(8), dtandTime, 108) AS NCMRtime, defectstatusName, ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVISecondLine where dtandTime > '" + tempfromdt + "' and dtandtime<'" + ntoDate + "' and wcName in('7009') )As a Where a.RowNumber=1";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    vi2DT.Load(myConnection.reader);
                }
                catch (Exception exc)
                {
                    myWebService.writeLogs(exc.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }


                //Get VI1 details
                try
                {
                    string tempfromdt = Convert.ToDateTime(nfromDate).AddDays(-20).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = @"SELECT gtbarcode as gtbarcodevi1,wcName,defectstatusName,defectAreaName,defectName,remarks,shift ,builderName,VIDate,VITime  FROM (select wcName,gtbarcode,convert(char(10), dtandTime, 105) AS VIDate, convert(char(8), dtandTime, 108) AS VITime, firstName + ' ' + lastName As builderName,remarks,shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN  convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END), defectstatusName,defectAreaName,defectName, ROW_NUMBER() OVER (PARTITION BY gtbarcode ORDER BY dtandTime desc) AS RowNumber from vTBRVisualInspectionReport where dtandTime > '" + tempfromdt + "' and dtandtime<'" + ntoDate + "' and wcName in('7001','7002','7003','7004','7005','7006') )As a Where a.RowNumber=1";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    vi1DT.Load(myConnection.reader);
                }
                catch (Exception ex)
                { myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }


                //Get curing details
                try
                {
                    string tempfromdt = Convert.ToDateTime(nfromDate).AddDays(-20).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = @"select gtbarCode as gtbarcodeCur,wcName,mouldNo,convert(char(10), dtandTime, 105) AS CURdate, convert(char(8), dtandTime, 108) AS CURtime from vCuringtbr where  dtandTime > '" + tempfromdt + "' and dtandtime<'" + ntoDate + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    curDT.Load(myConnection.reader);
                }
                catch (Exception exc)
                {
                    myWebService.writeLogs(exc.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

                //Get building Details

                try
                {
                    string tempfromdt = Convert.ToDateTime(nfromDate).AddDays(-20).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "	select gtbarCode as gtbarcodeTbm,wcName,convert(char(10), dtandTime, 105) AS TBMdate, convert(char(8), dtandTime, 108) AS TBMtime,firstName+' '+lastName as BilderName from vtbmtbr where  dtandTime > '" + tempfromdt + "' and dtandtime<'" + ntoDate + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    tbmDT.Load(myConnection.reader);
                }
                catch (Exception exc)
                {
                    myWebService.writeLogs(exc.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                }

                DataTable dt_defect = new DataTable();
                int serial_number = 1;
                var row = from r0w1 in ncmrDT.AsEnumerable()
                          join r0w2 in vi2DT.AsEnumerable()
                          on r0w1.Field<string>("gtbarcodeNCMR") equals r0w2.Field<string>("gtbarcodevi2") into ncm
                          from r0w2 in ncm.DefaultIfEmpty()

                          join r0w3 in vi1DT.AsEnumerable()
                           on r0w1.Field<string>("gtbarcodeNCMR") equals r0w3.Field<string>("gtbarcodevi1") into vi
                          from r0w3 in vi.DefaultIfEmpty()

                          join r0w4 in curDT.AsEnumerable()
                            on r0w1.Field<string>("gtbarcodeNCMR") equals r0w4.Field<string>("gtbarcodeCur") into ps
                          from r0w4 in ps.DefaultIfEmpty()

                          join r0w5 in tbmDT.AsEnumerable()
                           on r0w1.Field<string>("gtbarcodeNCMR") equals r0w5.Field<string>("gtbarcodeTbm") into tbm
                          from r0w5 in tbm.DefaultIfEmpty()

                           select new string[] { serial_number++.ToString() }
                              .Concat( r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "", "", "", "", "", "", "" }).Concat(r0w4 != null ? r0w4.ItemArray.Skip(1) : new object[] { "", "", "", "" }).Concat(r0w5 != null ? r0w5.ItemArray.Skip(1) : new object[] { "", "", "", "" })).ToArray();



                foreach (object[] values in row)
                    gridviewdt.Rows.Add(values);

                //Get vistage=2 details
                DataTable dtExcel = gridviewdt.Copy();

              
                ViewState["dt"] = dtExcel;
                  if (dtExcel.Rows.Count > 0)
                  {
                    grdinspectionsummary.DataSource = dtExcel;
                    grdinspectionsummary.DataBind();
                    grdinspectionsummary.Visible = true;
                    //GridView1.Visible = false;
                    lbltext.Visible = false;
                  }
                  else
                  {

                      lbltext.Text = "No Records Found";
                      lbltext.CssClass = "LabelTextAlignStyle";
                      lbltext.Visible = true;
                      grdinspectionsummary.Visible = false;
                  }
                if (dtExcel.Rows.Count > 0)
                {
                    GridView1.DataSource = dtExcel;
                    GridView1.DataBind();
                }

            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
       
        private void fillSizedropdownlist()
        {

            DataTable d_dt = new DataTable();

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "Select DISTINCT id as rID,description from recipemaster where description != '0' and description !='' and tyreSize!=''and processID in(5) order by description ";
            myConnection.reader = myConnection.comm.ExecuteReader();
            d_dt.Load(myConnection.reader);

            ddlRecipe.DataSource = d_dt;
            ddlRecipe.DataTextField = "description";
            ddlRecipe.DataValueField = "rID";
            ddlRecipe.DataBind();
            ddlRecipe.Items.Insert(0, new ListItem("ALL", "ALL"));
        }
 
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            if (ViewState["dt"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt"];
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TBR_NCMR_Report.xls");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TBR_NCMR_Report");
                ws.Cells["A1"].Value = "TBR NCMR Report ";

                using (ExcelRange r = ws.Cells["A1:AG1"])
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
        protected void OnDataBound(object sender, EventArgs e)
        {

            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();


            cell.Text = "NCMR AREA";
            cell.ColumnSpan = 8;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Second Line VI Area";
            cell.ColumnSpan = 3;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "First Line VI Area";
            cell.ColumnSpan = 9;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Curing Area";
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 4;
            cell.Text = "Building Area";
            row.Controls.Add(cell);
            grdinspectionsummary.HeaderRow.Parent.Controls.AddAt(0, row);
        }

        protected void ddlRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

