using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using OfficeOpenXml;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace SmartMIS.Report
{
    public partial class PCRInspectionsummaryReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
       // string moduleName = "PCRVIAdmin";
        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign=null;
        public int totalcheckedcount = 0, okcount = 0, NotOkCount = 0, reworkcount = 0, scrapcount = 0, majorokcount = 0, minorbuffcount = 0, majorholdcount = 0, minorCount = 0, majorCount = 0, majorscrapcount = 0, totalokcount = 0, totalbuffcount = 0, totalscrapcount = 0, totalholdcount = 0, totalminorcount = 0,  min_count = 0;
        string sqlquery = "";
        int status;                                                                      
        DateTime fromDate, toDate;                                                        
        DataTable wc_name_dt = new DataTable();
        DataTable dt = new DataTable();
               
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "TBRVisualInspectionReport.xlsx";
        string filepath; 


        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
            //{
                if (!IsPostBack)
                {
                    //fillDesigndropdownlist();
                    fillSizedropdownlist();


                    if (Session["userID"].ToString().Trim() == "")
                    {
                        Response.Redirect("/SmartMIS/Default.aspx", true);
                    }
                    else
                    {
                        reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    }
                }
            //}
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {
            //if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
            //{

                string getMonth = DropDownListMonth.SelectedValue;
                string getYear = DropDownListYear.SelectedItem.Text;
                string getyearwise = DropDownList2.SelectedItem.Text;
                string recipe = ddlRecipe.SelectedItem.Text;
                string tyredesign = "";
                string duration = "";
                var datetimebt = "";
                string getfromdate = reportMasterFromDateTextBox.Text;

                switch (DropDownListDuration.SelectedItem.Value)
                {
                    case "Date":
                        fromDate = DateTime.Parse(formatDate(getfromdate));
                        toDate = fromDate.AddDays(1);

                        string nfromDate = fromDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                        string ntoDate = toDate.ToString("dd/MMM/yyyy") + " 07:00:00";
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);
                        break;

                    case "Month":
                        nfromDate = getYear.ToString() + "-" + getMonth + "-01 07:00:00";
                        if (Convert.ToInt32(getMonth) < 12)
                        {
                            datetimebt = getYear.ToString() + "-" + (Convert.ToInt32(getMonth) + 1) + "-01 07:00:00";
                        }
                        else
                        { datetimebt = getYear.ToString() + "-" + (getMonth) + "-31 07:00:00"; }

                        ntoDate = datetimebt;
                        showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign);

                        break;
                    
                    case "DateFrom":

                        nfromDate = formatDate(tuoReportMasterFromDateTextBox.Text);

                        toDate = DateTime.Parse(formatDate(tuoReportMasterToDateTextBox.Text));
                        ntoDate = toDate.AddDays(1).ToString();
                        TimeSpan ts = DateTime.Parse(ntoDate) - DateTime.Parse(nfromDate);
                        int result = (int)ts.TotalDays;
                        if ((int)ts.TotalDays > 7)
                        {
                            ShowWarning.Visible = true;
                            ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more than 7 days!!!</font></strong></td></tr></table>";
                        }

                        else { showReportDateMonthWise(nfromDate, ntoDate, recipe, tyredesign); ; }
                        break;

                }
            //}
            //else
            //{

            //    lbltext.Text = "You Are not Authorized to See this report";
            //    lbltext.Visible = true;


            //}
        }
        public string formatDate(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    flag = day + "-" + month + "-" + year + " " + "07" + ":" + "00" + ":" + "00";

                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }

       

        protected void showReportDateMonthWise(string nfromDate, string ntoDate, string recipe, string tyredesign)
        {
            try
            {
            DataTable maindt = new DataTable();
            string barcodeInQuery = "("; string barcodeInQuery1 = "(";
                maindt.Columns.Add("WCID", typeof(int));
                maindt.Columns.Add("gtbarcode", typeof(string));
                maindt.Columns.Add("status", typeof(int));
                maindt.Columns.Add("mannigID", typeof(int));
                maindt.Columns.Add("defectstatus", typeof(string));
                maindt.Columns.Add("dtandtime", typeof(string));
                maindt.Columns.Add("REcipeID", typeof(int));
                maindt.Columns.Add("wid", typeof(int));
                maindt.Columns.Add("MachineName", typeof(string));
                maindt.Columns.Add("VIStage", typeof(int));
                maindt.Columns.Add("CuringRecipeId", typeof(int));
                maindt.Columns.Add("RecipeName", typeof(string));
            
               // maindt.Columns.Add("tyredesign", typeof(string));
                //maindt.Columns.Add("defect_id", typeof(string));
                maindt.Columns.Add("defectAreaID", typeof(string));
                maindt.Columns.Add("defectName", typeof(string));
               
                maindt.Columns.Add("firstname", typeof(string));
                maindt.Columns.Add("lastname", typeof(string));

               
               

                DataTable minor_gdt = new DataTable();
                DataTable dtTUO = new DataTable();
                DataTable rdt = new DataTable();
                DataTable wcdt = new DataTable();
                DataTable mdt = new DataTable();

                DataTable gridviewdt = new DataTable();
                DataTable majordt = new DataTable();
                DataRow dr;
                gridviewdt.Columns.Add("Tyre Size", typeof(string));
                //gridviewdt.Columns.Add("Design", typeof(string));
                gridviewdt.Columns.Add("Inspected", typeof(string));
                gridviewdt.Columns.Add("OK", typeof(string));
                gridviewdt.Columns.Add("Minor", typeof(string));
                gridviewdt.Columns.Add("Major", typeof(string));
                gridviewdt.Columns.Add("Minor Ok", typeof(string));
                gridviewdt.Columns.Add("Minor Buff", typeof(string));
                gridviewdt.Columns.Add("Minor Scrap", typeof(string));
                gridviewdt.Columns.Add("Major Ok", typeof(string));
                gridviewdt.Columns.Add("Hold", typeof(string));
                gridviewdt.Columns.Add("Major Scrap", typeof(string));
                gridviewdt.Columns.Add("Total Ok", typeof(string));
                gridviewdt.Columns.Add("Total Buff", typeof(string));
                gridviewdt.Columns.Add("Total Scrap", typeof(string));
                gridviewdt.Columns.Add("Total Hold", typeof(string));

                //rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select ID AS wcMasterID, name AS wcName,Vistage FROM wcMaster where vistage in(1,2,3) and ProcessId=9";
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


                //Get Recipe details
                try
                {
                   
                   
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select ID AS curingRecipeID, description as RecipeName,tyreDesign FROM recipeMaster  where   processID = 8   ";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    rdt.Load(myConnection.reader);
                }
                catch (Exception exc)
                {
                    myWebService.writeLogs(exc.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
              
                
                //Get PCRInspection details
                try
                {
                   
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (ddlRecipe.SelectedValue != "All")
                    {
                        myConnection.comm.CommandText = "select wcid,gtbarcode,status,manningId, CASE status WHEN 1 THEN 'Ok' WHEN 2 THEN 'Minor' WHEN 3 THEN 'Major' WHEN 21 THEN 'Buff' WHEN 22 THEN 'Scrap' WHEN 31 THEN 'majorOK' WHEN 32 THEN 'Hold' WHEN 33 THEN 'major_Scrap' END AS StatusName,dtandTime ,curingRecipeID from (select wcid,gtbarcode,status,defectstatusid,dtandTime,curingRecipeID,manningId,  row_number() over (partition by gtbarcode order by dtandtime desc) as rono from vInspectionPCR where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcid in ('82','83','84','85','86','87','88','89','107','109','267') and (curingRecipeID='" + ddlRecipe.SelectedValue + "')) as t where rono = 1 ";
                    }
                    else
                    {
                        myConnection.comm.CommandText = "select wcid,gtbarcode,status, manningId,CASE status WHEN 1 THEN 'Ok' WHEN 2 THEN 'Minor' WHEN 3 THEN 'Major' WHEN 21 THEN 'Buff' WHEN 23 THEN 'Scrap' WHEN 31 THEN 'majorOk' WHEN 32 THEN 'Hold' WHEN 33 THEN 'major_Scrap'  END AS StatusName,dtandTime ,curingRecipeID from (select wcid,gtbarcode,status,defectstatusid,dtandTime,curingRecipeID,manningId,  row_number() over (partition by gtbarcode order by dtandtime desc) as rono from vInspectionPCR where dtandTime > '" + nfromDate + "' and dtandtime<'" + ntoDate + "' and wcid in ('82','83','84','85','86','87','88','89','107','109','267')) as t where rono = 1 ";
                    }
                   
                
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dtTUO.Load(myConnection.reader);


                    
                }
                catch (Exception ex)
                { myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
                //Get manning Details

                try
                {


                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select ID AS manningID,firstname,lastname FROM manningMaster ";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    mdt.Load(myConnection.reader);
                }
                catch (Exception exc)
                {
                    myWebService.writeLogs(exc.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
                finally
                {
                    if (!myConnection.reader.IsClosed)
                        myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

                DataTable dt_defect = new DataTable();
               //join to get VIstage=(1,2,3 data
                var row = from r0w1 in dtTUO.AsEnumerable()
                          join r0w2 in wcdt.AsEnumerable()
                            on r0w1.Field<int>("wcID") equals r0w2.Field<int>("wcMasterID")
                          join r0w4 in mdt.AsEnumerable()
                           on r0w1.Field<int?>("manningId") equals r0w4.Field<int?>("manningID") into p
                          from r0w4 in p.DefaultIfEmpty()
                          join r0w3 in rdt.AsEnumerable()
                            on r0w1.Field<int?>("curingRecipeID") equals r0w3.Field<int?>("curingRecipeID") into ps
                          from r0w3 in ps.DefaultIfEmpty()

                          select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray : new object[] { 0, 0,0}).Concat(r0w4 != null ? r0w4.ItemArray : new object[] {  })).ToArray();


                             
                foreach (object[] values in row)
                    maindt.Rows.Add(values);

                //Get vistage=2 details
                DataTable dtExcel = maindt.Copy();



                string[] existingcol = { "REcipeID", "wid", "WCID", "status", "defectAreaID", "defectName", "curingRecipeID" };
                    foreach (string colName in existingcol)           
                    {                                                
                        dtExcel.Columns.Remove(colName);
                    }
               
                dtExcel.AcceptChanges();

                
                var wc_query = (maindt.AsEnumerable().GroupBy(l => l.Field<int>("curingRecipeID"))
                          .Select(g => new
                          {
                              wc_id = g.Key,

                          }));
                var items = wc_query.ToArray();
                foreach (var item in items)
                // for (int i = 0; i < distinctvalues.Count; i++)
                {

                    dr = gridviewdt.NewRow();

                    var data = maindt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.wc_id).Select(l => new
                                {
                                    ReceipeName = l.Field<string>("RecipeName"),
                                    CuringRecipeId = l.Field<int>("curingRecipeID"),
                                    //tyreDesign = l.Field<string>("tyreDesign"),
                                    vistage = l.Field<int>("vistage"),
                                    status = l.Field<int>("status"),
                                }).ToArray();
                    

                    if (data.Count() != 0 )
                    {
                        totalcheckedcount += data.Count();

                        okcount += data.Count(d => d.status == 1 && d.vistage == 1);
                        NotOkCount +=    data.Count(d => d.status != 1);
                        minorCount +=    data.Count(d => d.status == 2);//minorcount
                        majorCount +=    data.Count(d => d.status == 3);//majorcount
                       // totalminorcount += aa.Count();
                        min_count += data.Count(d => d.status == 1 && d.vistage == 2);//ok count
                        minorbuffcount +=  data.Count(d => d.status == 21);
                        scrapcount +=      data.Count(d => d.status == 23);//minorcount
                        majorokcount +=    data.Count(d => d.status == 31);//ok count
                        majorholdcount +=  data.Count(d => d.status == 32);//holdcount
                        majorscrapcount += data.Count(d => d.status == 33);//minorcount
                      }

                    
                    dr[0] = data[0].ReceipeName;
                    //dr[2] = data[0].tyreDesign;
                    dr[1] = data.Count();
                    dr[2] = data.Count(d => d.status == 1 && d.vistage == 1);
                    dr[3] = data.Count(d => d.status == 2);
                    dr[4] = data.Count(d => d.status == 3);
                    dr[5] = data.Count(d => d.status == 1 && d.vistage == 2);
                    dr[6] = data.Count(d => d.status == 21);
                    dr[7] = data.Count(d => d.status == 23);
                    dr[8] = data.Count(d => d.status == 31);
                    dr[9] =data.Count(d => d.status == 32);
                    dr[10] =data.Count(d => d.status == 33);
                    dr[11] =data.Count(d => d.status == 31) + data.Count(d => d.status == 1&& d.vistage == 2) + data.Count(d => d.status == 1 && d.vistage == 1);
                    dr[12] =data.Count(d => d.status == 21);
                    dr[13] =data.Count(d => d.status == 33) + data.Count(d => d.status == 23);
                    dr[14] = data.Count(d => d.status == 32);
                   
                    
                        gridviewdt.Rows.Add(dr);
                        //sorting data inspected wise in  desc
                        
                 
                   }
               
               // gridviewdt.DefaultView.Sort = "Inspected";
                //gridviewdt = gridviewdt.DefaultView.ToTable();
                DataTable dtMarks1 = gridviewdt.Clone();
                dtMarks1.Columns["Inspected"].DataType = Type.GetType("System.Int32");

                foreach (DataRow d1r in gridviewdt.Rows)
                {
                    dtMarks1.ImportRow(d1r);
                }
                dtMarks1.AcceptChanges();

                DataView dv = dtMarks1.DefaultView;
                dv.Sort = "Inspected DESC";
                DataTable dtsorted = dv.ToTable();

                DataRow drt = dtsorted.NewRow();
                drt[0] = "Total";
               
                drt[1] = totalcheckedcount;
                drt[2] = okcount;
                drt[3] = minorCount;
                drt[4] = majorCount;
                drt[5] = min_count;
                drt[6] = minorbuffcount;
                drt[7] = scrapcount;
                drt[8] = majorokcount;
                drt[9] = majorholdcount;
                drt[10] = majorscrapcount;
                drt[11] = majorokcount + min_count + okcount;
                drt[12] = minorbuffcount;
                drt[13] = majorscrapcount + scrapcount;
                drt[14] = majorholdcount;
                dtsorted.Rows.Add(drt);

               //for excel data 
               

                //DataView dv = dtMarks1.DefaultView;
                //dv.Sort = "Inspected DESC";
                //DataTable dtsorted = dv.ToTable();
                

                ViewState["dt"] = dtExcel;
                if (dtsorted.Rows.Count > 0)

                {



                    grdinspectionsummary.DataSource = dtsorted;
                    grdinspectionsummary.DataBind();
                    grdinspectionsummary.Visible = true;
                    //GridView1.Visible = false;
                    lbltext.Visible = false;
                }
                else
                {
                    lbltext.Text = "No Records Found";
                    lbltext.CssClass = "LabelTextAlignStyle";
                    lbltext.Visible = true;
                }
                if (dtExcel.Rows.Count > 0)
                {
                    GridView1.DataSource = dtExcel;
                    GridView1.DataBind();
                }

                }
            catch (Exception ex)
            {
                myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            
        }
        //private void fillDesigndropdownlist()
        //{
        //    DataTable t_dt = new DataTable();

        //    myConnection.open(ConnectionOption.SQL);
        //    myConnection.comm = myConnection.conn.CreateCommand();
        //    myConnection.comm.CommandText = "Select DISTINCT id as rID,tyreDesign from recipemaster where tyreDesign != '0' and description !='' and tyreSize!='' and processID = 8";
        //    myConnection.reader = myConnection.comm.ExecuteReader();
        //    t_dt.Load(myConnection.reader);

        //    pcrDDldesign.DataSource = t_dt;
        //    pcrDDldesign.DataTextField = "tyreDesign";
        //    pcrDDldesign.DataValueField = "rID";

        //    pcrDDldesign.DataBind();
        //    pcrDDldesign.Items.Insert(0, new ListItem("All", "All"));
        //    //pcrDDldesign.DataSource = null;
        //    //pcrDDldesign.DataSource = FillDropDownList("recipemaster", "tyreDesign");
        //    //pcrDDldesign.DataBind();
        //}
        private void fillSizedropdownlist()
        {

            DataTable d_dt = new DataTable();
           
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "Select DISTINCT id as rID,description from recipemaster where description != '0' and description !='' and tyreSize!=''and processID = 8";
            myConnection.reader = myConnection.comm.ExecuteReader();
            d_dt.Load(myConnection.reader);
            
            ddlRecipe.DataSource = d_dt;
            ddlRecipe.DataTextField = "description";
            ddlRecipe.DataValueField = "rID";
            ddlRecipe.DataBind();
            ddlRecipe.Items.Insert(0, new ListItem("All", "All"));
            //ddlRecipe.DataSource = null;
            //ddlRecipe.DataSource = FillDropDownList("recipemaster", "description");
            //ddlRecipe.DataBind();
        }
       
        //public ArrayList FillDropDownList(string tableName, string coloumnName)
        //{
        //    ArrayList flag = new ArrayList();
        //    string sqlQuery = "";

        //    flag.Add("All");
        //    try
        //    {
        //        myConnection.open(ConnectionOption.SQL);
        //        myConnection.comm = myConnection.conn.CreateCommand();

        //        sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " where "+coloumnName+" != '0' and tyreSize!=''";

        //        myConnection.comm.CommandText = sqlQuery;

        //        myConnection.reader = myConnection.comm.ExecuteReader();
        //        while (myConnection.reader.Read())
        //        {
        //            if (myConnection.reader[0].ToString() != "")
        //                flag.Add(myConnection.reader[0].ToString());
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //    }

        //    myConnection.reader.Close();
        //    myConnection.comm.Dispose();
        //    myConnection.close(ConnectionOption.SQL);


        //    return flag;
        //}
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PCRVInspectionSummaryReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PCRInspectionSummaryReport");
            ws.Cells["A1"].Value = "PCR Visual Inspection Report ";

            using (ExcelRange r = ws.Cells["A1:AG1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }


            ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();


            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();
          
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            
                        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                       TableHeaderCell cell = new TableHeaderCell();

                  
                            cell.Text = "";
                            cell.ColumnSpan = 5;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.Text = "Minor";
                            cell.ColumnSpan = 3;
                            row.Controls.Add(cell);

                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 3;
                            cell.Text = "Major";

                            row.Controls.Add(cell);
                            cell = new TableHeaderCell();
                            cell.ColumnSpan = 4;
                            cell.Text = "Total";
                            row.Controls.Add(cell);
                            

                            grdinspectionsummary.HeaderRow.Parent.Controls.AddAt(0, row);

                 
            }

        

        protected void ddlRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {


            
        }
    }
}
