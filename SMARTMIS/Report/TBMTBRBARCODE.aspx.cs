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
    public partial class TBMTBRBARCODE : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        // string moduleName = "PCRVIAdmin";
        #endregion
        DateTime fromDate, toDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //fillDesigndropdownlist();
                fillSizedropdownlist();

                rowCountLabel.Text = "";
                ShowWarning.Visible = false;
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
            string recipe = ddlRecipe.SelectedItem.Text;
            string tyredesign = "";// processDropDownList.SelectedItem.Text;
            string duration = "";
            var datetimebt = "";
            string getfromdate = reportMasterFromDateTextBox.Text;

            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                    toDate = fromDate.AddDays(1);

                    string nfromDate = fromDate.ToString("yyyy-MM-dd") + " 07:00:00";
                    string ntoDate = toDate.ToString("yyyy-MM-dd") + " 07:00:00";
                    showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);
                    ShowWarning.Visible = false;
                    break;
                case "DateFrom":

                    nfromDate = formatDate(tuoReportMasterFromDateTextBox.Text);

                    toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                    ntoDate = toDate.AddDays(1).ToString();
                    TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                    int result = (int)ts.TotalDays;
                    if ((int)ts.TotalDays > 2)
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 2 days!!!</font></strong></td></tr></table>";
                    }

                    else
                    {
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);
                        ShowWarning.Visible = false;
                    }
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


            }
        }
        protected void showReportDateMonthWise(string nfromDate, string ntoDate, string recipe, string tyredesign)
        {
            string wcname = ddlmachine.SelectedItem.Text;
            string recipename = ddlRecipe.SelectedItem.Text;
            string shift = ddlshift.SelectedValue;
            DataTable rdt = new DataTable();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (wcname != "ALL" && recipename != "All" && shift != "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT,TBMStatus AS STATUS  FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcName='" + wcname.TrimEnd() + "' and recipeCode='" + recipename + "' and shift='" + shift + "'order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else if (wcname != "ALL" && recipename == "All" && shift != "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT ,TBMStatus AS STATUS FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcName='" + wcname.TrimEnd() + "'  and shift='" + shift + "' order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else if (wcname != "ALL" && recipename == "All" && shift == "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT ,TBMStatus AS STATUS FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcName='" + wcname + "'  order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else if (wcname == "ALL" && recipename != "All" && shift == "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT,TBMStatus AS STATUS  FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' and recipeCode='" + recipename + "'  order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else if (wcname == "ALL" && recipename != "All" && shift != "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT ,TBMStatus AS STATUS FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' and recipeCode='" + recipename + "'  and shift='" + shift + "' order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else if (wcname == "ALL" && recipename == "All" && shift != "ALL")
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT ,TBMStatus AS STATUS FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "'  and shift='" + shift + "' order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";
                }
                else
                {
                    myConnection.comm.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,wcName as MACHINENAME,gtbarcode AS BARCODE,recipeCode AS RECIPE ,SAPMaterialCode AS SAPCODE,
convert(char(10), dtandtime, 105)  as DATE, convert(varchar(8), dtandtime, 108) as TIME,shift AS SHIFT ,TBMStatus AS STATUS  FROM vtbmtbr where dtandtime>='" + nfromDate + "' and dtandtime<'" + ntoDate + "' order by dtandtime desc";// "select dbo.vCuringtbr.gtbarCode, dbo.vTbmtbR.recipeCode, convert(char(10), vCuringtbr.dtandtime, 105) AS Date,convert(char(10), vCuringtbr.dtandtime, 108) AS Time, dbo.vCuringtbr.wcName, dbo.vCuringtbr.shift FROM  dbo.vCuringtbr INNER JOIN dbo.vTbmtbR ON dbo.vCuringtbr.gtbarCode = dbo.vTbmtbR.gtbarCode  where dbo.vCuringtbr.dtandtime>='" + nfromDate + "' and dbo.vCuringtbr.dtandtime<'" + ntoDate + "' order by dbo.vCuringtbr.dtandtime desc";

                } myConnection.reader = myConnection.comm.ExecuteReader();
                rdt.Load(myConnection.reader);

                string status = ddlstatus.SelectedValue;

                                if (status == "ALL")
                 {
                     if (rdt.Rows.Count > 0)
                     {
                         grdCuringBarcode.DataSource = rdt;
                         grdCuringBarcode.DataBind();
                         ViewState["dt"] = rdt;
                         rowCountLabel.Text = "Total Records: " + rdt.Rows.Count.ToString();
                     }
                     else
                     {
                         grdCuringBarcode.DataSource = "";
                         grdCuringBarcode.DataBind();
                         lbltext.Text = "No Records: " + rdt.Rows.Count.ToString();
                         ViewState["dt"] = "";
                         rowCountLabel.Text = "Total Records: 0";
                     }

                  
                 }
                 else
                 {

                     if (rdt.Rows.Count > 0)
                     {
                         if (status == "OK")
                         {
                             int tblCheckOK = rdt.AsEnumerable().Where(row => row.Field<String>("STATUS") == "OK").Count();
                             if (tblCheckOK > 0)
                             {
                                 DataTable tblFiltered = rdt.AsEnumerable().Where(row => row.Field<String>("STATUS") == "OK").CopyToDataTable();
                                 grdCuringBarcode.DataSource = tblFiltered;
                                 grdCuringBarcode.DataBind();
                                 ViewState["dt"] = tblFiltered;
                                 rowCountLabel.Text = "Total Records: " + tblFiltered.Rows.Count.ToString();
                             }
                             else
                             {
                                 grdCuringBarcode.DataSource = "";
                                 grdCuringBarcode.DataBind();
                                 ViewState["dt"] = "";
                                 rowCountLabel.Text = "Total Records: 0";
                             }

                           
                         }
                         else if (status == "HOLD")
                         {
                             int tblCheckHOLD = rdt.AsEnumerable().Where(row => row.Field<String>("STATUS") == "HOLD").Count();

                             if (tblCheckHOLD > 0)
                             {
                                 DataTable tblFiltered = rdt.AsEnumerable().Where(row => row.Field<String>("STATUS") == "HOLD").CopyToDataTable();
                                 grdCuringBarcode.DataSource = tblFiltered;
                                 grdCuringBarcode.DataBind();
                                 ViewState["dt"] = tblFiltered;
                                 rowCountLabel.Text = "Total Records: " + tblFiltered.Rows.Count.ToString();
                             }
                             else
                             {
                                 grdCuringBarcode.DataSource = "";
                                 grdCuringBarcode.DataBind();
                                 ViewState["dt"] = "";
                                 rowCountLabel.Text = "Total Records: 0";
                             }
                           
                         }
                     }
                     else
                     {
                         grdCuringBarcode.DataSource = "";
                         grdCuringBarcode.DataBind();
                         lbltext.Text = "No Records: " + rdt.Rows.Count.ToString();

                          ViewState["dt"] = "";
                          rowCountLabel.Text = "Total Records: 0";
                     }
                 }
              
            
               // ViewState["dt"] = rdt;
               // rowCountLabel.Text = "Total Records: " + rdt.Rows.Count.ToString();

            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                if (!myConnection.reader.IsClosed)
                    myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
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
        private void fillSizedropdownlist()
        {

            DataTable d_dt = new DataTable();

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "Select DISTINCT id as rID,name from recipemaster where description != '0' and description !='' and processID = 4 order by name asc ";
            myConnection.reader = myConnection.comm.ExecuteReader();
            d_dt.Load(myConnection.reader);
            ddlRecipe.DataSource = d_dt;
            ddlRecipe.DataTextField = "name";
            ddlRecipe.DataValueField = "rID";
            ddlRecipe.DataBind();
            ddlRecipe.Items.Insert(0, new ListItem("All", "All"));
            //ddlRecipe.DataSource = null;
            //ddlRecipe.DataSource = FillDropDownList("recipemaster", "description");
            //ddlRecipe.DataBind();
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {

            if (ViewState["dt"] != null && ViewState["dt"] != "")
            {
                DataTable dt = (DataTable)ViewState["dt"];
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TBMBTR.xls");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TBMTBRReport");
                ws.Cells["A1"].Value = "TBMTBR Report ";

                using (ExcelRange r = ws.Cells["A1:I1"])
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
        protected void ddlRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {



        }

    }
}
