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
    public partial class TUOReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        int total_, A_, B_, C_, D_, E_;

        Double pA_, pB_, pC_, pD_, pE_;
        public Double grandtotal, grandA, grandB, grandC, grandD, grandE;
        string tablename;

        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", machineStatus = "", wCenterName = "default";
        string query = "";
        string[] tempString2;
        int rowCount = 4, pid = -1;
                
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "performanceReportTUOmachineWise.xlsx";
        string filepath; 

        #endregion       

        public TUOReport()
        {
            filepath = myWebService.getExcelPath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                showDownload.Text = "";
                if (!IsPostBack)
                {
                    if (Session["userID"].ToString().Trim() == "")
                    {
                        Response.Redirect("/SmartMIS/Default.aspx", true);
                    }
                    else
                    {

                        fillSizedropdownlist();
                        fillDesigndropdownlist();
            
                        reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        string showToDate = "";
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

                        reportMasterToDateTextBox.Text = showToDate.ToString();

                        wcnamequery = wcquery();
                        //reportHeader._rDate = reportMasterFromDateTextBox.Text;

                        if (QualityReportTUOWise.Checked)
                        {                            
                            QualityReportTUOWisePanel.Visible = true;
                            QualityReportRecipeTUOWisePanel.Visible = false;
                        }
                        else
                        {
                            QualityReportTUOWisePanel.Visible = false;
                            QualityReportRecipeTUOWisePanel.Visible = true;


                        }

                        //Compare the hidden field if it contains the query string or not
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected string checkDigit(int digit)
        {
            string str = "";
            if (digit.ToString().Length == 1)
                str = "0" + digit;
            else
                str = digit.ToString();
            return str;
        }
        protected void Button_Click(object sender, EventArgs e)
        {
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());
            try
            {

                if (((Button)sender).ID == "ErankViewDetailButton")
                {
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string wcname = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWisewcNameTypeLabel")).Text);
                    string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(wcname, recipeCode);

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);
                }
                else
                {

                }
                if (((Button)sender).ID == "ErankRecipeWiseViewDetailButton")
                {
                    GridViewRow gridviewrow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string recipeCode = (((Label)gridviewrow.Cells[1].FindControl("performanceReportRecipeTUOWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(recipeCode);

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }

                if (((Button)sender).ID == "AllErankDetailButton")
                {
                    fillBarCodeDetailGridView();

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }


                if (((Button)sender).ID == "AllRecipeErankDetailButton")
                {
                    fillBarCodeDetailGridView();

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }
                else
                {

                }

                wcnamequery = wcquery();
                //reportHeader._rDate = reportMasterFromDateTextBox.Text;

                if (optionDropDownList.SelectedItem.Text == "No")
                    option = "1";
                else
                    option = "2";

                rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
                rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

               // reportHeader._rDate = reportMasterFromDateTextBox.Text;

                string dtnadtime = TotalformatDate(rToDate);

                dtnadtime1 = TotalprodataformatDate(rToDate);

                if (QualityReportTUOWise.Checked)
                {
                    fillGridView("Select DISTINCT  machinename from productiondataTUO order by machinename asc");

                    QualityReportTUOWisePanel.Visible = true;
                    QualityReportRecipeTUOWisePanel.Visible = false;
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    QualityReportTUOWisePanel.Visible = false;
                    QualityReportRecipeTUOWisePanel.Visible = true;
                    showReportRecipeWise(performanceReportRecipeTUOWiseGridView, "", rToDate, rFromDate);
                }
                Label1.Text = "false";
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            //Label1.Text = "true";
            wcnamequery = wcquery();
           // reportHeader._rDate = reportMasterFromDateTextBox.Text;

            if (optionDropDownList.SelectedItem.Text == "No")
                option = "1";
            else
                option = "2";

            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

          //  reportHeader._rDate = reportMasterFromDateTextBox.Text;
            
            string dtnadtime = TotalformatDate(rToDate);

            dtnadtime1 = TotalprodataformatDate(rToDate);

            if (QualityReportTUOWise.Checked)
            {
                fillGridView("Select DISTINCT  machinename from productiondataTUO order by machinename asc");

                QualityReportTUOWisePanel.Visible = true;
                QualityReportRecipeTUOWisePanel.Visible = false;
            }
            if (QualityReportRecipeTUOWise.Checked)
            {                
                QualityReportTUOWisePanel.Visible = false;
                QualityReportRecipeTUOWisePanel.Visible = true;
                showReportRecipeWise(performanceReportRecipeTUOWiseGridView, "", rToDate, rFromDate);
            }
            Label1.Text = "false";

        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (optionDropDownList.SelectedItem.Text == "No")
                option = "1";
            else
                option = "2";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceReportTUOWiseMainGridView")
                {

                    Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportTUOWiseWCNameLabel"));
                    workcentername = wcnameLabel.Text.ToString();
                    GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTUOWiseChildGridView"));
                    showReportRecipeWise(childGridView, workcentername, rToDate, rFromDate);                    
                }

            }

        }
        private void fillBarCodeDetailGridView(string wcname, string recipecode)
        {
            if (recipecode != "Total")
                recipecode = " and tireType='" + recipecode + "'";
            else
                recipecode = "";
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,tireType,barCode from vproductiondataTUO where machinename='" + wcname + "' " + recipecode + " and uniformitygrade='E'  and testtime>='" + rToDate + " 07:00:00' and testtime<='" + rFromDate + " 07:00:00'";

            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillBarCodeDetailGridView(string recipecode)
        {
            if (recipecode != "Total")
                recipecode = " and tireType='" + recipecode + "'";
            else
                recipecode = "";
            
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,TireType,barCode from vproductiondataTUO where uniformitygrade='E' " + recipecode + " and testtime>='" + rToDate + " 07:00:00' and testtime<='" + rFromDate + " 07:00:00'";

            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillBarCodeDetailGridView()
        {
            string query = "select wcname,machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E'  and testtime>='" + formattoDate(rToDate) + "' and testtime<='" + formatfromDate(rFromDate) + "'";
            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 13 June 2013
            //Date Updated  : 13 June 2013
            //Revision No.  : 01
            try
            {

                performanceReportTUOWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportTUOWiseMainGridView.DataBind();
            }
            catch (Exception exp)
            {

            }
        }

        private void fillRecipeWiseGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 12 July 2013
            //Date Updated  : 12 July 2013
            //Revision No.  : 01
            try
            {

                performanceReportRecipeTUOWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportRecipeTUOWiseGridView.DataBind();
            }
            catch (Exception exp)
            {

            }
        }
        public string fillCuringWCName(Object barcode)
        {
            string flag = "None";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select wcname from vcuringpcr where gtbarcode = '" + barcode.ToString() + "'";


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = myConnection.reader[0].ToString();
                    else
                        flag = "NOne";
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
        private void fillChildGridView(GridView childgridview, string query)
        {
            DataTable dt = new DataTable();
            dt = myWebService.fillGridView(query, ConnectionOption.SQL);
            if (dt.Rows.Count == 0)
                machineStatus = "<center><B>Machine is not connected with MIS</B></center>";

            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        public string formattoDate(String date)
        {
            string flag = "";
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
            return flag;
        }

        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });
            try
            {
                day = tempDate[1].ToString().Trim();
                month = tempDate[0].ToString().Trim();
                year = tempDate[2].ToString().Trim();
                // DateTime tempDate1 = Convert.ToDateTime(date);
                if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
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
            return flag;
        }
        protected void QualityReportRecipeTUOWise_CheckedChanged(object sender, EventArgs e)
        {            
            fillSizedropdownlist();
            fillDesigndropdownlist();
            QualityReportTUOWisePanel.Visible = false;
            QualityReportRecipeTUOWisePanel.Visible = true;
            showDownload.Text = "";
            performanceReportRecipeTUOWiseGridView.DataSource = null;
            performanceReportRecipeTUOWiseGridView.DataBind();
        }

        protected void QualityReportTUOWise_CheckedChanged(object sender, EventArgs e)
        {            
            performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

            performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
            QualityReportTUOWisePanel.Visible = true;
            QualityReportRecipeTUOWisePanel.Visible = false;
            performanceReportTUOWiseMainGridView.DataSource = null;
            performanceReportTUOWiseMainGridView.DataBind();
        }

        public string TotalformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });
            try
            {
                day = tempDate[1].ToString().Trim();
                month = tempDate[0].ToString().Trim();
                year = tempDate[2].ToString().Trim();

                flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                {
                    flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }
                else
                {
                    flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }

                flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' " + ")OR" + " " + "(testTime>'" + flag3 + "'and" + " " + "testTime<" + "'" + flag4 + "'))";

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }

        public string TotalprodataformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });
            try
            {
                day = tempDate[1].ToString().Trim();
                month = tempDate[0].ToString().Trim();
                year = tempDate[2].ToString().Trim();

                flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                {
                    flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }
                else
                {
                    flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }

                flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' " + ")OR" + " " + "(testTime>'" + flag3 + "'and" + " " + "testTime<" + "'" + flag4 + "'))";

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }

        public string wcquery(string query)
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
                        flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
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


        public string createQuery(String wcID, String fromDate, String toDate, String fromDateColoum, String toDateColoum)
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

        public string wcquery()
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select distinct machinename from productiondatatuo";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        flag = flag + "or" + " " + "machinename = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        flag = "machinename = '" + myConnection.reader[0] + "'";

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

        private void fillSizedropdownlist()
        {

            performanceReportTUOWiseSizeDropdownlist.DataSource = null;
            performanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            performanceReportTUOWiseSizeDropdownlist.DataBind();
        }

        private void fillDesigndropdownlist()
        {

            performanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            performanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            performanceReportTUOWiseRecipeDropdownlist.DataBind();
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
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));


            }
            else if (ddl.ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

            }


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

        public ArrayList FillDropDownList(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
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
        protected string getqueryWCWise(string wcNameString, string dtnadtime)
        {
            if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

            }
            return query;
        }
        private void showReportRecipeWise(GridView childgridview, string wcName, string rtodate, string rfromdate)
        {
            string query;
           DataTable dt = new DataTable();
           DataTable gridviewdt = new DataTable();
           gridviewdt.Columns.Add("wcName", typeof(string));
           gridviewdt.Columns.Add("tireType", typeof(string));
           gridviewdt.Columns.Add("Checked", typeof(string));
           gridviewdt.Columns.Add("A", typeof(string));
           gridviewdt.Columns.Add("B", typeof(string));
           gridviewdt.Columns.Add("C", typeof(string));
           gridviewdt.Columns.Add("D", typeof(string));
           gridviewdt.Columns.Add("E", typeof(string));
           dt.Columns.Add("tireType", typeof(string));
           dt.Columns.Add("uniformitygrade", typeof(string));
           int total, A, B, C, D, E;
           Double pA, pB, pC, pD, pE;


           query = "";
           string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

           if (QualityReportTUOWise.Checked)
           {
               query = getqueryWCWise("machineName='" + wcName + "'", dtnadtime);
           }
           else if (QualityReportRecipeTUOWise.Checked)
           {
               if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
               {
                   query = "Select  tireType ,uniformityGrade FROM  ProductionDataTUO  WHERE ((testTime>" + dtnadtime;
               }
               else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
               {

                   query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

               }
               else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
               {
                   query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

               }
               else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
               {
                   query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

               }
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

           total_ = 0; A_ = 0; B_ = 0; C_ = 0; D_ = 0; E_ = 0;

           for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
           {
               total = 0; A = 0; B = 0; C = 0; D = 0; E = 0;
               pA = 0; pB = 0; pC = 0; pD = 0; pE = 0;

               total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
               A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
               B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
               C = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
               D = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
               E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));

               total_ += total;
               A_ += A;
               B_ += B;
               C_ += C;
               D_ += D;
               E_ += E;

               DataRow dr = gridviewdt.NewRow();
               dr[0] = wcName.ToString();
               switch(option)
               {
                   case "1":
                   dr[1] = uniqrecipedt.Rows[i][0].ToString();
                   dr[2] = total.ToString();
                   dr[3] = A.ToString();
                   dr[4] = B.ToString();
                   dr[5] = C.ToString();
                   dr[6] = D.ToString();
                   dr[7] = E.ToString();
                       break;
                   case "2":
                   pA = ((double)(A * 100) / total);
                   pB = ((double)(B * 100) / total);
                   pC = ((double)(C * 100) / total);
                   pD = ((double)(D * 100) / total);
                   pE = ((double)(E * 100) / total);

                   dr[1] = uniqrecipedt.Rows[i][0].ToString();
                   dr[2] = total.ToString();
                   dr[3] = Math.Round(pA, 1);
                   dr[4] = Math.Round(pB, 1);

                   dr[5] = Math.Round(pC, 1);
                   dr[6] = Math.Round(pD, 1);
                   dr[7] = Math.Round(pE, 1);
                   break;
               }

               gridviewdt.Rows.Add(dr);



           }
           DataRow tdr = gridviewdt.NewRow();
           tdr[0] = wcName.ToString();
           switch (option)
           {
               case "1":
               tdr[1] = "Total";
               tdr[2] = total_;
               tdr[3] = A_;
               tdr[4] = B_;
               tdr[5] = C_;
               tdr[6] = D_;
               tdr[7] = E_;
                   break;
               case "2":
               pA_ = ((double)(A_ * 100) / total_);
               pB_ = ((double)(B_ * 100) / total_);
               pC_ = ((double)(C_ * 100) / total_);
               pD_ = ((double)(D_ * 100) / total_);
               pE_ = ((double)(E_ * 100) / total_);

               tdr[1] = "Total";
               tdr[2] = total_;
               tdr[3] = Math.Round(pA_, 1);
               tdr[4] = Math.Round(pB_, 1);
               tdr[5] = Math.Round(pC_, 1);
               tdr[6] = Math.Round(pD_, 1);
               tdr[7] = Math.Round(pE_, 1);
               break;
           }

           gridviewdt.Rows.Add(tdr);


           grandtotal += total_;
           grandA += A_;
           grandB += B_;
           grandC += C_;
           grandD += D_;
           grandE += E_;




           if (QualityReportTUOWise.Checked)
           {
               childgridview.DataSource = gridviewdt;
               childgridview.DataBind();
           }
           else if (QualityReportRecipeTUOWise.Checked)
           {
               performanceReportRecipeTUOWiseGridView.DataSource = gridviewdt;
               performanceReportRecipeTUOWiseGridView.DataBind();

           }
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            wcnamequery = wcquery();
            //reportHeader._rDate = reportMasterFromDateTextBox.Text;

            if (optionDropDownList.SelectedItem.Text == "No")
                option = "1";
            else
                option = "2";

            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            //reportHeader._rDate = reportMasterFromDateTextBox.Text;

            string dtnadtime = TotalformatDate(rToDate);

            dtnadtime1 = TotalprodataformatDate(rToDate);

            if (QualityReportTUOWise.Checked)
            {                
                QualityReportTUOWisePanel.Visible = true;
                QualityReportRecipeTUOWisePanel.Visible = false;
                excelReport(query);
            }
            if (QualityReportRecipeTUOWise.Checked)
            {
                QualityReportTUOWisePanel.Visible = false;
                QualityReportRecipeTUOWisePanel.Visible = true;
                excelReportRecipeWise("default", rToDate, rFromDate);
            }
            Label1.Text = "false";
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
       public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public void excelReport(string query)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("Select DISTINCT  machinename from productiondataTUO order by machinename asc", con);
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
                    xlWorkSheet.Cells[1, 2] = "Performance Report TUO Machine Wise";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[3, 2]).EntireColumn.ColumnWidth = "20";
                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.get_Range("D3", "E3").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "H3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
                    xlWorkSheet.Cells[2, 2] = "To : " + formattoDate(rFromDate);
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                    xlWorkSheet.Cells[3, 1] = "Machine name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "OE";
                    xlWorkSheet.Cells[3, 6] = "";
                    xlWorkSheet.Cells[3, 7] = "Rep";
                    xlWorkSheet.Cells[3, 8] = "Scrap";

                    xlWorkSheet.Cells[4, 4] = "A";
                    xlWorkSheet.Cells[4, 5] = "B";
                    xlWorkSheet.Cells[4, 6] = "C";
                    xlWorkSheet.Cells[4, 7] = "D";
                    xlWorkSheet.Cells[4, 8] = "E";

                    ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                    xlWorkSheet.get_Range("A3", "C3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("D4", "H4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                    xlWorkSheet.get_Range("D3", "H3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("A3", "H3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    xlWorkSheet.get_Range("A4", "H4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                    while (dr.Read())
                    {
                        excelReportRecipeWise(dr["machinename"].ToString().Trim(), rToDate, rFromDate);
                    }

                    xlWorkSheet.get_Range("A" + (rowCount + 1), "H" + (rowCount + 1)).Merge(misValue);
                    xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                    xlWorkSheet.Cells[rowCount + 2, 3] = grandtotal;
                    xlWorkSheet.Cells[rowCount + 2, 4] = grandA;
                    xlWorkSheet.Cells[rowCount + 2, 5] = grandB;
                    xlWorkSheet.Cells[rowCount + 2, 6] = grandC;
                    xlWorkSheet.Cells[rowCount + 2, 7] = grandD;
                    xlWorkSheet.Cells[rowCount + 2, 8] = grandE;

                    xlWorkSheet.get_Range("A1", "H" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "H" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report TUO Machine Wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
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
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            int total, A, B, C, D, E;
            Double pA, pB, pC, pD, pE;

            int colCount = 1, mergeCount = 0, typeCount = 0;           
            query = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportTUOWise.Checked)
            {
                typeCount = 1;
                query = getqueryWCWise("machineName='" + wcName + "'", dtnadtime);

                xlWorkSheet.Cells[rowCount + 1, 1] = wcName;
                mergeCount = rowCount;
            }
            else if (QualityReportRecipeTUOWise.Checked)
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
                xlWorkSheet.Cells[1, 2] = "Performance Report TUO Machine Wise";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("C3", "D3").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "G3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
                xlWorkSheet.Cells[2, 2] = "To : " + formattoDate(rFromDate);
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "OE";
                xlWorkSheet.Cells[3, 5] = "";
                xlWorkSheet.Cells[3, 6] = "Rep";
                xlWorkSheet.Cells[3, 7] = "Scrap";
                xlWorkSheet.get_Range("C3", "D3").Merge(Type.Missing);
                
                xlWorkSheet.Cells[4, 3] = "A";
                xlWorkSheet.Cells[4, 4] = "B";
                xlWorkSheet.Cells[4, 5] = "C";
                xlWorkSheet.Cells[4, 6] = "D";
                xlWorkSheet.Cells[4, 7] = "E";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A3", "B3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("C4", "G4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("C3", "G3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "G3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "G4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                
                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM  ProductionDataTUO  WHERE ((testTime>" + dtnadtime;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
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

            total_ = 0; A_ = 0; B_ = 0; C_ = 0; D_ = 0; E_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; C = 0; D = 0; E = 0;
                pA = 0; pB = 0; pC = 0; pD = 0; pE = 0; rowCount++;

                total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                C = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
                D = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
                E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));

                total_ = total_ + total;
                A_ = A_ + A;
                B_ = B_ + B;
                C_ = C_ + C;
                D_ = D_ + D;
                E_ = E_ + E;

                DataRow dr = gridviewdt.NewRow();
                switch(option)
                {
                    case "1" :
                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0];
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = C;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = D;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = E;
                        break;
                    case "2" :                    
                        pA = ((total == 0) ? 0 : ((double)(A * 100) / total));
                        pB = ((total == 0) ? 0 : ((double)(B * 100) / total));
                        pC = ((total == 0) ? 0 : ((double)(C * 100) / total));
                        pD = ((total == 0) ? 0 : ((double)(D * 100) / total));
                        pE = ((total == 0) ? 0 : ((double)(E * 100) / total));

                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB, 1);

                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pC, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pD, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pE, 1);
                        break;
                    }

                gridviewdt.Rows.Add(dr);



            }
            DataRow ndr = gridviewdt.NewRow();
            
            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            xlWorkSheet.Cells[rowCount + 2, colCount + typeCount] = "Total";
            switch(option)
            {
                case "1" :
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = A_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = B_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = C_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = D_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = E_;
                    break;
                case "2" :            
                pA = ((total_ == 0) ? 0 : ((double)(A_ * 100) / total_));
                pB = ((total_ == 0) ? 0 : ((double)(B_ * 100) / total_));
                pC = ((total_ == 0) ? 0 : ((double)(C_ * 100) / total_));
                pD = ((total_ == 0) ? 0 : ((double)(D_ * 100) / total_));
                pE = ((total_ == 0) ? 0 : ((double)(E_ * 100) / total_));

                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_.ToString();
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = Math.Round(pA, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = Math.Round(pB, 1);

                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = Math.Round(pC, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = Math.Round(pD, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = Math.Round(pE, 1);
                break;
            }

            gridviewdt.Rows.Add(tdr);

            rowCount += 2;
            grandtotal = grandtotal + total_;
            grandA = grandA + A_;
            grandB = grandB + B_;
            grandC = grandC + C_;
            grandD = grandD + D_;
            grandE = grandE + E_;
                        
            if (QualityReportTUOWise.Checked)
            {               
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeTUOWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "G" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "G" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "G" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report TUO Machine Wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
               
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
