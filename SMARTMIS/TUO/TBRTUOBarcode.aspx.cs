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
    public partial class TBRTUOBarcode : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable mainGVdt;
        DataTable dynamicDB = new DataTable();
        DataTable weightdt;

        string fromdate = "", todate = "";


        string wherequery = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (BarcodeFromTOWiseReport.Checked)
            {
                BarcodeFromToDiv.Visible = true;
                DatefromtoDiv.Visible = false;
            }
            else if (DateFromToReport.Checked)
            {
                BarcodeFromToDiv.Visible = false;
                DatefromtoDiv.Visible = true;
            }

            if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }
        }
        private void createGridView(DataTable gridviewdt, GridView gridview)
        {
            gridview.Columns.Clear();
            //Iterate through the columns of the datatable to set the data bound field dynamically.
            foreach (DataColumn col in gridviewdt.Columns)
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
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button clickedbutton = sender as Button;

                int totalrank = 0;
                mainGVdt = new DataTable();
                string[] mainGVdtColumns = new string[] { "MachineName", "TestTime", "TyreType", "Barcode", "Uniformity_Grade", "RFVCW", "RFV1HCW", "RFV1H0CW", "RFV1HCCW", "RFVCCW", "LFV1HCW", "LFVCW", "Lfv1HCCW", "LFVCCW", "CONICITY", "CONCALC", "PLY", "Weight", "TBM_MachineName", "Builder_Name", "TBM_dtandTime", "Press_Name", "Curing_Operator_Name", "Curing_dtandTime" };
                for (int i = 0; i < mainGVdtColumns.Count(); i++)
                    mainGVdt.Columns.Add(mainGVdtColumns[i]);

                createGridView(mainGVdt, MainGridView);

                if (clickedbutton.ID == "BarcodeWiseButton")
                {
                    int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text.Trim());
                    int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                    if (frombarcode.ToString().Length == 8)
                        wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray());
                    else if (frombarcode.ToString().Length == 7)
                        wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());


                }

                loadData();



                DataRow dr = mainGVdt.NewRow();
               mainGVdt.Rows.Add(dr);

                //foreach (DataRow row in mainGVdt.Rows)
                //{
                //    //if (row[0].ToString() == "30-352" || row[0].ToString() == "30-353" || row[0].ToString() == "30-354")
                //    //{
                //        var result = dynamicDB.AsEnumerable().Where(dr1 => dr1.Field<string>("BARCODE") == row[3].ToString()).ToList();

                //        if (result.Count >= 1)
                //        {

                //            row[0] = result[result.Count - 1][0].ToString();
                //            row[1] = result[result.Count - 1][1].ToString();
                //            row[2] = result[result.Count - 1][2].ToString();
                //            row[3] = result[result.Count - 1][3].ToString();
                //            row[4] = result[result.Count - 1][4].ToString();
                //            row[5] = result[result.Count - 1][5].ToString();
                //            row[6] = result[result.Count - 1][6].ToString();
                //            row[7] = result[result.Count - 1][7].ToString();
                //            row[8] = result[result.Count - 1][8].ToString();
                //            row[9] = result[result.Count - 1][9].ToString();
                //            row[10] = result[result.Count - 1][10].ToString();
                //            row[11] = result[result.Count - 1][11].ToString();
                //            row[12] = result[result.Count - 1][12].ToString();
                            
                //        }
                //        else
                //        {
                //            row[0] = "";
                //            row[1] ="";
                //            row[2] = "";
                //            row[3] = "";
                //            row[4] = "";
                //            row[5] = "";
                //            row[6] = "";
                //            row[7] = "";
                //            row[8] = "";
                //            row[9] = "";
                //            row[10] = "";
                //            row[11] = "";
                            
                //        }

                //}
                if (!DateFromToReport.Checked)
                {
                    mainGVdt.DefaultView.Sort = "barcode ASC";

                    DataView dw = mainGVdt.DefaultView;
                    DataTable sorteddt = dw.ToTable();

                    MainGridView.DataSource = mainGVdt;
                    MainGridView.DataBind();
                }
                else
                {
                    MainGridView.DataSource = mainGVdt;
                    MainGridView.DataBind();
                }


            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void FaultTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadData()
        {
            try
            {
                DataTable dtTUO = new DataTable();
                DataTable curdt = new DataTable();
                DataTable tbmdt = new DataTable();
                DataTable manndt = new DataTable();
                DataTable wcdt = new DataTable();
                weightdt = new DataTable();

                //Get WC details
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select ID AS wcMasterID, name AS wcName FROM wcMaster";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    wcdt.Load(myConnection.reader);
                }
                catch (Exception exc)
                { }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                }

                //Get Manning details
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select ID AS manningMasterID, firstName, lastName FROM manningMaster";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    manndt.Load(myConnection.reader);
                }
                catch (Exception exc)
                { }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }



                if (DateFromToReport.Checked)
                {
                    fromdate = myWebService.formatDate(tuoReportMasterFromDateTextBox.Text.ToString()) + " 07:00:00";
                    todate = myWebService.formatDate(tuoReportMasterToDateTextBox.Text.ToString()) + " 07:00:00";

                    string fromDate = myWebService.formatDate(tuoReportMasterFromDateTextBox.Text.ToString()); 
                    string toDate = myWebService.formatDate(tuoReportMasterToDateTextBox.Text.ToString());

                    //TimeSpan ts = DateTime.Parse(toDate) - DateTime.Parse(fromDate);
                    //int result = (int)ts.TotalDays;
                    //if ((int)ts.TotalDays >7)
                    //{
                    //    ShowWarning.Visible = true;
                    //    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more  than 7 days!!!</font></strong></td></tr></table>";
                    //}

                    List<string> conditions = new List<string>();
                    string query = "";




                    if (GradeDropDownList.SelectedItem.Text != "All")
                    {
                        conditions.Add("TotalRank='" + GradeDropDownList.SelectedItem.Text + "'");

                    }
                    else
                    {
                       
                    }
                    if (conditions.Any())
                        query = " AND " + string.Join(" AND ", conditions.ToArray());

                    //get barcode weight 
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "select distinct(gtbarcode), Weight FROM BuddeScannedTyreDetail t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM BuddeScannedTyreDetail t2 WHERE t2.gtbarcode = t1.gtbarcode and dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND gtbarcode not in ('??????????','') and stationNo='1' ) and dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'"; 

                        //myConnection.comm.CommandText = "Select distinct(gtbarcode), Weight FROM BuddeScannedTyreDetail where (dtandTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') and gtbarcode not in ('??????????','') and stationNo='1' order by gtbarcode";
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        weightdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }




                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = "Select name as MachineName ,dtandTime, recipecode AS TyreType,BARCODE,TotalRank, RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where (dtandtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " ";
                        myConnection.comm.CommandText = "select name as MachineName,dtandTime, recipecode AS TyreType,BARCODE,TotalRank, RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply  from vTBRUniformityData t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vTBRUniformityData t2 WHERE t2.barcode = t1.barcode and dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' " + query + ") and dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "' " + query + " order by dtandTime desc";
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        dtTUO.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }
                    

                    //Get TBM details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmtbR t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vTbmtbR t2  WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')  and (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss")  +"' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            //"Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmtbR where (dtandTime>'" + Convert.ToDateTime(fromdate).";AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        tbmdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }
                    //Get CUR details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringtbr t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vCuringtbr t2  WHERE t2.gtbarCode = t1.gtbarCode and dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')  and (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            //"Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringtbr where (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        curdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                }
                else if (BarcodeFromTOWiseReport.Checked)
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select distinct gtbarcode ,Weight FROM BuddeScannedTyreDetail where gtbarcode IN (" + wherequery + ") and stationNo='1' order by gtbarcode";
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        weightdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally





                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }


                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = "Select name as MachineName ,dtandtime, recipecode AS TyreType,BARCODE,TotalRank , RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vTBRUniformityData t2 WHERE t2.barcode = t1.barcode and barcode IN (" + wherequery + ")) and barcode IN (" + wherequery + "))order by name asc";
                        myConnection.comm.CommandText = "Select name as MachineName ,dtandtime, recipecode AS TyreType,BARCODE,TotalRank , RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where  barcode IN (" + wherequery + ") order by name asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dtTUO.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    { }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }



                    //try
                    //{
                    //    myConnection.open(ConnectionOption.SQL);
                    //    myConnection.comm = myConnection.conn.CreateCommand();
                    //    myConnection.comm.CommandText = "Select name as MachineName ,dtandtime, recipecode AS TyreType,BARCODE,TotalRank , RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData where  barcode IN (" + wherequery + ") order by name asc";
                    //    myConnection.reader = myConnection.comm.ExecuteReader();
                    //    dynamicDB.Load(myConnection.reader);

                    //}
                    //catch (Exception exc)
                    //{

                    //}
                    //finally
                    //{
                    //    if (!myConnection.reader.IsClosed)
                    //        myConnection.reader.Close();
                    //    myConnection.comm.Dispose();
                    //}

                    //Get TBM details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmTBR where gtbarCode IN (" + wherequery + ")";
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        tbmdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {
                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                    }

                    //Get CUR details
                    try
                    {
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringTBr where gtbarCode IN (" + wherequery + ")";
                        myConnection.comm.CommandTimeout = 0;
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        curdt.Load(myConnection.reader);
                    }
                    catch (Exception exc)
                    {

                    }
                    finally
                    {
                        if (!myConnection.reader.IsClosed)
                            myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                }

                var row = from r0w1 in dtTUO.AsEnumerable()
                          join row4 in weightdt.AsEnumerable()
                        on r0w1.Field<string>("barcode") equals row4.Field<string>("gtbarcode") into psw
                          from row4 in psw.DefaultIfEmpty()
                          join r0w2 in tbmdt.AsEnumerable()
                            on r0w1.Field<string>("barcode") equals r0w2.Field<string>("tbmgtbarCode") into p
                          from r0w2 in p.DefaultIfEmpty()
                          join r0w3 in curdt.AsEnumerable()
                            on r0w1.Field<string>("barcode") equals r0w3.Field<string>("curgtbarCode") into ps
                          from r0w3 in ps.DefaultIfEmpty()

                          select r0w1.ItemArray.Concat(row4 != null ? row4.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "" }).ToArray();

                foreach (object[] values in row)
                    mainGVdt.Rows.Add(values);
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void BarcodeFromTUOWiseReport_CheckedChanged(object sender, EventArgs e)
        {

            if (BarcodeFromTOWiseReport.Checked)
            {
                BarcodeFromToDiv.Visible = true;
                DatefromtoDiv.Visible = false;
            }
            else if (DateFromToReport.Checked)
            {
                BarcodeFromToDiv.Visible = false;
                DatefromtoDiv.Visible = true;

                tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                tuoReportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");


            }


        }
        protected void tbExportToexcel_Click(object sender, EventArgs e)
        {
            try
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
