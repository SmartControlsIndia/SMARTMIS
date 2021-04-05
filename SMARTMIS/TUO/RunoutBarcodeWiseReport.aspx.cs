
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
    public partial class RunoutBarcodeWiseReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable mainGVdt;
        DataTable dynamicDB = new DataTable();
        DataTable weightdt;
        DataTable curfilter;

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
                ShowWarning.Visible = false;
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
                        //string ntoDate = toDate.AddDays(1).ToString("yyyy-MM-dd") + " 06:59:59";
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
                    //flag = year + "-" + month + "-" + day + " " + "07" + ":" + "00" + ":" + "00";

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
                DataTable vidt = new DataTable();
                DataTable BUdt = new DataTable();
                DataTable manndt = new DataTable();
                DataTable wcdt = new DataTable();
                DataTable maindt = new DataTable();
                DataTable tro1dt = new DataTable();
                curfilter = new DataTable();
                DataTable curData = new DataTable();

                #region create datatable for maindt
                maindt.Columns.Add("MACHINENAME", typeof(string));
                maindt.Columns.Add("DATE", typeof(string));
                maindt.Columns.Add("TIME", typeof(string));
                maindt.Columns.Add("RECIPENo", typeof(string));
                maindt.Columns.Add("RECIPECODE", typeof(string));
                maindt.Columns.Add("BARCODE", typeof(string));
                maindt.Columns.Add("TOTALRANK", typeof(string));
                maindt.Columns.Add("UPPERAMOUNT", typeof(Double));
                maindt.Columns.Add("UPPERANGLE", typeof(Double));
                maindt.Columns.Add("UPPERRANK", typeof(string));
                maindt.Columns.Add("LOWERAMOUNT", typeof(Double));
                maindt.Columns.Add("LOWERANGLE", typeof(Double));
                maindt.Columns.Add("LOWERRANK", typeof(string));
                maindt.Columns.Add("UPLOAMOUNT", typeof(Double));
                maindt.Columns.Add("UPLORANK", typeof(string));
                maindt.Columns.Add("STATICAMOUNT", typeof(Double));
                maindt.Columns.Add("STATICANGLE", typeof(Double));
                maindt.Columns.Add("STATICRANK", typeof(string));
                maindt.Columns.Add("COUPLEAMOUNT", typeof(Double));
                maindt.Columns.Add("COUPLEANGLE", typeof(Double));
                maindt.Columns.Add("COUPLERANK", typeof(string));
                maindt.Columns.Add("LROTOAAMOUNT", typeof(Double));
                maindt.Columns.Add("LROTOAANGLERANk", typeof(Double));
                maindt.Columns.Add("LROTOARANK", typeof(string));
                maindt.Columns.Add("LROBOAAMOUNT", typeof(Double));
                maindt.Columns.Add("LROBOAANGLE", typeof(Double));
                maindt.Columns.Add("LROBOARANK", typeof(string));
                maindt.Columns.Add("RRO1_OAAmount", typeof(Double));
                maindt.Columns.Add("RRO1_OAAngle", typeof(Double));
                maindt.Columns.Add("RRO1_OARank", typeof(string));
                maindt.Columns.Add("RRO2_OAAmount", typeof(Double));
                maindt.Columns.Add("RRO2_OAAngle", typeof(Double));
                maindt.Columns.Add("RRO2_OARank", typeof(string));
                maindt.Columns.Add("RRO_3OAAmount", typeof(Double));
                maindt.Columns.Add("RRO3_OAAngle", typeof(float));
                maindt.Columns.Add("RRO3_OARank", typeof(string));
                maindt.Columns.Add("LROT1AMOUNT", typeof(Double));
                maindt.Columns.Add("LROT1ANGLE", typeof(Double));
                maindt.Columns.Add("LROT1ARANK", typeof(string));
                maindt.Columns.Add("LROB1AMOUNT", typeof(Double));
                maindt.Columns.Add("LROB1ANGLE", typeof(Double));
                maindt.Columns.Add("LROB1ARANK", typeof(string));
                maindt.Columns.Add("RRO1AMOUNT", typeof(Double));
                maindt.Columns.Add("RRO1ANGLE", typeof(Double));
                maindt.Columns.Add("RRO1RANK", typeof(string));
                maindt.Columns.Add("RRO2_istAmount", typeof(string));
                maindt.Columns.Add("RRO2_istAngle", typeof(Double));
                maindt.Columns.Add("RRO2_1stRank", typeof(string));
                maindt.Columns.Add("RRO3_istAmount", typeof(Double));
                maindt.Columns.Add("RRO3_istAngle", typeof(Double));
                maindt.Columns.Add("RRO3_1stRank", typeof(string));
                maindt.Columns.Add("lroT2amount", typeof(Double));
                maindt.Columns.Add("lroT2angle", typeof(Double));
                maindt.Columns.Add("lroT2rank", typeof(string));
                maindt.Columns.Add("lroB2amount", typeof(Double));
                maindt.Columns.Add("lroB2angle", typeof(Double));
                maindt.Columns.Add("lroB2rank", typeof(string));
                maindt.Columns.Add("RRO1_IsTAmount", typeof(Double));
                maindt.Columns.Add("RRO1_IsTAngle", typeof(Double));
                maindt.Columns.Add("RRO1_IsTRank", typeof(string));
                maindt.Columns.Add("RRO2_2Amount", typeof(Double));
                maindt.Columns.Add("RRO2_2ndtangle", typeof(Double));
                maindt.Columns.Add("RRO2_2ndRank", typeof(string));
                maindt.Columns.Add("RRO3_2Amount", typeof(Double));
                maindt.Columns.Add("RRO3_2ndtangle", typeof(Double));
                maindt.Columns.Add("RRO3_2ndRank", typeof(string));
                maindt.Columns.Add("LROTBULGEAMOUNT", typeof(Double));
                maindt.Columns.Add("LROTBULGEANGLE", typeof(Double));
                maindt.Columns.Add("LROTBULGERANK", typeof(string));
                maindt.Columns.Add("LROBBULGEAMOUNT", typeof(Double));
                maindt.Columns.Add("LROBBULGEANGLE", typeof(Double));
                maindt.Columns.Add("LROBBULGERANK", typeof(string));
                maindt.Columns.Add("LROTDENTAmount", typeof(Double));
                maindt.Columns.Add("LROTDENTAngle", typeof(Double));
                maindt.Columns.Add("LROTDENTRank", typeof(string));
                maindt.Columns.Add("LROBDENTAMOUNT", typeof(Double));
                maindt.Columns.Add("LROBDENTANGLE", typeof(Double));
                maindt.Columns.Add("LROBDENTRANK", typeof(string));
                maindt.Columns.Add("OUTERDIAMETERVALUE", typeof(Double));
                maindt.Columns.Add("OUTERDIAMETERRANK", typeof(string));
                maindt.Columns.Add("OUTERDIAMETER2VALUE", typeof(string));
                maindt.Columns.Add("OUTERDIAMETER2RANK", typeof(string));
                maindt.Columns.Add("OUTERDIAMETER3VALUE", typeof(Double));
                maindt.Columns.Add("OUTERDIAMETER3RANK", typeof(string));
                maindt.Columns.Add("ROTOTALRANK", typeof(string));
                maindt.Columns.Add("MEASUREPRESSURE", typeof(Double));
                maindt.Columns.Add("TBMMACHINE", typeof(string));
                maindt.Columns.Add("TBMBUILDER", typeof(string));
                maindt.Columns.Add("TBM_DATE_TIME", typeof(string));
               // maindt.Columns.Add("TBM_DATE", typeof(string));

                maindt.Columns.Add("CUR_MACHINE", typeof(string));
                //maindt.Columns.Add("PRESS_CAVITY", typeof(string));
                maindt.Columns.Add("CUR_OPERATOR", typeof(string));
                maindt.Columns.Add("CUR_DATE_TIME", typeof(string));
                //maindt.Columns.Add("CUR_TIME", typeof(string));

                maindt.Columns.Add("SERIALNO", typeof(string));
                maindt.Columns.Add("MOULDNO", typeof(string));
               

                maindt.Columns.Add("VI_WCNAME", typeof(string));
                maindt.Columns.Add("STATUS", typeof(string));
                maindt.Columns.Add("VI_DEFECTNAME", typeof(string));
                maindt.Columns.Add("REMARK", typeof(string));
                maindt.Columns.Add("VI_OPERATOR", typeof(string));
                maindt.Columns.Add("VI_DATE", typeof(string));
                maindt.Columns.Add("VI_TIME", typeof(string));
                // maindt.Columns.Add("WEIGHT", typeof(string));
                // maindt.Columns.Add("WEIGHT12", typeof(string));
                // maindt.Columns.Add("WEIGHT13", typeof(string));
                // maindt.Columns.Add("WEIGHT14", typeof(string));  
               
                #endregion

                //get barcode weight 
                if (GradeDropDownList.SelectedItem.Text == "TRO1")
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = @"select distinct WCNAME, convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), dtandTime, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), dtandTime, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS CUR_TIME,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME,mouldNo As MOULDNO from vtbrrunoutdatatbm_update where dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "' and wcName='TRO1' and barcode!='' order by WCNAME,date,time asc";
                        // myConnection.comm.CommandText = @"select distinct WCNAME,  convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), dtandTime, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), dtandTime, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS CUR_TIME ,mouldNo As MOULDNO ,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME from vtbrrunoutdatatbm_update where dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' order by WCNAME,date,time asc";
                        //myConnection.comm.CommandText = @" select distinct WCNAME,  convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS [TIME],RECIPENO, vtbrrunoutdatatbm_update.RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), vtbrrunoutdatatbm_update.tbmDate, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.tbmDate , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), vtbrrunoutdatatbm_update.Cur_Time, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.Cur_Time , 108) AS CUR_TIME ,mouldNo As MOULDNO ,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME,[BuddeScannedTyreDetail].weight As WEIGHT from vtbrrunoutdatatbm_update inner join [dbo].[BuddeScannedTyreDetail] on vtbrrunoutdatatbm_update.barcode = [BuddeScannedTyreDetail].gtbarcode where vtbrrunoutdatatbm_update.dtandtime>'" + nfromDate + "' AND vtbrrunoutdatatbm_update.dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' and [BuddeScannedTyreDetail].stationNo='1' order by WCNAME,date,time asc";

                        myConnection.comm.CommandText = @" select distinct WCNAME,  convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS [TIME],RECIPENO, vtbrrunoutdatatbm_update.RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), vtbrrunoutdatatbm_update.tbmDate, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.tbmDate , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), vtbrrunoutdatatbm_update.Cur_Time, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.Cur_Time , 108) AS CUR_TIME ,mouldNo As MOULDNO ,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME from vtbrrunoutdatatbm_update where vtbrrunoutdatatbm_update.dtandtime>'" + nfromDate + "' AND vtbrrunoutdatatbm_update.dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' order by WCNAME,date,time asc";
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
                else
                {
                    if (GradeDropDownList.SelectedItem.Text == "ALL")
                    {
                        try
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();
                            //myConnection.comm.CommandText = "select distinct WCNAME, DTANDTIME,RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLERANK,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE from vtbrrunoutdata where dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'order by dtandTime desc";
                            //myConnection.comm.CommandText = "Select name as MachineName ,dtandTime, recipecode AS TyreType,BARCODE,TotalRank, RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where (dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " ";
                            myConnection.comm.CommandText = "select distinct WCNAME, convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLERANK,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,RRO_2OAAMOUNT,RRO2_OAANGLE,RRO2_OARANK,RRO_3OAAMOUNT,RRO3_OAANGLE,RRO3_OARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,RRO2_ISTAMOUNT,RRO2_ISTANGLE,RRO2_1STRANK,RRO3_1STAMOUNT,RRO3_1STANGLE,RRO3_1STRANK,LROT2AMOUNT,LROT2ANGLE,LROT2RANK,LROB2AMOUNT,LROB2ANGLE,LROB2RANK,RRO12NDAMOUNT,RRO12NDANGLE,RRO12NDRANK,RRO3_2AMOUNT,RRO2_2NDTANGLE,RRO2_2NDRANK,RRO3_2NDAMOUNT,RRO3_2NDANGLE,RRO3_2NDRANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,OUTERDIAMETERVALUE,OUTERDIAMETERRANK,OUTERDIAMETER2VALUE,OUTERDIAMETER2RANK,OUTERDIAMETER3VALUE,OUTERDIAMETER3RANK,ROTOTALRANK,MEASPRESSURE from vtbrrunoutdata1 where dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!=''  order by date, Time asc";
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
                    }
                    else
                    {
                        try
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();
                            // myConnection.comm.CommandText = "select distinct WCNAME, DTANDTIME,RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLERANK,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE from vtbrrunoutdata1 where dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'order by dtandTime desc";
                            //myConnection.comm.CommandText = "Select name as MachineName ,dtandTime, recipecode AS TyreType,BARCODE,TotalRank, RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where (dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " ";
                            myConnection.comm.CommandText = "SELECT DISTINCT WCNAME, convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],RECIPENO, RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLERANK,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,RRO_2OAAMOUNT,RRO2_OAANGLE,RRO2_OARANK,RRO_3OAAMOUNT,RRO3_OAANGLE,RRO3_OARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,RRO2_ISTAMOUNT,RRO2_ISTANGLE,RRO2_1STRANK,RRO3_1STAMOUNT,RRO3_1STANGLE,RRO3_1STRANK,LROT2AMOUNT,LROT2ANGLE,LROT2RANK,LROB2AMOUNT,LROB2ANGLE,LROB2RANK,RRO12NDAMOUNT,RRO12NDANGLE,RRO12NDRANK,RRO3_2AMOUNT,RRO2_2NDTANGLE,RRO2_2NDRANK,RRO3_2NDAMOUNT,RRO3_2NDANGLE,RRO3_2NDRANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,OUTERDIAMETERVALUE,OUTERDIAMETERRANK,OUTERDIAMETER2VALUE,OUTERDIAMETER2RANK,OUTERDIAMETER3VALUE,OUTERDIAMETER3RANK,ROTOTALRANK,MEASPRESSURE from vtbrrunoutdata1 where dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "' and wcName='" + GradeDropDownList.SelectedValue + "' and barcode!='' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' order by WCNAME,date,time asc";
                            myConnection.comm.CommandTimeout = 60;
                            myConnection.reader = myConnection.comm.ExecuteReader();
                            myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                            dtTUO.Load(myConnection.reader);
                        }
                        catch (Exception exc)
                        {
                            myWebService.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        }
                        finally
                        {
                            if (!myConnection.reader.IsClosed)
                                myConnection.reader.Close();
                            myConnection.comm.Dispose();
                        }
                    }
                    #region Get TBM details
                    //Get TBM details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_DateTime FROM vTbmtbR t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vTbmtbR t2  WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        //"Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmtbR where (dtandTime>'" + Convert.ToDateTime(fromdate).";AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        myConnection.comm.CommandTimeout = 60;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        tbmdt.Load(myConnection.reader);
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }
                    #endregion
                    #region  Get CUR details
                    //Get CUR details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime as Cur_DateTime,serialNo,mouldNo FROM vCuringtbr t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vCuringtbr t2  WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";
                       
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime as Cur_DateTime,serialNo,mouldNo FROM vCuringtbr t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vCuringtbr t2  WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";
                        //"Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringtbr where (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        myConnection.comm.CommandTimeout = 60;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        curdt.Load(myConnection.reader);

                       // System.Data.DataColumn newColumn = new System.Data.DataColumn("Press_Cavity", typeof(System.String));
                       // newColumn.DefaultValue = "";
                       // curdt.Columns.Add(newColumn);
                       // int temp = 0; string prevmouldNo = "";

                        //foreach (DataRow row in curdt.Rows)
                        //{
                        //    if (row["mouldNo"].ToString() != prevmouldNo)
                        //    {
                        //        row["Press_Cavity"] = "LHS";
                        //        row.EndEdit();
                        //        curdt.AcceptChanges();
                        //        prevmouldNo = row["mouldNo"].ToString();
                        //    }
                        //    else
                        //    {
                        //        row["Press_Cavity"] = "RHS";
                        //        row.EndEdit();
                        //        curdt.AcceptChanges();
                        //        prevmouldNo = "";
                        //    }
                        //}

                        //curdt.Columns["Press_Cavity"].SetOrdinal(2);

                     

                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        //myConnection.close(ConnectionOption.SQL);
                    }
                    #endregion
                    #region  Get VI details
                    //Get VI details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        // myConnection.comm.CommandText = @"Select vipcr.gtbarCode AS VI_gtbarCode,(select top 1 name from wcMaster where id in(vipcr.wcID))AS VI_WcName,vipcr.serialNo,vipcr.status,dbo.TBRDefectMaster.defectName As VI_DefectName,vipcr.remarks,(mm.firstName + ' '+ mm.LastName) AS VI_Operator,convert(char(10), vipcr.dtandTime, 105) AS VI_Date,CONVERT(VARCHAR(8) , vipcr.dtandTime , 108) AS VI_Time FROM [TBRVisualInspection] vipcr inner join [manningMaster] mm on mm.id=vipcr.manningID inner join  dbo.TBRDefectMaster  ON dbo.TBRDefectMaster.iD = vipcr.defectid  where vipcr.dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND vipcr.dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "'";
                        myConnection.comm.CommandText = @"Select vipcr.gtbarCode AS VI_gtbarCode,(select top 1 name from wcMaster where id in(vipcr.wcID))AS VI_WcName, CASE vipcr.status WHEN 1 THEN 'OK' WHEN 2 THEN 'Rework' WHEN 3 THEN 'Ncmr' WHEN 4 THEN 'scrap' WHEN 21 THEN 'Buff' WHEN 22 THEN 'Repair' WHEN 23 THEN
                       'ncmr' WHEN 31 THEN 'ok' WHEN 32 THEN 'rework' WHEN 33 THEN 'downgrade' WHEN 34 THEN 'scrap' END AS status,dbo.TBRDefectMaster.defectName As VI_DefectName,vipcr.remarks,(mm.firstName + ' '+ mm.LastName) AS VI_Operator,convert(char(10), vipcr.dtandTime, 105) AS VI_Date,CONVERT(VARCHAR(8) , vipcr.dtandTime , 108) AS VI_Time FROM [TBRVisualInspection] vipcr left join [manningMaster] mm on mm.id=vipcr.manningID left join  dbo.TBRDefectMaster  ON dbo.TBRDefectMaster.iD = vipcr.defectid where vipcr.dtandtime = (SELECT MAX(vipcr2.dtandTime)FROM TBRVisualInspection vipcr2  WHERE vipcr2.gtbarCode = vipcr.gtbarCode and dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (vipcr.dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND vipcr.dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";
                        //                        myConnection.comm.CommandText = @"Select vipcr.gtbarCode AS VI_gtbarCode,(select top 1 name from wcMaster where id in(vipcr.wcID))AS VI_WcName, CASE vipcr.status WHEN 1 THEN 'OK' WHEN 2 THEN 'Rework' WHEN 3 THEN 'Ncmr' WHEN 4 THEN 'scrap' WHEN 21 THEN 'Buff' WHEN 22 THEN 'Repair' WHEN 23 THEN
                        //                       'ncmr' WHEN 31 THEN 'ok' WHEN 32 THEN 'rework' WHEN 33 THEN 'downgrade' WHEN 34 THEN 'scrap' END AS status,dbo.TBRDefectMaster.defectName As VI_DefectName,vipcr.remarks,(mm.firstName + ' '+ mm.LastName) AS VI_Operator,convert(char(10), vipcr.dtandTime, 105) AS VI_Date,CONVERT(VARCHAR(8) , vipcr.dtandTime , 108) AS VI_Time,BB.weight FROM [TBRVisualInspection] vipcr left join [manningMaster] mm on mm.id=vipcr.manningID left join  dbo.TBRDefectMaster  ON dbo.TBRDefectMaster.iD = vipcr.defectid inner join BuddeScannedTyreDetail BB on vipcr.gtbarCode =BB.gtbarcode where BB.stationNo='1' and  vipcr.dtandtime = (SELECT MAX(vipcr2.dtandTime)FROM TBRVisualInspection vipcr2  WHERE vipcr2.gtbarCode = vipcr.gtbarCode and dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (vipcr.dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-5).ToString() + "' AND vipcr.dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";

                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        myConnection.comm.CommandTimeout = 60;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        vidt.Load(myConnection.reader);
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();

                    }

                    #endregion
                    //Get buddheexit details
                    //try
                    //{
                    //    myConnection.comm = myConnection.conn.CreateCommand();
                    //    myConnection.comm.CommandText = "Select gtbarCode AS BUD_gtbarCode, weight from BuddeScannedTyreDetail  WHERE stationNo='1' and  (dtandtime>'" + Convert.ToDateTime(nfromDate).AddDays(-2).ToString() + "' AND dtandtime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')  and (dtandTime>'" + Convert.ToDateTime(nfromDate).AddDays(-2).ToString() + "' AND dtandTime<'" + Convert.ToDateTime(ntoDate).AddDays(1).ToString() + "')";
                    //    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    //    myConnection.comm.CommandTimeout = 60;
                    //    myConnection.reader = myConnection.comm.ExecuteReader();
                    //    BUdt.Load(myConnection.reader);
                    //}
                    //catch (Exception exp)
                    //{
                    //    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    //}
                    //finally
                    //{
                    //    if (!myConnection.reader.IsClosed)
                    //        myConnection.reader.Close();
                    //    myConnection.comm.Dispose();
                    //    //myConnection.close(ConnectionOption.SQL);
                    //}

                    try
                    {
                        var row = from r0w1 in dtTUO.AsEnumerable()

                                  join r0w2 in tbmdt.AsEnumerable()
                                    on r0w1.Field<string>("barcode") equals r0w2.Field<string>("tbmgtbarCode") into p
                                  from r0w2 in p.DefaultIfEmpty()

                                  join r0w3 in curdt.AsEnumerable()
                                    on r0w1.Field<string>("barcode") equals r0w3.Field<string>("curgtbarCode") into ps
                                  from r0w3 in ps.DefaultIfEmpty()

                                  join r0w4 in vidt.AsEnumerable()
                                 on r0w1.Field<string>("barcode") equals r0w4.Field<string>("VI_gtbarCode") into pss
                                  from r0w4 in pss.DefaultIfEmpty()

                                  //join r0w5 in BUdt.AsEnumerable()
                                  //on r0w1.Field<string>("barcode") equals r0w5.Field<string>("BUD_gtbarCode") into psss
                                  //from r0w5 in psss.DefaultIfEmpty()

                                  select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "", "", "" }).Concat(r0w4 != null ? r0w4.ItemArray.Skip(1) : new object[] { "", "", "", "", "", "", "" }).ToArray();
                        // select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "" }).ToArray();

                        foreach (object[] values in row)
                            maindt.Rows.Add(values);

                        MainGridView.DataSource = maindt;
                        MainGridView.DataBind();
                        MainGridView.Visible = true;
                        ViewState["xmldt"] = null;
                        ViewState.Remove("xmldt");
                        ViewState["xmldt"] = maindt;

                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                }
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
                DataTable dt = (DataTable)ViewState["xmldt"];
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=RUNOUTReport.xls");
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


                ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["xmldt"], true, OfficeOpenXml.Table.TableStyles.Light1);
                ws.Cells.AutoFitColumns();


                var ms = new MemoryStream();
                pck.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);

                Response.Flush();
                Response.End();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
