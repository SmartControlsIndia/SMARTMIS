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
    public partial class waveformReportNew : System.Web.UI.Page
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
            DataTable dtwaveform = new DataTable();
            DataTable productionTUOdt = new DataTable();
            DataTable productionDataDBNewdt = new DataTable();
            DataTable maindt = new DataTable();

            DataRow dr;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                fromdate = myWebService.formatDate(fromdatecalendertextbox.Text.ToString()) + " 07:00:00";
                todate = myWebService.formatDate(TodateCalendertextbox.Text.ToString()) + " 07:00:00";

                string fromDate = myWebService.formatDate(fromdatecalendertextbox.Text.ToString());
                string toDate = myWebService.formatDate(TodateCalendertextbox.Text.ToString());

                TimeSpan ts = DateTime.Parse(toDate) - DateTime.Parse(fromDate);
                int result = (int)ts.TotalDays;
                if ((int)ts.TotalDays > 7)
                {
                    ShowWarning.Visible = true;
                    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more  than 7 days!!!</font></strong></td></tr></table>";
                }

                try
                {
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select distinct [Production_ID],[MachineName],[BARCODE],[TestTime],[State] ,[RadialHarmonicPhase1] ,[isClockwise] ,[RadialHarmonicMagnitude4],[RadialHarmonicPhase4],[RadialHarmonicMagnitude3] ,[RadialHarmonicPhase3],[RadialHarmonicMagnitude2] ,[RadialHarmonicPhase2],[RadialHarmonicMagnitude1],[RadialHarmonicPhase5] ,[RadialHarmonicMagnitude5],[RadialHarmonicPhase6],[RadialHarmonicMagnitude6],[RadialHarmonicMagnitude7],[RadialHarmonicPhase7],[RadialHarmonicPhase8],[RadialHarmonicMagnitude8],[RadialHarmonicPhase9],[RadialHarmonicMagnitude9],[RadialHarmonicPhase10],[RadialHarmonicMagnitude10],[LateralHarmonicPhase1] ,[LateralHarmonicMagnitude1] ,[LateralHarmonicPhase2] ,[LateralHarmonicMagnitude2],[LateralHarmonicPhase3],[LateralHarmonicMagnitude3],[LateralHarmonicPhase4],[LateralHarmonicMagnitude4],[LateralHarmonicPhase5],[LateralHarmonicMagnitude5],[LateralHarmonicPhase6],LateralHarmonicPhase7,[LateralHarmonicMagnitude6],[LateralHarmonicPhase10],[LateralHarmonicMagnitude9],[LateralHarmonicPhase9] ,[LateralHarmonicMagnitude8],[LateralHarmonicPhase8] ,[LateralHarmonicMagnitude7],[LateralHarmonicMagnitude10],[TireType] ,[RFV] ,[H1RFV],[H2RFV] ,[LFV],[PEAK],[HNRFV],[UpperRRO] ,[RRO],[ConicityPolarity],[UniformityGrade] ,[CONICITY],[PLY] ,[H1LFV],[H1LowerLRO],[H1UpperLRO],[LowerLRO],[UpperLRO] ,[LowerH1RRO],[UpperH1RRO] ,[LowerRRO],[H1RRO],[WOBBLE],[LowerDepression],[UpperDepression],[LowerBulge],[UpperBulge],[RFVCW] ,[H1RFVCW],[H2RFVCW] ,[HNRFVCW],[PEAKCW],[LFVCW],[H1LFVCW],[GradeH2RFVCW],[GradeH1RFVCW],[GradeRFVCW],[H1LFVCCW] ,[LFVCCW],[PEAKCCW],[HNRFVCCW] ,[H2RFVCCW] ,[H1RFVCCW] ,[RFVCCW],[GradeRFVCCW] ,[GradeH1LFVCW] ,[GradeLFVCW] ,[GradePEAKCW] ,[GradeHNRFVCW] ,[GradePLY],[GradeCONICITY] ,[GradeH1LFVCCW] ,[GradeLFVCCW],[GradePEAKCCW],[GradeHNRFVCCW],[GradeH2RFVCCW] ,[GradeH1RFVCCW] ,[GradeUpperLRO] ,[GradeLowerH1RRO],[GradeUpperH1RRO],[GradeH1RRO] ,[GradeLowerRRO],[GradeUpperRRO] ,[GradeLowerDepression] ,[GradeUpperDepression] ,[GradeLowerBulge],[GradeUpperBulge],[GradeRRO],[UniformityBeadDiameter] ,[UniformityRimWidth] ,[UniformityInflationPress] , [LoadForce] ,[Circumference] ,[SpringRate],[GradeWobble],[GradeH1LowerLRO],[GradeH1UpperLRO] ,[GradeLowerLRO]Upper,Lower  ,GRADEDEF,BARCODE ,RFVCW,csv_H1RFVCW,csv_H2RFVCW,HNRFVCW,PEAKCW,LFVCW,H1LFVCW,RFVCCW ,c_H1RFVCCW,H2RFVCCW,HNRFVCCW  ,PEAKCCW,LFVCCW ,H1LFVCCW ,PLY,CONICITY ,c_RRO,GradeRRO ,UpperLRO,GradeUpperLRO ,c_LowerLRO  ,GradeLowerLRO ,UpperBulge ,c_GradeUpperBulge ,LowerBulge ,GradeLowerBulge ,c_UpperDepression  ,GradeUpperDepression ,LowerDepression ,GradeLowerDepression,SPOT ,GradeSpot  ,Wobble ,GradeWobble,Static  ,StaticAngle ,StaticGrade,Couple ,CoupleAngle,CoupleGrade ,UpperAngle ,UpperGrade ,LowerGrade ,LowerAngle  ,Weight ,WeightGrade FROM [vwaveformdata] where  TestTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and barcode !='' and machinename='30-353' order by Testtime desc";
                    myConnection.comm.CommandTimeout = 720;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtwaveform.Load(myConnection.reader);
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
              
                MainGridView.DataSource = dtwaveform;
                MainGridView.DataBind();
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
            }
            return true;
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {

            try
            {
                Response.Clear();

                Response.AddHeader("content-disposition", "attachment; filename=waveform.xls");
                Response.Charset = "";

                Response.ContentType = "application/vnd.xls";

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

                MainGridView.RenderControl(htmlWriter);
                Response.Write(stringWriter.ToString());

                Response.End();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}
