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
    public partial class RUnoutReportNew1 : System.Web.UI.Page
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
                // Button clickedbutton = sender as Button;
                // int totalrank = 0;
                //createGridView(MainGridView);
                // loadData(); 

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
                DataTable maindt1 = new DataTable();
                DataTable tro1dt = new DataTable();


                #region create datatable for maindt1
                maindt1.Columns.Add("MACHINENAME", typeof(string));
                maindt1.Columns.Add("DATE", typeof(string));
                maindt1.Columns.Add("TIME", typeof(string));
                maindt1.Columns.Add("RECIPENo", typeof(string));
                maindt1.Columns.Add("RECIPECODE", typeof(string));
                maindt1.Columns.Add("BARCODE", typeof(string));
                maindt1.Columns.Add("TOTALRANK", typeof(string));
                maindt1.Columns.Add("UPPERAMOUNT", typeof(Double));
                maindt1.Columns.Add("UPPERANGLE", typeof(Double));
                maindt1.Columns.Add("UPPERRANK", typeof(string));
                maindt1.Columns.Add("LOWERAMOUNT", typeof(Double));
                maindt1.Columns.Add("LOWERANGLE", typeof(Double));
                maindt1.Columns.Add("LOWERRANK", typeof(string));
                maindt1.Columns.Add("UPLOAMOUNT", typeof(Double));
                maindt1.Columns.Add("UPLORANK", typeof(string));
                maindt1.Columns.Add("STATICAMOUNT", typeof(Double));
                maindt1.Columns.Add("STATICANGLE", typeof(Double));
                maindt1.Columns.Add("STATICRANK", typeof(string));
                maindt1.Columns.Add("COUPLEAMOUNT", typeof(Double));
                maindt1.Columns.Add("COUPLEANGLE", typeof(Double));
                maindt1.Columns.Add("COUPLERANK", typeof(string));
                maindt1.Columns.Add("LROTOAAMOUNT", typeof(Double));
                maindt1.Columns.Add("LROTOAANGLERANk", typeof(Double));
                maindt1.Columns.Add("LROTOARANK", typeof(string));
                maindt1.Columns.Add("LROBOAAMOUNT", typeof(Double));
                maindt1.Columns.Add("LROBOAANGLE", typeof(Double));
                maindt1.Columns.Add("LROBOARANK", typeof(string));
                maindt1.Columns.Add("RROOAAmount", typeof(Double));
                maindt1.Columns.Add("RROOAAngle", typeof(Double));
                maindt1.Columns.Add("RROOARank", typeof(string));
                maindt1.Columns.Add("LROT1AMOUNT", typeof(Double));
                maindt1.Columns.Add("LROT1ANGLE", typeof(Double));
                maindt1.Columns.Add("LROT1ARANK", typeof(string));
                maindt1.Columns.Add("LROB1AMOUNT", typeof(Double));
                maindt1.Columns.Add("LROB1ANGLE", typeof(Double));
                maindt1.Columns.Add("LROB1ARANK", typeof(string));
                maindt1.Columns.Add("RRO1AMOUNT", typeof(Double));
                maindt1.Columns.Add("RRO1ANGLE", typeof(Double));
                maindt1.Columns.Add("RRO1RANK", typeof(string));           
                maindt1.Columns.Add("LROTBULGEAMOUNT", typeof(Double));
                maindt1.Columns.Add("LROTBULGEANGLE", typeof(Double));
                maindt1.Columns.Add("LROTBULGERANK", typeof(string));
                maindt1.Columns.Add("LROBBULGEAMOUNT", typeof(Double));
                maindt1.Columns.Add("LROBBULGEANGLE", typeof(Double));
                maindt1.Columns.Add("LROBBULGERANK", typeof(string));
                maindt1.Columns.Add("LROTDENTAmount", typeof(Double));
                maindt1.Columns.Add("LROTDENTAngle", typeof(Double));
                maindt1.Columns.Add("LROTDENTRank", typeof(string));
                maindt1.Columns.Add("LROBDENTAMOUNT", typeof(Double));
                maindt1.Columns.Add("LROBDENTANGLE", typeof(Double));
                maindt1.Columns.Add("LROBDENTRANK", typeof(string));
                maindt1.Columns.Add("ROTOTALRANK", typeof(string));
                maindt1.Columns.Add("MEASUREPRESSURE", typeof(Double));

                maindt1.Columns.Add("TBMMACHINE", typeof(string));
                maindt1.Columns.Add("TBMBUILDER", typeof(string));
                maindt1.Columns.Add("TBM_TIme", typeof(string));
                maindt1.Columns.Add("TBM_DATE", typeof(string));

                maindt1.Columns.Add("Cur_Mchine", typeof(string));
                maindt1.Columns.Add("CUR_OPERATOR", typeof(string));
                maindt1.Columns.Add("CUR_DATE", typeof(string));
                maindt1.Columns.Add("CUR_TIME", typeof(string));

                maindt1.Columns.Add("SERIALNO", typeof(string));
                maindt1.Columns.Add("MOULDNO", typeof(string));

                maindt1.Columns.Add("VI_WCNAME", typeof(string));
                maindt1.Columns.Add("VI_OPERATOR", typeof(string));
                maindt1.Columns.Add("STATUS", typeof(string));
                maindt1.Columns.Add("VI_DEFECTNAME", typeof(string));
                maindt1.Columns.Add("REMARK", typeof(string));

                maindt1.Columns.Add("VI_DATE", typeof(string));
                maindt1.Columns.Add("VI_TIME", typeof(string));
                maindt1.Columns.Add("WEIGHT", typeof(string));
             
                #endregion


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
                maindt.Columns.Add("TBM_TIme", typeof(string));
                maindt.Columns.Add("TBM_DATE", typeof(string));

                maindt.Columns.Add("Cur_Mchine", typeof(string));
                maindt.Columns.Add("CUR_OPERATOR", typeof(string));
                maindt.Columns.Add("CUR_DATE", typeof(string));
                maindt.Columns.Add("CUR_TIME", typeof(string));

                maindt.Columns.Add("SERIALNO", typeof(string));
                maindt.Columns.Add("MOULDNO", typeof(string));

                maindt.Columns.Add("VI_WCNAME", typeof(string));
                maindt.Columns.Add("STATUS", typeof(string));
                maindt.Columns.Add("VI_DEFECTNAME", typeof(string));
                maindt.Columns.Add("REMARK", typeof(string));
                maindt.Columns.Add("VI_OPERATOR", typeof(string));
                maindt.Columns.Add("VI_DATE", typeof(string));
                maindt.Columns.Add("VI_TIME", typeof(string));
                maindt.Columns.Add("WEIGHT", typeof(string));
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
                       // myConnection.comm.CommandText = @" select distinct WCNAME,  convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS [TIME],RECIPENO, vtbrrunoutdatatbm_update.RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS CUR_TIME ,mouldNo As MOULDNO ,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME,[BuddeScannedTyreDetail].weight As WEIGHT from vtbrrunoutdatatbm_update inner join [dbo].[BuddeScannedTyreDetail] on vtbrrunoutdatatbm_update.barcode = [BuddeScannedTyreDetail].gtbarcode where vtbrrunoutdatatbm_update.dtandtime>'" + nfromDate + "' AND vtbrrunoutdatatbm_update.dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' and [BuddeScannedTyreDetail].stationNo='1' order by WCNAME,date,time asc";
                       // myConnection.comm.CommandText = @" select distinct WCNAME,  convert(char(10), [vtbrun].dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , [vtbrun].dtandTime , 108) AS [TIME],RECIPENO, [vtbrun].RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE from vtbrun where vtbrun.dtandtime>'" + nfromDate + "' AND vtbrun.dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' order by WCNAME,date,time asc";
                        myConnection.comm.CommandText = @" select distinct WCNAME,  convert(char(10), vtbrrunoutdatatbm_update.dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.dtandTime , 108) AS [TIME],RECIPENO, vtbrrunoutdatatbm_update.RECIPECODE,BARCODE,TOTALRANK,UPPERAMOUNT,UPPERANGLE,UPPERRANK,LOWERAMOUNT,LOWERANGLE,LOWERRANK,UPLOAMOUNT,UPLORANK,STATICAMOUNT,STATICANGLE,STATICRANK,COUPLEAMOUNT,COUPLEANGLE,COUPLERANK,LROTOAAMOUNT,LROTOAANGLErank,LROTOARANK,LROBOAAMOUNT,LROBOAANGLE,LROBOARANK,RROOAAMOUNT,RROOAANGLE,RROOARANK,LROT1AMOUNT,LROT1ANGLE,LROT1RANK,LROB1AMOUNT,LROB1ANGLE,LROB1RANK,RRO1AMOUNT,RRO1ANGLE,RRO1RANK,LROTBULGEAMOUNT,LROTBULGEANGLE,LROTBULGERANK,LROBBULGEAMOUNT,LROBBULGEANGLE,LROBBULGERANK,LROTDENTAMOUNT,LROTDENTANGLE,LROTDENTRANK,LROBDENTAMOUNT,LROBDENTANGLE,LROBDENTRANK,ROTOTALRANK,MEASPRESSURE, TBM_Machine AS TBM_MACHINE, TBM_op AS TBM_OPERATOR, convert(char(10), vtbrrunoutdatatbm_update.tbmDate, 105) AS TBM_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.tbmDate , 108) AS TBM_TIME,cur_machine AS CUR_MACHINE,cur_op AS CUR_OPERATOR,convert(char(10), vtbrrunoutdatatbm_update.Cur_Time, 105) AS CUR_DATE,CONVERT(VARCHAR(8) , vtbrrunoutdatatbm_update.Cur_Time , 108) AS CUR_TIME ,mouldNo As MOULDNO ,(select top 1 name from  wcmaster where iD in(VI_wcid))AS VI_WCNAME,serialNo AS SERIALNO,defectstatusName As STATUS,VI_DefectName AS VI_DEFECTNAME,remarks AS REMARK,VI_op AS VI_OPERATOR,convert(char(10), VI_dtandTime, 105) AS VI_DATE,CONVERT(VARCHAR(8) , VI_dtandTime , 108) AS VI_TIME from vtbrrunoutdatatbm_update where vtbrrunoutdatatbm_update.dtandtime>'" + nfromDate + "' AND vtbrrunoutdatatbm_update.dtandtime<'" + ntoDate + "' and wcName='TRO1' and UPPERAMOUNT !='' and lowerAmount!='' and LROTBULGEAMOUNT!='' and LROBBULGEAMOUNT!='' order by WCNAME,date,time asc";
                      
                        myConnection.comm.CommandTimeout = 120;
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

                    #region Get BUDDE details
                   
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = @" Select gtbarCode AS BUgtbarCode, Weight FROM [BuddeScannedTyreDetail] where stationNo='1' and gtbarCode!='' and Weight!='' and dtandtime>'" + nfromDate + "' AND dtandtime<'" + ntoDate + "'";
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        myConnection.comm.CommandTimeout = 120;
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
                    
                   
                    try
                    {
                        var rowDATA = from r0w1 in dtTUO.AsEnumerable()
                                  join r0w2 in tbmdt.AsEnumerable()
                                    on r0w1.Field<string>("BARCODE") equals r0w2.Field<string>("BUgtbarCode") into p
                                  from r0w2 in p.DefaultIfEmpty()
                                  select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { }).ToArray();

                                 myWebService.writeLogs("CUSTAM_MSG00_" + dtTUO.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
              
                        try
                        {
                            foreach (object[] values in rowDATA)
                            {
                                maindt1.Rows.Add(values);
                            }
                            myWebService.writeLogs("CUSTAM_MSG03_" + tbmdt.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        }
                        catch (Exception ex)
                        {
                            myWebService.writeLogs("CUSTAM_MSG04_" + ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        }
                        myWebService.writeLogs("CUSTAM_MSG05_" + maindt1.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                        MainGridView.DataSource = maindt1;
                        MainGridView.DataBind();
                        MainGridView.Visible = true;
                        ViewState["dt"] = maindt1;

                        myWebService.writeLogs("CUSTAM_MSG06_" + maindt1.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
              
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
                DataTable dt = (DataTable)ViewState["dt"];
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


                ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Light1);
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


