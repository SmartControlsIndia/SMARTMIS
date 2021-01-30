using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;


namespace SmartMIS.TUO
{
    public partial class TUOBarcodeWiseReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
       DataTable mainGVdt;       
        string fromdate = "", todate = "";


        string wherequery = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (BarcodeFromTOWiseReport.Checked)
            {
                BarcodeFromToDiv.Visible = true;
                DatefromtoDiv.Visible = false;
            }
            else if (DateFromTOReport.Checked)
            {
                BarcodeFromToDiv.Visible = false;
                DatefromtoDiv.Visible = true;
 
            }

           if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }
        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            Button clickedbutton = sender as Button;

           int totalrank = 0;
            mainGVdt = new DataTable();
            mainGVdt.Columns.Add("wcName");
            mainGVdt.Columns.Add("dtandTime");
            mainGVdt.Columns.Add("recipeCode");
            mainGVdt.Columns.Add("barcode");
            mainGVdt.Columns.Add("Total_Rank");
            mainGVdt.Columns.Add("CWLFVOA_N");
      
            mainGVdt.Columns.Add("CWLFVOA_Rank");
            mainGVdt.Columns.Add("CWRFV_OA_N");

            mainGVdt.Columns.Add("CWRFV_OA_Rank");
            mainGVdt.Columns.Add("CWRFVOA_1H_N");
   
            mainGVdt.Columns.Add("CWRFVOA_1H_Rank");
            mainGVdt.Columns.Add("CCWRFVOA_N");
  
            mainGVdt.Columns.Add("CCWRFVOA_Rank");
            mainGVdt.Columns.Add("CCWRFVOA_1H_N");
   
            mainGVdt.Columns.Add("CCWRFVOA_1H_Rank");
            mainGVdt.Columns.Add("CCWLFVOA_N");

            mainGVdt.Columns.Add("CCWLFVOA_Rank");
            mainGVdt.Columns.Add("CON_N");        
            mainGVdt.Columns.Add("CON_Rank");
            mainGVdt.Columns.Add("PLY_N");
            mainGVdt.Columns.Add("PLY_Rank");
            mainGVdt.Columns.Add("DB_wcname");
            mainGVdt.Columns.Add("DB_dtandtime");
            mainGVdt.Columns.Add("DB_recipecode");
            mainGVdt.Columns.Add("DB_barcode");
            mainGVdt.Columns.Add("TotalRank");
            mainGVdt.Columns.Add("RORank");
            mainGVdt.Columns.Add("LROT1OAAmount");

            mainGVdt.Columns.Add("LROT1OARank");
            mainGVdt.Columns.Add("LROB1OAAmount");
    
            mainGVdt.Columns.Add("LROB1OARank");
            mainGVdt.Columns.Add("RROCOAAmount");
  
            mainGVdt.Columns.Add("RROCOARank");
            mainGVdt.Columns.Add("LROT1BulgeAmount");
 
            mainGVdt.Columns.Add("LROT1BulgeRank");
            mainGVdt.Columns.Add("LROB1BulgeAmount");
 
            mainGVdt.Columns.Add("LROB1BulgeRank");
            mainGVdt.Columns.Add("LROT1DentAmount");
   
            mainGVdt.Columns.Add("LROT1DentRank");
            mainGVdt.Columns.Add("LROB1DentAmount");
      
            mainGVdt.Columns.Add("LROB1DentRank");
            mainGVdt.Columns.Add("UpperAmount");
     
            mainGVdt.Columns.Add("UpperRank");
            mainGVdt.Columns.Add("LowerAmount");
   
            mainGVdt.Columns.Add("LowerRank");
            mainGVdt.Columns.Add("StaticAmount");
     
            mainGVdt.Columns.Add("StaticRank");


            if (clickedbutton.ID == "BarcodeWiseButton")
            {
                int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text);
                int tocount = Convert.ToInt32(barcodeToTextBox.Text);
                int barcode;

                wherequery ="'"+ frombarcode.ToString()+"'";
                for (int i = 0; i < tocount; i++)
                {
                    barcode = frombarcode + i;
                    wherequery = wherequery + "," +"'"+ barcode+"'";
                }


            }

            loadData();
            foreach (DataRow row in mainGVdt.Rows)
            {
                for (int i = 4; i < mainGVdt.Columns.Count-1; i++)
                {
                    if (row[i].ToString() == "1")
                        row.SetField(i, "A");
                    else if (row[i].ToString() == "2")
                        row.SetField(i, "B");
                    else if (row[i].ToString() == "3")
                        row.SetField(i, "C");
                    else if (row[i].ToString() == "4")
                        row.SetField(i, "D");
                    else if (row[i].ToString() == "5")
                        row.SetField(i, "E");
                }
            }
            mainGVdt.Columns.Remove("DB_wcname");
            mainGVdt.Columns.Remove("DB_dtandtime");
            mainGVdt.Columns.Remove("DB_recipecode");
            mainGVdt.Columns.Remove("DB_barcode");

            if (clickedbutton.ID != "BarcodeWiseButton" && GradeDropDownList.SelectedItem.Text != "All")
            {
                mainGVdt = mainGVdt.Select("Total_Rank='" + GradeDropDownList.SelectedItem.Text + "'").CopyToDataTable();

            }
            MainGridView.DataSource = mainGVdt;
            MainGridView.DataBind();
          
        }
        protected void FaultTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadData()
        {
           
            DataTable dtDB = new DataTable();
            DataTable dtTUO = new DataTable();
            DataTable dtMP = new DataTable();
            if (!BarcodeFromTOWiseReport.Checked)
            {
                fromdate = myWebService.formatDate(tuoReportMasterFromDateTextBox.Text).ToString() + " " + "07:00:00";
                todate = myWebService.formatDate(tuoReportMasterToDateTextBox.Text).ToString() + " " + "07:00:00";
                if (MachineDropDownList.SelectedItem.Text == "All" || MachineDropDownList.SelectedItem.Text == "TUO-1" || MachineDropDownList.SelectedItem.Text == "TUO-2")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (MachineDropDownList.SelectedItem.Text == "All")
                        myConnection.comm.CommandText = "select wcname, dtandtime, recipeCode, barcode,total_rank,CWLFVOA_N, CWLFVOA_Rank,CWRFV_OA_N, CWRFV_OA_Rank, CWRFVOA_1H_N,CWRFVOA_1H_Rank,CCWRFVOA_Rank as  CCWLFVOA_N, ccwrfvoa_1H_Deg as CCWRFVOA_Rank, CCWRFVOA_1H_Rank as CCWRFVOA_1H_N ,CCWRFVOA_2H_Deg as ccwrfvoa_1H_Rank ,CCWLFVOA_Rank as CCWLFVOA_N ,CCWLFVOA_1H_Deg as CCWLFVOA_Rank,PLY_N as CON_N,PLY_Rank as CON_Rank,UFM_Rank as PLY_N,LTOA_MM as PLY_Rank from ProductionDataTUO  where (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";
                    else if (MachineDropDownList.SelectedItem.Text == "TUO-1")
                        myConnection.comm.CommandText = "select wcname, dtandtime, recipeCode, barcode,total_rank,CWLFVOA_N, CWLFVOA_Rank,CWRFV_OA_N, CWRFV_OA_Rank, CWRFVOA_1H_N,CWRFVOA_1H_Rank,CCWRFVOA_Rank as  CCWLFVOA_N, ccwrfvoa_1H_Deg as CCWRFVOA_Rank, CCWRFVOA_1H_Rank as CCWRFVOA_1H_N ,CCWRFVOA_2H_Deg as ccwrfvoa_1H_Rank ,CCWLFVOA_Rank as CCWLFVOA_N ,CCWLFVOA_1H_Deg as CCWLFVOA_Rank,PLY_N as CON_N,PLY_Rank as CON_Rank,UFM_Rank as PLY_N,LTOA_MM as PLY_Rank from ProductionDataTUO   where  wcName='TUO-1' and (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";
                    else if (MachineDropDownList.SelectedItem.Text == "TUO-2")
                        myConnection.comm.CommandText = "select wcname, dtandtime, recipeCode, barcode,total_rank,CWLFVOA_N, CWLFVOA_Rank,CWRFV_OA_N, CWRFV_OA_Rank, CWRFVOA_1H_N,CWRFVOA_1H_Rank,CCWRFVOA_Rank as  CCWLFVOA_N, ccwrfvoa_1H_Deg as CCWRFVOA_Rank, CCWRFVOA_1H_Rank as CCWRFVOA_1H_N ,CCWRFVOA_2H_Deg as ccwrfvoa_1H_Rank ,CCWLFVOA_Rank as CCWLFVOA_N ,CCWLFVOA_1H_Deg as CCWLFVOA_Rank,PLY_N as CON_N,PLY_Rank as CON_Rank,UFM_Rank as PLY_N,LTOA_MM as PLY_Rank from ProductionDataTUO   where wcName='TUO-2' and (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtTUO.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (MachineDropDownList.SelectedItem.Text == "All")
                        myConnection.comm.CommandText = "select wcname ,dtandtime, recipecode,barcode,TotalRank,RORank,LROT1OAAmount,LROT1OARank,LROB1OAAmount,LROB1OARank,RROCOAAmount,RROCOARank,LROT1BulgeAmount,LROT1BulgeRank,LROB1BulgeAmount,LROB1BulgeRank,LROT1DentAmount,LROT1DentRank,LROB1DentAmount,LROB1DentRank,UpperAmount, UpperRank,LowerAmount,LowerRank,StaticAmount,StaticRank  from productionDataDB where (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";
                    else if (MachineDropDownList.SelectedItem.Text == "TUO-1")
                        myConnection.comm.CommandText = "select wcname ,dtandtime, recipecode,barcode,TotalRank,RORank,LROT1OAAmount,LROT1OARank,LROB1OAAmount,LROB1OARank,RROCOAAmount,RROCOARank,LROT1BulgeAmount,LROT1BulgeRank,LROB1BulgeAmount,LROB1BulgeRank,LROT1DentAmount,LROT1DentRank,LROB1DentAmount,LROB1DentRank,UpperAmount, UpperRank,LowerAmount,LowerRank,StaticAmount,StaticRank  from productionDataDB where wcName='TUO-1' and (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";
                    else if (MachineDropDownList.SelectedItem.Text == "TUO-2")
                        myConnection.comm.CommandText = "select wcname ,dtandtime, recipecode,barcode,TotalRank,RORank,LROT1OAAmount,LROT1OARank,LROB1OAAmount,LROB1OARank,RROCOAAmount,RROCOARank,LROT1BulgeAmount,LROT1BulgeRank,LROB1BulgeAmount,LROB1BulgeRank,LROT1DentAmount,LROT1DentRank,LROB1DentAmount,LROB1DentRank,UpperAmount, UpperRank,LowerAmount,LowerRank,StaticAmount,StaticRank  from productionDataDB where wcName='TUO-2' and (dtandtime>'" + fromdate + "' AND dtandTime<'" + todate + "') order by wcName asc";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtDB.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    var row = from r0w1 in dtTUO.AsEnumerable()
                              join r0w2 in dtDB.AsEnumerable()
                              on r0w1.Field<string>("barcode").Trim() equals r0w2.Field<string>("barcode").Trim()
                              select r0w1.ItemArray.Concat(r0w2.ItemArray).ToArray();
                    foreach (object[] values in row)
                        mainGVdt.Rows.Add(values);
                }

                if (MachineDropDownList.SelectedItem.Text == "All" || MachineDropDownList.SelectedItem.Text == "TUO-3")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select wcName,testTime as dtandtime,tireType as recipeCode,barcode,uniformityGrade as totalrank,LFVCW as CWLFVOA_N, GradeLFVCW as CWLFVOA_Rank,RFVCW as CWRFV_OA_N, GradeRFVCW as CWRFV_OA_Rank,H1RFVCCW as CWRFVOA_1H_N,GradeH1RFVCW as CWRFVOA_1H_Rank,RFVCCW as CCWRFVOA_N,GradeRFVCCW as CCWRFVOA_Rank,H1RFVCCW as CCWRFVOA_1H_N,GradeH1RFVCCW as CCWRFVOA_1H_Rank,LFVCCW as CCWLFVOA_N,GradeLFVCCW as CCWLFVOA_Rank,CONICITY as CON_N ,GradeCONICITY as CON_Rank,'0' as ply_N,'0' as PLY_Rank,'0' as DB_wcname,'0' as DB_dtandtime,'0' as DB_recipecode,'0' as DB_barcode,LowerLRO as LROT1OAAmount, GradeLowerLRO as LROT1OARank,UpperLRO as LROB1OAAmount,GradeUpperLRO as LROB1OARank,RRO asRROCOAAmount , GradeRRO as RROCOARank,LowerBulge as LROT1BulgeAmount ,GradeLowerBulge as LROT1BulgeRank,UpperBulge as LROB1BulgeAmount ,GradeUpperBulge as LROB1BulgeRank, LowerDepression as LROT1DentAmount,GradeLowerDepression as LROT1DentRank,UpperDepression as LROB1DentAmount ,GradeUpperDepression as LROB1DentRank,'0' as UpperAmount,'0' as UpperRank,'0' as LowerAmount , '0' as LowerRank,'0' as staticAmount,'0' as StaticRank from MicropoiseProductionDataTUO   where (testTime>'" + fromdate + "' AND testTime<'" + todate + "')";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtMP.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    var row1 = from r0w1 in dtMP.AsEnumerable()
                               select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row1)
                        mainGVdt.Rows.Add(values);

                }
            }
            else
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
               myConnection.comm.CommandText = "select wcname, dtandtime, recipeCode, barcode,total_rank,CWLFVOA_N, CWLFVOA_Rank,CWRFV_OA_N, CWRFV_OA_Rank, CWRFVOA_1H_N,CWRFVOA_1H_Rank,CCWRFVOA_Rank as  CCWLFVOA_N, ccwrfvoa_1H_Deg as CCWRFVOA_Rank, CCWRFVOA_1H_Rank as CCWRFVOA_1H_N ,CCWRFVOA_2H_Deg as ccwrfvoa_1H_Rank ,CCWLFVOA_Rank as CCWLFVOA_N ,CCWLFVOA_1H_Deg as CCWLFVOA_Rank,PLY_N as CON_N,PLY_Rank as CON_Rank,UFM_Rank as PLY_N,LTOA_MM as PLY_Rank from ProductionDataTUO  where barcode in("+wherequery+") order by wcName asc";              
                myConnection.reader = myConnection.comm.ExecuteReader();
                dtTUO.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                 myConnection.comm.CommandText = "select wcname ,dtandtime, recipecode,barcode,TotalRank,RORank,LROT1OAAmount,LROT1OARank,LROB1OAAmount,LROB1OARank,RROCOAAmount,RROCOARank,LROT1BulgeAmount,LROT1BulgeRank,LROB1BulgeAmount,LROB1BulgeRank,LROT1DentAmount,LROT1DentRank,LROB1DentAmount,LROB1DentRank,UpperAmount, UpperRank,LowerAmount,LowerRank,StaticAmount,StaticRank  from productionDataDB where barcode in("+wherequery+") order by wcName asc";
                myConnection.reader = myConnection.comm.ExecuteReader();
                dtDB.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                if (dtTUO.Rows.Count > 0 && dtDB.Rows.Count > 0)
                {
                    var row = from r0w1 in dtTUO.AsEnumerable()
                              join r0w2 in dtDB.AsEnumerable()
                              on r0w1.Field<string>("barcode").Trim() equals r0w2.Field<string>("barcode").Trim()
                              select r0w1.ItemArray.Concat(r0w2.ItemArray).ToArray();
                    foreach (object[] values in row)
                        mainGVdt.Rows.Add(values);
                }



                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select wcName,testTime as dtandtime,tireType as recipeCode,barcode,uniformityGrade as totalrank,LFVCW as CWLFVOA_N, GradeLFVCW as CWLFVOA_Rank,RFVCW as CWRFV_OA_N, GradeRFVCW as CWRFV_OA_Rank,H1RFVCCW as CWRFVOA_1H_N,GradeH1RFVCW as CWRFVOA_1H_Rank,RFVCCW as CCWRFVOA_N,GradeRFVCCW as CCWRFVOA_Rank,H1RFVCCW as CCWRFVOA_1H_N,GradeH1RFVCCW as CCWRFVOA_1H_Rank,LFVCCW as CCWLFVOA_N,GradeLFVCCW as CCWLFVOA_Rank,CONICITY as CON_N ,GradeCONICITY as CON_Rank,'0' as ply_N,'0' as PLY_Rank,'0' as DB_wcname,'0' as DB_dtandtime,'0' as DB_recipecode,'0' as DB_barcode,LowerLRO as LROT1OAAmount, GradeLowerLRO as LROT1OARank,UpperLRO as LROB1OAAmount,GradeUpperLRO as LROB1OARank,RRO asRROCOAAmount , GradeRRO as RROCOARank,LowerBulge as LROT1BulgeAmount ,GradeLowerBulge as LROT1BulgeRank,UpperBulge as LROB1BulgeAmount ,GradeUpperBulge as LROB1BulgeRank, LowerDepression as LROT1DentAmount,GradeLowerDepression as LROT1DentRank,UpperDepression as LROB1DentAmount ,GradeUpperDepression as LROB1DentRank,'0' as UpperAmount,'0' as UpperRank,'0' as LowerAmount , '0' as LowerRank,'0' as staticAmount,'0' as StaticRank from MicropoiseProductionDataTUO   where barcode in("+wherequery+") ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                dtMP.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
                if (dtMP.Rows.Count > 0)
                {
                    var row1 = from r0w1 in dtMP.AsEnumerable()
                               select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row1)
                        mainGVdt.Rows.Add(values);
                }

            }

        }

        protected void BarcodeFromTOWiseReport_CheckedChanged(object sender, EventArgs e)
        {

            if (BarcodeFromTOWiseReport.Checked)
            {
                BarcodeFromToDiv.Visible = true;
                DatefromtoDiv.Visible = false;
            }
            else if (DateFromTOReport.Checked)
            {
                BarcodeFromToDiv.Visible = false;
                DatefromtoDiv.Visible = true;

            }


        }

        protected void tbExportToexcel_Click(object sender, EventArgs e)
        {

            Response.Clear();

            Response.AddHeader("content-disposition", "attachment; filename=Products.xls");
            Response.Charset = "";

            Response.ContentType = "application/vnd.xls";

            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

            MainGridView.RenderControl(htmlWriter);
            Response.Write(stringWriter.ToString());

            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

    }
}
