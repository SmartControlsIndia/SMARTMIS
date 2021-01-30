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
    public partial class waveformReport : System.Web.UI.Page
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
            DataTable dtwaveform = new DataTable();
            DataTable productionTUOdt = new DataTable();
            DataTable productionDataDBNewdt = new DataTable();
            DataTable maindt = new DataTable();

            try
            {
                //maindt.Columns.Add("MachineName", typeof(string));
                //maindt.Columns.Add("Production_ID", typeof(string));
                //maindt.Columns.Add("TestTime", typeof(string));
                //maindt.Columns.Add("BARCODE", typeof(string));
                //maindt.Columns.Add("State", typeof(string));
                ////maindt.Columns.Add("isClockwise", typeof(string));
                //maindt.Columns.Add("Lateral HarmonicPhase1", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude1", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase2", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude2", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase3", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude3", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase4", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude4", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase5", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude5", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase6", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude6", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase7", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude7", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase8", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude8", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase9", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude9", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicPhase10", typeof(Double));
                //maindt.Columns.Add("Lateral HarmonicMagnitude10", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase1", typeof(string));
                //maindt.Columns.Add("Radial HarmonicMagnitude1", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude2", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase2", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase3", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude3", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase4", typeof(float));
                //maindt.Columns.Add("Radial HarmonicMagnitude4", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase5", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude5", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase6", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude6", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude7", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase7", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicPhase8", typeof(Double));
                //maindt.Columns.Add("Radial HarmonicMagnitude8", typeof(int));
                //maindt.Columns.Add("Radial HarmonicPhase9", typeof(Double));
                ////maindt.Columns.Add("RadialHarmonicPhase1", typeof(string));
                //maindt.Columns.Add("RadialHarmonicMagnitude9", typeof(Double));
                //maindt.Columns.Add("RadialHarmonicPhase10", typeof(Double));
                //maindt.Columns.Add("RadialHarmonicMagnitude10", typeof(Double));
                //maindt.Columns.Add("TireType", typeof(string));
                //maindt.Columns.Add("RFV", typeof(Double));
                //maindt.Columns.Add("H1RFV", typeof(Double));
                //maindt.Columns.Add("H2RFV", typeof(Double));
                //maindt.Columns.Add("HNRFV", typeof(Double));
                //maindt.Columns.Add("PEAK", typeof(Double));
                //maindt.Columns.Add("LFV", typeof(Double));
                //maindt.Columns.Add("H1LFV", typeof(string));

                //maindt.Columns.Add("PLY", typeof(string));
                //maindt.Columns.Add("CONICITY", typeof(Double));
                //maindt.Columns.Add("UniformityGrade", typeof(string));
                //maindt.Columns.Add("ConicityPolarity", typeof(Double));
                //maindt.Columns.Add("RRO", typeof(Double));
                //maindt.Columns.Add("UpperRRO", typeof(Double));
                //maindt.Columns.Add("LowerRRO", typeof(Double));
                //maindt.Columns.Add("H1RRO", typeof(Double));
                //maindt.Columns.Add("UpperH1RRO", typeof(Double));
                //maindt.Columns.Add("LowerH1RRO", typeof(Double));
                //maindt.Columns.Add("UpperLRO", typeof(Double));
                //maindt.Columns.Add("LowerLRO", typeof(Double));
                //maindt.Columns.Add("H1LowerLRO", typeof(Double));
                //maindt.Columns.Add("H1UpperLRO", typeof(int));
                //maindt.Columns.Add("UpperBulge", typeof(Double));
                //maindt.Columns.Add("LowerBulge", typeof(Double));
                //maindt.Columns.Add("UpperDepression", typeof(Double));
                //maindt.Columns.Add("LowerDepression", typeof(string));
                //maindt.Columns.Add("WOBBLE", typeof(Double));
                //maindt.Columns.Add("proBARCODE", typeof(string));
                //maindt.Columns.Add("Production_TIME", typeof(string));


                //// maindt.Columns.Add("CSV_TIME", typeof(string));
                //maindt.Columns.Add("RFVCW", typeof(Double));
                //maindt.Columns.Add("H1RFVCW", typeof(Double));
                //maindt.Columns.Add("H2RFVCW", typeof(Double));
                //maindt.Columns.Add("HNRFVCW", typeof(Double));
                //maindt.Columns.Add("PEAKCW", typeof(Double));
                //maindt.Columns.Add("LFVCW", typeof(string));
                //maindt.Columns.Add("H1LFVCW", typeof(Double));
                //maindt.Columns.Add("RFVCCW", typeof(Double));
                //maindt.Columns.Add("H1RFVCCW", typeof(Double));
                //maindt.Columns.Add("H2RFVCCW", typeof(Double));
                //maindt.Columns.Add("HNRFVCCW", typeof(string));
                //maindt.Columns.Add("PEAKCCW", typeof(string));

                //maindt.Columns.Add("LFVCCW", typeof(string));
                //maindt.Columns.Add("H1LFVCCW", typeof(string));

                //maindt.Columns.Add("Grade RFVCW", typeof(string));
                //maindt.Columns.Add("Grade H1RFVCW", typeof(string));
                //maindt.Columns.Add("Grade H2RFVCW", typeof(string));
                //maindt.Columns.Add("Grade HNRFVCW", typeof(string));
                //maindt.Columns.Add("Grade PEAKCW", typeof(string));
                //maindt.Columns.Add("Grade LFVCW", typeof(string));
                //maindt.Columns.Add("Grade H1LFVCW", typeof(string));
                //maindt.Columns.Add("Grade RFVCCW", typeof(string));
                //maindt.Columns.Add("Grade H1RFVCCW", typeof(string));
                //maindt.Columns.Add("Grade H2RFVCCW", typeof(string));
                //maindt.Columns.Add("Grade HNRFVCCW", typeof(string));
                //maindt.Columns.Add("Grade PEAKCCW", typeof(string));
                //maindt.Columns.Add("Grade LFVCCW", typeof(string));
                //maindt.Columns.Add("Grade H1LFVCCW", typeof(string));
                //maindt.Columns.Add("Grade CONICITY", typeof(string));
                //maindt.Columns.Add("Grade PLY", typeof(string));
                //maindt.Columns.Add("Grade UpperBulge", typeof(string));
                //maindt.Columns.Add("Grade LowerBulge", typeof(string));
                //maindt.Columns.Add("Grade UpperDepression", typeof(string));
                //maindt.Columns.Add("Grade LowerDepression", typeof(string));
                //maindt.Columns.Add("Grade RRO", typeof(string));
                //maindt.Columns.Add("Grade UpperRRO", typeof(string));
                //maindt.Columns.Add("Grade LowerRRO", typeof(string));
                //maindt.Columns.Add("Grade H1RRO", typeof(string));
                //maindt.Columns.Add("Grade LowerH1RRO", typeof(string));
                //maindt.Columns.Add("Grade UpperH1RRO", typeof(string));
                //maindt.Columns.Add("Grade LowerLRO", typeof(string));
                //maindt.Columns.Add("Grade UpperLRO", typeof(string));
                //maindt.Columns.Add("Grade H1UpperLRO", typeof(string));
                //maindt.Columns.Add("Grade H1LowerLRO", typeof(string));
                //maindt.Columns.Add("Grade Wobble", typeof(string));
                //maindt.Columns.Add("SpringRate", typeof(Double));
                //maindt.Columns.Add("Circumference", typeof(Double));
                //maindt.Columns.Add("LoadForce", typeof(Double));
                //maindt.Columns.Add("Uniformity InflationPress", typeof(Double));
                //maindt.Columns.Add("Uniformity RimWidth", typeof(Double));
                //maindt.Columns.Add("Uniformity BeadDiameter", typeof(Double));


                ////csv column name
                //maindt.Columns.Add("1RFVCW", typeof(Double));
                //maindt.Columns.Add("1H1RFVCW", typeof(Double));
                //maindt.Columns.Add("1H2RFVCW", typeof(Double));
                //maindt.Columns.Add("1HNRFVCW", typeof(Double));
                //maindt.Columns.Add("1PEAKCW", typeof(Double));
                //maindt.Columns.Add("1LFVCW", typeof(string));
                //maindt.Columns.Add("1H1LFVCW", typeof(Double));
                //maindt.Columns.Add("1RFVCCW", typeof(Double));
                //maindt.Columns.Add("1H1RFVCCW", typeof(Double));
                //maindt.Columns.Add("1H2RFVCCW", typeof(Double));
                //maindt.Columns.Add("1HNRFVCCW", typeof(Double));
                //maindt.Columns.Add("1PEAKCCW", typeof(Double));
                //maindt.Columns.Add("1LFVCCW", typeof(Double));
                //maindt.Columns.Add("1H1LFVCCW", typeof(Double));
                //maindt.Columns.Add("1PLY", typeof(int));
                //maindt.Columns.Add("Conicity", typeof(Double));
                //maindt.Columns.Add("1RRO", typeof(Double));
                //maindt.Columns.Add("1GradeRRO", typeof(Double));
                //maindt.Columns.Add("1LowerGradeLRO", typeof(Double));
                //maindt.Columns.Add("1UpperLRO", typeof(Double));
                //maindt.Columns.Add("1UpperGradeLRO", typeof(Double));
                //maindt.Columns.Add("1LowerLRO", typeof(Double));
                ////  maindt.Columns.Add("1LowerGradeLRO", typeof(Double));
                //maindt.Columns.Add("1UpperBulge", typeof(Double));
                //maindt.Columns.Add("1GradeUpperBulge", typeof(int));
                //maindt.Columns.Add("1LowerBulge", typeof(Double));
                //maindt.Columns.Add("1GradeLowerBulge", typeof(int));
                //maindt.Columns.Add("CSV UpperDepression", typeof(string));
                //maindt.Columns.Add("CSV GradeUpperDepression", typeof(string));
                //maindt.Columns.Add("CSV LowerDepression", typeof(string));
                //maindt.Columns.Add("CSV GradeLowerDepression", typeof(int));
                //maindt.Columns.Add("SPOT", typeof(int));
                //maindt.Columns.Add("GradeSpot", typeof(int));
                //maindt.Columns.Add("Wobble", typeof(int));
                //maindt.Columns.Add("1GradeWobble", typeof(int));
                //maindt.Columns.Add("Static", typeof(int));
                //maindt.Columns.Add("StaticAngle", typeof(int));
                //maindt.Columns.Add("StaticGrade", typeof(int));
                //maindt.Columns.Add("Couple", typeof(int));
                //maindt.Columns.Add("CoupleAngle", typeof(int));
                //maindt.Columns.Add("CoupleGrade", typeof(int));
                //maindt.Columns.Add("Upper", typeof(int));
                //maindt.Columns.Add("UpperAngle", typeof(int));
                //maindt.Columns.Add("UpperGrade", typeof(int));
                //maindt.Columns.Add("Lower", typeof(int));
                //maindt.Columns.Add("LowerAngle", typeof(int));
                //maindt.Columns.Add("LowerGrade", typeof(int));
                //maindt.Columns.Add("Weight", typeof(int));
                //maindt.Columns.Add("WeightGrade", typeof(int));
                //maindt.Columns.Add("GRADEDEF", typeof(int));
                ////  maindt.Columns.Add("1Circumference", typeof(int));
                //maindt.Columns.Add("CSV BARCODE", typeof(string));

            }
            catch (Exception ex)
            {

                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
           
            

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

                List<string> conditions = new List<string>();
                string query = "";

                //Get waveform and TUO details

                //try
                //{
                //    myConnection.open(ConnectionOption.SQL);
                //    myConnection.comm = myConnection.conn.CreateCommand();
                //    myConnection.comm.CommandText = "Select  MachineName,Production_ID ,TestTime,BARCODE ,State,LateralHarmonicPhase1,LateralHarmonicMagnitude1,LateralHarmonicPhase2,LateralHarmonicMagnitude2 ,LateralHarmonicPhase3,LateralHarmonicMagnitude3 ,LateralHarmonicPhase4,LateralHarmonicMagnitude4,LateralHarmonicPhase5,LateralHarmonicMagnitude5 ,LateralHarmonicPhase6,LateralHarmonicMagnitude6,LateralHarmonicPhase7,LateralHarmonicMagnitude7,LateralHarmonicPhase8 ,LateralHarmonicMagnitude8 ,LateralHarmonicPhase9,LateralHarmonicMagnitude9 ,LateralHarmonicPhase10,[LateralHarmonicMagnitude9],RadialHarmonicPhase1,RadialHarmonicMagnitude1,RadialHarmonicPhase2 ,RadialHarmonicMagnitude2,RadialHarmonicPhase3,RadialHarmonicMagnitude3,RadialHarmonicPhase4,RadialHarmonicMagnitude4 ,RadialHarmonicPhase5,RadialHarmonicMagnitude5,RadialHarmonicPhase6,RadialHarmonicMagnitude6,RadialHarmonicPhase7,RadialHarmonicMagnitude7,RadialHarmonicPhase8,RadialHarmonicMagnitude8,RadialHarmonicPhase9,RadialHarmonicMagnitude9,RadialHarmonicPhase10,RadialHarmonicMagnitude10 from WaveformData where MachineName='30-353' and TestTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and barcode !='' and barcode !='no barcode' and barcode in (select barcode from productionDataTUO) ";
                //    myConnection.comm.CommandTimeout = 120;
                //    myConnection.reader = myConnection.comm.ExecuteReader();
                //    dtwaveform.Load(myConnection.reader);
                //    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}
                //catch (Exception exc)
                //{
                //    myWebService.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}
                //finally
                //{
                //    if (!myConnection.reader.IsClosed)
                //        myConnection.reader.Close();
                //    myConnection.comm.Dispose();
                //}


                ////Get ProductionTUO details

                //try
                //{
                //    myConnection.open(ConnectionOption.SQL);
                //    myConnection.comm = myConnection.conn.CreateCommand();
                //    myConnection.comm.CommandText = "Select TireType,RFV,H1RFV,H2RFV,HNRFV ,PEAK ,LFV,H1LFV,PLY,CONICITY,UniformityGrade,ConicityPolarity,RRO,UpperRRO  ,LowerRRO,H1RRO,UpperH1RRO ,LowerH1RRO ,UpperLRO,LowerLRO,H1UpperLRO,H1LowerLRO,UpperBulge ,LowerBulge ,UpperDepression ,LowerDepression,WOBBLE,BARCODE ,TestTime ,RFVCW ,H1RFVCW ,H2RFVCW,HNRFVCW ,PEAKCW,LFVCW,H1LFVCW,RFVCCW,H1RFVCCW,H2RFVCCW,HNRFVCCW,PEAKCCW,LFVCCW,H1LFVCCW ,GradeRFVCW ,GradeH1RFVCW,GradeH2RFVCW,GradeHNRFVCW,GradePEAKCW,GradeLFVCW ,GradeH1LFVCW,GradeRFVCCW,GradeH1RFVCCW,GradeH2RFVCCW,GradeHNRFVCCW,GradePEAKCCW,GradeLFVCCW,GradeH1LFVCCW,GradeCONICITY ,GradePLY ,GradeUpperBulge ,GradeLowerBulge,GradeUpperDepression,GradeLowerDepression,GradeRRO,GradeUpperRRO ,GradeLowerRRO,GradeH1RRO,GradeUpperH1RRO,GradeLowerH1RRO,GradeUpperLRO,GradeLowerLRO,GradeH1UpperLRO,GradeH1LowerLRO ,GradeWobble,SpringRate,Circumference,LoadForce,UniformityInflationPress ,UniformityRimWidth,UniformityBeadDiameter from productionDataTUO where MachineName='30-353' and TestTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and barcode !='' and MachineName='30-353' ";
                //    myConnection.comm.CommandTimeout = 120;
                //    //myConnection.comm.CommandText = "Select RFV,H1RFV,H2RFV,HNRFV ,PEAK ,LFV,H1LFV,PLY,CONICITY,UniformityGrade,ConicityPolarity,RRO,UpperRRO  ,LowerRRO,H1RRO,UpperH1RRO ,LowerH1RRO ,UpperLRO,LowerLRO,H1UpperLRO,H1LowerLRO,UpperBulge ,LowerBulge ,UpperDepression ,LowerDepression,WOBBLE,BARCODE ,TestTime ,RFVCW ,H1RFVCW ,H2RFVCW,HNRFVCW ,PEAKCW,LFVCW,H1LFVCW,RFVCCW,H1RFVCCW,H2RFVCCW,HNRFVCCW,PEAKCCW,LFVCCW,H1LFVCCW ,GradeRFVCW ,GradeH1RFVCW,GradeH2RFVCW,GradeHNRFVCW,GradePEAKCW,GradeLFVCW ,GradeH1LFVCW,GradeRFVCCW,GradeH1RFVCCW,GradeH2RFVCCW,GradeHNRFVCCW,GradePEAKCCW,GradeLFVCCW,GradeH1LFVCCW,GradeCONICITY ,GradePLY ,GradeUpperBulge ,GradeLowerBulge,GradeUpperDepression,GradeLowerDepression,GradeRRO,GradeUpperRRO ,GradeLowerRRO,GradeH1RRO,GradeUpperH1RRO,GradeLowerH1RRO,GradeUpperLRO,GradeLowerLRO,GradeH1UpperLRO,GradeH1LowerLRO ,GradeWobble,SpringRate,Circumference,LoadForce,UniformityInflationPress ,UniformityRimWidth,UniformityBeadDiameter,LSFTCW,LSFTCCW,MTMS_Uniformity_Grade,MTMS_Overall_Grade,Uniformity_Units,CycleTime ,MxN_Cycle from productionDataTUO where MachineName='30-353' and TestTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND TestTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and barcode !='' ";
                //    myConnection.reader = myConnection.comm.ExecuteReader();
                //    productionTUOdt.Load(myConnection.reader);
                //    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                //}
                //catch (Exception exc)
                //{
                //    myWebService.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); 
                //}
                //finally
                //{
                //    if (!myConnection.reader.IsClosed)
                //        myConnection.reader.Close();
                //    myConnection.comm.Dispose();
                //}
                ////Get productionPCRDB csv file details

                //try
                //{
                //    myConnection.open(ConnectionOption.SQL);
                //    myConnection.comm = myConnection.conn.CreateCommand();
                //    myConnection.comm.CommandText = "Select RFVCW,H1RFVCW,H2RFVCW,HNRFVCW  ,PEAKCW,LFVCW,H1LFVCW,RFVCCW,H1RFVCCW,H2RFVCCW,HNRFVCCW ,PEAKCCW,LFVCCW ,H1LFVCCW,PLY ,CONICITY,RRO ,GradeRRO,UpperLRO,GradeUpperLRO,LowerLRO,GradeLowerLRO,UpperBulge,GradeUpperBulge ,LowerBulge ,GradeLowerBulge,UpperDepression ,GradeUpperDepression  ,LowerDepression ,GradeLowerDepression  ,SPOT ,GradeSpot ,Wobble ,GradeWobble ,Static ,StaticAngle ,StaticGrade ,Couple  ,CoupleAngle,CoupleGrade ,Upper ,UpperAngle  ,UpperGrade ,Lower ,LowerAngle,LowerGrade ,Weight ,WeightGrade,GRADEDEF  ,BARCODE from ProductionDataPCRDBNew where wcid='239' and dtandTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "'  AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' and barcode !='' ";
                //    myConnection.comm.CommandTimeout = 120;
                //    myConnection.reader = myConnection.comm.ExecuteReader();
                //    productionDataDBNewdt.Load(myConnection.reader);
                //    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}
                //catch (Exception exp)
                //{
                //    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}
                //finally
                //{
                //    if (!myConnection.reader.IsClosed)
                //        myConnection.reader.Close();
                //    myConnection.comm.Dispose();
                //}
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
                //try
                //{
                //    //var row = from r0w1 in dtwaveform.AsEnumerable()
                //    //          join r0w2 in productionTUOdt.AsEnumerable()
                //    //            on r0w1.Field<string>("BARCODE") equals r0w2.Field<string>("BARCODE")
                //    //         join r0w3 in productionDataDBNewdt.AsEnumerable()
                //    //            on r0w1.Field<string>("BARCODE") equals r0w3.Field<string>("BARCODE") into ps
                //    //          from r0w3 in ps.DefaultIfEmpty()
                //    //          // select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray : new object[] { 0, 0, 0, 0, 0, 0 }).ToArray();
                //    //          select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray : new object[] { 0, 0 })).Take(600).ToArray();



                //    //foreach (object[] values in row)
                //    //    maindt.Rows.Add(values);
                //    //myWebService.writeLogs("row", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}
                //catch(Exception exp)
                //{
                //    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                //}


               
                    MainGridView.DataSource = dtwaveform;
                    MainGridView.DataBind();
                   // myWebService.writeLogs("maindt", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                       //DataTable dt = dtwaveform.Copy();
                     // ViewState["dt"] = dtwaveform;

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
