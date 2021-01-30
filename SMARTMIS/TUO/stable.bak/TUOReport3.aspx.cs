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

namespace SmartMIS.TUO
{
    public partial class TUOReport3 : System.Web.UI.Page
    {
               #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        double gradetotal_, gradeA_, gradeB_, gradeC_, gradeD_, gradeE_, AvgRFVCW_, AvgRFVCCW_, AvgH1RFVCW_, AvgH1RFVCCW_, AvgLFVCW_, AvgLFVCCW_, CON_, AvgLowerBulge_, AvgUpperBulge_, AvgLowerLRO_, AvgUpperLRO_, AvgRRO_, AvgLowerDEP_, AvgUpperDEP_;

        public Double grandGradetotal, grandGradeA, grandGradeB, grandGradeC, grandGradeD, grandGradeE, grandAvgRFVCW, grandAvgRFVCCW, grandAvgH1RFVCW, grandAvgH1RFVCCW, grandAvgLFVCW, grandAvgLFVCCW, grandCON, grandAvgLowerBulge, grandAvgUpperBulge, grandAvgLowerLRO, grandAvgUpperLRO, grandAvgRRO, grandAvgLowerDEP, grandAvgUpperDEP;
        string tablename;
        DataRow tdr;
        
        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", query = "";
        string[] tempString2;
        string grade;
        public string _rDate;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "AverageANDstdperformanceReport.xlsx";
        string filepath;

        int rowCount = 4, pid = -1;
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        public String ReportDate
        {
            get
            {
                return _rDate;
            }

            set
            {
                _rDate = value;

            }
        }
        public String Visiblity
        {

            get
            {
                return performanceAvgReportMainPanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                performanceAvgReportMainPanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        #endregion

        public TUOReport3()
        {
            filepath = myWebService.getExcelPath();
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            if (tuoFilterOptionDropDownList.SelectedIndex == 1)
                option = "1";
            else if (tuoFilterOptionDropDownList.SelectedIndex == 2)
                option = "2";

            grade = GradeDropDownList.SelectedItem.ToString();
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);

            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);
            if (tempString2[1] != "0")
            {
                if (tempString2[3] == "0")
                {
                    rType = "monthWise";
                    rToMonth = tempString2[5];
                    rToYear = tempString2[6];
                }
                else if (tempString2[3] != "0")
                {
                    rType = "dayWise";
                    rToDate = myWebService.formatDate(tempString2[3]);
                    rFromDate = myWebService.formatDate(tempString2[4]);
                }

                wcnamequery = wcquery(query);
                dtnadtime1 = (rType == "dayWise") ? TotalprodataformatDate(rToDate, rFromDate) : null;

                if (QualityReportTBMWise.Checked)
                {
                    performanceAvgReportMainGridView.Visible = true;
                    productionReport2RecipeWiseMainPanel.Visible = false;
                    showReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    performanceAvgReportMainGridView.Visible = false;
                    productionReport2RecipeWiseMainPanel.Visible = true;
                    showReportRecipeWise(performanceAvgReportMainGridView, "", rToDate, rFromDate);
                }                                
            }
            else
            {
                performanceAvgReportTBMRecipeWiseMainGridView.DataSource = null;
                performanceAvgReportTBMRecipeWiseMainGridView.DataBind();

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }
            showDownload.Text = "";
            if (!IsPostBack)
            {
                fillSizedropdownlist();
                fillDesigndropdownlist(); 
            }
            performanceAvgReportMainGridView.Visible = false;
            productionReport2RecipeWiseMainPanel.Visible = false;
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceAvgReportMainGridView")
                {

                    Label wcnameLabel = ((Label)e.Row.FindControl("performanceAvgReportWCNameLabel"));
                    workcentername = wcnameLabel.Text.ToString();
                    GridView childGridView = ((GridView)e.Row.FindControl("performanceAvgReportChildGridView"));
                    showReportRecipeWise(childGridView, workcentername, rToDate, rFromDate);


                }

            }
            grandGradetotal = Math.Round(grandGradetotal, 1);
            grandGradeA = Math.Round(grandGradeA, 1);
            grandGradeB = Math.Round(grandGradeB, 1);
            grandGradeC = Math.Round(grandGradeC, 1);
            grandGradeD = Math.Round(grandGradeD, 1);
            grandGradeE = Math.Round(grandGradeE, 1);
            grandAvgRFVCW = Math.Round(grandAvgRFVCW, 1);
            grandAvgRFVCCW = Math.Round(grandAvgRFVCCW, 1);
            grandAvgH1RFVCW = Math.Round(grandAvgH1RFVCW, 1);
            grandAvgH1RFVCCW = Math.Round(grandAvgH1RFVCCW, 1);
            grandAvgLFVCW = Math.Round(grandAvgLFVCW, 1);
            grandAvgLFVCCW = Math.Round(grandAvgLFVCCW, 1);
            grandCON = Math.Round(grandCON, 1);
            grandAvgLowerBulge = Math.Round(grandAvgLowerBulge, 1);
            grandAvgUpperBulge = Math.Round(grandAvgUpperBulge, 1);
            grandAvgLowerLRO = Math.Round(grandAvgLowerLRO, 1);
            grandAvgUpperLRO = Math.Round(grandAvgUpperLRO, 1);
            grandAvgRRO = Math.Round(grandAvgRRO, 1);
            grandAvgLowerDEP = Math.Round(grandAvgLowerDEP, 1);
            grandAvgUpperDEP = Math.Round(grandAvgUpperDEP, 1);
        }
        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private void showReport(string query)
        {
            fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
        }
        private double validateDT(DataTable dt, string avgOf, DataTable uniqrecipedt, int i, string grade)
        {
            try
            {
                string getGrade = GradeDropDownList.SelectedItem.ToString();
                if (getGrade == "AllChecked")
                {
                    if (tuoFilterOptionDropDownList.SelectedValue == "0")
                        return (double)Math.Round((double)(dt.Compute("avg(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'")), 1);
                    else if (tuoFilterOptionDropDownList.SelectedValue == "1")
                        return (double)Math.Round((double)(dt.Compute("STDEV(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'")), 1);
                    else
                        return 0;
                }
                else
                {
                    if (tuoFilterOptionDropDownList.SelectedValue == "0")
                        return (double)Math.Round((double)(dt.Compute("avg(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and " + grade)), 1);
                    else if (tuoFilterOptionDropDownList.SelectedValue == "1")
                        return (double)Math.Round((double)(dt.Compute("STDEV(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and " + grade)), 1);
                    else
                        return 0;
                }
            }
            catch (Exception exp)
            {
                return 0;
            }
        }
        /*private double getstandardDev(DataTable dt, string avgOf, DataTable uniqrecipedt, int i, string grade)
        {
            try
            {
                return (double)Math.Round((double)(dt.Compute("STDEV(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and " + grade + "")), 1);

            }
            catch(Exception exp)
            {
                return 0;
            }
        }*/
        protected string getqueryWCWise(string tableName, string wcNameString, string dtnadtime)
        {
            if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            return query;
        }
        private void showReportRecipeWise(GridView childgridview, string wcName, string rtodate, string rfromdate)
        {
            string query;
            DataTable dt = new DataTable();
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("A", typeof(string));
            gridviewdt.Columns.Add("B", typeof(string));
            gridviewdt.Columns.Add("C", typeof(string));
            gridviewdt.Columns.Add("D", typeof(string));
            gridviewdt.Columns.Add("E", typeof(string));
            gridviewdt.Columns.Add("AvgRFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgRFVCCW", typeof(string));
            gridviewdt.Columns.Add("AvgH1RFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgH1RFVCCW", typeof(string));
            gridviewdt.Columns.Add("AvgLFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgLFVCCW", typeof(string));
            gridviewdt.Columns.Add("CON", typeof(string));
            gridviewdt.Columns.Add("AvgLowerBulge", typeof(string));
            gridviewdt.Columns.Add("AvgUpperBulge", typeof(string));
            gridviewdt.Columns.Add("AvgLowerLRO", typeof(string));
            gridviewdt.Columns.Add("AvgUpperLRO", typeof(string));
            gridviewdt.Columns.Add("AvgRRO", typeof(string));
            gridviewdt.Columns.Add("AvgLowerDEP", typeof(string));
            gridviewdt.Columns.Add("AvgUpperDEP", typeof(string));
            gridviewdt.Columns.Add("stdDevRFVCW", typeof(string));
                        
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("RFVCW", typeof(double));
            dt.Columns.Add("RFVCCW", typeof(double));
            dt.Columns.Add("H1RFVCW", typeof(double));
            dt.Columns.Add("H1RFVCCW", typeof(double));
            dt.Columns.Add("LFVCW", typeof(double));
            dt.Columns.Add("LFVCCW", typeof(double));
            dt.Columns.Add("CONICITY", typeof(double));
            dt.Columns.Add("LowerBulge", typeof(double));
            dt.Columns.Add("UpperBulge", typeof(double));
            dt.Columns.Add("LowerLRO", typeof(double));
            dt.Columns.Add("UpperLRO", typeof(double));
            dt.Columns.Add("RRO", typeof(double));
            dt.Columns.Add("LowerDepression", typeof(double));
            dt.Columns.Add("UpperDepression", typeof(double));
            dt.Columns.Add("GradeRFVCW", typeof(string));
            dt.Columns.Add("GradeH1RFVCW", typeof(string));
            dt.Columns.Add("GradeH1RFVCCW", typeof(string));
            dt.Columns.Add("GradeLFVCW", typeof(string));
            dt.Columns.Add("GradeLFVCCW", typeof(string));
            dt.Columns.Add("GradeCONICITY", typeof(string));
            dt.Columns.Add("GradeLowerBulge", typeof(string));
            dt.Columns.Add("GradeUpperBulge", typeof(string));
            dt.Columns.Add("GradeLowerLRO", typeof(string));
            dt.Columns.Add("GradeUpperLRO", typeof(string));
            dt.Columns.Add("GradeRRO", typeof(string));
            dt.Columns.Add("GradeLowerDepression", typeof(string));
            dt.Columns.Add("GradeUpperDepression", typeof(string));
            double gradetotal, gradeA, gradeB, gradeC, gradeD, gradeE, AvgRFVCW, AvgRFVCCW, AvgH1RFVCW, AvgH1RFVCCW, AvgLFVCW, AvgLFVCCW, CON, AvgLowerBulge, AvgUpperBulge, AvgLowerLRO, AvgUpperLRO, AvgRRO, AvgLowerDEP, AvgUpperDEP;
            
            query = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportRecipeWise.Checked)
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                if(tempString2[tempString2.Length-1].ToString()=="0")
                    query = getqueryWCWise(tablename,"wcname='" + wcName + "'",dtnadtime); 
               else if(tempString2[tempString2.Length-1].ToString()=="1")
                   query = getqueryWCWise(tablename, "machineName='" + wcName + "'", dtnadtime); 
               else if(tempString2[tempString2.Length-1].ToString()=="2")
                   query = getqueryWCWise(tablename, "wcname='" + wcName + "'", dtnadtime); 

            }
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
            catch (Exception exp)
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");

            gradetotal_ = 0; gradeA_ = 0; gradeB_ = 0; gradeC_ = 0; gradeD_ = 0; gradeE_ = 0; AvgRFVCW_ = 0; AvgRFVCCW_ = 0; AvgH1RFVCW_ = 0; AvgH1RFVCCW_ = 0; AvgLFVCW_ = 0; AvgLFVCCW_ = 0; CON_ = 0; AvgLowerBulge_ = 0; AvgUpperBulge_ = 0; AvgLowerLRO_ = 0; AvgUpperLRO_ = 0; AvgRRO_ = 0; AvgLowerDEP_ = 0; AvgUpperDEP_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                gradetotal = 0; gradeA = 0; gradeB = 0; gradeC = 0; gradeD = 0; gradeE = 0; AvgRFVCW = 0; AvgRFVCCW = 0; AvgH1RFVCW = 0; AvgH1RFVCCW = 0; AvgLFVCW = 0; AvgLFVCCW = 0; CON = 0; AvgLowerBulge = 0; AvgUpperBulge = 0; AvgLowerLRO = 0; AvgUpperLRO = 0; AvgRRO = 0; AvgLowerDEP = 0; AvgUpperDEP = 0;
                
                gradetotal = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                gradeA = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                gradeB = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                gradeC = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
                gradeD = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
                gradeE = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));
                
                if (grade != "AllChecked" && grade != "SelectGrade")
                {
                    AvgRFVCW = validateDT(dt, "RFVCW", uniqrecipedt, i, ("GradeRFVCW='" + grade + "'")); 
                    AvgRFVCCW = validateDT(dt, "RFVCCW", uniqrecipedt, i, ("GradeRFVCW='" + grade + "'")); 
                    AvgH1RFVCW = validateDT(dt, "H1RFVCW", uniqrecipedt, i, ("GradeH1RFVCW='" + grade + "'"));
                    AvgH1RFVCCW = validateDT(dt, "H1RFVCCW", uniqrecipedt, i, ("GradeH1RFVCCW='" + grade + "'"));
                    AvgLFVCW = validateDT(dt, "LFVCW", uniqrecipedt, i, ("GradeLFVCW='" + grade + "'"));
                    AvgLFVCCW = validateDT(dt, "LFVCCW", uniqrecipedt, i, ("GradeLFVCCW='" + grade + "'"));
                    CON = validateDT(dt, "CONICITY", uniqrecipedt, i, ("GradeCONICITY='" + grade + "'"));
                    AvgLowerBulge = validateDT(dt, "LowerBulge", uniqrecipedt, i, ("GradeLowerBulge='" + grade + "'"));
                    AvgUpperBulge = validateDT(dt, "UpperBulge", uniqrecipedt, i, ("GradeUpperBulge='" + grade + "'"));
                    AvgLowerLRO = validateDT(dt, "LowerLRO", uniqrecipedt, i, ("GradeLowerLRO='" + grade + "'"));
                    AvgUpperLRO = validateDT(dt, "UpperLRO", uniqrecipedt, i, ("GradeUpperLRO='" + grade + "'"));
                    AvgRRO = validateDT(dt, "RRO", uniqrecipedt, i, ("GradeRRO='" + grade + "'"));
                    AvgLowerDEP = validateDT(dt, "LowerDepression", uniqrecipedt, i, ("GradeLowerDepression='" + grade + "'"));
                    AvgUpperDEP = validateDT(dt, "UpperDepression", uniqrecipedt, i, ("GradeUpperDepression='" + grade + "'"));

                }
                else if (grade == "AllChecked")
                {
                    AvgRFVCW = validateDT(dt, "RFVCW", uniqrecipedt, i, (""));
                    AvgRFVCCW = validateDT(dt, "RFVCCW", uniqrecipedt, i, (""));
                    AvgH1RFVCW = validateDT(dt, "H1RFVCW", uniqrecipedt, i, (""));
                    AvgH1RFVCCW = validateDT(dt, "H1RFVCCW", uniqrecipedt, i, (""));
                    AvgLFVCW = validateDT(dt, "LFVCW", uniqrecipedt, i, (""));
                    AvgLFVCCW = validateDT(dt, "LFVCCW", uniqrecipedt, i, (""));
                    CON = validateDT(dt, "CONICITY", uniqrecipedt, i, (""));
                    AvgLowerBulge = validateDT(dt, "LowerBulge", uniqrecipedt, i, (""));
                    AvgUpperBulge = validateDT(dt, "UpperBulge", uniqrecipedt, i, (""));
                    AvgLowerLRO = validateDT(dt, "LowerLRO", uniqrecipedt, i, (""));
                    AvgUpperLRO = validateDT(dt, "UpperLRO", uniqrecipedt, i, (""));
                    AvgRRO = validateDT(dt, "RRO", uniqrecipedt, i, (""));
                    AvgLowerDEP = validateDT(dt, "LowerDepression", uniqrecipedt, i, (""));
                    AvgUpperDEP = validateDT(dt, "UpperDepression", uniqrecipedt, i, (""));

                }
                gradetotal_ += gradetotal;
                gradeA_ += gradeA;
                gradeB_ += gradeB;
                gradeC_ += gradeC;
                gradeD_ += gradeD;
                gradeE_ += gradeE;
                AvgRFVCW_ += AvgRFVCW;
                AvgRFVCCW_ += AvgRFVCCW;
                AvgH1RFVCW_ += AvgH1RFVCW;
                AvgH1RFVCCW_ += AvgH1RFVCCW;
                AvgLFVCW_ += AvgLFVCW;
                AvgLFVCCW_ += AvgLFVCCW;
                CON_ += CON;
                AvgLowerBulge_ += AvgLowerBulge;
                AvgUpperBulge_ += AvgUpperBulge;
                AvgLowerLRO_ += AvgLowerLRO;
                AvgUpperLRO_+= AvgUpperLRO;
                AvgRRO_ += AvgRRO;
                AvgLowerDEP_ += AvgLowerDEP;
                AvgUpperDEP_ += AvgUpperDEP;

                DataRow dr = gridviewdt.NewRow();
                
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = gradetotal.ToString();
                    dr[2] = gradeA.ToString();
                    dr[3] = gradeB.ToString();
                    dr[4] = gradeC.ToString();
                    dr[5] = gradeD.ToString();
                    dr[6] = gradeE.ToString();
                    dr[7] = AvgRFVCW.ToString();
                    dr[8] = AvgRFVCCW.ToString();
                    dr[9] = AvgH1RFVCW.ToString();
                    dr[10] = AvgH1RFVCCW.ToString();
                    dr[11] = AvgLFVCW.ToString();
                    dr[12] = AvgLFVCCW.ToString();
                    dr[13] = CON.ToString();
                    dr[14] = AvgLowerBulge.ToString();
                    dr[15] = AvgUpperBulge.ToString();
                    dr[16] = AvgLowerLRO.ToString();
                    dr[17] = AvgUpperLRO.ToString();
                    dr[18] = AvgRRO.ToString();
                    dr[19] = AvgLowerDEP.ToString();
                    dr[20] = AvgUpperDEP.ToString();
                
                gridviewdt.Rows.Add(dr);
                
            }
            DataRow ndr = gridviewdt.NewRow();
            ndr[0] = "";
            ndr[1] = "";
            ndr[2] = "";
            ndr[3] = "";
            ndr[4] = "";
            ndr[5] = "";
            ndr[6] = "";

            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            
                tdr[0] = "Total";
                tdr[1] = gradetotal_;
                tdr[2] = gradeA_;
                tdr[3] = gradeB_;
                tdr[4] = gradeC_;
                tdr[5] = gradeD_;
                tdr[6] = gradeE_;
                tdr[7] = AvgRFVCW_;
                tdr[8] = AvgRFVCCW_;
                tdr[9] = AvgH1RFVCW_;
                tdr[10] = AvgH1RFVCCW_;
                tdr[11] = AvgLFVCW_;
                tdr[12] = AvgLFVCCW_;
                tdr[13] = CON_;
                tdr[14] = AvgLowerBulge_;
                tdr[15] = AvgUpperBulge_;
                tdr[16] = AvgLowerLRO_;
                tdr[17] = AvgUpperLRO_;
                tdr[18] = AvgRRO_;
                tdr[19] = AvgUpperDEP_;
                tdr[20] = AvgLowerDEP_;

            gridviewdt.Rows.Add(tdr);

            grandGradetotal += gradetotal_;
            grandGradeA += gradeA_;
            grandGradeB += gradeB_;
            grandGradeC += gradeC_;
            grandGradeD += gradeD_;
            grandGradeE += gradeE_;
            grandAvgRFVCW += AvgRFVCW_;
            grandAvgRFVCCW += AvgRFVCCW_;
            grandAvgH1RFVCW += AvgH1RFVCW_;
            grandAvgH1RFVCCW += AvgH1RFVCCW_;
            grandAvgLFVCW += AvgLFVCW_;
            grandAvgLFVCCW += AvgLFVCCW_;
            grandCON += CON_;
            grandAvgLowerBulge += AvgLowerBulge_;
            grandAvgUpperBulge += AvgUpperBulge_;
            grandAvgLowerLRO += AvgLowerLRO_;
            grandAvgUpperLRO += AvgUpperLRO_;
            grandAvgRRO += AvgRRO_;
            grandAvgLowerDEP += AvgLowerDEP_;
            grandAvgUpperDEP += AvgUpperDEP_;

            if (QualityReportTBMWise.Checked)
            {
                childgridview.DataSource = gridviewdt;
                childgridview.DataBind();
            }
            else if (QualityReportRecipeWise.Checked)
            {
                performanceAvgReportTBMRecipeWiseMainGridView.DataSource = gridviewdt;
                performanceAvgReportTBMRecipeWiseMainGridView.DataBind();

            }

        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceAvgReportMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceAvgReportMainGridView.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private string wcquery(string query)
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name from wcmaster where " + query + " ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "1")
                            flag = flag + "or" + " " + "machineName = '" + myConnection.reader[0] + "'";
                        else
                            flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "1")
                            flag = "machineName = '" + myConnection.reader[0] + "'";
                        else

                            flag = "wcname = '" + myConnection.reader[0] + "'";

                    }

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private string createQuery(String wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "ID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
        private string createwcIDQuery(String wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "wcID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
        private string TotalprodataformatDate(String fromDate, String toDate)
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

                    flag1 = fmonth + "-" + fday + "-" + fyear + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = tmonth + "-" + tday + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";


                    flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' ))";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

            }
            return flag;
        }

        private void fillSizedropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataBind();
        }
        private void fillDesigndropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataBind();
        }
        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 01 April 2011
            //Date Updated  : 01 April 2011
            //Revision No.  : 01

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (myConnection.reader[0].ToString() != "")
                        flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);


            return flag;
        }
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {
            productionReport2RecipeWiseMainPanel.Visible = true;
            performanceAvgReportMainPanel.Visible = false;
            performanceAvgReportTBMRecipeWiseMainGridView.DataSource = null;
            performanceAvgReportTBMRecipeWiseMainGridView.DataBind();
        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {

            productionReport2RecipeWiseMainPanel.Visible = false;
            performanceAvgReportMainPanel.Visible = true;
            performanceAvgReportMainGridView.DataSource = null;
            performanceAvgReportMainGridView.DataBind();
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));


            }
            else if (ddl.ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

            }


        }

        protected void expToExcel_Click(object sender, EventArgs e)
        {
            if (tuoFilterOptionDropDownList.SelectedIndex == 1)
                option = "1";
            else if (tuoFilterOptionDropDownList.SelectedIndex == 2)
                option = "2";

            grade = GradeDropDownList.SelectedItem.ToString();
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);

            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);
            if (tempString2[1] != "0")
            {
                if (tempString2[3] == "0")
                {
                    rType = "monthWise";
                    rToMonth = tempString2[5];
                    rToYear = tempString2[6];
                }
                else if (tempString2[3] != "0")
                {
                    rType = "dayWise";
                    rToDate = myWebService.formatDate(tempString2[3]);
                    rFromDate = myWebService.formatDate(tempString2[4]);
                }

                wcnamequery = wcquery(query);
                dtnadtime1 = (rType == "dayWise") ? TotalprodataformatDate(rToDate, rFromDate) : null;

                if (QualityReportTBMWise.Checked)
                {
                    performanceAvgReportMainGridView.Visible = true;
                    productionReport2RecipeWiseMainPanel.Visible = false;
                    excelReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    performanceAvgReportMainGridView.Visible = false;
                    productionReport2RecipeWiseMainPanel.Visible = true;
                    excelReportRecipeWise("default", rToDate, rFromDate);
                }
            }
            else
            {
                performanceAvgReportTBMRecipeWiseMainGridView.DataSource = null;
                performanceAvgReportTBMRecipeWiseMainGridView.DataBind();

            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public void excelReport(string query)
        {
            showDownload.Text = "";
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query, con);
                var dr = cmd.ExecuteReader();

                // for reciepe column
                if (dr.HasRows)
                {
                    xlApp = new Excel.ApplicationClass();
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkBook.CheckCompatibility = false;
                    xlWorkBook.DoNotPromptForConvert = true;

                    //Get PID
                    HandleRef hwnd = new HandleRef(xlApp, (IntPtr)xlApp.Hwnd);
                    GetWindowThreadProcessId(hwnd, out pid);

                    xlApp.Visible = true; // ensure that the excel app is visible.
                    xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                    Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary
                    
                    xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                    xlWorkSheet.Cells[1, 2] = "Average and standard Performance Report";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.get_Range("D3", "H3").Merge(misValue);
                    xlWorkSheet.get_Range("H3", "W3").Merge(misValue);                        
                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "W3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                    xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;
                      
                    xlWorkSheet.Cells[3, 1] = "Machine Name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "Uni Grade";
                    xlWorkSheet.Cells[3, 7] = "Average of selected parameter";
                        
                    xlWorkSheet.Cells[4, 4] = "GradeA";
                    xlWorkSheet.Cells[4, 5] = "GradeB";
                    xlWorkSheet.Cells[4, 6] = "GradeC";
                    xlWorkSheet.Cells[4, 7] = "GradeD";
                    xlWorkSheet.Cells[4, 8] = "GradeE";
                    xlWorkSheet.Cells[4, 9] = "RFVCW";
                    xlWorkSheet.Cells[4, 10] = "RFVCCW";
                    xlWorkSheet.Cells[4, 11] = "H1RFVCW";
                    xlWorkSheet.Cells[4, 12] = "H1RFVCW";
                    xlWorkSheet.Cells[4, 13] = "LFVCW";
                    xlWorkSheet.Cells[4, 14] = "LFVCCW";

                    xlWorkSheet.Cells[4, 15] = "CONICITY";
                    xlWorkSheet.Cells[4, 16] = "LowerBulge";
                    xlWorkSheet.Cells[4, 17] = "UpperBulge";
                    xlWorkSheet.Cells[4, 18] = "LowerLRO";
                    xlWorkSheet.Cells[4, 19] = "UpperLRO";
                    xlWorkSheet.Cells[4, 20] = "RRO";
                    xlWorkSheet.Cells[4, 21] = "LowerDep";
                    xlWorkSheet.Cells[4, 22] = "UpperDep";
                        
                   ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                    xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("D4", "V4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                    xlWorkSheet.get_Range("A3", "V3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("A3", "V3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    xlWorkSheet.get_Range("A4", "V4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        
                    while (dr.Read())
                    {
                        excelReportRecipeWise(dr["workCenterName"].ToString().Trim(), rToDate, rFromDate);
                    }
                    xlWorkSheet.get_Range("A" + (rowCount + 1), "V" + (rowCount + 1)).Merge(misValue);
                    xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                    xlWorkSheet.Cells[rowCount + 2, 3] = grandGradetotal;

                    xlWorkSheet.Cells[rowCount + 2,  4] = grandGradeA;
                    xlWorkSheet.Cells[rowCount + 2,  5] = grandGradeB;
                    xlWorkSheet.Cells[rowCount + 2,  6] = grandGradeC;
                    xlWorkSheet.Cells[rowCount + 2,  7] = grandGradeD;
                    xlWorkSheet.Cells[rowCount + 2,  8] = grandGradeE;
                    xlWorkSheet.Cells[rowCount + 2,  9] = grandAvgRFVCW;
                    xlWorkSheet.Cells[rowCount + 2,  10] = grandAvgRFVCCW;
                    xlWorkSheet.Cells[rowCount + 2,  11] = grandAvgH1RFVCW;
                    xlWorkSheet.Cells[rowCount + 2,  12] = grandAvgH1RFVCCW;
                    xlWorkSheet.Cells[rowCount + 2,  13] = grandAvgLFVCW;
                    xlWorkSheet.Cells[rowCount + 2,  14] = grandAvgLFVCCW;
                    xlWorkSheet.Cells[rowCount + 2,  15] = grandCON;
                    xlWorkSheet.Cells[rowCount + 2,  16] = grandAvgLowerBulge;
                    xlWorkSheet.Cells[rowCount + 2,  17] = grandAvgUpperBulge;
                    xlWorkSheet.Cells[rowCount + 2,  18] = grandAvgLowerLRO;
                    xlWorkSheet.Cells[rowCount + 2,  19] = grandAvgUpperLRO;
                    xlWorkSheet.Cells[rowCount + 2, 20] = grandAvgRRO;
                    xlWorkSheet.Cells[rowCount + 2, 21] = grandAvgLowerDEP;
                    xlWorkSheet.Cells[rowCount + 2,  22] = grandAvgUpperDEP;

                    xlWorkSheet.get_Range("A1", "V" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "V" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report Curing WC wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                KillProcess(pid, "EXCEL");
            }
        }
        public void excelReportRecipeWise(string wcName, string rToDate, string rFromDate)
        {
            string query;
            DataTable dt = new DataTable();
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("A", typeof(string));
            gridviewdt.Columns.Add("B", typeof(string));
            gridviewdt.Columns.Add("C", typeof(string));
            gridviewdt.Columns.Add("D", typeof(string));
            gridviewdt.Columns.Add("E", typeof(string));
            gridviewdt.Columns.Add("AvgRFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgRFVCCW", typeof(string));
            gridviewdt.Columns.Add("AvgH1RFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgH1RFVCCW", typeof(string));
            gridviewdt.Columns.Add("AvgLFVCW", typeof(string));
            gridviewdt.Columns.Add("AvgLFVCCW", typeof(string));
            gridviewdt.Columns.Add("CON", typeof(string));
            gridviewdt.Columns.Add("AvgLowerBulge", typeof(string));
            gridviewdt.Columns.Add("AvgUpperBulge", typeof(string));
            gridviewdt.Columns.Add("AvgLowerLRO", typeof(string));
            gridviewdt.Columns.Add("AvgUpperLRO", typeof(string));
            gridviewdt.Columns.Add("AvgRRO", typeof(string));
            gridviewdt.Columns.Add("AvgLowerDEP", typeof(string));
            gridviewdt.Columns.Add("AvgUpperDEP", typeof(string));

            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("RFVCW", typeof(double));
            dt.Columns.Add("RFVCCW", typeof(double));
            dt.Columns.Add("H1RFVCW", typeof(double));
            dt.Columns.Add("H1RFVCCW", typeof(double));
            dt.Columns.Add("LFVCW", typeof(double));
            dt.Columns.Add("LFVCCW", typeof(double));
            dt.Columns.Add("CONICITY", typeof(double));
            dt.Columns.Add("LowerBulge", typeof(double));
            dt.Columns.Add("UpperBulge", typeof(double));
            dt.Columns.Add("LowerLRO", typeof(double));
            dt.Columns.Add("UpperLRO", typeof(double));
            dt.Columns.Add("RRO", typeof(double));
            dt.Columns.Add("LowerDepression", typeof(double));
            dt.Columns.Add("UpperDepression", typeof(double));
            dt.Columns.Add("GradeRFVCW", typeof(string));
            dt.Columns.Add("GradeH1RFVCW", typeof(string));
            dt.Columns.Add("GradeH1RFVCCW", typeof(string));
            dt.Columns.Add("GradeLFVCW", typeof(string));
            dt.Columns.Add("GradeLFVCCW", typeof(string));
            dt.Columns.Add("GradeCONICITY", typeof(string));
            dt.Columns.Add("GradeLowerBulge", typeof(string));
            dt.Columns.Add("GradeUpperBulge", typeof(string));
            dt.Columns.Add("GradeLowerLRO", typeof(string));
            dt.Columns.Add("GradeUpperLRO", typeof(string));
            dt.Columns.Add("GradeRRO", typeof(string));
            dt.Columns.Add("GradeLowerDepression", typeof(string));
            dt.Columns.Add("GradeUpperDepression", typeof(string));
            double gradetotal, gradeA, gradeB, gradeC, gradeD, gradeE, AvgRFVCW, AvgRFVCCW, AvgH1RFVCW, AvgH1RFVCCW, AvgLFVCW, AvgLFVCCW, CON, AvgLowerBulge, AvgUpperBulge, AvgLowerLRO, AvgUpperLRO, AvgRRO, AvgLowerDEP, AvgUpperDEP;

            query = "";
            int colCount = 1, mergeCount = 0, typeCount = 0;
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportRecipeWise.Checked)
            {
                typeCount = 0;
                xlApp = new Excel.ApplicationClass();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkBook.CheckCompatibility = false;
                xlWorkBook.DoNotPromptForConvert = true;

                //Get PID
                HandleRef hwnd = new HandleRef(xlApp, (IntPtr)xlApp.Hwnd);
                GetWindowThreadProcessId(hwnd, out pid);

                xlApp.Visible = true; // ensure that the excel app is visible.
                xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

                xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                xlWorkSheet.Cells[1, 2] = "Average and standard Performance Report";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("C3", "G3").Merge(misValue);
                xlWorkSheet.get_Range("H3", "V3").Merge(misValue);

                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "V3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "Uni Grade";
                xlWorkSheet.Cells[3, 8] = "Average of selected parameter";

                xlWorkSheet.Cells[4, 3] = "GradeA";
                xlWorkSheet.Cells[4, 4] = "GradeB";
                xlWorkSheet.Cells[4, 5] = "GradeC";
                xlWorkSheet.Cells[4, 6] = "GradeD";
                xlWorkSheet.Cells[4, 7] = "GradeE";
                xlWorkSheet.Cells[4, 8] = "RFVCW";
                xlWorkSheet.Cells[4, 9] = "RFVCCW";
                xlWorkSheet.Cells[4, 10] = "H1RFVCW";
                xlWorkSheet.Cells[4, 11] = "H1RFVCW";
                xlWorkSheet.Cells[4, 12] = "LFVCW";
                xlWorkSheet.Cells[4, 13] = "LFVCCW";

                xlWorkSheet.Cells[4, 14] = "CONICITY";
                xlWorkSheet.Cells[4, 15] = "LowerBulge";
                xlWorkSheet.Cells[4, 16] = "UpperBulge";
                xlWorkSheet.Cells[4, 17] = "LowerLRO";
                xlWorkSheet.Cells[4, 18] = "UpperLRO";
                xlWorkSheet.Cells[4, 19] = "RRO";
                xlWorkSheet.Cells[4, 20] = "LowerDep";
                xlWorkSheet.Cells[4, 21] = "UpperDep";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("D4", "U4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("A3", "U3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "U3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "U4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        
                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeLowerRRO, GradeUpperRRO, GradeLowerDepression, GradeUpperDepression FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeLowerRRO, GradeUpperRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, RFVCW, RFVCCW, H1RFVCW, H1RFVCCW, LFVCW, LFVCCW, CONICITY, LowerBulge, UpperBulge, LowerLRO, UpperLRO, RRO, LowerDepression, UpperDepression, GradeRFVCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeLowerRRO, GradeUpperRRO, GradeLowerDepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                typeCount = 1;
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                    query = getqueryWCWise(tablename, "wcname='" + wcName + "'", dtnadtime);
                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    query = getqueryWCWise(tablename, "machineName='" + wcName + "'", dtnadtime);
                else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    query = getqueryWCWise(tablename, "wcname='" + wcName + "'", dtnadtime); 

                xlWorkSheet.Cells[rowCount + 1, 1] = wcName;
                mergeCount = rowCount;
            }
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
            catch (Exception exp)
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");

            gradetotal_ = 0; gradeA_ = 0; gradeB_ = 0; gradeC_ = 0; gradeD_ = 0; gradeE_ = 0; AvgRFVCW_ = 0; AvgRFVCCW_ = 0; AvgH1RFVCW_ = 0; AvgH1RFVCCW_ = 0; AvgLFVCW_ = 0; AvgLFVCCW_ = 0; CON_ = 0; AvgLowerBulge_ = 0; AvgUpperBulge_ = 0; AvgLowerLRO_ = 0; AvgUpperLRO_ = 0; AvgRRO_ = 0; AvgLowerDEP_ = 0; AvgUpperDEP_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                gradetotal = 0; gradeA = 0; gradeB = 0; gradeC = 0; gradeD = 0; gradeE = 0; AvgRFVCW = 0; AvgRFVCCW = 0; AvgH1RFVCW = 0; AvgH1RFVCCW = 0; AvgLFVCW = 0; AvgLFVCCW = 0; CON = 0; AvgLowerBulge = 0; AvgUpperBulge = 0; AvgLowerLRO = 0; AvgUpperLRO = 0; AvgRRO = 0; AvgLowerDEP = 0; AvgUpperDEP = 0;
                rowCount++;
                gradetotal = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                gradeA = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                gradeB = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                gradeC = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
                gradeD = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
                gradeE = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));

                if (grade != "AllChecked" && grade != "SelectGrade")
                {
                    AvgRFVCW = validateDT(dt, "RFVCW", uniqrecipedt, i, ("GradeRFVCW='" + grade + "'"));
                    AvgRFVCCW = validateDT(dt, "RFVCCW", uniqrecipedt, i, ("GradeRFVCW='" + grade + "'"));
                    AvgH1RFVCW = validateDT(dt, "H1RFVCW", uniqrecipedt, i, ("GradeH1RFVCW='" + grade + "'"));
                    AvgH1RFVCCW = validateDT(dt, "H1RFVCCW", uniqrecipedt, i, ("GradeH1RFVCCW='" + grade + "'"));
                    AvgLFVCW = validateDT(dt, "LFVCW", uniqrecipedt, i, ("GradeLFVCW='" + grade + "'"));
                    AvgLFVCCW = validateDT(dt, "LFVCCW", uniqrecipedt, i, ("GradeLFVCCW='" + grade + "'"));
                    CON = validateDT(dt, "CONICITY", uniqrecipedt, i, ("GradeCONICITY='" + grade + "'"));
                    AvgLowerBulge = validateDT(dt, "LowerBulge", uniqrecipedt, i, ("GradeLowerBulge='" + grade + "'"));
                    AvgUpperBulge = validateDT(dt, "UpperBulge", uniqrecipedt, i, ("GradeUpperBulge='" + grade + "'"));
                    AvgLowerLRO = validateDT(dt, "LowerLRO", uniqrecipedt, i, ("GradeLowerLRO='" + grade + "'"));
                    AvgUpperLRO = validateDT(dt, "UpperLRO", uniqrecipedt, i, ("GradeUpperLRO='" + grade + "'"));
                    AvgRRO = validateDT(dt, "RRO", uniqrecipedt, i, ("GradeRRO='" + grade + "'"));
                    AvgLowerDEP = validateDT(dt, "LowerDepression", uniqrecipedt, i, ("GradeLowerDepression='" + grade + "'"));
                    AvgUpperDEP = validateDT(dt, "UpperDepression", uniqrecipedt, i, ("GradeUpperDepression='" + grade + "'"));
                }
                else if (grade == "AllChecked")
                {
                    AvgRFVCW = validateDT(dt, "RFVCW", uniqrecipedt, i, (""));
                    AvgRFVCCW = validateDT(dt, "RFVCCW", uniqrecipedt, i, (""));
                    AvgH1RFVCW = validateDT(dt, "H1RFVCW", uniqrecipedt, i, (""));
                    AvgH1RFVCCW = validateDT(dt, "H1RFVCCW", uniqrecipedt, i, (""));
                    AvgLFVCW = validateDT(dt, "LFVCW", uniqrecipedt, i, (""));
                    AvgLFVCCW = validateDT(dt, "LFVCCW", uniqrecipedt, i, (""));
                    CON = validateDT(dt, "CONICITY", uniqrecipedt, i, (""));
                    AvgLowerBulge = validateDT(dt, "LowerBulge", uniqrecipedt, i, (""));
                    AvgUpperBulge = validateDT(dt, "UpperBulge", uniqrecipedt, i, (""));
                    AvgLowerLRO = validateDT(dt, "LowerLRO", uniqrecipedt, i, (""));
                    AvgUpperLRO = validateDT(dt, "UpperLRO", uniqrecipedt, i, (""));
                    AvgRRO = validateDT(dt, "RRO", uniqrecipedt, i, (""));
                    AvgLowerDEP = validateDT(dt, "LowerDepression", uniqrecipedt, i, (""));
                    AvgUpperDEP = validateDT(dt, "UpperDepression", uniqrecipedt, i, (""));

                }
                gradetotal_ += gradetotal;
                gradeA_ += gradeA;
                gradeB_ += gradeB;
                gradeC_ += gradeC;
                gradeD_ += gradeD;
                gradeE_ += gradeE;
                AvgRFVCW_ += AvgRFVCW;
                AvgRFVCCW_ += AvgRFVCCW;
                AvgH1RFVCW_ += AvgH1RFVCW;
                AvgH1RFVCCW_ += AvgH1RFVCCW;
                AvgLFVCW_ += AvgLFVCW;
                AvgLFVCCW_ += AvgLFVCCW;
                CON_ += CON;
                AvgLowerBulge_ += AvgLowerBulge;
                AvgUpperBulge_ += AvgUpperBulge;
                AvgLowerLRO_ += AvgLowerLRO;
                AvgUpperLRO_ += AvgUpperLRO;
                AvgRRO_ += AvgRRO;
                AvgLowerDEP_ += AvgLowerDEP;
                AvgUpperDEP_ += AvgUpperDEP;

                DataRow dr = gridviewdt.NewRow();

                xlWorkSheet.Cells[rowCount, colCount + typeCount + 0] = uniqrecipedt.Rows[i][0].ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = gradetotal.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = gradeA.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = gradeB.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = gradeC.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = gradeD.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = gradeE.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = AvgRFVCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = AvgRFVCCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = AvgH1RFVCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = AvgH1RFVCCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = AvgLFVCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = AvgLFVCCW.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = CON.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = AvgLowerBulge.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 15] = AvgUpperBulge.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 16] = AvgLowerLRO.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 17] = AvgUpperLRO.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 18] = AvgRRO.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 19] = AvgLowerDEP.ToString();
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 20] = AvgUpperDEP.ToString();

                gridviewdt.Rows.Add(dr);

            }
            DataRow ndr = gridviewdt.NewRow();
            
            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();

            rowCount += 2;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 0] = "Total";
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = gradetotal_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = gradeA_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = gradeB_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = gradeC_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = gradeD_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = gradeE_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = AvgRFVCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = AvgRFVCCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = AvgH1RFVCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = AvgH1RFVCCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = AvgLFVCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = AvgLFVCCW_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = CON_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = AvgLowerBulge_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 15] = AvgUpperBulge_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 16] = AvgLowerLRO_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 17] = AvgUpperLRO_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 18] = AvgRRO_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 19] = AvgLowerDEP_;
            xlWorkSheet.Cells[rowCount, colCount + typeCount + 20] = AvgUpperDEP_;

            gridviewdt.Rows.Add(tdr);


            grandGradetotal += gradetotal_;
            grandGradeA += gradeA_;
            grandGradeB += gradeB_;
            grandGradeC += gradeC_;
            grandGradeD += gradeD_;
            grandGradeE += gradeE_;
            grandAvgRFVCW += AvgRFVCW_;
            grandAvgRFVCCW += AvgRFVCCW_;
            grandAvgH1RFVCW += AvgH1RFVCW_;
            grandAvgH1RFVCCW += AvgH1RFVCCW_;
            grandAvgLFVCW += AvgLFVCW_;
            grandAvgLFVCCW += AvgLFVCCW_;
            grandCON += CON_;
            grandAvgLowerBulge += AvgLowerBulge_;
            grandAvgUpperBulge += AvgUpperBulge_;
            grandAvgLowerLRO += AvgLowerLRO_;
            grandAvgUpperLRO += AvgUpperLRO_;
            grandAvgRRO += AvgRRO_;
            grandAvgLowerDEP += AvgLowerDEP_;
            grandAvgUpperDEP += AvgUpperDEP_;

            if (QualityReportTBMWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "U" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "U" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "U" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report Curing WC wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
             
            }
        }

        private void KillProcess(int pid, string processName)
        {
            // to kill current process of excel
            System.Diagnostics.Process[] AllProcesses = System.Diagnostics.Process.GetProcessesByName(processName);
            foreach (System.Diagnostics.Process process in AllProcesses)
            {
                if (process.Id == pid)
                {
                    process.Kill();
                }
            }
            AllProcesses = null;
        } 
    }
}
