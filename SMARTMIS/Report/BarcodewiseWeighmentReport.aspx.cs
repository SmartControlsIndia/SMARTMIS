using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using OfficeOpenXml;

namespace SmartMIS.Report
{
    public partial class BarcodewiseWeighmentReport : System.Web.UI.Page
    {

        myConnection myconnection = new myConnection();
        DateTime fromdate, todate;
        string rToDate, rFromdate;
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "BarcodewiseWeighmentReport.xlsx";
        string filepath;

        smartMISWebService myWebService = new smartMISWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //fromdatecalendertextbox.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                //TodateCalendertextbox.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                //fromdate = Convert.ToDateTime(formatDate(fromdatecalendertextbox.Text));
                //todate = Convert.ToDateTime(formatDate(TodateCalendertextbox.Text)).AddDays(1);
                //ShowReport();

            }
        }

        //private void ShowReport()
        //{


        //    rToDate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd" + " 07:00:00");
        //    rFromdate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).ToString("yyyy-MM-dd" + " 07:00:00");
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        myconnection.open(ConnectionOption.SQL);
        //        myconnection.comm = myconnection.conn.CreateCommand();

        //        if (processDropDownList.SelectedItem.Text == "PCR")
        //        {
        //            myconnection.comm.CommandText = "select distinct( gtbarcode) as gtbarcode, weight,recipeCode,SpecWeight,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],SAPMaterialCode from vPCRBuddeScannedTyreDetail t1 where SpecWeight is not null and t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' and t2.weight!= '' ) and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' order by date,time desc ";
        //        }

        //        else if (processDropDownList.SelectedItem.Text == "TBR")
        //        {
        //            myconnection.comm.CommandText = "select distinct( gtbarcode) as gtbarcode, weight,recipeCode,SpecWeight,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],SAPMaterialCode from VBuddeScannedTyreDetail t1 where t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' ) and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' order by date,time desc ";
        //        }

        //        myconnection.reader = myconnection.comm.ExecuteReader();
        //        dt.Load(myconnection.reader);
        //        myconnection.comm.Dispose();
        //        myconnection.reader.Close();
        //        myconnection.close(ConnectionOption.SQL);

        //        string tempweight = "";

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            String[] weight = dt.Rows[i][1].ToString().Split('_');
        //            tempweight = "";

        //            for (int j = 0; j < weight.Length; j++)
        //            {
        //                if (weight[j].Length > 2)
        //                {
        //                    tempweight = weight[j].ToString();
        //                }
        //            }


        //            if (tempweight.Length < 2 || double.Parse(tempweight) < 1)
        //            {
        //                dt.Rows[i].Delete();
        //                // tbldt.Rows[i][1] = 0.0;
        //            }
        //            else
        //                dt.Rows[i][1] = tempweight;

        //        }
        //        dt.AcceptChanges();


        //        DataTable tbldt1 = new DataTable();
        //        tbldt1.Columns.Add("BARCODE", typeof(string));
        //        tbldt1.Columns.Add("WEIGHT", typeof(Double));
        //        tbldt1.Columns.Add("RECIPECODE", typeof(string));
        //        tbldt1.Columns.Add("SPECWEIGHT", typeof(Double));
        //        tbldt1.Columns.Add("DATE", typeof(string));
        //        tbldt1.Columns.Add("TIME", typeof(string));
        //        tbldt1.Columns.Add("SAPCODE", typeof(string));
        //        tbldt1.Columns.Add("VARIANCE", typeof(Double));

        //        //tbldt1 = dt.Copy();

        //        //tbldt1.Columns.Add("Variance", typeof(Double));
        //        foreach (DataRow sourcerow in dt.Rows)
        //        {
        //            DataRow destRow = tbldt1.NewRow();
        //            double res = Math.Round(Convert.ToDouble(sourcerow["SpecWeight"]) - Convert.ToDouble(sourcerow["weight"]), 2);
        //            destRow[0] = sourcerow[0].ToString();
        //            destRow[1] = sourcerow[1].ToString();
        //            destRow[2] = sourcerow[2].ToString();
        //            destRow[3] = sourcerow[3].ToString();
        //            destRow[4] = sourcerow[4].ToString();
        //            destRow[5] = sourcerow[5].ToString();
        //            destRow[6] = sourcerow[6].ToString();
        //            destRow[7] = res;
        //            tbldt1.Rows.Add(destRow);
        //        }

        //        ViewState["dt"] = tbldt1;

        //        WeighmentGridView.DataSource = tbldt1;
        //        WeighmentGridView.DataBind();


        //    }
        //    catch (Exception exc)
        //    {
        //        //mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

        //    }



        //}
    

        private void ShowReport()
        {


            rToDate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).AddDays(1).ToString("yyyy-MM-dd" + " 07:00:00");
            rFromdate = Convert.ToDateTime(myWebService.formatDate(fromdatecalendertextbox.Text)).ToString("yyyy-MM-dd" + " 07:00:00");
            DataTable dt = new DataTable();
          
            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();

                if (processDropDownList.SelectedItem.Text == "PCR")
                {
                    myconnection.comm.CommandText = "select distinct( gtbarcode) as gtbarcode, weight,recipeCode,SpecWeight,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],SAPMaterialCode from vPCRBuddeScannedTyreDetail t1 where SpecWeight is not null and t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' and t2.weight!= '' ) and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' order by date,time desc ";

                    myconnection.reader = myconnection.comm.ExecuteReader();
                    dt.Load(myconnection.reader);
                    myconnection.comm.Dispose();
                    myconnection.reader.Close();
                    myconnection.close(ConnectionOption.SQL);

                    string tempweight = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        String[] weight = dt.Rows[i][1].ToString().Split('_');
                        tempweight = "";

                        for (int j = 0; j < weight.Length; j++)
                        {
                            if (weight[j].Length > 2)
                            {
                                tempweight = weight[j].ToString();
                            }
                        }


                        if (tempweight.Length < 2 || double.Parse(tempweight) < 1)
                        {
                            dt.Rows[i].Delete();
                            // tbldt.Rows[i][1] = 0.0;
                        }
                        else
                            dt.Rows[i][1] = tempweight;

                    }

                    dt.AcceptChanges();


                    DataTable tbldt1 = new DataTable();
                    tbldt1.Columns.Add("BARCODE", typeof(string));
                    tbldt1.Columns.Add("WEIGHT", typeof(Double));
                    tbldt1.Columns.Add("RECIPECODE", typeof(string));
                    tbldt1.Columns.Add("SPECWEIGHT", typeof(Double));
                    tbldt1.Columns.Add("DATE", typeof(string));
                    tbldt1.Columns.Add("TIME", typeof(string));
                    tbldt1.Columns.Add("SAPCODE", typeof(string));
                    tbldt1.Columns.Add("VARIANCE", typeof(Double));
                    
                    foreach (DataRow sourcerow in dt.Rows)
                    {
                        DataRow destRow = tbldt1.NewRow();
                        double res = Math.Round(Convert.ToDouble(sourcerow["SpecWeight"]) - Convert.ToDouble(sourcerow["weight"]), 2);
                        destRow[0] = sourcerow[0].ToString();
                        destRow[1] = sourcerow[1].ToString();
                        destRow[2] = sourcerow[2].ToString();
                        destRow[3] = sourcerow[3].ToString();
                        destRow[4] = sourcerow[4].ToString();
                        destRow[5] = sourcerow[5].ToString();
                        destRow[6] = sourcerow[6].ToString();
                        destRow[7] = res;
                        tbldt1.Rows.Add(destRow);
                    }

                    ViewState["dt"] = tbldt1;

                    WeighmentGridView.DataSource = tbldt1;
                    WeighmentGridView.DataBind();

                }

                else if (processDropDownList.SelectedItem.Text == "TBR")
                {
                    //myconnection.comm.CommandText = "select distinct( gtbarcode) as gtbarcode, weight,recipeCode,SpecWeight,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],SAPMaterialCode from VBuddeScannedTyreDetail t1 where t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' ) and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' order by date,time desc ";
                    myconnection.comm.CommandText = @" Select Data.gtbarcode,Data.weight,Data.recipeCode,Data.SpecWeight,Data.DATE,Data.TIME,Data.SAPMaterialCode,New.name AS CuringRecipe,New.SAPMaterialCode AS CuringSapcode from (select distinct( gtbarcode) as gtbarcode, weight,recipeCode,SpecWeight,convert(char(10), dtandTime, 105) AS DATE,CONVERT(VARCHAR(8) , dtandTime , 108) AS [TIME],SAPMaterialCode from VBuddeScannedTyreDetail t1 where SpecWeight is not null and t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "' and t2.weight!= '' ) and dtandtime>='" + rFromdate + "' and dtandtime<'" + rToDate + "'  ) AS Data inner join VBuddeScannedTyreDetail_New New on Data.gtbarcode=New.gtbarCode order by date,time desc ";

                    myWebService.writeLogs(myconnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    myconnection.reader = myconnection.comm.ExecuteReader();
                    dt.Load(myconnection.reader);
                    myconnection.comm.Dispose();
                    myconnection.reader.Close();
                    myconnection.close(ConnectionOption.SQL);

                    string tempweight = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        String[] weight = dt.Rows[i][1].ToString().Split('_');
                        tempweight = "";

                        for (int j = 0; j < weight.Length; j++)
                        {
                            if (weight[j].Length > 2)
                            {
                                tempweight = weight[j].ToString();
                            }
                        }

                        if (tempweight.Length < 2 || double.Parse(tempweight) < 1)
                        {
                            dt.Rows[i].Delete();
                        }
                        else
                            dt.Rows[i][1] = tempweight;
                    }

                    dt.AcceptChanges();


                    DataTable tbldt1 = new DataTable();
                    tbldt1.Columns.Add("BARCODE", typeof(string));
                    tbldt1.Columns.Add("WEIGHT", typeof(Double));
                    tbldt1.Columns.Add("RECIPECODE", typeof(string));
                    tbldt1.Columns.Add("SPECWEIGHT", typeof(Double));
                    tbldt1.Columns.Add("DATE", typeof(string));
                    tbldt1.Columns.Add("TIME", typeof(string));
                    tbldt1.Columns.Add("SAPCODE", typeof(string));
                    tbldt1.Columns.Add("CuringRecipe", typeof(string));
                    tbldt1.Columns.Add("CuringSapcode", typeof(string));
                    tbldt1.Columns.Add("VARIANCE", typeof(Double));
                   

                   
                    foreach (DataRow sourcerow in dt.Rows)
                    {
                        DataRow destRow = tbldt1.NewRow();
                        double res = Math.Round(Convert.ToDouble(sourcerow["SpecWeight"]) - Convert.ToDouble(sourcerow["weight"]), 2);
                        destRow[0] = sourcerow[0].ToString();
                        destRow[1] = sourcerow[1].ToString();
                        destRow[2] = sourcerow[2].ToString();
                        destRow[3] = sourcerow[3].ToString();
                        destRow[4] = sourcerow[4].ToString();
                        destRow[5] = sourcerow[5].ToString();
                        destRow[6] = sourcerow[6].ToString();
                        destRow[7] = sourcerow[7].ToString();
                        destRow[8] = sourcerow[8].ToString();
                        destRow[9] = res;
                        tbldt1.Rows.Add(destRow);
                    }

                    ViewState["dt"] = tbldt1;

                    WeighmentGridView1.DataSource = tbldt1;
                    WeighmentGridView1.DataBind();
                }
  
            }
            catch (Exception exc)
            {
                //mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }



        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {

            if (processDropDownList.SelectedItem.Text == "TBR" || processDropDownList.SelectedItem.Text == "PCR")
                ShowReport();

            if (processDropDownList.SelectedItem.Text == "TBR")
            {
                WeighmentGridView1.Visible = true;
                WeighmentGridView.Visible = false;
                if (WeighmentGridView1.Rows.Count > 0)
                {
                    // Bind your grid here
                    lblNoRecord.Visible = false;
                }
                else
                {
                    lblNoRecord.Visible = true; // label for "Record Not Found"
                }
            }
            else if (processDropDownList.SelectedItem.Text == "PCR")
            {
                WeighmentGridView.Visible = true;
                WeighmentGridView1.Visible = false;
                if (WeighmentGridView.Rows.Count > 0)
                {
                    // Bind your grid here
                    lblNoRecord.Visible = false;
                }
                else
                {
                    lblNoRecord.Visible = true; // label for "Record Not Found"
                }
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
                    //myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }

        protected void excelButton_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=WeighmentReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("BarcodeWise Weighment Report");
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
