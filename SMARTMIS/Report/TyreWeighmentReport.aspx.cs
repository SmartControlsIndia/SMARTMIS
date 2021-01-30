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
    public partial class TyreWeighmentReport : System.Web.UI.Page
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

           

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Create an event handler for the master page's contentCallEvent event
            Master.contentCallEvent += new EventHandler(Master1_ButtonClick);
            Master.contentCallEvent1 += new EventHandler(ExportToExcel_ButtonClick);


        }
        private void ExportToExcel_ButtonClick(object sender, EventArgs e)
        {
          //  DataTable dt = 
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=WeighmentReport.xls");
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
        private void Master1_ButtonClick(object sender, EventArgs e)
        {
            // Get all the inputs from the user
            string getProcess = Master.reportMasterWCProcessDropDownListValue;

            recipeCode = Master.DropDownListTypeValue;
            duration = Master.DropDownListDurationValue;

            //getOperator = Master.DropDownListOperatorsValue;

            string getDate = Master.reportMasterFromDateTextBoxValue;
            string getMonth = Master.DropDownListMonthValue;
            string getYear = Master.DropDownListYearValue;
            string getYearwise = Master.DropDownListYearWiseValue;

            getRecipeSpec();
          
                // Redirect the flow according to the duration selected by the user
                switch (duration)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getDate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = fromDate.ToString("MM-dd-yyyy");
                        string ntoDate = toDate.ToString("MM-dd-yyyy");

                        showReportDateWise(nfromDate, ntoDate, recipeCode,getProcess);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>Tyre Weighment Report</strong></td><td width=12% align=right>Date : " + getDate + "</td><td width=16% align=right> Type : " + recipeCode.ToString() + "</td></tr></table></div>";
                        break;
                    case "Month":
                        showReportMonthWise(Convert.ToInt32(getMonth), Convert.ToInt32(getYear), recipeCode,getProcess);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=60%><strong>Tyre Weighment Report</strong></td><td width=14% align=right>Month : " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(getMonth)) + " " + getYear + "</td><td width=16% align=right> Type : " + recipeCode.ToString() +"</td></tr></table></div>";

                        break;
                    case "Year":
                        //showReportYearWise(Convert.ToInt32(getYearwise), recipeCode);
                        HeaderText.Text = "<div width=30% class=\"infobox\"><table width=100%><tr><td width=11%>" + getProcess.ToString() + "</td><td width=63%><strong>Tyre Weighment Report</strong></td><td width=12% align=right>Year : " + getYearwise + "</td><td width=16% align=right> Type : " + recipeCode.ToString() + "</td></tr></table></div>";
                        break;
                    case "Select":
                        ErrorMsg.Visible = true;
                        ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>Select the duration!!!</font></strong></td></tr></table>";
                        ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);
                        break;
                    default:
                        ErrorMsg.Visible = true;
                        ErrorMsg.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=70%><strong> <font color=#9F6000>Not the valid selection!!!</font></strong></td></tr></table>";
                        ScriptManager.RegisterClientScriptBlock(ErrorMsg, this.GetType(), "myScript", "javascript:closePopup();", true);

                        break;
                }
                    
        }
        protected void showReportDateWise(string fromDate, string toDate, string type, string process)
        {
            // Validate the user input with proper message
            if (validateInput("date", type, fromDate, toDate, 0, 0, 0))
            {     
               showReportDayWcWise(fromDate, toDate, type,process);
            }
        }
        protected void showReportMonthWise(int getMonth, int getYear, string type,string process)
        {
            if (validateInput("month", type, "", "", getMonth, getYear, 0))
            {
                
                        showReportMonthWcWise(getMonth, getYear,process);
                      
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
                        durationQuery += "(dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
                        break;
                    case "month":
                        string rtoDate = "";
                        string rfromDate = year.ToString() + "-" + month + "-01 07:00:00";
                        if (month == 12)
                            rtoDate = (year + 1).ToString() + "-01-01 07:00:00";
                        else
                            rtoDate = year.ToString() + "-" + (month + 1) + "-01 07:00:00";
                        durationQuery += "(dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
                        break;
                    case "year":
                        string nfromDate = yearwise.ToString() + "-01-01 07:00:00";
                        string ntoDate = (yearwise + 1).ToString() + "-01-01 07:00:00";
                        durationQuery += "(dtandTime >= '" + nfromDate + "' AND dtandTime < '" + ntoDate + "')";
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
        protected void showReportDayWcWise(string fromDate, string toDate, string recipecode,string process)
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

                gridviewdt.Columns.Add("Curing Recipe", typeof(string));
                gridviewdt.Columns.Add("Curing Sapcode", typeof(string));


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

                finaldt.Columns.Add("Curing Recipe", typeof(string));
                finaldt.Columns.Add("Curing Sapcode", typeof(string));


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

                if (process == "TBR")
                    tablename = "VBuddeScannedTyreDetail";
                else if (process == "PCR")
                    tablename = "vPCRBuddeScannedTyreDetail";

                if (recipeCode == "All")
                {
                    if (process == "TBR")
                    {
                       // myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from VBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and weight!='')  and (dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
                        myConnection.comm.CommandText = " Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from VBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details CRD on t1.gtbarcode=CRD.gtbarcode  where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and weight!='')  and (dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
                
                    }
                    else
                    {
                      //  myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from vPCRBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')   and dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00'";
                        myConnection.comm.CommandText = " Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from vPCRBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details1 CRD on t1.gtbarcode=CRD.gtbarcode  where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and weight!='')  and (dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00')";
              
                    }

                    // myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode from "+tablename+" WHERE  " + durationQuery + "";
                }
                else
                {

                    if (process == "TBR")
                    {
                      //  myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from VBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and recipeCode='" + recipeCode + "' and weight!='')  and (dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00') and recipeCode='" + recipeCode + "'";
                        myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from VBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details CRD on t1.gtbarcode=CRD.gtbarcode where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and recipeCode='" + recipeCode + "' and weight!='')  and (dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00') and recipeCode='" + recipeCode + "'";
                   
                    }
                    else
                    {
                     //   myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from vPCRBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and recipeCode='" + recipeCode + "' ) and recipeCode='" + recipeCode + "' and dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' ";
                        myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from vPCRBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details1 CRD on t1.gtbarcode=CRD.gtbarcode where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' and recipeCode='" + recipeCode + "' ) and recipeCode='" + recipeCode + "' and dtandTime >= '" + fromDate + " 07:00:00' AND dtandTime < '" + toDate + " 07:00:00' ";
                 
                    }
                }
                    //myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode from " + tablename + "WHERE recipeCode='" + recipeCode + "'  AND " + durationQuery + "";
                    //myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode from " + tablename + " WHERE recipeCode='" + recipeCode + "'  AND " + durationQuery + "";

                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                string tempweight = "";
                
                for (int i = 0; i < tbldt.Rows.Count; i++)
                {
                    String[] weight = tbldt.Rows[i][1].ToString().Split('_');
                    tempweight = "";

                    for (int j = 0; j < weight.Length; j++)
                    {
                        if (weight[j].Length > 2)
                        {
                            tempweight = weight[j].ToString();
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
                tbldt1.Columns.Add("weight", typeof(Double));
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
                       SapCode = s.Field<string>("SapCode"),
                       Curing_Recipe = s.Field<string>("Curing_Recipe"),
                       Curing_Sapcode = s.Field<string>("Curing_Sapcode")

                   })
                   .Distinct().ToList();
                int totalyire=0;

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
                       //    date1 = row.Max(T => T.Field<DateTime>("dtandtime")),
                       //    //weight = row.Key.weight,
                       //})
                       //.ToList();

                        var count = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Select(row => row.Field<Double>("weight")).ToList();
                        Double average = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Average(row => row.Field<Double>("weight"));
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
                        dr[4] = distinctvalues[i].Curing_Recipe;
                        dr[5] = distinctvalues[i].Curing_Sapcode;
                        dr[6] = aa[0].ToString();

                        var plusone = new List<Double>();
                        var minusone= new List<Double>();
                        var plusMinusOne= new List<Double>();
                        var plusTwopointfive= new List<Double>();
                        var minusTwoPointFive= new List<Double>();
                        var plusMinusTwoPointFive = new List<Double>();
                        var plusfive = new List<Double>();
                        var minusFive = new List<Double>();
                        var plusMinusFive = new List<Double>();
                        try
                        {
                             plusone = (from r in tbldt1.AsEnumerable()
                                           where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                           select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        try
                        {
                             minusone = (from r in tbldt1.AsEnumerable()
                                         where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                            select r.Field<double>("weight")).ToList();
                        }
                        catch(Exception ex)
                        {
                        
                        }
                            try
                            {

                         plusMinusOne = (from r in tbldt1.AsEnumerable()
                                         where r.Field<Double>("weight") < (specweight + specwOnePecent) && r.Field<Double>("weight") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                            select r.Field<double>("weight")).ToList();
                            }
                            catch(Exception ex)
                            {
                            
                            
                            }
                                try
                                {

                                    plusTwopointfive = (from r in tbldt1.AsEnumerable()
                                                        where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                               select r.Field<double>("weight")).ToList();

                         //plusTwopointfive = (from r in tbldt1.AsEnumerable()
                         //                       where r.Field<Double>("weight") > specweight && r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                         //                       select r.Field<double>("weight")).ToList();
                                }
                                catch(Exception ex)
                                {
                                    
                                }
                                    try
                                    {
                        minusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                             where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                 select r.Field<double>("weight")).ToList();
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                        try

                                        {

                        plusMinusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                 where r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<Double>("weight") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                     select r.Field<double>("weight")).ToList();
                                        }
                                        catch(Exception ex)
                                        {

                                        }

                                        try
                                        {
                                            plusfive = (from r in tbldt1.AsEnumerable()
                                                        where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                        select r.Field<double>("weight")).ToList();
                                        }
                                        catch (Exception ex)
                                        { }
                                        try
                                        {
                                            minusFive = (from r in tbldt1.AsEnumerable()
                                                         where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                         select r.Field<double>("weight")).ToList();
                                        }
                                        catch (Exception ex)
                                        { }
                                        try
                                        {

                                            plusMinusFive = (from r in tbldt1.AsEnumerable()
                                                             where r.Field<Double>("weight") < (specweight + specPlusfive) && r.Field<Double>("weight") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                             select r.Field<double>("weight")).ToList();
                                        }
                                        catch (Exception ex)
                                        { }


                         float finalplusone =(float)(plusone.Count * 100) / count.Count;
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


                       
                        dr[7] = count.Count.ToString();
                        dr[8] = average;
                        dr[9] = plusone.Count;
                        dr[10] = finalplusone;

                        dr[11] = minusone.Count;
                        dr[12] = finalminusone;

                        dr[13] = plusMinusOne.Count;
                        dr[14] = finalplusminusone;
                        dr[15] = plusTwopointfive.Count;
                        dr[16] = finalplustwo;

                        dr[17] = minusTwoPointFive.Count;
                        dr[18] = finalminustwo;
                        dr[19] = plusMinusTwoPointFive.Count;
                        dr[20] = finalplusminustwo;
                        dr[21] = plusfive.Count;
                        dr[22] = finalplusfive;
                        dr[23] = minusFive.Count;
                        dr[24] = finalminusfive;
                        dr[25] = plusMinusFive.Count;
                        dr[26] = finalplusminusfive;

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
                dr1[25] = "";
                dr1[26] = "";

                DataTable dtExcel = new DataTable();

                finaldt.Rows.Add(dr1);
                //ViewState["dt"] = finaldt;

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

//                myConnection.comm.CommandText = "Select recipeCode,description, dtandTime from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + durationQuery + " ORDER BY recipeCode, dtandTime";
//                myConnection.reader = myConnection.comm.ExecuteReader();
//                wcdt.Load(myConnection.reader);

//                myConnection.reader.Close();
//                myConnection.comm.Dispose();
//                myConnection.close(ConnectionOption.SQL);

//                var query = wcdt.AsEnumerable()
//    .GroupBy(row => new
//    {
//        Hour = row.Field<DateTime>("dtandTime").Hour,
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
//                dt.Columns.Add("dtandTime", typeof(DateTime));

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

//                myConnection.comm.CommandText = "Select manningID, firstName, lastName, dtandTime from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + processType + " AND " + durationQuery + " " + sqlquery + " ORDER BY manningID, dtandTime";
//                myConnection.reader = myConnection.comm.ExecuteReader();
//                wcdt.Load(myConnection.reader);

//                myConnection.reader.Close();
//                myConnection.comm.Dispose();
//                myConnection.close(ConnectionOption.SQL);

//                var query = wcdt.AsEnumerable()
//    .GroupBy(row => new
//    {
//        Hour = row.Field<DateTime>("dtandTime").Hour,
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
        protected void showReportMonthWcWise(int getMonth, int getYear , string process)
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


                gridviewdt.Columns.Add("Curing Recipe", typeof(string));
                gridviewdt.Columns.Add("Curing Sapcode", typeof(string));

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


                finaldt.Columns.Add("Curing Recipe", typeof(string));
                finaldt.Columns.Add("Curing Sapcode", typeof(string));

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

                if (process == "TBR")
                    tablename = "VBuddeScannedTyreDetail";
                else if (process == "PCR")
                    tablename = "vPCRBuddeScannedTyreDetail";

                if (recipeCode == "All")
                {
                    if (process == "TBR")
                    {
                      //  myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from VBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and weight!='')  and (dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
                          myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from VBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details CRD on t1.gtbarcode=CRD.gtbarcode  where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and weight!='')  and (dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
   
                    }
                    else
                    {
                       // myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from vPCRBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + " ' AND dtandTime < '" + rtoDate + "')   and dtandTime >= '" + rfromDate + " ' AND dtandTime < '" + rtoDate + "'";

                        myConnection.comm.CommandText = " Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from vPCRBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details1 CRD on t1.gtbarcode=CRD.gtbarcode  where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and weight!='')  and (dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "')";
             
                    }

                    // myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode from "+tablename+" WHERE  " + durationQuery + "";
                }
                else
                {

                    if (process == "TBR")
                    {
                       // myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from VBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + " ' and recipeCode='" + recipeCode + "' and weight!='')  and (dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "') and recipeCode='" + recipeCode + "'";
                        myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from VBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details CRD on t1.gtbarcode=CRD.gtbarcode where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM VBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and recipeCode='" + recipeCode + "' and weight!='')  and (dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "') and recipeCode='" + recipeCode + "'";
       
                    
                    }
                    else
                    {
                      //  myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,gtbarcode,SAPMaterialCode as Sapcode from vPCRBuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and recipeCode='" + recipeCode + "' ) and recipeCode='" + recipeCode + "' and dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' ";
                        myConnection.comm.CommandText = "Select recipeCode,weight,dtandTime,t1.gtbarcode,SAPMaterialCode as Sapcode,CRD.Curing_Recipe,CRD.Curing_Sapcode from vPCRBuddeScannedTyreDetail t1 inner join Curing_Recipe_Details1 CRD on t1.gtbarcode=CRD.gtbarcode where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vPCRBuddeScannedTyreDetail t2 WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' and recipeCode='" + recipeCode + "' ) and recipeCode='" + recipeCode + "' and dtandTime >= '" + rfromDate + "' AND dtandTime < '" + rtoDate + "' ";
                 
                    }
                }
                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                string tempweight = "";
                for (int i = 0; i < tbldt.Rows.Count; i++)
                {
                    String[] weight = tbldt.Rows[i][1].ToString().Split('_');
                    tempweight = "";

                    for (int j = 0; j < weight.Length; j++)
                    {
                        if (weight[j].Length > 2)
                        {
                            tempweight = weight[j].ToString();
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
                tbldt1.Columns.Add("weight", typeof(Double));
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
                       Sapcode = s.Field<string>("Sapcode"),
                       Curing_Recipe = s.Field<string>("Curing_Recipe"),
                       Curing_Sapcode = s.Field<string>("Curing_Sapcode")

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
                     //    date1 = row.Max(T => T.Field<DateTime>("dtandtime")),
                     //    //weight = row.Key.weight,
                     //})
                     //.ToList();

                        var count = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Select(row => row.Field<Double>("weight")).ToList();
                        Double average = tbldt1.AsEnumerable().Where(row => row.Field<string>("recipeCode") == distinctvalues[i].recipeCode).Average(row => row.Field<Double>("weight"));
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
                        dr[4] = distinctvalues[i].Curing_Recipe;
                        dr[5] = distinctvalues[i].Curing_Sapcode;
                        dr[6] = aa[0].ToString();
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
                                       where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                       select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusone = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusOne = (from r in tbldt1.AsEnumerable()
                                            where r.Field<Double>("weight") < (specweight + specwOnePecent) && r.Field<Double>("weight") > (specweight - specwOnePecent) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                            select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {

                            plusTwopointfive = (from r in tbldt1.AsEnumerable()
                                                where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                select r.Field<double>("weight")).ToList();

                            //plusTwopointfive = (from r in tbldt1.AsEnumerable()
                            //                       where r.Field<Double>("weight") > specweight && r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                            //                       select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            minusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                 where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                 select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {

                            plusMinusTwoPointFive = (from r in tbldt1.AsEnumerable()
                                                     where r.Field<Double>("weight") < (specweight + specPlustwopintfive) && r.Field<Double>("weight") > (specweight - specPlustwopintfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                                     select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            plusfive = (from r in tbldt1.AsEnumerable()
                                        where r.Field<Double>("weight") >= specweight && r.Field<Double>("weight") < (specweight + specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                        select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            minusFive = (from r in tbldt1.AsEnumerable()
                                         where r.Field<Double>("weight") < specweight && r.Field<Double>("weight") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                         select r.Field<double>("weight")).ToList();
                        }
                        catch (Exception ex)
                        { }
                        try
                        {

                            plusMinusFive = (from r in tbldt1.AsEnumerable()
                                             where r.Field<Double>("weight") < (specweight + specPlusfive) && r.Field<Double>("weight") > (specweight - specPlusfive) && r.Field<String>("recipeCode").Equals(distinctvalues[i].recipeCode)
                                             select r.Field<double>("weight")).ToList();
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

                        dr[7] = count.Count.ToString();
                        dr[8] = average;
                        dr[9] = plusone.Count;
                        dr[10] = finalplusone;

                        dr[11] = minusone.Count;
                        dr[12] = finalminusone;

                        dr[13] = plusMinusOne.Count;
                        dr[14] = finalplusminusone;
                        dr[15] = plusTwopointfive.Count;
                        dr[16] = finalplustwo;

                        dr[17] = minusTwoPointFive.Count;
                        dr[18] = finalminustwo;
                        dr[19] = plusMinusTwoPointFive.Count;
                        dr[20] = finalplusminustwo;
                        dr[21] = plusfive.Count;
                        dr[22] = finalplusfive;
                        dr[23] = minusFive.Count;
                        dr[24] = finalminusfive;
                        dr[25] = plusMinusFive.Count;
                        dr[26] = finalplusminusfive;
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
                dr1[25] = "";
                dr1[26] = "";


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
    //    protected void showReportYearWcWise(int getYear)
    //    {
    //        try
    //        {
    //            DataTable dt = new DataTable();
    //            DataTable gridviewdt = new DataTable();

    //            gridviewdt.Columns.Add("wcName", typeof(string));
    //            gridviewdt.Columns.Add("tireType", typeof(string));
    //            gridviewdt.Columns.Add("Product Name", typeof(string));

    //            //Generate datatable columns dynamically
    //            for (int i = 1; i <= 12; i++)
    //                gridviewdt.Columns.Add(i.ToString(), typeof(int));
    //            gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

    //            string fromDate = getYear.ToString() + "-01-01 07:00:00";
    //            string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

    //            myConnection.open(ConnectionOption.SQL);
    //            myConnection.comm = myConnection.conn.CreateCommand();

    //            myConnection.comm.CommandText = "Select wcName, recipeCode,description, dtandTime from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + processType + " AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
    //            myConnection.reader = myConnection.comm.ExecuteReader();
    //            dt.Load(myConnection.reader);

    //            myConnection.reader.Close();
    //            myConnection.comm.Dispose();
    //            myConnection.close(ConnectionOption.SQL);

    //            var query = dt.AsEnumerable()
    //.GroupBy(row => new
    //{
    //    month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
    //    recipeCode = row.Field<string>("recipeCode"),
    //    description = row.Field<string>("description"),
    //    wcName = row.Field<string>("wcName")
    //})
    //.Select(g => new
    //{
    //    WcName = g.Key.wcName,
    //    month = g.Key.month,
    //    recipeCode = g.Key.recipeCode,
    //    description = g.Key.description,
    //    quantity = g.Count()
    //}
    //    );
    //            DataRow dr = gridviewdt.NewRow();
    //            var items = query.ToArray();
    //            int total = 0;
    //            for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
    //            {

    //                if (i == 0)
    //                {
    //                    dr[0] = items[i].WcName.ToString();
    //                    dr[1] = items[i].recipeCode.ToString();
    //                    if (items[i].description == null)
    //                        dr[2] = "";
    //                    else
    //                        dr[2] = items[i].description.ToString();

    //                    // Insert rows in all the days places by default
    //                    for (int h = 3; h <= (12 + 2); h++)
    //                        dr[h] = 0;
    //                }
    //                else if ((items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i - 1].WcName.ToString())) //If Recipe or Workcenter changes, then create new data row
    //                {
    //                    dr = gridviewdt.NewRow();
    //                    dr[0] = items[i].WcName.ToString();
    //                    dr[1] = items[i].recipeCode.ToString();
    //                    if (items[i].description == null)
    //                        dr[2] = "";
    //                    else
    //                        dr[2] = items[i].description.ToString();

    //                    // Insert rows in all the hours places by default
    //                    for (int h = 3; h <= (12 + 2); h++)
    //                        dr[h] = 0;
    //                }

    //                dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


    //                if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
    //                {
    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 3; v <= (12 + 2); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 3] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }
    //                else if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) || (items[i].WcName.ToString() != items[i + 1].WcName.ToString())) //Check if next recipe or workcenter is different from current one, then insert row in datatable
    //                {
    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 3; v <= (12 + 2); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 3] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }

    //                // Wcwise Total

    //                if (i == (items.Length - 1))  //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
    //                {
    //                    DataRow ndr = gridviewdt.NewRow();
    //                    ndr[0] = "Total";
    //                    ndr[1] = "";
    //                    for (int v = 3; v <= (12 + 3); v++)
    //                    {
    //                        //ndr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(r => r.Field<int>(v));
    //                        ndr[v] = gridviewdt.AsEnumerable().Sum(r => r.Field<int>(v));

    //                    }
    //                    gridviewdt.Rows.Add(ndr);
    //                }
    //                else if (items[i].WcName.ToString() != items[i + 1].WcName.ToString()) //Check if next Workcenter is different from current one, then insert row in datatable
    //                {
    //                    //if ((items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()))
    //                    //{

    //                    //DataRow ndr = gridviewdt.NewRow();
    //                    //ndr[0] = "Total";
    //                    //ndr[1] = "";
    //                    //for (int v = 3; v <= (12 + 3); v++)
    //                    //{
    //                    //    //ndr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(r => r.Field<int>(v));
    //                    //    ndr[v] = gridviewdt.AsEnumerable().Sum(r => r.Field<int>(v));
    //                    //}
    //                    //gridviewdt.Rows.Add(ndr);
    //                    //}
    //                }
    //                //End WCwise Total
    //            }

    //            MainGridView.Columns.Clear();
    //            //Iterate through the columns of the datatable to set the data bound field dynamically.
    //            foreach (DataColumn col in gridviewdt.Columns)
    //            {
    //                //Declare the bound field and allocate memory for the bound field.
    //                BoundField bfield = new BoundField();

    //                //Initalize the DataField value.
    //                bfield.DataField = col.ColumnName;

    //                //Initialize the HeaderText field value.
    //                if (col.ColumnName == "wcName" || col.ColumnName == "tireType" || col.ColumnName == "Product Name" || col.ColumnName == "Total")
    //                    bfield.HeaderText = col.ColumnName;
    //                else
    //                    bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);


    //                //Add the newly created bound field to the GridView.
    //                MainGridView.Columns.Add(bfield);

    //            }

    //            // Calculate total of all workstations of every hour
    //            //dr = gridviewdt.NewRow();
    //            //gridviewdt.Rows.Add(dr);
    //            //dr = gridviewdt.NewRow();
    //            //dr[0] = "Total";
    //            //dr[1] = "";
    //            //for (int v = 3; v <= (12 + 3); v++)
    //            //{
    //            //    dr[v] = gridviewdt.AsEnumerable().Where(r => r.Field<string>(1) == "Total").Sum(dra => dra.Field<int>(v));

    //            //}
    //            //gridviewdt.Rows.Add(dr);

    //            MainGridView.DataSource = gridviewdt;
    //            MainGridView.DataBind();

    //            // Select the rows where showing total & make them bold
    //            IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    //.Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text == "Total");

    //            foreach (var row in rows)
    //                row.Font.Bold = true;

    //            gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

    //            DataTable newdt = new DataTable();
    //            newdt.Columns.Add("columnName", typeof(string));
    //            newdt.Columns.Add("months", typeof(string));
    //            for (int i = 0; i < (gridviewdt.Columns.Count - 3); i++)
    //            {
    //                if (gridviewdt.Columns[3 + i].ColumnName != "Total" && gridviewdt.Columns[3 + i].ColumnName != "wcName" && gridviewdt.Columns[3 + i].ColumnName != "tireType" && gridviewdt.Columns[3 + i].ColumnName != "description")
    //                {
    //                    DataRow drow = newdt.NewRow();
    //                    drow[0] = gridviewdt.Columns[3 + i].ColumnName;
    //                    drow[1] = gridviewdt.Rows[0][3 + i];
    //                    newdt.Rows.Add(drow);
    //                }
    //            }

    //            TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
    //            TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
    //            TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
    //            TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

    //            TBMChart.DataSource = newdt;
    //            TBMChart.Series["TBMSeries"].XValueMember = "columnName";
    //            TBMChart.Series["TBMSeries"].YValueMembers = "months";
    //            TBMChart.DataBind();
    //        }
    //        catch (Exception exp)
    //        {
    //            myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
    //        }
    //    }
    //    protected void showReportYearRecipeWise(int getYear)
    //    {
    //        try
    //        {
    //            DataTable dt = new DataTable();
    //            DataTable gridviewdt = new DataTable();

    //            gridviewdt.Columns.Add("tireType", typeof(string));
    //            gridviewdt.Columns.Add("Product Name", typeof(string));
    //            //Generate datatable columns dynamically
    //            for (int i = 1; i <= 12; i++)
    //                gridviewdt.Columns.Add(i.ToString(), typeof(int));
    //            gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

    //            string fromDate = getYear.ToString() + "-01-01 07:00:00";
    //            string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

    //            myConnection.open(ConnectionOption.SQL);
    //            myConnection.comm = myConnection.conn.CreateCommand();

    //            myConnection.comm.CommandText = "Select wcName, recipeCode,description,dtandTime from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + processType + " AND " + durationQuery + " ORDER BY wcID, recipeCode, dtandTime asc";
    //            myConnection.reader = myConnection.comm.ExecuteReader();
    //            dt.Load(myConnection.reader);

    //            myConnection.reader.Close();
    //            myConnection.comm.Dispose();
    //            myConnection.close(ConnectionOption.SQL);

    //            var query = dt.AsEnumerable()
    //.GroupBy(row => new
    //{
    //    month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
    //    recipeCode = row.Field<string>("recipeCode"),
    //    description = row.Field<string>("description")
    //})
    //.Select(g => new
    //{
    //    month = g.Key.month,
    //    recipeCode = g.Key.recipeCode,
    //    description = g.Key.description,
    //    quantity = g.Count()
    //}
    //    );
    //            DataRow dr = gridviewdt.NewRow();
    //            var items = query.ToArray();
    //            int total = 0;
    //            for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
    //            {

    //                if (i == 0)
    //                {
    //                    dr[0] = items[i].recipeCode.ToString();
    //                    if (items[i].description == null)
    //                        dr[1] = "";
    //                    else
    //                        dr[1] = items[i].description.ToString();

    //                    // Insert rows in all the days places by default
    //                    for (int h = 2; h <= (12 + 1); h++)
    //                        dr[h] = 0;
    //                }
    //                else if (items[i - 1].recipeCode.ToString() != items[i].recipeCode.ToString()) //If Recipe or Workcenter changes, then create new data row
    //                {
    //                    dr = gridviewdt.NewRow();
    //                    dr[0] = items[i].recipeCode.ToString();
    //                    if (items[i].description == null)
    //                        dr[1] = "";
    //                    else
    //                        dr[1] = items[i].description.ToString();

    //                    // Insert rows in all the hours places by default
    //                    for (int h = 2; h <= (12 + 1); h++)
    //                        dr[h] = 0;
    //                }

    //                dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)


    //                if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
    //                {
    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 2; v <= (12 + 1); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 2] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }
    //                else if (items[i].recipeCode.ToString() != items[i + 1].recipeCode.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
    //                {

    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 2; v <= (12 + 1); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 2] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }
    //            }

    //            MainGridView.Columns.Clear();
    //            //Iterate through the columns of the datatable to set the data bound field dynamically.
    //            foreach (DataColumn col in gridviewdt.Columns)
    //            {
    //                //Declare the bound field and allocate memory for the bound field.
    //                BoundField bfield = new BoundField();

    //                //Initalize the DataField value.
    //                bfield.DataField = col.ColumnName;

    //                //Initialize the HeaderText field value.
    //                if (col.ColumnName == "tireType" || col.ColumnName == "Product Name" || col.ColumnName == "Total")
    //                    bfield.HeaderText = col.ColumnName;
    //                else
    //                    bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);

    //                //Add the newly created bound field to the GridView.
    //                MainGridView.Columns.Add(bfield);

    //            }

    //            // Calculate total of all workstations of every hour
    //            dr = gridviewdt.NewRow();
    //            dr[0] = "Total";
    //            for (int v = 2; v <= (12 + 1); v++)
    //            {
    //                dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

    //            }
    //            gridviewdt.Rows.Add(dr);

    //            MainGridView.DataSource = gridviewdt;
    //            MainGridView.DataBind();

    //            gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

    //            DataTable newdt = new DataTable();
    //            newdt.Columns.Add("columnName", typeof(string));
    //            newdt.Columns.Add("months", typeof(string));
    //            for (int i = 0; i < (gridviewdt.Columns.Count - 2); i++)
    //            {
    //                if (gridviewdt.Columns[2 + i].ColumnName != "Total" && gridviewdt.Columns[2 + i].ColumnName != "wcName" && gridviewdt.Columns[2 + i].ColumnName != "tireType" && gridviewdt.Columns[2 + i].ColumnName != "description")
    //                {
    //                    DataRow drow = newdt.NewRow();
    //                    drow[0] = gridviewdt.Columns[2 + i].ColumnName;
    //                    drow[1] = gridviewdt.Rows[0][2 + i];
    //                    newdt.Rows.Add(drow);
    //                }
    //            }

    //            TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
    //            TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
    //            TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
    //            TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

    //            TBMChart.DataSource = newdt;
    //            TBMChart.Series["TBMSeries"].XValueMember = "columnName";
    //            TBMChart.Series["TBMSeries"].YValueMembers = "months";
    //            TBMChart.DataBind();
    //        }
    //        catch (Exception exp)
    //        {
    //            myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
    //        }
    //    }
    //    protected void showReportYearOperatorWise(int getYear)
    //    {
    //        try
    //        {
    //            string sqlquery = (getOperator != "All") ? " AND manningID=" + getOperator : "";

    //            DataTable wcdt = new DataTable();
    //            DataTable manningdt = new DataTable();
    //            DataTable dt = new DataTable();
    //            dt.Columns.Add("operator", typeof(string));
    //            dt.Columns.Add("dtandTime", typeof(DateTime));

    //            DataTable gridviewdt = new DataTable();
    //            gridviewdt.Columns.Add("operator", typeof(string));

    //            //Generate datatable columns dynamically
    //            for (int i = 1; i <= 12; i++)
    //                gridviewdt.Columns.Add(i.ToString(), typeof(int));
    //            gridviewdt.Columns.Add("Total", typeof(int));  //Column for the total of whole month

    //            // Get the Data based on WCName
    //            string fromDate = getYear.ToString() + "-01-01 07:00:00";
    //            string toDate = (getYear + 1).ToString() + "-01-01 07:00:00";

    //            myConnection.open(ConnectionOption.SQL);
    //            myConnection.comm = myConnection.conn.CreateCommand();

    //            myConnection.comm.CommandText = "Select manningID, firstName, lastName, dtandTime from vCuringProduction1 WHERE wcID IN " + wcIDInQuery.ToString() + " AND " + processType + " AND " + durationQuery + " " + sqlquery + " ORDER BY manningID, dtandTime asc";
    //            myConnection.comm.CommandTimeout = 0;
    //            myConnection.reader = myConnection.comm.ExecuteReader();
    //            wcdt.Load(myConnection.reader);

    //            myConnection.reader.Close();
    //            myConnection.comm.Dispose();
    //            myConnection.close(ConnectionOption.SQL);

    //            var query = wcdt.AsEnumerable()
    //.GroupBy(row => new
    //{
    //    month = row.Field<DateTime>("dtandTime").AddHours(-7).Month,
    //    manningID = row.Field<int>("manningID"),
    //    firstName = row.Field<string>("firstName"),
    //    lastName = row.Field<string>("lastName")
    //})
    //.Select(g => new
    //{
    //    month = g.Key.month,
    //    manningID = g.Key.manningID,
    //    firstName = g.Key.firstName,
    //    lastName = g.Key.lastName,
    //    quantity = g.Count()
    //}
    //    );
    //            DataRow dr = gridviewdt.NewRow();
    //            var items = query.ToArray();
    //            int total = 0;
    //            for (int i = 0; i <= (items.Length - 1); i++) // Run loop until get all the quantities in the array
    //            {
    //                if (i == 0) //If loops execute first time, then insert wcName & recipeCode in the datarow
    //                {
    //                    dr[0] = items[i].firstName.ToString() + " " + items[i].lastName.ToString();

    //                    // Insert rows in all the hours places by default
    //                    for (int h = 1; h <= (12 + 1); h++)
    //                        dr[h] = 0;
    //                }
    //                else if (items[i - 1].manningID.ToString() != items[i].manningID.ToString()) //If Recipe or Workcenter changes, then create new data row
    //                {
    //                    dr = gridviewdt.NewRow();
    //                    dr[0] = items[i].firstName.ToString() + " " + items[i].lastName.ToString();

    //                    // Insert rows in all the hours places by default
    //                    for (int h = 1; h <= (12 + 1); h++)
    //                        dr[h] = 0;
    //                }

    //                dr[items[i].month.ToString()] = items[i].quantity.ToString(); //If rows are consecutive then simply insert column values(quantity)

    //                if (i == (items.Length - 1)) //If it is the last row of the error then go here otherwise items[i + 1] will give "Index bound error"
    //                {
    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 1; v <= (12 + 1); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 1] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }
    //                else if (items[i].manningID.ToString() != items[i + 1].manningID.ToString()) //Check if next recipe or workcenter is different from current one, then insert row in datatable
    //                {
    //                    total = 0;
    //                    // Display Whole Month Total
    //                    for (int v = 1; v <= (12 + 1); v++)
    //                        total += Convert.ToInt32(dr[v]);

    //                    dr[12 + 1] = total;

    //                    gridviewdt.Rows.Add(dr);
    //                }

    //            }

    //            MainGridView.Columns.Clear();
    //            //Iterate through the columns of the datatable to set the data bound field dynamically.
    //            foreach (DataColumn col in gridviewdt.Columns)
    //            {
    //                //Declare the bound field and allocate memory for the bound field.
    //                BoundField bfield = new BoundField();

    //                //Initalize the DataField value.
    //                bfield.DataField = col.ColumnName;

    //                //Initialize the HeaderText field value.
    //                //Initialize the HeaderText field value.
    //                if (col.ColumnName == "operator" || col.ColumnName == "Total")
    //                    bfield.HeaderText = col.ColumnName;
    //                else
    //                    bfield.HeaderText = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(col.ColumnName)).Substring(0, 3);


    //                //Add the newly created bound field to the GridView.
    //                MainGridView.Columns.Add(bfield);

    //            }

    //            // Calculate total of all workstations of every day
    //            dr = gridviewdt.NewRow();
    //            dr[0] = "Total";
    //            for (int v = 1; v <= (12 + 1); v++)
    //            {
    //                dr[v] = gridviewdt.AsEnumerable().Sum(dra => dra.Field<int>(v));

    //            }
    //            gridviewdt.Rows.Add(dr);

    //            MainGridView.DataSource = gridviewdt;
    //            MainGridView.DataBind();


    //            gridviewdt = gridviewdt.AsEnumerable().Skip(gridviewdt.Rows.Count - 1).CopyToDataTable();

    //            DataTable newdt = new DataTable();
    //            newdt.Columns.Add("columnName", typeof(string));
    //            newdt.Columns.Add("months", typeof(string));
    //            for (int i = 0; i < (gridviewdt.Columns.Count - 1); i++)
    //            {
    //                if (gridviewdt.Columns[1 + i].ColumnName != "Total" && gridviewdt.Columns[1 + i].ColumnName != "wcName" && gridviewdt.Columns[1 + i].ColumnName != "tireType")
    //                {
    //                    DataRow drow = newdt.NewRow();
    //                    drow[0] = gridviewdt.Columns[1 + i].ColumnName;
    //                    drow[1] = gridviewdt.Rows[0][1 + i];
    //                    newdt.Rows.Add(drow);
    //                }
    //            }

    //            TBMChart.Series["TBMSeries"].ChartType = SeriesChartType.Column;
    //            TBMChart.Series["TBMSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
    //            TBMChart.ChartAreas["TBMChartArea"].Area3DStyle.Enable3D = true;
    //            TBMChart.Series["TBMSeries"].IsValueShownAsLabel = true;

    //            TBMChart.DataSource = newdt;
    //            TBMChart.Series["TBMSeries"].XValueMember = "columnName";
    //            TBMChart.Series["TBMSeries"].YValueMembers = "months";
    //            TBMChart.DataBind();
    //        }
    //        catch (Exception exp)
    //        {
    //            myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
    //        }
    //    }
        #endregion
        #region User Defined Function
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            //        {
            //            GridViewRow gvRow = MainGridView.Rows[rowIndex];
            //            GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
            //            for (int cellCount = 0; cellCount < 1 /*gvRow.Cells.Count*/; cellCount++)
            //            {
            //                if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
            //                {
            //                    if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
            //                    {
            //                        gvRow.Cells[cellCount].RowSpan = 2;
            //                    }
            //                    else
            //                    {
            //                        gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
            //                    }
            //                    gvPreviousRow.Cells[cellCount].Visible = false;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception exp)
            //{
            //    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            //}
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
           
               
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    TableHeaderCell cell = new TableHeaderCell();

                    switch (duration)
                    {
                        case "Date":
                            cell.Text = "";
                            cell.ColumnSpan = 7;
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
                            cell.ColumnSpan = 7;
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
