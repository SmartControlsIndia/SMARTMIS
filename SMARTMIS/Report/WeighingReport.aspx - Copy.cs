using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml;

namespace SmartMIS.Report
{
    public partial class WeighingReport : System.Web.UI.Page
    {
        string recipeCode = "";
        string duration, durationQuery = "";
        DateTime fromDate, toDate;
        string processType, day, month, year;
        DataTable recipedt = new DataTable();
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        protected void Page_Load(object sender, EventArgs e)
        {

            

            if (!IsPostBack)
            {

                fillSizedropdownlist();


                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    //reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
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
        protected void exptoexcel_Click(object sender, EventArgs e)
        {
            //  DataTable dt = 
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GTWeighinhReport.xls");
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
        protected void Button_click(object sender, EventArgs e)
        {
            string getMonth = DropDownListMonth.SelectedItem.Value;
            string getYear = DropDownListYear.SelectedItem.Text;
            string getProcess ="PCR";
            string recipeCode = ddlRecipe.SelectedItem.Text;
           // string shift = ddlshift.SelectedItem.Text;
            string tyredesign = "";
            string duration = "";
            var datetimebt = "";
            string getfromdate = reportMasterFromDateTextBox.Text;

            getRecipeSpec();

            // Redirect the flow according to the duration selected by the user
            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    fromDate = DateTime.Parse(formatDate(getfromdate));
                    toDate = fromDate.AddDays(1);

                    string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                    string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                      showReportDateWise(nfromDate, ntoDate, recipeCode,getProcess);
                  
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
                      showReportDateWise(nfromDate, ntoDate, recipeCode,getProcess);

                    break;

                //case "DateFrom":
                //    fromDate = DateTime.Parse(formatDate(tuoReportMasterFromDateTextBox.Text));
                //    nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";// formatDate(tuoReportMasterFromDateTextBox.Text);

                //    toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                //    ntoDate = toDate.AddDays(1).ToString();
                //    TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                //    int result = (int)ts.TotalDays;
                //    if ((int)ts.TotalDays > 7)
                //    {
                //        ShowWarning.Visible = true;
                //        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 7 days!!!</font></strong></td></tr></table>";
                //    }

                //    else { showReportDateWise(nfromDate, ntoDate, recipeCode, getProcess); }
                //    break;

            }





        }
         //protected void showReportDateMonthWise(string nfromDate, string ntoDate, string recipe, string tyredesign)
        protected void showReportDateWise(string fromDate, string toDate, string type, string process)
        {
            // Validate the user input with proper message
            if (validateInput("date", type, fromDate, toDate, 0, 0, 0))
            {

                showReportDayWcWise(fromDate, toDate, type, process);

            }
        }
        protected void showReportMonthWise(int getMonth, int getYear, string type, string process)
        {
            if (validateInput("month", type, "", "", getMonth, getYear, 0))
            {

                showReportMonthWcWise(getMonth, getYear, process);

            }
        }
        //protected void showReportYearWise(int getYearwise, string type)
        //{
        //    if (validateInput("year", type, "", "", 0, 0, getYearwise))
        //    {
        //        switch (type)
        //        {
        //            case "WcWise":
        //                showReportYearWcWise(getYearwise);
        //                break;

        //            case "RecipeWise":
        //                showReportYearRecipeWise(getYearwise);
        //                break;

        //            case "OperatorWise":
        //                showReportYearOperatorWise(getYearwise);
        //                break;
        //        }
        //    }
        //}
        public bool validateInput(string duration, string type, string fromDate, string toDate, int month, int year, int yearwise)
        {
            try
            {
                durationQuery = "";
                // Create query for particular duration
                switch (duration)
                {
                    case "date":
                        durationQuery += "(DateInserted >= '" + fromDate + " 07:00:00' AND DateInserted < '" + toDate + " 07:00:00')";
                        break;
                    case "month":
                        string rtoDate = "";
                        string rfromDate = year.ToString() + "-" + month + "-01 07:00:00";
                        if (month == 12)
                            rtoDate = (year + 1).ToString() + "-01-01 07:00:00";
                        else
                            rtoDate = year.ToString() + "-" + (month + 1) + "-01 07:00:00";
                        durationQuery += "(DateInserted >= '" + rfromDate + "' AND DateInserted < '" + rtoDate + "')";
                        break;
                    case "year":
                        string nfromDate = yearwise.ToString() + "-01-01 07:00:00";
                        string ntoDate = (yearwise + 1).ToString() + "-01-01 07:00:00";
                        durationQuery += "(DateInserted >= '" + nfromDate + "' AND DateInserted < '" + ntoDate + "')";
                        break;
                }




            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return true;
        }
        #region DayWise Report
        protected void showReportDayWcWise(string fromDate, string toDate, string recipecode, string process)
        {
            try
            {
                string tablename = "";
                DataTable tbldt = new DataTable();
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("TireType", typeof(string));
                gridviewdt.Columns.Add("ProductType", typeof(string));
                gridviewdt.Columns.Add("TotalTire", typeof(string));
                gridviewdt.Columns.Add("Sapcode", typeof(string));
                gridviewdt.Columns.Add("Spec Weight", typeof(string));
                gridviewdt.Columns.Add("Avg Weight", typeof(string));
                gridviewdt.Columns.Add("WithIn <= +1%", typeof(string));
                gridviewdt.Columns.Add("WithIn -1%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-1%", typeof(string));
                gridviewdt.Columns.Add("WithIn<= +2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn -2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn<= +5%", typeof(string));
                gridviewdt.Columns.Add("WithIn -5%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-5%", typeof(string));


                DataTable finaldt = new DataTable();

                finaldt.Columns.Add("TireType", typeof(string));
                finaldt.Columns.Add("ProductType", typeof(string));
                finaldt.Columns.Add("TotalTire", typeof(string));
                finaldt.Columns.Add("Sapcode", typeof(string));
                finaldt.Columns.Add("Spec Weight", typeof(string));
                finaldt.Columns.Add("Qty", typeof(string));
                finaldt.Columns.Add("Per.", typeof(string));
                finaldt.Columns.Add("1Qty", typeof(string));
                finaldt.Columns.Add("1%", typeof(string));
                finaldt.Columns.Add("-1Qty.", typeof(string));
                finaldt.Columns.Add("-1%", typeof(string));
                finaldt.Columns.Add("+/-1Qty", typeof(string));
                finaldt.Columns.Add(" +/-1%", typeof(string));
                finaldt.Columns.Add("2.5Qty", typeof(string));
                finaldt.Columns.Add("2.5%", typeof(string));
                finaldt.Columns.Add("-2.5Qty", typeof(string));
                finaldt.Columns.Add("-2.5%", typeof(string));
                finaldt.Columns.Add(" +/-2.5Qty", typeof(string));
                finaldt.Columns.Add(" +/-2.5%", typeof(string));
                finaldt.Columns.Add("+5Qty", typeof(string));
                finaldt.Columns.Add("+5%", typeof(string));
                finaldt.Columns.Add("-5Qty", typeof(string));
                finaldt.Columns.Add("-5%", typeof(string));
                finaldt.Columns.Add("+/-5Qty", typeof(string));
                finaldt.Columns.Add("+/-5%", typeof(string));
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

               if (process == "PCR")
                    tablename = "vGTweighing";

               if (recipecode == "All")
                {
                    myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode,SAPMaterialCode as Sapcode  from vGTweighing t1 where  t1.DateInserted = (SELECT MAX(t2.DateInserted)FROM vGTweighing t2 WHERE t2.gtbarCode = t1.gtbarCode and DateInserted>= '" + fromDate + "' AND DateInserted < '" + toDate + "')   and DateInserted >= '" + fromDate + "' AND DateInserted < '" + toDate + "'";
                    
                    // myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode from "+tablename+" WHERE  " + durationQuery + "";
                }
                else
                {
                    myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode,SAPMaterialCode as Sapcode from vGTweighing t1 where  t1.DateInserted = (SELECT MAX(t2.DateInserted)FROM vGTweighing t2 WHERE t2.gtbarCode = t1.gtbarCode and DateInserted>= '" + fromDate + "' AND DateInserted < '" + toDate + "' and recipeCode='" + recipecode + "' ) and recipeCode='" + recipecode + "' and DateInserted >= '" + fromDate + "' AND DateInserted < '" + toDate + "' ";
                   
                }
                //myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode from " + tablename + "WHERE recipeCode='" + recipeCode + "'  AND " + durationQuery + "";
                //myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode from " + tablename + " WHERE recipeCode='" + recipeCode + "'  AND " + durationQuery + "";

                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                string tempweight = "";

                for (int i = 0; i < tbldt.Rows.Count; i++)
                {
                    String[] WeightScale = tbldt.Rows[i][1].ToString().Split('_');
                    tempweight = "";

                    for (int j = 0; j < WeightScale.Length; j++)
                    {
                        if (WeightScale[j].Length > 2)
                        {
                            tempweight = WeightScale[j].ToString();
                        }
                    }
                    //if (WeightScale[j].Length < 2)
                    //{
                    //    tempweight = WeightScale[j].ToString();
                    //}
                    //else
                    //{
                    //    tbldt.Rows[i][1] = tempweight;
                    //}


                }
                tbldt.AcceptChanges();

                DataTable tbldt1 = new DataTable();
                tbldt1.Columns.Add("recipeCode", typeof(string));
                tbldt1.Columns.Add("WeightScale", typeof(Double));
                tbldt1.Columns.Add("DateInserted", typeof(DateTime));

                tbldt1.Load(tbldt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);


                for (int i = tbldt1.Rows.Count - 1; i >= 0; i--)
                {
                    if (tbldt1.Rows[i][0] == DBNull.Value)
                        tbldt1.Rows[i].Delete();
                }
                tbldt1.AcceptChanges();


                var distinctvalues = tbldt1.AsEnumerable()
                   .Select(s => new
                   {
                       recipeCode = s.Field<string>("recipeCode"),
                       SapCode = s.Field<string>("SapCode")
                   })
                   .Distinct().ToList();
                int totalyire = 0;

                for (int i = 0; i < distinctvalues.Count; i++)
                {
                    try
                    {
                        DataRow dr = finaldt.NewRow();

                        // var count = tbldt1.AsEnumerable()
                        //  .Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode)
                        //  .GroupBy(g => new { barcode = g.Field<string>("gtbarcode"), })
                        //.Select(row => new
                        //{
                        //    date1 = row.Max(T => T.Field<DateTime>("DateInserted")),
                        //    //weight = row.Key.weight,
                        //})
                        //.ToList();

                        var count = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Select(row => row.Field<Double>("WeightScale")).ToList();
                        Double average = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Average(row => row.Field<Double>("WeightScale"));
                        average = Math.Round(average, 2);
                        dr[0] = distinctvalues[i].recipeCode;

                        var description = (from r in recipedt.AsEnumerable()
                                           where r.Field<string>("name").Equals(distinctvalues[i].recipeCode)
                                           select r).ToList();


                        dr[1] = description[0][2].ToString();

                        var aa = (from r in recipedt.AsEnumerable() where r.Field<string>("name").Equals(distinctvalues[i].recipeCode) select r.Field<double>("specweight")).ToList();
                        Double specweight = aa[0];
                        Double specwOnePecent = (specweight * 1) / 100;
                        //Double specminusOne = (specweight * -1) / 100;
                        Double specPlustwopintfive = (specweight * 2.5) / 100;
                        //  Double specMinuswopintfive = (specweight * -2.5) / 100;
                        Double specPlusfive = (specweight * 5) / 100;


                        specwOnePecent = Math.Round(specwOnePecent, 2);
                        specPlustwopintfive = Math.Round(specPlustwopintfive, 2);
                        specPlusfive = Math.Round(specPlusfive, 2);
                        totalyire = totalyire + count.Count;
                        dr[2] = count.Count.ToString();
                        dr[3] = distinctvalues[i].SapCode;
                        dr[4] = aa[0].ToString();
                        var plusone = new List<Double>();
                        var minusone = new List<Double>();
                        var plusMinusOne = new List<Double>();
                        var plusTwopointfive = new List<Double>();
                        var minusTwoPointFive = new List<Double>();
                        var plusMinusTwoPointFive = new List<Double>();
                        var plusfive = new List<Double>();
                        var minusFive = new List<Double>();
                        var plusMinusFive = new List<Double>();
                        try
                        {
                            plusone = (from r in tbldt1.AsEnumerable()
                                       where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                       select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusone = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusOne = (from r in tbldt1.AsEnumerable()
                                            where r.Field<Double>("WeightScale") < (specweight + specwOnePecent) && r.Field<Double>("WeightScale") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                            select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {

                            plusTwopointfive = (from r in tbldt1.AsEnumerable()
                                                where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                select r.Field<double>("WeightScale")).ToList();

                            //plusTwopointfive = (from r in tbldt1.AsEnumerable()
                            //                       where r.Field<Double>("WeightScale") > specweight && r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                            //                       select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                 where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                 select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                     where r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<Double>("WeightScale") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                     select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            plusfive = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            minusFive = (from r in tbldt1.AsEnumerable()
                                         where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                         select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {

                            plusMinusFive = (from r in tbldt1.AsEnumerable()
                                             where r.Field<Double>("WeightScale") < (specweight + specPlusfive) && r.Field<Double>("WeightScale") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                             select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }


                        float finalplusone = (float)(plusone.Count * 100) / count.Count;
                        float finalminusone = (float)(minusone.Count * 100) / count.Count;
                        float finalplusminusone = (float)(plusMinusOne.Count * 100) / count.Count;
                        float finalplustwo = (float)(plusTwopointfive.Count * 100) / count.Count;
                        float finalminustwo = (float)(minusTwoPointFive.Count * 100) / count.Count;
                        float finalplusminustwo = (float)(plusMinusTwoPointFive.Count * 100) / count.Count;
                        float finalplusfive = (float)(plusfive.Count * 100) / count.Count;
                        float finalminusfive = (float)(minusFive.Count * 100) / count.Count;
                        float finalplusminusfive = (float)(plusMinusFive.Count * 100) / count.Count;


                        finalplusone = (float)Math.Round(finalplusone, 2);
                        finalminusone = (float)Math.Round(finalminusone, 2);
                        finalplusminusone = (float)Math.Round(finalplusminusone, 2);
                        finalplustwo = (float)Math.Round(finalplustwo, 2);
                        finalminustwo = (float)Math.Round(finalminustwo, 2);
                        finalplusminustwo = (float)Math.Round(finalplusminustwo, 2);
                        finalplusfive = (float)Math.Round(finalplusfive, 2);
                        finalminusfive = (float)Math.Round(finalminusfive, 2);
                        finalplusminusfive = (float)Math.Round(finalplusminusfive, 2);

                        dr[5] = count.Count.ToString();
                        dr[6] = average;
                        dr[7] = plusone.Count;
                        dr[8] = finalplusone;

                        dr[9] = minusone.Count;
                        dr[10] = finalminusone;

                        dr[11] = plusMinusOne.Count;
                        dr[12] = finalplusminusone;
                        dr[13] = plusTwopointfive.Count;
                        dr[14] = finalplustwo;

                        dr[15] = minusTwoPointFive.Count;
                        dr[16] = finalminustwo;
                        dr[17] = plusMinusTwoPointFive.Count;
                        dr[18] = finalplusminustwo;
                        dr[19] = plusfive.Count;
                        dr[20] = finalplusfive;

                        dr[21] = minusFive.Count;
                        dr[22] = finalminusfive;
                        dr[23] = plusMinusFive.Count;
                        dr[24] = finalplusminusfive;
                        finaldt.Rows.Add(dr);
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    }
                }

                DataRow dr1 = finaldt.NewRow();

                dr1[0] = "Total Tyre";
                dr1[1] = "Total Tyre";
                dr1[2] = totalyire;
                dr1[3] = "";
                dr1[4] = "";
                dr1[5] = "";
                dr1[6] = "";
                dr1[7] = "";
                dr1[8] = "";
                dr1[9] = "";
                dr1[10] = "";
                dr1[11] = "";
                dr1[12] = "";
                dr1[13] = "";
                dr1[14] = "";
                dr1[15] = "";
                dr1[16] = "";
                dr1[17] = "";
                dr1[18] = "";
                dr1[19] = "";
                dr1[20] = "";
                dr1[21] = "";
                dr1[22] = "";
                dr1[23] = "";
                dr1[24] = "";



                DataTable dtExcel = new DataTable();

                finaldt.Rows.Add(dr1);
                //ViewState["dt"] = finaldt;

                MainGridView.DataSource = finaldt;
                MainGridView.DataBind();
                ReportMonthWisePanel.Visible = true;

                DataTable dtexcel = new DataTable();

                dtexcel.Columns.Add("TireType", typeof(string));
                dtexcel.Columns.Add("ProductType", typeof(string));
                dtexcel.Columns.Add("TotalTire", typeof(string));
                dtexcel.Columns.Add("Sapcode", typeof(string));
                dtexcel.Columns.Add("Spec Weight", typeof(string));
                dtexcel.Columns.Add("Qty", typeof(string));
                dtexcel.Columns.Add("Per.", typeof(string));
                dtexcel.Columns.Add("WithIn +1Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +1%", typeof(string));
                dtexcel.Columns.Add("WithIn-1Qty.", typeof(string));
                dtexcel.Columns.Add("WithIn -1%", typeof(string));
                dtexcel.Columns.Add("WithIn+/-1Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-1%", typeof(string));
                dtexcel.Columns.Add("WithIn <=2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn <=2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn -2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn -2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn +/-2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn +5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +5%", typeof(string));
                dtexcel.Columns.Add("WithIn -5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn -5%", typeof(string));
                dtexcel.Columns.Add("WithIn +/-5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-5%", typeof(string));

                dtExcel = finaldt.Copy();
                ViewState["dt"] = dtExcel;

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        //        protected void showReportDayRecipeWise(string fromDate, string toDate)
        //        {
        //            try
        //            {
        //                DataTable wcdt = new DataTable();
        //                DataTable gridviewdt = new DataTable();
        //                gridviewdt.Columns.Add("tireType", typeof(string));
        //                gridviewdt.Columns.Add("Product Name", typeof(string));

        //                for (int i = 7; i <= 23; i++)
        //                {
        //                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
        //                    if (i == 14)
        //                        gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
        //                    else if (i == 22)
        //                        gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
        //                }
        //                for (int i = 0; i <= 6; i++)
        //                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
        //                gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
        //                gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));

        //                // Get the Data based on WCName
        //                myConnection.open(ConnectionOption.SQL);
        //                myConnection.comm = myConnection.conn.CreateCommand();

        //                myConnection.comm.CommandText = "Select recipeCode,description, DateInserted from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY recipeCode, DateInserted";
        //                myConnection.reader = myConnection.comm.ExecuteReader();
        //                wcdt.Load(myConnection.reader);

        //                myConnection.reader.Close();
        //                myConnection.comm.Dispose();
        //                myConnection.close(ConnectionOption.SQL);

        //                var query = wcdt.AsEnumerable()
        //    .GroupBy(row => new
        //    {
        //        Hour = row.Field<DateTime>("DateInserted").Hour,
        //        recipeCode = row.Field<string>("recipeCode"),
        //        description = row.Field<string>("description")
        //    })
        //    .Select(g => new
        //    {
        //        Hour = g.Key.Hour,
        //        recipeCode = g.Key.recipeCode,
        //        description = g.Key.description,
        //        quantity = g.Count()
        //    }
        //        );
        //                DataRow dr = gridviewdt.NewRow();
        //                var items = query.ToArray();
        //                int total = 0;
        //                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
        //                {
        //                    if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
        //                    {
        //                        dr[0] = items[i].recipeCode.ToString();
        //                        if (items[i].description == null)
        //                            dr[1] = "";
        //                        else
        //                            dr[1] = items[i].description.ToString();

        //                        // Insert rows in all the hours places by default
        //                        for (int h = 2; h <= 29; h++)
        //                        {
        //                            dr[h] = 0;
        //                        }
        //                    }
        //                    else if (items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) //If Recipe or Workcenter changes, then create new data row
        //                    {
        //                        dr = gridviewdt.NewRow();
        //                        dr[0] = items[i].recipeCode.ToString();
        //                        if (items[i].description == null)
        //                            dr[1] = "";
        //                        else
        //                            dr[1] = items[i].description.ToString();

        //                        // Insert rows in all the hours places by default
        //                        for (int h = 2; h <= 29; h++)
        //                        {
        //                            dr[h] = 0;
        //                        }
        //                    }
        //                    int getHour = items[i].Hour; //Store current array hour
        //                    int getDifference = 0; //Store time difference of current to previous day
        //                    if (i > 0)
        //                    {
        //                        if (items[i].Hour > items[i - 1].Hour) //Check if current hour is greater than previous hour
        //                            getDifference = items[i].Hour - items[i - 1].Hour;
        //                        else //If current hour is smaller than previous hour in case of new day begins which starts at 00:00
        //                            getDifference = (items[i].Hour + 23) - items[i - 1].Hour;
        //                    }

        //                    dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

        //                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
        //                    {
        //                        total = 0;
        //                        // Display Shift Total
        //                        for (int v = 2; v <= 28; v++)
        //                        {
        //                            if (v >= 2 && v <= 9) //Calculate A shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 10 && v < 19) //Calculate B shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 19 && v < 28) //Calculate C shift values
        //                                total += Convert.ToInt32(dr[v]);

        //                            if (v == 10) //if all A shift values processed, then add total of A shift
        //                            {
        //                                dr[10] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 19) //if all B shift values processed, then add total of A shift
        //                            {
        //                                dr[19] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 28) //if all C shift values processed, then add total of A shift
        //                            {
        //                                dr[28] = total;
        //                                total = 0;

        //                                //Set Day Total
        //                                dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
        //                            }

        //                        }
        //                        gridviewdt.Rows.Add(dr);
        //                    }
        //                    else if (items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
        //                    {
        //                        total = 0;
        //                        // Display Shift Total
        //                        for (int v = 2; v <= 28; v++)
        //                        {
        //                            if (v >= 2 && v <= 9) //Calculate A shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 10 && v < 19) //Calculate B shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 19 && v < 28) //Calculate C shift values
        //                                total += Convert.ToInt32(dr[v]);

        //                            if (v == 10) //if all A shift values processed, then add total of A shift
        //                            {
        //                                dr[10] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 19) //if all B shift values processed, then add total of A shift
        //                            {
        //                                dr[19] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 28) //if all C shift values processed, then add total of A shift
        //                            {
        //                                dr[28] = total;
        //                                total = 0;

        //                                //Set Day Total
        //                                dr[29] = Convert.ToInt32(dr[10]) + Convert.ToInt32(dr[19]) + Convert.ToInt32(dr[28]);
        //                            }

        //                        }
        //                        gridviewdt.Rows.Add(dr);
        //                    }
        //                }

        //                MainGridView.Columns.Clear();
        //                //Iterate through the columns of the datatable to set the data bound field dynamically.
        //                foreach (DataColumn col in gridviewdt.Columns)
        //                {
        //                    //Declare the bound field and allocate memory for the bound field.
        //                    BoundField bfield = new BoundField();

        //                    //Initalize the DataField value.
        //                    bfield.DataField = col.ColumnName;

        //                    //Initialize the HeaderText field value.
        //                    bfield.HeaderText = col.ColumnName;

        //                    //Add the newly created bound field to the GridView.
        //                    MainGridView.Columns.Add(bfield);

        //                }

        //                // Calculate total of all workstations of every hour
        //                dr = gridviewdt.NewRow();
        //                dr[0] = "Total";
        //                for (int v = 2; v <= 29; v++)
        //                {
        //                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

        //                }
        //                gridviewdt.Rows.Add(dr);

        //                dr = gridviewdt.NewRow();
        //                gridviewdt.Rows.Add(dr);

        //                MainGridView.DataSource = gridviewdt;
        //                MainGridView.DataBind();

        //                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
        //.Where(row => row.Cells[0].Text == "Total" || row.Cells[0].Text == "GrandTotal");

        //                foreach (var row in rows)
        //                    row.Font.Bold = true;

        //                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 2).CopyToDataTable();

        //                DataTable newdt = new DataTable();
        //                newdt.Columns.Add("columnName", typeof(string));
        //                newdt.Columns.Add("hours", typeof(string));
        //                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
        //                {
        //                    if (gridviewdt.Columns[2 + i].ColumnName != "TotalA" && gridviewdt.Columns[2 + i].ColumnName != "TotalB" && gridviewdt.Columns[2 + i].ColumnName != "TotalC" && gridviewdt.Columns[2 + i].ColumnName != "DayTotal" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
        //                    {
        //                        DataRow drow = newdt.NewRow();
        //                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
        //                        drow[1] = gridviewdt.Rows[0][2 + i];
        //                        newdt.Rows.Add(drow);
        //                    }
        //                }

        //                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column; //SeriesChartType.Column;
        //                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
        //                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
        //                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

        //                TBMChart.DataSource = newdt;
        //                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
        //                TBMChart.Series["TBMSeries"].YValueMembers = "hours";
        //                TBMChart.DataBind();
        //            }
        //            catch (Exception exp)
        //            {
        //                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //            }
        //        }
        //        protected void showReportDayOperatorWise(string fromDate, string toDate)
        //        {
        //            try
        //            {
        //                string sqlquery = (getOperator != "All") ? " AND manningID=" + getOperator : "";

        //                DataTable wcdt = new DataTable();
        //                DataTable manningdt = new DataTable();
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("operator", typeof(string));
        //                dt.Columns.Add("DateInserted", typeof(DateTime));

        //                DataTable gridviewdt = new DataTable();
        //                gridviewdt.Columns.Add("operator", typeof(string));

        //                for (int i = 7; i <= 23; i++)
        //                {
        //                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
        //                    if (i == 14)
        //                        gridviewdt.Columns.Add("TotalA".ToString(), typeof(int));
        //                    else if (i == 22)
        //                        gridviewdt.Columns.Add("TotalB".ToString(), typeof(int));
        //                }
        //                for (int i = 0; i <= 6; i++)
        //                    gridviewdt.Columns.Add(i.ToString(), typeof(int));
        //                gridviewdt.Columns.Add("TotalC".ToString(), typeof(int));
        //                gridviewdt.Columns.Add("DayTotal".ToString(), typeof(int));

        //                // Get the Data based on WCName
        //                myConnection.open(ConnectionOption.SQL);
        //                myConnection.comm = myConnection.conn.CreateCommand();

        //                myConnection.comm.CommandText = "Select manningID, firstName, lastName, DateInserted from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + processType + " AND " + durationQuery + " " + sqlquery + " ORDER BY manningID, DateInserted";
        //                myConnection.reader = myConnection.comm.ExecuteReader();
        //                wcdt.Load(myConnection.reader);

        //                myConnection.reader.Close();
        //                myConnection.comm.Dispose();
        //                myConnection.close(ConnectionOption.SQL);

        //                var query = wcdt.AsEnumerable()
        //    .GroupBy(row => new
        //    {
        //        Hour = row.Field<DateTime>("DateInserted").Hour,
        //        manningID = row.Field<int>("manningID"),
        //        firstName = row.Field<string>("firstName"),
        //        lastName = row.Field<string>("lastName")
        //    })
        //    .Select(g => new
        //    {
        //        Hour = g.Key.Hour,
        //        manningID = g.Key.manningID,
        //        firstName = g.Key.firstName,
        //        lastName = g.Key.lastName,
        //        quantity = g.Count()
        //    }
        //        );
        //                DataRow dr = gridviewdt.NewRow();
        //                var items = query.ToArray();
        //                int total = 0;
        //                for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
        //                {
        //                    if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
        //                    {
        //                        dr[0] = items[i].firstName.ToString() + " " + items[i].lastName.ToString();

        //                        // Insert rows in all the hours places by default
        //                        for (int h = 1; h <= 27; h++)
        //                        {
        //                            dr[h] = 0;
        //                        }
        //                    }
        //                    else if (items[i - 1].manningID.ToString() != items[i].manningID.ToString()) //If Recipe or Workcenter changes, then create new data row
        //                    {
        //                        dr = gridviewdt.NewRow();
        //                        dr[0] = items[i].firstName.ToString() + " " + items[i].lastName.ToString();

        //                        // Insert rows in all the hours places by default
        //                        for (int h = 1; h <= 27; h++)
        //                        {
        //                            dr[h] = 0;
        //                        }
        //                    }

        //                    dr[items[i].Hour.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

        //                    if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
        //                    {
        //                        total = 0;
        //                        // Display Shift Total
        //                        for (int v = 1; v <= 27; v++)
        //                        {
        //                            if (v >= 1 && v <= 8) //Calculate A shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 9 && v < 18) //Calculate B shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 18 && v < 27) //Calculate C shift values
        //                                total += Convert.ToInt32(dr[v]);

        //                            if (v == 9) //if all A shift values processed, then add total of A shift
        //                            {
        //                                dr[9] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 18) //if all B shift values processed, then add total of A shift
        //                            {
        //                                dr[18] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 27) //if all C shift values processed, then add total of A shift
        //                            {
        //                                dr[27] = total;
        //                                total = 0;

        //                                //Set Day Total
        //                                dr[28] = Convert.ToInt32(dr[9]) + Convert.ToInt32(dr[18]) + Convert.ToInt32(dr[27]);
        //                            }

        //                        }
        //                        gridviewdt.Rows.Add(dr);
        //                    }
        //                    else if (items[i].manningID.ToString() != items[i + 1].manningID.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
        //                    {
        //                        total = 0;
        //                        // Display Shift Total
        //                        for (int v = 1; v <= 27; v++)
        //                        {
        //                            if (v >= 1 && v <= 8) //Calculate A shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 9 && v < 18) //Calculate B shift values
        //                                total += Convert.ToInt32(dr[v]);
        //                            else if (v > 18 && v < 27) //Calculate C shift values
        //                                total += Convert.ToInt32(dr[v]);

        //                            if (v == 9) //if all A shift values processed, then add total of A shift
        //                            {
        //                                dr[9] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 18) //if all B shift values processed, then add total of A shift
        //                            {
        //                                dr[18] = total;
        //                                total = 0;
        //                            }
        //                            else if (v == 27) //if all C shift values processed, then add total of A shift
        //                            {
        //                                dr[27] = total;
        //                                total = 0;

        //                                //Set Day Total
        //                                dr[28] = Convert.ToInt32(dr[9]) + Convert.ToInt32(dr[18]) + Convert.ToInt32(dr[27]);
        //                            }

        //                        }
        //                        gridviewdt.Rows.Add(dr);
        //                    }
        //                }

        //                MainGridView.Columns.Clear();
        //                //Iterate through the columns of the datatable to set the data bound field dynamically.
        //                foreach (DataColumn col in gridviewdt.Columns)
        //                {
        //                    //Declare the bound field and allocate memory for the bound field.
        //                    BoundField bfield = new BoundField();

        //                    //Initalize the DataField value.
        //                    bfield.DataField = col.ColumnName;

        //                    //Initialize the HeaderText field value.
        //                    bfield.HeaderText = col.ColumnName;

        //                    //Add the newly created bound field to the GridView.
        //                    MainGridView.Columns.Add(bfield);

        //                }

        //                // Calculate total of all workstations of every hour

        //                dr = gridviewdt.NewRow();
        //                dr[0] = "Total";
        //                for (int v = 1; v <= 28; v++)
        //                {
        //                    dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

        //                }
        //                gridviewdt.Rows.Add(dr);

        //                dr = gridviewdt.NewRow();
        //                gridviewdt.Rows.Add(dr);

        //                MainGridView.DataSource = gridviewdt;
        //                MainGridView.DataBind();


        //                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
        //.Where(row => row.Cells[0].Text == "Total" || row.Cells[0].Text == "GrandTotal");

        //                foreach (var row in rows)
        //                    row.Font.Bold = true;

        //                gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 2).CopyToDataTable();

        //                DataTable newdt = new DataTable();
        //                newdt.Columns.Add("columnName", typeof(string));
        //                newdt.Columns.Add("hours", typeof(string));
        //                for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
        //                {
        //                    if (gridviewdt.Columns[2 + i].ColumnName != "TotalA" && gridviewdt.Columns[2 + i].ColumnName != "TotalB" && gridviewdt.Columns[2 + i].ColumnName != "TotalC" && gridviewdt.Columns[2 + i].ColumnName != "DayTotal" && gridviewdt.Columns[2 + i].ColumnName != "wcName" && gridviewdt.Columns[2 + i].ColumnName != "tireType")
        //                    {
        //                        DataRow drow = newdt.NewRow();
        //                        drow[0] = gridviewdt.Columns[2 + i].ColumnName;
        //                        drow[1] = gridviewdt.Rows[0][2 + i];
        //                        newdt.Rows.Add(drow);
        //                    }
        //                }

        //                TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
        //                TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
        //                TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
        //                TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

        //                TBMChart.DataSource = newdt;
        //                TBMChart.Series["TBMSeries"].XValueMember = "columnName";
        //                TBMChart.Series["TBMSeries"].YValueMembers = "hours";
        //                TBMChart.DataBind();
        //            }
        //            catch (Exception exp)
        //            {
        //                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //            }
        //        }
        #endregion
        #region MonthWise Report
        protected void showReportMonthWcWise(int getMonth, int getYear, string process)
        {
            try
            {
                string rtoDate = "";
                string rfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                if (getMonth == 12)
                    rtoDate = (getYear + 1).ToString() + "-01-01 07:00:00";
                else
                    rtoDate = getYear.ToString() + "-" + (getMonth + 1) + "-01 07:00:00";
                string tablename = "";
                DataTable tbldt = new DataTable();
                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("TireType", typeof(string));
                gridviewdt.Columns.Add("Product Type", typeof(string));
                gridviewdt.Columns.Add("TotalTire", typeof(string));
                gridviewdt.Columns.Add("Sapcode", typeof(string));
                gridviewdt.Columns.Add("Spec Weight", typeof(string));

                gridviewdt.Columns.Add("Avg Weight", typeof(string));
                gridviewdt.Columns.Add("WithIn +1%", typeof(string));
                gridviewdt.Columns.Add("WithIn -1%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-1%", typeof(string));
                gridviewdt.Columns.Add("WithIn +2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn -2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-2.5%", typeof(string));
                gridviewdt.Columns.Add("WithIn +5%", typeof(string));
                gridviewdt.Columns.Add("WithIn -5%", typeof(string));
                gridviewdt.Columns.Add("WithIn +/-5%", typeof(string));



                DataTable finaldt = new DataTable();
                finaldt.Columns.Add("TireType", typeof(string));
                finaldt.Columns.Add("ProductType", typeof(string));
                finaldt.Columns.Add("TotalTire", typeof(string));
                finaldt.Columns.Add("Sapcode", typeof(string));
                finaldt.Columns.Add("Spec Weight", typeof(string));

                finaldt.Columns.Add("Qty", typeof(string));
                finaldt.Columns.Add("Per.", typeof(string));
                finaldt.Columns.Add("1Qty", typeof(string));
                finaldt.Columns.Add("1%", typeof(string));
                finaldt.Columns.Add("-1Qty.", typeof(string));
                finaldt.Columns.Add("-1%", typeof(string));
                finaldt.Columns.Add("+/-1Qty", typeof(string));
                finaldt.Columns.Add(" +/-1%", typeof(string));
                finaldt.Columns.Add("<=2.5Qty", typeof(string));
                finaldt.Columns.Add("<=2.5%", typeof(string));
                finaldt.Columns.Add("-2.5Qty", typeof(string));
                finaldt.Columns.Add("-2.5%", typeof(string));
                finaldt.Columns.Add("+/-2.5Qty", typeof(string));
                finaldt.Columns.Add("+/-2.5%", typeof(string));
                finaldt.Columns.Add("+5Qty", typeof(string));
                finaldt.Columns.Add("+5%", typeof(string));
                finaldt.Columns.Add("-5Qty", typeof(string));
                finaldt.Columns.Add("-5%", typeof(string));
                finaldt.Columns.Add("+/-5Qty", typeof(string));
                finaldt.Columns.Add("+/-5%", typeof(string));
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

              if (process == "PCR")
                    tablename = "vGTweighing";

                if (recipeCode == "All")
                {
                    
                        myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode,SAPMaterialCode as Sapcode from vGTweighing t1 where  t1.DateInserted = (SELECT MAX(t2.DateInserted)FROM vGTweighing t2 WHERE t2.gtbarCode = t1.gtbarCode and DateInserted>= '" + rfromDate + " ' AND DateInserted < '" + rtoDate + "')   and DateInserted >= '" + rfromDate + " ' AND DateInserted < '" + rtoDate + "'";
                    

                    // myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode from "+tablename+" WHERE  " + durationQuery + "";
                }
               
                 else
                 {
                       myConnection.comm.CommandText = "Select recipeCode,WeightScale,DateInserted,gtbarcode,SAPMaterialCode as Sapcode from vGTweighing t1 where  t1.DateInserted = (SELECT MAX(t2.DateInserted)FROM vGTweighing t2 WHERE t2.gtbarCode = t1.gtbarCode and DateInserted>= '" + rfromDate + "' AND DateInserted < '" + rtoDate + "' and recipeCode='" + recipeCode + "' ) and recipeCode='" + recipeCode + "' and DateInserted >= '" + rfromDate + "' AND DateInserted < '" + rtoDate + "' ";
                    
                }
                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                string tempweight = "";
                for (int i = 0; i < tbldt.Rows.Count; i++)
                {
                    String[] WeightScale = tbldt.Rows[i][1].ToString().Split('_');
                    tempweight = "";

                    for (int j = 0; j < WeightScale.Length; j++)
                    {
                        if (WeightScale[j].Length > 2)
                        {
                            tempweight = WeightScale[j].ToString();
                        }
                    }


                    if (tempweight.Length < 2)
                    {
                        tbldt.Rows[i].Delete();
                        // tbldt.Rows[i][1] = 0.0;
                    }
                    else
                        tbldt.Rows[i][1] = tempweight;

                }
                tbldt.AcceptChanges();


                DataTable tbldt1 = new DataTable();
                tbldt1.Columns.Add("recipeCode", typeof(string));
                tbldt1.Columns.Add("WeightScale", typeof(Double));
                tbldt1.Columns.Add("dtandTime", typeof(DateTime));

                tbldt1.Load(tbldt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);


                for (int i = tbldt1.Rows.Count - 1; i >= 0; i--)
                {
                    if (tbldt1.Rows[i][0] == DBNull.Value)
                        tbldt1.Rows[i].Delete();
                }
                tbldt1.AcceptChanges();


                var distinctvalues = tbldt1.AsEnumerable()
                   .Select(s => new
                   {
                       recipeCode = s.Field<string>("recipeCode"),
                       Sapcode = s.Field<string>("Sapcode")
                   })
                   .Distinct().ToList();
                int totalyire = 0;

                for (int i = 0; i < distinctvalues.Count; i++)
                {
                    try
                    {
                        DataRow dr = finaldt.NewRow();

                        //   var count = tbldt1.AsEnumerable()
                        //  .Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode)
                        //  .GroupBy(g => new { barcode = g.Field<string>("gtbarcode"), })
                        //.Select(row => new
                        //{
                        //    date1 = row.Max(T => T.Field<DateTime>("DateInserted")),
                        //    //WeightScale = row.Key.weight,
                        //})
                        //.ToList();

                        var count = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Select(row => row.Field<Double>("WeightScale")).ToList();
                        Double average = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Average(row => row.Field<Double>("WeightScale"));
                        average = Math.Round(average, 2);
                        dr[0] = distinctvalues[i].recipeCode;

                        var description = (from r in recipedt.AsEnumerable()
                                           where r.Field<string>("name").Equals(distinctvalues[i].recipeCode)
                                           select r).ToList();

                        if (description.Count > 0)
                            dr[1] = description[0][2].ToString();
                        else
                            dr[1] = "";

                        var aa = (from r in recipedt.AsEnumerable() where r.Field<string>("name").Equals(distinctvalues[i].recipeCode) select r.Field<double>("specweight")).ToList();
                        Double specweight = aa[0];
                        Double specwOnePecent = (specweight * 1) / 100;
                        //Double specminusOne = (specweight * -1) / 100;
                        Double specPlustwopintfive = (specweight * 2.5) / 100;
                        //  Double specMinuswopintfive = (specweight * -2.5) / 100;
                        Double specPlusfive = (specweight * 5) / 100;


                        specwOnePecent = Math.Round(specwOnePecent, 2);
                        specPlustwopintfive = Math.Round(specPlustwopintfive, 2);
                        specPlusfive = Math.Round(specPlusfive, 2);
                        totalyire = totalyire + count.Count;
                        dr[2] = count.Count.ToString();
                        dr[3] = distinctvalues[i].Sapcode;
                        dr[4] = aa[0].ToString();
                        var plusone = new List<Double>();
                        var minusone = new List<Double>();
                        var plusMinusOne = new List<Double>();
                        var plusTwopointfive = new List<Double>();
                        var minusTwoPointFive = new List<Double>();
                        var plusMinusTwoPointFive = new List<Double>();
                        var plusfive = new List<Double>();
                        var minusFive = new List<Double>();
                        var plusMinusFive = new List<Double>();
                        try
                        {
                            plusone = (from r in tbldt1.AsEnumerable()
                                       where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                       select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusone = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusOne = (from r in tbldt1.AsEnumerable()
                                            where r.Field<Double>("WeightScale") < (specweight + specwOnePecent) && r.Field<Double>("WeightScale") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                            select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {

                            plusTwopointfive = (from r in tbldt1.AsEnumerable()
                                                where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                select r.Field<double>("WeightScale")).ToList();

                            //plusTwopointfive = (from r in tbldt1.AsEnumerable()
                            //                       where r.Field<Double>("WeightScale") > specweight && r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                            //                       select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                 where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                 select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                     where r.Field<Double>("WeightScale") < (specweight + specPlustwopintfive) && r.Field<Double>("WeightScale") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                     select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            plusfive = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("WeightScale") >= specweight && r.Field<Double>("WeightScale") < (specweight + specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            minusFive = (from r in tbldt1.AsEnumerable()
                                         where r.Field<Double>("WeightScale") < specweight && r.Field<Double>("WeightScale") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                         select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {

                            plusMinusFive = (from r in tbldt1.AsEnumerable()
                                             where r.Field<Double>("WeightScale") < (specweight + specPlusfive) && r.Field<Double>("WeightScale") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                             select r.Field<double>("WeightScale")).ToList();
                        }
                        catch (Exception ex)
                        { }


                        float finalplusone = (float)(plusone.Count * 100) / count.Count;
                        float finalminusone = (float)(minusone.Count * 100) / count.Count;
                        float finalplusminusone = (float)(plusMinusOne.Count * 100) / count.Count;
                        float finalplustwo = (float)(plusTwopointfive.Count * 100) / count.Count;
                        float finalminustwo = (float)(minusTwoPointFive.Count * 100) / count.Count;
                        float finalplusminustwo = (float)(plusMinusTwoPointFive.Count * 100) / count.Count;
                        float finalplusfive = (float)(plusfive.Count * 100) / count.Count;
                        float finalminusfive = (float)(minusFive.Count * 100) / count.Count;
                        float finalplusminusfive = (float)(plusMinusFive.Count * 100) / count.Count;


                        finalplusone = (float)Math.Round(finalplusone, 2);
                        finalminusone = (float)Math.Round(finalminusone, 2);
                        finalplusminusone = (float)Math.Round(finalplusminusone, 2);
                        finalplustwo = (float)Math.Round(finalplustwo, 2);
                        finalminustwo = (float)Math.Round(finalminustwo, 2);
                        finalplusminustwo = (float)Math.Round(finalplusminustwo, 2);
                        finalplusfive = (float)Math.Round(finalplusfive, 2);
                        finalminusfive = (float)Math.Round(finalminusfive, 2);
                        finalplusminusfive = (float)Math.Round(finalplusminusfive, 2);

                        dr[5] = count.Count.ToString();
                        dr[6] = average;
                        dr[7] = plusone.Count;
                        dr[8] = finalplusone;

                        dr[9] = minusone.Count;
                        dr[10] = finalminusone;

                        dr[11] = plusMinusOne.Count;
                        dr[12] = finalplusminusone;
                        dr[13] = plusTwopointfive.Count;
                        dr[14] = finalplustwo;

                        dr[15] = minusTwoPointFive.Count;
                        dr[16] = finalminustwo;
                        dr[17] = plusMinusTwoPointFive.Count;
                        dr[18] = finalplusminustwo;
                        dr[19] = plusfive.Count;
                        dr[20] = finalplusfive;

                        dr[21] = minusFive.Count;
                        dr[22] = finalminusfive;
                        dr[23] = plusMinusFive.Count;
                        dr[24] = finalplusminusfive;
                        finaldt.Rows.Add(dr);
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    }
                }

                DataRow dr1 = finaldt.NewRow();

                dr1[0] = "Total Tyre";
                dr1[1] = "Total Tyre";
                dr1[2] = totalyire;
                dr1[3] = "";
                dr1[4] = "";
                dr1[5] = "";
                dr1[6] = "";
                dr1[7] = "";
                dr1[8] = "";
                dr1[9] = "";
                dr1[10] = "";
                dr1[11] = "";
                dr1[12] = "";
                dr1[13] = "";
                dr1[14] = "";
                dr1[15] = "";
                dr1[16] = "";
                dr1[17] = "";
                dr1[18] = "";
                dr1[19] = "";
                dr1[20] = "";
                dr1[21] = "";
                dr1[22] = "";
                dr1[23] = "";
                dr1[24] = "";


                finaldt.Rows.Add(dr1);
                // ViewState["dt"] = finaldt;

                MainGridView.DataSource = finaldt;
                MainGridView.DataBind();

                DataTable dtexcel = new DataTable();

                dtexcel.Columns.Add("TireType", typeof(string));
                dtexcel.Columns.Add("ProductType", typeof(string));
                dtexcel.Columns.Add("TotalTire", typeof(string));
                dtexcel.Columns.Add("Sapcode", typeof(string));
                dtexcel.Columns.Add("Spec Weight", typeof(string));
                dtexcel.Columns.Add("Qty", typeof(string));
                dtexcel.Columns.Add("Per.", typeof(string));
                dtexcel.Columns.Add("WithIn +1Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +1%", typeof(string));
                dtexcel.Columns.Add("WithIn-1Qty.", typeof(string));
                dtexcel.Columns.Add("WithIn -1%", typeof(string));
                dtexcel.Columns.Add("WithIn+/-1Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-1%", typeof(string));
                dtexcel.Columns.Add("WithIn <=2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn <=2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn -2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn -2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn +/-2.5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-2.5%", typeof(string));
                dtexcel.Columns.Add("WithIn +5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +5%", typeof(string));
                dtexcel.Columns.Add("WithIn -5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn -5%", typeof(string));
                dtexcel.Columns.Add("WithIn +/-5Qty", typeof(string));
                dtexcel.Columns.Add("WithIn +/-5%", typeof(string));

                dtexcel = finaldt.Copy();
                ViewState["dt"] = dtexcel
                    ;

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        #endregion
        #region YearWise Report
      
        #endregion
        #region User Defined Function
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        protected void OnDataBound(object sender, EventArgs e)
        {


            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();

            switch (duration)
            {
                case "Date":
                    cell.Text = "";
                    cell.ColumnSpan = 5;
                    row.Controls.Add(cell);
                    cell = new TableHeaderCell();
                    cell.Text = "Avg Weight";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.Text = "WithIn <= +1";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.Text = "WithIn -1";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-1";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn<= +2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn -2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn<= +5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn -5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-5";
                    row.Controls.Add(cell);
                    break;

                case "Month":
                    cell.Text = "";
                    cell.ColumnSpan = 5;
                    row.Controls.Add(cell);
                    cell = new TableHeaderCell();
                    cell.Text = "Avg Weight";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);
                    cell = new TableHeaderCell();
                    cell.Text = "WithIn <= +1";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.Text = "WithIn -1";
                    cell.ColumnSpan = 2;
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-1";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn<= +2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn -2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-2.5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn<= +5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn -5";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.ColumnSpan = 2;
                    cell.Text = "WithIn +/-5";
                    row.Controls.Add(cell);
                    break;




            }
            MainGridView.HeaderRow.Parent.Controls.AddAt(0, row);




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
        public void getRecipeSpec()
        {


            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select name, specweight,description from recipemaster where specweight is not null ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                recipedt.Load(myConnection.reader);
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);



        }



        #endregion


    }
}
