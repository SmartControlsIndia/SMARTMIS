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
    public partial class TUOReport4 : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        int total_, A_, B_, specPlus10_, SpecPlus20_, SpecPlus30_, SpecGreaterThan30_;

        Double pA_, pB_, pspecPlus10_, pSpecPlus20_, pSpecPlus30_, pSpecGreaterThan30_;
        public Double grandtotal, grandA, grandB, grandspecPlus10, grandSpecPlus20, grandSpecPlus30, grandSpecGreaterThan30;
        string tablename, parameter;
        int value;
        DataRow tdr;
        
        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", query = "";
        string[] tempString2;
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
        
        #endregion

        public TUOReport4()
        {
            filepath = myWebService.getExcelPath();
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });

            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";

            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            /*if (tempString2[tempString2.Length - 1].ToString() == "0")
                tablename = "vCuringWiseproductionDataTUO";
            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                tablename = "productionDataTUO";
            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                tablename = "vproductionDataTUO";
            */
            parameter = tuoFilterSpecParameterDropDownList.SelectedValue;
            if (SpecTextBox.Text.Length > 0)
                value = Convert.ToInt32(SpecTextBox.Text);
            else
            {
                value = 0;
                SpecTextBox.Text = "0";
            }
                    
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
                    PerformaceReportSpecMachinewisePanel.Visible = true;
                    performanceReportSpecRcipeWisePanel.Visible = false;
                    showReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    PerformaceReportSpecMachinewisePanel.Visible = false;
                    performanceReportSpecRcipeWisePanel.Visible = true;
                    showReportRecipeWise(performanceReportTBMWiseSpecMainGridView, "", rToDate, rFromDate);
                }

                switch (option)
                {
                    case "2":
                    grandA = Math.Round(((float)(grandA * 100) / grandtotal), 1);
                    grandB = Math.Round(((float)(grandB * 100) / grandtotal), 1);
                    grandspecPlus10 = Math.Round(((float)(grandspecPlus10 * 100) / grandtotal), 1);
                    grandSpecPlus20 = Math.Round(((float)(grandSpecPlus30 * 100) / grandtotal), 1);
                    grandSpecPlus30 = Math.Round(((float)(grandSpecPlus30 * 100) / grandtotal), 1);
                    grandSpecGreaterThan30 = Math.Round(((float)(grandSpecGreaterThan30 * 100) / grandtotal), 1);
                    break;
                }
            }
            else
            {
                performanceReportTBMWiseSpecMainGridView.DataSource = null;
                performanceReportTBMWiseSpecMainGridView.DataBind();

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
            PerformaceReportSpecMachinewisePanel.Visible = false;
            performanceReportSpecRcipeWisePanel.Visible = false;
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //wcnamequery = wcquery(query);
            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceReportTBMWiseSpecMainGridView")
                {

                    Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportTBMWiseSpecWCNameLabel"));
                    workcentername = wcnameLabel.Text.ToString();
                    GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTBMWiseSpecChildGridView"));
                    showReportRecipeWise(childGridView, workcentername, rToDate, rFromDate);


                }

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
        protected string getqueryWCWise(string tableName, string wcNameString, string dtnadtime)
        {
            if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade,RFV,LFV FROM  " + tablename + " WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade,RFV,LFV FROM  " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade,RFV,LFV FROM  " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
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
            gridviewdt.Columns.Add("specPlus10", typeof(string));
            gridviewdt.Columns.Add("SpecPlus20", typeof(string));
            gridviewdt.Columns.Add("SpecPlus30", typeof(string));
            gridviewdt.Columns.Add("Specgreaterthan30", typeof(string));
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("RFV", typeof(float));
            dt.Columns.Add("LFV", typeof(float));
            int total, A, B, specPlus10, SpecPlus20, SpecPlus30, SpecGreaterThan30;
            Double pA, pB, pspecPlus10, pSpecPlus20, pSpecPlus30, pSpecGreaterThan30;


            query = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportRecipeWise.Checked)
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

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

            total_ = 0; A_ = 0; B_ = 0; specPlus10_ = 0; SpecPlus20_ = 0; SpecPlus30_ = 0; SpecGreaterThan30_ = 0;
            
            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; specPlus10 = 0; SpecPlus20 = 0; SpecPlus30 = 0;
                pA = 0; pB = 0; pspecPlus10 = 0; pSpecPlus20 = 0; pSpecPlus30 = 0;
                total = (int)dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                specPlus10 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + "<" + value));
                SpecPlus20 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value + 2 + " and " + parameter + "<" + value + 4));
                SpecPlus30 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value + 4 + " and " + parameter + "<" + value + 6));
                SpecGreaterThan30 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value+6));
                
                total_ += total;
                A_ += A;
                B_ += B;
                specPlus10_ += specPlus10;
                SpecPlus20_ += SpecPlus20;
                SpecPlus30_ += SpecPlus30;
                SpecGreaterThan30_ += SpecGreaterThan30;

                DataRow dr = gridviewdt.NewRow();
                switch (option)
                {
                    case "1":
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                    dr[2] = A.ToString();
                    dr[3] = B.ToString();
                    dr[4] = specPlus10.ToString();
                    dr[5] = SpecPlus20.ToString();
                    dr[6] = SpecPlus30.ToString();
                    dr[7] = SpecGreaterThan30.ToString();
                        break;
                    case "2":
                    pA = ((double)(A * 100) / total);
                    pB = ((double)(B * 100) / total);
                    pspecPlus10 = ((double)(specPlus10 * 100) / total);
                    pSpecPlus20 = ((double)(SpecPlus20 * 100) / total);
                    pSpecPlus30 = ((double)(SpecPlus30 * 100) / total);
                    pSpecGreaterThan30 = ((double)(SpecGreaterThan30 * 100) / total);

                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                    dr[2] = Math.Round(pA, 1);
                    dr[3] = Math.Round(pB, 1);

                    dr[4] = Math.Round(pspecPlus10, 1);
                    dr[5] = Math.Round(pSpecPlus20, 1);
                    dr[6] = Math.Round(pSpecPlus30, 1);
                    dr[7] = Math.Round(pSpecGreaterThan30, 1);
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
            ndr[7] = "";

            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            switch (option)
            {
                case "1":
                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = A_;
                tdr[3] = B_;
                tdr[4] = specPlus10_;
                tdr[5] = SpecPlus20_;
                tdr[6] = SpecPlus30_;
                tdr[7] = SpecGreaterThan30_;
                break;
                case "2":
                pA_ = ((double)(A_ * 100) / total_);
                pB_ = ((double)(B_ * 100) / total_);
                pspecPlus10_ = ((double)(specPlus10_ * 100) / total_);
                pSpecPlus20_ = ((double)(SpecPlus20_ * 100) / total_);
                pSpecPlus30_ = ((double)(SpecPlus30_ * 100) / total_);
                pSpecGreaterThan30_ = ((double)(SpecGreaterThan30_ * 100) / total_);

                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = Math.Round(pA_, 1);
                tdr[3] = Math.Round(pB_, 1);
                tdr[4] = Math.Round(pspecPlus10_, 1);
                tdr[5] = Math.Round(pSpecPlus20_, 1);
                tdr[6] = Math.Round(pSpecPlus30_, 1);
                tdr[7] = Math.Round(pSpecGreaterThan30_, 1);
                break;
            }

            gridviewdt.Rows.Add(tdr);
            
            grandtotal += total_;
            grandA += A_;
            grandB += B_;
            grandspecPlus10 += specPlus10_;
            grandSpecPlus20 += SpecPlus20_;
            grandSpecPlus30 += SpecPlus30_;
            grandSpecGreaterThan30 += SpecGreaterThan30_;
            
            if (QualityReportTBMWise.Checked)
            {
                childgridview.DataSource = gridviewdt;
                childgridview.DataBind();
            }
            else if (QualityReportRecipeWise.Checked)
            {
                performanceReportRecipeWiseSpecGridView.DataSource = gridviewdt;
                performanceReportRecipeWiseSpecGridView.DataBind();

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

                performanceReportTBMWiseSpecMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportTBMWiseSpecMainGridView.DataBind();
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
            performanceReportSpecRcipeWisePanel.Visible = true;
            PerformaceReportSpecMachinewisePanel.Visible = false;
            performanceReportRecipeWiseSpecGridView.DataSource = null;
            performanceReportRecipeWiseSpecGridView.DataBind();
        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {

            performanceReportSpecRcipeWisePanel.Visible = false;
            PerformaceReportSpecMachinewisePanel.Visible = true;
            performanceReportTBMWiseSpecMainGridView.Visible = false;
            performanceReportTBMWiseSpecMainGridView.DataSource = null;
            performanceReportTBMWiseSpecMainGridView.DataBind();
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
           Response.AddHeader("content-disposition", "attachment;filename=PerformanceReportSpecWise" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
           Response.Charset = "";
           Response.ContentType = "application/vnd.xls";
           System.IO.StringWriter stringWrite = new System.IO.StringWriter();
           System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
           //stringWrite.Write("<table><tr><td><b>TBM Production Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + type + "</td><td><b>" + reportMasterWCProcessDropDownList.SelectedItem.Value + "</b></td></tr></table>");
           System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
           Controls.Add(form);
           
           if (QualityReportTBMWise.Checked)
           {
               PerformaceReportSpecMachinewisePanel.Visible = true;
               performanceReportSpecRcipeWisePanel.Visible = false;
               form.Controls.Add(PerformaceReportSpecMachinewisePanel);
           
           }
           else if (QualityReportRecipeWise.Checked)
           {
               PerformaceReportSpecMachinewisePanel.Visible = false;
               performanceReportSpecRcipeWisePanel.Visible = true;
               form.Controls.Add(performanceReportSpecRcipeWisePanel);
           
           } 
           
           form.RenderControl(htmlWrite);

           //gv.RenderControl(htmlWrite);
           Response.Write(stringWrite.ToString());
           Response.End(); 
           
           /*queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });

            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";

            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            

            parameter = tuoFilterSpecParameterDropDownList.SelectedValue;
            if (SpecTextBox.Text.Length > 0)
                value = Convert.ToInt32(SpecTextBox.Text);
            else
            {
                value = 0;
                SpecTextBox.Text = "0";
            }

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
                    PerformaceReportSpecMachinewisePanel.Visible = true;
                    performanceReportSpecRcipeWisePanel.Visible = false;
                    excelReport(query);

                }
                else if (QualityReportRecipeWise.Checked)
                {
                    PerformaceReportSpecMachinewisePanel.Visible = false;
                    performanceReportSpecRcipeWisePanel.Visible = true;
                    excelReportRecipeWise("default", rToDate, rFromDate);
                }

                switch (option)
                {
                    case "2":
                    grandA = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandA * 100) / grandtotal)), 1);
                    grandB = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandB * 100) / grandtotal)), 1);
                    grandspecPlus10 = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandspecPlus10 * 100) / grandtotal)), 1);
                    grandSpecPlus20 = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandSpecPlus30 * 100) / grandtotal)), 1);
                    grandSpecPlus30 = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandSpecPlus30 * 100) / grandtotal)), 1);
                    grandSpecGreaterThan30 = Math.Round(((grandtotal == 0) ? 0 : ((float)(grandSpecGreaterThan30 * 100) / grandtotal)), 1);
                    break;
                }
            }
            else
            {
                performanceReportTBMWiseSpecMainGridView.DataSource = null;
                performanceReportTBMWiseSpecMainGridView.DataBind();

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
                    xlWorkSheet.Cells[1, 2] = "performance report Spec Wise";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.get_Range("D3", "E3").Merge(misValue);
                    xlWorkSheet.get_Range("F3", "I3").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "I3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                    xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;
                        
                    xlWorkSheet.Cells[3, 1] = "Machine Name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "OE";
                    xlWorkSheet.Cells[3, 7] = "";

                        xlWorkSheet.Cells[4, 4] = "A";
                        xlWorkSheet.Cells[4, 5] = "B";
                        xlWorkSheet.Cells[4, 6] = "Spec>&<Spec+2";
                        xlWorkSheet.Cells[4, 7] = "Spec+2>&<Spec+4";
                        xlWorkSheet.Cells[4, 8] = "Spec+4>&<Spec+6";
                        xlWorkSheet.Cells[4, 9] = "Spec+6>";
                        
                        ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                        xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                        xlWorkSheet.get_Range("D4", "I4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                        xlWorkSheet.get_Range("A3", "I3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                        xlWorkSheet.get_Range("A3", "I3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        xlWorkSheet.get_Range("A4", "I4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        
                        while (dr.Read())
                        {
                            excelReportRecipeWise(dr["workCenterName"].ToString().Trim(), rToDate, rFromDate);
                        }

                        xlWorkSheet.get_Range("A" + (rowCount + 1), "I" + (rowCount + 1)).Merge(misValue);
                        xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                        xlWorkSheet.Cells[rowCount + 2, 3] = grandtotal;
                        
                        switch(option)
                        {
                            case "1" :
                                xlWorkSheet.Cells[rowCount + 2, 4] = grandA;
                                xlWorkSheet.Cells[rowCount + 2, 5] = grandB;
                                xlWorkSheet.Cells[rowCount + 2, 6] = grandspecPlus10;
                                xlWorkSheet.Cells[rowCount + 2, 7] = grandSpecPlus20;
                                xlWorkSheet.Cells[rowCount + 2, 8] = grandSpecPlus30;
                                xlWorkSheet.Cells[rowCount + 2, 9] = grandSpecGreaterThan30;
                            break;

                            case "2":
                                xlWorkSheet.Cells[rowCount + 2, 4] = ((grandtotal == 0) ? 0 : ((double)(grandA * 100) / grandtotal));
                                xlWorkSheet.Cells[rowCount + 2, 5] = ((grandtotal == 0) ? 0 : ((double)(grandB * 100) / grandtotal));
                                xlWorkSheet.Cells[rowCount + 2, 6] = ((grandtotal == 0) ? 0 : ((double)(grandspecPlus10 * 100) / grandtotal));
                                xlWorkSheet.Cells[rowCount + 2, 7] = ((grandtotal == 0) ? 0 : ((double)(grandSpecPlus20 * 100) / grandtotal));
                                xlWorkSheet.Cells[rowCount + 2, 8] = ((grandtotal == 0) ? 0 : ((double)(grandSpecPlus30 * 100) / grandtotal));
                                xlWorkSheet.Cells[rowCount + 2, 9] = ((grandtotal == 0) ? 0 : ((double)(grandSpecGreaterThan30 * 100) / grandtotal));
                            break;
                        }
                    xlWorkSheet.get_Range("A1", "I" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "I" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
            gridviewdt.Columns.Add("specPlus10", typeof(string));
            gridviewdt.Columns.Add("SpecPlus20", typeof(string));
            gridviewdt.Columns.Add("SpecPlus30", typeof(string));
            gridviewdt.Columns.Add("Specgreaterthan30", typeof(string));
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("RFV", typeof(float));
            dt.Columns.Add("LFV", typeof(float));
            int total, A, B, specPlus10, SpecPlus20, SpecPlus30, SpecGreaterThan30;
            Double pA, pB, pspecPlus10, pSpecPlus20, pSpecPlus30, pSpecGreaterThan30;


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

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                xlWorkSheet.Cells[1, 2] = "Average and standard Performance Report";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("D3", "E3").Merge(misValue);
                xlWorkSheet.get_Range("F3", "H3").Merge(misValue);

                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "H3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + rToDate;
                xlWorkSheet.Cells[2, 2] = "To : " + rFromDate;
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "OE";
                xlWorkSheet.Cells[3, 6] = "";

                xlWorkSheet.Cells[4, 3] = "A";
                xlWorkSheet.Cells[4, 4] = "B";
                xlWorkSheet.Cells[4, 5] = "Spec>&<Spec+2";
                xlWorkSheet.Cells[4, 6] = "Spec+2>&<Spec+4";
                xlWorkSheet.Cells[4, 7] = "Spec+4>&<Spec+6";
                xlWorkSheet.Cells[4, 8] = "Spec+6>";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("D4", "H4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("A3", "H3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "H3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "H4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                 
                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM  " + tablename + "  WHERE ((testTime>" + dtnadtime;
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade,RFV,LFV FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;

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

            total_ = 0; A_ = 0; B_ = 0; specPlus10_ = 0; SpecPlus20_ = 0; SpecPlus30_ = 0; SpecGreaterThan30_ = 0;
            
            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; specPlus10 = 0; SpecPlus20 = 0; SpecPlus30 = 0; pSpecGreaterThan30 = 0;
                pA = 0; pB = 0; pspecPlus10 = 0; pSpecPlus20 = 0; pSpecPlus30 = 0; rowCount++;
                total = (int)dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                specPlus10 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + "<" + value));
                SpecPlus20 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value + 2 + " and " + parameter + "<" + value + 4));
                SpecPlus30 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value + 4 + " and " + parameter + "<" + value + 6));
                SpecGreaterThan30 = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + parameter + ">" + value+6));
                                
                DataRow dr = gridviewdt.NewRow();
                switch(option)
                {
                    case "1":
                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0];
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = specPlus10.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = SpecPlus20.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = SpecPlus30.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = SpecGreaterThan30.ToString();
                    break;
                    case "2":
                        pA = ((total == 0) ? 0 : ((double)(A * 100) / total));
                        pB = ((total == 0) ? 0 : ((double)(B * 100) / total));
                        pspecPlus10 = ((total == 0) ? 0 : ((double)(specPlus10 * 100) / total));
                        pSpecPlus20 = ((total == 0) ? 0 : ((double)(SpecPlus20 * 100) / total));
                        pSpecPlus30 = ((total == 0) ? 0 : ((double)(SpecPlus30 * 100) / total));
                        pSpecGreaterThan30 = ((total == 0) ? 0 : ((double)(SpecGreaterThan30 * 100) / total));

                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB, 1);

                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pspecPlus10, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pSpecPlus20, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pSpecPlus30, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = Math.Round(pSpecGreaterThan30, 1);
                    break;
                }
                total_ += total;
                A_ += A;
                B_ += B;
                specPlus10_ += specPlus10;
                SpecPlus20_ += SpecPlus20;
                SpecPlus30_ += SpecPlus30;
                SpecGreaterThan30_ += SpecGreaterThan30;

                gridviewdt.Rows.Add(dr);
                                
            }
            DataRow ndr = gridviewdt.NewRow();
            
            xlWorkSheet.Cells[rowCount + 2, colCount + typeCount] = "Total";

            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            rowCount += 2;
            switch(option)
            {
                case "1":
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 0] = "Total";
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = specPlus10_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = SpecPlus20_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = SpecPlus30_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = SpecGreaterThan30_;
                break;
                case "2":
                    pA_ = ((total_ == 0) ? 0 : ((double)(A_ * 100) / total_));
                    pB_ = ((total_ == 0) ? 0 : ((double)(B_ * 100) / total_));
                    pspecPlus10_ = ((total_ == 0) ? 0 : ((double)(specPlus10_ * 100) / total_));
                    pSpecPlus20_ = ((total_ == 0) ? 0 : ((double)(SpecPlus20_ * 100) / total_));
                    pSpecPlus30_ = ((total_ == 0) ? 0 : ((double)(SpecPlus30_ * 100) / total_));
                    pSpecGreaterThan30_ = ((total_ == 0) ? 0 : ((double)(SpecGreaterThan30_ * 100) / total_));
                                    
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 0] = "Total";
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA_, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB_, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pspecPlus10_, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pSpecPlus20_, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pSpecPlus30_, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = Math.Round(pSpecGreaterThan30_, 1);
                    break;
            }

            gridviewdt.Rows.Add(tdr);
            grandtotal += total_;
            grandA += A_;
            grandB += B_;
            grandspecPlus10 += specPlus10_;
            grandSpecPlus20 += SpecPlus20_;
            grandSpecPlus30 += SpecPlus30_;
            grandSpecGreaterThan30 += SpecGreaterThan30_;

            if (QualityReportTBMWise.Checked)
            {                
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "H" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "H" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "H" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
