using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Text;
using OfficeOpenXml;
using System.Drawing;


namespace SmartMIS.Report
{
    public partial class UniformityReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable maindt = new DataTable();
        DataTable mainGVdt;

        string day, month, year;
        string duration, getType, getOperator;
        string fromdate = "", todate = "";
        ArrayList tempwcname;
        DataTable exldt;
        DataTable dtTUO = new DataTable();
        DataTable curdt = new DataTable();
        DataTable tbmdt = new DataTable();
        DataTable manndt = new DataTable();
        DataTable wcdt = new DataTable();
        DataTable dtclassification = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



                fromdatecalendertextbox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                TodateCalendertextbox.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                // viewReport_Click(sender, e);

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
        protected void viewReport_Click(object sender, EventArgs e)
        {
            try
            {
                int totalrank = 0;
                    createGridView(MainGridView);
                     loadData();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void loadData()
        {
            DataTable maindt = new DataTable();

            maindt.Columns.Add("Barcode", typeof(string));
            maindt.Columns.Add("ClassifiactionName", typeof(string));
            maindt.Columns.Add("TyreSize", typeof(string));
            maindt.Columns.Add("status", typeof(string));
            maindt.Columns.Add("defectname", typeof(string));
            maindt.Columns.Add("Parametername", typeof(string));
            maindt.Columns.Add("ParameterValue", typeof(string));
            maindt.Columns.Add("ClassifierName", typeof(string));
            maindt.Columns.Add("Remarks", typeof(string));
            maindt.Columns.Add("CFdate", typeof(string));
            maindt.Columns.Add("Classi_Time", typeof(string));
           
            maindt.Columns.Add("machine_TUO", typeof(string));
            maindt.Columns.Add("dtandtime_TUO", typeof(string));
            maindt.Columns.Add("MachineName", typeof(string));
            maindt.Columns.Add("Builder_Name", typeof(string));
            maindt.Columns.Add("TBM_dtandTime", typeof(string));
           
            maindt.Columns.Add("PressNo", typeof(string));
            maindt.Columns.Add("cavity", typeof(string));
            maindt.Columns.Add("MouldNo", typeof(string));
            maindt.Columns.Add("Cur_operator", typeof(string));
            maindt.Columns.Add("CureDAte", typeof(string));
           
            DataRow dr;
            try
            {
                    myConnection.open(ConnectionOption.SQL);
             fromdate = myWebService.formatDate(fromdatecalendertextbox.Text.ToString()) + " 07:00:00";
             todate = myWebService.formatDate(TodateCalendertextbox.Text.ToString()) + " 07:00:00";

             string fromDate = myWebService.formatDate(fromdatecalendertextbox.Text.ToString());
             string toDate = myWebService.formatDate(TodateCalendertextbox.Text.ToString());

                    //TimeSpan ts = DateTime.Parse(toDate) - DateTime.Parse(fromDate);
                    //int result = (int)ts.TotalDays;
                    //if ((int)ts.TotalDays >7)
                    //{
                    //    ShowWarning.Visible = true;
                    //    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more  than 7 days!!!</font></strong></td></tr></table>";
                    //}

                    List<string> conditions = new List<string>();
                    string query = "";

                    //Get TUO details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select distinct  barcode,Name,Sizename,Statusname ,defectArea,Parametername,parameterValue,firstName,Defect_name as Remark , convert(char(10), dtandTime, 105) AS CFdate,CONVERT(VARCHAR(8) , dtandtime , 108) AS [class_time] from vAfterTUOInspection where  dtandTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' order by CFdate,class_time asc";


                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dtclassification.Load(myConnection.reader);
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
                //productionDataTuo
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select  barcode ,MachineName,TestTime from (select  barcode ,MachineName,TestTime,  row_number() over (partition by barcode order by TestTime desc) as rono from ProductionDataTUO where TestTime > '" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' and TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' ) as t where rono = 1";
                       // myConnection.comm.CommandText = "Select distinct barcode as tuogtbarCode,MachineName,TestTime from productiondataTUO where  TestTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";


                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
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
                    //Get TBM details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmPCR t1 where   dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' ";

                        
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        tbmdt.Load(myConnection.reader);
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
                    //Get CUR details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, RIGHT(pressbarcode,8) as cavityNo,  case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as  mouldNo,Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringpcr where  (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        curdt.Load(myConnection.reader);
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
                        myConnection.close(ConnectionOption.SQL);
                    }

                    var row = from r0w1 in dtclassification.AsEnumerable()
                              join r0w2 in dtTUO.AsEnumerable()
                                on r0w1.Field<string>("barcode") equals r0w2.Field<string>("barcode") into p
                              from r0w2 in p.DefaultIfEmpty()
                              join r0w3 in tbmdt.AsEnumerable()
                                on r0w1.Field<string>("barcode") equals r0w3.Field<string>("tbmgtbarCode") into ps
                              from r0w3 in ps.DefaultIfEmpty()

                              join r0w4 in curdt.AsEnumerable()
                                on r0w1.Field<string>("barcode") equals r0w4.Field<string>("curgtbarCode") into pst
                              from r0w4 in pst.DefaultIfEmpty()
                              //select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray : new object[] { 0, 0, 0 })).ToArray();
                              select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "","" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "","","" }).Concat(r0w4 != null ? r0w4.ItemArray.Skip(1) : new object[] { "", "", "", "" }).ToArray();


                    MainGridView.DataSource = dtclassification;
                MainGridView.DataBind();

                foreach (object[] values in row)
                    maindt.Rows.Add(values);
               // DataTable dt = maindt.Copy();
                ViewState["dt"] = maindt;

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

      


        public string formatDate(string date)
        {
            string flag = "";
            string[] tempDate = date.Split(new char[] { '-' });
            month = tempDate[1].ToString();
            day = tempDate[0].ToString();
            year = tempDate[2].ToString();
            flag = month + "-" + day + "-" + year;
            return flag;
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
        public bool validateInput(string duration, string type, string fromDate, string toDate, int month, int year, int yearwise)
        {
            try
            {
                string durationQuery = "";
                var tdqte = "";
                durationQuery += "(dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";

            }
            catch (Exception exp)
            {
                // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Uniformity.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Uniformity");

            ws.Cells["A1"].Value = "Uniformity Report";

            using (ExcelRange r = ws.Cells["A1:S1"])
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
}
