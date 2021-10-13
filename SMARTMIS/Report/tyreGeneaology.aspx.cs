using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SmartMIS.SmartWebReference;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Diagnostics;
using System.Data.OleDb;


namespace SmartMIS
{
    public partial class tyreGenealogy : System.Web.UI.Page
    {
        DataTable dbdt = new DataTable();
        private DateTime[] shiftName;
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        OleDbConnection conntest = new OleDbConnection();
        OleDbCommand cmd = new OleDbCommand();
        
        string tempgtbarcode = "";
        EventLog myLog = new EventLog();
        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            clearPage();
            if (!Page.IsPostBack)
            {
                string gtbarcode = Request.QueryString["gtbarcode"];
                if (gtbarcode!=null&& gtbarcode.Length == 10) 
                {
                    tgBarcodeTextBox.Text = gtbarcode;
                    showTyreGenealogyReport();
                }
                else
                { 
                }
            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "tgXRayGridView")
                    {
                        String wcName = ((Label)e.Row.FindControl("tgXRayWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("tgXRayInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "tgXRay2GridView")
                    {
                        String wcName = ((Label)e.Row.FindControl("tgXRay2WCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("tgXRayInnerGridView2"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "tgUniBalGridView")
                    {
                        String wcName = ((Label)e.Row.FindControl("tgUniBalWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("tgUniBalInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "RunOUTGridView")
                    {
                        String wcName = ((Label)e.Row.FindControl("RunOUTWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("RunOUTInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "TUOGrid")
                    {
                        String wcName = ((Label)e.Row.FindControl("TUOWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("TUOInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "tgVIGridView")
                    {
                        String wcName = ((Label)e.Row.FindControl("tgVIWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("tgVIInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }
                    else if (((GridView)sender).ID == "InspectionDiv1")
                    {
                        String wcName = ((Label)e.Row.FindControl("InspectionVIWCNameLabel")).Text;
                        GridView childGridView = ((GridView)e.Row.FindControl("InspectionVIInnerGridView"));

                        fillChildGridView(childGridView, wcName, tempgtbarcode);
                    }

                }
            }
            catch(Exception exp)
            {

            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            showTyreGenealogyReport();
        }



        private void showTyreGenealogyReport()
        {
            try
            {
                 tempgtbarcode = "";
                if (tgBarcodeTextBox.Text.Contains("C") || tgBarcodeTextBox.Text.Contains("c"))
                {
                  tempgtbarcode= getBarcodefromserialNumber(tgBarcodeTextBox.Text);
                }
                else
                tempgtbarcode=tgBarcodeTextBox.Text;

                TyreType tyreType = getTyreType(tempgtbarcode);
                reportHeader.ReportDate = DateTime.Today.ToString("dd-MM-yyyy");
                tgGTBarcodeNumber.Text = tempgtbarcode;

                if (tyreType == TyreType.TBR)
                {
                    TrimmingDetailDiv.Visible = false;
                    fillXRayReport(tempgtbarcode, "vTyreXray");
                    fillXRayReport2(tempgtbarcode, "vTyreXray2");
                    fillUniBalReport(tempgtbarcode, "vtbrrunoutData");
                    fillTUOReport(tempgtbarcode, "vTBRUniformityData");
                    fillVItbrReport(tempgtbarcode, "vTBRVisualInspectionReport");
                  
                    fillCuringReport(tempgtbarcode, "vCuringTBR");
                    fillTBMReport(tempgtbarcode, "vTbmTBR");
                    fillVISecond(tempgtbarcode, "vTBRVISecondLine");
                    fillSHreography(tempgtbarcode, "vShearographyData");
                }
                else if (tyreType == TyreType.PCR)
                {
                    TrimmingDetailDiv.Visible = true;
                    fillUniBalReport(tempgtbarcode, "productiondataTUO");
                    fillTrimReport(tempgtbarcode, "TrimmingData");
                    fillVIReport(tempgtbarcode, "vVisualInspectionPCR");
                    fillVI2Second(tempgtbarcode, "vVisualInspectionPCR2nd");
                    fillCuringReport(tempgtbarcode, "vCuringPCR");
                    fillTBMReport(tempgtbarcode, "vTbmPCR");
                    fillSHreography(tempgtbarcode, "vShearographyData");
                    fillTUOClassification(tempgtbarcode, "vAfterTUOInspection");
                    fillsmartExitBAy(tempgtbarcode, "VbayExitPlan");
                }
                else
                {
                    clearPage();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void ChartButton_Click(object sender, EventArgs e)
        {
            try
            {
                tempgtbarcode = "";
                if (tgBarcodeTextBox.Text.Contains("C") || tgBarcodeTextBox.Text.Contains("c"))
                {
                    tempgtbarcode = getBarcodefromserialNumber(tgBarcodeTextBox.Text);

                }
                else
                    tempgtbarcode = tgBarcodeTextBox.Text;

                TyreType tyreType = getTyreType(tempgtbarcode);
                reportHeader.ReportDate = DateTime.Today.ToString("dd-MM-yyyy");
                tgGTBarcodeNumber.Text = tempgtbarcode;

                tgCuringChart.Visible = true;

                if (tyreType == TyreType.TBR)
                {
                    fillXRayReport(tempgtbarcode, "vTyreXray");
                    fillXRayReport2(tempgtbarcode, "vTyreXray2");
                    fillUniBalReport(tempgtbarcode, "vtbrrunoutData");
                    fillTUOReport(tempgtbarcode, "vTBRUniformityData");
                    fillVIReport(tempgtbarcode, "vTBRVisualInspectionReport");
                    fillCuringReport(tempgtbarcode, "vCuringTBR");
                    fillTBMReport(tempgtbarcode, "vTbmTBR");

                    int notifyIcon = 0;

                    if (myWebService.IsRecordExist("vCuringTBR", "gtbarCode", "WHERE gtbarCode = '" + tempgtbarcode + "'", out notifyIcon) == true)
                        //fillCuringParameter(tempgtbarcode, "vCuringTBR", "TBRCuring", 120);
                        fillCuringParameter(tempgtbarcode, "vCuringTBR", "TBRCuring", 60);

                    fillTBMReport(tempgtbarcode, "vTbmTBR");
                }
                else if (tyreType == TyreType.PCR)
                {
                    fillUniBalReport(tempgtbarcode, "productiondataTUO");
                    fillVIReport(tempgtbarcode, "vVisualInspectionPCR");
                   
                    fillCuringReport(tempgtbarcode, "vCuringPCR");
                   // fillTBMReport(tempgtbarcode, "vTbmPCR");
                    int notifyIcon = 0;

                    if (myWebService.IsRecordExist("vCuringPCR", "gtbarCode", "WHERE gtbarCode = '" + tempgtbarcode + "'", out notifyIcon) == true)
                        //fillCuringParameter(tempgtbarcode, "vCuringPCR", "PCRCuring", 120);// fill curing graph details
                        fillCuringParameter(tempgtbarcode, "vCuringPCR", "PCRCuring", 60);

                    fillTBMReport(tempgtbarcode, "vTbmPCR");
                }
                else
                {
                    clearPage();
                }
            }
            catch(Exception ex)
            {
            }
        }

        #endregion

        #region User Defined Function

        private TyreType getTyreType(string gtBarcode)
        {
            TyreType flag;
            int notifyIcon = 0;
           
                if (myWebService.IsRecordExist("tbmtbr", "gtbarCode", "WHERE gtbarCode = '" + gtBarcode + "'", out notifyIcon) == true)
                    flag = TyreType.TBR;
                else if (myWebService.IsRecordExist("tbmpcr", "gtbarCode", "WHERE gtbarCode = '" + gtBarcode + "'", out notifyIcon) == true)
                    flag = TyreType.PCR;
                else if (myWebService.IsRecordExist("tbmpcr16Jan2021", "gtbarCode", "WHERE gtbarCode = '" + gtBarcode + "'", out notifyIcon) == true)
                    flag = TyreType.PCR;
                else
                    flag = TyreType.None;
           
            return flag;
        }
        private String getBarcodefromserialNumber(string serialNo)
        {
            String flag="";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select gtbarcode from CuringTBR where serialNo like '%" + tgBarcodeTextBox.Text + "%'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exc)
            {


            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }


            return flag;
        }
        private void fillXRayReport(string gtBarcode, string tableName)
        {
            try
            {
                
                    tgXRayGridView.DataSource = myWebService.fillGridView("SELECT DiSTINCT wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    tgXRayGridView.DataBind();
                
               
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            if (tgXRayGridView.Rows.Count > 0)
                tgXrayDiv.Visible = true;
        }

        private void fillXRayReport2(string gtBarcode, string tableName)
        {
            try
            {

                    tgXRay2GridView.DataSource = myWebService.fillGridView("SELECT DiSTINCT wcName FROM vTyreXray2 WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    tgXRay2GridView.DataBind();
                

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            if (tgXRay2GridView.Rows.Count > 0)
                Xra2Div2.Visible = true;
        }
      
        private void fillUniBalReport(string gtBarcode, string tableName)
        {
            if (tableName == "productiondataTUO")
            {
                try
                {

                    DataTable dtUni =myWebService.fillGridView("SELECT DISTINCT tireType as recipeCode, machineName as wcName FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    if (dtUni.Rows.Count > 0)
                    {
                        tgUniBalGridView.DataSource = dtUni;
                        tgUniBalGridView.DataBind();
                    }
                    else
                    {
                        tableName = "ProductionDataTUO_3de2020";
                        DataTable dtUni1 = myWebService.fillGridView("SELECT DISTINCT tireType as recipeCode, machineName as wcName FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                        tgUniBalGridView.DataSource = dtUni1;
                        tgUniBalGridView.DataBind();
                    }
                   
                }
                catch (Exception ex)
                {
                }


                try
                {
                    tgUniBalCSVGridView.DataSource = myWebService.fillGridView("select * from " + tableName + " where barcode='" + gtBarcode + "'", ConnectionOption.SQL);
                    tgUniBalCSVGridView.DataBind();
                }
                catch (Exception exc)
                {

                }
                try
                {
                    dbdt.Clear();
                    dbdt = myWebService.fillGridView("select * from vproductionDataPCRDBNew where barcode='" + gtBarcode + "'", ConnectionOption.SQL);
                    if (dbdt.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        dbdt = myWebService.fillGridView("select * from vproductionDataPCRDB where barcode='" + gtBarcode + "'", ConnectionOption.SQL);

                    }
                    DBGridView.DataSource = dbdt;
                    DBGridView.DataBind();

                    //DBGridView.DataSource = myWebService.fillGridView("select * from productionDataPCRDBNew where barcode='" + gtBarcode + "'", ConnectionOption.SQL);
                    //DBGridView.DataBind();
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

                if (tgUniBalGridView.Rows.Count > 0)
                    tgUniBalDiv.Visible = true;
            }
            else if (tableName == "vtbrrunoutData")
            {
                try
                {
                    RunOUTGridView.DataSource = myWebService.fillGridView("SELECT DISTINCT  recipeCode,  wcName FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    RunOUTGridView.DataBind();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    tgRunOUTCSVGridView.DataSource = myWebService.fillGridView("select * from " + tableName + " where barcode='" + gtBarcode + "'", ConnectionOption.SQL);
                    tgRunOUTCSVGridView.DataBind();
                }
                catch (Exception exc)
                {

                }

                if (RunOUTGridView.Rows.Count > 0)
                    RunOUTDiv.Visible = true;
 
            }

        }
        //added by sarita on 16-feb-2018
        private void fillTUOReport(string gtBarcode, string tableName)
        {
             {
                try
                {
                    TUOGrid.DataSource = myWebService.fillGridView("SELECT DISTINCT  recipeCode,  Name FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    TUOGrid.DataBind();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    TUOcsvGrid.DataSource = myWebService.fillGridView("select * from " + tableName + " where barcode='" + gtBarcode + "'", ConnectionOption.SQL);
                    TUOcsvGrid.DataBind();
                }
                catch (Exception exc)
                {

                }

                if (TUOGrid.Rows.Count > 0)
                    TUODiv1.Visible = true;

            }

        }
        private void fillTrimReport(string gtBarcode, string tableName)
        {
            try
            {
                TrimmimgGridView.DataSource = myWebService.fillGridView("SELECT wcName,trimNo,dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                TrimmimgGridView.DataBind();
            }
            catch (Exception ex)
            {
 
            }

            if (TrimmimgGridView.Rows.Count > 0)
                TrimmimgGridView.Visible = true;
        }
        private void fillVItbrReport(string gtBarcode, string tableName)
        {
            DataTable dtnewtbr = new DataTable();
            try
            { //

               
                 dtnewtbr= myWebService.fillGridView("SELECT DISTINCT wcID, wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                 if (dtnewtbr.Rows.Count > 0)
                 {
                     tgVIGridView.DataSource = dtnewtbr;
                     tgVIGridView.DataBind();
                 }
                 else
                 {
                     tableName = "vTBRVisualInspection09082021Report";
                     dtnewtbr = myWebService.fillGridView("SELECT DISTINCT wcID, wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);

                     if (dtnewtbr.Rows.Count > 0)
                     {
                         tgVIGridView.DataSource = dtnewtbr;
                         tgVIGridView.DataBind();
                     }
                 }
                //else {

                //    if (tableName == "vVisualInspectionPCR")
                //    {
                //        tableName = "vVisualInspectionPCR16Jan2021";
                //        DataTable dtnew1 = myWebService.fillGridView("SELECT DISTINCT wcID, wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                //        tgVIGridView.DataSource = dtnew1;
                //        tgVIGridView.DataBind();
                //    }
                //}
                
            }
            catch (Exception ex)
            {

            }

            if (dtnewtbr.Rows.Count > 0)
                tgVIDiv.Visible = true;
        }
       
            private void fillVIReport(string gtBarcode, string tableName)
        {
            try
            { //

               
                DataTable dtnew= myWebService.fillGridView("SELECT DISTINCT wcID, wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                if (dtnew.Rows.Count > 0)
                {
                    tgVIGridView.DataSource = dtnew;
                    tgVIGridView.DataBind();
                }
                else {

                    if (tableName == "vVisualInspectionPCR")
                    {
                        tableName = "vVisualInspectionPCR16Jan2021";
                        DataTable dtnew1 = myWebService.fillGridView("SELECT DISTINCT wcID, wcName FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                        tgVIGridView.DataSource = dtnew1;
                        tgVIGridView.DataBind();
                    }
                }
                
            }
            catch (Exception ex)
            {

            }

            if (tgVIGridView.Rows.Count > 0)
                tgVIDiv.Visible = true;
        }
            private void fillTUOClassification(string gtBarcode, string tableName)
        {
            try
            {
                classificationGridView1.DataSource = myWebService.fillGridView("select name,Sizename as recipeCode,firstName,statusname,defect_name ,defectArea,parameterName,dtandtime FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                classificationGridView1.DataBind();
            }
            catch (Exception ex)
            {

            }

            if (classificationGridView1.Rows.Count > 0)
                ClassificationDiv1.Visible = true;
        }

            private void fillsmartExitBAy(string gtBarcode, string tableName)
        {
            try
            {   
                exitBayGridView1.DataSource = myWebService.fillGridView(" select distinct WcName,recipecode,firstName,gtbarCode,bayPlanNo ,dtandTime ,Consignee_Name,InvoiceNo,Carrier,Destination_Name FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                exitBayGridView1.DataBind();
            }
            catch (Exception ex)
            {

            }

            if (exitBayGridView1.Rows.Count > 0)
                exitBayDiv1.Visible = true;
        }
        private void fillCuringReport(string gtBarcode, string tableName)
        {
            try
            {
                 
                DataTable dt  = myWebService.fillGridView("SELECT iD, pressbarCode,SerialNo as TyreSerialNo,RIGHT( pressbarCode,8) as CavityNO,case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as mouldNo, recipeCode, manningID, sapCode, firstName, lastName, wcID, wcName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);

                if (dt.Rows.Count > 0)
                {
                    tgCuringGridView.DataSource = dt;
                    tgCuringGridView.DataBind();
                }
                else
                {
                    if (tableName == "vCuringPCR")
                    {
                        tableName = "vCuringpcr16Jan2021";
                        DataTable dtNew = myWebService.fillGridView("SELECT iD, pressbarCode,SerialNo as TyreSerialNo,RIGHT( pressbarCode,8) as CavityNO,case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as mouldNo, recipeCode, manningID, sapCode, firstName, lastName, wcID, wcName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                        tgCuringGridView.DataSource = dtNew;
                        tgCuringGridView.DataBind();
                    }
                    else
                    {
                        tableName = "vCuringtbr11sep2019";
                        DataTable dtNew = myWebService.fillGridView("SELECT iD, pressbarCode,SerialNo as TyreSerialNo,RIGHT( pressbarCode,8) as CavityNO,case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as mouldNo, recipeCode, manningID, sapCode, firstName, lastName, wcID, wcName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                        tgCuringGridView.DataSource = dtNew;
                        tgCuringGridView.DataBind();
                    }
                   
                }
                
            }
            catch(Exception exp)
            {
                            }

            if (tgCuringGridView.Rows.Count > 0)
                tgCuringDiv.Visible = true;
        }
       
        private void fillCuringParameter(string gtBarcode, string tableName, string projectName, int timeInterval)
        {
            this.shiftName = new DateTime[3];
            this.SetShiftName();

            string wcName = "";
            string sourceFile = "";
            string fileName = "";
            DateTime dtandTime = DateTime.Now;

            DataTable flag = new DataTable("MainTable");
            DataTable tempflag = new DataTable("TempTable");

            StreamReader myReader;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

               // myConnection.comm.CommandText = "SELECT wcName, cycleUpdate  FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')";
                myConnection.comm.CommandText = "SELECT wcName,cycleUpdate  FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                while (myConnection.reader.Read())
                {
                    wcName = myConnection.reader[0].ToString();
                    dtandTime = Convert.ToDateTime(myConnection.reader[1]);
                }
            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally 
            {

                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            DataSet tgCuringChartDataSet = new DataSet(wcName);

            DataTable y_axis_Table = new DataTable(wcName + "_y_axis");
            DataTable channel_Table = new DataTable(wcName + "_channel");

            tgCuringChartDataSet.Tables.Add(y_axis_Table);
            tgCuringChartDataSet.Tables.Add(channel_Table);

            myWebService.writeLogs("Before", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
               
            #region Y_Axis
           
            OleDbDataReader readertest ;

            try
            {
                String DBPath = "";
                if (projectName == "TBRCuring")
                {
                    //DBPath = "C:\\TBR_scada\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007";
                    DBPath = "\\\\10.250.11.52\\SmartSCADA\\Projects\\TBRCuring\\ConfigFiles\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007";
                    conntest = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + DBPath);
                }
                else 
                {
                    //DBPath = "C:\\PCR_scada\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007";
                    DBPath = "\\\\10.250.11.51\\SmartSCADA\\Projects\\PCRCuring\\ConfigFiles\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007";
                    conntest = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + DBPath);
                }
                myWebService.writeLogs("DBPath"+DBPath, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    // "D:\\SmartSCADADATA\\Smart\\SmartSCADA\\Projects\\TBRCuring\\ConfigFiles\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007";
               
               
                cmd = conntest.CreateCommand();
                conntest.Open();
                myWebService.writeLogs("Start Data", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                cmd.CommandText = "Select trendspan, name, TitleText from y_axis WHERE trendname = '" + wcName + "'";
                var readerdata = cmd.ExecuteReader();
                myWebService.writeLogs("End Data", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                y_axis_Table.Load(readerdata);//readertest = cmd.ExecuteReader();
                cmd.Connection = conntest;
               
                
                conntest.Close();
               
            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            //String connectionString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @"\10.250.11.13\O:\SmartSCADA\Projects\" + projectName + @"\ConfigFiles\ConfigDB.mdb; Jet OLEDB:Database Password = smart26062007";

           
            //string connectionString = ("Provider=Microsoft.JET.OLEDB.4.0;data source=E:\\SmartSCADA\\Projects\\TBRCuring\\ConfigFiles\\ConfigDB.mdb;Jet OLEDB:Database Password=smart26062007");
            
            //try
            //{
               
            //    myConnection.open(ConnectionOption.MSAccess);

               
            //    myConnection.oComm = myConnection.oConn.CreateCommand();
               
            //    myConnection.oComm.CommandText = "Select trendspan, name, TitleText from y_axis WHERE trendname = '" + wcName + "'";
             
            //    myConnection.oReader = myConnection.oComm.ExecuteReader(CommandBehavior.CloseConnection);
            //    myWebService.writeLogs("reader execute", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            //    y_axis_Table.Load(myConnection.oReader);

            //   // myWebService.writeLogs(dbPATh, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                
            //}
            //catch (Exception ex)
            //{
            //    //myWebService.writeLogs("catch", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            //    myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            //}
            //finally
            //{
            //    myConnection.oReader.Close();
            //    myConnection.oReader.Dispose();
            //    myConnection.oComm.Dispose();
            //    myConnection.close(ConnectionOption.MSAccess);
            //}

            #endregion

            #region Channel
            try
            {

                conntest.Open();
                cmd.CommandText = "Select y_axis_name, channeltitle from channel WHERE trendname = '" + wcName + "'";
                var readerdata = cmd.ExecuteReader();
                channel_Table.Load(readerdata);//readertest = cmd.ExecuteReader();
                cmd.Connection = conntest;


                conntest.Close();
                //myConnection.open(ConnectionOption.MSAccess, conntest);
                //myConnection.oComm = myConnection.oConn.CreateCommand();
                //myConnection.oComm.CommandText = "Select y_axis_name, channeltitle from channel WHERE trendname = '" + wcName + "'";
                //myConnection.oReader = myConnection.oComm.ExecuteReader();
                //channel_Table.Load(myConnection.oReader);
              

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                //myConnection.oReader.Close();
                //myConnection.oReader.Dispose();
                //myConnection.oComm.Dispose();
                //myConnection.close(ConnectionOption.MSAccess);
            }

            #endregion

            #region ChannelData

            sourceFile = SetFileName(dtandTime);
            if (projectName == "TBRCuring")
            {
                //fileName = "C:\\TBRRCuring\\TrendsLog\\" + wcName + "\\" + sourceFile;
                fileName = "\\\\10.250.11.52\\TrendsLog\\" + wcName + "\\" + sourceFile;
            }
            else
            {
                //fileName = "C:\\PCRCuring\\TrendsLog\\" + wcName + "\\" + sourceFile;
                fileName = "\\\\10.250.11.51\\SmartSCADA\\Projects\\PCRCuring\\TrendsLog\\" + wcName + "\\" + sourceFile;
            }
                // "10.250.11.52\\O:\\SmartSCADA\\Projects\\TBRCuring\\TrendsLog\\" + wcName + "\\" + sourceFile;
            //fileName = @"E:\SmartSCADA\Projects\" + projectName + @"\TrendsLog\" + wcName + @"\" + sourceFile;
            //fileName = @"D:\SmartSCADADATA\Smart\SmartSCADA\Projects\" + projectName + @"\TrendsLog\" + wcName + @"\" + sourceFile;
            myWebService.writeLogs(fileName, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            if (File.Exists(fileName))
            {
                try
                {
                    myWebService.writeLogs(fileName+"filenameexists", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    myReader = new StreamReader(fileName);
                  
                    //Split the first line into the columns       
                    string[] columns = myReader.ReadLine().Split(new char[] { '\t' });

                    int coloumnCount = 0;
                    //Cycle the colums, adding those that don't exist yet

                    flag.Columns.Add("TimeStamp", typeof(System.DateTime));
                    tempflag.Columns.Add("TimeStamp", typeof(System.DateTime));
                    myWebService.writeLogs(channel_Table.Rows.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    foreach (DataRow row in channel_Table.Rows)
                    {
                        flag.Columns.Add(row[1].ToString());
                        tempflag.Columns.Add(row[1].ToString());
                        coloumnCount++;
                    }

                    while (coloumnCount < 12)
                    {
                        flag.Columns.Add(coloumnCount.ToString());
                        tempflag.Columns.Add(coloumnCount.ToString());
                        coloumnCount++;
                       myWebService.writeLogs(coloumnCount.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                    }

                    //Read the rest of the data in the file.        
                    string AllData = myReader.ReadToEnd();
                   //  myWebService.writeLogs(AllData.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    //Split off each row at the Carriage Return/Line Feed
                    //Default line ending in most windows exports.  
                    //You may have to edit this to match your particular file.
                    //This will work for Excel, Access, etc. default exports.
                    string[] rows = AllData.Split("\r\n".ToCharArray());

                    //Now add each row to the DataTable        
                    foreach (string row in rows)
                    {
                        //Split the row at the delimiter.
                        string[] items = row.Split(new char[] { '\t' });

                        if (items.Length > 1)
                        {
                            //Convert Automation DateTime to System DateTime
                            items[0] = DateTime.FromOADate(Convert.ToDouble(items[0])).ToString();

                            //Add the item
                            flag.Rows.Add(items);
                        }
                    }

                    myReader.Close();
                    myReader.Dispose();

                    DataRow[] results = flag.Select("TimeStamp >= #" + dtandTime + "# AND TimeStamp	< #" + dtandTime.AddMinutes(timeInterval) + "#");

                    //DataRow[] results = flag.Select("TimeStamp >= #" + dtandTime.ToString() + "#");
                    foreach (DataRow row in results)
                    {
                        tempflag.Rows.Add(row.ItemArray);
                        myWebService.writeLogs(results.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                }

                catch (Exception ex)
                { myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); }
            }

            #endregion

            #region Chart

            int i = 0;

            tgCuringChart.Titles.Add(wcName);

            foreach (DataRow row in y_axis_Table.Rows)
            {
                tgCuringChart.ChartAreas.Add("tgCuringChart_" + row[1].ToString());
                tgCuringChart.ChartAreas[i].AxisY.Title = row[1].ToString();
                tgCuringChart.ChartAreas[i].AxisY.Maximum = Convert.ToDouble(row[0].ToString());
                tgCuringChart.ChartAreas[i].AxisY.MajorGrid.Enabled = false;

                tgCuringChart.ChartAreas[i].AxisX.Title = "Time";
                tgCuringChart.ChartAreas[i].AxisX.MajorGrid.Enabled = false;

                tgCuringChart.ChartAreas[i].AxisX.LabelAutoFitMaxFontSize = 8;
                tgCuringChart.ChartAreas[i].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep45;
                i++;
               
            }

            i = 0;

            foreach (DataRow row in channel_Table.Rows)
            {
                tgCuringChart.Series.Add(row[1].ToString());
                tgCuringChart.Series[i].ChartArea = tgCuringChart.ChartAreas["tgCuringChart_" + row[0].ToString()].Name;
                tgCuringChart.Series[i].ChartType = SeriesChartType.Spline;
                i++;
            }

            ArrayList myArrayList = new ArrayList();
            string[] channelx_1 = new string[tempflag.Rows.Count];

            for (int j = 0; j < channel_Table.Rows.Count; j++)
            {
                double[] channely = new double[tempflag.Rows.Count];
                myArrayList.Add(channely);
            }



            for (int j = 0; j < tempflag.Rows.Count; j++)
            {
                channelx_1.SetValue(Convert.ToDateTime(tempflag.Rows[j][0]).ToLongTimeString(), j);

                for (int k = 0; k < myArrayList.Count; k++)
                    ((double[])myArrayList[k]).SetValue(Convert.ToDouble(tempflag.Rows[j][k + 1]), j);

            }

            for (int j = 0; j < myArrayList.Count; j++)
            {
                tgCuringChart.Series[j].Points.DataBindXY(channelx_1, (double[])myArrayList[j]);
            }


            #endregion
        }

        private void fillSHreography(string gtBarcode, string tableName)
        {
            try
            {
                if (tableName == "vShearographyData")
                {
                    shreographyGridView.DataSource = myWebService.fillGridView("SELECT iD, wcName, Name,Grade,shift,dtandTime FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    shreographyGridView.DataBind();
                }
                else
                {
                    shreographyGridView.DataSource = myWebService.fillGridView("SELECT iD, wcName, Name,Grade,shift,dtandTime FROM " + tableName + " WHERE (barCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    shreographyGridView.DataBind();
                }
            }
            catch (Exception exp)
            {

            }

            //displayMHEBarcode(tgBarcodeTextBox.Text.Trim());

            if (shreographyGridView.Rows.Count > 0)
                shreographyDiv1.Visible = true;
        } 
        //added by sarita on 29-
        //9-2018
        private void fillVISecond(string gtBarcode, string tableName)
        {
            try
            {
                if (tableName == "vTBRVISecondLine")
                {
                    SecondlineGridView.DataSource = myWebService.fillGridView("SELECT iD, curingRecipeName, manningID, sapCode, firstName, lastName, wcID, wcName,defectstatusName,defectName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    SecondlineGridView.DataBind();
                }
                else
                {
                    SecondlineGridView.DataSource = myWebService.fillGridView("SELECT iD, curingRecpeName, manningID, sapCode, firstName, lastName, wcName,statusName, defectName,dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    SecondlineGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            //displayMHEBarcode(tgBarcodeTextBox.Text.Trim());

            if (SecondlineGridView.Rows.Count > 0)
                VISecondlinediv.Visible = true;
        }

        private void fillVI2Second(string gtBarcode, string tableName)
        {
            try
            {
                //PCR2ndGridview  

               DataTable dtnew=   myWebService.fillGridView("SELECT iD, curingRecpeName, manningID, sapCode, firstName, lastName, wcName,statusName, defectName,dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
               if (dtnew.Rows.Count > 0)
               {
                   PCRGridView1.DataSource = dtnew;
                   PCRGridView1.DataBind();
               }
               else
               {
                   if (tableName == "vVisualInspectionPCR2nd")
                   {
                       tableName = "vVisualInspectionPCR2nd_3dec2020";
                       PCRGridView1.DataSource = dtnew;
                       PCRGridView1.DataBind();
                   }
                   
               }
              
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            //displayMHEBarcode(tgBarcodeTextBox.Text.Trim());

            if (PCRGridView1.Rows.Count > 0)
                PCRDiv1.Visible = true;
        } 
        private void fillTBMReport(string gtBarcode, string tableName)
        {
            try
            {
                if (tableName == "vTbmTBR")
                {
                  //  tgTBMGridView.DataSource = myWebService.fillGridView("SELECT iD, recipeCode, manningID, sapCode, firstName, lastName,(select top 1 weight from BuddeScannedTyreDetail where gtbarcode='"+gtBarcode+"' and stationNo=1 ) as gtWeight, mheID, wcID, wcName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    tgTBMGridView.DataSource = myWebService.fillGridView("SELECT vTbmTBR.iD, recipeCode, vTbmTBR.manningID, sapCode, firstName, lastName,GTRejection.status,(select top 1 weight from BuddeScannedTyreDetail where gtbarcode='" + gtBarcode + "' and stationNo=1 ) as gtWeight, mheID, vTbmTBR.wcID, wcName, vTbmTBR.dtandTime FROM vTbmTBR left join GTRejection on vTbmTBR.gtbarCode=GTRejection.GTBarcode WHERE (vTbmTBR.gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                 
                    tgTBMGridView.DataBind();
                }
                else
                {
                    //vTbmPCR16Jan2021
                   // tgTBMGridView.DataSource = myWebService.fillGridView("SELECT iD, recipeCode, manningID, sapCode, firstName, lastName,(select top 1 weight from PCRBuddeScannedTyreDetail where gtbarcode='" + gtBarcode + "' order by dtandtime desc) as gtWeight, mheID, wcID, wcName, dtandTime FROM " + tableName + " WHERE (gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);

                    DataTable dt = myWebService.fillGridView("SELECT vTbmPCR.iD, recipeCode, vTbmPCR.manningID, sapCode, firstName, lastName,GTRejection.status,(select top 1 weight from PCRBuddeScannedTyreDetail where gtbarcode='" + gtBarcode + "' order by dtandtime desc ) as gtWeight, mheID, vTbmPCR.wcID, wcName, vTbmPCR.dtandTime FROM vTbmPCR left join GTRejection on vTbmPCR.gtbarCode=GTRejection.GTBarcode WHERE (vTbmPCR.gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                    if (dt.Rows.Count > 0)
                    {
                        tgTBMGridView.DataSource = dt;
                        tgTBMGridView.DataBind();
                    }
                    else
                    {
                        DataTable dt1 = myWebService.fillGridView("SELECT vTbmPCR16Jan2021.iD, recipeCode, vTbmPCR16Jan2021.manningID, sapCode, firstName, lastName,GTRejection.status,(select top 1 weight from PCRBuddeScannedTyreDetail where gtbarcode='" + gtBarcode + "' order by dtandtime desc ) as gtWeight, mheID, vTbmPCR16Jan2021.wcID, wcName, vTbmPCR16Jan2021.dtandTime FROM vTbmPCR16Jan2021 left join GTRejection on vTbmPCR16Jan2021.gtbarCode=GTRejection.GTBarcode WHERE (vTbmPCR16Jan2021.gtbarCode = '" + gtBarcode + "')", ConnectionOption.SQL);
                        tgTBMGridView.DataSource = dt1;
                        tgTBMGridView.DataBind();
                    }
                    
                }
            }
            catch(Exception exp)
            {

            }

            //displayMHEBarcode(tgBarcodeTextBox.Text.Trim());

            if (tgTBMGridView.Rows.Count > 0)
                tgTBMDiv.Visible = true;
        }
        public string displayStatus(Object obj)
        {

            //Description   : Function for making a decision status of tgXRayGridView
            //Author        : Brajesh kumar
            //Date Created  : 08 April 2011
            //Date Updated  : 08 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
               
            }
            return flag;
        }
        private void fillChildGridView(GridView childGridView, string wcName, string gtBarcode)
        {//ss
            try
            {
                TyreType tyreType = getTyreType(tempgtbarcode);
                reportHeader.ReportDate = DateTime.Today.ToString("dd-MM-yyyy");

                if (tyreType == TyreType.TBR)
                {
                    if (childGridView.ID == "tgXRayInnerGridView")
                    {
                        myWebService.writeLogs("wcname"+wcName, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                         
                            childGridView.DataSource = myWebService.fillGridView("SELECT wcName, manningID, sapCode, firstName, lastName, status, defectName, dtandTime FROM dbo.vTyreXray WHERE (gtbarCode = '" + gtBarcode + "') AND (wcName = '" + wcName + "')", ConnectionOption.SQL);
                            childGridView.DataBind();
                        
                    }
                    else if (childGridView.ID == "tgXRayInnerGridView2")
                    {
                        childGridView.DataSource = myWebService.fillGridView("SELECT wcName, manningID, sapCode, firstName, lastName, status, defectName, dtandTime FROM dbo.vTyreXray2 WHERE (gtbarCode = '" + gtBarcode + "') AND (wcName = '" + wcName + "')", ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "tgUniBalInnerGridView")
                    {
                        childGridView.DataSource = fillunibalReport();
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "TUOInnerGridView")
                    {
                        childGridView.DataSource = filltuoReport();
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "RunOUTInnerGridView")
                    {
                        childGridView.DataSource = fillunibalReport();
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "tgVIInnerGridView")
                    {
                        childGridView.DataSource = fillTBRVisualInspectionReport(wcName, gtBarcode);
                        childGridView.DataBind();
                    }
                }
                else if (tyreType == TyreType.PCR)
                {
                    if (childGridView.ID == "tgXRayInnerGridView")
                    {
                        childGridView.DataSource = myWebService.fillGridView("SELECT wcName, manningID, sapCode, firstName, lastName, status, defectName, dtandTime FROM dbo.vTyreXray WHERE (gtbarCode = '" + gtBarcode + "') AND (wcName = '" + wcName + "')", ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "tgUniBalInnerGridView")
                    {
                        childGridView.DataSource = fillunibalReport();
                        childGridView.DataBind();
                    }
                    else if (childGridView.ID == "tgVIInnerGridView")
                    {
                        childGridView.DataSource = fillPCRVisualInspectionReport(wcName, gtBarcode);
                        childGridView.DataBind();
                    }
                   

                }
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }


        }
        private DataTable fillunibalReport()
        {
            TyreType tyreType = getTyreType(tempgtbarcode);

            DataTable flag = new DataTable();
            flag.Columns.Add("sapCode",typeof(string));
            flag.Columns.Add("firstName", typeof(string));
            flag.Columns.Add("lastName", typeof(string));
            flag.Columns.Add("action", typeof(string));
            flag.Columns.Add("status", typeof(string));
            flag.Columns.Add("dtandTime", typeof(string));
            DataRow dr = flag.NewRow();
            dr["sapcode"] = "Unknown";
            dr["firstName"] = "Unknown";
            dr["lastName"] = "";


            string a= "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tyreType==TyreType.PCR)
                {
                    myConnection.comm.CommandText = "SELECT uniformitygrade,testTime FROM productiondataTUO WHERE (barCode = '" + tempgtbarcode + "')";

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        a = myConnection.reader[0].ToString();
                        if (a != "E")
                        {
                            dr["status"] = "OK";
                        }
                        else
                        {
                            dr[4] = "NOT OK";
                        }
                        dr["dtandTime"] = myConnection.reader[1].ToString();
                    }
                }
                else if (tyreType == TyreType.TBR)
                {
                    myConnection.comm.CommandText = "SELECT totalRank as status,dtandTime FROM vtbrrunoutData WHERE (barCode = '" + tempgtbarcode + "')";


                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        a = myConnection.reader[0].ToString();
                        if (a =="A"|| a=="B")
                        {
                            dr["status"] = "OK";
                        }
                        else
                        {
                            dr[4] = "NOT OK";
                        }
                        dr["dtandTime"] = myConnection.reader[1].ToString();
                    }
                }
                flag.Rows.Add(dr);
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
          

            if (!(flag.Rows.Count > 0))
            {

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tyreType == TyreType.PCR)
                    {
                        myConnection.comm.CommandText = "SELECT uniformitygrade,testTime FROM ProductionDataTUO_3de2020 WHERE (barCode = '" + tempgtbarcode + "')";

                        myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                        while (myConnection.reader.Read())
                        {
                            a = myConnection.reader[0].ToString();
                            if (a != "E")
                            {
                                dr["status"] = "OK";
                            }
                            else
                            {
                                dr[4] = "NOT OK";
                            }
                            dr["dtandTime"] = myConnection.reader[1].ToString();
                        }
                    }
                    else if (tyreType == TyreType.TBR)
                    {
                        myConnection.comm.CommandText = "SELECT totalRank as status,dtandTime FROM vtbrrunoutData WHERE (barCode = '" + tempgtbarcode + "')";


                        myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                        while (myConnection.reader.Read())
                        {
                            a = myConnection.reader[0].ToString();
                            if (a == "A" || a == "B")
                            {
                                dr["status"] = "OK";
                            }
                            else
                            {
                                dr[4] = "NOT OK";
                            }
                            dr["dtandTime"] = myConnection.reader[1].ToString();
                        }
                    }
                    flag.Rows.Add(dr);
                }
                catch (Exception exp)
                {

                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
          
            
            
            }
            return flag;
        }
        //added by sarita
        private DataTable filltuoReport()
        {
            TyreType tyreType = getTyreType(tempgtbarcode);
            DataTable flag = new DataTable();
            flag.Columns.Add("sapCode", typeof(string));
            flag.Columns.Add("firstName", typeof(string));
            flag.Columns.Add("lastName", typeof(string));
            flag.Columns.Add("action", typeof(string));
            flag.Columns.Add("status", typeof(string));
            flag.Columns.Add("dtandTime", typeof(string));
            DataRow dr = flag.NewRow();
            dr["sapcode"] = "Unknown";
            dr["firstName"] = "Unknown";
            dr["lastName"] = "";


            string a = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT totalRank as status,dtandTime FROM vTBRUniformityData WHERE (barCode = '" + tempgtbarcode + "')";


                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        a = myConnection.reader[0].ToString();
                        if (a == "A" || a == "B")
                        {
                            dr["status"] = "OK";
                        }
                        else
                        {
                            dr[4] = "NOT OK";
                        }
                        dr["dtandTime"] = myConnection.reader[1].ToString();
                    }



                

            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            flag.Rows.Add(dr);


            return flag;
        }
        private DataTable fillPCRVisualInspectionReport( string wcName,string gtbarCode)
        {
            DataTable flag = new DataTable();
            flag.Columns.Add("sapCode", typeof(string));
            flag.Columns.Add("firstName", typeof(string));
            flag.Columns.Add("lastName", typeof(string));
            flag.Columns.Add("defectStatusName", typeof(string));
            flag.Columns.Add("faultSideName", typeof(string));
            flag.Columns.Add("faultAreaName", typeof(string));
            flag.Columns.Add("faultName", typeof(string));
            flag.Columns.Add("reasonName", typeof(string));
            flag.Columns.Add("SerialNo", typeof(string));
            flag.Columns.Add("Remark", typeof(string));
            flag.Columns.Add("dtandTime", typeof(string));
            
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select sapCode,firstName,lastName,statusName as defectstatusName,ssornssName as faultSideName,defectlocationName as faultAreaName,defectname as faultName, reasonName,dtandTime from vVisualInspectionPCR WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
        
                    while (myConnection.reader.Read())
                    {
                        DataRow dr = flag.NewRow();
                        dr["sapCode"] = myConnection.reader[0].ToString();
                        dr["firstName"] = myConnection.reader[1].ToString();
                        dr["lastName"] = myConnection.reader[2].ToString();
                        dr["defectStatusName"] = myConnection.reader[3].ToString();
                        dr["faultSideName"] = myConnection.reader[4].ToString();
                        dr["faultAreaName"] = myConnection.reader[5].ToString();
                        dr["faultName"] = myConnection.reader[6].ToString();
                        dr["reasonName"] = myConnection.reader[7].ToString();
                        dr["SerialNo"] = "--";
                        dr["Remark"] = "--";
                        dr["dtandTime"] = myConnection.reader[8].ToString();
                        flag.Rows.Add(dr);
                    }
            }
            catch (Exception exp)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            if (!(flag.Rows.Count > 0))
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select sapCode,firstName,lastName,statusName as defectstatusName,ssornssName as faultSideName,defectlocationName as faultAreaName,defectname as faultName, reasonName,dtandTime from vVisualInspectionPCR16Jan2021 WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        DataRow dr = flag.NewRow();
                        dr["sapCode"] = myConnection.reader[0].ToString();
                        dr["firstName"] = myConnection.reader[1].ToString();
                        dr["lastName"] = myConnection.reader[2].ToString();
                        dr["defectStatusName"] = myConnection.reader[3].ToString();
                        dr["faultSideName"] = myConnection.reader[4].ToString();
                        dr["faultAreaName"] = myConnection.reader[5].ToString();
                        dr["faultName"] = myConnection.reader[6].ToString();
                        dr["reasonName"] = myConnection.reader[7].ToString();
                        dr["SerialNo"] = "--";
                        dr["Remark"] = "--";
                        dr["dtandTime"] = myConnection.reader[8].ToString();
                        flag.Rows.Add(dr);
                    }
                }
                catch (Exception exp)
                {
                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }

            return flag;
        }
        private DataTable fillPCRVisualInspection2Report(string wcName, string gtbarCode)
        {
            DataTable flag = new DataTable();
            flag.Columns.Add("sapCode", typeof(string));
            flag.Columns.Add("firstName", typeof(string));
            flag.Columns.Add("lastName", typeof(string));
            flag.Columns.Add("defectStatusName", typeof(string));
            flag.Columns.Add("faultSideName", typeof(string));
            flag.Columns.Add("faultAreaName", typeof(string));
            flag.Columns.Add("faultName", typeof(string));
            flag.Columns.Add("reasonName", typeof(string));
            flag.Columns.Add("SerialNo", typeof(string));
            flag.Columns.Add("Remark", typeof(string));
            flag.Columns.Add("dtandTime", typeof(string));



            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select sapCode,firstName,lastName,statusName as defectstatusName,defectlocationName as faultAreaName,defectname as faultName, curingRecpeName,dtandTime from vVisualInspectionPCR2nd WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                while (myConnection.reader.Read())
                {
                    DataRow dr = flag.NewRow();

                    dr["sapCode"] = myConnection.reader[0].ToString();
                    dr["firstName"] = myConnection.reader[1].ToString();
                    dr["lastName"] = myConnection.reader[2].ToString();
                    dr["defectStatusName"] = myConnection.reader[3].ToString();
                    dr["faultSideName"] = myConnection.reader[4].ToString();
                    dr["faultAreaName"] = myConnection.reader[5].ToString();
                    dr["faultName"] = myConnection.reader[6].ToString();
                    dr["reasonName"] = myConnection.reader[7].ToString();
                    dr["SerialNo"] = "--";
                    dr["Remark"] = "--";
                    dr["dtandTime"] = myConnection.reader[8].ToString();
                    flag.Rows.Add(dr);

                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            if (!(flag.Rows.Count > 0))
            {

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select sapCode,firstName,lastName,statusName as defectstatusName,defectlocationName as faultAreaName,defectname as faultName, curingRecpeName,dtandTime from vVisualInspectionPCR2nd_3dec2020 WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        DataRow dr = flag.NewRow();

                        dr["sapCode"] = myConnection.reader[0].ToString();
                        dr["firstName"] = myConnection.reader[1].ToString();
                        dr["lastName"] = myConnection.reader[2].ToString();
                        dr["defectStatusName"] = myConnection.reader[3].ToString();
                        dr["faultSideName"] = myConnection.reader[4].ToString();
                        dr["faultAreaName"] = myConnection.reader[5].ToString();
                        dr["faultName"] = myConnection.reader[6].ToString();
                        dr["reasonName"] = myConnection.reader[7].ToString();
                        dr["SerialNo"] = "--";
                        dr["Remark"] = "--";
                        dr["dtandTime"] = myConnection.reader[8].ToString();
                        flag.Rows.Add(dr);

                    }
                }
                catch (Exception exp)
                {

                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            
            }
            return flag;
        }
        private DataTable fillTBRVisualInspectionReport(string wcName, string gtbarCode)
        {
            string defectAreaID = ""; string defectNames = ""; string faultAreaNames = "";
            DataTable flag = new DataTable();
            flag.Columns.Add("sapCode", typeof(string));
            flag.Columns.Add("firstName", typeof(string));
            flag.Columns.Add("lastName", typeof(string));
            flag.Columns.Add("defectStatusName", typeof(string));
            flag.Columns.Add("faultSideName", typeof(string));
            flag.Columns.Add("faultAreaName", typeof(string));
            flag.Columns.Add("faultName", typeof(string));
            flag.Columns.Add("reasonName", typeof(string));
            flag.Columns.Add("SerialNo", typeof(string));
            flag.Columns.Add("Remark", typeof(string));
            flag.Columns.Add("dtandTime", typeof(string));



            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select sapCode,firstName,lastName,defectstatusName,SSorNSS as faultSideName, defectname as faultName,  reasonName, serialNo, remarks as Remark,dtandTime,defectAreaName,defectID,wcName from vTBRVisualInspectionReport WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                defectNames = ""; faultAreaNames = "";
                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                if (myConnection.reader.HasRows == true)
                {
                    while (myConnection.reader.Read())
                    {
                        DataRow dr = flag.NewRow();

                        dr["sapCode"] = myConnection.reader[0].ToString();
                        dr["firstName"] = myConnection.reader[1].ToString();
                        dr["lastName"] = myConnection.reader[2].ToString();

                        if (myConnection.reader[12].ToString().Trim() == "7010")
                        {
                            if (myConnection.reader[11].ToString() == "31")
                                dr["defectStatusName"] = "OK - " + myConnection.reader[3].ToString().ToUpper();
                            else
                                dr["defectStatusName"] = myConnection.reader[3].ToString().ToUpper();
                        }
                        else
                        {
                            if ((myConnection.reader[12].ToString().Trim() == "7007" || myConnection.reader[12].ToString().Trim() == "7008") && (myConnection.reader[11].ToString() == "31" || myConnection.reader[11].ToString() == "21" || myConnection.reader[11].ToString() == "22" || myConnection.reader[11].ToString() == "27"))
                            {
                                if (myConnection.reader[11].ToString() == "21")
                                    dr["defectStatusName"] = "BUFF - " + myConnection.reader[3].ToString().ToUpper();

                                if (myConnection.reader[11].ToString() == "22")
                                    dr["defectStatusName"] = "REPAIR - " + myConnection.reader[3].ToString().ToUpper();

                                if (myConnection.reader[11].ToString() == "27")
                                    dr["defectStatusName"] = "Comflauge - " + myConnection.reader[3].ToString().ToUpper();

                                if (myConnection.reader[11].ToString() == "31")
                                    dr["defectStatusName"] = "OK - " + myConnection.reader[3].ToString().ToUpper();
                            }
                            else
                            {
                                dr["defectStatusName"] = myConnection.reader[3].ToString().ToUpper();
                            }
                        }


                        if (myConnection.reader[4].ToString() == "1")
                            dr["faultSideName"] = "SS";
                        else if (myConnection.reader[4].ToString() == "2")
                            dr["faultSideName"] = "NSS";
                        else if (myConnection.reader[4].ToString() == "0")
                            dr["faultSideName"] = "N/A";



                        if (myConnection.reader[12].ToString().Trim() == "7001" || myConnection.reader[12].ToString().Trim() == "7002" || myConnection.reader[12].ToString().Trim() == "7003" || myConnection.reader[12].ToString().Trim() == "7004" || myConnection.reader[12].ToString().Trim() == "7005" || myConnection.reader[12].ToString().Trim() == "7006")
                        {
                            dr["faultName"] = myConnection.reader[5].ToString();
                            defectNames = myConnection.reader[5].ToString();
                            dr["faultAreaName"] = myConnection.reader[10].ToString();
                            faultAreaNames = myConnection.reader[10].ToString();
                        }
                        else
                        {
                            if (myConnection.reader[12].ToString().Trim() == "7011" || myConnection.reader[12].ToString().Trim() == "7007" || myConnection.reader[12].ToString().Trim() == "7008")
                            {
                                dr["faultName"] = defectNames;
                                dr["faultAreaName"] = faultAreaNames;
                            }
                            else
                            {
                                dr["faultName"] = myConnection.reader[5].ToString();
                                dr["faultAreaName"] = myConnection.reader[10].ToString();
                            }
                        }

                        dr["reasonName"] = myConnection.reader[6].ToString();
                        dr["SerialNo"] = myConnection.reader[7].ToString();
                        dr["Remark"] = myConnection.reader[8].ToString();
                        dr["dtandTime"] = myConnection.reader[9].ToString();
                        flag.Rows.Add(dr);

                    }


                }
                else
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL); 

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select sapCode,firstName,lastName,defectstatusName,SSorNSS as faultSideName, defectname as faultName,  reasonName, serialNo, remarks as Remark,dtandTime,defectAreaName,defectID,wcName from vTBRVisualInspection09082021Report WHERE (gtbarCode = '" + gtbarCode + "') AND (wcName = '" + wcName + "')";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    defectNames = ""; faultAreaNames = "";
                    myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                  

                    while (myConnection.reader.Read())
                    {
                        DataRow dr = flag.NewRow();

                        dr["sapCode"] = myConnection.reader[0].ToString();
                        dr["firstName"] = myConnection.reader[1].ToString();
                        dr["lastName"] = myConnection.reader[2].ToString();
                        dr["defectStatusName"] = myConnection.reader[3].ToString().ToUpper();


                        if (myConnection.reader[4].ToString() == "1")
                            dr["faultSideName"] = "SS";
                        else if (myConnection.reader[4].ToString() == "2")
                            dr["faultSideName"] = "NSS";
                        else if (myConnection.reader[4].ToString() == "0")
                            dr["faultSideName"] = "N/A";

                        dr["faultName"] = myConnection.reader[5].ToString();
                        dr["faultAreaName"] = myConnection.reader[10].ToString();


                        dr["reasonName"] = myConnection.reader[6].ToString();
                        dr["SerialNo"] = myConnection.reader[7].ToString();
                        dr["Remark"] = myConnection.reader[8].ToString();
                        dr["dtandTime"] = myConnection.reader[9].ToString();
                        flag.Rows.Add(dr);

                    }
                }

            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }


            return flag;
        }
        public string displayUserDecision(Object obj)
        {

            //Description   : Function for making a decision status of tgXRayInnerGridView
            //Author        : Brajesh kumar
            //Date Created  : 11 April 2011
            //Date Updated  : 11 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            return myWebService.displayUserDecision(obj);

        }
        public void displayMHEBarcode(string gtBarcode)
        {
            int i = 0;
            int arrayLength = 0;
            string a = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT mheID FROM tbmPCR WHERE (gtbarCode = '" + gtBarcode + "')";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                while (myConnection.reader.Read())
                {
                    a = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            string[] tmpstr = a.Split(new char[] { '#' });

            arrayLength = tmpstr.Length;

            while (i < arrayLength)
            {
                tmpstr[i] = getMHEName(tmpstr[i].Substring(2, 1));
                i++;
            }

            tgTBMBeadBarcodeLabel.Text = tmpstr[0];
            tgTBMILBarcodeLabel.Text = tmpstr[1];
            tgTBMPlyBarcodeLabel.Text = tmpstr[2];
            tgTBMSideWallBarcodeLabel.Text = tmpstr[3];

        }
        public DataTable csvRecord(string gtBarcode)
        {
            DataTable flag = new DataTable();

            int i = 0;
            int index;
            string rawString = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT csvFileData FROM unibalrunoutPCR WHERE (gtbarCode = '" + gtBarcode + "')";

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    rawString = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }



            string[] tmpHeader = rawString.Split(new char[] { '#' });
            string[] tmpRow = rawString.Split(new char[] { '#' });

            foreach (string item in tmpHeader)
            {
                index = Convert.ToInt32(tmpHeader[i].Substring(0, 3));
                tmpHeader[i] = myWebService.csvHeaders[index];

                flag.Columns.Add(tmpHeader[i], typeof(String));
                i++;
            }

            i = 0;

            foreach (string item in tmpRow)
            {
                tmpRow[i] = tmpRow[i].Substring(3);
                i++;
            }

            flag.Rows.Add(tmpRow);

            return flag;
        }
        public string getMHEName(string mheID)
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "SELECT name FROM mheMaster WHERE iD = " + Convert.ToInt32(mheID) + "";

                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exp)
            {
            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }
        private String SetFileName(DateTime dtandTime)
        {
            String fileName = "";
            try
            {
              
                String shift = GetShiftName(dtandTime);

                if (shift == "C")
                {
                    if ((Convert.ToInt32(dtandTime.Hour.ToString() + dtandTime.ToString("mm")) >= 0) && (Convert.ToInt32(dtandTime.Hour.ToString() + dtandTime.ToString("mm")) < Convert.ToInt32(Convert.ToDateTime(shiftName[0]).Hour.ToString() + Convert.ToDateTime(shiftName[0]).ToString("mm"))))
                        fileName = SetDayName((dtandTime.AddDays(-1).Day).ToString()) + dtandTime.ToString("MMM") + dtandTime.ToString("yy") + shift.ToString() + ".txt";
                    else
                        fileName = SetDayName((dtandTime.Day).ToString()) + dtandTime.ToString("MMM") + dtandTime.ToString("yy") + shift.ToString() + ".txt";
                }
                else
                    fileName = SetDayName((dtandTime.Day).ToString()) + dtandTime.ToString("MMM") + dtandTime.ToString("yy") + shift.ToString() + ".txt";

                
            }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return fileName;
        }
        private void SetShiftName()
        {
            int i = 0;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT startTime from shiftMaster";
            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                shiftName[i] = Convert.ToDateTime(myConnection.reader[0].ToString());
                i++;
            }

            myConnection.reader.Close();
            myConnection.close(ConnectionOption.SQL);
        }
        private String SetDayName(String day)
        {
            String flag = "";

            if (day.Length < 2)
                flag = "0" + day;
            else
                flag = day;

            return flag;
        }
        private String GetShiftName(DateTime dtandTime)
        {
           
            String flag = "";

            if ((dtandTime >= Convert.ToDateTime(dtandTime.ToShortDateString() + " " + shiftName[0].ToLongTimeString())) && ((dtandTime < Convert.ToDateTime(dtandTime.ToShortDateString() + " " + shiftName[1].ToLongTimeString()))))
                flag = "A";
            else if ((dtandTime >= Convert.ToDateTime(dtandTime.ToShortDateString() + " " + shiftName[1].ToLongTimeString())) && ((dtandTime < Convert.ToDateTime(dtandTime.ToShortDateString() + " " + shiftName[2].ToLongTimeString()))))
                flag = "B";
            else
                flag = "C";

            return flag; ;
        }
        public void clearPage()
        {
            tgXrayDiv.Visible = false;
            tgUniBalDiv.Visible = false;
            tgVIDiv.Visible = false;
            tgCuringDiv.Visible = false;
            tgTBMDiv.Visible = false;
            TUODiv1.Visible = false;
            RunOUTDiv.Visible = false;
            Xra2Div2.Visible = false;
        }

        #endregion
    }
}
