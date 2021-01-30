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
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Collections.Generic;

namespace SmartMIS.Home
{
    public partial class home : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string datetime = DateTime.Now.ToString("yyyy-MM-dd") + " 07:00:00";//

        protected void Page_Load(object sender, EventArgs e)
        {
             try
            {
                TBRPanel.Visible = true;
                PCRPanel.Visible = false;

                // For TBM TBR WorkCenterwise //
                TBMTBRChart.Series["TBMTBRSeries"].ChartType = SeriesChartType.Pie;
                TBMTBRChart.Series["TBMTBRSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMTBRChart.ChartAreas["TBMTBRChartArea"].Area3DStyle.Enable3D = true;
                TBMTBRChart.Series["TBMTBRSeries"].IsValueShownAsLabel = true;

                FillTBMTBRData();

                // For TBM TBR Recipewise
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].ChartType = SeriesChartType.Column;
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMTBRRecipeChart.ChartAreas["TBMTBRRecipeChartArea"].Area3DStyle.Enable3D = true;
                TBMTBRRecipeChart.ChartAreas["TBMTBRRecipeChartArea"].AxisX.Interval = 1; 
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].IsValueShownAsLabel = true;

                //FillTBMTBRRecipeData();
                FillTBRCURData();
                // For Curing TBR
                TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Column;
                //TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Pie;
                //TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Bar;
                TBRCURChart.Series["TBRCURSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBRCURChart.ChartAreas["TBRCURChartArea"].Area3DStyle.Enable3D = true;
                TBMTBRChart.ChartAreas["TBRCURChartArea"].AxisX.Interval = 1; 
                TBRCURChart.Series["TBRCURSeries"].IsValueShownAsLabel = true;

                //FillTBRCURData();
                TBR.BackColor = System.Drawing.ColorTranslator.FromHtml("#040F25");
                PCR.BackColor = System.Drawing.ColorTranslator.FromHtml("#165188");
            }
             catch (Exception exp)
             {
                 myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
             }
        }

        protected void TBR_Click(object sender, EventArgs e)
        {
            try
            {
                TBRPanel.Visible = true;
                PCRPanel.Visible = false;
                
                // For TBM TBR WorkCenterwise
                TBMTBRChart.Series["TBMTBRSeries"].ChartType = SeriesChartType.Pie;
                TBMTBRChart.Series["TBMTBRSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMTBRChart.ChartAreas["TBMTBRChartArea"].Area3DStyle.Enable3D = true;
                TBMTBRChart.Series["TBMTBRSeries"].IsValueShownAsLabel = true;

                FillTBMTBRData();

                // For TBM TBR Recipewise
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].ChartType = SeriesChartType.Column;
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMTBRRecipeChart.ChartAreas["TBMTBRRecipeChartArea"].Area3DStyle.Enable3D = true;
                TBMTBRRecipeChart.ChartAreas["TBMTBRRecipeChartArea"].AxisX.Interval = 1; 
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].IsValueShownAsLabel = true;

                //FillTBMTBRRecipeData();

                // For Curing TBR
                TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Column;
                //TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Pie;
                //TBRCURChart.Series["TBRCURSeries"].ChartType = SeriesChartType.Bar;
                TBRCURChart.Series["TBRCURSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBRCURChart.ChartAreas["TBRCURChartArea"].Area3DStyle.Enable3D = true;
                TBRCURChart.ChartAreas["TBRCURChartArea"].AxisX.Interval = 1; 
                TBRCURChart.Series["TBRCURSeries"].IsValueShownAsLabel = true;

                FillTBRCURData();
                TBR.BackColor = System.Drawing.ColorTranslator.FromHtml("#040F25");
                PCR.BackColor = System.Drawing.ColorTranslator.FromHtml("#165188");
                
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void PCR_Click(object sender, EventArgs e)
        {
             try
            {
                TBRPanel.Visible = false;
                PCRPanel.Visible = true;
                
                // For TBM PCR WorkCenterwise
                //TBMPCRChart.Series["TBMPCRSeries"].ChartType = SeriesChartType.Column;
                TBMPCRChart.Series["TBMPCRSeries"].ChartType = SeriesChartType.Pie;
                TBMPCRChart.Series["TBMPCRSeries"]["DrawingStyle"] = "Emboss"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMPCRChart.ChartAreas["TBMPCRChartArea"].Area3DStyle.Enable3D = true;
                TBMPCRChart.Series["TBMPCRSeries"].IsValueShownAsLabel = true;
                
                FillTBMPCRData();

                // For TBM PCR WorkCenterwise
                TBMPCRRecipeChart.Series["TBMPCRRecipeSeries"].ChartType = SeriesChartType.Column;
                TBMPCRRecipeChart.Series["TBMPCRRecipeSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                TBMPCRRecipeChart.ChartAreas["TBMPCRRecipeChartArea"].Area3DStyle.Enable3D = true;
                TBMPCRRecipeChart.ChartAreas["TBMPCRRecipeChartArea"].AxisX.Interval = 1; 
                TBMPCRRecipeChart.Series["TBMPCRRecipeSeries"].IsValueShownAsLabel = true;

                //FillTBMPCRRecipewiseData();
                
                // For PCR Curing
                PCRCURChart.Series["PCRCURSeries"].ChartType = SeriesChartType.Column;
                PCRCURChart.Series["PCRCURSeries"]["DrawingStyle"] = "Cylinder"; //Emboss,Cylinder,LightToDark,Wedge,Default
                PCRCURChart.ChartAreas["PCRCURChartArea"].Area3DStyle.Enable3D = true;
                PCRCURChart.ChartAreas["PCRCURChartArea"].AxisX.Interval = 1; 
                PCRCURChart.Series["PCRCURSeries"].IsValueShownAsLabel = true;

                FillPCRCURData();

                TBR.BackColor = System.Drawing.ColorTranslator.FromHtml("#165188");
                PCR.BackColor = System.Drawing.ColorTranslator.FromHtml("#040F25");

            }
             catch (Exception exp)
             {
                 myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
             }
        }
        private void FillTBMTBRData()
        {
            try
            {
                DataTable dt = new DataTable();
            
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                
                myConnection.comm.CommandText = "Select recipeCode, wcName, gtbarcode AS quantity from vTbmTBR where dtandtime>='" + datetime + "'";//DateTime.Today.ToString("dd-MM-yyyy");
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
               
                var gridviewdt = new DataTable();
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("quantity", typeof(int));
                var query = dt.AsEnumerable()
    .GroupBy(row => row.Field<string>("wcName"))
    .Select(g =>
    {
        var row = gridviewdt.NewRow();
        row.SetField("wcName", g.Key);
        row.SetField("quantity", g.Count());
       
        return row;
    }).CopyToDataTable();

                var rows = (from row in query.AsEnumerable()
                            orderby row["wcName"] ascending
                            select row);
                DataTable fdt = rows.AsDataView().ToTable();
                TBMTBRChart.DataSource = fdt;
                TBMTBRChart.Series["TBMTBRSeries"].XValueMember = "wcName";
                TBMTBRChart.Series["TBMTBRSeries"].YValueMembers = "quantity";
                TBMTBRChart.DataBind();

                DataTable recipedt = new DataTable();
                recipedt.Columns.Add("recipeCode", typeof(string));
                recipedt.Columns.Add("quantity", typeof(string));
                var recipequery = dt.AsEnumerable()
     .GroupBy(row => row.Field<string>("recipeCode"))
     .Select(g =>
     {
         var row = recipedt.NewRow();
         row.SetField("recipeCode", g.Key);
         row.SetField("Quantity", g.Count());
         return row;
     }).CopyToDataTable();

                TBMTBRRecipeChart.DataSource = recipequery;
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].XValueMember = "recipeCode";
                TBMTBRRecipeChart.Series["TBMTBRRecipeSeries"].YValueMembers = "quantity";
                TBMTBRRecipeChart.DataBind();

                TBMProductionLabel.Text = "Total TBM : " + dt.Rows.Count.ToString();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void FillTBMPCRData()
        {
            try
            {
                DataTable dt = new DataTable();

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                
                myConnection.comm.CommandText = "Select recipeCode, wcName, gtbarcode AS quantity from vTbmPCR where dtandtime>='" + datetime + "'";//DateTime.Today.ToString("dd-MM-yyyy");
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("quantity", typeof(string));
                var query = dt.AsEnumerable()
               .GroupBy(row => row.Field<string>("wcName"))
               .Select(g =>
               {
               var row = gridviewdt.NewRow();
               row.SetField("wcName", g.Key);
               row.SetField("Quantity", g.Count());
               return row;
               }).CopyToDataTable();

                var rows = (from row in query.AsEnumerable()
                            orderby row["wcName"] ascending
                            select row);
                DataTable fdt = rows.AsDataView().ToTable();
                TBMPCRChart.DataSource = fdt;
                TBMPCRChart.Series["TBMPCRSeries"].XValueMember = "wcName";
                TBMPCRChart.Series["TBMPCRSeries"].YValueMembers = "quantity";
                TBMPCRChart.DataBind();

                DataTable recipedt = new DataTable();
                recipedt.Columns.Add("recipeCode", typeof(string));
                recipedt.Columns.Add("quantity", typeof(string));
                var recipequery = dt.AsEnumerable()
               .GroupBy(row => row.Field<string>("recipeCode"))
               .Select(g =>
               {
               var row = recipedt.NewRow();
               row.SetField("recipeCode", g.Key);
               row.SetField("Quantity", g.Count());
              return row;
              }).CopyToDataTable();
                TBMPCRRecipeChart.DataSource = recipequery;
                TBMPCRRecipeChart.Series["TBMPCRRecipeSeries"].XValueMember = "recipeCode";
                TBMPCRRecipeChart.Series["TBMPCRRecipeSeries"].YValueMembers = "quantity";
                TBMPCRRecipeChart.DataBind();

                TBMProductionLabel.Text = "Total TBM : " + dt.Rows.Count.ToString();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void FillTBRCURData()
        {
            try
            {
                DataTable dt = new DataTable();
            
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName, wcName AS quantity from vCuringproduction1 where processid='5' and dtandtime>='" + datetime + "'";//DateTime.Today.ToString("dd-MM-yyyy");
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("quantity", typeof(string));
                var query = dt.AsEnumerable()
    .GroupBy(row => row.Field<string>("wcName"))
    .Select(g =>
    {
        var row = gridviewdt.NewRow();
        row.SetField("wcName", g.Key);
        row.SetField("Quantity", g.Count());
        return row;
    }).CopyToDataTable();
                var rows = (from row in query.AsEnumerable()
                             orderby row["wcName"] ascending
                             select row);
                DataTable fdt = rows.AsDataView().ToTable();
 
                TBRCURChart.DataSource = fdt;
                TBRCURChart.Series["TBRCURSeries"].XValueMember = "wcName";
                TBRCURChart.Series["TBRCURSeries"].YValueMembers = "quantity";
                TBRCURChart.DataBind();

                CURProductionLabel.Text = "Total Curing : " + dt.Rows.Count.ToString();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void FillPCRCURData()
        {
            
            try
            {
                DataTable dt = new DataTable();
            
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName, wcName AS quantity from vCuringproduction1 where processid='8' and dtandtime>='" + datetime + "'";//DateTime.Today.ToString("dd-MM-yyyy");
                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("wcName", typeof(string));
                gridviewdt.Columns.Add("quantity", typeof(string));
                var query = dt.AsEnumerable()
    .GroupBy(row => row.Field<string>("wcName"))
    .Select(g =>
    {
        var row = gridviewdt.NewRow();
        row.SetField("wcName", g.Key);
        row.SetField("Quantity", g.Count());
        return row;
    }).CopyToDataTable();
                var rows = (from row in query.AsEnumerable()
                            orderby row["wcName"] ascending
                            select row);
                DataTable fdt = rows.AsDataView().ToTable();
                PCRCURChart.DataSource = fdt;
                PCRCURChart.Series["PCRCURSeries"].XValueMember = "wcName";
                PCRCURChart.Series["PCRCURSeries"].YValueMembers = "quantity";
                PCRCURChart.DataBind();

                CURProductionLabel.Text = "Total Curing : " + dt.Rows.Count.ToString();
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

    }
}