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

namespace SmartMIS.Report
{
    public partial class visualinspection2 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery;
        public string recipeCode, globalrecipecode,percent_sign = null;
        string tableName;
        myConnection myConnection = new myConnection();

        int pid = -1;

        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "SecondTBRVisualInspectionReport.xlsx";
        string filepath;

        public visualinspection2()
        {
            filepath = myWebService.getExcelPath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tuoReportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
                {
                    tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
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

                    tuoReportMasterToDateTextBox.Text = showToDate.ToString();

                }
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {

                }
                showDownload.Text = "";
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
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "VIRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("VISizeWiseTyreTypeLabel"));
                        GridView ssornssGridView = ((GridView)e.Row.FindControl("ssornssGridView"));
                        
                        fillssornssGridView(ssornssGridView, recipeCodeLabel.Text, rToDate, rFromDate);
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    else if (((GridView)sender).ID == "ssornssGridView")
                    {

                        Label recipeIDLabel = ((Label)e.Row.FindControl("VISizeRecipeIDLabel"));

                         GridView childGridView = ((GridView)e.Row.FindControl("VIRecipeWiseChildGridView"));
                         Label ssORnss = ((Label)e.Row.FindControl("VISizeWisessORnssLabel"));

                         fillChildInnerGridView("3401", "gg", childGridView, recipeIDLabel.Text, rToDate, rFromDate, "1", ssORnss.Text);
                         e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                         e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    if (((GridView)sender).ID == "VIRecipeWiseChildGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    
                }

            }

            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        public void fillssornssGridView(GridView childGridView, string recipeCodeLabel, String toDate, String fromDate)
        {
            DataTable dt = new DataTable();
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select distinct curingRecipeID,ssORnss=(CASE WHEN ssORnss='1' THEN 'ss' WHEN ssORnss='2' THEN 'nss' WHEN ssORnss='0' THEN '' END) from vTBRVisualInspectionReport WHERE curingRecipeID = '" + recipeCodeLabel + "' AND  dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcName = '7003'";
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            childGridView.DataSource = dt;
            childGridView.DataBind();
        }
        
        private void fillChildGridView(GridView childgridview, string query)
        {
            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        private void fillChildInnerGridView(string faltType, string faltArea, GridView childGridView, string recipecode, String toDate, String fromDate, String option, string ssornss)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {


                if (childGridView.ID == "VIRecipeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_TBRVIReportRecipeWise2_Nos", faltType, recipecode, toDate, fromDate, ssornss, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }

                else if (childGridView.ID == "VIRecipeFaultWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_TBRVIReportRecipeFaultWise_Nos", faltType, recipecode, toDate, fromDate, ssornss, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }
            }

            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        public int totalcheckedcount = 0, buffcount = 0, repaircount = 0, ncmrcount = 0;
        public DataTable fillGridView(string procedureName, string faltType, string recipeCode, string rToDate, string rFromDate, string ssornss, ConnectionOption option)
        {
            DataTable flag = new DataTable();
            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            if (getdisplaytype == "Percent")
                percent_sign = "%";
            
            string ssnss = null;
            if (ssornss == "ss")
                ssnss = "1";
            else if (ssornss == "nss")
                ssnss = "2";
            else
                ssnss = "0";

            if (option == ConnectionOption.SQL)
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter displayType = new System.Data.SqlClient.SqlParameter("@displayType", System.Data.SqlDbType.VarChar);
                    displayType.Direction = System.Data.ParameterDirection.Input;
                    displayType.Value = getdisplaytype;

                    System.Data.SqlClient.SqlParameter machineNameParameter = new System.Data.SqlClient.SqlParameter("@faultType", System.Data.SqlDbType.VarChar);
                    machineNameParameter.Direction = System.Data.ParameterDirection.Input;
                    machineNameParameter.Value = ssnss;

                    System.Data.SqlClient.SqlParameter tyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    tyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    tyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter toDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    toDateParameter.Direction = System.Data.ParameterDirection.Input;
                    toDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter fromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    fromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    fromDateParameter.Value = rFromDate;

                    myConnection.comm.Parameters.Add(displayType);
                    myConnection.comm.Parameters.Add(machineNameParameter);
                    myConnection.comm.Parameters.Add(tyreTypeParameter);
                    myConnection.comm.Parameters.Add(toDateParameter);
                    myConnection.comm.Parameters.Add(fromDateParameter);
                    
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter ndisplayType = new System.Data.SqlClient.SqlParameter("@displayType", System.Data.SqlDbType.VarChar);
                    ndisplayType.Direction = System.Data.ParameterDirection.Input;
                    ndisplayType.Value = "Numbers";
                    
                    System.Data.SqlClient.SqlParameter nmachineNameParameter = new System.Data.SqlClient.SqlParameter("@faultType", System.Data.SqlDbType.VarChar);
                    nmachineNameParameter.Direction = System.Data.ParameterDirection.Input;
                    nmachineNameParameter.Value = ssnss;

                    System.Data.SqlClient.SqlParameter ntyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    ntyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    ntyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter ntoDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    ntoDateParameter.Direction = System.Data.ParameterDirection.Input;
                    ntoDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter nfromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    nfromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    nfromDateParameter.Value = rFromDate;

                    myConnection.comm.Parameters.Add(ndisplayType);
                    myConnection.comm.Parameters.Add(nmachineNameParameter);
                    myConnection.comm.Parameters.Add(ntyreTypeParameter);
                    myConnection.comm.Parameters.Add(ntoDateParameter);
                    myConnection.comm.Parameters.Add(nfromDateParameter);

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (myConnection.reader.HasRows)
                    {
                        myConnection.reader.Read();
                        totalcheckedcount += Convert.ToInt32(myConnection.reader["TotalChecked"].ToString());
                        buffcount += Convert.ToInt32(myConnection.reader["Buff"].ToString());
                        repaircount += Convert.ToInt32(myConnection.reader["Repair"].ToString());
                        ncmrcount += Convert.ToInt32(myConnection.reader["NCMR"].ToString());
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

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
            con.Open();

            SqlCommand cmd = new SqlCommand("select distinct curingRecipeID from vTBRVisualInspectionReport where dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcName = '7003'", con);
            var dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();


            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    string recipeid = dr["curingRecipeID"].ToString();
                    myConnection.comm.CommandText = "select iD, description FROM recipeMaster WHERE iD = '" + recipeid + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }

            VIRecipeWiseGridView.DataSource = dt;
            VIRecipeWiseGridView.DataBind();            
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
        
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));
                
                DataTable dt = new DataTable();
                DataTable curdt = new DataTable();
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
                xlWorkSheet.Cells[1, 2] = "Second TBR Visual Inspection Report";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 2]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 3]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 8]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 9]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 10]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 11]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 12]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[3, 15]).EntireColumn.ColumnWidth = "20";
                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("A2", "B2").Merge(misValue);
                xlWorkSheet.get_Range("C2", "D2").Merge(misValue);
                xlWorkSheet.get_Range("C3", "C4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "H3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
                xlWorkSheet.Cells[2, 3] = "To : " + formattoDate(rFromDate);
                xlWorkSheet.get_Range("A2", "D2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "S. No.";
                xlWorkSheet.Cells[3, 2] = "Shift";
                xlWorkSheet.Cells[3, 3] = "TyreSize";
                xlWorkSheet.Cells[3, 4] = "Press No.";
                xlWorkSheet.Cells[3, 5] = "Cavity";
                xlWorkSheet.Cells[3, 6] = "Mould No.";
                xlWorkSheet.Cells[3, 7] = "Side";
                xlWorkSheet.Cells[3, 8] = "Building Date";
                xlWorkSheet.Cells[3, 9] = "Building Time";
                xlWorkSheet.Cells[3, 10] = "Builder Name";
                xlWorkSheet.Cells[3, 11] = "Barcode";
                xlWorkSheet.Cells[3, 12] = "Defect";
                xlWorkSheet.Cells[3, 13] = "Disposal";
                xlWorkSheet.Cells[3, 14] = "Remark";
                xlWorkSheet.Cells[3, 15] = "Responsibility";
                xlWorkSheet.get_Range("A3", "O3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                xlWorkSheet.get_Range("A3", "O3").Font.Bold = true;

                string query = "select description, wcName, CAST(dtandTime AS DATE) AS getdate, convert(char(8), dtandTime, 108) AS gettime, firstName + ' ' + lastName As builderName, gtbarcode, defectName, defectstatusName, remarks, shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END) from vTBRVisualInspectionReport where dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "' AND wcName = '7003'";

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
                    string InQuery = "(";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        InQuery += "'" + dt.Select()[i][5].ToString() + "',";
                    }
                    InQuery = InQuery.TrimEnd(',');
                    InQuery += ")";

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT wcName, mouldNo, gtbarCode FROM vCuringpcr WHERE gtbarCode IN " + InQuery.ToString(), con);
                    var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    curdt.Load(dread);

                    con.Close();
                    cmd.Dispose();
                    dread.Close();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        xlWorkSheet.Cells[i + 4, 1] = i+1;
                        xlWorkSheet.Cells[i + 4, 2] = dt.Rows[i][9];
                        xlWorkSheet.Cells[i + 4, 3] = dt.Rows[i][0];
                        xlWorkSheet.Cells[i + 4, 4] = dt.Rows[i][1];

                        for (int j = 0; j < curdt.Rows.Count; j++)
                        {
                            try
                            {
                                xlWorkSheet.Cells[i + 4, 6] = curdt.Select("gtbarCode='" + dt.Rows[i][5].ToString() + "'")[j][1].ToString();
                            }
                            catch (Exception ex) { }
                        }

                        xlWorkSheet.Cells[i + 4, 8] = dt.Rows[i][2];
                        xlWorkSheet.Cells[i + 4, 9] = dt.Rows[i][3];
                        xlWorkSheet.Cells[i + 4, 10] = dt.Rows[i][4];
                        xlWorkSheet.Cells[i + 4, 11] = dt.Rows[i][5];
                        xlWorkSheet.Cells[i + 4, 12] = dt.Rows[i][6];
                        xlWorkSheet.Cells[i + 4, 13] = dt.Rows[i][7];
                        xlWorkSheet.Cells[i + 4, 14] = dt.Rows[i][8];
                    }
                    xlWorkSheet.get_Range("A1", "O" + (dt.Rows.Count + 3)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "O" + (dt.Rows.Count + 3)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }


                xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Second TBR Visual Inspection Report</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
            
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                KillProcess(pid, "EXCEL");
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
