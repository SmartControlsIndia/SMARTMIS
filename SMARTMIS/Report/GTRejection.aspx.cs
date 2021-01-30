using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Collections;

namespace SmartMIS.Report
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        myConnection myconnection = new myConnection();
        StringBuilder htmlTable;
        DataTable rejectiondt;
        DataTable tbmdt;
        DataTable fulldatatable= new DataTable();

        DataTable manningdt;
        DataRow row;
        string TBMtype;

        smartMISWebService mywebservice = new smartMISWebService();
        //string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "GTRejection.xlsx";
        //string filepath;

        //public WebForm1()
        //{
        //    filepath = mywebservice.getExcelPath();
        //}
   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                doworkfunc();

                //ReportDocument rptdoc = new ReportDocument();
                //rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
                //rptdoc.SetDataSource(fulldatatable);
                //CrystalReportViewer1.ReportSource = rptdoc;

            }
           
           
        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            doworkfunc();

            // ReportDocument rptdoc = new ReportDocument();
            //rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
            //rptdoc.SetDataSource(fulldatatable);
            //CrystalReportViewer1.ReportSource=rptdoc;
            if (GridView1.Rows.Count > 0)
            {
               // Bind your grid here
                lblNoRecord.Visible = false;
            }
            else
            {
                lblNoRecord.Visible = true; // label for "Record Not Found"
            }
                

        }
        private void doworkfunc()
        {
            htmlTable = new StringBuilder();
           // fulldatatable = new DataTable();
            tbmdt = new DataTable();
            rejectiondt = new DataTable();
            manningdt = new DataTable();

            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = "select * from manningmaster";
                myconnection.reader = myconnection.comm.ExecuteReader();
                manningdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }

            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = "select wcName,type as status,defectName,firstName,lastName,gtBarcode,dtandTime from vGTRejection where  wcname='" + processDropDownList.SelectedItem.Text + "' and dtandtime>'" + mywebservice.formatDate(fromdatecalendertextbox.Text) + " " + "07:00:00" + "' and dtandtime<'" + mywebservice.formatDate(TodateCalendertextbox.Text) + " " + "07:00:00" + "'";
                mywebservice.writeLogs(myconnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                myconnection.reader = myconnection.comm.ExecuteReader();
                rejectiondt.Load(myconnection.reader);
                var barcode = rejectiondt.AsEnumerable().Select(row => row.Field<string>("gtbarcode")).ToArray();
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);


                if (barcode.Length != 0)
                {
                    string InQuery = "(";
                    for (int i = 0; i < barcode.Length; i++)
                    {
                        InQuery += "'" + barcode[i].ToString() + "',";
                    }
                    InQuery = InQuery.TrimEnd(',');
                    InQuery += ")";
                    if (processDropDownList.SelectedItem.Text == "TBRGTScrap")
                    {
                        TBMtype = "vTBMTBR";
                    }
                    else
                    {
                         TBMtype ="vTBMPCR";
                    }
                   

                    myconnection.open(ConnectionOption.SQL);
                    myconnection.comm = myconnection.conn.CreateCommand();
                    myconnection.comm.CommandText = "select GTbarcode, wcName,recipeCode,dtandTime as DOP,manningID,manningID2,manningID3 from " + TBMtype + " where gtbarcode in " + InQuery + "";
                    mywebservice.writeLogs(myconnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    myconnection.reader = myconnection.comm.ExecuteReader();
                    tbmdt.Load(myconnection.reader);
                    myconnection.comm.Dispose();
                    myconnection.reader.Close();
                    myconnection.close(ConnectionOption.SQL);
                }


                   GTRejectiondataplaceholder.Controls.Clear();htmlTable.Append("<table border='1' width=100%>");

                htmlTable.Append("<tr><th class=tableheadercolumn>Status</th><th class=tableheadercolumn>DefectName</th><th class=tableheadercolumn>InspectorName</th><th class=tableheadercolumn>GTbarcode</th><th class=tableheadercolumn>InspectionDate</th><th class=tableheadercolumn>InspectionTime</th><th class=tableheadercolumn>TBM WCName</th><th class=tableheadercolumn>RecipeCode</th><th class=tableheadercolumn>Date O.P.</th><th class=tableheadercolumn>Shift</th><th class=tableheadercolumn>Builder1</th><th class=tableheadercolumn>Builder2</th><th class=tableheadercolumn>Builder3</th></tr>");
                fulldatatable.Columns.Add("Status"); fulldatatable.Columns.Add("DefectName"); fulldatatable.Columns.Add("InspectorName"); fulldatatable.Columns.Add("GTbarcode"); fulldatatable.Columns.Add("InspectionDate"); fulldatatable.Columns.Add("InspectionTime"); fulldatatable.Columns.Add("TBM WCName"); fulldatatable.Columns.Add("RecipeCode"); fulldatatable.Columns.Add("Date O.P."); fulldatatable.Columns.Add("Shift"); fulldatatable.Columns.Add("Builder1"); fulldatatable.Columns.Add("Builder2"); fulldatatable.Columns.Add("Builder3");


                for (int i = 0; i < rejectiondt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    row = fulldatatable.NewRow();
                    var dr = rejectiondt.Select("gtbarcode = " + barcode[i] + "");
                    
                        row["Status"] = dr[0][1];
                        htmlTable.Append("<td class=tablecolumn>" + dr[0][1] + "</td>");
                        row["DefectName"] = dr[0][2];
                        htmlTable.Append("<td class=tablecolumn>" + dr[0][2] + "</td>");
                        row["InspectorName"] = dr[0][3] + " " + dr[0][4];

                        htmlTable.Append("<td class=tablecolumn>" + dr[0][3] + " " + dr[0][4] + "</td>");
                        row["GTbarcode"] = dr[0][5];

                        htmlTable.Append("<td class=tablecolumn>" + dr[0][5] + "</td>");
                        
                        //htmlTable.Append("<td class=tablecolumn>" + dr[0][6] + "</td>");
                         
                        DateTime dtValue = Convert.ToDateTime(dr[0][6]); 
                        var dt1=dtValue.ToString("dd-MM-yyyy");
                        var dt2=dtValue.ToString("HH:mm:ss");
                        row["InspectionDate"] = dt1;
                        row["InspectionTime"] = dt2;
                        //htmlTable.Append("<td class=tablecolumn>" +  dt1  + " " + dt2 + "</td>");
                        htmlTable.Append("<td class=tablecolumn>" + dt1 + "</td>");
                        htmlTable.Append("<td class=tablecolumn>" + dt2 + "</td>");
                        var dr1 = tbmdt.Select("gtbarcode =" + barcode[i] + "");
                        if (dr1.Length >= 1)
                        {
                            if (dr1[0][1] != DBNull.Value)
                            {
                                row["TBM WCName"] = dr1[0][1];

                                htmlTable.Append("<td class=tablecolumn>" + dr1[0][1] + "</td>");
                            }
                            else
                            {
                                row["TBM WCName"] = "";

                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }

                            if (dr1[0][2] != DBNull.Value)
                            {
                                row["Recipecode"] = dr1[0][2];

                                htmlTable.Append("<td class=tablecolumn>" + dr1[0][2] + "</td>");
                            }
                            else
                            {
                                row["Recipecode"] = "";
                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }
                            if (dr1[0][3] != DBNull.Value)
                            {
                                row["Date O.P."] = dr1[0][3];

                                htmlTable.Append("<td class=tablecolumn>" + dr1[0][3] + "</td>");
                            }
                            else
                            {
                                row["Date O.P."] = "";
                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }

                            
                            DateTime dop = Convert.ToDateTime(dr1[0][3]);
                            row["Shift"] = getshift(dop);
                            htmlTable.Append("<td class=tablecolumn>" + getshift(dop) + "</td>");
                            if (dr1[0][4] != DBNull.Value)
                            {

                                var results = manningdt.Select("iD =" + Convert.ToInt32(dr1[0][4]));

                                row["Builder1"] = results[0][2].ToString() + " " + results[0][3].ToString();

                                htmlTable.Append("<td class=tablecolumn>" + results[0][2].ToString() + " " + results[0][3].ToString() + "</td>");
                            }
                            else
                            {
                                row["Builder1"] = "";
                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }

                            if (dr1[0][5] != DBNull.Value)
                            {
                                var results1 = manningdt.Select("iD=" + Convert.ToInt32(dr1[0][5]));
                                htmlTable.Append("<td class=tablecolumn>" + results1[0][2].ToString() + " " + results1[0][3].ToString() + "</td>");
                                row["Builder2"] = results1[0][2].ToString() + " " + results1[0][3].ToString();

                            }
                            else
                            {
                                row["Builder2"] = "";
                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }

                            if (dr1[0][6] != DBNull.Value)
                            {
                                var results2 = manningdt.Select("iD=" + Convert.ToInt32(dr1[0][6]));
                                htmlTable.Append("<td class=tablecolumn>" + results2[0][2].ToString() + " " + results2[0][3].ToString() + "</td>");
                                row["Builder3"] = results2[0][2].ToString() + " " + results2[0][3].ToString();

                            }
                            else
                            {
                                row["Builder3"] = "";
                                htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");
                            }
                        }
                      
                    fulldatatable.Rows.Add(row);
                    htmlTable.Append("</tr>");
            }
                
                htmlTable.Append("</table>");

                ViewState["htmltable"] = htmlTable;
                ViewState["fulldatatable"] = fulldatatable;
                GridView1.DataSource = fulldatatable;
                GridView1.DataBind();
                //GTRejectiondataplaceholder.Controls.Add(new Literal { Text = htmlTable.ToString() });

            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        private string getshift(DateTime dt)
        {
            string shift = "";

            if (dt.Hour > 6 && dt.Hour < 15)
                shift = "A";
            else if (dt.Hour > 14 && dt.Hour < 23)
                shift = "B";
            else if (dt.Hour == 23 || (dt.Hour >= 0 && dt.Hour < 7))
                shift = "C";
            return shift;
 
        }



        protected void excelButton_Click(object sender, EventArgs e)
        {
            
            string type = processDropDownList.SelectedItem.Value;
            string getTimeDuration = "";

         
            Response.Clear();
            string filename = "GTRejection Report" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition","attachment;filename="+filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            stringWrite.Write("<table><tr><td><b>GTRejection Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + processDropDownList.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr></table>");

            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            GridView1.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();   
        }
        //Hide by sarita on 9-11-2015
        //    try
        //    {
        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        object misValue = System.Reflection.Missing.Value;
        //        string data=null;

        //        DataTable dt = new DataTable();
        //        DataTable curdt = new DataTable();
        //        //DataTable fulldatatable = new DataTable();

        //            xlApp = new Excel.ApplicationClass();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkBook.CheckCompatibility = false;
        //        xlWorkBook.DoNotPromptForConvert = true;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
        //        xlWorkSheet.Cells[1, 2] = "GT Rejection Excel Sheet";
        //        xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
        //        xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
        //        ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 2]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 3]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 8]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 9]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 10]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 11]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 12]).EntireColumn.ColumnWidth = "28";
        //        ((Excel.Range)xlWorkSheet.Cells[2, 15]).EntireColumn.ColumnWidth = "28";
        //        xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


        //        //add some text
        //        xlWorkSheet.Cells[1, 1] = "";
        //       //xlWorkSheet.Cells[2, 1] = "GTRejection Excel File";
        //        xlWorkSheet.Cells[2, 1] = "Status";
        //        xlWorkSheet.Cells[2, 2] = "DefectName";
        //        xlWorkSheet.Cells[2, 3] = "InspectorName";
        //        xlWorkSheet.Cells[2, 5] = "GTBarcode";
        //        xlWorkSheet.Cells[2, 5] = "InspectionDate";
        //        xlWorkSheet.Cells[2, 6] = "InspectionTime";
        //        xlWorkSheet.Cells[2, 7] = "TBM WCName";
        //        xlWorkSheet.Cells[2, 8] = "RecipeCode";
        //        xlWorkSheet.Cells[2, 9] = "Date O.P.";
        //        xlWorkSheet.Cells[2, 10] = "Shift";
        //        xlWorkSheet.Cells[2, 11] = "Builder1";
        //        xlWorkSheet.Cells[2, 12] = "Builder2";
        //        xlWorkSheet.Cells[2, 13] = "Builder3";

        //        int j = 0;

        //        foreach (GridViewRow row in GridView1.Rows)
        //        {
        //            j++;
        //            for (int i = 0; i < row.Cells.Count; i++)
        //            {
        //                xlWorkSheet.Cells[j + 2, i+1] = row.Cells[i].Text.ToString();

        //            }
        //        }
                

        //        xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlApp);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlWorkSheet);

        //        //MessageBox.Show ("File created !");
        //    }
        //    catch (Exception ex)
        //    {
        //        mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

        //    }
        //}
       // properly clean excel file
        //private void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //}

    }
        
     }

 

