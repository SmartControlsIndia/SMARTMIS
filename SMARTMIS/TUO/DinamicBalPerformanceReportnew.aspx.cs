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
using OfficeOpenXml;

namespace SmartMIS.TUO
{
    public partial class DinamicBalPerformanceReportnew : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable exceldt = new DataTable();
        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "";
        string query = "";
        string[] tempString2;
        double grandtotal, grandupperA, grandlowerA, grandstaticA, grandupperB, grandlowerB, grandstaticB, grandupperC, grandlowerC, grandstaticC, grandupperD, grandlowerD, grandstaticD, grandupperE, grandlowerE, grandstaticE;

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {

                    reportHeader._rDate = DateTime.Now.ToString("dd-MM-yyyy");
                    if (!Page.IsPostBack)
                    {
                        fillSizedropdownlist();
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

                    }
                    DynamicBalReportPanel.Visible = false;
                    backDiv.Visible = false;
                    RecipeWisePanel.Visible = false;
                }
            }
            catch (Exception exp)
            {
            
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
        protected void viewData_Click(object sender, EventArgs e)
        {
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text);

            fromDate.Text = rFromDate.ToString();
            toDate.Text = rToDate.ToString();
            
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);


            exceldt.Columns.Add("wcName", typeof(string));
            exceldt.Columns.Add("tireType", typeof(string));
            exceldt.Columns.Add("Checked", typeof(string));
            exceldt.Columns.Add("UpperA", typeof(string));
            exceldt.Columns.Add("LowerA", typeof(string));
            exceldt.Columns.Add("StaticA", typeof(string));
            exceldt.Columns.Add("UpperB", typeof(string));
            exceldt.Columns.Add("LowerB", typeof(string));
            exceldt.Columns.Add("StaticB", typeof(string));
            exceldt.Columns.Add("UpperC", typeof(string));
            exceldt.Columns.Add("LowerC", typeof(string));
            exceldt.Columns.Add("StaticC", typeof(string));
            exceldt.Columns.Add("UpperD", typeof(string));
            exceldt.Columns.Add("LowerD", typeof(string));
            exceldt.Columns.Add("StaticD", typeof(string));
            exceldt.Columns.Add("UpperE", typeof(string));
            exceldt.Columns.Add("LowerE", typeof(string));
            exceldt.Columns.Add("StaticE", typeof(string));
         


            if (QualityReportTBMWise.Checked)
            {
                    dinamicBalReportRecipeWisePanel.Visible = true;
                    RecipeWisePanel.Visible = false;
                    showReport(query);

             }
             else if (QualityReportRecipeWise.Checked)
             {
                    dinamicBalReportRecipeWisePanel.Visible = false;
                    RecipeWisePanel.Visible = true;
                    showReportRecipeWise(dinamicBalMainRecipeWiseGridView, "", rToDate, rFromDate);
             }


            //reportHeader.ReportDate = tempString2[3].ToString();
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
        private void fillBarCodeDetailGridView(string tyreType, string wcName)
        {
            try
            {
                DataTable gridviewdt = new DataTable();
                DataTable dt = new DataTable();
                DataTable pcrdt = new DataTable();

                dt.Columns.Add("TireType", typeof(string));
                dt.Columns.Add("barCode", typeof(string));
                pcrdt.Columns.Add("wcName", typeof(string));
                pcrdt.Columns.Add("gtbarCode", typeof(string));

                gridviewdt.Columns.Add("tbmWCName", typeof(string));
                gridviewdt.Columns.Add("size", typeof(string));
                gridviewdt.Columns.Add("barcode", typeof(string));

                if (tyreType != "Total")
                    tyreType = " and tireType='" + tyreType + "'";
                else
                    tyreType = "";

                if (!string.IsNullOrEmpty(wcName))
                    tyreType += " AND wcID='" + getWCID(wcName) + "'";
                string dtnadtime = TotalprodataformatDate(toDate.Text, fromDate.Text);
                string query = "SELECT TireType,barCode FROM productionDataPCRDB WHERE ((dtandTime>=" + dtnadtime + tyreType;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(myConnection.reader);
                
                myConnection.conn.Close();
                myConnection.comm.Dispose();
                myConnection.reader.Close();

                if (dt.Rows.Count != 0)
                {
                    string InQuery = "(";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        InQuery += "'" + dt.Select()[i][1].ToString() + "',";
                    }
                    InQuery = InQuery.TrimEnd(',');
                    InQuery += ")";

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT wcName,gtbarCode FROM vTbmPCR WHERE gtbarCode IN " + InQuery.ToString(), con);
                    var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    pcrdt.Load(dread);

                    con.Close();
                    cmd.Dispose();
                    dread.Close();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = gridviewdt.NewRow();
                        dr[1] = dt.Select()[i][0].ToString();
                        dr[2] = dt.Select()[i][1].ToString();

                        for (int j = 0; j < pcrdt.Rows.Count; j++)
                        {
                            try
                            {
                                dr[0] = pcrdt.Select("gtbarCode='" + dt.Rows[i][1].ToString() + "'")[j][0].ToString();
                            }
                            catch (Exception e) { }
                        }

                        gridviewdt.Rows.Add(dr);
                    }
                }

                DynamicBalReportBarcodeDetailGridView.DataSource = gridviewdt;
                DynamicBalReportBarcodeDetailGridView.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void viewDetails_Click(object sender, EventArgs e)
        {
            //rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            //rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());
            
            try
            {
               
                if (((Button)sender).ID == "RejectionDetails")
                {
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    Button bt = ((Button)gridViewRow.Cells[1].FindControl("RejectionDetails"));
                    string wcName = (((Label)gridViewRow.Cells[1].FindControl("dinamicBalSizeWisewcNameLabel")).Text);
                    string tireType = (((Label)gridViewRow.Cells[1].FindControl("dinamicBalSizeWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(tireType, "6301");

                    DynamicBalReportPanel.Visible = true;
                    backDiv.Visible = true;
                }
                if (((Button)sender).ID == "RejectionDetailsRecipeWise")
                {
                    GridViewRow gridviewrow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string tireType = (((Label)gridviewrow.Cells[1].FindControl("dinamicBalSizeWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(tireType, "");

                    DynamicBalReportPanel.Visible = true;
                    backDiv.Visible = true;

                }
                               
                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void fillRecipeWiseGridView(string query)
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add("workcenterName", typeof(string));
            DataRow dr = dt.NewRow();
            dr[0] = "6301";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1[0] = "mitsubishiDB";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2[0] = "6304";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3[0] = "6305";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4[0] = "6306";
            dt.Rows.Add(dr4);





            try
            {

                dinamicBalMainRecipeWiseGridView.DataSource = dt; //myWebService.fillGridView(query, ConnectionOption.SQL);
                dinamicBalMainRecipeWiseGridView.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "dinamicBalMainRecipeWiseGridView")
                    {

                        string workcentername = ((Label)e.Row.FindControl("dinamicBalSizeWisewcNameLabel")).Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("DinamicBalRecipeWiseChildGridView"));
                        showReportRecipeWise(childGridView, workcentername, rToDate, rFromDate);
                        //fillChildInnerGridView("3401", childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), "1");
                    }
                    if (((GridView)sender).ID == "DinamicBalRecipeWiseGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    if (((GridView)sender).ID == "DinamicBalRecipeWiseChildGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    if (((GridView)sender).ID == "DynamicBalReportBarcodeDetailGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                }

            }

            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void fillSizedropdownlist()
        {
            FilterDynamicBalancingSizeDropdownlist.Items.Clear();
            FilterDynamicBalancingSizeDropdownlist.DataSource = FillDropDownList("productionDataPCRDB", "tireType");
            FilterDynamicBalancingSizeDropdownlist.DataBind();
        }
        #endregion
        #region User Defined Function
        private void showReport(string query)
        {
            fillRecipeWiseGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE processID=19 order by workCenterName asc");

            ViewState["dt"] = exceldt;

        }
        private string getWCID(string wcname)
        {
            string ret = "";
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "SELECT iD FROM vworkCenter WHERE workcentername= '" + wcname + "'";
            myConnection.reader = myConnection.comm.ExecuteReader();

            if (myConnection.reader.HasRows)
            {
                if (myConnection.reader.Read())
                {
                    ret = myConnection.reader["iD"].ToString();
                }
            }
            myConnection.close(ConnectionOption.SQL);
            myConnection.comm.Dispose();
            myConnection.reader.Close();
            return ret;
        }
        private double validateDT(DataTable dt, string avgOf, DataTable uniqrecipedt, int i, string grade)
        {
            try
            {
                if (FilterOptionDropDownList.SelectedValue == "1")
                    return Math.Round(Convert.ToDouble(dt.Compute("avg(" + avgOf + ")", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + grade).ToString()), 1);
                else if (FilterOptionDropDownList.SelectedValue == "0")
                    return Convert.ToDouble(dt.Compute("count(tireType)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND " + grade).ToString());
                else
                        return 0;
               
            }
            catch (Exception exp)
            {
                return 0;
            }
        }
        private void showReportRecipeWise(GridView childgridview, string wcName, string rtodate, string rfromdate)
        {
            string tablename="";
            if (wcName == "6301" || wcName == "mitsubishiDB")
                tablename = "productionDataPCRDB";
            else
                tablename = "productionDataPCRDBNew";
            string wcID = getWCID(wcName);


            string query;
            string query1;
            DataTable dt = new DataTable();
            DataTable temp = new DataTable();
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("wcName", typeof(string));
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("UpperA", typeof(string));
            gridviewdt.Columns.Add("LowerA", typeof(string));
            gridviewdt.Columns.Add("StaticA", typeof(string));
            gridviewdt.Columns.Add("UpperB", typeof(string));
            gridviewdt.Columns.Add("LowerB", typeof(string));
            gridviewdt.Columns.Add("StaticB", typeof(string));
            gridviewdt.Columns.Add("UpperC", typeof(string));
            gridviewdt.Columns.Add("LowerC", typeof(string));
            gridviewdt.Columns.Add("StaticC", typeof(string));
            gridviewdt.Columns.Add("UpperD", typeof(string));
            gridviewdt.Columns.Add("LowerD", typeof(string));
            gridviewdt.Columns.Add("StaticD", typeof(string));
            gridviewdt.Columns.Add("UpperE", typeof(string));
            gridviewdt.Columns.Add("LowerE", typeof(string));
            gridviewdt.Columns.Add("StaticE", typeof(string));
            gridviewdt.Columns.Add("AvgRFVCW", typeof(string));

            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("UPPER", typeof(double));
            dt.Columns.Add("LOWER", typeof(double));
            dt.Columns.Add("STATIC", typeof(double));
            dt.Columns.Add("GRADEDEF", typeof(double));
            dt.Columns.Add("UPPERGRADE", typeof(double));
            dt.Columns.Add("LOWERGRADE", typeof(double));
            dt.Columns.Add("STATICGRADE", typeof(double));

            temp.Columns.Add("tireType", typeof(string));
            temp.Columns.Add("UPPER", typeof(double));
            temp.Columns.Add("LOWER", typeof(double));
            temp.Columns.Add("STATIC", typeof(double));
            temp.Columns.Add("GRADEDEF", typeof(double));
            temp.Columns.Add("UPPERGRADE", typeof(double));
            temp.Columns.Add("LOWERGRADE", typeof(double));
            temp.Columns.Add("STATICGRADE", typeof(double));
                 
            query = "";
            query1 = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportRecipeWise.Checked)
            {

                if (FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM  productionDataPCRDB  WHERE (dtandTime> '" + rtodate + "' and dtandTime<'" + rfromdate + "')";
                    query1 = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM  productionDataPCRDBNew  WHERE (dtandTime> '" + rtodate + "' and dtandTime<'" + rfromdate + "')";



                }
                else if (FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text != "All")
                {

                    query = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM productionDataPCRDB  WHERE  tireType='" + FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text + "' and  (dtandTime> '" + rtodate + "' and dtandTime<'" + rfromdate + "')";
                    query1 = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM productionDataPCRDBNew  WHERE  tireType='" + FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text + "' and  (dtandTime> '" + rtodate + "' and dtandTime<'" + rfromdate + "')";

                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                if (FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM " + tablename + "  WHERE wcID='" + wcID + "' AND (dtandTime> '"+rtodate+"' and dtandTime<'"+rfromdate+"')";
                }
                else if (FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select tireType, UPPER, LOWER, STATIC, GRADEDEF, UPPERGRADE, LOWERGRADE, STATICGRADE FROM " + tablename + "  WHERE wcID='" +wcID + "' AND tireType='" + FilterDynamicBalancingSizeDropdownlist.SelectedItem.Text + "' and  (dtandTime> '"+rtodate+"' and dtandTime<'"+rfromdate+"')";
                }
            }
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

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

            if (query1 != "")
            {
                temp.Clear();
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = query1;

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    temp.Load(myConnection.reader);
                    dt.Merge(temp);

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



            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");
            double total, upperA, lowerA, staticA, upperB, lowerB, staticB, upperC, lowerC, staticC, upperD, lowerD, staticD, upperE, lowerE, staticE;
            double total_, upperA_, lowerA_, staticA_, upperB_, lowerB_, staticB_, upperC_, lowerC_, staticC_, upperD_, lowerD_, staticD_, upperE_, lowerE_, staticE_; 
            total_ = 0; upperA_ = 0; lowerA_ = 0; staticA_ = 0; upperB_ = 0; lowerB_ = 0; staticB_ = 0; upperC_ = 0; lowerC_ = 0; staticC_ = 0; upperD_ = 0; lowerD_ = 0; staticD_ = 0; upperE_ = 0; lowerE_ = 0; staticE_ = 0;
            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; upperA = 0; lowerA = 0; staticA = 0; upperB = 0; lowerB = 0; staticB = 0; upperC = 0; lowerC = 0; staticC = 0; upperD = 0; lowerD = 0; staticD = 0; upperE = 0; lowerE = 0; staticE = 0;

                total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                upperA = validateDT(dt, "UPPER", uniqrecipedt, i,  (" UPPERGRADE = '1'"));
                lowerA = validateDT(dt, "LOWER", uniqrecipedt, i,  (" LOWERGRADE = '1'"));
                staticA = validateDT(dt, "STATIC", uniqrecipedt, i, (" STATICGRADE = '1'"));
                upperB = validateDT(dt, "UPPER", uniqrecipedt, i, (" UPPERGRADE = '2'"));
                lowerB = validateDT(dt, "LOWER", uniqrecipedt, i, (" LOWERGRADE = '2'"));
                staticB = validateDT(dt, "STATIC", uniqrecipedt, i, (" STATICGRADE = '2'"));
                upperC = validateDT(dt, "UPPER", uniqrecipedt, i, (" UPPERGRADE = '3'"));
                lowerC = validateDT(dt, "LOWER", uniqrecipedt, i, (" LOWERGRADE = '3'"));
                staticC = validateDT(dt, "STATIC", uniqrecipedt, i, (" STATICGRADE = '3'"));
                upperD = validateDT(dt, "UPPER", uniqrecipedt, i, (" UPPERGRADE = '4'"));
                lowerD = validateDT(dt, "LOWER", uniqrecipedt, i, (" LOWERGRADE = '4'"));
                staticD = validateDT(dt, "STATIC", uniqrecipedt, i, (" STATICGRADE = '4'"));
                upperE = validateDT(dt, "UPPER", uniqrecipedt, i, (" UPPERGRADE = '5'"));
                lowerE = validateDT(dt, "LOWER", uniqrecipedt, i, (" LOWERGRADE = '5'"));
                staticE = validateDT(dt, "STATIC", uniqrecipedt, i, (" STATICGRADE = '5'"));

                total_ += total;
                upperA_ += upperA;
                lowerA_ += lowerA;
                staticA_ += staticA;
                upperB_ += upperB;
                lowerB_ += lowerB;
                staticB_ += staticB;
                upperC_ += upperC;
                lowerC_ += lowerC;
                staticC_ += staticC;
                upperD_ += upperD;
                lowerD_ += lowerD;
                staticD_ += staticD;
                upperE_ += upperE;
                lowerE_ += lowerE;
                staticE_ += staticE;

                DataRow dr = gridviewdt.NewRow();
                dr[0] = wcName.ToString();
                dr[1] = uniqrecipedt.Rows[i][0].ToString();
                dr[2] = total.ToString();
                dr[3] = upperA.ToString();
                dr[4] = lowerA.ToString();
                dr[5] = staticA.ToString();
                dr[6] = upperB.ToString();
                dr[7] = lowerB.ToString();
                dr[8] = staticC.ToString();
                dr[9] = upperC.ToString();
                dr[10] = lowerC.ToString();
                dr[11] = staticC.ToString();
                dr[12] = upperD.ToString();
                dr[13] = lowerD.ToString();
                dr[14] = staticD.ToString();
                dr[15] = upperE.ToString();
                dr[16] = lowerE.ToString();
                dr[17] = staticE.ToString();

                gridviewdt.Rows.Add(dr);

            }
            
            DataRow tdr = gridviewdt.NewRow();

            tdr[0] = wcName.ToString();
            tdr[1] = "Total";
            tdr[2] = total_;
            tdr[3] = upperA_;
            tdr[4] = lowerA_;
            tdr[5] = staticA_;
            tdr[6] = upperB_;
            tdr[7] = lowerB_;
            tdr[8] = staticB_;
            tdr[9] = upperC_;
            tdr[10] = lowerC_;
            tdr[11] = staticC_;
            tdr[12] = upperD_;
            tdr[13] = lowerD_;
            tdr[14] = staticD_;
            tdr[15] = upperE_;
            tdr[16] = lowerD_;
            tdr[17] = staticE_;

            gridviewdt.Rows.Add(tdr);

            grandtotal += total_;
            grandupperA += upperA_;
            grandlowerA += lowerA_;
            grandstaticA += staticA_;
            grandupperB += upperB_;
            grandlowerA += lowerA_;
            grandstaticB += staticB_;
            grandupperC += upperC_;
            grandlowerC += lowerC_;
            grandstaticC += staticC_;
            grandupperD += upperD_;
            grandlowerD += lowerD_;
            grandstaticD += staticD_;
            grandupperE += upperE_;
            grandlowerE += lowerE_;
            grandstaticE += staticE_;


            exceldt.Merge(gridviewdt);

            if (QualityReportTBMWise.Checked)
            {
                childgridview.DataSource = gridviewdt;
                childgridview.DataBind();
              
            }
            else if (QualityReportRecipeWise.Checked)
            {
                ViewState["dt"] = gridviewdt;
                DinamicBalRecipeWiseGridView.DataSource = gridviewdt;
                DinamicBalRecipeWiseGridView.DataBind();
              

            }

        }
        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private void showReportRecipeWise(string query)
        {
            if (rType == "dayWise")
            {
                string todate = formattoDate(rToDate);
                string fromdate = formatfromDate(rFromDate);
                fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataPCRDB WHERE (" + wcIDQuery + ") and (dtandTime>'" + todate + "' and dtandTime<='" + fromdate + "')");

            }
            else if (rType == "monthWise")
            {
                fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataPCRDB WHERE (" + wcIDQuery + ") and (datepart(MM,dtandTime)='" + rToMonth + "' and datepart(YYYY,dtandTime)='" + rToYear + "')");


            }
        }
        private void fillChildGridView(GridView childgridview, string query)
        {
            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        private void fillChildInnerGridView(string wcname, GridView childGridView, string recipecode, String toDate, String fromDate, String option)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {
                if (rType == "dayWise")
                {
                    string dtnadtime = TotalformatDate(rToDate);

                    if (childGridView.ID == "DinamicBalRecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_dinamicBalReportRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                    }

                }


                if (rType == "monthWise")
                {

                    if (childGridView.ID == "DinamicBalRecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_dinamicBalReportMonthRecipeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                    }

                }


            }










            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public DataTable fillGridView(string procedureName, string wcName, string recipeCode, string rToDate, string rFromDate, ConnectionOption option)
        {
            DataTable flag = new DataTable();

            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01

            if (option == ConnectionOption.SQL)
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter machineNameParameter = new System.Data.SqlClient.SqlParameter("@wcname", System.Data.SqlDbType.VarChar);
                    machineNameParameter.Direction = System.Data.ParameterDirection.Input;
                    machineNameParameter.Value = wcName;

                    System.Data.SqlClient.SqlParameter tyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    tyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    tyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter toDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    toDateParameter.Direction = System.Data.ParameterDirection.Input;
                    toDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter fromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    fromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    fromDateParameter.Value = rFromDate;

                    myConnection.comm.Parameters.Add(machineNameParameter);
                    myConnection.comm.Parameters.Add(tyreTypeParameter);
                    myConnection.comm.Parameters.Add(toDateParameter);
                    myConnection.comm.Parameters.Add(fromDateParameter);

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);
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
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {
            RecipeWisePanel.Visible = true;
            dinamicBalReportRecipeWisePanel.Visible = false;
            DinamicBalRecipeWiseGridView.DataSource = null;
            DinamicBalRecipeWiseGridView.DataBind();
        }
        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {

            RecipeWisePanel.Visible = false;
            dinamicBalReportRecipeWisePanel.Visible = true;
            dinamicBalMainRecipeWiseGridView.DataSource = null;
            dinamicBalMainRecipeWiseGridView.DataBind();
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                FilterDynamicBalancingSizeDropdownlist.SelectedIndex = FilterDynamicBalancingSizeDropdownlist.Items.IndexOf(FilterDynamicBalancingSizeDropdownlist.Items.FindByText("All"));


            }

        }
        public int AlltotalcheckedQuantity()
        {
            int flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))

                            flag = Convert.ToInt32(myConnection.reader[0]);

                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where  (" + wcIDQuery + ")  and datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + "";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))

                            flag = Convert.ToInt32(myConnection.reader[0]);

                        else
                            flag = 0;
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
        public Double AlltotalAQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate, rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='1' and  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='1' and  (" + wcIDQuery + ")  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";

                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalBQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate, rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='2' and  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }

                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='2' and  (" + wcIDQuery + ")  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalCQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate, rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='3' and  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")

                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='3' and  (" + wcIDQuery + ")  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")

                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalDQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate, rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='4' and  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }

                }
                else if (rType == "monthWise")
                {

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='4' and  (" + wcIDQuery + ")  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalEQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate, rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='5' and  (" + wcIDQuery + ")  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from productionDataPCRDB where GradeDEF='5' and (" + wcIDQuery + ")  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";



                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(date);
                    flag = tempDate.ToString("yyyy-MM-dd");
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
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = year + "-" + month + "-" + (Convert.ToInt32(day) + 1).ToString() + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag = year + "-" + (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        public string TotalformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";
            if (date != null)
            {
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

                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' " + ")OR" + " " + "(dtandTime>'" + flag3 + "'and" + " " + "dtandTime<" + "'" + flag4 + "'))";

                }

                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
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

                    if (Convert.ToInt32(tday) == 12 && Convert.ToInt32(tday) == 31)
                    {
                        flag2 = "01-01-" + (Convert.ToInt32(tyear) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    if (DateTime.DaysInMonth(Convert.ToInt32(tyear), Convert.ToInt32(tmonth)) != Convert.ToInt32(tday))
                    {
                        flag2 = tmonth + "-" + (Convert.ToInt32(tday) + 1).ToString() + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag2 = (Convert.ToInt32(tmonth) + 1).ToString() + "-" + "01" + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    //flag2 = tmonth + "-" + tday + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";


                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' ))";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

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
        public string createQuery(String wcID)
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
        public string createwcIDQuery(String wcID)
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
        #endregion

        protected void expToExcel_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DBReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            ws.Cells["A1"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Medium2);
            ws.Cells.AutoFitColumns();
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();




        }
    }
}
