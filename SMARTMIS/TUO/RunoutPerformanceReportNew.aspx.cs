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
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart;

namespace SmartMIS.TUO
{
    public partial class RunoutPerformanceReportNew : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        int total_ = 0, finalyield_=0, runoutA_ = 0, runoutB_ = 0, runoutC_ = 0, runoutD_ = 0, runoutE_ = 0, DentA_ = 0, DentB_ = 0, DentC_ = 0, DentD_ = 0, DentE_ = 0, BulgeA_ = 0, BulgeB_ = 0, BulgeC_ = 0, BulgeD_ = 0, BulgeE_ = 0, balancingA_ = 0, balancingB_ = 0, balancingC_ = 0, balancingD_ = 0, balancingE_ = 0;
        
        Double pA_, pB_, pC_, pD_, pE_;
        public Double grandtotal, grandA, grandB, grandC, grandD, grandE;
        string tablename;

        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", machineStatus = "", wCenterName = "default";
        string query = "", getdisplaytype="";
        string[] tempString2;
        int rowCount = 4, pid = -1;
                
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable exldt;
        
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "performanceReportTUOmachineWise.xlsx";
        string filepath; 

        #endregion       
        public RunoutPerformanceReportNew()
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
                        reportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");

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
        protected void Button_Click(object sender, EventArgs e)
        {
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());
            try
            {

                if (((Button)sender).ID == "ErankViewDetailButton")
                {
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string wcname = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseWCNameLabel")).Text);
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
               
                Label1.Text = "false";
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        //private void loadWCData1(string dtnadtime1)
        //{
        //    try
        //    {
        //        DataTable recipe_dt = new DataTable();
        //        DataTable tuo_dt = new DataTable();

        //        myConnection.open(ConnectionOption.SQL);
        //        myConnection.comm = myConnection.conn.CreateCommand();

        //        myConnection.comm.CommandText = "select name, tyreSize, tyreDesign from recipeMaster";
        //        myConnection.reader = myConnection.comm.ExecuteReader();
        //        recipe_dt.Load(myConnection.reader);

        //        myConnection.comm.CommandText = "select *  from vtbrrunoutData1 WHERE ((dtandTime>" + dtnadtime1 + " and   wcName in('TRO2','TRO3','TRO4')  ORDER BY wcname ASC";
        //        myConnection.reader = myConnection.comm.ExecuteReader();
        //        tuo_dt.Load(myConnection.reader);

        //        if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
        //        {
        //            dt1 = tuo_dt.Copy(); ;
        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
        //        {
        //            var row = from r0w1 in tuo_dt.AsEnumerable()
        //                      join r0w2 in recipe_dt.AsEnumerable()
        //                        on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
        //                      where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text
        //                      select r0w1.ItemArray.ToArray();
        //            foreach (object[] values in row)
        //                dt1.Rows.Add(values);
        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
        //        {
        //            var row = from r0w1 in tuo_dt.AsEnumerable()
        //                      join r0w2 in recipe_dt.AsEnumerable()
        //                        on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
        //                      where r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
        //                      select r0w1.ItemArray.ToArray();
        //            foreach (object[] values in row)
        //                dt1.Rows.Add(values);
        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
        //        {
        //            var row = from r0w1 in tuo_dt.AsEnumerable()
        //                      join r0w2 in recipe_dt.AsEnumerable()
        //                        on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
        //                      where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text && r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
        //                      select r0w1.ItemArray.ToArray();
        //            foreach (object[] values in row)
        //                dt1.Rows.Add(values);
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //    }
        //    finally
        //    {
        //        myConnection.reader.Close();
        //        myConnection.comm.Dispose();
        //        myConnection.close(ConnectionOption.SQL);

        //    }
        //}
     
        private void loadWCData(string dtnadtime1)
        {
            try
            {
                DataTable recipe_dt = new DataTable();
                DataTable tuo_dt = new DataTable();
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name, tyreSize, tyreDesign from recipeMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                recipe_dt.Load(myConnection.reader);
               // myConnection.comm.CommandText = "select *  from vtbrrunoutData1 WHERE ((dtandTime>" + dtnadtime1 + " and   wcName in('TRO1')  ORDER BY wcname ASC";

                myConnection.comm.CommandText = "select *,wcMaster.name As WcName  from tbrrunoutData  inner join wcmaster on wcMaster.iD=tbrrunoutData.wcID  WHERE ((dtandTime>" + dtnadtime1 + " and wcID='261' ORDER BY wcMaster.name ASC";
                myConnection.reader = myConnection.comm.ExecuteReader();
                tuo_dt.Load(myConnection.reader);

                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    dt = tuo_dt.Copy(); 
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                          join r0w2 in recipe_dt.AsEnumerable()
                            on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("recipeCode") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text && r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values); 
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
        private void showQualityReportWise()
        {
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("wcName", typeof(string));
            gridviewdt.Columns.Add("recipeCode", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("finalYield", typeof(string));
            gridviewdt.Columns.Add("RunoutA", typeof(string));
            gridviewdt.Columns.Add("RunoutB", typeof(string));
            gridviewdt.Columns.Add("RunoutC", typeof(string));
            gridviewdt.Columns.Add("RunoutD", typeof(string));
            gridviewdt.Columns.Add("RunoutE", typeof(string));
            gridviewdt.Columns.Add("DentA", typeof(string));
            gridviewdt.Columns.Add("DentB", typeof(string));
            gridviewdt.Columns.Add("DentC", typeof(string));
            gridviewdt.Columns.Add("DentD", typeof(string));
            gridviewdt.Columns.Add("DentE", typeof(string));
            gridviewdt.Columns.Add("BulgeA", typeof(string));
            gridviewdt.Columns.Add("BulgeB", typeof(string));
            gridviewdt.Columns.Add("BulgeC", typeof(string));
            gridviewdt.Columns.Add("BulgeD", typeof(string));
            gridviewdt.Columns.Add("BulgeE", typeof(string));
            gridviewdt.Columns.Add("BalancingA", typeof(string));
            gridviewdt.Columns.Add("BalancingB", typeof(string));
            gridviewdt.Columns.Add("BalancingC", typeof(string));
            gridviewdt.Columns.Add("BalancingD", typeof(string));
            gridviewdt.Columns.Add("BalancingE", typeof(string));

            DataRow drt;
            int total = 0, finalyield, runoutA = 0, runoutB = 0, runoutC = 0, runoutD = 0, runoutE = 0, DentA = 0, DentB = 0, DentC = 0, DentD = 0, DentE = 0, BulgeA = 0, BulgeB = 0, BulgeC = 0, BulgeD = 0, BulgeE = 0, balancingA = 0, balancingB = 0, balancingC = 0, balancingD = 0, balancingE = 0;
            Double ptotal = 0, Pfinalyield=0, prunoutA = 0, prunoutB = 0, prunoutC = 0, prunoutD = 0, prunoutE = 0, pDentA = 0, pDentB = 0, pDentC = 0, pDentD = 0, pDentE = 0, pBulgeA = 0, pBulgeB = 0, pBulgeC = 0, pBulgeD = 0, pBulgeE = 0, pbalancingA = 0, pbalancingB = 0, pbalancingC = 0, pbalancingD = 0, pbalancingE = 0;
            Double dtotal = 0, drunoutA = 0, drunoutB = 0, drunoutC = 0, drunoutD = 0, drunoutE = 0, dDentA = 0, dDentB = 0, dDentC = 0, dDentD = 0, dDentE = 0, dBulgeA = 0, dBulgeB = 0, dBulgeC = 0, dBulgeD = 0, dBulgeE = 0, dbalancingA = 0, dbalancingB = 0, dbalancingC = 0, dbalancingD = 0, dbalancingE = 0;
            
                        
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            var wc_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("wcName"))
                    .Select(g => new
                    {
                        wc_id = g.Key
                    }));
           

            var wc_items = wc_query.ToArray();
         
            switch (getdisplaytype)
            {
                
                case "Numbers":

                   
                    #region TRO1
                    foreach (var wc_item in wc_items)
                    {
                        ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;

                            var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("recipeCode")) .Select(g => new  {  tyreType_id = g.Key }));
                            var tyreType_items = tyreType_query.ToArray();
                            #region loop2
                            foreach (var tyreType_item in tyreType_items)
                            {
                                var data = dt.AsEnumerable().Where(l => l.Field<string>("wcName") == wc_item.wc_id && l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                                {
                                    wcname = l.Field<string>("wcName"),
                                    tyreType = l.Field<string>("recipeCode"),
                                    RoRank = l.Field<string>("ROTotalRank"),
                                    upperGrade = l.Field<string>("upperRank"),
                                    lowerGrade = l.Field<string>("lowerRank"),
                                    staticGrade = l.Field<string>("staticRank"),
                                    uplowGrade = l.Field<string>("uploRank"),
                                    TbulgeGrade = l.Field<string>("lroTbulgeRank"),
                                    BbulgeGrade = l.Field<string>("lroBbulgeRank"),
                                    TdentGrade = l.Field<string>("lroTdentRank"),
                                    BdentGrade = l.Field<string>("lroBdentRank"),
                                    TotalRank_balance = l.Field<string>("TotalRank")
                                }).ToArray();
                                #region fill data gradewise
                                if (data.Count() != 0)
                                {
                                    DataRow dr = gridviewdt.NewRow();
                                    total = data.Count();
                                    finalyield = data.Count(d => d.RoRank == "A" && d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A" && d.TbulgeGrade == "A" && d.BbulgeGrade == "A" && d.TdentGrade == "A" && d.BdentGrade == "A");
                                    runoutA = data.Count(d => d.RoRank == "A");
                                    runoutB = data.Count(d => d.RoRank == "B");
                                    runoutC = data.Count(d => d.RoRank == "C");
                                    runoutD = data.Count(d => d.RoRank == "D");
                                    runoutE = data.Count(d => d.RoRank == "E");

                                     balancingA = data.Count(d => d.TotalRank_balance == "A");
                                     balancingB = data.Count(d => d.TotalRank_balance == "B");
                                     balancingC = data.Count(d => d.TotalRank_balance == "C");
                                     balancingD = data.Count(d => d.TotalRank_balance == "D");
                                     balancingE = data.Count(d => d.TotalRank_balance == "E");





                                    //balancingA = data.Count(d => d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A");
                                    //balancingB = data.Count(d => (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "B"));
                                    //balancingC = data.Count(d => (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "B" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C"));
                                    //balancingD = data.Count(d => d.upperGrade == "D" || d.lowerGrade == "D" || d.staticGrade == "D" || d.uplowGrade == "D");
                                    //balancingE = data.Count(d => (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "E") || (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "E" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "E"));

                                    

                                    BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                    BulgeB = data.Count(d => (d.TbulgeGrade == "B" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "B"));
                                    BulgeC = data.Count(d => (d.TbulgeGrade == "C" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "C"));
                                    BulgeD = data.Count(d => (d.TbulgeGrade == "D" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "D"));
                                    BulgeE = data.Count(d => (d.TbulgeGrade == "E" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "E"));


                                    DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                    DentB = data.Count(d => (d.TdentGrade == "B" && d.BdentGrade == "A") || (d.TdentGrade == "A" && d.BdentGrade == "B") || (d.TdentGrade == "B" && d.BdentGrade == "B"));
                                    DentC = data.Count(d => (d.TdentGrade == "C" && d.BdentGrade == "A") || (d.TdentGrade == "C" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "C") || (d.TdentGrade == "B" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "C"));
                                    DentD = data.Count(d => (d.TdentGrade == "D" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "D" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "D") || (d.TdentGrade == "B" && d.BdentGrade == "D"));
                                    DentE = data.Count(d => (d.TdentGrade == "E" && d.BdentGrade == "E") || (d.TdentGrade == "E" && d.BdentGrade == "D") || (d.TdentGrade == "E" && d.BdentGrade == "C") || (d.TdentGrade == "E" && d.BdentGrade == "B") || (d.TdentGrade == "E" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "E") || (d.TdentGrade == "C" && d.BdentGrade == "E") || (d.TdentGrade == "B" && d.BdentGrade == "E") || (d.TdentGrade == "A" && d.BdentGrade == "E"));

                                 
                                    total_ += total;
                                    finalyield_ += finalyield;
                                    runoutA_ += runoutA;
                                    runoutB_ += runoutB;
                                    runoutC_ += runoutC;
                                    runoutD_ += runoutD;
                                    runoutE_ += runoutE;
                                    balancingA_ += balancingA;
                                    balancingB_ += balancingB;
                                    balancingC_ += balancingC;
                                    balancingD_ += balancingD;
                                    balancingE_ += balancingE;
                                    BulgeA_ += BulgeA;
                                    BulgeB_ += BulgeB;
                                    BulgeC_ += BulgeC;
                                    BulgeD_ += BulgeD;
                                    BulgeE_ += BulgeE;
                                    DentA_ += DentA;
                                    DentB_ += DentB;
                                    DentC_ += DentC;
                                    DentD_ += DentD;
                                    DentE_ += DentE;

                                    ptotal += total;
                                    Pfinalyield += finalyield;
                                    prunoutA += runoutA;
                                    prunoutB += runoutB;
                                    prunoutC += runoutC;
                                    prunoutD += runoutD;
                                    prunoutE += runoutE;
                                    pbalancingA += balancingA;
                                    pbalancingB += balancingB;
                                    pbalancingC += balancingC;
                                    pbalancingD += balancingD;
                                    pbalancingE += balancingE;
                                    pBulgeA += BulgeA;
                                    pBulgeB += BulgeB;
                                    pBulgeC += BulgeC;
                                    pBulgeD += BulgeD;
                                    pBulgeE += BulgeE;
                                    pDentA += DentA;
                                    pDentB += DentB;
                                    pDentC += DentC;
                                    pDentD += DentD;
                                    pDentE += DentE;


                                    dr[0] = data[0].wcname;
                                    dr[1] = data[0].tyreType;
                                    dr[2] = total;
                                    dr[3] = finalyield;
                                    dr[4] = runoutA;
                                    dr[5] = runoutB;
                                    dr[6] = runoutC;
                                    dr[7] = runoutD;
                                    dr[8] = runoutE;
                                    dr[9] = DentA;
                                    dr[10] = DentB;
                                    dr[11] = DentC;
                                    dr[12] = DentD;
                                    dr[13] = DentE;
                                    dr[14] = BulgeA;
                                    dr[15] = BulgeB;
                                    dr[16] = BulgeC;
                                    dr[17] = BulgeD;
                                    dr[18] = BulgeE;
                                    dr[19] = balancingA;
                                    dr[20] = balancingB;
                                    dr[21] = balancingC;
                                    dr[22] = balancingD;
                                    dr[23] = balancingE;
                                    gridviewdt.Rows.Add(dr);
                                }
                                #endregion
                            }
                            drt = gridviewdt.NewRow();
                             drt[0] = wc_item.wc_id;
                        drt[1] = "Total"; drt[2] = ptotal;drt[3] = Pfinalyield; drt[4] = prunoutA; drt[5] = prunoutB; drt[6] = prunoutC; drt[7] = prunoutD;  drt[8] = prunoutE;
                       drt[9] = pDentA; drt[10] = pDentB; drt[11] = pDentC; drt[12] = pDentD; drt[13] = pDentE;
                        drt[14] = pBulgeA;drt[15] = pBulgeB; drt[16] = pBulgeC; drt[17] = pBulgeD; drt[18] = pBulgeE;
                        drt[19] = pbalancingA; drt[20] = pbalancingB; drt[21] = pbalancingC; drt[22] = pbalancingD; drt[23] = pbalancingE;
                        gridviewdt.Rows.Add(drt);


                            #endregion
                     
                    }

                    #endregion
                    


                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[2] = total_;
                    drt[3] = finalyield_;
                    drt[4] = runoutA_;
                    drt[5] = runoutB_;
                    drt[6] = runoutC_;
                    drt[7] = runoutD_;
                    drt[8] = runoutE_;

                    drt[9] = DentA_;
                    drt[10] = DentB_;
                    drt[11] = DentC_;
                    drt[12] = DentD_;
                    drt[13] = DentE_;

                    drt[14] = BulgeA_;
                    drt[15] = BulgeB_;
                    drt[16] = BulgeC_;
                    drt[17] = BulgeD_;
                    drt[18] = BulgeE_;


                    drt[19] = balancingA_;
                    drt[20] = balancingB_;
                    drt[21] = balancingC_;
                    drt[22] = balancingD_;
                    drt[23] = balancingE_;
                    gridviewdt.Rows.Add(drt);
                    break;
                    
                case "Percent":


                    #region TR01
                    foreach (var wc_item in wc_items)
                    {
                        ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;
                        var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("recipeCode"))
                            .Select(g => new
                            {
                                tyreType_id = g.Key
                            }));
                        var tyreType_items = tyreType_query.ToArray();
                        #region loop2
                        foreach (var tyreType_item in tyreType_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("wcName") == wc_item.wc_id && l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                            {
                                wcname = l.Field<string>("wcName"),
                                tyreType = l.Field<string>("recipeCode"),
                                RoRank = l.Field<string>("ROTotalRank"),
                                upperGrade = l.Field<string>("upperRank"),
                                lowerGrade = l.Field<string>("lowerRank"),
                                staticGrade = l.Field<string>("staticRank"),
                                uplowGrade = l.Field<string>("uploRank"),
                                TbulgeGrade = l.Field<string>("lroTbulgeRank"),
                                BbulgeGrade = l.Field<string>("lroBbulgeRank"),
                                TdentGrade = l.Field<string>("lroTdentRank"),
                                BdentGrade = l.Field<string>("lroBdentRank"),
                                TotalRank_balance = l.Field<string>("TotalRank")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                total = data.Count();
                                finalyield = data.Count(d => d.RoRank == "A" && d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A" && d.TbulgeGrade == "A" && d.BbulgeGrade == "A" && d.TdentGrade == "A" && d.BdentGrade == "A");

                                runoutA = data.Count(d => d.RoRank == "A");
                                runoutB = data.Count(d => d.RoRank == "B");
                                runoutC = data.Count(d => d.RoRank == "C");
                                runoutD = data.Count(d => d.RoRank == "D");
                                runoutE = data.Count(d => d.RoRank == "E");

                                //balancingA = data.Count(d => d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A");
                                //balancingB = data.Count(d => (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "B"));
                                //balancingC = data.Count(d => (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "B" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "C" && d.uplowGrade == "B") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "A" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "C"));
                                //balancingD = data.Count(d => d.upperGrade == "D" || d.lowerGrade == "D" || d.staticGrade == "D" || d.uplowGrade == "D");
                                //balancingE = data.Count(d => (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "E") || (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "B" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "B" && d.lowerGrade == "B" && d.staticGrade == "A" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "E" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "A" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "C" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "E" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "A" && d.staticGrade == "B" && d.uplowGrade == "E") || (d.upperGrade == "C" && d.lowerGrade == "E" && d.staticGrade == "E" && d.uplowGrade == "A") || (d.upperGrade == "A" && d.lowerGrade == "E" && d.staticGrade == "C" && d.uplowGrade == "C") || (d.upperGrade == "C" && d.lowerGrade == "C" && d.staticGrade == "B" && d.uplowGrade == "E"));

                                balancingA = data.Count(d => d.TotalRank_balance == "A");
                                balancingB = data.Count(d => d.TotalRank_balance == "B");
                                balancingC = data.Count(d => d.TotalRank_balance == "C");
                                balancingD = data.Count(d => d.TotalRank_balance == "D");
                                balancingE = data.Count(d => d.TotalRank_balance == "E");

   

                                BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                BulgeB = data.Count(d => (d.TbulgeGrade == "B" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "B"));
                                BulgeC = data.Count(d => (d.TbulgeGrade == "C" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "C"));
                                BulgeD = data.Count(d => (d.TbulgeGrade == "D" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "D"));
                                BulgeE = data.Count(d => (d.TbulgeGrade == "E" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "E"));


                                DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                DentB = data.Count(d => (d.TdentGrade == "B" && d.BdentGrade == "A") || (d.TdentGrade == "A" && d.BdentGrade == "B") || (d.TdentGrade == "B" && d.BdentGrade == "B"));
                                DentC = data.Count(d => (d.TdentGrade == "C" && d.BdentGrade == "A") || (d.TdentGrade == "C" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "C") || (d.TdentGrade == "B" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "C"));
                                DentD = data.Count(d => (d.TdentGrade == "D" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "D" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "D") || (d.TdentGrade == "B" && d.BdentGrade == "D"));
                                DentE = data.Count(d => (d.TdentGrade == "E" && d.BdentGrade == "E") || (d.TdentGrade == "E" && d.BdentGrade == "D") || (d.TdentGrade == "E" && d.BdentGrade == "C") || (d.TdentGrade == "E" && d.BdentGrade == "B") || (d.TdentGrade == "E" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "E") || (d.TdentGrade == "C" && d.BdentGrade == "E") || (d.TdentGrade == "B" && d.BdentGrade == "E") || (d.TdentGrade == "A" && d.BdentGrade == "E"));

                                total_ += total;
                                finalyield_ += finalyield;
                                runoutA_ += runoutA;
                                runoutB_ += runoutB;
                                runoutC_ += runoutC;
                                runoutD_ += runoutD;
                                runoutE_ += runoutE;
                                balancingA_ += balancingA;
                                balancingB_ += balancingB;
                                balancingC_ += balancingC;
                                balancingD_ += balancingD;
                                balancingE_ += balancingE;
                                BulgeA_ += BulgeA;
                                BulgeB_ += BulgeB;
                                BulgeC_ += BulgeC;
                                BulgeD_ += BulgeD;
                                BulgeE_ += BulgeE;
                                DentA_ += DentA;
                                DentB_ += DentB;
                                DentC_ += DentC;
                                DentD_ += DentD;
                                DentE_ += DentE;

                                ptotal += total;
                                Pfinalyield += finalyield;
                                prunoutA += runoutA;
                                prunoutB += runoutB;
                                prunoutC += runoutC;
                                prunoutD += runoutD;
                                prunoutE += runoutE;
                                pbalancingA += balancingA;
                                pbalancingB += balancingB;
                                pbalancingC += balancingC;
                                pbalancingD += balancingD;
                                pbalancingE += balancingE;
                                pBulgeA += BulgeA;
                                pBulgeB += BulgeB;
                                pBulgeC += BulgeC;
                                pBulgeD += BulgeD;
                                pBulgeE += BulgeE;
                                pDentA += DentA;
                                pDentB += DentB;
                                pDentC += DentC;
                                pDentD += DentD;
                                pDentE += DentE;
                                
                                dr[0] = data[0].wcname;
                                dr[1] = data[0].tyreType;
                                dr[2] = total;
                                dr[3] = (total != 0) ? Math.Round((finalyield * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[4] = (total != 0) ? Math.Round((runoutA * 100 /Convert.ToDouble(total)), 1)  :0.0;
                                dr[5] = (total != 0) ? Math.Round((runoutB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[6] = (total != 0) ? Math.Round((runoutC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[7] = (total != 0) ? Math.Round((runoutD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[8] = (total != 0) ? Math.Round((runoutE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[9] = (total != 0) ? Math.Round((DentA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[10] = (total != 0) ? Math.Round((DentB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[11] = (total != 0) ? Math.Round((DentC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[12] = (total != 0) ? Math.Round((DentD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[13] = (total != 0) ? Math.Round((DentE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[14] = (total != 0) ? Math.Round((BulgeA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[15] = (total != 0) ? Math.Round((BulgeB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[16] = (total != 0) ? Math.Round((BulgeC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[17] = (total != 0) ? Math.Round((BulgeD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[18] = (total != 0) ? Math.Round((BulgeE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[19] = (total != 0) ? Math.Round((balancingA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[20] = (total != 0) ? Math.Round((balancingB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[21] = (total != 0) ? Math.Round((balancingC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[22] = (total != 0) ? Math.Round((balancingD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[23] = (total != 0) ? Math.Round((balancingE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                gridviewdt.Rows.Add(dr);

                            }
                        }
                        #endregion

                        drt = gridviewdt.NewRow();
                        drt[0] = "Total";
                        drt[1] = "Total";
                        drt[2] = ptotal;
                        drt[3] = (ptotal != 0) ? Math.Round((Pfinalyield * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[4] = (ptotal != 0) ? Math.Round((prunoutA * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[5] = (ptotal != 0) ? Math.Round((prunoutB * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[6] = (ptotal != 0) ? Math.Round((prunoutC * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[7] = (ptotal != 0) ? Math.Round((prunoutD * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[8] = (ptotal != 0) ? Math.Round((prunoutE * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[9] = (ptotal != 0) ? Math.Round((pDentA * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[10] = (ptotal != 0) ? Math.Round((pDentB * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[11] = (ptotal != 0) ? Math.Round((pDentC * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[12] = (ptotal != 0) ? Math.Round((pDentD * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[13] = (ptotal != 0) ? Math.Round((pDentE * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[14] = (ptotal != 0) ? Math.Round((pBulgeA * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[15] = (ptotal != 0) ? Math.Round((pBulgeB * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[16] = (ptotal != 0) ? Math.Round((pBulgeC * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[17] = (ptotal != 0) ? Math.Round((pBulgeD * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[18] = (ptotal != 0) ? Math.Round((pBulgeE * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[19] = (ptotal != 0) ? Math.Round((pbalancingA * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[20] = (ptotal != 0) ? Math.Round((pbalancingB * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[21] = (ptotal != 0) ? Math.Round((pbalancingC * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[22] = (ptotal != 0) ? Math.Round((pbalancingD * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;
                        drt[23] = (ptotal != 0) ? Math.Round((pbalancingE * 100 / Convert.ToDouble(ptotal)), 1) : 0.0;


                        gridviewdt.Rows.Add(drt);
                    }
                    #endregion

                   

                    drt = gridviewdt.NewRow();
                    drt[0] = "";
                    drt[1] = "Total";
                    drt[2] = total_;
                    drt[3] = (total_ != 0) ? Math.Round((finalyield_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[4] = (total_ != 0) ? Math.Round((runoutA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[5] = (total_ != 0) ? Math.Round((runoutB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[6] = (total_ != 0) ? Math.Round((runoutC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[7] = (total_ != 0) ? Math.Round((runoutD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[8] = (total_ != 0) ? Math.Round((runoutE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[9] = (total_ != 0) ? Math.Round((DentA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[10] = (total_ != 0) ? Math.Round((DentB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[11] = (total_ != 0) ? Math.Round((DentC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[12] = (total_ != 0) ? Math.Round((DentD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[13] = (total_ != 0) ? Math.Round((DentE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[14] = (total_ != 0) ? Math.Round((BulgeA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[15] = (total_ != 0) ? Math.Round((BulgeB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[16] = (total_ != 0) ? Math.Round((BulgeC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[17] = (total_ != 0) ? Math.Round((BulgeD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[18] = (total_ != 0) ? Math.Round((BulgeE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[19] = (total_ != 0) ? Math.Round((balancingA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[20] = (total_ != 0) ? Math.Round((balancingB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[21] = (total_ != 0) ? Math.Round((balancingC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[22] = (total_ != 0) ? Math.Round((balancingD_ * 100 / Convert.ToDouble(total_)), 1) :0.0;
                    drt[23] = (total_ != 0) ? Math.Round((balancingE_ * 100 / Convert.ToDouble(total_)), 1): 0.0;
                    gridviewdt.Rows.Add(drt);
                    break;
                
            }
            performanceReportTUOWiseMainGridView.DataSource = gridviewdt;
            performanceReportTUOWiseMainGridView.DataBind();

            // Select the rows where showing total & make them bold
            IEnumerable<GridViewRow> rows = performanceReportTUOWiseMainGridView.Rows.Cast<GridViewRow>();
//.Where(row => row.Cells[0].Text == "Total" || row.Cells[1].Text=="Total");

            foreach (var row in rows)
            {
                Label aa = (Label)row.Cells[0].FindControl("performanceReportTUOWiseWCNameLabel");
                Label bb = (Label)row.Cells[1].FindControl("performanceReportTUOWiseTyreTypeLabel");
                if(aa.Text=="Total"|| bb.Text=="Total")
                row.Font.Bold = true;
            }

            
            
            
            exldt = gridviewdt.Clone();
           // exldt = gridviewdt.Copy();

            exldt.Columns[2].DataType = typeof(Double);
            exldt.Columns[3].DataType = typeof(Double);
            exldt.Columns[4].DataType = typeof(Double);
            exldt.Columns[5].DataType = typeof(Double);
            exldt.Columns[6].DataType = typeof(Double);
            exldt.Columns[7].DataType = typeof(Double);
            exldt.Columns[8].DataType = typeof(Double);
            exldt.Columns[9].DataType = typeof(Double);
            exldt.Columns[10].DataType = typeof(Double);
            exldt.Columns[11].DataType = typeof(Double);
            exldt.Columns[12].DataType = typeof(Double);
            exldt.Columns[13].DataType = typeof(Double);
            exldt.Columns[14].DataType = typeof(Double);
            exldt.Columns[15].DataType = typeof(Double);
            exldt.Columns[16].DataType = typeof(Double);
            exldt.Columns[17].DataType = typeof(Double);
            exldt.Columns[18].DataType = typeof(Double);
            exldt.Columns[19].DataType = typeof(Double);
            exldt.Columns[20].DataType = typeof(Double);
            exldt.Columns[21].DataType = typeof(Double);
            exldt.Columns[22].DataType = typeof(Double);
            exldt.Columns[23].DataType = typeof(Double);
            exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);


            ViewState["xmldt"] = null;
            ViewState.Remove("xmldt");
            ViewState["xmldt"] = exldt;


        }        
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            showData();
        }
        private void showData()
        {
            getdisplaytype = optionDropDownList.SelectedItem.Text;

            rFromDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rToDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            if (!string.IsNullOrEmpty(rToDate) && !string.IsNullOrEmpty(rFromDate))
            {
                string dtnadtime = TotalprodataformatDate(rFromDate, rToDate);

                if (QualityReportTUOWise.Checked)
                {
                    loadWCData(dtnadtime);
                    //loadWCData1(dtnadtime);
                    showQualityReportWise();
                    QualityReportTUOWisePanel.Visible = true;
                    QualityReportRecipeTUOWisePanel.Visible = false;
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    loadWCData(dtnadtime);
                    //loadWCData1(dtnadtime);
                    showQualityRecipeReportWise();
                    QualityReportTUOWisePanel.Visible = false;
                    QualityReportRecipeTUOWisePanel.Visible = true;
                }
                Label1.Text = "false";
            }
        }
        private void loadRecipeData(string dtnadtime1)
        {
            try
            {
                DataTable recipe_dt = new DataTable();
                DataTable tuo_dt = new DataTable();

                dt.Columns.Add("tireType", typeof(string));
                dt.Columns.Add("uniformityGrade", typeof(string));
            
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name, tyreSize, tyreDesign from recipeMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                recipe_dt.Load(myConnection.reader);

                myConnection.comm.CommandText = "select tireType, uniformityGrade from productiondatatuo WHERE ((testTime>" + dtnadtime1;
                myConnection.reader = myConnection.comm.ExecuteReader();
                tuo_dt.Load(myConnection.reader);

                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    dt = tuo_dt.Copy(); ;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text && r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
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
        private void showQualityRecipeReportWise()
        {
            DataTable gridviewdt = new DataTable();        
            gridviewdt.Columns.Add("recipeCode", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("finalYield", typeof(string));
            gridviewdt.Columns.Add("RunoutA", typeof(string));
            gridviewdt.Columns.Add("RunoutB", typeof(string));
            gridviewdt.Columns.Add("RunoutC", typeof(string));
            gridviewdt.Columns.Add("RunoutD", typeof(string));
            gridviewdt.Columns.Add("RunoutE", typeof(string));
            gridviewdt.Columns.Add("DentA", typeof(string));
            gridviewdt.Columns.Add("DentB", typeof(string));
            gridviewdt.Columns.Add("DentC", typeof(string));
            gridviewdt.Columns.Add("DentD", typeof(string));
            gridviewdt.Columns.Add("DentE", typeof(string));
            gridviewdt.Columns.Add("BulgeA", typeof(string));
            gridviewdt.Columns.Add("BulgeB", typeof(string));
            gridviewdt.Columns.Add("BulgeC", typeof(string));
            gridviewdt.Columns.Add("BulgeD", typeof(string));
            gridviewdt.Columns.Add("BulgeE", typeof(string));
            gridviewdt.Columns.Add("BalancingA", typeof(string));
            gridviewdt.Columns.Add("BalancingB", typeof(string));
            gridviewdt.Columns.Add("BalancingC", typeof(string));
            gridviewdt.Columns.Add("BalancingD", typeof(string));
            gridviewdt.Columns.Add("BalancingE", typeof(string));

            DataRow drt;
            int total = 0,  finalYield=0,runoutA = 0, runoutB = 0, runoutC = 0, runoutD = 0, runoutE = 0, DentA = 0, DentB = 0, DentC = 0, DentD = 0, DentE = 0, BulgeA = 0, BulgeB = 0, BulgeC = 0, BulgeD = 0, BulgeE = 0, balancingA = 0, balancingB = 0, balancingC = 0, balancingD = 0, balancingE = 0;
            Double ptotal = 0,pfinalYield=0, prunoutA = 0, prunoutB = 0, prunoutC = 0, prunoutD = 0, prunoutE = 0, pDentA = 0, pDentB = 0, pDentC = 0, pDentD = 0, pDentE = 0, pBulgeA = 0, pBulgeB = 0, pBulgeC = 0, pBulgeD = 0, pBulgeE = 0, pbalancingA = 0, pbalancingB = 0, pbalancingC = 0, pbalancingD = 0, pbalancingE = 0;
            total_ = 0; finalyield_=0; runoutA_ = 0; runoutB_ = 0; runoutC_ = 0; runoutD_ = 0; runoutE_ = 0; DentA_ = 0; DentB_ = 0; DentC_ = 0; DentD_ = 0; DentE_ = 0; BulgeA_ = 0; BulgeB_ = 0; BulgeC_ = 0; BulgeD_ = 0; BulgeE_ = 0; balancingA_ = 0; balancingB_ = 0; balancingC_ = 0; balancingD_ = 0; balancingE_ = 0;


            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);
            var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("recipeCode"))
                           .Select(g => new
                           {
                               tyreType_id = g.Key
                           }));
            var tyreType_items = tyreType_query.ToArray();
         
            switch (getdisplaytype)
            {
                case "Numbers":
                    foreach (var tyreType_item in tyreType_items)
                    {
                        ptotal = 0; prunoutA = 0; prunoutB = 0; prunoutC = 0; prunoutD = 0; prunoutE = 0; pDentA = 0; pDentB = 0; pDentC = 0; pDentD = 0; pDentE = 0; pBulgeA = 0; pBulgeB = 0; pBulgeC = 0; pBulgeD = 0; pBulgeE = 0; pbalancingA = 0; pbalancingB = 0; pbalancingC = 0; pbalancingD = 0; pbalancingE = 0;
                       
                      
                            var data = dt.AsEnumerable().Where(l =>l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                            {
                                tyreType = l.Field<string>("recipeCode"),
                                RoRank = l.Field<string>("ROTotalRank"),
                                upperGrade = l.Field<string>("upperRank"),
                                lowerGrade = l.Field<string>("lowerRank"),
                                staticGrade = l.Field<string>("staticRank"),
                                uplowGrade = l.Field<string>("uploRank"),
                                TbulgeGrade = l.Field<string>("lroTbulgeRank"),
                                BbulgeGrade = l.Field<string>("lroBbulgeRank"),
                                TdentGrade = l.Field<string>("lroTdentRank"),
                                BdentGrade = l.Field<string>("lroBdentRank"),
                                TotalRank_balance = l.Field<string>("TotalRank")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                total = data.Count();
                                finalYield = data.Count(d => d.RoRank == "A" && d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A" && d.TbulgeGrade == "A" && d.BbulgeGrade == "A" && d.TdentGrade == "A" && d.BdentGrade == "A");

                                runoutA = data.Count(d => d.RoRank == "A");
                                runoutB = data.Count(d => d.RoRank == "B");
                                runoutC = data.Count(d => d.RoRank == "C");
                                runoutD = data.Count(d => d.RoRank == "D");
                                runoutE = data.Count(d => d.RoRank == "E");
                                //balancingA = data.Count(d => d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A");
                                //balancingB = data.Count(d => d.upperGrade == "B" || d.lowerGrade == "B" || d.staticGrade == "B" || d.uplowGrade == "B");
                                //balancingC = data.Count(d => d.upperGrade == "C" || d.lowerGrade == "C" || d.staticGrade == "C" || d.uplowGrade == "C");
                                //balancingD = data.Count(d => d.upperGrade == "D" || d.lowerGrade == "D" || d.staticGrade == "D" || d.uplowGrade == "D");
                                //balancingE = data.Count(d => d.upperGrade == "E" || d.lowerGrade == "E" || d.staticGrade == "E" || d.uplowGrade == "E");

                                //BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                //BulgeB = data.Count(d => d.TbulgeGrade == "B" || d.BbulgeGrade == "B");
                                //BulgeC = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");
                                //BulgeD = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");
                                //BulgeE = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");

                                //DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                //DentB = data.Count(d => d.TdentGrade == "B" || d.BdentGrade == "B");
                                //DentC = data.Count(d => d.TdentGrade == "C" || d.BdentGrade == "C");
                                //DentD = data.Count(d => d.TdentGrade == "D" || d.BdentGrade == "D");
                                //DentE = data.Count(d => d.TdentGrade == "E" || d.BdentGrade == "E");

                                balancingA = data.Count(d => d.TotalRank_balance == "A");
                                balancingB = data.Count(d => d.TotalRank_balance == "B");
                                balancingC = data.Count(d => d.TotalRank_balance == "C");
                                balancingD = data.Count(d => d.TotalRank_balance == "D");
                                balancingE = data.Count(d => d.TotalRank_balance == "E");



                                BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                BulgeB = data.Count(d => (d.TbulgeGrade == "B" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "B"));
                                BulgeC = data.Count(d => (d.TbulgeGrade == "C" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "C"));
                                BulgeD = data.Count(d => (d.TbulgeGrade == "D" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "D"));
                                BulgeE = data.Count(d => (d.TbulgeGrade == "E" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "E"));


                                DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                DentB = data.Count(d => (d.TdentGrade == "B" && d.BdentGrade == "A") || (d.TdentGrade == "A" && d.BdentGrade == "B") || (d.TdentGrade == "B" && d.BdentGrade == "B"));
                                DentC = data.Count(d => (d.TdentGrade == "C" && d.BdentGrade == "A") || (d.TdentGrade == "C" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "C") || (d.TdentGrade == "B" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "C"));
                                DentD = data.Count(d => (d.TdentGrade == "D" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "D" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "D") || (d.TdentGrade == "B" && d.BdentGrade == "D"));
                                DentE = data.Count(d => (d.TdentGrade == "E" && d.BdentGrade == "E") || (d.TdentGrade == "E" && d.BdentGrade == "D") || (d.TdentGrade == "E" && d.BdentGrade == "C") || (d.TdentGrade == "E" && d.BdentGrade == "B") || (d.TdentGrade == "E" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "E") || (d.TdentGrade == "C" && d.BdentGrade == "E") || (d.TdentGrade == "B" && d.BdentGrade == "E") || (d.TdentGrade == "A" && d.BdentGrade == "E"));


                                total_ += total;
                                finalyield_ += finalYield;
                                runoutA_ += runoutA;
                                runoutB_ += runoutB;
                                runoutC_ += runoutC;
                                runoutD_ += runoutD;
                                runoutE_ += runoutE;
                                balancingA_ += balancingA;
                                balancingB_ += balancingB;
                                balancingC_ += balancingC;
                                balancingD_ += balancingD;
                                balancingE_ += balancingE;
                                BulgeA_ += BulgeA;
                                BulgeB_ += BulgeB;
                                BulgeC_ += BulgeC;
                                BulgeD_ += BulgeD;
                                BulgeE_ += BulgeE;
                                DentA_ += DentA;
                                DentB_ += DentB;
                                DentC_ += DentC;
                                DentD_ += DentD;
                                DentE_ += DentE;

                               

                             
                                dr[0] = data[0].tyreType;
                                dr[1] = total;
                                dr[2] = finalYield;
                                dr[3] = runoutA;
                                dr[4] = runoutB;
                                dr[5] = runoutC;
                                dr[6] = runoutD;
                                dr[7] = runoutE;
                                dr[8] = DentA;
                                dr[9] = DentB;
                                dr[10] = DentC;
                                dr[11] = DentD;
                                dr[12] = DentE;
                                dr[13] = BulgeA;
                                dr[14] = BulgeB;
                                dr[15] = BulgeC;
                                dr[16] = BulgeD;
                                dr[17] = BulgeE;
                                dr[18] = balancingA;
                                dr[19] = balancingB;
                                dr[20] = balancingC;
                                dr[21] = balancingD;
                                dr[22] = balancingE;
                                gridviewdt.Rows.Add(dr);

                            }
                        
                       
                    }
                    drt = gridviewdt.NewRow();
                
                    drt[0] = "Total";
                    drt[1] = total_;
                    drt[2] = finalyield_;
                    drt[3] = runoutA_;
                    drt[4] = runoutB_;
                    drt[5] = runoutC_;
                    drt[6] = runoutD_;
                    drt[7] = runoutE_;

                    drt[8] = DentA_;
                    drt[9] = DentB_;
                    drt[10] = DentC_;
                    drt[11] = DentD_;
                    drt[12] = DentE_;

                    drt[13] = BulgeA_;
                    drt[14] = BulgeB_;
                    drt[15] = BulgeC_;
                    drt[16] = BulgeD_;
                    drt[17] = BulgeE_;


                    drt[18] = balancingA_;
                    drt[19] = balancingB_;
                    drt[20] = balancingC_;
                    drt[21] = balancingD_;
                    drt[22] = balancingE_;
                    gridviewdt.Rows.Add(drt);
                    break;
                case "Percent":
                    foreach (var tyreType_item in tyreType_items)
                    {
                                            
                            var data = dt.AsEnumerable().Where(l =>l.Field<string>("recipeCode") == tyreType_item.tyreType_id).Select(l => new
                            {
                                tyreType = l.Field<string>("recipeCode"),
                                RoRank = l.Field<string>("ROTotalRank"),
                                upperGrade = l.Field<string>("upperRank"),
                                lowerGrade = l.Field<string>("lowerRank"),
                                staticGrade = l.Field<string>("staticRank"),
                                uplowGrade = l.Field<string>("uploRank"),
                                TbulgeGrade = l.Field<string>("lroTbulgeRank"),
                                BbulgeGrade = l.Field<string>("lroBbulgeRank"),
                                TdentGrade = l.Field<string>("lroTdentRank"),
                                BdentGrade = l.Field<string>("lroBdentRank"),
                                TotalRank_balance = l.Field<string>("TotalRank")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                total = data.Count();
                                finalYield = data.Count(d => d.RoRank == "A" && d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A" && d.TbulgeGrade == "A" && d.BbulgeGrade == "A" && d.TdentGrade == "A" && d.BdentGrade == "A");

                                runoutA = data.Count(d => d.RoRank == "A");
                                runoutB = data.Count(d => d.RoRank == "B");
                                runoutC = data.Count(d => d.RoRank == "C");
                                runoutD = data.Count(d => d.RoRank == "D");
                                runoutE = data.Count(d => d.RoRank == "E");
                                //balancingA = data.Count(d => d.upperGrade == "A" && d.lowerGrade == "A" && d.staticGrade == "A" && d.uplowGrade == "A");
                                //balancingB = data.Count(d => d.upperGrade == "B" || d.lowerGrade == "B" || d.staticGrade == "B" || d.uplowGrade == "B");
                                //balancingC = data.Count(d => d.upperGrade == "C" || d.lowerGrade == "C" || d.staticGrade == "C" || d.uplowGrade == "C");
                                //balancingD = data.Count(d => d.upperGrade == "D" || d.lowerGrade == "D" || d.staticGrade == "D" || d.uplowGrade == "D");
                                //balancingE = data.Count(d => d.upperGrade == "E" || d.lowerGrade == "E" || d.staticGrade == "E" || d.uplowGrade == "E");

                                //BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                //BulgeB = data.Count(d => d.TbulgeGrade == "B" || d.BbulgeGrade == "B");
                                //BulgeC = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");
                                //BulgeD = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");
                                //BulgeE = data.Count(d => d.TbulgeGrade == "C" || d.BbulgeGrade == "C");

                                //DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                //DentB = data.Count(d => d.TdentGrade == "B" || d.BdentGrade == "B");
                                //DentC = data.Count(d => d.TdentGrade == "C" || d.BdentGrade == "C");
                                //DentD = data.Count(d => d.TdentGrade == "D" || d.BdentGrade == "D");
                                //DentE = data.Count(d => d.TdentGrade == "E" || d.BdentGrade == "E");

                                balancingA = data.Count(d => d.TotalRank_balance == "A");
                                balancingB = data.Count(d => d.TotalRank_balance == "B");
                                balancingC = data.Count(d => d.TotalRank_balance == "C");
                                balancingD = data.Count(d => d.TotalRank_balance == "D");
                                balancingE = data.Count(d => d.TotalRank_balance == "E");



                                BulgeA = data.Count(d => d.TbulgeGrade == "A" && d.BbulgeGrade == "A");
                                BulgeB = data.Count(d => (d.TbulgeGrade == "B" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "B"));
                                BulgeC = data.Count(d => (d.TbulgeGrade == "C" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "C"));
                                BulgeD = data.Count(d => (d.TbulgeGrade == "D" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "D"));
                                BulgeE = data.Count(d => (d.TbulgeGrade == "E" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "D") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "C") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "B") || (d.TbulgeGrade == "E" && d.BbulgeGrade == "A") || (d.TbulgeGrade == "D" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "C" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "B" && d.BbulgeGrade == "E") || (d.TbulgeGrade == "A" && d.BbulgeGrade == "E"));


                                DentA = data.Count(d => d.TdentGrade == "A" && d.BdentGrade == "A");
                                DentB = data.Count(d => (d.TdentGrade == "B" && d.BdentGrade == "A") || (d.TdentGrade == "A" && d.BdentGrade == "B") || (d.TdentGrade == "B" && d.BdentGrade == "B"));
                                DentC = data.Count(d => (d.TdentGrade == "C" && d.BdentGrade == "A") || (d.TdentGrade == "C" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "C") || (d.TdentGrade == "B" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "C"));
                                DentD = data.Count(d => (d.TdentGrade == "D" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "D" && d.BdentGrade == "C") || (d.TdentGrade == "C" && d.BdentGrade == "D") || (d.TdentGrade == "D" && d.BdentGrade == "B") || (d.TdentGrade == "A" && d.BdentGrade == "D") || (d.TdentGrade == "B" && d.BdentGrade == "D"));
                                DentE = data.Count(d => (d.TdentGrade == "E" && d.BdentGrade == "E") || (d.TdentGrade == "E" && d.BdentGrade == "D") || (d.TdentGrade == "E" && d.BdentGrade == "C") || (d.TdentGrade == "E" && d.BdentGrade == "B") || (d.TdentGrade == "E" && d.BdentGrade == "A") || (d.TdentGrade == "D" && d.BdentGrade == "E") || (d.TdentGrade == "C" && d.BdentGrade == "E") || (d.TdentGrade == "B" && d.BdentGrade == "E") || (d.TdentGrade == "A" && d.BdentGrade == "E"));


                                total_ += total;
                                finalyield_ += finalYield;
                                runoutA_ += runoutA;
                                runoutB_ += runoutB;
                                runoutC_ += runoutC;
                                runoutD_ += runoutD;
                                runoutE_ += runoutE;
                                balancingA_ += balancingA;
                                balancingB_ += balancingB;
                                balancingC_ += balancingC;
                                balancingD_ += balancingD;
                                balancingE_ += balancingE;
                                BulgeA_ += BulgeA;
                                BulgeB_ += BulgeB;
                                BulgeC_ += BulgeC;
                                BulgeD_ += BulgeD;
                                BulgeE_ += BulgeE;
                                DentA_ += DentA;
                                DentB_ += DentB;
                                DentC_ += DentC;
                                DentD_ += DentD;
                                DentE_ += DentE;
                  
                                dr[0] = data[0].tyreType;
                                dr[1] = total;
                                dr[2] = (total != 0) ? Math.Round((finalYield * 100 / Convert.ToDouble(total)), 1) :0.0;

                                dr[3] = (total != 0) ? Math.Round((runoutA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[4] = (total != 0) ? Math.Round((runoutB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[5] = (total != 0) ? Math.Round((runoutC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[6] = (total != 0) ? Math.Round((runoutD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[7] = (total != 0) ? Math.Round((runoutE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[8] = (total != 0) ? Math.Round((DentA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[9] = (total != 0) ? Math.Round((DentB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[10] = (total != 0) ? Math.Round((DentC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[11] = (total != 0) ? Math.Round((DentD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[12] = (total != 0) ? Math.Round((DentE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[13] = (total != 0) ? Math.Round((BulgeA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[14] = (total != 0) ? Math.Round((BulgeB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[15] = (total != 0) ? Math.Round((BulgeC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[16] = (total != 0) ? Math.Round((BulgeD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[17] = (total != 0) ? Math.Round((BulgeE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[18] = (total != 0) ? Math.Round((balancingA * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[19] = (total != 0) ? Math.Round((balancingB * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[20] = (total != 0) ? Math.Round((balancingC * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[21] = (total != 0) ? Math.Round((balancingD * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                dr[22] = (total != 0) ? Math.Round((balancingE * 100 / Convert.ToDouble(total)), 1) : 0.0;
                                gridviewdt.Rows.Add(dr);

                            }
                        }
                         
                    drt = gridviewdt.NewRow();
                  
                    drt[0] = "Total";
                    drt[1] = total_;
                    drt[2] = (total_ != 0) ? Math.Round((finalyield_ * 100 / Convert.ToDouble(total_)), 1):0.0;
                    drt[3] = (total_ != 0) ? Math.Round((runoutA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[4] = (total_ != 0) ? Math.Round((runoutB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[5] = (total_ != 0) ? Math.Round((runoutC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[6] = (total_ != 0) ? Math.Round((runoutD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[7] = (total_ != 0) ? Math.Round((runoutE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[8] = (total_ != 0) ? Math.Round((DentA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[9] = (total_ != 0) ? Math.Round((DentB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[10] = (total_ != 0) ? Math.Round((DentC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[11] = (total_ != 0) ? Math.Round((DentD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[12] = (total_ != 0) ? Math.Round((DentE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[13] = (total_ != 0) ? Math.Round((BulgeA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[14] = (total_ != 0) ? Math.Round((BulgeB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[15] = (total_ != 0) ? Math.Round((BulgeC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[16] = (total_ != 0) ? Math.Round((BulgeD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[17] = (total_ != 0) ? Math.Round((BulgeE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[18] = (total_ != 0) ? Math.Round((balancingA_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[19] = (total_ != 0) ? Math.Round((balancingB_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[20] = (total_ != 0) ? Math.Round((balancingC_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[21] = (total_ != 0) ? Math.Round((balancingD_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;
                    drt[22] = (total_ != 0) ? Math.Round((balancingE_ * 100 / Convert.ToDouble(total_)), 1) : 0.0;

                    gridviewdt.Rows.Add(drt);
                    break;
            }

            performanceReportRecipeTUOWiseGridView.DataSource = gridviewdt;
            performanceReportRecipeTUOWiseGridView.DataBind();

            exldt = gridviewdt.Clone();

            exldt.Columns[1].DataType = typeof(Double);
            exldt.Columns[2].DataType = typeof(Double);
            exldt.Columns[3].DataType = typeof(Double);
            exldt.Columns[4].DataType = typeof(Double);
            exldt.Columns[5].DataType = typeof(Double);
            exldt.Columns[6].DataType = typeof(Double);
            exldt.Columns[7].DataType = typeof(Double);
            exldt.Columns[8].DataType = typeof(Double);
            exldt.Columns[9].DataType = typeof(Double);
            exldt.Columns[10].DataType = typeof(Double);
            exldt.Columns[11].DataType = typeof(Double);
            exldt.Columns[12].DataType = typeof(Double);
            exldt.Columns[13].DataType = typeof(Double);
            exldt.Columns[14].DataType = typeof(Double);
            exldt.Columns[15].DataType = typeof(Double);
            exldt.Columns[16].DataType = typeof(Double);
            exldt.Columns[17].DataType = typeof(Double);
            exldt.Columns[18].DataType = typeof(Double);
            exldt.Columns[19].DataType = typeof(Double);
            exldt.Columns[20].DataType = typeof(Double);
            exldt.Columns[21].DataType = typeof(Double);
            exldt.Columns[22].DataType = typeof(Double);
            exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);

            ViewState["xmldt"] = null;
            ViewState.Remove("xmldt");
            ViewState["xmldt"] = exldt;

           
        }        
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {   
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceReportTUOWiseMainGridView")
                {
                    //Label cell;
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseCheckedLabel"));
                    //cell.Text = NullIf(cell.Text);
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseAGradeLabel"));
                    //cell.Text = NullIf(cell.Text);
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseBGradeLabel"));
                    //cell.Text = NullIf(cell.Text);
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseCGradeLabel"));
                    //cell.Text = NullIf(cell.Text);
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseDGradeLabel"));
                    //cell.Text = NullIf(cell.Text);
                    //cell = ((Label)e.Row.FindControl("performanceReportTUOWiseEGradeLabel"));
                    //cell.Text = NullIf(cell.Text);

                    for (int rowIndex = performanceReportTUOWiseMainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = performanceReportTUOWiseMainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = performanceReportTUOWiseMainGridView.Rows[rowIndex + 1];
                        if (((Label)performanceReportTUOWiseMainGridView.Rows[rowIndex].FindControl("performanceReportTUOWiseWCNameLabel")).Text == ((Label)performanceReportTUOWiseMainGridView.Rows[rowIndex + 1].FindControl("performanceReportTUOWiseWCNameLabel")).Text)
                        {
                            if (gvPreviousRow.Cells[0].RowSpan < 2)
                            {
                                gvRow.Cells[0].RowSpan = 2;
                            }
                            else
                            {
                                gvRow.Cells[0].RowSpan = gvPreviousRow.Cells[0].RowSpan + 1;
                            }
                            gvPreviousRow.Cells[0].Visible = false;
                        }
                    }
                }
                if (((GridView)sender).ID == "performanceReportRecipeTUOWiseGridView")
                {
                    if (string.IsNullOrEmpty(((Label)e.Row.FindControl("performanceReportRecipeTUOWiseTyreTypeLabel")).Text))
                        e.Row.Visible = false;
                    
                    /*Label cell;
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseCheckedLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseAGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseBGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseCGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseDGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseEGradeLabel"));
                    cell.Text = NullIf(cell.Text);*/
                }
            }
        }
        private void fillBarCodeDetailGridView(string wcname, string recipecode)
        {
            List<string> conditions = new List<string>();
            string Query = "";

            if (recipecode != "Total" && recipecode != "")
                conditions.Add("tireType='" + recipecode + "'");
            if (wcname != "Total")
                conditions.Add("machinename='" + wcname + "'");

            if (conditions.Any())
                Query = " AND " + string.Join(" AND ", conditions.ToArray());
            
            /*if (wcname != "Total" && recipecode != "Total"  && recipecode != "")
                recipeQuery = " and ";
            if (recipecode != "Total" && recipecode != "")
                recipeQuery += "tireType='" + recipecode + "' and";
            else if (wcname != "Total")
                recipeQuery = " and ";
            if (wcname != "Total")
                wcname = "machinename='" + wcname + "'";
            else
                wcname = "";*/
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E' AND ((testtime>=" + TotalprodataformatDate(rToDate, rFromDate) + Query + "";

            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void    fillBarCodeDetailGridView(string recipecode)
        {
            if (recipecode != "Total")
                recipecode = " and tireType='" + recipecode + "'";
            else
                recipecode = "";
            
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,TireType,barCode from vproductiondataTUO where uniformitygrade='E' " + recipecode + " and ((testtime>'" + TotalprodataformatDate(rToDate, rFromDate);

            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillBarCodeDetailGridView()
        {
            string query = "select wcname,machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E'  and ((testtime>" + TotalprodataformatDate(rToDate, rFromDate);
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
            showData();
            QualityReportTUOWisePanel.Visible = false;
            QualityReportRecipeTUOWisePanel.Visible = true;
            showDownload.Text = "";
        }
        protected void QualityReportTUOWise_CheckedChanged(object sender, EventArgs e)
        {            
            performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

            performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
            QualityReportTUOWisePanel.Visible = true;
            QualityReportRecipeTUOWisePanel.Visible = false;
            showData();
        }
        public string TotalformatDate(string fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string fromday, frommonth, fromyear, today, tomonth, toyear;

            string[] tempfromDate = fromDate.Split(new char[] { '-' });
            string[] temptoDate = toDate.Split(new char[] { '-' });
            try
            {
                fromday = tempfromDate[1].ToString().Trim();
                frommonth = tempfromDate[0].ToString().Trim();
                fromyear = tempfromDate[2].ToString().Trim();
                today = temptoDate[1].ToString().Trim();
                tomonth = temptoDate[0].ToString().Trim();
                toyear = temptoDate[2].ToString().Trim();

                flag1 = frommonth + "-" + fromday + "-" + fromyear + " 07:00:00";
                flag2 = tomonth + "-" + today + "-" + toyear + " 06:59:59";
                /*if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                {
                    flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }
                else
                {
                    flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }*/

                flag = "testTime>='" + flag1 + "' and " + "testTime<='" + flag2 + "'";

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
                myConnection.comm.CommandTimeout = 0;
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

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " where processID=4";

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
                showData();

            }
            else if (ddl.ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
                showData();
            }


        }
        private string TotalprodataformatDate(String fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string fday, fmonth, fyear;
            int tday, tmonth, tyear;

            string[] ftempDate = fromDate.Split(new char[] { '-' });
            string[] ttempDate = toDate.Split(new char[] { '-' });

            try
            {
                fday = ftempDate[1].ToString().Trim();
                fmonth = ftempDate[0].ToString().Trim();
                fyear = ftempDate[2].ToString().Trim();
                tday = Convert.ToInt32(ttempDate[1].ToString().Trim());
                tmonth = Convert.ToInt32(ttempDate[0].ToString().Trim());
                tyear = Convert.ToInt32(ttempDate[2].ToString().Trim());

                flag = "'" + fyear + "-" + fmonth + "-" + fday + " 06:59:59' AND dtandTime<='" + formatToDate(tday, tmonth, tyear) + "'))";

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
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
        private string formatToDate(int day, int month, int year)
        {
            string showToDate = "";
            if (month == 12 && day == 31)
                showToDate = (year + 1) + "-01-01";
            else if (day == 31 && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10))
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else if (day == 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                showToDate = year.ToString() + "-" + (checkDigit(month + 1)) + "-01";
            else if (month == 2 && (year == 2016) && (day == 29))
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else if (month == 2 && (day == 28) && year != 2016)
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else
                showToDate = year + "-" + checkDigit(month) + "-" + checkDigit((day + 1));
            return showToDate + " 06:59:59";
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
        protected void expToExcel_Click(object sender, EventArgs e)
        {

            getdisplaytype = optionDropDownList.SelectedItem.Text;

           DataTable dt = (DataTable)ViewState["xmldt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=RunoutReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Runoutreport");
            ws.Cells["A1"].Value = "Runout performance Report date Time from"+reportMasterFromDateTextBox.Text+ "TO "+reportMasterToDateTextBox.Text;

            using (ExcelRange r = ws.Cells["A1:w1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }


            ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["xmldt"], true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();

            if (QualityReportTUOWise.Checked)
            {
                if (getdisplaytype == "Numbers")
                {
                    // runout yield chart
                    var pieChart = ws.Drawings.AddChart("Runout_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2

                    var colcount = ws.Dimension.End.Column;
                    var rowCnt = ws.Dimension.End.Row;
            
                    pieChart.SetPosition(rowCnt+2, 0, 0, 0);
                    pieChart.SetSize(400, 400);
                   
                    var ser1 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                          ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'C3");
                    var ser2 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["D4:D" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
             
                    
                    pieChart.Title.Text = "Over All A Grade Yield Size Wise";
                    pieChart.ShowDataLabelsOverMaximum = true;


                    var pieChartrunout = ws.Drawings.AddChart("RunoutRecipeWise_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChartrunout.SetPosition(rowCnt + 2, 0, 5, 0);
                    pieChartrunout.SetSize(400, 400);
                
                    var Runcolcount = ws.Dimension.End.Column;
                    var RunrowCnt = ws.Dimension.End.Row;
                    var serrun = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                          ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    serrun.HeaderAddress = new ExcelAddress("'Runoutreport'J3");
                    var serrun2 = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["E4:E" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                   pieChartrunout.Title.Text = "Runout Size Wise Yield";
                   // pieChartrunout.ShowDataLabelsOverMaximum = true;
                    //pieChartrunout.UseSecondaryAxis = true;
                    pieChartrunout.YAxis.Deleted = true;
              


                    // dent yield chart
                    var pieChart1 = ws.Drawings.AddChart("Dent_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart1.SetPosition(rowCnt + 2, 0, 12, 0);
                    pieChart1.SetSize(400, 400);
                    var colcount1 = ws.Dimension.End.Column;
                    var rowCnt1 = ws.Dimension.End.Row;
                    var ser11 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                          ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'J3");
                    var ser21 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["J4:J" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart1.Title.Text = "Dent Size Wise Yield";
                    pieChart1.ShowDataLabelsOverMaximum = true;


                    var pieChart2 = ws.Drawings.AddChart("Bulge_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart2.SetPosition(rowCnt + 2, 0, 19, 0);
                    pieChart2.SetSize(400, 400);
                    var colcount2 = ws.Dimension.End.Column;
                    var rowCnt2 = ws.Dimension.End.Row;
                    var ser12 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                          ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'O3");
                    var ser22 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["O4:O" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart2.Title.Text = "Bulge Size Wise Yield";
                    pieChart2.ShowDataLabelsOverMaximum = true;


                    var pieChart3 = ws.Drawings.AddChart("balancing_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    //pieChart3.SetPosition((rowCnt + 4)*2, 0, 0, 0);
                    pieChart3.SetPosition(rowCnt + 2, 0, 24, 0);
                    pieChart3.SetSize(400, 400);
                    var colcount3 = ws.Dimension.End.Column;
                    var rowCnt3 = ws.Dimension.End.Row;
                    var ser13 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                          ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'T3");
                    var ser23 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["T4:T" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart3.Title.Text = "Balancing Size Wise Yield";
                    pieChart3.ShowDataLabelsOverMaximum = true;
                }
                else if (getdisplaytype == "Percent")
                {

                    // runout yield chart
                    var pieChart = ws.Drawings.AddChart("Runout_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart.SetPosition(17, 0, 0, 0);
                    pieChart.SetSize(400, 400);
                    var colcount = ws.Dimension.End.Column;
                    var rowCnt = ws.Dimension.End.Row;
                  
                    var ser2 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["D4:D" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart.Title.Text = "Over All A Grade Yield Size Wise";
                    pieChart.ShowDataLabelsOverMaximum = true;


                    var pieChartrunout = ws.Drawings.AddChart("RunoutRecipeWise_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChartrunout.SetPosition(rowCnt + 2, 0, 5, 0);
                    pieChartrunout.SetSize(400, 400);
                    var Runcolcount = ws.Dimension.End.Column;
                    var RunrowCnt = ws.Dimension.End.Row;
                   
                    var serrun2 = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["E4:E" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChartrunout.Title.Text = "Runout Size Wise Yield";
                    pieChartrunout.ShowDataLabelsOverMaximum = true;




                    // dent yield chart
                    var pieChart1 = ws.Drawings.AddChart("Dent_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart1.SetPosition(rowCnt + 2, 0, 12, 0);
                    pieChart1.SetSize(400, 400);
                    var colcount1 = ws.Dimension.End.Column;
                    var rowCnt1 = ws.Dimension.End.Row;
                   
                    var ser21 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["J4:J" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart1.Title.Text = "Dent Size Wise Yield";
                    pieChart1.ShowDataLabelsOverMaximum = true;


                    var pieChart2 = ws.Drawings.AddChart("Bulge_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart2.SetPosition(rowCnt + 2, 0, 19, 0);
                    pieChart2.SetSize(400, 400);
                    var colcount2 = ws.Dimension.End.Column;
                    var rowCnt2 = ws.Dimension.End.Row;
                   
                    var ser22 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["O4:O" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart2.Title.Text = "Bulge Size Wise Yield";
                    pieChart2.ShowDataLabelsOverMaximum = true;


                    var pieChart3 = ws.Drawings.AddChart("balancing_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    //pieChart3.SetPosition((rowCnt + 4)*2, 0, 0, 0);
                    pieChart3.SetPosition(rowCnt + 2, 0, 24, 0);
                    pieChart3.SetSize(400, 400);
                    var colcount3 = ws.Dimension.End.Column;
                    var rowCnt3 = ws.Dimension.End.Row;
                    
                    var ser23 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["T4:T" + (rowCnt - 2) + ""],
                            ws.Cells["B4:B" + (rowCnt - 2) + ""]));
                    pieChart3.Title.Text = "Balancing Size Wise Yield";
                    pieChart3.ShowDataLabelsOverMaximum = true;
                }

            }
            else if (QualityReportRecipeTUOWise.Checked)
            {

                if (getdisplaytype == "Numbers")
                {
                    // runout yield chart
                    var pieChart = ws.Drawings.AddChart("finalyield_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2

                    var colcount = ws.Dimension.End.Column;
                    var rowCnt = ws.Dimension.End.Row;
                    pieChart.SetPosition(rowCnt+2, 0, 0, 0);
                    pieChart.SetSize(400, 400);
                  
                    var ser1 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["B4:B" + (rowCnt - 2) + ""],
                          ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'B3");
                    var ser2 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart.Title.Text = "Over All A Grade Yield Size Wise";
                    pieChart.ShowDataLabelsOverMaximum = true;


                    // dent yield chart
                    var pieChartrunout = ws.Drawings.AddChart("Runpourt_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChartrunout.SetPosition(rowCnt + 2, 0, 5, 0);
                    pieChartrunout.SetSize(400, 400);
                  
                    var serrunout = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["B4:B" + (rowCnt - 2) + ""],
                          ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'I3");
                    var serrunout2 = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["I4:I" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChartrunout.Title.Text = "Runout Size Wise Yield";
                    pieChartrunout.ShowDataLabelsOverMaximum = true;




                    // dent yield chart
                    var pieChart1 = ws.Drawings.AddChart("Dent_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart1.SetPosition(rowCnt + 2, 0, 12, 0);
                    pieChart1.SetSize(400, 400);
                   
                    var ser11 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["B4:B" + (rowCnt - 2) + ""],
                          ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'I3");
                    var ser21 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["I4:I" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart1.Title.Text = "Dent Size Wise Yield";
                    pieChart1.ShowDataLabelsOverMaximum = true;


                    var pieChart2 = ws.Drawings.AddChart("Bulge_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart2.SetPosition(rowCnt + 2, 0, 19, 0);
                    pieChart2.SetSize(400, 400);
                  
                    var ser12 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["B4:B" + (rowCnt - 2) + ""],
                          ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'N3");
                    var ser22 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["N4:N" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart2.Title.Text = "Bulge Size Wise Yield";
                    pieChart2.ShowDataLabelsOverMaximum = true;


                    var pieChart3 = ws.Drawings.AddChart("balancing_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                   // pieChart3.SetPosition((rowCnt + 4)*2, 0, 0, 0);
                    pieChart3.SetPosition(rowCnt + 2, 0, 24, 0);
                    pieChart3.SetSize(400, 400);
                 
                    var ser13 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["B4:B" + (rowCnt - 2) + ""],
                          ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    ser1.HeaderAddress = new ExcelAddress("'Runoutreport'S3");
                    var ser23 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["S4:S" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart3.Title.Text = "Balancing Size Wise Yield";
                    pieChart3.ShowDataLabelsOverMaximum = true;
                }
                else if (getdisplaytype == "Percent")
                {

                    // runout yield chart
                    var pieChart = ws.Drawings.AddChart("finalYield_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2

                    var colcount = ws.Dimension.End.Column;
                    var rowCnt = ws.Dimension.End.Row;
                    pieChart.SetPosition(rowCnt+2, 0, 0, 0);
                    pieChart.SetSize(400, 400);
               
                    var ser2 = (ExcelChartSerie)(pieChart.Series.Add(ws.Cells["C4:C" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart.Title.Text = "Over All A Grade Yield Size Wise";
                    pieChart.ShowDataLabelsOverMaximum = true;


                    var pieChartrunout = ws.Drawings.AddChart("runout_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChartrunout.SetPosition(rowCnt + 2, 0, 5, 0);
                    pieChartrunout.SetSize(400, 400);

                    var serrunout = (ExcelChartSerie)(pieChartrunout.Series.Add(ws.Cells["D4:D" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChartrunout.Title.Text = "Runout Size Wise Yield";
                    pieChartrunout.ShowDataLabelsOverMaximum = true;





                    // dent yield chart
                    var pieChart1 = ws.Drawings.AddChart("Dent_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart1.SetPosition(rowCnt + 2, 0, 12, 0);
                    pieChart1.SetSize(400, 400);
                    
                    var ser21 = (ExcelChartSerie)(pieChart1.Series.Add(ws.Cells["I4:I" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart1.Title.Text = "Dent Size Wise Yield";
                    pieChart1.ShowDataLabelsOverMaximum = true;


                    var pieChart2 = ws.Drawings.AddChart("Bulge_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                    pieChart2.SetPosition(rowCnt + 2, 0, 19, 0);
                    pieChart2.SetSize(400, 400);
                  

                    var ser22 = (ExcelChartSerie)(pieChart2.Series.Add(ws.Cells["N4:N" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                    pieChart2.Title.Text = "Bulge Size Wise Yield";
                    pieChart2.ShowDataLabelsOverMaximum = true;


                    var pieChart3 = ws.Drawings.AddChart("balancing_recipeWise", eChartType.ColumnClustered);
                    //Set top left corner to row 1 column 2
                   // pieChart3.SetPosition((rowCnt + 4)*2, 0, 0, 0);
                    pieChart3.SetPosition(rowCnt + 2, 0, 24, 0);
                    pieChart3.SetSize(400, 400);
                 

                    var ser23 = (ExcelChartSerie)(pieChart3.Series.Add(ws.Cells["S4:S" + (rowCnt - 2) + ""],
                            ws.Cells["A4:A" + (rowCnt - 2) + ""]));
                 
             

                    pieChart3.Title.Text = "Balancing Size Wise Yield";
                    pieChart3.ShowDataLabelsOverMaximum = true;

                }


            }
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();


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
                cmd.CommandTimeout = 0;
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
                        //excelReportRecipeWise(dr["machinename"].ToString().Trim(), rToDate, rFromDate);
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
        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        //public void excelReportRecipeWise(string wcName, string rToDate, string rFromDate)
        //{
        //    string query;
        //    DataTable dt = new DataTable();
        //    DataTable gridviewdt = new DataTable();
        //    gridviewdt.Columns.Add("tireType", typeof(string));
        //    gridviewdt.Columns.Add("Checked", typeof(string));
        //    gridviewdt.Columns.Add("A", typeof(string));
        //    gridviewdt.Columns.Add("B", typeof(string));
        //    gridviewdt.Columns.Add("C", typeof(string));
        //    gridviewdt.Columns.Add("D", typeof(string));
        //    gridviewdt.Columns.Add("E", typeof(string));
        //    dt.Columns.Add("tireType", typeof(string));
        //    dt.Columns.Add("uniformitygrade", typeof(string));
        //    int total, A, B, C, D, E;
        //    Double pA, pB, pC, pD, pE;

        //    int colCount = 1, mergeCount = 0, typeCount = 0;           
        //    query = "";
        //    string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

        //    if (QualityReportTUOWise.Checked)
        //    {
        //        typeCount = 1;
        //        query = getqueryWCWise("machineName='" + wcName + "'", dtnadtime);

        //        xlWorkSheet.Cells[rowCount + 1, 1] = wcName;
        //        mergeCount = rowCount;
        //    }
        //    else if (QualityReportRecipeTUOWise.Checked)
        //    {
        //        typeCount = 0;
        //        xlApp = new Excel.ApplicationClass();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkBook.CheckCompatibility = false;
        //        xlWorkBook.DoNotPromptForConvert = true;

        //        //Get PID
        //        HandleRef hwnd = new HandleRef(xlApp, (IntPtr)xlApp.Hwnd);
        //        GetWindowThreadProcessId(hwnd, out pid);

        //        xlApp.Visible = true; // ensure that the excel app is visible.
        //        xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
        //        Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

        //        xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
        //        xlWorkSheet.Cells[1, 2] = "Performance Report TUO Machine Wise";
        //        xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
        //        xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
        //        ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
        //        ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

        //        xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //        xlWorkSheet.get_Range("C3", "D3").Merge(misValue);
        //        xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
        //        xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
        //        xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //        xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //        xlWorkSheet.get_Range("A3", "G3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //        xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
        //        xlWorkSheet.Cells[2, 2] = "To : " + formattoDate(rFromDate);
        //        xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

        //        xlWorkSheet.Cells[3, 1] = "Tyre Type";
        //        xlWorkSheet.Cells[3, 2] = "Checked";
        //        xlWorkSheet.Cells[3, 3] = "OE";
        //        xlWorkSheet.Cells[3, 5] = "";
        //        xlWorkSheet.Cells[3, 6] = "Rep";
        //        xlWorkSheet.Cells[3, 7] = "Scrap";
        //        xlWorkSheet.get_Range("C3", "D3").Merge(Type.Missing);
                
        //        xlWorkSheet.Cells[4, 3] = "A";
        //        xlWorkSheet.Cells[4, 4] = "B";
        //        xlWorkSheet.Cells[4, 5] = "C";
        //        xlWorkSheet.Cells[4, 6] = "D";
        //        xlWorkSheet.Cells[4, 7] = "E";

        //        ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
        //        xlWorkSheet.get_Range("A3", "B3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
        //        xlWorkSheet.get_Range("C4", "G4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
        //        xlWorkSheet.get_Range("C3", "G3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
        //        xlWorkSheet.get_Range("A3", "G3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
        //        xlWorkSheet.get_Range("A4", "G4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                
        //        if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
        //        {
        //            query = "Select  tireType ,uniformityGrade FROM  ProductionDataTUO  WHERE ((testTime>" + dtnadtime;
        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
        //        {

        //            query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
        //        {
        //            query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

        //        }
        //        else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
        //        {
        //            query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

        //        }
        //    }
        //    try
        //    {
        //        myConnection.open(ConnectionOption.SQL);
        //        myConnection.comm = myConnection.conn.CreateCommand();
        //        myConnection.comm.CommandText = query;

        //        myConnection.reader = myConnection.comm.ExecuteReader();
        //        dt.Load(myConnection.reader);
        //        myConnection.reader.Close();
        //        myConnection.comm.Dispose();
        //        myConnection.conn.Close();
        //    }
        //    catch (Exception exp)
        //    {
        //        myConnection.reader.Close();
        //        myConnection.comm.Dispose();
        //        myConnection.conn.Close();
        //        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //    }

        //    DataTable uniqrecipedt = new DataTable();
        //    uniqrecipedt = GetDistinctRecords(dt, "tireType");

        //    total_ = 0; A_ = 0; B_ = 0; C_ = 0; D_ = 0; E_ = 0;

        //    for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
        //    {
        //        total = 0; A = 0; B = 0; C = 0; D = 0; E = 0;
        //        pA = 0; pB = 0; pC = 0; pD = 0; pE = 0; rowCount++;

        //        total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
        //        A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
        //        B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
        //        C = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
        //        D = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
        //        E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));

        //        total_ = total_ + total;
        //        A_ = A_ + A;
        //        B_ = B_ + B;
        //        C_ = C_ + C;
        //        D_ = D_ + D;
        //        E_ = E_ + E;

        //        DataRow dr = gridviewdt.NewRow();
        //        switch(option)
        //        {
        //            case "1" :
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0];
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total;
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A;
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B;
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = C;
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = D;
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = E;
        //                break;
        //            case "2" :                    
        //                pA = ((total == 0) ? 0 : ((double)(A * 100) / total));
        //                pB = ((total == 0) ? 0 : ((double)(B * 100) / total));
        //                pC = ((total == 0) ? 0 : ((double)(C * 100) / total));
        //                pD = ((total == 0) ? 0 : ((double)(D * 100) / total));
        //                pE = ((total == 0) ? 0 : ((double)(E * 100) / total));

        //                xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA, 1);
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB, 1);

        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pC, 1);
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pD, 1);
        //                xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pE, 1);
        //                break;
        //            }

        //        gridviewdt.Rows.Add(dr);



        //    }
        //    DataRow ndr = gridviewdt.NewRow();
            
        //    gridviewdt.Rows.Add(ndr);
        //    DataRow tdr = gridviewdt.NewRow();
        //    xlWorkSheet.Cells[rowCount + 2, colCount + typeCount] = "Total";
        //    switch(option)
        //    {
        //        case "1" :
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_;
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = A_;
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = B_;
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = C_;
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = D_;
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = E_;
        //            break;
        //        case "2" :            
        //        pA = ((total_ == 0) ? 0 : ((double)(A_ * 100) / total_));
        //        pB = ((total_ == 0) ? 0 : ((double)(B_ * 100) / total_));
        //        pC = ((total_ == 0) ? 0 : ((double)(C_ * 100) / total_));
        //        pD = ((total_ == 0) ? 0 : ((double)(D_ * 100) / total_));
        //        pE = ((total_ == 0) ? 0 : ((double)(E_ * 100) / total_));

        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_.ToString();
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = Math.Round(pA, 1);
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = Math.Round(pB, 1);

        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = Math.Round(pC, 1);
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = Math.Round(pD, 1);
        //        xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = Math.Round(pE, 1);
        //        break;
        //    }

        //    gridviewdt.Rows.Add(tdr);

        //    rowCount += 2;
        //    grandtotal = grandtotal + total_;
        //    grandA = grandA + A_;
        //    grandB = grandB + B_;
        //    grandC = grandC + C_;
        //    grandD = grandD + D_;
        //    grandE = grandE + E_;
                        
        //    if (QualityReportTUOWise.Checked)
        //    {               
        //        xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
        //        xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
        //    }
        //    else if (QualityReportRecipeTUOWise.Checked)
        //    {
        //        xlWorkSheet.get_Range("A" + (rowCount + 1), "G" + (rowCount + 1)).Merge(misValue);

        //        xlWorkSheet.get_Range("A1", "G" + rowCount).Font.Bold = true;
        //        xlWorkSheet.get_Range("A1", "G" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

        //        xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report TUO Machine Wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
               
        //    }
        //}
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
        string NullIf(string num)
        {
            return num == "0" ? "" : num.ToString();
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            /*try
            {
                if (QualityReportTUOWise.Checked)
                {
                    if (performanceReportTUOWiseMainGridView.Rows.Count != 0)
                    {
                        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                        TableHeaderCell cell = new TableHeaderCell();

                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "OE";
                        cell.ColumnSpan = 2;
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "Rep";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "Rejection";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "ViewDetail";
                        row.Controls.Add(cell);


                        performanceReportTUOWiseMainGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    if (performanceReportRecipeTUOWiseGridView.Rows.Count != 0)
                    {
                        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                        TableHeaderCell cell = new TableHeaderCell();

                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "OE";
                        cell.ColumnSpan = 2;
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "Rep";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "Scrap";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "ScrapDetail";
                        row.Controls.Add(cell);

                        performanceReportRecipeTUOWiseGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }*/
        }
    }
}

