using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
namespace SmartMIS.TUO
{
    public partial class TUOReport5 : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        int  JANTotal_, FEBTotal_, MARTotal_, APRTotal_, MAYTotal_, JUNTotal_, JULTotal_, AUGTotal_, SEPTotal_, OCTTotal_, NOVTotal_, DECETotal_;

        int total_, YTD_, JAN_, FEB_, MAR_, APR_, MAY_, JUN_, JUL_, AUG_, SEP_, OCT_, NOV_, DECE_;

        Double totalchecked;
        Double pYTD_, pJAN_, pFEB_, pMAR_, pAPR_, pMAY_, pJUN_, pJUL_, pAUG_, pSEP_, pOCT_, pNOV_, pDECE_;
        public Double grandtotal, grandYTD, grandJAN, grandFEB, grandMAR, grandAPR, grandMAY, grandJUN, grandJUL, grandAUG, grandSEP, grandOCT, grandNOV, grandDECE;
        string tablename, parameter;
        int value;
        DataRow tdr;
        DataTable graphdt;
        DataTable globaldt = new DataTable();
                

        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", query = "";
        string[] tempString2;
        public string _rDate;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "performanceReportOAYGraf.xlsx";
        string filepath;
        int rowCount = 4, pid = -1;
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        string[] tempString;
        
        DataTable maindt = new DataTable();
        DataTable gridviewdt = new DataTable();
        #endregion

        public TUOReport5()
        {
            filepath = myWebService.getExcelPath();
        }
        
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
                    if (!IsPostBack)
                    {
                        fillSizedropdownlist();
                        fillDesigndropdownlist();
                    }
                    showDownload.Text = "";
                    if (QualityReportTBMWise.Checked)
                    {                        
                        QualityReportOAYGRAFTBMWisePanel.Visible = true;
                        QualityReportOAYGRAFRecipeWisePanel.Visible = false;
                    }
                    else if (QualityReportRecipeWise.Checked)
                    {
                        QualityReportOAYGRAFTBMWisePanel.Visible = false;
                        QualityReportOAYGRAFRecipeWisePanel.Visible = true;
                    }
                    //Compare the hidden field if it contains the query string or not

                   

                        //  Compare which type of report user had selected//
                        //
                        //  Plant wide = 0
                        //  Workcenter wide = 1
                        //


                        if (rType == "0")
                        {
                        }
                        else if (rType == "1")
                        {                            
                            }
                            else if (rChoice == "1")
                            {
                            }
                            else if (rChoice == "2")
                            {
                            }

                        }
                    }
              
            catch (Exception exp)
            {
                using (StreamWriter w = new StreamWriter(Server.MapPath("..\\ErrorLogs\\data.txt"), true))
                {
                   myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
        }
        protected void magicButton_Click(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            tempString = magicHidden.Value.Split(new char[] { '?' });
            option = (tuoFilterOptionDropDownList.SelectedIndex == 0) ? "1" : ((tuoFilterOptionDropDownList.SelectedIndex == 1) ? "2" : "");
            
            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            
            if (tempString.Length > 1)
            {
                rType = tempString[0];
                rWCID = tempString[1];
                rChoice = tempString[2];
                rToDate = tempString[3];
                rFromDate = tempString[3];
                rToMonth = tempString[5];
                rToYear = tempString[7];
            }

            if (rChoice == "2")
            {
                notifyLabel.Visible = false;
                string query = myWebService.createQuery(rWCID);
                wcnamequery = myWebService.wcquery(query, tempString[tempString.Length - 1]);
                if (QualityReportTBMWise.Checked)
                {
                    QualityReportOAYGRAFTBMWisePanel.Visible = true;
                    showReport(query, rToYear);
                }
                else
                {    
                    QualityReportOAYGRAFRecipeWisePanel.Visible = true;
                    showReportRecipeWise(performanceReportOAYGRAFRecipeWiseGridView, "", rToYear);
                }
                
                fillchart();

            }
            else
            {
                notifyLabel.Visible = true;
                QualityReportOAYGRAFTBMWisePanel.Visible = false;
            }



        }
        private void fillchart()
        {
            /*graphdt = new DataTable();
            graphdt.Columns.Add("month", typeof(string));
            graphdt.Columns.Add("uniformitygrade", typeof(string));
            if (tempString[tempString.Length - 1].ToString() == "0")
            {
               if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                   query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " )) and datepart(YYYY,testTime)=" + rToYear + ")";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                   query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                   query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear;
            }
            else if (tempString[tempString.Length - 1].ToString() == "1")
            {
                if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + ") and datepart(YYYY,testTime)=" + rToYear;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear;
            }
            else if (tempString[tempString.Length - 1].ToString() == "2")
            {
                if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'))  and datepart(YYYY,testTime)=" + rToYear + ")";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    query = "select uniformitygrade, testTime, datepart(MM,testTime) AS month from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear;

            }

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                graphdt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
            catch (Exception exp)
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
             
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                
            }*/

            double[] TotalCheckedSeries = new double[13];
            double[] ABgradeSeries = new double[13];
            string[] xvalues = { "YTD", "jan", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };


            TotalCheckedSeries.SetValue(AlltotalcheckedQuantity(globaldt), 0);
            TotalCheckedSeries.SetValue(AlltotalJANCheckedQuantity(globaldt), 1);
            TotalCheckedSeries.SetValue(AlltotalFEBCheckedQuantity(globaldt), 2);
            TotalCheckedSeries.SetValue(AlltotalMARCheckedQuantity(globaldt), 3);
            TotalCheckedSeries.SetValue(AlltotalAPRCheckedQuantity(globaldt), 4);
            TotalCheckedSeries.SetValue(AlltotalMAYCheckedQuantity(globaldt), 5);
            TotalCheckedSeries.SetValue(AlltotalJUNCheckedQuantity(globaldt), 6);
            TotalCheckedSeries.SetValue(AlltotalJULCheckedQuantity(globaldt), 7);
            TotalCheckedSeries.SetValue(AlltotalAUGCheckedQuantity(globaldt), 8);
            TotalCheckedSeries.SetValue(AlltotalSEPCheckedQuantity(globaldt), 9);
            TotalCheckedSeries.SetValue(AlltotalOCTCheckedQuantity(globaldt), 10);
            TotalCheckedSeries.SetValue(AlltotalNOVCheckedQuantity(globaldt), 11);
            TotalCheckedSeries.SetValue(AlltotalDECCheckedQuantity(globaldt), 12);
            if (option == "1")
            {
                ABgradeSeries.SetValue(AlltotalYTDQuantity(globaldt), 0);
                ABgradeSeries.SetValue(AlltotalJANQuantity(globaldt), 1);
                ABgradeSeries.SetValue(AlltotalFEBQuantity(globaldt), 2);

                ABgradeSeries.SetValue(AlltotalMARQuantity(globaldt), 3);
                ABgradeSeries.SetValue(AlltotalAPRQuantity(globaldt), 4);
                ABgradeSeries.SetValue(AlltotalMAYQuantity(globaldt), 5);
                ABgradeSeries.SetValue(AlltotalJUNQuantity(globaldt), 6);
                ABgradeSeries.SetValue(AlltotalJULQuantity(globaldt), 7);
                ABgradeSeries.SetValue(AlltotalAUGQuantity(globaldt), 8);

                ABgradeSeries.SetValue(AlltotalSEPQuantity(globaldt), 9);
                ABgradeSeries.SetValue(AlltotalOCTQuantity(globaldt), 10);
                ABgradeSeries.SetValue(AlltotalNOVQuantity(globaldt), 11);
                ABgradeSeries.SetValue(AlltotalDECQuantity(globaldt), 12);
            }
            else if (option == "2")
            {
                ABgradeSeries.SetValue(AlltotalYTDQuantity(globaldt), 0);
                ABgradeSeries.SetValue(AlltotalJANQuantity(globaldt), 1);
                ABgradeSeries.SetValue(AlltotalFEBQuantity(globaldt), 2);

                ABgradeSeries.SetValue(AlltotalMARQuantity(globaldt), 3);
                ABgradeSeries.SetValue(AlltotalAPRQuantity(globaldt), 4);
                ABgradeSeries.SetValue(AlltotalMAYQuantity(globaldt), 5);
                ABgradeSeries.SetValue(AlltotalJUNQuantity(globaldt), 6);
                ABgradeSeries.SetValue(AlltotalJULQuantity(globaldt), 7);
                ABgradeSeries.SetValue(AlltotalAUGQuantity(globaldt), 8);

                ABgradeSeries.SetValue(AlltotalSEPQuantity(globaldt), 9);
                ABgradeSeries.SetValue(AlltotalOCTQuantity(globaldt), 10);
                ABgradeSeries.SetValue(AlltotalNOVQuantity(globaldt), 11);
                ABgradeSeries.SetValue(AlltotalDECQuantity(globaldt), 12);
            }

            performanceReportOAYGrafTBMChart.Series["TotalCheckedSeries"].Points.DataBindXY(xvalues, TotalCheckedSeries);
            performanceReportOAYGrafTBMChart.Series["ABgradeSeries"].Points.DataBindXY(xvalues, ABgradeSeries);

        }

        public int AlltotalcheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)",""));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            
            return flag;
        }
        public int AlltotalJANCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='1'"));
                               
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;

        }
        public int AlltotalFEBCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='2'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;

        }
        public int AlltotalMARCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='3'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;

        }
        public int AlltotalAPRCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='4'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;

        }
        public int AlltotalMAYCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='5'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            
            return flag;

        }
        public int AlltotalJUNCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='6'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            
            return flag;

        }
        public int AlltotalJULCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='7'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;

        }
        public int AlltotalAUGCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='8'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;

        }
        public int AlltotalSEPCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='9'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;

        }
        public int AlltotalOCTCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='10'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;

        }
        public int AlltotalNOVCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='11'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            
            return flag;

        }
        public int AlltotalDECCheckedQuantity(DataTable dt)
        {
            int flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "TestTime='12'"));
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;

        }
        public Double AlltotalYTDQuantity(DataTable dt)
        {
            Double flag = 0;
            totalchecked = AlltotalcheckedQuantity(dt);
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B')")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
        
            return flag;
        }

        public Double AlltotalJANQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='1'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;
        }
        public Double AlltotalFEBQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='2'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
         
            return flag;
        }
        public Double AlltotalMARQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='3'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            
            return flag;
        }
        public Double AlltotalAPRQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='4'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;
        }
        public Double AlltotalMAYQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='5'")); 
                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;
        }
        public Double AlltotalJUNQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='6'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
         
            return flag;
        }
        public Double AlltotalJULQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='7'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;
        }
        public Double AlltotalAUGQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='8'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;
        }
        public Double AlltotalSEPQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='9'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
           
            return flag;
        }
        public Double AlltotalOCTQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='10'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;
        }
        public Double AlltotalNOVQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='11'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
          
            return flag;
        }
        public Double AlltotalDECQuantity(DataTable dt)
        {
            Double flag = 0;
            try
            {
                flag = Convert.ToInt32(dt.Compute("count(uniformitygrade)", " UniformityGrade in('A','B') AND TestTime='12'")); 
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }            
            return flag;
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            option = (tuoFilterOptionDropDownList.SelectedItem.Text == "No") ? "1" : "2";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceReportOAYGRAFWiseMainGridView")
                {

                    /*Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseWCNameLabel"));
                    workcentername = wcnameLabel.Text.ToString();
                    GridView childGridView = ((GridView)e.Row.FindControl("performanceReportOAYGRAFWiseChildGridView"));
                    showReportRecipeWise(childGridView, workcentername, rToYear);
                    */

                    for (int rowIndex = performanceReportOAYGRAFWiseMainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = performanceReportOAYGRAFWiseMainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = performanceReportOAYGRAFWiseMainGridView.Rows[rowIndex + 1];
                        if (((Label)performanceReportOAYGRAFWiseMainGridView.Rows[rowIndex].FindControl("performanceReportOAYGRAFWiseWCNameLabel")).Text == ((Label)performanceReportOAYGRAFWiseMainGridView.Rows[rowIndex + 1].FindControl("performanceReportOAYGRAFWiseWCNameLabel")).Text)
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

                    if (((Label)e.Row.FindControl("performanceReportOAYGRAFWiseWCNameLabel")).Text == "Grand Total" || ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseTyreTypeLabel")).Text == "Total")
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                            //performanceReport2TBMWiseMainGridView.Rows[i].Font.Bold = true;
                        }
                    }
                    Label cell;
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseCheckedLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseYTDGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseJANGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseFEBGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseMARGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseAPRGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseMAYGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseJUNGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseJULGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseAUGGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseSEPGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseOCTGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseNOVGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseDECGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                }
                if (((GridView)sender).ID == "performanceReportOAYGRAFRecipeWiseGridView")
                {
                    Label cell;
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseCheckedLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseYTDGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseJANGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseFEBGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseMARGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseAPRGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseMAYGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseJUNGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseJULGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseAUGGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseSEPGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseOCTGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseNOVGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseDECGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                }
            }

        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceReportOAYGRAFWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportOAYGRAFWiseMainGridView.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private string createQuery(String wcID)
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
        private string createwcIDQuery(String wcID)
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
        
        private void fillSizedropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataBind();
        }
        private void fillDesigndropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataBind();
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
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {
            QualityReportOAYGRAFRecipeWisePanel.Visible = true;
            QualityReportOAYGRAFTBMWisePanel.Visible = false;
        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {

            QualityReportOAYGRAFRecipeWisePanel.Visible = false;
            QualityReportOAYGRAFTBMWisePanel.Visible = true;
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));


            }
            else if (ddl.ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

            }


        }
        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private void showReport(string query, string rToYear)
        {
            try
            {
                gridviewdt.Columns.Add("workCenterName", typeof(string));
                gridviewdt.Columns.Add("tireType", typeof(string));
                gridviewdt.Columns.Add("Checked", typeof(string));
                gridviewdt.Columns.Add("YTD", typeof(string));
                gridviewdt.Columns.Add("JAN", typeof(string));
                gridviewdt.Columns.Add("FEB", typeof(string));
                gridviewdt.Columns.Add("MAR", typeof(string));
                gridviewdt.Columns.Add("APR", typeof(string));
                gridviewdt.Columns.Add("MAY", typeof(string));
                gridviewdt.Columns.Add("JUN", typeof(string));
                gridviewdt.Columns.Add("JUL", typeof(string));
                gridviewdt.Columns.Add("AUG", typeof(string));
                gridviewdt.Columns.Add("SEP", typeof(string));
                gridviewdt.Columns.Add("OCT", typeof(string));
                gridviewdt.Columns.Add("NOV", typeof(string));
                gridviewdt.Columns.Add("DECE", typeof(string));
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select DISTINCT  workCenterName from vWorkCenter WHERE " + query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                maindt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();

                for (int i = 0; i < maindt.Rows.Count; i++)
                    fillData(maindt.Rows[i][0].ToString(), rToYear);

                //DataRow dr = gridviewdt.NewRow();
                //dr[0] = "Grand Total";
                //dr[1] = "";
                //double[] last_row = new double[] { grandtotal, grandYTD, grandJAN, grandFEB, grandMAR, grandAPR, grandMAY, grandJUN, grandJUL, grandAUG, grandSEP, grandOCT, grandNOV, grandDECE };

                //switch (option)
                //{
                //    case "1":
                //        for (int i = 0; i < last_row.Count(); i++)
                //            dr[i + 2] = last_row[i];
                //        break;
                //    case "2":
                //        for (int i = 0; i < last_row.Count(); i++)
                //            dr[i + 2] = Math.Round(((last_row[i] * 100) / grandtotal)) + "%";
                //        break;
                //}
                //gridviewdt.Rows.Add(dr);
                
                if (QualityReportTBMWise.Checked)
                {
                    performanceReportOAYGRAFWiseMainGridView.DataSource = gridviewdt;
                    performanceReportOAYGRAFWiseMainGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            } 
        }
        private void fillData(string wcName, string rtoYear)
        {
            string query;
            DataTable dt = new DataTable();
                        
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("TestTime", typeof(int));
            int total, YTD, JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DECE;
            int  JANTotal, FEBTotal, MARTotal, APRTotal, MAYTotal, JUNTotal, JULTotal, AUGTotal, SEPTotal, OCTTotal, NOVTotal, DECETotal;
            Double pYTD, pJAN, pFEB, pMAR, pAPR, pMAY, pJUN, pJUL, pAUG, pSEP, pOCT, pNOV, pDECE;


            query = "";

            if (QualityReportRecipeWise.Checked)
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM  " + tablename + "  WHERE ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                query = (tempString2[tempString2.Length - 1].ToString() == "0") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "1") ? getqueryWCWise(tablename, "machineName='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "2") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ""));
                dt = getFilledDT(query, rtoYear);
            }
            globaldt.Merge(dt);
            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");
            JANTotal_ = 0; FEBTotal_ = 0; MARTotal_ = 0; APRTotal_ = 0; MAYTotal_ = 0; JUNTotal_ = 0; JULTotal_ = 0; AUGTotal_ = 0; SEPTotal_ = 0; OCTTotal_ = 0; NOVTotal_ = 0; DECETotal_ = 0;
            total_ = 0; YTD_ = 0; JAN_ = 0; FEB_ = 0; MAR_ = 0; APR_ = 0; MAY_ = 0; JUN_ = 0; JUL_ = 0; AUG_ = 0; SEP_ = 0; OCT_ = 0; NOV_ = 0; DECE_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                JANTotal = 0; FEBTotal = 0; MARTotal = 0; APRTotal = 0; MAYTotal = 0; JUNTotal = 0; JULTotal = 0; AUGTotal = 0; SEPTotal = 0; OCTTotal = 0; NOVTotal = 0; DECETotal = 0;
                total = 0; YTD = 0; JAN = 0; FEB = 0; MAR = 0; APR = 0; MAY = 0; JUN = 0; JUL = 0; AUG = 0; SEP = 0; OCT = 0; NOV = 0; DECE = 0;
                pYTD = 0; pJAN = 0; pFEB = 0; pMAR = 0; pAPR = 0; pMAY = 0; pJUN = 0; pJUL = 0; pAUG = 0; pSEP = 0; pOCT = 0; pNOV = 0; pDECE = 0;
                total = (int)dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                YTD = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B')"));
                JANTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='01'"));
                JAN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  UniformityGrade in('A','B') AND TestTime='01'"));
                FEBTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  TestTime='02'"));
                FEB = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='02'"));
                MARTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='03'"));
                MAR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='03'"));
                APRTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='04'"));
                APR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='04'"));
                MAYTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='05'"));
                MAY = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='05'"));
                JUNTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='06'"));
                JUN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='06'"));
                JULTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='07'"));
                JUL = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='07'"));
                AUGTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='08'"));
                AUG = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='08'"));
                SEPTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='09'"));
                SEP = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='09'"));
                OCTTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='10'"));
                OCT = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='10'"));
                NOVTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND   TestTime='11'"));
                NOV = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='11'"));
                DECETotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='12'"));
                DECE = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='12'"));

                total_ += total;
                YTD_ += YTD;
                JANTotal_ += JANTotal;
                JAN_ += JAN;
                FEBTotal_ += FEBTotal;
                FEB_ += FEB;
                MARTotal_ += MARTotal;
                MAR_ += MAR;
                APRTotal_ += APRTotal;
                APR_ += APR;
                MAYTotal_ += MAYTotal;
                MAY_ += MAY;
                JUNTotal_ += JUNTotal;
                JUN_ += JUN;
                JULTotal_ += JULTotal;
                JUL_ += JUL;
                AUGTotal_+=AUGTotal;
                AUG_ += AUG;
                SEPTotal_+=SEPTotal;
                SEP_ += SEP;
                OCTTotal_+=OCTTotal;
                OCT_ += OCT;
                NOVTotal_+=NOVTotal;
                NOV_ += NOV;
                DECETotal_+=DECETotal;
                DECE_ += DECE;

                DataRow dr = gridviewdt.NewRow();
                dr[0] = wcName;
                switch (option)
                {
                    case "1":
                        dr[1] = uniqrecipedt.Rows[i][0].ToString();
                        dr[2] = total.ToString();
                        dr[3] = YTD.ToString();
                        dr[4] = JAN.ToString();
                        dr[5] = FEB.ToString();
                        dr[6] = MAR.ToString();
                        dr[7] = APR.ToString();
                        dr[8] = MAY.ToString();
                        dr[9] = JUN.ToString();
                        dr[10] = JUL.ToString();
                        dr[11] = AUG.ToString();
                        dr[12] = SEP.ToString();
                        dr[13] = OCT.ToString();
                        dr[14] = NOV.ToString();
                        dr[15] = DECE.ToString();
                        break;
                    case "2":
                        pYTD = ((double)(YTD * 100) / total);
                        pJAN = ((double)(JAN * 100) / JANTotal);
                        pFEB = ((double)(FEB * 100) / FEBTotal);
                        pMAR = ((double)(MAY * 100) / MARTotal);
                        pAPR = ((double)(APR * 100) / APRTotal);
                        pMAY = ((double)(MAY * 100) / MAYTotal);
                        pJUN = ((double)(JUN * 100) / JUNTotal);
                        pJUL = ((double)(JUL * 100) / JULTotal);
                        pAUG = ((double)(AUG * 100) / AUGTotal);
                        pSEP = ((double)(SEP * 100) / SEPTotal);
                        pOCT = ((double)(OCT * 100) / OCTTotal);
                        pNOV = ((double)(NOV * 100) / NOVTotal);
                        pDECE = ((double)(DECE * 100) / DECETotal);

                        dr[1] = uniqrecipedt.Rows[i][0].ToString();
                        dr[2] = total.ToString();
                        dr[3] = Math.Round(pYTD, 1);
                        dr[4] = Math.Round(pJAN, 1).ToString() == "NaN" ? "" : Math.Round(pJAN, 1).ToString();                    
                        dr[5] = Math.Round(pFEB, 1).ToString() == "NaN" ? "" : Math.Round(pFEB, 1).ToString();
                        dr[6] = Math.Round(pMAR, 1).ToString() == "NaN" ? "" : Math.Round(pMAR, 1).ToString();
                        dr[7] = Math.Round(pAPR, 1).ToString() == "NaN" ? "" : Math.Round(pAPR, 1).ToString();
                        dr[8] = Math.Round(pMAY, 1).ToString() == "NaN" ? "" : Math.Round(pMAY, 1).ToString();
                        dr[9] = Math.Round(pJUN, 1).ToString() == "NaN" ? "" : Math.Round(pJUN, 1).ToString();
                        dr[10] = Math.Round(pJUL, 1).ToString() == "NaN" ? "" : Math.Round(pJUL, 1).ToString();
                        dr[11] = Math.Round(pAUG, 1).ToString() == "NaN" ? "" : Math.Round(pAUG, 1).ToString();
                        dr[12] = Math.Round(pSEP, 1).ToString() == "NaN" ? "" : Math.Round(pSEP, 1).ToString();
                        dr[13] = Math.Round(pOCT, 1).ToString() == "NaN" ? "" : Math.Round(pOCT, 1).ToString();
                        dr[14] = Math.Round(pNOV, 1).ToString() == "NaN" ? "" : Math.Round(pNOV, 1).ToString();
                        dr[15] = Math.Round(pDECE, 1).ToString() == "NaN" ? "" : Math.Round(pDECE, 1).ToString();
                        break;
                }

                gridviewdt.Rows.Add(dr);



            }
            if (gridviewdt.Rows.Count > 1)
            {
                DataRow tdr = gridviewdt.NewRow();
                tdr[0] = "";
                switch (option)
                {
                    case "1":
                        tdr[1] = "Total";
                        tdr[2] = total_;
                        tdr[3] = YTD_;
                        tdr[4] = JAN_;
                        tdr[5] = FEB_;
                        tdr[6] = MAR_;
                        tdr[7] = APR_;
                        tdr[8] = MAY_;
                        tdr[9] = JUN_;
                        tdr[10] = JUL_;
                        tdr[11] = AUG_;
                        tdr[12] = SEP_;
                        tdr[13] = OCT_;
                        tdr[14] = NOV_;
                        tdr[15] = DECE_;
                        break;
                    case "2":
                        pYTD_ = ((double)(YTD_ * 100) / total_);
                        pJAN_ = ((double)(JAN_ * 100) / JANTotal_);
                        pFEB_ = ((double)(FEB_ * 100) / FEBTotal_);
                        pMAR_ = ((double)(MAR_ * 100) / MARTotal_);
                        pAPR_ = ((double)(APR_ * 100) / APRTotal_);
                        pMAY_ = ((double)(MAY_ * 100) / MAYTotal_);
                        pJUN_ = ((double)(JUN_ * 100) / JUNTotal_);
                        pJUL_ = ((double)(JUL_ * 100) / JULTotal_);
                        pAUG_ = ((double)(AUG_ * 100) / AUGTotal_);
                        pSEP_ = ((double)(SEP_ * 100) / SEPTotal_);
                        pOCT_ = ((double)(OCT_ * 100) / OCTTotal_);
                        pNOV_ = ((double)(NOV_ * 100) / NOVTotal_);
                        pDECE_ = ((double)(DECE_ * 100) / DECETotal_);

                        tdr[1] = "Total";
                        tdr[2] = total_;
                        tdr[3] = Math.Round(pYTD_, 1).ToString() == "NaN" ? "" : Math.Round(pYTD_, 1).ToString();
                        tdr[4] = Math.Round(pJAN_, 1).ToString() == "NaN" ? "" : Math.Round(pJAN_, 1).ToString();
                        tdr[5] = Math.Round(pFEB_, 1).ToString() == "NaN" ? "" : Math.Round(pFEB_, 1).ToString();
                        tdr[6] = Math.Round(pMAR_, 1).ToString() == "NaN" ? "" : Math.Round(pMAR_, 1).ToString();
                        tdr[7] = Math.Round(pAPR_, 1).ToString() == "NaN" ? "" : Math.Round(pAPR_, 1).ToString();
                        tdr[8] = Math.Round(pMAY_, 1).ToString() == "NaN" ? "" : Math.Round(pMAY_, 1).ToString();
                        tdr[9] = Math.Round(pJUN_, 1).ToString() == "NaN" ? "" : Math.Round(pJUN_, 1).ToString();
                        tdr[10] = Math.Round(pJUL_, 1).ToString() == "NaN" ? "" : Math.Round(pJUL_, 1).ToString();
                        tdr[11] = Math.Round(pAUG_, 1).ToString() == "NaN" ? "" : Math.Round(pAUG_, 1).ToString();
                        tdr[12] = Math.Round(pSEP_, 1).ToString() == "NaN" ? "" : Math.Round(pSEP_, 1).ToString();
                        tdr[13] = Math.Round(pOCT_, 1).ToString() == "NaN" ? "" : Math.Round(pOCT_, 1).ToString();
                        tdr[14] = Math.Round(pNOV_, 1).ToString() == "NaN" ? "" : Math.Round(pNOV_, 1).ToString();
                        tdr[15] = Math.Round(pDECE_, 1).ToString() == "NaN" ? "" : Math.Round(pDECE_, 1).ToString();
                        break;
                }

                gridviewdt.Rows.Add(tdr);

                grandtotal += total_;
                grandYTD += YTD_;
                grandJAN += JAN_;
                grandFEB += FEB_;
                grandMAR += MAR_;
                grandAPR += APR_;
                grandMAY += MAY_;
                grandJUN += JUN_;
                grandJUL += JUL_;
                grandAUG += AUG_;
                grandSEP += SEP_;
                grandOCT += OCT_;
                grandNOV += NOV_;
                grandDECE += DECE_;
            }

        }
        protected string getqueryWCWise(string tableName, string wcNameString, string rToYear)
        {
            if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE " + wcNameString + " AND ";
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ";
            }
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ";
            }
            return query;
        }
        protected DataTable executeQuery(string query)
        {
            DataTable innerdt = new DataTable();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                innerdt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
            catch (Exception exp)
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return innerdt;
        }
        protected DataTable getFilledDT(string query, string rtoYear)
        {
            DataTable dt = new DataTable();
            //try
            //{
            //    myConnection.conn.Open();
            //    myConnection.comm = myConnection.conn.CreateCommand();
            //    myConnection.comm.CommandText = "select tireType, uniformitygrade,convert(int,datepart(MM,testTime)) as TestTime from " + tablename + " where machinename='"+query+"' and DATEPART(yyyy,TestTime)=" + rtoYear + "";
            //    myConnection.reader = myConnection.comm.ExecuteReader();
            //    dt.Load(myConnection.reader);
            //}
            //catch (Exception exc)
            //{

            //}
            //finally
            //{
            //    if (!myConnection.reader.IsClosed)
            //        myConnection.reader.Close();
            //       myConnection.comm.Dispose();
            //       myConnection.conn.Close();
 
            //}


            dt=executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + "");
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='2'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='3'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='4'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='5'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='6'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='7'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='8'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='9'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='10'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='11'"));
            //dt.Merge(executeQuery(query + "DATEPART(yyyy,TestTime)=" + rtoYear + " AND DATEPART(MM,TestTime)='12'"));
            return dt;
        }
        private void showReportRecipeWise(GridView childgridview, string wcName, string rtoYear)
        {
            string query;
            DataTable dt = new DataTable();
            DataTable gridviewdt = new DataTable();

            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("YTD", typeof(string));
            gridviewdt.Columns.Add("JAN", typeof(string));
            gridviewdt.Columns.Add("FEB", typeof(string));
            gridviewdt.Columns.Add("MAR", typeof(string));
            gridviewdt.Columns.Add("APR", typeof(string));
            gridviewdt.Columns.Add("MAY", typeof(string));
            gridviewdt.Columns.Add("JUN", typeof(string));
            gridviewdt.Columns.Add("JUL", typeof(string));
            gridviewdt.Columns.Add("AUG", typeof(string));
            gridviewdt.Columns.Add("SEP", typeof(string));
            gridviewdt.Columns.Add("OCT", typeof(string));
            gridviewdt.Columns.Add("NOV", typeof(string));
            gridviewdt.Columns.Add("DECE", typeof(string));
            
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("TestTime", typeof(int));
            int  JANTotal, FEBTotal, MARTotal, APRTotal, MAYTotal, JUNTotal, JULTotal, AUGTotal, SEPTotal, OCTTotal, NOVTotal, DECETotal;
            int total, YTD, JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DECE;
            Double pYTD, pJAN, pFEB, pMAR, pAPR, pMAY, pJUN, pJUL, pAUG, pSEP, pOCT, pNOV, pDECE;


            query = "";
            
            if (QualityReportRecipeWise.Checked)
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM  " + tablename + "  WHERE ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                query = (tempString2[tempString2.Length - 1].ToString() == "0") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "1") ? getqueryWCWise(tablename, "machineName='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "2") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ""));
                dt = getFilledDT(query, rtoYear);
            }
            globaldt.Merge(dt);
            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");
            JANTotal_ = 0; FEBTotal_ = 0; MARTotal_ = 0; APRTotal_ = 0; MAYTotal_ = 0; JUNTotal_ = 0; JULTotal_ = 0; AUGTotal_ = 0; SEPTotal_ = 0; OCTTotal_ = 0; NOVTotal_ = 0; DECETotal_ = 0;

            total_ = 0; YTD_ = 0; JAN_ = 0; FEB_ = 0; MAR_ = 0; APR_ = 0; MAY_ = 0; JUN_ = 0; JUL_ = 0; AUG_ = 0; SEP_ = 0; OCT_ = 0; NOV_ = 0; DECE_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                JANTotal = 0; FEBTotal = 0; MARTotal = 0; APRTotal = 0; MAYTotal = 0; JUNTotal = 0; JULTotal = 0; AUGTotal = 0; SEPTotal = 0; OCTTotal = 0; NOVTotal = 0; DECETotal = 0;
                total = 0; YTD = 0; JAN = 0; FEB = 0; MAR = 0; APR = 0; MAY = 0; JUN = 0; JUL = 0; AUG = 0; SEP = 0; OCT = 0; NOV = 0; DECE = 0;
                pYTD = 0; pJAN = 0; pFEB = 0; pMAR = 0; pAPR = 0; pMAY = 0; pJUN = 0; pJUL = 0; pAUG = 0; pSEP = 0; pOCT = 0; pNOV = 0; pDECE = 0;
                total = (int)dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                YTD = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B')"));
                JANTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='01'"));
                JAN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  UniformityGrade in('A','B') AND TestTime='01'"));
                FEBTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='02'"));
                FEB = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='02'"));
                MARTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='03'"));
                MAR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='03'"));
                APRTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='04'"));
                APR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='04'"));
                MAYTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='05'"));
                MAY = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='05'"));
                JUNTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='06'"));
                JUN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='06'"));
                JULTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND   TestTime='07'"));
                JUL = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='07'"));
                AUGTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='08'"));
                AUG = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='08'"));
                SEPTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='09'"));
                SEP = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='09'"));
                OCTTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  TestTime='10'"));
                OCT = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='10'"));
                NOVTotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND TestTime='11'"));
                NOV = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='11'"));
                DECETotal = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "'  AND TestTime='12'"));
                DECE = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='12'"));
                
                total_ += total;
                YTD_ += YTD;
                JANTotal_ += JANTotal;
                JAN_ += JAN;
                FEBTotal_ += FEBTotal;
                FEB_ += FEB;
                MARTotal_ += MARTotal;
                MAR_ += MAR;
                APRTotal_ += APRTotal;
                APR_ += APR;
                MAYTotal_ += MAYTotal;
                MAY_ += MAY;
                JUNTotal_ += JUNTotal;
                JUN_ += JUN;
                JULTotal_ += JULTotal;
                JUL_ += JUL;
                AUGTotal_ += AUGTotal;
                AUG_ += AUG;
                SEPTotal_ += SEPTotal;
                SEP_ += SEP;
                OCTTotal_ += OCTTotal;
                OCT_ += OCT;
                NOVTotal_ += NOVTotal;
                NOV_ += NOV;
                DECETotal_ += DECETotal;
                DECE_ += DECE;
                
                DataRow dr = gridviewdt.NewRow();
                switch (option)
                {
                    case  "1":
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                    dr[2] = YTD.ToString();
                    dr[3] = JAN.ToString();
                    dr[4] = FEB.ToString();
                    dr[5] = MAR.ToString();
                    dr[6] = APR.ToString();
                    dr[7] = MAY.ToString();
                    dr[8] = JUN.ToString();
                    dr[9] = JUL.ToString();
                    dr[10] = AUG.ToString();
                    dr[11] = SEP.ToString();
                    dr[12] = OCT.ToString();
                    dr[13] = NOV.ToString();
                    dr[14] = DECE.ToString();
                    break;
                    case "2":
                    pYTD = ((double)(YTD * 100) / total);
                    pJAN = ((double)(JAN * 100) / JANTotal);
                    pFEB = ((double)(FEB * 100) / FEBTotal);
                    pMAR = ((double)(MAY * 100) / MARTotal);
                    pAPR = ((double)(APR * 100) / APRTotal);
                    pMAY = ((double)(MAY * 100) / MAYTotal);
                    pJUN = ((double)(JUN * 100) / JUNTotal);
                    pJUL = ((double)(JUL * 100) / JULTotal);
                    pAUG = ((double)(AUG * 100) / AUGTotal);
                    pSEP = ((double)(SEP * 100) / SEPTotal);
                    pOCT = ((double)(OCT * 100) / OCTTotal);
                    pNOV = ((double)(NOV * 100) / NOVTotal);
                    pDECE = ((double)(DECE * 100) / DECETotal);
                    
                    dr[0] = uniqrecipedt.Rows[i][0].ToString();
                    dr[1] = total.ToString();
                  

                    dr[2] = Math.Round(pYTD, 1).ToString() == "NaN" ? "" : Math.Round(pYTD, 1).ToString();
                    dr[3] = Math.Round(pJAN, 1).ToString() == "NaN" ? "" : Math.Round(pJAN, 1).ToString();
                    dr[4] = Math.Round(pFEB, 1).ToString() == "NaN" ? "" : Math.Round(pFEB, 1).ToString();
                    dr[5] = Math.Round(pMAR, 1).ToString() == "NaN" ? "" : Math.Round(pMAR, 1).ToString();
                    dr[6] = Math.Round(pAPR, 1).ToString() == "NaN" ? "" : Math.Round(pAPR, 1).ToString();
                    dr[7] = Math.Round(pMAY, 1).ToString() == "NaN" ? "" : Math.Round(pMAY, 1).ToString();
                    dr[8] = Math.Round(pJUN, 1).ToString() == "NaN" ? "" : Math.Round(pJUN, 1).ToString();
                    dr[9] = Math.Round(pJUL, 1).ToString() == "NaN" ? "" : Math.Round(pJUL, 1).ToString();
                    dr[10] = Math.Round(pAUG, 1).ToString() == "NaN" ? "" : Math.Round(pAUG, 1).ToString();
                    dr[11] = Math.Round(pSEP, 1).ToString() == "NaN" ? "" : Math.Round(pSEP, 1).ToString();
                    dr[12] = Math.Round(pOCT, 1).ToString() == "NaN" ? "" : Math.Round(pOCT, 1).ToString();
                    dr[13] = Math.Round(pNOV, 1).ToString() == "NaN" ? "" : Math.Round(pNOV, 1).ToString();
                    dr[14] = Math.Round(pDECE, 1).ToString() == "NaN" ? "" : Math.Round(pDECE, 1).ToString();

                    break;
                }

                gridviewdt.Rows.Add(dr);



            }
            DataRow ndr = gridviewdt.NewRow();
            ndr[0] = "";
            ndr[1] = "";
            ndr[2] = "";
            ndr[3] = "";
            ndr[4] = "";
            ndr[5] = "";
            ndr[6] = "";
            ndr[7] = "";

            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            switch (option)
            {
                case "1":
                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = YTD_;
                tdr[3] = JAN_;
                tdr[4] = FEB_;
                tdr[5] = MAR_;
                tdr[6] = APR_;
                tdr[7] = MAY_;
                tdr[8] = JUN_;
                tdr[9] = JUL_;
                tdr[10] = AUG_;
                tdr[11] = SEP_;
                tdr[12] = OCT_;
                tdr[13] = NOV_;
                tdr[14] = DECE_;
                    break;
                case"2" :
                pYTD_ = ((double)(YTD_ * 100) / total_);
                pJAN_ = ((double)(JAN_ * 100) / JANTotal_);
                pFEB_ = ((double)(FEB_ * 100) / FEBTotal_);
                pMAR_ = ((double)(MAR_ * 100) / MARTotal_);
                pAPR_ = ((double)(APR_ * 100) / APRTotal_);
                pMAY_ = ((double)(MAY_ * 100) / MAYTotal_);
                pJUN_ = ((double)(JUN_ * 100) / JUNTotal_);
                pJUL_ = ((double)(JUL_ * 100) / JULTotal_);
                pAUG_ = ((double)(AUG_ * 100) / AUGTotal_);
                pSEP_ = ((double)(SEP_ * 100) / SEPTotal_);
                pOCT_ = ((double)(OCT_ * 100) / OCTTotal_);
                pNOV_ = ((double)(NOV_ * 100) / NOVTotal_);
                pDECE_ = ((double)(DECE_ * 100) / DECETotal_);

                tdr[0] = "Total";
                tdr[1] = total_;
                tdr[2] = Math.Round(pYTD_, 1).ToString() == "NaN" ? "" : Math.Round(pYTD_, 1).ToString();
                tdr[3] = Math.Round(pJAN_, 1).ToString() == "NaN" ? "" : Math.Round(pJAN_, 1).ToString();
                tdr[4] = Math.Round(pFEB_, 1).ToString() == "NaN" ? "" : Math.Round(pFEB_, 1).ToString();
                tdr[5] = Math.Round(pMAR_, 1).ToString() == "NaN" ? "" : Math.Round(pMAR_, 1).ToString();
                tdr[6] = Math.Round(pAPR_, 1).ToString() == "NaN" ? "" : Math.Round(pAPR_, 1).ToString();
                tdr[7] = Math.Round(pMAY_, 1).ToString() == "NaN" ? "" : Math.Round(pMAY_, 1).ToString();
                tdr[8] = Math.Round(pJUN_, 1).ToString() == "NaN" ? "" : Math.Round(pJUN_, 1).ToString();
                tdr[9] = Math.Round(pJUL_, 1).ToString() == "NaN" ? "" : Math.Round(pJUL_, 1).ToString();
                tdr[10] = Math.Round(pAUG_, 1).ToString() == "NaN" ? "" : Math.Round(pAUG_, 1).ToString();
                tdr[11] = Math.Round(pSEP_, 1).ToString() == "NaN" ? "" : Math.Round(pSEP_, 1).ToString();
                tdr[12] = Math.Round(pOCT_, 1).ToString() == "NaN" ? "" : Math.Round(pOCT_, 1).ToString();
                tdr[13] = Math.Round(pNOV_, 1).ToString() == "NaN" ? "" : Math.Round(pNOV_, 1).ToString();
                tdr[14] = Math.Round(pDECE_, 1).ToString() == "NaN" ? "" : Math.Round(pDECE_, 1).ToString();


           
                break;
            }

            gridviewdt.Rows.Add(tdr);

            grandtotal += total_;
            grandYTD += YTD_;
            grandJAN += JAN_;
            grandFEB += FEB_;
            grandMAR += MAR_;
            grandAPR += APR_;
            grandMAY += MAY_;
            grandJUN += JUN_;
            grandJUL += JUL_;
            grandAUG += AUG_;
            grandSEP += SEP_;
            grandOCT += OCT_;
            grandNOV += NOV_;
            grandDECE += DECE_;

            if (QualityReportTBMWise.Checked)
            {
                childgridview.DataSource = gridviewdt;
                childgridview.DataBind();
            }
            else if (QualityReportRecipeWise.Checked)
            {
                performanceReportOAYGRAFRecipeWiseGridView.DataSource = gridviewdt;
                performanceReportOAYGRAFRecipeWiseGridView.DataBind();

            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=PerformanceOAYGrafReport" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
           Response.Charset = "";
           Response.ContentType = "application/vnd.xls";
           System.IO.StringWriter stringWrite = new System.IO.StringWriter();
           System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
           //stringWrite.Write("<table><tr><td><b>TBM Production Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + type + "</td><td><b>" + reportMasterWCProcessDropDownList.SelectedItem.Value + "</b></td></tr></table>");
           System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
           Controls.Add(form);
           
           if (QualityReportTBMWise.Checked)
           {
               QualityReportOAYGRAFTBMWisePanel.Visible = true;
               QualityReportOAYGRAFRecipeWisePanel.Visible = false;
               form.Controls.Add(QualityReportOAYGRAFTBMWisePanel);
           
           }
           else if (QualityReportRecipeWise.Checked)
           {
               QualityReportOAYGRAFTBMWisePanel.Visible = false;
               QualityReportOAYGRAFRecipeWisePanel.Visible = true;
               form.Controls.Add(QualityReportOAYGRAFRecipeWisePanel);
           
           } 
           
           form.RenderControl(htmlWrite);

           //gv.RenderControl(htmlWrite);
           Response.Write(stringWrite.ToString());
           Response.End(); 
           
           /*queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            tempString = magicHidden.Value.Split(new char[] { '?' });
            option = (tuoFilterOptionDropDownList.SelectedIndex == 0) ? "1" : ((tuoFilterOptionDropDownList.SelectedIndex == 1) ? "2" : "");
            tablename = (tempString2[tempString2.Length - 1].ToString() == "0") ? "vCuringWiseproductionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "1") ? "productionDataTUO" : ((tempString2[tempString2.Length - 1].ToString() == "2") ? "vproductionDataTUO" : null)); 
            
            if (tempString.Length > 1)
            {
                rType = tempString[0];
                rWCID = tempString[1];
                rChoice = tempString[2];
                rToDate = tempString[3];
                rFromDate = tempString[3];
                rToMonth = tempString[5];
                rToYear = tempString[7];
            }

            if (rChoice == "2")
            {
                notifyLabel.Visible = false;
                string query = myWebService.createQuery(rWCID);
                wcnamequery = myWebService.wcquery(query, tempString[tempString.Length - 1]);
                if (QualityReportTBMWise.Checked)
                {
                    QualityReportOAYGRAFTBMWisePanel.Visible = true;
                    excelReport(query);
                }
                else
                {
                    QualityReportOAYGRAFRecipeWisePanel.Visible = true;
                    excelReportRecipeWise("", rToYear);
                }

                fillchart();

            }
            else
            {
                notifyLabel.Visible = true;
                QualityReportOAYGRAFTBMWisePanel.Visible = false;
            }
            */
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
       public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        public void excelReport(string query)
        {
            showDownload.Text = "";
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query, con);
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

                    xlApp.Visible = true; // ensure that the excel app is visible.
                    xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                    Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

                    xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                    xlWorkSheet.Cells[1, 2] = "Average and standard Performance Report";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.get_Range("D3", "H3").Merge(misValue);
                    xlWorkSheet.get_Range("H3", "P3").Merge(misValue);

                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "P3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "Year : " + rToYear;
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                    xlWorkSheet.Cells[3, 1] = "Machine Name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "UNIFORMITY GRADE WITH A & B RANK";

                    xlWorkSheet.Cells[4, 4] = "YTD";
                    xlWorkSheet.Cells[4, 5] = "JAN";
                    xlWorkSheet.Cells[4, 6] = "FEB";
                    xlWorkSheet.Cells[4, 7] = "MAR";
                    xlWorkSheet.Cells[4, 8] = "APR";
                    xlWorkSheet.Cells[4, 9] = "MAY";
                    xlWorkSheet.Cells[4, 10] = "JUN";
                    xlWorkSheet.Cells[4, 11] = "JUL";
                    xlWorkSheet.Cells[4, 12] = "AUG";
                    xlWorkSheet.Cells[4, 13] = "SEP";
                    xlWorkSheet.Cells[4, 14] = "OCT";
                    xlWorkSheet.Cells[4, 15] = "NOV";
                    xlWorkSheet.Cells[4, 16] = "DEC";

                    ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                    xlWorkSheet.get_Range("A4", "C4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("D4", "P4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                    xlWorkSheet.get_Range("A3", "P3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("A3", "P3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    xlWorkSheet.get_Range("A4", "P4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                    while (dr.Read())
                    {
                        excelReportRecipeWise(dr["workCenterName"].ToString().Trim(), rToYear);
                    }
                    xlWorkSheet.get_Range("A" + (rowCount + 1), "P" + (rowCount + 1)).Merge(misValue);
                    xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                    xlWorkSheet.Cells[rowCount + 2, 3] = grandtotal.ToString();
                    switch (option)
                    {
                        case "1":
                            xlWorkSheet.Cells[rowCount + 2, 4] = grandYTD.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 5] = grandJAN.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 6] = grandFEB.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 7] = grandMAR.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 8] = grandAPR.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 9] = grandMAY.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 10] = grandJUN.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 11] = grandJUL.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 12] = grandAUG.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 13] = grandSEP.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 14] = grandOCT.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 15] = grandNOV.ToString();
                            xlWorkSheet.Cells[rowCount + 2, 16] = grandDECE.ToString();
                            break;
                        case "2":
                            xlWorkSheet.Cells[rowCount + 2, 4] = ((grandtotal == 0) ? 0 : ((double)(grandYTD * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 5] = ((grandtotal == 0) ? 0 : ((double)(grandJAN * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 6] = ((grandtotal == 0) ? 0 : ((double)(grandFEB * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 7] = ((grandtotal == 0) ? 0 : ((double)(grandMAR * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 8] = ((grandtotal == 0) ? 0 : ((double)(grandAPR * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 9] = ((grandtotal == 0) ? 0 : ((double)(grandMAY * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 10] = ((grandtotal == 0) ? 0 : ((double)(grandJUN * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 11] = ((grandtotal == 0) ? 0 : ((double)(grandJUL * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 12] = ((grandtotal == 0) ? 0 : ((double)(grandAUG * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 13] = ((grandtotal == 0) ? 0 : ((double)(grandSEP * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 14] = ((grandtotal == 0) ? 0 : ((double)(grandOCT * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 15] = ((grandtotal == 0) ? 0 : ((double)(grandNOV * 100) / grandtotal));
                            xlWorkSheet.Cells[rowCount + 2, 16] = ((grandtotal == 0) ? 0 : ((double)(grandDECE * 100) / grandtotal));
                            break;
                    }
                    xlWorkSheet.get_Range("A1", "P" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "P" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report Curing WC wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
                }
            }
            catch (Exception exp)
            {
                //myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                KillProcess(pid, "EXCEL");
            }
        }
        public void excelReportRecipeWise(string wcName, string rtoYear)
        {
            string query;
            DataTable dt = new DataTable();
            DataTable gridviewdt = new DataTable();

            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("YTD", typeof(string));
            gridviewdt.Columns.Add("JAN", typeof(string));
            gridviewdt.Columns.Add("FEB", typeof(string));
            gridviewdt.Columns.Add("MAR", typeof(string));
            gridviewdt.Columns.Add("APR", typeof(string));
            gridviewdt.Columns.Add("MAY", typeof(string));
            gridviewdt.Columns.Add("JUN", typeof(string));
            gridviewdt.Columns.Add("JUL", typeof(string));
            gridviewdt.Columns.Add("AUG", typeof(string));
            gridviewdt.Columns.Add("SEP", typeof(string));
            gridviewdt.Columns.Add("OCT", typeof(string));
            gridviewdt.Columns.Add("NOV", typeof(string));
            gridviewdt.Columns.Add("DECE", typeof(string));

            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            dt.Columns.Add("TestTime", typeof(int));
            int total, YTD, JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DECE;
            Double pYTD, pJAN, pFEB, pMAR, pAPR, pMAY, pJUN, pJUL, pAUG, pSEP, pOCT, pNOV, pDECE;


            query = "";
            int colCount = 1, mergeCount = 0, typeCount = 0;
           
            if (QualityReportRecipeWise.Checked)
            {
                typeCount = 0;
                xlApp = new Excel.ApplicationClass();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkBook.CheckCompatibility = false;
                xlWorkBook.DoNotPromptForConvert = true;

                xlApp.Visible = true; // ensure that the excel app is visible.
                xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

                xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                xlWorkSheet.Cells[1, 2] = "Average and standard Performance Report";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("C3", "O3").Merge(misValue);

                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("C3", "O3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "Year : " + rToYear;
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "UNIFORMITY GRADE WITH A & B RANK";

                xlWorkSheet.Cells[4, 3] = "YTD";
                xlWorkSheet.Cells[4, 4] = "JAN";
                xlWorkSheet.Cells[4, 5] = "FEB";
                xlWorkSheet.Cells[4, 6] = "MAR";
                xlWorkSheet.Cells[4, 7] = "APR";
                xlWorkSheet.Cells[4, 8] = "MAY";
                xlWorkSheet.Cells[4, 9] = "JUN";
                xlWorkSheet.Cells[4, 10] = "JUL";
                xlWorkSheet.Cells[4, 11] = "AUG";
                xlWorkSheet.Cells[4, 12] = "SEP";
                xlWorkSheet.Cells[4, 13] = "OCT";
                xlWorkSheet.Cells[4, 14] = "NOV";
                xlWorkSheet.Cells[4, 15] = "DEC";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A4", "B4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("C4", "O4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("A3", "O3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "O3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "O4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM  " + tablename + "  WHERE ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade, CONVERT(int,DATEPART(MM,TestTime)) AS TestTime FROM " + tablename + " WHERE tireType in(select name from recipeMaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ";
                    dt = getFilledDT(query, rtoYear);
                }

            }
            else if (QualityReportTBMWise.Checked)
            {
                typeCount = 1;
                query = (tempString2[tempString2.Length - 1].ToString() == "0") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "1") ? getqueryWCWise(tablename, "machineName='" + wcName + "'", rtoYear) : ((tempString2[tempString2.Length - 1].ToString() == "2") ? getqueryWCWise(tablename, "wcname='" + wcName + "'", rtoYear) : ""));
                dt = getFilledDT(query, rtoYear); 
                xlWorkSheet.Cells[rowCount + 1, 1] = wcName;
                mergeCount = rowCount;
            }
            globaldt.Merge(dt);
            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");

            total_ = 0; YTD_ = 0; JAN_ = 0; FEB_ = 0; MAR_ = 0; APR_ = 0; MAY_ = 0; JUN_ = 0; JUL_ = 0; AUG_ = 0; SEP_ = 0; OCT_ = 0; NOV_ = 0; DECE_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; YTD = 0; JAN = 0; FEB = 0; MAR = 0; APR = 0; MAY = 0; JUN = 0; JUL = 0; AUG = 0; SEP = 0; OCT = 0; NOV = 0; DECE = 0;
                pYTD = 0; pJAN = 0; pFEB = 0; pMAR = 0; pAPR = 0; pMAY = 0; pJUN = 0; pJUL = 0; pAUG = 0; pSEP = 0; pOCT = 0; pNOV = 0; pDECE = 0; rowCount++;
                total = (int)dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                YTD = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B')"));
                JAN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and  UniformityGrade in('A','B') AND TestTime='01'"));
                FEB = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='02'"));
                MAR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='03'"));
                APR = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='04'"));
                MAY = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='05'"));
                JUN = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='06'"));
                JUL = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='07'"));
                AUG = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='08'"));
                SEP = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='09'"));
                OCT = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='10'"));
                NOV = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='11'"));
                DECE = (int)(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  UniformityGrade in('A','B') AND TestTime='12'"));

                total_ += total;
                YTD_ += YTD;
                JAN_ += JAN;
                FEB_ += FEB;
                MAR_ += MAR;
                APR_ += APR;
                MAY_ += MAY;
                JUN_ += JUN;
                JUL_ += JUL;
                AUG_ += AUG;
                SEP_ += SEP;
                OCT_ += OCT;
                NOV_ += NOV;
                DECE_ += DECE;

                DataRow dr = gridviewdt.NewRow();
                switch(option)
                {
                    case  "1" :
                    xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = YTD.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = JAN.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = FEB.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = MAR.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = APR.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = MAY.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = JUN.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = JUL.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = AUG.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = SEP.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = OCT.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = NOV.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = DECE.ToString();
                    break;
                    case "2" :
                    pYTD = ((total_ == 0) ? 0 : ((double)(YTD * 100) / total));
                    pJAN = ((total_ == 0) ? 0 : ((double)(JAN * 100) / total));
                    pFEB = ((total_ == 0) ? 0 : ((double)(FEB * 100) / total));
                    pMAR = ((total_ == 0) ? 0 : ((double)(MAY * 100) / total));
                    pAPR = ((total_ == 0) ? 0 : ((double)(APR * 100) / total));
                    pMAY = ((total_ == 0) ? 0 : ((double)(MAY * 100) / total));
                    pJUN = ((total_ == 0) ? 0 : ((double)(JUN * 100) / total));
                    pJUL = ((total_ == 0) ? 0 : ((double)(JUL * 100) / total));
                    pAUG = ((total_ == 0) ? 0 : ((double)(AUG * 100) / total));
                    pSEP = ((total_ == 0) ? 0 : ((double)(SEP * 100) / total));
                    pOCT = ((total_ == 0) ? 0 : ((double)(OCT * 100) / total));
                    pNOV = ((total_ == 0) ? 0 : ((double)(NOV * 100) / total));
                    pDECE = ((total_ == 0) ? 0 : ((double)(DECE * 100) / total));

                    xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pYTD, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pJAN, 1);

                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pFEB, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pMAR, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pAPR, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = Math.Round(pMAY, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = Math.Round(pJUN, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = Math.Round(pJUL, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = Math.Round(pAUG, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = Math.Round(pSEP, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = Math.Round(pOCT, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = Math.Round(pNOV, 1);
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = Math.Round(pDECE, 1);
                    break;
                }

                gridviewdt.Rows.Add(dr);
            }
            rowCount += 2;
            DataRow ndr = gridviewdt.NewRow();
            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            switch(option)
            {
                case  "1" :
                    xlWorkSheet.Cells[rowCount, colCount + typeCount] = "Total";
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = YTD_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = JAN_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = FEB_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = MAR_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = APR_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = MAY_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = JUN_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = JUL_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = AUG_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = SEP_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = OCT_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = NOV_;
                    xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = DECE_;
                    break;
                case "2" :
                pYTD_ = ((total_ == 0) ? 0 : ((double)(YTD_ * 100) / total_));
                pJAN_ = ((total_ == 0) ? 0 : ((double)(JAN_ * 100) / total_));
                pFEB_ = ((total_ == 0) ? 0 : ((double)(FEB_ * 100) / total_));
                pMAR_ = ((total_ == 0) ? 0 : ((double)(MAR_ * 100) / total_));
                pAPR_ = ((total_ == 0) ? 0 : ((double)(APR_ * 100) / total_));
                pMAY_ = ((total_ == 0) ? 0 : ((double)(MAY_ * 100) / total_));
                pJUN_ = ((total_ == 0) ? 0 : ((double)(JUN_ * 100) / total_));
                pJUL_ = ((total_ == 0) ? 0 : ((double)(JUL_ * 100) / total_));
                pAUG_ = ((total_ == 0) ? 0 : ((double)(AUG_ * 100) / total_));
                pSEP_ = ((total_ == 0) ? 0 : ((double)(SEP_ * 100) / total_));
                pOCT_ = ((total_ == 0) ? 0 : ((double)(OCT_ * 100) / total_));
                pNOV_ = ((total_ == 0) ? 0 : ((double)(NOV_ * 100) / total_));
                pDECE_ = ((total_ == 0) ? 0 : ((double)(DECE_ * 100) / total_));

                xlWorkSheet.Cells[rowCount, colCount + typeCount + 0] = "Total";
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total_;
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pYTD_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pJAN_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pFEB_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pMAR_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pAPR_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 7] = Math.Round(pMAY_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 8] = Math.Round(pJUN_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 9] = Math.Round(pJUL_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 10] = Math.Round(pAUG_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 11] = Math.Round(pSEP_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 12] = Math.Round(pOCT_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 13] = Math.Round(pNOV_, 1);
                xlWorkSheet.Cells[rowCount, colCount + typeCount + 14] = Math.Round(pDECE_, 1);
                break;
            }

            gridviewdt.Rows.Add(tdr);

            grandtotal += total_;
            grandYTD += YTD_;
            grandJAN += JAN_;
            grandFEB += FEB_;
            grandMAR += MAR_;
            grandAPR += APR_;
            grandMAY += MAY_;
            grandJUN += JUN_;
            grandJUL += JUL_;
            grandAUG += AUG_;
            grandSEP += SEP_;
            grandOCT += OCT_;
            grandNOV += NOV_;
            grandDECE += DECE_;

            if (QualityReportTBMWise.Checked)
            {
                
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "O" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "O" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "O" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report Curing WC wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
               
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
        string NullIf(string num)
        {
            return (num == "0" || num == "0%") ? "" : num.ToString();
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell;

                if (performanceReportOAYGRAFWiseMainGridView.Rows.Count != 0)
                {
                    cell = new TableHeaderCell();
                    cell.Text = "";
                    row.Controls.Add(cell);
                }
                if (performanceReportOAYGRAFRecipeWiseGridView.Rows.Count != 0 || performanceReportOAYGRAFWiseMainGridView.Rows.Count != 0)
                {
                    cell = new TableHeaderCell();
                    cell.Text = "";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.Text = "";
                    row.Controls.Add(cell);

                    cell = new TableHeaderCell();
                    cell.Text = "UNIFORMITY GRADE WITH A & B RANK";
                    cell.ColumnSpan = 13;
                    row.Controls.Add(cell);



                    if (performanceReportOAYGRAFRecipeWiseGridView.Rows.Count != 0)
                    {
                        performanceReportOAYGRAFRecipeWiseGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                    if (performanceReportOAYGRAFWiseMainGridView.Rows.Count != 0)
                    {
                        performanceReportOAYGRAFWiseMainGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

    }
}
