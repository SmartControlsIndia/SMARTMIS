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
    public partial class PCRpaintingReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
      
        #endregion
        #region globle variable
       
        string sqlquery = "";
     
        DateTime fromDate, toDate;
      
        DataTable dt = new DataTable();

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "PCRpaintingReport.xlsx";
        string filepath;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            rowCountLabel.Text = "";

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
            string shift = ddlshift.SelectedItem.Text;
            string tyredesign = "";
            string duration = "";
            var datetimebt = "";
            string getfromdate = reportMasterFromDateTextBox.Text;

            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                    toDate = fromDate.AddDays(1);

                    string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                    string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                    showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);
                    break;

                //case "Month":
                //    nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                //    if (Convert.ToInt32(getMonth) < 12)
                //    {
                //        datetimebt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                //    }
                //    else
                //    { datetimebt = getYear.ToString() + "-" + (getMonth) + "-31 07:00:00"; }

                //    ntoDate = datetimebt;
                //    showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);

                //    break;

                case "DateFrom":
                    fromDate = DateTime.Parse(formatDate(tuoReportMasterFromDateTextBox.Text));
                     nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";// formatDate(tuoReportMasterFromDateTextBox.Text);

                    toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                    ntoDate = toDate.AddDays(1).ToString();
                    TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                    int result = (int)ts.TotalDays;
                    if ((int)ts.TotalDays > 7)
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 7 days!!!</font></strong></td></tr></table>";
                    }

                    else { showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign); ; }
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
               
                try
                {
                    dt.Columns.Add("SN", typeof(int));
                    dt.Columns.Add("Barcode", typeof(string));
                    dt.Columns.Add("Recipe", typeof(string));
                    dt.Columns.Add("WeightScale", typeof(string));
                    dt.Columns.Add("Shift", typeof(string));
                    dt.Columns.Add("Date", typeof(string));
                    dt.Columns.Add("Time", typeof(string));

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (ddlRecipe.SelectedItem.Text != "All" && ddlshift.SelectedItem.Text!="ALL")
                    {
                        myConnection.comm.CommandText = "select Barcode, Recipe ,WeightScale, Shift,convert(char(10), DateInserted, 105) AS Date,CONVERT(VARCHAR(8) , DateInserted , 108) AS [Time]  from paintingDataPCR where DateInserted > '" + nfromDate + "' and DateInserted<'" + ntoDate + "' and (recipe='" + ddlRecipe.SelectedItem.Text + "') and shift = '" + ddlshift.SelectedItem.Text + "'";
                    }
                    else if ((ddlRecipe.SelectedItem.Text != "All" )&&( ddlshift.SelectedItem.Text == "ALL"))
                    {
                        myConnection.comm.CommandText = "select Barcode, Recipe, WeightScale, Shift,convert(char(10), DateInserted, 105) AS Date,CONVERT(VARCHAR(8) , DateInserted , 108) AS [Time]  from paintingDataPCR where DateInserted > '" + nfromDate + "' and DateInserted<'" + ntoDate + "' and (recipe='" + ddlRecipe.SelectedItem.Text + "') ";
                    }
                    else if ((ddlRecipe.SelectedItem.Text == "All") && (ddlshift.SelectedItem.Text != "ALL"))
                    {
                        myConnection.comm.CommandText = "select Barcode, Recipe, WeightScale, Shift,convert(char(10), DateInserted, 105) AS Date,CONVERT(VARCHAR(8) , DateInserted , 108) AS [Time]  from paintingDataPCR where DateInserted > '" + nfromDate + "' and DateInserted<'" + ntoDate + "' and shift = '" + ddlshift.SelectedItem.Text + "'";
                    }
                    else 
                    {
                        myConnection.comm.CommandText = "select Barcode, Recipe, WeightScale, Shift,convert(char(10), DateInserted, 105) AS Date,CONVERT(VARCHAR(8) , DateInserted , 108) AS [Time]  from paintingDataPCR where DateInserted > '" + nfromDate + "' and DateInserted<'" + ntoDate + "' ";
                    }
                    myConnection.reader = myConnection.comm.ExecuteReader();

                    //dt.Load(myConnection.reader);
                    DataRow dr;
                    int sn = 1;
                    
                    if (myConnection.reader.HasRows)
                    {
                        while (myConnection.reader.Read())
                        {
                            dr = dt.NewRow();

                            dr["SN"] = sn;
                            dr["Barcode"] = myConnection.reader["Barcode"].ToString();
                            dr["Recipe"] = myConnection.reader["Recipe"].ToString();
                            //dr["WeightScale"] = Math.Round(Convert.ToDecimal(Math.Round(Convert.ToDecimal(myConnection.reader["WeightScale"].ToString()), 2)), 2);
                            dr["WeightScale"] = string.Format("{0:0.00}", Convert.ToDouble(myConnection.reader["WeightScale"].ToString())); 

                            dr["Shift"] = myConnection.reader["Shift"].ToString();
                            dr["Date"] = myConnection.reader["Date"].ToString();
                            dr["Time"] = myConnection.reader["Time"].ToString();

                            dt.Rows.Add(dr);

                            sn++;
                        }
                    }

                    rowCountLabel.Text = "Total Records: "+dt.Rows.Count.ToString();

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
               
                DataTable dtExcel = dt.Copy();
                ViewState["dt"] = dtExcel;

                if (dt.Rows.Count > 0)
                {
                    MainGridView.DataSource = dt;
                    MainGridView.DataBind();
                    MainGridView.Visible = true;
                    lbltext.Visible = false;
                }
                else
                {
                    lbltext.Text = "No Records Found";
                    lbltext.CssClass = "LabelTextAlignStyle";
                    lbltext.Visible = true;
                    MainGridView.Visible = false;
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
            myConnection.comm.CommandText = "Select DISTINCT recipe from paintingDataPCR where recipe != '0' and recipe !=''";
            myConnection.reader = myConnection.comm.ExecuteReader();
            d_dt.Load(myConnection.reader);

            ddlRecipe.DataSource = d_dt;
            ddlRecipe.DataTextField = "recipe";
            ddlRecipe.DataValueField = "recipe";
            ddlRecipe.DataBind();
            ddlRecipe.Items.Insert(0, new ListItem("All", "All"));
            
        }

        
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PCRpaintingReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PCRpaintingReport");
            ws.Cells["A1"].Value = "PCR Painting Report ";

            using (ExcelRange r = ws.Cells["A1:F1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(13, 25, 93));
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
}
