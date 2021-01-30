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
    public partial class TUOReport2 : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        int total_, A_, B_, E_, RFV_ , R1H_ , LFV_ , CON_ , BLG_ , LRO_ , CRRO_ , DEF_ , DRFV_ , DR1H_ , DLFV_ , DCON_ , DBLG_ , DLRO_ , DCRRO_ , DDEF_ , ERFV_ , ER1H_ , ELFV_ , ECON_ , EBLG_ , ELRO_ , ECRRO_ , EDEF_;

        Double pA_, pB_, pC_, pD_, pE_;
        public Double grandtotal, grandA, grandB, grandRFV, grandR1H, grandE, grandLFV, grandCON, grandBLG, grandLRO, grandCRRO, grandDEF, grandDRFV, grandDR1H, grandDLFV, grandDCON, grandDBLG, grandDLRO, grandDCRRO, grandDDEF, grandERFV, grandER1H, grandELFV, grandECON, grandEBLG, grandELRO, grandECRRO, grandEDEF;
        string tablename;
        DataRow tdr;
        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", machineStatus = "", wCenterName = "default";
        string query = "";
        string[] tempString2;
        int rowCount = 4, pid = -1;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "performanceReportCuringWCWise.xlsx";
        string filepath;

        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;


        #endregion

        public TUOReport2()
        {
            filepath = myWebService.getExcelPath();
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
            productionReport2TBMWiseMainPanel.Visible = false;
            productionReport2RecipeWiseMainPanel.Visible = false;

        }


        protected void magicButton_Click(object sender, EventArgs e)
        {

            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });

            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";

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
                    productionReport2TBMWiseMainPanel.Visible = true;
                    productionReport2RecipeWiseMainPanel.Visible = false;
                    showReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    productionReport2TBMWiseMainPanel.Visible = false;
                    productionReport2RecipeWiseMainPanel.Visible = true;
                    showReportRecipeWise(performanceReport2TBMWiseMainGridView, "", rToDate, rFromDate);
                }

                switch (option)
                {
                    case "2":
                    grandA = Math.Round(((float)(grandA * 100) / grandtotal), 1);
                    grandB = Math.Round(((float)(grandB * 100) / grandtotal), 1);
                    grandRFV = Math.Round(((float)(grandRFV * 100) / grandtotal), 1);
                    grandR1H = Math.Round(((float)(grandR1H * 100) / grandtotal), 1);
                    grandE = Math.Round(((float)(grandE * 100) / grandtotal), 1);
                    break;
                }
            }
            else
            {
                performanceReport2TBMRecipeWiseMainGridView.DataSource = null;
                performanceReport2TBMRecipeWiseMainGridView.DataBind();

            }

        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //wcnamequery = wcquery(query);
            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReport2TBMWiseMainGridView")
                    {

                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReport2TBMWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMWiseChildGridView"));
                        showReportRecipeWise(childGridView, workcentername, rToDate, rFromDate);


                    }
                    
                }
                
        }
        protected string getqueryWCWise(string tableName, string wcNameString, string dtnadtime)
        {
            if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
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
            gridviewdt.Columns.Add("E", typeof(string));
            gridviewdt.Columns.Add("RFV", typeof(string));
            gridviewdt.Columns.Add("R1H", typeof(string));
            gridviewdt.Columns.Add("LFV", typeof(string));
            gridviewdt.Columns.Add("CON", typeof(string));
            gridviewdt.Columns.Add("BLG", typeof(string));
            gridviewdt.Columns.Add("LRO", typeof(string));
            gridviewdt.Columns.Add("CRRO", typeof(string));
            gridviewdt.Columns.Add("DEF", typeof(string));
            gridviewdt.Columns.Add("DRFV", typeof(string));
            gridviewdt.Columns.Add("DR1H", typeof(string));
            gridviewdt.Columns.Add("DLFV", typeof(string));
            gridviewdt.Columns.Add("DCON", typeof(string));
            gridviewdt.Columns.Add("DBLG", typeof(string));
            gridviewdt.Columns.Add("DLRO", typeof(string));
            gridviewdt.Columns.Add("DCRRO", typeof(string));
            gridviewdt.Columns.Add("DDEF", typeof(string));
            gridviewdt.Columns.Add("ERFV", typeof(string));
            gridviewdt.Columns.Add("ER1H", typeof(string));
            gridviewdt.Columns.Add("ELFV", typeof(string));
            gridviewdt.Columns.Add("ECON", typeof(string));
            gridviewdt.Columns.Add("EBLG", typeof(string));
            gridviewdt.Columns.Add("ELRO", typeof(string));
            gridviewdt.Columns.Add("ECRRO", typeof(string));
            gridviewdt.Columns.Add("EDEF", typeof(string));
            
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("GradeRFVCW", typeof(string));
            dt.Columns.Add("GradeRFVCCW", typeof(string));
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
            dt.Columns.Add("GradeUpperRRO", typeof(string));
            dt.Columns.Add("gradelowerdepression", typeof(string));
            dt.Columns.Add("GradeUpperDepression", typeof(string));
                        
            query = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);
            int total, A, B, E, RFV, R1H, LFV, CON, BLG, LRO, CRRO, DEF, DRFV, DR1H, DLFV, DCON, DBLG, DLRO, DCRRO, DDEF, ERFV, ER1H, ELFV, ECON, EBLG, ELRO, ECRRO, EDEF;
            total_ = 0; A_ = 0; B_ = 0; E_ = 0; RFV_ = 0; R1H_ = 0; LFV_ = 0; CON_ = 0; BLG_ = 0; LRO_ = 0; CRRO_ = 0; DEF_ = 0; DRFV_ = 0; DR1H_ = 0; DLFV_ = 0; DCON_ = 0; DBLG_ = 0; DLRO_ = 0; DCRRO_ = 0; DDEF_ = 0; ERFV_ = 0; ER1H_ = 0; ELFV_ = 0; ECON_ = 0; EBLG_ = 0; ELRO_ = 0; ECRRO_ = 0; EDEF_ = 0;
            Double pA, pB, pE, pRFV, pR1H, pLFV, pCON, pBLG, pLRO, pCRRO, pDEF, pDRFV, pDR1H, pDLFV, pDCON, pDBLG, pDLRO, pDCRRO, pDDEF, pERFV, pER1H, pELFV, pECON, pEBLG, pELRO, pECRRO, pEDEF;
            
            if (QualityReportRecipeWise.Checked)
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                    query = getqueryWCWise(tablename, "wcname='" + wcName + "'", dtnadtime);
                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    query = getqueryWCWise(tablename, "machineName='" + wcName + "'", dtnadtime);
                else if (tempString2[tempString2.Length - 1].ToString() == "2")
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
                        
            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; RFV = 0; R1H = 0; E = 0;
                pA = 0; pB = 0; pE = 0; pRFV = 0; pR1H = 0; pLFV = 0; pCON = 0; pBLG = 0; pLRO = 0; pCRRO = 0; pDEF = 0; pDRFV = 0; pDR1H = 0; pDLFV = 0; pDCON = 0; pDBLG = 0; pDLRO = 0; pDCRRO = 0; pDDEF = 0; pERFV = 0; pER1H = 0; pELFV = 0; pECON = 0; pEBLG = 0; pELRO = 0; pECRRO = 0; pEDEF = 0;

                total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));
                RFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'C' or GradeRFVCCW='C')"));
                R1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'C' or GradeH1RFVCCW='C')"));
                LFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'C' or GradeLFVCCW='C')"));
                CON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  GradeCONICITY = 'C'"));
                BLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'C' or GradeUpperBulge='C')"));
                LRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'C' or GradeUpperLRO='C')"));
                CRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'C' or GradeUpperRRO='C')"));
                DEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (gradelowerdepression= 'C' or GradeUpperDepression='C')"));
                DRFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'D' or GradeRFVCCW='D')"));
                DR1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'D' or GradeH1RFVCCW='D')"));
                DLFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'D' or GradeLFVCCW='D')"));
                DCON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and GradeCONICITY = 'D'"));
                DBLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'D' or GradeUpperBulge='D')"));
                DLRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'D' or GradeUpperLRO='D')"));
                DCRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'D' or GradeUpperRRO='D')"));
                DDEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (gradelowerdepression= 'D' or GradeUpperDepression='D')"));
                ERFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'E' or GradeRFVCCW='E')"));

                ER1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'E' or GradeH1RFVCCW='E')"));
                ELFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'E' or GradeLFVCCW='E')"));
                ECON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and GradeCONICITY = 'E'"));
                EBLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'E' or GradeUpperBulge='E')"));
                ELRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'E' or GradeUpperLRO='E')"));
                ECRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'E' or GradeUpperRRO='E')"));
                EDEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerdepression= 'E' or GradeUpperDepression='E')"));
                
                total_ = total_ + total;
                A_ = A_ + A;
                B_ = B_ + B;
                RFV_ = RFV_ + RFV;
                R1H_ = R1H_ + R1H;
                E_ = E_ + E;
                LFV_ = LFV_ + LFV;
                CON_ = CON_ + CON;
                BLG_ = BLG_ + BLG;
                LRO_ = LRO_ + LRO;
                CRRO_ = CRRO_ + CRRO;
                DEF_ = DEF_ + DEF;
                DRFV_ = DRFV_ + DRFV;
                DR1H_ = DR1H_ + DR1H;
                DLFV_ = DLFV_ + DLFV;
                DCON_ = DCON_ + DCON;
                DBLG_ = DBLG_ + DBLG;
                DLRO_ = DLRO_ + DLRO;
                DCRRO_ = DCRRO_ + DCRRO;
                DDEF_ = DDEF_ + DDEF;
                ERFV_ = ERFV_ + ERFV;
                ER1H_ = ER1H_ + ER1H;
                ELFV_ = ELFV_ + ELFV;
                ECON_ = ECON_ + ECON;
                EBLG_ = EBLG_ + EBLG;
                ELRO_ = ELRO_ + ELRO;
                ECRRO_ = ECRRO_ + ECRRO;
                EDEF_ = EDEF_ + EDEF;
                
                DataRow dr = gridviewdt.NewRow();
                switch (option)
                {
                    case  "1":
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                    dr[2] = A.ToString();
                    dr[3] = B.ToString();
                    dr[4] = E.ToString();
                    dr[5] = RFV.ToString();
                    dr[6] = R1H.ToString();
                    dr[7] = LFV.ToString();
                    dr[8] = CON.ToString();
                    dr[9] = BLG.ToString();
                    dr[10] = LRO.ToString();
                    dr[11] = CRRO.ToString();
                    dr[12] = DEF.ToString();
                    dr[13] = DRFV.ToString();
                    dr[14] = DR1H.ToString();
                    dr[15] = DLFV.ToString();
                    dr[16] = DCON.ToString();
                    dr[17] = DBLG.ToString();
                    dr[18] = DLRO.ToString();
                    dr[19] = DCRRO.ToString();
                    dr[20] = DDEF.ToString();
                    dr[21] = ERFV.ToString();
                    dr[22] = ER1H.ToString();
                    dr[23] = ELFV.ToString();
                    dr[24] = ECON.ToString();
                    dr[25] = EBLG.ToString();
                    dr[26] = ELRO.ToString();
                    dr[27] = ECRRO.ToString();
                    dr[28] = EDEF.ToString();
                    break;
                    case "2":
                    pA = ((double)(A * 100) / total);
                    pB = ((double)(B * 100) / total);
                    pE = ((double)(E * 100) / total);
                    pRFV = ((double)(RFV * 100) / total);
                    pR1H = ((double)(R1H * 100) / total);
                    
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                    dr[2] = Math.Round(pA, 1);
                    dr[3] = Math.Round(pB, 1);

                    dr[4] = Math.Round(pE, 1);
                    dr[5] = Math.Round(pRFV, 1);
                    dr[6] = Math.Round(pR1H, 1);
                    dr[7] = Math.Round(pLFV, 1);
                    dr[8] = Math.Round(pCON, 1);
                    dr[9] = Math.Round(pBLG, 1);
                    dr[10] = Math.Round(pLRO, 1);
                    dr[11] = Math.Round(pCRRO, 1);
                    dr[12] = Math.Round(pDEF, 1);
                    dr[13] = Math.Round(pDRFV, 1);
                    dr[14] = Math.Round(pDR1H, 1);
                    dr[15] = Math.Round(pDLFV, 1);
                    dr[16] = Math.Round(pDCON, 1);
                    dr[17] = Math.Round(pDBLG, 1);
                    dr[18] = Math.Round(pDLRO, 1);
                    dr[19] = Math.Round(pDCRRO, 1);
                    dr[20] = Math.Round(pDDEF, 1);
                    dr[21] = Math.Round(pERFV, 1);
                    dr[22] = Math.Round(pER1H, 1);
                    dr[23] = Math.Round(pELFV, 1);
                    dr[24] = Math.Round(pECON, 1);
                    dr[25] = Math.Round(pEBLG, 1);
                    dr[26] = Math.Round(pELRO, 1);
                    dr[27] = Math.Round(pECRRO, 1);
                    dr[28] = Math.Round(pEDEF, 1);
                    break;
                }

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
            tdr = gridviewdt.NewRow();
            switch (option)
            {
                case "1":
                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = A_;
                tdr[3] = B_;
                tdr[4] = E_;
                tdr[5] = RFV_;
                tdr[6] = R1H_;
                tdr[7] = LFV_;
                tdr[8] = CON_;
                tdr[9] = BLG_;
                tdr[10] = LRO_;
                tdr[11] = CRRO_;
                tdr[12] = DEF_;
                tdr[13] = DRFV_;
                tdr[14] = DR1H_;
                tdr[15] = DLFV_;
                tdr[16] = DCON_;
                tdr[17] = DBLG_;
                tdr[18] = DLRO_;
                tdr[19] = DCRRO_;
                tdr[20] = DDEF_;
                tdr[21] = ERFV_;
                tdr[22] = ER1H_;
                tdr[23] = ELFV_;
                tdr[24] = ECON_;
                tdr[25] = EBLG_;
                tdr[26] = ELRO_;
                tdr[27] = ECRRO_;
                tdr[28] = EDEF_;
                break;
                case "2":
                pA_ = ((double)(A_ * 100) / total_);
                pB_ = ((double)(B_ * 100) / total_);
                pRFV = ((double)(RFV_ * 100) / total_);
                pR1H = ((double)(R1H_ * 100) / total_);
                pE_ = ((double)(E_ * 100) / total_);
                pLFV = ((double)(LFV_ * 100) / total_);
                pCON = ((double)(CON_ * 100) / total_);
                pBLG = ((double)(BLG_ * 100) / total_);
                pLRO = ((double)(LRO_ * 100) / total_);
                pCRRO = ((double)(CRRO_ * 100) / total_);
                pDEF = ((double)(DEF_ * 100) / total_);
                pDRFV = ((double)(DRFV_ * 100) / total_);
                pDR1H = ((double)(DR1H_ * 100) / total_);
                pDLFV = ((double)(DLFV_ * 100) / total_);
                pDCON = ((double)(DCON_ * 100) / total_);
                pDBLG = ((double)(DBLG_ * 100) / total_);
                pDLRO = ((double)(DLRO_ * 100) / total_);
                pDCRRO = ((double)(CRRO_ * 100) / total_);
                pDDEF = ((double)(DDEF_ * 100) / total_);
                pERFV = ((double)(ERFV_ * 100) / total_);
                pER1H = ((double)(ER1H_ * 100) / total_);
                pELFV = ((double)(ELFV_ * 100) / total_);
                pECON = ((double)(ECON_ * 100) / total_);
                pEBLG = ((double)(EBLG_ * 100) / total_);
                pELRO = ((double)(ELRO_ * 100) / total_);
                pECRRO = ((double)(ECRRO_ * 100) / total_);
                pEDEF = ((double)(EDEF_ * 100) / total_);

                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = Math.Round(pA_, 1);
                tdr[3] = Math.Round(pB_, 1);
                tdr[4] = Math.Round(pE_, 1);
                tdr[5] = Math.Round(pRFV, 1);
                tdr[6] = Math.Round(pR1H, 1);
                tdr[7] = Math.Round(pLFV, 1);
                tdr[8] = Math.Round(pCON, 1);
                tdr[9] = Math.Round(pBLG, 1);
                tdr[10] = Math.Round(pLRO, 1);
                tdr[11] = Math.Round(pCRRO, 1);
                tdr[12] = Math.Round(pDEF, 1);
                tdr[13] = Math.Round(pDRFV, 1);
                tdr[14] = Math.Round(pDR1H, 1);
                tdr[15] = Math.Round(pDLFV, 1);
                tdr[16] = Math.Round(pDCON, 1);
                tdr[17] = Math.Round(pDBLG, 1);
                tdr[18] = Math.Round(pDLRO, 1);
                tdr[19] = Math.Round(pDCRRO, 1);
                tdr[20] = Math.Round(pDDEF, 1);
                tdr[21] = Math.Round(pERFV, 1);
                tdr[22] = Math.Round(pER1H, 1);
                tdr[23] = Math.Round(pELFV, 1);
                tdr[24] = Math.Round(pECON, 1);
                tdr[25] = Math.Round(pEBLG, 1);
                tdr[26] = Math.Round(pELRO, 1);
                tdr[27] = Math.Round(pECRRO, 1);
                tdr[28] = Math.Round(pEDEF, 1);
                break;
            }

            gridviewdt.Rows.Add(tdr);


            grandtotal = grandtotal + total_;
            grandA += A_;
            grandB += B_;
            grandE += E_;
            grandRFV += RFV_;
            grandR1H += R1H_;
            grandLFV += LFV_;
            grandCON += CON_;
            grandBLG += BLG_;
            grandLRO += LRO_;
            grandCRRO += CRRO_;
            grandDEF += DEF_;
            grandDRFV += DRFV_;
            grandDR1H += DR1H_;
            grandDLFV += DLFV_;
            grandDCON += DCON_;
            grandDBLG += DBLG_;
            grandDLRO += DLRO_;
            grandDCRRO += DCRRO_;
            grandDDEF += DDEF_;
            grandERFV += ERFV_;
            grandER1H += ER1H_;
            grandELFV += ELFV_;
            grandECON += ECON_;
            grandEBLG += EBLG_;
            grandELRO += ELRO_;
            grandECRRO += ECRRO_;
            grandEDEF += EDEF_;
            

            if (QualityReportTBMWise.Checked)
            {
                childgridview.DataSource = gridviewdt;
                childgridview.DataBind();
            }
            else if (QualityReportRecipeWise.Checked)
            {
                performanceReport2TBMRecipeWiseMainGridView.DataSource = gridviewdt;
                performanceReport2TBMRecipeWiseMainGridView.DataBind();

            }


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
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceReport2TBMWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReport2TBMWiseMainGridView.DataBind();
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
            productionReport2TBMWiseMainPanel.Visible = false;
            performanceReport2TBMRecipeWiseMainGridView.DataSource = null;
            performanceReport2TBMRecipeWiseMainGridView.DataBind();
        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {

            productionReport2RecipeWiseMainPanel.Visible = false;
            productionReport2TBMWiseMainPanel.Visible = true;
            performanceReport2TBMWiseMainGridView.DataSource = null;
            performanceReport2TBMWiseMainGridView.DataBind();
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {
           Response.Clear();
           Response.AddHeader("content-disposition", "attachment;filename=PerformanceReportWithRejectionDetail" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
           Response.Charset = "";
           Response.ContentType = "application/vnd.xls";
           System.IO.StringWriter stringWrite = new System.IO.StringWriter();
           System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
           //stringWrite.Write("<table><tr><td><b>TBM Production Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + type + "</td><td><b>" + reportMasterWCProcessDropDownList.SelectedItem.Value + "</b></td></tr></table>");
           System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
           Controls.Add(form);
           
           if (QualityReportTBMWise.Checked)
           {
               productionReport2TBMWiseMainPanel.Visible = true;
               productionReport2RecipeWiseMainPanel.Visible = false;
               form.Controls.Add(productionReport2TBMWiseMainPanel);
           
           }
           else if (QualityReportRecipeWise.Checked)
           {
               productionReport2TBMWiseMainPanel.Visible = false;
               productionReport2RecipeWiseMainPanel.Visible = true;
               form.Controls.Add(productionReport2RecipeWiseMainPanel);
           
           } 
           
           form.RenderControl(htmlWrite);

           //gv.RenderControl(htmlWrite);
           Response.Write(stringWrite.ToString());
           Response.End(); 
           
           /*queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });

            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";

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
                    productionReport2TBMWiseMainPanel.Visible = true;
                    productionReport2RecipeWiseMainPanel.Visible = false;
                    excelReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    productionReport2TBMWiseMainPanel.Visible = false;
                    productionReport2RecipeWiseMainPanel.Visible = true;
                    excelReportRecipeWise("default", rToDate, rFromDate);
                }
            }*/
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
                    xlWorkSheet.Cells[1, 2] = "Performance Report with Rejection Details";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.get_Range("D3", "F3").Merge(misValue);
                    xlWorkSheet.get_Range("G3", "N3").Merge(misValue);
                    xlWorkSheet.get_Range("O3", "V3").Merge(misValue);
                    xlWorkSheet.get_Range("W3", "AD3").Merge(misValue);

                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "AD3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                    xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                    xlWorkSheet.Cells[3, 1] = "Machine Name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "Uni Grade";
                    xlWorkSheet.Cells[3, 7] = "C";
                    xlWorkSheet.Cells[3, 15] = "D";
                    xlWorkSheet.Cells[3, 23] = "E";


                    xlWorkSheet.Cells[4, 4] = "A";
                    xlWorkSheet.Cells[4, 5] = "B";
                    xlWorkSheet.Cells[4, 6] = "REJ";

                    xlWorkSheet.Cells[4, 7] = "RFV";
                    xlWorkSheet.Cells[4, 8] = "R1H";
                    xlWorkSheet.Cells[4, 9] = "LFV";
                    xlWorkSheet.Cells[4, 10] = "CON";
                    xlWorkSheet.Cells[4, 11] = "BLG";
                    xlWorkSheet.Cells[4, 12] = "LRO";
                    xlWorkSheet.Cells[4, 13] = "CRO";
                    xlWorkSheet.Cells[4, 14] = "DEF";

                    xlWorkSheet.Cells[4, 15] = "RFV";
                    xlWorkSheet.Cells[4, 16] = "R1H";
                    xlWorkSheet.Cells[4, 17] = "LFV";
                    xlWorkSheet.Cells[4, 18] = "CON";
                    xlWorkSheet.Cells[4, 19] = "BLG";
                    xlWorkSheet.Cells[4, 20] = "LRO";
                    xlWorkSheet.Cells[4, 21] = "CRO";
                    xlWorkSheet.Cells[4, 22] = "DEF";

                    xlWorkSheet.Cells[4, 23] = "RFV";
                    xlWorkSheet.Cells[4, 24] = "R1H";
                    xlWorkSheet.Cells[4, 25] = "LFV";
                    xlWorkSheet.Cells[4, 26] = "CON";
                    xlWorkSheet.Cells[4, 27] = "BLG";
                    xlWorkSheet.Cells[4, 28] = "LRO";
                    xlWorkSheet.Cells[4, 29] = "CRO";
                    xlWorkSheet.Cells[4, 30] = "DEF";

                    ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                    xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("D4", "AD4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                    xlWorkSheet.get_Range("A3", "AD3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("A3", "AD3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    xlWorkSheet.get_Range("A4", "AD4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        
                    while (dr.Read())
                    {
                        excelReportRecipeWise(dr["workCenterName"].ToString().Trim(), rToDate, rFromDate);
                    }

                    xlWorkSheet.get_Range("A" + (rowCount + 1), "AD" + (rowCount + 1)).Merge(misValue);
                    xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                    xlWorkSheet.Cells[rowCount + 2, 3] = grandtotal;
                    xlWorkSheet.Cells[rowCount + 2, 4] = grandA;
                    xlWorkSheet.Cells[rowCount + 2, 5] = grandB;
                    xlWorkSheet.Cells[rowCount + 2, 6] = grandE;
                    xlWorkSheet.Cells[rowCount + 2, 7] = grandRFV;
                    xlWorkSheet.Cells[rowCount + 2, 8] = grandR1H;

                    xlWorkSheet.Cells[rowCount + 2, 9] = grandLFV;
                    xlWorkSheet.Cells[rowCount + 2, 10] = grandCON;
                    xlWorkSheet.Cells[rowCount + 2, 11] = grandBLG;
                    xlWorkSheet.Cells[rowCount + 2, 12] = grandLRO;
                    xlWorkSheet.Cells[rowCount + 2, 13] = grandCRRO;
                    xlWorkSheet.Cells[rowCount + 2, 14] = grandDEF;
                    xlWorkSheet.Cells[rowCount + 2, 15] = grandDRFV;
                    xlWorkSheet.Cells[rowCount + 2, 16] = grandDR1H;
                    xlWorkSheet.Cells[rowCount + 2, 17] = grandDLFV;
                    xlWorkSheet.Cells[rowCount + 2, 18] = grandDCON;
                    xlWorkSheet.Cells[rowCount + 2, 19] = grandDBLG;
                    xlWorkSheet.Cells[rowCount + 2, 20] = grandDLRO;
                    xlWorkSheet.Cells[rowCount + 2, 21] = grandCRRO;
                    xlWorkSheet.Cells[rowCount + 2, 22] = grandDDEF;
                    xlWorkSheet.Cells[rowCount + 2, 23] = grandERFV;
                    xlWorkSheet.Cells[rowCount + 2, 24] = grandER1H;
                    xlWorkSheet.Cells[rowCount + 2, 25] = grandELFV;
                    xlWorkSheet.Cells[rowCount + 2, 26] = grandECON;
                    xlWorkSheet.Cells[rowCount + 2, 27] = grandEBLG;
                    xlWorkSheet.Cells[rowCount + 2, 28] = grandELRO;
                    xlWorkSheet.Cells[rowCount + 2, 29] = grandECRRO;
                    xlWorkSheet.Cells[rowCount + 2, 30] = grandEDEF;
                    
                    xlWorkSheet.get_Range("A1", "AD" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "AD" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
            gridviewdt.Columns.Add("E", typeof(string));
            gridviewdt.Columns.Add("RFV", typeof(string));
            gridviewdt.Columns.Add("R1H", typeof(string));
            gridviewdt.Columns.Add("LFV", typeof(string));
            gridviewdt.Columns.Add("CON", typeof(string));
            gridviewdt.Columns.Add("BLG", typeof(string));
            gridviewdt.Columns.Add("LRO", typeof(string));
            gridviewdt.Columns.Add("CRRO", typeof(string));
            gridviewdt.Columns.Add("DEF", typeof(string));
            gridviewdt.Columns.Add("DRFV", typeof(string));
            gridviewdt.Columns.Add("DR1H", typeof(string));
            gridviewdt.Columns.Add("DLFV", typeof(string));
            gridviewdt.Columns.Add("DCON", typeof(string));
            gridviewdt.Columns.Add("DBLG", typeof(string));
            gridviewdt.Columns.Add("DLRO", typeof(string));
            gridviewdt.Columns.Add("DCRRO", typeof(string));
            gridviewdt.Columns.Add("DDEF", typeof(string));
            gridviewdt.Columns.Add("ERFV", typeof(string));
            gridviewdt.Columns.Add("ER1H", typeof(string));
            gridviewdt.Columns.Add("ELFV", typeof(string));
            gridviewdt.Columns.Add("ECON", typeof(string));
            gridviewdt.Columns.Add("EBLG", typeof(string));
            gridviewdt.Columns.Add("ELRO", typeof(string));
            gridviewdt.Columns.Add("ECRRO", typeof(string));
            gridviewdt.Columns.Add("EDEF", typeof(string));

            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("GradeRFVCW", typeof(string));
            dt.Columns.Add("GradeRFVCCW", typeof(string));
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
            dt.Columns.Add("GradeUpperRRO", typeof(string));
            dt.Columns.Add("gradelowerdepression", typeof(string));
            dt.Columns.Add("GradeUpperDepression", typeof(string));
            query = "";
            int colCount = 1, mergeCount = 0, typeCount = 0;
            
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);
            int total, A, B, E, RFV, R1H, LFV, CON, BLG, LRO, CRRO, DEF, DRFV, DR1H, DLFV, DCON, DBLG, DLRO, DCRRO, DDEF, ERFV, ER1H, ELFV, ECON, EBLG, ELRO, ECRRO, EDEF;
            total_ = 0; A_ = 0; B_ = 0; E_ = 0; RFV_ = 0; R1H_ = 0; LFV_ = 0; CON_ = 0; BLG_ = 0; LRO_ = 0; CRRO_ = 0; DEF_ = 0; DRFV_ = 0; DR1H_ = 0; DLFV_ = 0; DCON_ = 0; DBLG_ = 0; DLRO_ = 0; DCRRO_ = 0; DDEF_ = 0; ERFV_ = 0; ER1H_ = 0; ELFV_ = 0; ECON_ = 0; EBLG_ = 0; ELRO_ = 0; ECRRO_ = 0; EDEF_ = 0;
            Double pA, pB, pE, pRFV, pR1H, pLFV, pCON, pBLG, pLRO, pCRRO, pDEF, pDRFV, pDR1H, pDLFV, pDCON, pDBLG, pDLRO, pDCRRO, pDDEF, pERFV, pER1H, pELFV, pECON, pEBLG, pELRO, pECRRO, pEDEF;

            if (QualityReportRecipeWise.Checked)
            {
                typeCount = 0;
                // Excel code for Recipe Wise
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
                xlWorkSheet.Cells[1, 2] = "Performance Report with Rejection Details";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("C3", "F3").Merge(misValue);
                xlWorkSheet.get_Range("G3", "N3").Merge(misValue);
                xlWorkSheet.get_Range("O3", "V3").Merge(misValue);
                xlWorkSheet.get_Range("W3", "AC3").Merge(misValue);

                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "AC3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "Uni Grade";
                xlWorkSheet.Cells[3, 6] = "C";
                xlWorkSheet.Cells[3, 14] = "D";
                xlWorkSheet.Cells[3, 22] = "E";


                xlWorkSheet.Cells[4, 3] = "A";
                xlWorkSheet.Cells[4, 4] = "B";
                xlWorkSheet.Cells[4, 5] = "REJ";

                xlWorkSheet.Cells[4, 6] = "RFV";
                xlWorkSheet.Cells[4, 7] = "R1H";
                xlWorkSheet.Cells[4, 8] = "LFV";
                xlWorkSheet.Cells[4, 9] = "CON";
                xlWorkSheet.Cells[4, 10] = "BLG";
                xlWorkSheet.Cells[4, 11] = "LRO";
                xlWorkSheet.Cells[4, 12] = "CRO";
                xlWorkSheet.Cells[4, 13] = "DEF";

                xlWorkSheet.Cells[4, 14] = "RFV";
                xlWorkSheet.Cells[4, 15] = "R1H";
                xlWorkSheet.Cells[4, 16] = "LFV";
                xlWorkSheet.Cells[4, 17] = "CON";
                xlWorkSheet.Cells[4, 18] = "BLG";
                xlWorkSheet.Cells[4, 19] = "LRO";
                xlWorkSheet.Cells[4, 20] = "CRO";
                xlWorkSheet.Cells[4, 21] = "DEF";

                xlWorkSheet.Cells[4, 22] = "RFV";
                xlWorkSheet.Cells[4, 23] = "R1H";
                xlWorkSheet.Cells[4, 24] = "LFV";
                xlWorkSheet.Cells[4, 25] = "CON";
                xlWorkSheet.Cells[4, 26] = "BLG";
                xlWorkSheet.Cells[4, 27] = "LRO";
                xlWorkSheet.Cells[4, 28] = "CRO";
                xlWorkSheet.Cells[4, 29] = "DEF";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A4", "B4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("C4", "AC4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("A3", "AC3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "AC3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "AC4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                // End Excel Code

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, GradeRFVCW, GradeRFVCCW, GradeH1RFVCW, GradeH1RFVCCW, GradeLFVCW, GradeLFVCCW, GradeCONICITY, GradeLowerBulge, GradeUpperBulge, GradeLowerLRO, GradeUpperLRO, GradeRRO, GradeUpperRRO, gradelowerdepression, GradeUpperDepression FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

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

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; RFV = 0; R1H = 0; E = 0;
                pA = 0; pB = 0; pE = 0; pRFV = 0; pR1H = 0; pLFV = 0; pCON = 0; pBLG = 0; pLRO = 0; pCRRO = 0; pDEF = 0; pDRFV = 0; pDR1H = 0; pDLFV = 0; pDCON = 0; pDBLG = 0; pDLRO = 0; pDCRRO = 0; pDDEF = 0; pERFV = 0; pER1H = 0; pELFV = 0; pECON = 0; pEBLG = 0; pELRO = 0; pECRRO = 0; pEDEF = 0;
                rowCount++;

                total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));
                RFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'C' or GradeRFVCCW='C')"));
                R1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'C' or GradeH1RFVCCW='C')"));
                LFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'C' or GradeLFVCCW='C')"));
                CON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  GradeCONICITY = 'C'"));
                BLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'C' or GradeUpperBulge='C')"));
                LRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'C' or GradeUpperLRO='C')"));
                CRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'C' or GradeUpperRRO='C')"));
                DEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (gradelowerdepression= 'C' or GradeUpperDepression='C')"));
                DRFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'D' or GradeRFVCCW='D')"));
                DR1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'D' or GradeH1RFVCCW='D')"));
                DLFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'D' or GradeLFVCCW='D')"));
                DCON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and GradeCONICITY = 'D'"));
                DBLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'D' or GradeUpperBulge='D')"));
                DLRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'D' or GradeUpperLRO='D')"));
                DCRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'D' or GradeUpperRRO='D')"));
                DDEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (gradelowerdepression= 'D' or GradeUpperDepression='D')"));
                ERFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRFVCW = 'E' or GradeRFVCCW='E')"));

                ER1H = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeH1RFVCW = 'E' or GradeH1RFVCCW='E')"));
                ELFV = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLFVCW= 'E' or GradeLFVCCW='E')"));
                ECON = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and GradeCONICITY = 'E'"));
                EBLG = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerBulge = 'E' or GradeUpperBulge='E')"));
                ELRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerLRO= 'E' or GradeUpperLRO='E')"));
                ECRRO = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeRRO= 'E' or GradeUpperRRO='E')"));
                EDEF = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and (GradeLowerdepression= 'E' or GradeUpperDepression='E')"));

                if (option == "1")
                {
                    xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0];
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = E;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = RFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = R1H;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = LFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = CON;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = BLG;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = LRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = CRRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = DEF;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = DRFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = DR1H;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 15] = DLFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 16] = DCON;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 17] = DBLG;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 18] = DLRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 19] = DCRRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 20] = DDEF;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 21] = ERFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 22] = ER1H;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 23] = ELFV;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 24] = ECON;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 25] = EBLG;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 26] = ELRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 27] = ECRRO;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 28] = EDEF;
                }
                else if (option == "2")
                {
                    pA = ((total == 0) ? 0 : ((double)(A * 100) / total));
                    pB = ((total == 0) ? 0 : ((double)(B * 100) / total));
                    pRFV = ((total == 0) ? 0 : ((double)(RFV * 100) / total));
                    pR1H = ((total == 0) ? 0 : ((double)(R1H * 100) / total));
                    pE = ((total == 0) ? 0 : ((double)(E * 100) / total));
                    pLFV = ((total == 0) ? 0 : ((double)(LFV * 100) / total));
                    pCON = ((total == 0) ? 0 : ((double)(CON * 100) / total));
                    pBLG = ((total == 0) ? 0 : ((double)(BLG * 100) / total));
                    pLRO = ((total == 0) ? 0 : ((double)(LRO * 100) / total));
                    pCRRO = ((total == 0) ? 0 : ((double)(CRRO * 100) / total));
                    pDEF = ((total == 0) ? 0 : ((double)(DEF * 100) / total));
                    pDRFV = ((total == 0) ? 0 : ((double)(DRFV * 100) / total));
                    pDR1H = ((total == 0) ? 0 : ((double)(DR1H * 100) / total));
                    pDLFV = ((total == 0) ? 0 : ((double)(DLFV * 100) / total));
                    pDCON = ((total == 0) ? 0 : ((double)(DCON * 100) / total));
                    pDBLG = ((total == 0) ? 0 : ((double)(DBLG * 100) / total));
                    pDLRO = ((total == 0) ? 0 : ((double)(DLRO * 100) / total));
                    pDCRRO = ((total == 0) ? 0 : ((double)(DCRRO * 100) / total));
                    pDDEF = ((total == 0) ? 0 : ((double)(DDEF * 100) / total));
                    pERFV = ((total == 0) ? 0 : ((double)(ERFV * 100) / total));
                    pER1H = ((total == 0) ? 0 : ((double)(ER1H * 100) / total));
                    pELFV = ((total == 0) ? 0 : ((double)(ELFV * 100) / total));
                    pECON = ((total == 0) ? 0 : ((double)(ECON * 100) / total));
                    pEBLG = ((total == 0) ? 0 : ((double)(EBLG * 100) / total));
                    pELRO = ((total == 0) ? 0 : ((double)(ELRO * 100) / total));
                    pECRRO = ((total == 0) ? 0 : ((double)(ECRRO * 100) / total));
                    pEDEF = ((total == 0) ? 0 : ((double)(EDEF * 100) / total));

                    xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pE, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pRFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pR1H, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = Math.Round(pLFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = Math.Round(pCON, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = Math.Round(pBLG, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = Math.Round(pLRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = Math.Round(pCRRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = Math.Round(pDEF, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = Math.Round(pDRFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = Math.Round(pDR1H, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 15] = Math.Round(pDLFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 16] = Math.Round(pDCON, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 17] = Math.Round(pDBLG, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 18] = Math.Round(pDLRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 19] = Math.Round(pDCRRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 20] = Math.Round(pDDEF, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 21] = Math.Round(pERFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 22] = Math.Round(pER1H, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 23] = Math.Round(pELFV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 24] = Math.Round(pECON, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 25] = Math.Round(pEBLG, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 26] = Math.Round(pELRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 27] = Math.Round(pECRRO, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 28] = Math.Round(pEDEF, 1);
                }

                total_ = total_ + total;
                A_ = A_ + A;
                B_ = B_ + B;
                RFV_ = RFV_ + RFV;
                R1H_ = R1H_ + R1H;
                E_ = E_ + E;
                LFV_ = LFV_ + LFV;
                CON_ = CON_ + CON;
                BLG_ = BLG_ + BLG;
                LRO_ = LRO_ + LRO;
                CRRO_ = CRRO_ + CRRO;
                DEF_ = DEF_ + DEF;
                DRFV_ = DRFV_ + DRFV;
                DR1H_ = DR1H_ + DR1H;
                DLFV_ = DLFV_ + DLFV;
                DCON_ = DCON_ + DCON;
                DBLG_ = DBLG_ + DBLG;
                DLRO_ = DLRO_ + DLRO;
                DCRRO_ = DCRRO_ + DCRRO;
                DDEF_ = DDEF_ + DDEF;
                ERFV_ = ERFV_ + ERFV;
                ER1H_ = ER1H_ + ER1H;
                ELFV_ = ELFV_ + ELFV;
                ECON_ = ECON_ + ECON;
                EBLG_ = EBLG_ + EBLG;
                ELRO_ = ELRO_ + ELRO;
                ECRRO_ = ECRRO_ + ECRRO;
                EDEF_ = EDEF_ + EDEF;

                DataRow dr = gridviewdt.NewRow();
                gridviewdt.Rows.Add(dr);
            }
            DataRow ndr = gridviewdt.NewRow();
            xlWorkSheet.Cells[rowCount + 1, colCount + typeCount + 1] = "";
            ndr[1] = "";
            ndr[2] = "";
            ndr[3] = "";
            ndr[4] = "";
            ndr[5] = "";
            ndr[6] = "";

            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            xlWorkSheet.Cells[rowCount + 2, colCount + typeCount] = "Total";
            if (option == "1")
            {
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = A_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = B_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = E_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = RFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = R1H_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 7] = LFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 8] = CON_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 9] = BLG_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 10] = LRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 11] = CRRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 12] = DEF_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 13] = DRFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 14] = DR1H_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 15] = DLFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 16] = DCON_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 17] = DBLG_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 18] = DLRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 19] = DCRRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 20] = DDEF_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 21] = ERFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 22] = ER1H_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 23] = ELFV_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 24] = ECON_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 25] = EBLG_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 26] = ELRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 27] = ECRRO_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 28] = EDEF_;

            }
            else if (option == "2")
            {
                pA = ((total_ == 0) ? 0 : ((double)(A_ * 100) / total_));
                pB = ((total_ == 0) ? 0 : ((double)(B_ * 100) / total_));
                pRFV = ((total_ == 0) ? 0 : ((double)(RFV_ * 100) / total_));
                pR1H = ((total_ == 0) ? 0 : ((double)(R1H_ * 100) / total_));
                pE = ((total_ == 0) ? 0 : ((double)(E_ * 100) / total_));
                pLFV = ((total_ == 0) ? 0 : ((double)(LFV_ * 100) / total_));
                pCON = ((total_ == 0) ? 0 : ((double)(CON_ * 100) / total_));
                pBLG = ((total_ == 0) ? 0 : ((double)(BLG_ * 100) / total_));
                pLRO = ((total_ == 0) ? 0 : ((double)(LRO_ * 100) / total_));
                pCRRO = ((total_ == 0) ? 0 : ((double)(CRRO_ * 100) / total_));
                pDEF = ((total_ == 0) ? 0 : ((double)(DEF_ * 100) / total_));
                pDRFV = ((total_ == 0) ? 0 : ((double)(DRFV_ * 100) / total_));
                pDR1H = ((total_ == 0) ? 0 : ((double)(DR1H_ * 100) / total_));
                pDLFV = ((total_ == 0) ? 0 : ((double)(DLFV_ * 100) / total_));
                pDCON = ((total_ == 0) ? 0 : ((double)(DCON_ * 100) / total_));
                pDBLG = ((total_ == 0) ? 0 : ((double)(DBLG_ * 100) / total_));
                pDLRO = ((total_ == 0) ? 0 : ((double)(DLRO_ * 100) / total_));
                pDCRRO = ((total_ == 0) ? 0 : ((double)(DCRRO_ * 100) / total_));
                pDDEF = ((total_ == 0) ? 0 : ((double)(DDEF_ * 100) / total_));
                pERFV = ((total_ == 0) ? 0 : ((double)(ERFV_ * 100) / total_));
                pER1H = ((total_ == 0) ? 0 : ((double)(ER1H_ * 100) / total_));
                pRFV = ((total_ == 0) ? 0 : ((double)(RFV_ * 100) / total_));
                pELFV = ((total_ == 0) ? 0 : ((double)(ELFV_ * 100) / total_));
                pECON = ((total_ == 0) ? 0 : ((double)(ECON_ * 100) / total_));
                pEBLG = ((total_ == 0) ? 0 : ((double)(EBLG_ * 100) / total_));
                pELRO = ((total_ == 0) ? 0 : ((double)(ELRO_ * 100) / total_));
                pECRRO = ((total_ == 0) ? 0 : ((double)(ECRRO_ * 100) / total_));
                pEDEF = ((total_ == 0) ? 0 : ((double)(EDEF_ * 100) / total_));
                
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_.ToString();
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = Math.Round(pA, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = Math.Round(pB, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = Math.Round(pE, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = Math.Round(pRFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = Math.Round(pR1H, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 7] = Math.Round(pLFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 8] = Math.Round(pCON, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 9] = Math.Round(pBLG, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 10] = Math.Round(pLRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 11] = Math.Round(pCRRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 12] = Math.Round(pDEF, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 13] = Math.Round(pDRFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 14] = Math.Round(pDR1H, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 15] = Math.Round(pDLFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 16] = Math.Round(pDCON, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 17] = Math.Round(pDBLG, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 18] = Math.Round(pDLRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 19] = Math.Round(pDCRRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 20] = Math.Round(pDDEF, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 21] = Math.Round(pERFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 22] = Math.Round(pER1H, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 23] = Math.Round(pELFV, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 24] = Math.Round(pECON, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 25] = Math.Round(pEBLG, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 26] = Math.Round(pELRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 27] = Math.Round(pECRRO, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 28] = Math.Round(pEDEF, 1);
            }

            gridviewdt.Rows.Add(tdr);


            grandtotal += total_;
            grandA += A_;
            grandB += B_;
            grandE += E_;
            grandRFV += RFV_;
            grandR1H += R1H_;
            grandLFV += LFV_;
            grandCON += CON_;
            grandBLG += BLG_;
            grandLRO += LRO_;
            grandCRRO += CRRO_;
            grandDEF += DEF_;
            grandDRFV += DRFV_;
            grandDR1H += DR1H_;
            grandDLFV += DLFV_;
            grandDCON += DCON_;
            grandDBLG += DBLG_;
            grandDLRO += DLRO_;
            grandDCRRO += DCRRO_;
            grandDDEF += DDEF_;
            grandERFV += ERFV_;
            grandER1H += ER1H_;
            grandELFV += ELFV_;
            grandECON += ECON_;
            grandEBLG += EBLG_;
            grandELRO += ELRO_;
            grandECRRO += ECRRO_;
            grandEDEF += EDEF_;

            rowCount += 2;

            if (QualityReportTBMWise.Checked)
            {                
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "AC" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "AC" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "AC" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
