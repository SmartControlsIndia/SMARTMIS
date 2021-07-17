using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;


namespace SmartMIS.TUO
{
    public partial class TUOBarcodeWiseReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
       DataTable mainGVdt;
       DataTable dynamicDB = new DataTable();
       DataTable dynamicDBOLD = new DataTable();
       DataTable weightdt;

        string fromdate = "", todate = "";


        string wherequery = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { fillSizedropdownlist(); }
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
        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " where " + coloumnName + " != '0' and tyreSize!='' and ProcessId=7";

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
        private void fillSizedropdownlist()
        {

            //ddlRecipe.DataSource = null;
            ddlRecipe.DataSource = FillDropDownList("recipemaster", "name");
            ddlRecipe.DataBind();
        }
        protected void ViewButton_Click(object sender, EventArgs e)
      {
            try
            {
                Button clickedbutton = sender as Button;

                int totalrank = 0;
                mainGVdt = new DataTable();
                string[] mainGVdtColumns = new string[] { "MachineName", "TestTime", "TyreType", "Barcode", "Uniformity_Grade", "RFVCW", "GradeRFVCW", "RFVCCW", "GRADERFVCCW", "H1RFVCW", "GradeH1RFVCW", "H1RFVCCW", "GradeH1RFVCCW","H2RFVCW", "GradeH2RFVCW" ,"H2RFVCCW", "GradeH2RFVCCW" , "LFVCW", "GradeLFVCW", "LFVCCW", "GradeLFVCCW", "CONICITY", "GradeCONICITY", "LowerBulge", "GradeLOwerBulge", "UpperBulge", "GradeUpperBulge", "LowerLRO", "GradeLowerLRO", "UpperLRO", "GradeUpperLRO", "RRO", "GradeRRO", "UpperRRO", "GradeUpperRRO", "LowerDepression", "GradeLowerDepression", "UpperDepression", "GradeUpperDepression", "Static", "StaticAngle", "StaticGrade", "Couple", "CoupleAngle", "coupleGrade", "Upper", "UpperAngle", "UpperGrade", "Lower", "LowerAngle", "LowerGrade", "GradeDef", "Weight", "TBM_MachineName", "Builder_Name", "TBM_dtandTime", "Press_Name", "CavityNo", "MouldNo", "Curing_Operator_Name", "Curing_dtandTime", "GTWeight", "FinalVI" };
                for (int i = 0; i < mainGVdtColumns.Count(); i++)
                    mainGVdt.Columns.Add(mainGVdtColumns[i]);

                createGridView(mainGVdt, MainGridView);

                if (clickedbutton.ID == "BarcodeWiseButton")
                {
                    int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text.Trim());
                    int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                    if(frombarcode.ToString().Length==8)
                    wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray());
                    else if(frombarcode.ToString().Length==7)
                    wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());
                    else if (frombarcode.ToString().Length == 6)
                        wherequery = "'0000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'0000" + (frombarcode + i) + "'").ToArray());
                    else if (frombarcode.ToString().Length == 5)
                        wherequery = "'00000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00000" + (frombarcode + i) + "'").ToArray());
                    else if (frombarcode.ToString().Length == 9)
                        wherequery = "'0" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'0" + (frombarcode + i) + "'").ToArray());

                }

                loadData();


              
                DataRow dr = mainGVdt.NewRow();
                mainGVdt.Rows.Add(dr);

                foreach (DataRow row in mainGVdt.Rows)
                {
                    if (row[0].ToString() == "30-352" || row[0].ToString() == "30-353" || row[0].ToString() == "30-354")
                    {
                        var result = dynamicDB.AsEnumerable().Where(dr1 => dr1.Field<string>("BARCODE") == row[3].ToString()).ToList();

                        if (result.Count >= 1)
                        {


                            //row[19] = result[result.Count - 1][4].ToString();
                            //row[20] = result[result.Count - 1][5].ToString();
                            //row[21] = result[result.Count - 1][4].ToString();
                            //row[22] = result[result.Count - 1][5].ToString();
                            row[23] = result[result.Count - 1][4].ToString();
                            row[24] = result[result.Count - 1][5].ToString();
                            row[25] = result[result.Count - 1][6].ToString();
                            row[26] = result[result.Count - 1][7].ToString();
                            row[27] = result[result.Count - 1][8].ToString();
                            row[28] = result[result.Count - 1][9].ToString();
                            row[29] = result[result.Count - 1][10].ToString();
                            row[30] = result[result.Count - 1][11].ToString();
                            row[31] = result[result.Count - 1][12].ToString();
                            row[32] = result[result.Count - 1][13].ToString();
                            row[33] = "";
                            row[34] = "";
                            row[35] = result[result.Count - 1][14].ToString();
                            row[36] = result[result.Count - 1][15].ToString();
                            row[37] = result[result.Count - 1][16].ToString();
                            row[38] = result[result.Count - 1][17].ToString();
                            row[39] = result[result.Count - 1][18].ToString();
                            row[40] = result[result.Count - 1][19].ToString();
                            row[41] = result[result.Count - 1][20].ToString();
                            row[42] = result[result.Count - 1][21].ToString();
                            row[43] = result[result.Count - 1][22].ToString();
                            row[44] = result[result.Count - 1][23].ToString();
                            row[45] = result[result.Count - 1][24].ToString();
                            row[46] = result[result.Count - 1][25].ToString();
                            row[47] = result[result.Count - 1][26].ToString();
                            row[48] = result[result.Count - 1][27].ToString();
                            row[49] = result[result.Count - 1][28].ToString();
                            row[50] = result[result.Count - 1][29].ToString();
                            row[51] = result[result.Count - 1][30].ToString();

                        }
                        else
                        {

                            // row[19] = "";
                            //row[20] = "";
                            //row[21] = "";
                            //row[22] = "";
                            row[23] = "";
                            row[24] = "";
                            row[25] = "";
                            row[26] = "";
                            row[27] = "";
                            row[28] = "";
                            row[29] = "";
                            row[30] = "";
                            row[31] = "";
                            row[32] = "";
                            row[33] = "";
                            row[34] = "";
                            row[35] = "";
                            row[36] = "";
                            row[37] = "";
                            row[38] = "";
                            row[39] = "";
                            row[40] = "";
                            row[41] = "";
                            row[42] = "";
                            row[43] = "";
                            row[44] = "";
                            row[45] = "";
                            row[46] = "";
                            row[47] = "";
                            row[48] = "";
                            row[49] = "";
                            row[50] = "";
                            row[51] = "";
                            // row[49] = "";
                        }
                    }
                    else if (row[0].ToString() == "30-349" || row[0].ToString() == "30-350")
                    
                    {
                        var result = dynamicDBOLD.AsEnumerable().Where(dr1 => dr1.Field<string>("BARCODE") == row[3].ToString()).ToList();
                        //var result = dynamicDB.AsEnumerable().ToList();

                        if (result.Count >= 1)
                        {
                            row[39]= result[result.Count - 1][4].ToString();
                            row[40]= result[result.Count - 1][5].ToString();
                            row[41]= result[result.Count - 1][6].ToString();
                            row[42]= result[result.Count - 1][7].ToString();
                            row[43]= result[result.Count - 1][8].ToString();
                            row[44]= result[result.Count - 1][9].ToString();
                            row[45]= result[result.Count - 1][10].ToString();
                            row[46]= result[result.Count - 1][11].ToString();
                            row[47]= result[result.Count - 1][12].ToString();
                            row[48]= result[result.Count - 1][13].ToString();
                            row[49]= result[result.Count - 1][14].ToString();
                            row[50]= result[result.Count - 1][15].ToString();
                            row[51]= result[result.Count - 1][16].ToString();
                            
                        }
                        else
                        {
                            row[39] = "";
                            row[40] = "";
                            row[41] = "";
                            row[42] = "";
                            row[43] = "";
                            row[44] = "";
                            row[45] = "";
                            row[46] = "";
                            row[47] = "";
                            row[48] = "";
                            row[49] = "";
                            row[50] = "";
                            row[51] = "";
                            
                            
                        }
                    }

                    else if (row[0].ToString() == "30-351")
                    {
                        var result = dynamicDBOLD.AsEnumerable().Where(dr1 => dr1.Field<string>("BARCODE") == row[3].ToString()).ToList();
                        //var result = dynamicDB.AsEnumerable().ToList();

                        if (result.Count >= 1)
                        {
                            row[39] = result[result.Count - 1][4].ToString();
                            row[40] = result[result.Count - 1][5].ToString();
                            row[41] = result[result.Count - 1][6].ToString();
                            row[42] = result[result.Count - 1][7].ToString();
                            row[43] = result[result.Count - 1][8].ToString();
                            row[44] = result[result.Count - 1][9].ToString();
                            row[45] = result[result.Count - 1][10].ToString();
                            row[46] = result[result.Count - 1][11].ToString();
                            row[47] = result[result.Count - 1][12].ToString();
                            row[48] = result[result.Count - 1][13].ToString();
                            row[49] = result[result.Count - 1][14].ToString();
                            row[50] = result[result.Count - 1][15].ToString();
                            row[51] = result[result.Count - 1][16].ToString();

                        }
                        else
                        {
                            row[39] = "";
                            row[40] = "";
                            row[41] = "";
                            row[42] = "";
                            row[43] = "";
                            row[44] = "";
                            row[45] = "";
                            row[46] = "";
                            row[47] = "";
                            row[48] = "";
                            row[49] = "";
                            row[50] = "";
                            row[51] = "";


                        }
                    }
                }


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
                DataTable VIdt = new DataTable();
                DataTable paintingweightdt = new DataTable();

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
                    //  fillSizedropdownlist();




                    List<string> conditions = new List<string>();
                    string query = "";




                    if (GradeDropDownList.SelectedItem.Text != "All")
                        conditions.Add("uniformityGrade='" + GradeDropDownList.SelectedItem.Text + "'");
                    if (MachineDropDownList.SelectedItem.Text != "All")
                        conditions.Add("MachineName='" + MachineDropDownList.SelectedItem.Text + "'");
                    if (ddlRecipe.SelectedItem.Text != "All")
                        conditions.Add("TireType='" + ddlRecipe.SelectedItem.Text + "'");
                    if (conditions.Any())
                        query = " AND " + string.Join(" AND ", conditions.ToArray());

                    //get barcode weight 
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //Select distinct gtbarcode ,Weight  from (select gtbarcode, Weight,  row_number() over (partition by Weight order by dtandTime desc) as rono FROM PCRBuddeScannedTyreDetail where gtbarcode IN (" + wherequery + ") ) as t where rono = 1
                        myConnection.comm.CommandText = "Select gtbarcode ,Weight  from (select gtbarcode, Weight,  row_number() over (partition by gtbarcode order by dtandTime desc) as rono FROM PCRBuddeScannedTyreDetail where (dtandTime>'" + Convert.ToDateTime(fromdate).AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') and gtbarcode not in ('??????????','') ) as t where rono = 1";// "Select gtbarcode, Weight  from (select gtbarcode, Weight,  row_number() over (partition by Weight order by dtandTime desc) as rono FROM PCRBuddeScannedTyreDetail where (dtandTime>'" + Convert.ToDateTime(fromdate).AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')  and gtbarcode not in ('??????????','')) as t where rono = 1 ";

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        weightdt.Load(myConnection.reader);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                        //
                        //myConnection.comm.CommandText = "Select MachineName ,TestTime, TyreType ,BARCODE ,Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13  from (select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,uniformityGrade AS Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13,  row_number() over(partition by BARCODE order by testtime desc) as rono FROM productionDataTUO where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')" + query + " )as t where rono = 1";
                         myConnection.comm.CommandText = "Select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,uniformityGrade AS Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13 FROM productionDataTUO where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " order by machineName asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        dtTUO.Load(myConnection.reader);

                        if (dtTUO.Rows.Count == 0)
                        {
                            myConnection.comm.CommandText = "Select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,uniformityGrade AS Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13 FROM ProductionDataTUO_3de2020 where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') " + query + " order by machineName asc";
                            myConnection.reader = myConnection.comm.ExecuteReader();
                            myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                            dtTUO.Load(myConnection.reader);
                        }
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


                    //get old dynimic balancing data 
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        // myConnection.comm.CommandText = "Select MachineName ,TestTime, TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge,GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO,GradeUpperLRO,RRO, GradeRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,GradeDef from (select wcname as MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge,GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,GradeDef,  row_number() over (partition by BARCODE order by testtime desc) as rono FROM vproductionDataPCRDBNew where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') ) as t where rono = 1 ";
                        myConnection.comm.CommandText = "Select MachineName,dtandTime as TestTime, tireType AS TyreType ,BARCODE ,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,circumference as GradeDef FROM vproductionDataPCRDB where (dtandTime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                 
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dynamicDBOLD.Load(myConnection.reader);
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


                    //get dynimic balancing data 
                    try
                    {
                        //myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                       // myConnection.comm.CommandText = "Select MachineName ,TestTime, TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge,GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO,GradeUpperLRO,RRO, GradeRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,GradeDef from (select wcname as MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge,GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,GradeDef,  row_number() over (partition by BARCODE order by testtime desc) as rono FROM vproductionDataPCRDBNew where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') ) as t where rono = 1 ";
                        myConnection.comm.CommandText = "Select wcname as MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,GradeDef FROM vproductionDataPCRDBNew where (testtime>'" + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd HH:mm:ss") + "' AND testtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') order by wcname asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dynamicDB.Load(myConnection.reader);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmPCR where (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
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
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name,RIGHT(pressbarcode,8) as cavityNo,case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as mouldNo, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringpcr where (dtandTime>'" + Convert.ToDateTime(fromdate).AddDays(-5).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandTime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')";
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
                    // paintaing  detail addde on 6/2/2019 by sarita to add painting weight 
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        // Select distinct(barcode) as paintingbarCode, WeightScale from (select Select distinct(barcode) as paintingbarCode, WeightScale,  row_number() over (partition by WeightScale order by dtandTime desc) as rono ) FROM paintingDataPCR where gtbarcode IN (" + wherequery + ") ) as t where rono = 1 ";
                        myConnection.comm.CommandText = "Select  paintingbarCode, WeightScale  from (select barcode as paintingbarCode, WeightScale,  row_number() over (partition by barcode order by DateInserted desc) as rono FROM paintingDataPCR where (DateInserted>'" + Convert.ToDateTime(fromdate).AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss") + "' AND DateInserted<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "') ) as t where rono = 1"; 
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                        paintingweightdt.Load(myConnection.reader);
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
                    //VI status  Detail addded on 12/2/2019 by sarita to add final status
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select gtbarcode as VIbarCode, StatusName from (select gtbarcode, StatusName,  row_number() over (partition by gtbarcode order by dtandTime desc) as rono FROM vVisualInspectionPCR where (dtandtime>'" + Convert.ToDateTime(fromdate).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "' AND dtandtime<'" + Convert.ToDateTime(todate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "')) as t where rono = 1";
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        VIdt.Load(myConnection.reader);
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

                }
                else if (BarcodeFromTOWiseReport.Checked)
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select distinct gtbarcode ,Weight  from (select gtbarcode, Weight,  row_number() over (partition by gtbarcode order by dtandTime desc) as rono FROM PCRBuddeScannedTyreDetail where gtbarcode IN (" + wherequery + ") ) as t where rono = 1";
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
                       // myConnection.comm.CommandText = "Select MachineName ,TestTime, TyreType ,BARCODE ,Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13  from (select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,uniformityGrade AS Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13,  row_number() over(partition by BARCODE order by testtime desc) as rono FROM productionDataTUO where barcode IN (" + wherequery + "))as t where rono = 1";
                        myConnection.comm.CommandText = "Select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,uniformityGrade AS Uniformity_Grade, RFVCW, GradeRFVCW,RFVCCW, GradeRFVCCW,H1RFVCW, GradeH1RFVCW,H1RFVCCW, GradeH1RFVCCW,H2RFVCW, GradeH2RFVCW,H2RFVCCW, GradeH2RFVCCW,LFVCW, GradeLFVCW, LFVCCW,GradeLFVCCW,CONICITY, GradeCONICITY,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, UpperRRO, GradeUpperRRO,LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,spare1,spare2,spare3,spare4,spare5,spare6,spare7,spare8,spare9,spare10,spare11,spare12,spare13 FROM productionDataTUO  where barcode IN (" + wherequery + ") order by machineName asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dtTUO.Load(myConnection.reader);
                        myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                    catch (Exception exc)
                    { }
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
                       // myConnection.comm.CommandText = "Select MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,gradeDef, StatusName from (select wcname as MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,gradeDef,  row_number() over (partition by BARCODE order by TestTime desc) as rono FROM vproductionDataPCRDBNew where  barcode IN (" + wherequery + ")) as t where rono = 1";
                        myConnection.comm.CommandText = "Select wcname as MachineName ,TestTime, tireType AS TyreType ,BARCODE ,LowerBulge, GradeLowerBulge,UpperBulge, GradeUpperBulge,LowerLRO, GradeLowerLRO,UpperLRO, GradeUpperLRO,RRO, GradeRRO, LowerDepression, gradelowerdepression,UpperDepression, GradeUpperDepression,static, staticAngle,staticGrade,couple,coupleAngle,coupleGrade,upper,upperAngle,uppergrade,lower,lowerAngle,lowergrade,gradeDef FROM vproductionDataPCRDBNew where  barcode IN (" + wherequery + ") order by wcname asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dynamicDB.Load(myConnection.reader);

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
                        myConnection.comm.CommandText = "Select gtbarCode AS tbmgtbarCode, wcName AS TBM_MachineName, Builder_Name = firstName + LastName, dtandTime AS TBM_dtandTime FROM vTbmPCR where gtbarCode IN (" + wherequery + ")";
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
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name,RIGHT(pressbarcode,8) as cavityNo,case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as mouldNo, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringpcr where gtbarCode IN (" + wherequery + ")";
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
                    //paintaing  detail addde on 6/2/2019 by sarita to add painting weight 
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select  paintingbarCode, WeightScale  from (select barcode as paintingbarCode, WeightScale,  row_number() over (partition by barcode order by DateInserted desc) as rono FROM paintingDataPCR where barcode IN (" + wherequery + ")  order by barcode";
                        //myConnection.comm.CommandText = "Select distinct(barcode) as paintingbarCode, WeightScale FROM paintingDataPCR where barcode IN (" + wherequery + ")  order by barcode";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        paintingweightdt.Load(myConnection.reader);
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
                    //VI status  Detail addded on 12/2/2019 by sarita to add final status
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = "Select VIbarCode, StatusName from (select gtbarcode as VIbarCode, StatusName,  row_number() over (partition by Weight order by dtandTime desc) as rono ) FROM vVisualInspectionPCR where gtbarcode IN (" + wherequery + ") ) as t where rono = 1 ";
                        myConnection.reader = myConnection.comm.ExecuteReader();

                        VIdt.Load(myConnection.reader);
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
                          join r0w5 in paintingweightdt.AsEnumerable()
                           on r0w1.Field<string>("barcode") equals r0w5.Field<string>("paintingbarCode") into s
                          from r0w5 in s.DefaultIfEmpty()
                          ////to add VI status
                          join r0w6 in VIdt.AsEnumerable()
                           on r0w1.Field<string>("barcode") equals r0w6.Field<string>("VIbarCode") into VI
                          from r0w6 in VI.DefaultIfEmpty()

                          //select r0w1.ItemArray.Concat(row4 != null ? row4.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w5 != null ? r0w5.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w6 != null ? r0w6.ItemArray.Skip(1) : new object[] { "" }).ToArray();
                          select r0w1.ItemArray.Concat(row4 != null ? row4.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "" }).Concat(r0w5 != null ? r0w5.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w6 != null ? r0w6.ItemArray.Skip(1) : new object[] { "" }).ToArray();

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
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.Charset = "";
                Response.AddHeader("content-disposition", "attachment; filename=TUOBarCodeWise.xls");
                Response.ContentType = "application/vnd.xls";

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                MainGridView.RenderControl(htmlWriter);
                Response.Write(stringWriter.ToString());
                Response.Flush();
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
