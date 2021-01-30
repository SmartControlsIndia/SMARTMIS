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
    public partial class PCRinspectionDefectwise : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
                #endregion
        #region globle variable
        public int totalcheckedcount = 0, okcount = 0, datatotal = 0, data_total = 0, carcass_total = 0, bead_total = 0, tread_Total = 0, s_total = 0, o_total = 0;
        int status;
        DataTable exldt; DataTable mainGVdt;
        DataTable dtInspectoon = new DataTable();
        DataTable gridviewdt = new DataTable();
        DataTable tbldt = new DataTable();
        string nfromDate;
        string ntoDate;
        string wcIDInQuery = "(";
        ArrayList wcID = new ArrayList(); 
        #endregion
        string getType;

        string wherequery = "";
        string duration = "";
        
        string getfromdate;
        string gettodate;
        DateTime fromDate;
        DateTime toDate;
       
        string durationQuery = "";
        //string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "TBRVisualInspectionReport.xlsx";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

               
                ShowWarning.Visible = false;
                Label2.Visible = false;

                if (string.IsNullOrEmpty(tuoReportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
                {
                    tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy") ;
                    tuoReportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                   string rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                   string rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                   fillSizedropdownlist();

                }
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " where " + coloumnName + " != '0' and tyreSize!='' and ProcessId=8";

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
            ddlRecipe.DataSource = FillDropDownList("recipemaster", "description");
            ddlRecipe.DataBind();
        }

        

        
        private void showReportWcWise(string fromDate, string toDate, string type)
        {
            try
            {
                var per = 0;
                // int totalcount = 0;
                DataRow drt;
                gridviewdt.Clear();
                MainGridView.DataSource = null;
                MainGridView.DataBind();
                string rToDate = "";
               
                    //rToDate = formatDate();
                    durationQuery = "dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 07:00:00'";

                    TimeSpan ts = DateTime.Parse(toDate) - DateTime.Parse(fromDate);
                    int result = (int)ts.TotalDays;
                    if ((int)ts.TotalDays > 31)
                    {
                        ShowWarning.Visible = true;
                        ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>You cannot select more  than 1 month!!!</font></strong></td></tr></table>";
                    }
                gridviewdt.Columns.Add("WcName", typeof(string));
                gridviewdt.Columns.Add("SizeName", typeof(string));
                gridviewdt.Columns.Add("No.Inspected", typeof(string));
                gridviewdt.Columns.Add("DefectCount", typeof(string));
               // gridviewdt.Columns.Add("Percentage", typeof(string));

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (ddlRecipe.SelectedItem.Text == "All")
                {
                    myConnection.comm.CommandText = "select curingWCID,wcname, gtbarcode,status,defectId,defectname,dtandtime,CuringRecipeName,description from vInspectionPCRDefect VPCR where  " + durationQuery + " and curingRecipeName!='null'  and defectname!='null'  and defectName!='' ORDER BY wcname asc";
                }
                else { myConnection.comm.CommandText = "select curingWCID,wcname, gtbarcode,status,defectId,defectname,dtandtime,CuringRecipeName ,description from vInspectionPCRDefect VPCR where  " + durationQuery + " and curingRecipeName!='null'  and defectname!='null'  and defectName!='' and description='" + ddlRecipe.SelectedItem.Text + "'   ORDER BY wcname asc"; }
               
                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                DataRow dr = gridviewdt.NewRow();
                
               //for distinct defctname based on dfecet arae
                if (ddlFault.SelectedItem.Text != "Select")
                {
                    int category = 0; int category1 = 0; int category2 = 0;
                    if (ddlFault.SelectedItem.Text == "Buff" && ddlCategory.SelectedItem.Text == "Minor")
                    { 
                        category = 21;
                        category1 = 2;
                    }
                    else  if (ddlFault.SelectedItem.Text == "Buff" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 32;//null
                        category1 = 2;
                    }
                    else if (ddlFault.SelectedItem.Text == "Scrap" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 23;//null
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "Scrap" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 33;
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "OK" && ddlCategory.SelectedItem.Text=="Minor")
                    {
                        category = 2;
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "OK" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 31;
                        category1 = 3;
                    }
                    var distinctdefectvalues = tbldt.AsEnumerable().Where(x => x.Field<int>("status") == category  )
                        .GroupBy(row => new
                        {

                            defectName = row.Field<string>("defectName"),


                        })
                        .Select(g => new
                        {
                            defectName = g.Key.defectName,

                            quantity = g.Count()

                        }).Distinct().OrderBy(x => x.defectName).ToList();


                    var items = distinctdefectvalues.ToArray();

                    for (int j = 0; j < distinctdefectvalues.Count; j++)
                    {

                        gridviewdt.Columns.Add((distinctdefectvalues[j].defectName), typeof(int));


                    }

                    // gridviewdt.Rows.Add(dr);


                    var distinctvalues = tbldt.AsEnumerable()
                         .Select(s => new
                         {
                             wcName = s.Field<string>("wcName"),


                         })
                         .Distinct().ToList();


                    for (int i = 0; i < distinctvalues.Count; i++)
                    {
                        var distinctRecipe = tbldt.AsEnumerable().Where(r => r.Field<string>("wcName") == distinctvalues[i].wcName)
                           .Select(s => new
                           {

                               RecipeName = s.Field<string>("description"),
                           })
                           .Distinct().ToList();
                        for (int h = 0; h < distinctRecipe.Count; h++)
                        {
                            //distinct defectName
                            dr = gridviewdt.NewRow();
                            dr[0] = distinctvalues[i].wcName;
                            dr[1] = distinctRecipe[h].RecipeName;

                            var total = tbldt.AsEnumerable().Where(s => s.Field<string>("wcName") == distinctvalues[i].wcName &&
                                s.Field<string>("description") == distinctRecipe[h].RecipeName)
                                .Select(k => new
                                {
                                    status = k.Field<int>("status")
                                }).ToList();
                            dr[2] = total.Count();

                            //for buff data is showing
                            if (ddlCategory.SelectedValue == "1")
                            {
                                dr[3] = total.Count(s => s.status == category);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == category);
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "2")
                            {

                                dr[3] = total.Count(s => s.status == 21);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == 21);
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 21)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "3")
                            {
                                dr[3] = total.Count(s => s.status == category);

                                data_total += total.Count(s => s.status == category);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 3)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                //dr[4] = total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                //dr[3] =Convert.ToDecimal( (total.Count(s => s.status == 2 || s.status == 3 || s.status == 4)* 100)/total.Count());
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                // data_total += total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                               s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                           .GroupBy(row => new
                           {

                               shortDefectName = row.Field<string>("defectName"),

                           })
                  .Select(g => new
                  {
                      shortDefectName = g.Key.shortDefectName,

                      quantity = g.Count()

                  }).Distinct().OrderBy(x => x.shortDefectName).ToList();

                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }

                            }

                            //MainGridView.PageIndex = +1;
                            gridviewdt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    int category = 0; int category1 = 0; int category2 = 0;
                    if (ddlFault.SelectedItem.Text == "Select" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 2;
                        category1 = 2;
                    }
                    else if (ddlFault.SelectedItem.Text == "Select" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 3;//null
                        category1 = 2;
                    }
                    
                    var distinctdefectvalues = tbldt.AsEnumerable().Where(x => x.Field<int>("status") == category)
               .GroupBy(row => new
               {

                   defectName = row.Field<string>("defectName"),


               })
               .Select(g => new
               {
                   defectName = g.Key.defectName,

                   quantity = g.Count()

               }).Distinct().OrderBy(x => x.defectName).ToList();


                    var items = distinctdefectvalues.ToArray();

                    for (int j = 0; j < distinctdefectvalues.Count; j++)
                    {

                        gridviewdt.Columns.Add((distinctdefectvalues[j].defectName), typeof(int));


                    }

                    // gridviewdt.Rows.Add(dr);


                    var distinctvalues = tbldt.AsEnumerable()
                         .Select(s => new
                         {
                             wcName = s.Field<string>("wcName"),


                         })
                         .Distinct().ToList();


                    for (int i = 0; i < distinctvalues.Count; i++)
                    {
                        var distinctRecipe = tbldt.AsEnumerable().Where(r => r.Field<string>("wcName") == distinctvalues[i].wcName)
                           .Select(s => new
                           {

                               RecipeName = s.Field<string>("description"),
                           })
                           .Distinct().ToList();
                        for (int h = 0; h < distinctRecipe.Count; h++)
                        {
                            //distinct defectName
                            dr = gridviewdt.NewRow();
                            dr[0] = distinctvalues[i].wcName;
                            dr[1] = distinctRecipe[h].RecipeName;

                            var total = tbldt.AsEnumerable().Where(s => s.Field<string>("wcName") == distinctvalues[i].wcName &&
                                s.Field<string>("description") == distinctRecipe[h].RecipeName)
                                .Select(k => new
                                {
                                    status = k.Field<int>("status")
                                }).ToList();
                            dr[2] = total.Count();

                            //for buff data is showing
                            if (ddlCategory.SelectedValue == "1")
                            {
                                dr[3] = total.Count(s => s.status == category);

                                data_total += total.Count(s => s.status == category);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "2")
                            {
                                dr[3] = total.Count(s => s.status == 21);

                                data_total += total.Count(s => s.status == 21);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 21)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "3")
                            {
                                dr[3] = total.Count(s => s.status == category);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 3)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == category);

                                //dr[4] = total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                //dr[3] =Convert.ToDecimal( (total.Count(s => s.status == 2 || s.status == 3 || s.status == 4)* 100)/total.Count());
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                // data_total += total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                               s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 31)
                           .GroupBy(row => new
                           {

                               shortDefectName = row.Field<string>("defectName"),

                           })
                  .Select(g => new
                  {
                      shortDefectName = g.Key.shortDefectName,

                      quantity = g.Count()

                  }).Distinct().OrderBy(x => x.shortDefectName).ToList();

                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }

                            }

                            //MainGridView.PageIndex = +1;
                            gridviewdt.Rows.Add(dr);
                        }
                    }
                }
                //MainGridView.Columns.Clear();
                dr = gridviewdt.NewRow();
                dr[0] = "GrandTotal";
                dr[1] = "";
                dr[2] = totalcheckedcount;
               // string per1 = (Convert.ToDecimal(data_total) * 100 / Convert.ToDecimal(totalcheckedcount)).ToString("N2");
               
                dr[3] = data_total;
                //dr[4] = per1;
                int sum = 0;
                for (int v = 4; v < gridviewdt.Columns.Count; v++)
                {
                    decimal d = 0;
                    string aa = gridviewdt.AsEnumerable()
                     .Where(r => decimal.TryParse(r.Field<int?>(gridviewdt.Columns[v].ColumnName.ToString()).ToString(), out d))
                     .Sum(r => d).ToString();
                    dr[v] = aa;
                }
                gridviewdt.Rows.Add(dr);
                MainGridView.Columns.Clear();

                foreach (DataColumn col in gridviewdt.Columns)
                {
                    //Declare the bound field and allocate memory for the bound field.
                    BoundField bfield = new BoundField();
                    //Initalize the DataField value.
                    bfield.DataField = col.ColumnName;

                    bfield.HeaderText = col.ColumnName;

                    //Add the newly created bound field to the GridView.
                    MainGridView.Columns.Add(bfield);
                }
                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();
                //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                //MainGridView.Controls[0].Controls.AddAt(0, HeaderGridRow);
                //MainGridView.HeaderRow.CssClass = "VertiColumn";
                //MainGridView.HeaderRow.Height = 100;
                //MainGridView.HeaderRow.Width = 50;
                //gridviewdt.Clear();

                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "GrandTotal");

                foreach (var row in rows)
                    row.Font.Bold = true;

              
                exldt = gridviewdt.Clone();
                //exldt = tbldt.Copy();
                exldt.Columns[0].DataType = typeof(string);
                exldt.Columns[1].DataType = typeof(string);
                exldt.Columns[2].DataType = typeof(string);
                exldt.Columns[3].DataType = typeof(string);
                // exldt.Columns[4].DataType = typeof(string);

                exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);




                ViewState["dt"] = exldt;


                //int x1 = Request.Browser.ScreenPixelsWidth;
                //MainGridView.Width = x1;

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
        protected void BarcodeFromReport_CheckedChanged(object sender, EventArgs e)
        {

            if (BarcodeFromTOReport.Checked)
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
        
        private void showReportRecipeWise(string fromDate, string toDate, string type)
        {
            try
            {
                var per = 0;
                // int totalcount = 0;
                DataRow drt;
                gridviewdt.Clear();
                MainGridView.DataSource = null;
                MainGridView.DataBind();
                durationQuery = "dtandtime>='" + fromDate + " 07:00:00' and dtandtime<'" + toDate + " 07:00:00'";

               
                gridviewdt.Columns.Add("SizeName", typeof(string));
                gridviewdt.Columns.Add("No.Inspected", typeof(string));
                gridviewdt.Columns.Add("DefectCount", typeof(string));
                //gridviewdt.Columns.Add("Percentage", typeof(string));


                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (ddlRecipe.SelectedItem.Text == "All")
                {
                    myConnection.comm.CommandText = "select curingWCID,wcname, gtbarcode,status,defectId,defectname,dtandtime,CuringRecipeName,description from vInspectionPCRDefect VPCR where  " + durationQuery + " and curingRecipeName!='null'  and defectname!='null'  and defectName!='' ORDER BY wcname asc";
                }
                else {
                    myConnection.comm.CommandText = "select curingWCID,wcname, gtbarcode,status,defectId,defectname,dtandtime,CuringRecipeName ,description from vInspectionPCRDefect VPCR where  " + durationQuery + " and curingRecipeName!='null'  and defectname!='null'  and defectName!='' and description='" + ddlRecipe.SelectedItem.Text + "'   ORDER BY wcname asc";
                }

                myConnection.reader = myConnection.comm.ExecuteReader();
                tbldt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                DataRow dr = gridviewdt.NewRow();

                //for distinct defctname based on dfecet arae
                if (ddlFault.SelectedItem.Text != "Select")
                {
                    int category = 0; int category1 = 0; int category2 = 0;
                    if (ddlFault.SelectedItem.Text == "Buff" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 21;
                        category1 = 2;
                    }
                    else if (ddlFault.SelectedItem.Text == "Buff" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 32;//null
                        category1 = 2;
                    }
                    else if (ddlFault.SelectedItem.Text == "Scrap" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 23;//null
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "Scrap" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 33;
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "OK" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 2;
                        category1 = 3;
                    }
                    else if (ddlFault.SelectedItem.Text == "OK" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 31;
                        category1 = 3;
                    }
                    var distinctdefectvalues = tbldt.AsEnumerable().Where(x => x.Field<int>("status") == category)
                        .GroupBy(row => new
                        {

                            defectName = row.Field<string>("defectName"),


                        })
                        .Select(g => new
                        {
                            defectName = g.Key.defectName,

                            quantity = g.Count()

                        }).Distinct().OrderBy(x => x.defectName).ToList();


                    var items = distinctdefectvalues.ToArray();

                    for (int j = 0; j < distinctdefectvalues.Count; j++)
                    {

                        gridviewdt.Columns.Add((distinctdefectvalues[j].defectName), typeof(int));


                    }

                    // gridviewdt.Rows.Add(dr);


                    var distinctvalues = tbldt.AsEnumerable()
                         .Select(s => new
                         {
                             wcName = s.Field<string>("wcName"),


                         })
                         .Distinct().ToList();


                    for (int i = 0; i < distinctvalues.Count; i++)
                    {
                        var distinctRecipe = tbldt.AsEnumerable().Where(r => r.Field<string>("wcName") == distinctvalues[i].wcName)
                           .Select(s => new
                           {

                               RecipeName = s.Field<string>("description"),
                           })
                           .Distinct().ToList();
                        for (int h = 0; h < distinctRecipe.Count; h++)
                        {
                            //distinct defectName
                            dr = gridviewdt.NewRow();
                            //dr[0] = distinctvalues[i].wcName;
                            dr[0] = distinctRecipe[h].RecipeName;

                            var total = tbldt.AsEnumerable().Where(s => s.Field<string>("wcName") == distinctvalues[i].wcName &&
                                s.Field<string>("description") == distinctRecipe[h].RecipeName)
                                .Select(k => new
                                {
                                    status = k.Field<int>("status")
                                }).ToList();
                            dr[1] = total.Count();

                            //for buff data is showing
                            if (ddlCategory.SelectedValue == "1")
                            {
                                dr[2] = total.Count(s => s.status == category);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == category);
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "2")
                            {
                                dr[2] = total.Count(s => s.status == 21);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == 21);
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 21)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "3")
                            {
                                dr[2] = total.Count(s => s.status == category);

                                data_total += total.Count(s => s.status == category);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 3)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                //dr[4] = total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                //dr[3] =Convert.ToDecimal( (total.Count(s => s.status == 2 || s.status == 3 || s.status == 4)* 100)/total.Count());
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                // data_total += total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                               s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                           .GroupBy(row => new
                           {

                               shortDefectName = row.Field<string>("defectName"),

                           })
                  .Select(g => new
                  {
                      shortDefectName = g.Key.shortDefectName,

                      quantity = g.Count()

                  }).Distinct().OrderBy(x => x.shortDefectName).ToList();

                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }

                            }

                            //MainGridView.PageIndex = +1;
                            gridviewdt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    int category = 0; int category1 = 0; int category2 = 0;
                    if (ddlFault.SelectedItem.Text == "Select" && ddlCategory.SelectedItem.Text == "Minor")
                    {
                        category = 2;
                        category1 = 2;
                    }
                    else if (ddlFault.SelectedItem.Text == "Select" && ddlCategory.SelectedItem.Text == "Major")
                    {
                        category = 3;//null
                        category1 = 2;
                    }

                    var distinctdefectvalues = tbldt.AsEnumerable().Where(x => x.Field<int>("status") == category)
               .GroupBy(row => new
               {

                   defectName = row.Field<string>("defectName"),


               })
               .Select(g => new
               {
                   defectName = g.Key.defectName,

                   quantity = g.Count()

               }).Distinct().OrderBy(x => x.defectName).ToList();


                    var items = distinctdefectvalues.ToArray();

                    for (int j = 0; j < distinctdefectvalues.Count; j++)
                    {

                        gridviewdt.Columns.Add((distinctdefectvalues[j].defectName), typeof(int));


                    }

                    // gridviewdt.Rows.Add(dr);


                    var distinctvalues = tbldt.AsEnumerable()
                         .Select(s => new
                         {
                             wcName = s.Field<string>("wcName"),


                         })
                         .Distinct().ToList();


                    for (int i = 0; i < distinctvalues.Count; i++)
                    {
                        var distinctRecipe = tbldt.AsEnumerable().Where(r => r.Field<string>("wcName") == distinctvalues[i].wcName)
                           .Select(s => new
                           {

                               RecipeName = s.Field<string>("description"),
                           })
                           .Distinct().ToList();
                        for (int h = 0; h < distinctRecipe.Count; h++)
                        {
                            //distinct defectName
                            dr = gridviewdt.NewRow();
                            //dr[0] = distinctvalues[i].wcName;
                            dr[0] = distinctRecipe[h].RecipeName;

                            var total = tbldt.AsEnumerable().Where(s => s.Field<string>("wcName") == distinctvalues[i].wcName &&
                                s.Field<string>("description") == distinctRecipe[h].RecipeName)
                                .Select(k => new
                                {
                                    status = k.Field<int>("status")
                                }).ToList();
                            dr[1] = total.Count();
                            if (ddlCategory.SelectedValue == "1")
                            {
                                dr[2] = total.Count(s => s.status == category);

                                data_total += total.Count(s => s.status == category);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == category)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            //for buff data is showing
                            else if (ddlCategory.SelectedValue == "2")
                            {
                                dr[2] = total.Count(s => s.status == 21);

                                data_total += total.Count(s => s.status == 21);
                                //dr[4] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 2)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                                  s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 21)
                              .GroupBy(row => new
                              {

                                  shortDefectName = row.Field<string>("defectName"),

                              })
                     .Select(g => new
                     {
                         shortDefectName = g.Key.shortDefectName,
                         quantity = g.Count()

                     }).Distinct().OrderBy(x => x.shortDefectName).ToList();
                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }
                            }
                            else if (ddlCategory.SelectedValue == "3")
                            {
                                dr[2] = total.Count(s => s.status == category);
                                //dr[3] = Convert.ToDecimal((Convert.ToDecimal(total.Count(s => s.status == 3)) * 100) / Convert.ToDecimal(total.Count())).ToString("N2");
                                data_total += total.Count(s => s.status == category);

                                //dr[4] = total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                //dr[3] =Convert.ToDecimal( (total.Count(s => s.status == 2 || s.status == 3 || s.status == 4)* 100)/total.Count());
                                totalcheckedcount += total.Count();
                                //for (int v = 5; v < gridviewdt.Columns.Count; v++)
                                //{
                                //    dr[v] = 0;
                                //}
                                // data_total += total.Count(s => s.status == 2 || s.status == 3 || s.status == 4);
                                var distinctdefect = tbldt.AsEnumerable().Where(s => s.Field<string>("wcname") == distinctvalues[i].wcName &&
                               s.Field<string>("description") == distinctRecipe[h].RecipeName && s.Field<int>("status") == 31)
                           .GroupBy(row => new
                           {

                               shortDefectName = row.Field<string>("defectName"),

                           })
                  .Select(g => new
                  {
                      shortDefectName = g.Key.shortDefectName,

                      quantity = g.Count()

                  }).Distinct().OrderBy(x => x.shortDefectName).ToList();

                                for (int d = 0; d < distinctdefect.Count; d++)
                                {
                                    //dr[d] = 0;
                                    dr[distinctdefect[d].shortDefectName] = distinctdefect[d].quantity;
                                }

                            }

                            //MainGridView.PageIndex = +1;
                            gridviewdt.Rows.Add(dr);
                        }
                    }
                }
                //MainGridView.Columns.Clear();
                dr = gridviewdt.NewRow();


                dr[0] = "GrandTotal";
               
                dr[1] = totalcheckedcount;
               // string per1 = (Convert.ToDecimal(data_total) * 100 / Convert.ToDecimal(totalcheckedcount)).ToString("N2");
                
                dr[2] = data_total;
                //dr[3] = per1;

                int sum = 0;
                for (int v = 3; v < gridviewdt.Columns.Count; v++)
                {
                    decimal d = 0;
                    string aa = gridviewdt.AsEnumerable()
                     .Where(r => decimal.TryParse(r.Field<int?>(gridviewdt.Columns[v].ColumnName.ToString()).ToString(), out d))
                     .Sum(r => d).ToString();
                    dr[v] = aa;
                }
                gridviewdt.Rows.Add(dr);
                MainGridView.Columns.Clear();

                foreach (DataColumn col in gridviewdt.Columns)
                {
                    //Declare the bound field and allocate memory for the bound field.
                    BoundField bfield = new BoundField();
                    //Initalize the DataField value.
                    bfield.DataField = col.ColumnName;

                    bfield.HeaderText = col.ColumnName;

                    //Add the newly created bound field to the GridView.
                    MainGridView.Columns.Add(bfield);
                }
                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();


               // MainGridView.HeaderRow.CssClass = "VertiColumn";
                //MainGridView.HeaderRow.Height = 100;
                //gridviewdt.Clear();

                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>().Where(row => row.Cells[0].Text == "GrandTotal");

                foreach (var row in rows)
                    row.Font.Bold = true;


                exldt = gridviewdt.Clone();
                //exldt = tbldt.Copy();
                exldt.Columns[0].DataType = typeof(string);
                exldt.Columns[1].DataType = typeof(string);
               
                // exldt.Columns[4].DataType = typeof(string);

                exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);




                ViewState["dt"] = exldt;


                //int x1 = Request.Browser.ScreenPixelsWidth;
                //MainGridView.Width = x1;

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            string hgridvalue = "";

            Button clickedbutton = sender as Button;


            getfromdate = tuoReportMasterFromDateTextBox.Text;
            gettodate = tuoReportMasterToDateTextBox.Text;
            getType = TypeDropDownList.Text;
            fromDate = DateTime.Parse(formatDate(getfromdate));
            toDate = DateTime.Parse(formatDate(gettodate)).AddDays(1);

            nfromDate = fromDate.ToString("dd/MMM/yyyy");
            ntoDate = toDate.ToString("dd/MMM/yyyy");

            if (clickedbutton.ID == "ViewButton")
            {
                if (getType == "WcWise" && ddlCategory.SelectedItem.Text != "Select")
                {
                    showReportWcWise(nfromDate, ntoDate, getType);
                }
                else if (getType == "Select" && ddlCategory.SelectedItem.Text == "Select")
                {
                    Label2.Text = "Please Select all Fields";
                    Label2.Visible = true;
                }
                else if (getType == "Select" && ddlCategory.SelectedItem.Text != "Select")
                {
                    Label2.Text = "Please Select all Fields";
                    Label2.Visible = true;
                }
                //else if (getType == "Curing" && ddlCategory.SelectedItem.Text!="Select")
                //    {
                //        showReportCuringWise(nfromDate, ntoDate, getType);
                //    }
                else if (getType == "RecipeWise" && ddlCategory.SelectedItem.Text != "Select")
                {
                    showReportRecipeWise(nfromDate, ntoDate, getType);
                }
            }
            if (clickedbutton.ID == "BarcodeWiseButton")
            {
            mainGVdt = new DataTable();
            string[] mainGVdtColumns = new string[] { "MachineName", "Defect", "Barcode", "TBM_MachineName", "Builder_Name", "TBM_dtandTime", "Press_Name", "Curing_Operator_Name", "Curing_dtandTime" };
            for (int i = 0; i < mainGVdtColumns.Count(); i++)
                mainGVdt.Columns.Add(mainGVdtColumns[i]);

            createGridView(mainGVdt, MainGridView);
            
                long frombarcode =Convert.ToInt64(BarcodeFromTextBox.Text);
                int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                if (frombarcode.ToString().Length == 8)
                    wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray());
                else if (frombarcode.ToString().Length == 7)
                    wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());
                else
                    wherequery = "'" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'" + (frombarcode + i) + "'").ToArray());

                loadData();

                DataRow dr = mainGVdt.NewRow();
                mainGVdt.Rows.Add(dr);


                if (!DateFromToReport.Checked)
                {
                    mainGVdt.DefaultView.Sort = "barcode ASC";

                    DataView dw = mainGVdt.DefaultView;
                    DataTable sorteddt = dw.ToTable();

                    MainGridView.DataSource = mainGVdt;
                    MainGridView.DataBind();
                }
                
            }
            
            //lbldetail.Text = " &nbsp;&nbsp;  From " + nfromDate + " &nbsp; To" + ntoDate + "&nbsp; <b>Type:</b> " + TypeDropDownList.SelectedItem.Text + "&nbsp; <b>Category:</b> " + ddlCategory.SelectedItem.Text;
        
        
        }
        private void loadData()
        {
            try
            {
                DataTable dtInspection = new DataTable();
                DataTable curdt = new DataTable();
                DataTable tbmdt = new DataTable();
                DataTable manndt = new DataTable();
                DataTable wcdt = new DataTable();







                if (BarcodeFromTOReport.Checked)
                {

                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        //myConnection.comm.CommandText = "Select name as MachineName ,dtandtime, recipecode AS TyreType,BARCODE,TotalRank , RfvCW,Rfv1HCW,Rfv1HoCW,Rfv1HCCW,  RfvCCw,Lfv1HCW,LfvCW,Lfv1HCCW,LfvCCW, Con,ConCalc, Ply FROM vTBRUniformityData t1 where  t1.dtandTime = (SELECT MAX(t2.dtandTime)FROM vTBRUniformityData t2 WHERE t2.barcode = t1.barcode and barcode IN (" + wherequery + ")) and barcode IN (" + wherequery + "))order by name asc";
                        myConnection.comm.CommandText = "Select wcName as MachineName ,defectName, gtbarcode FROM vInspectionPCRDefect where  gtbarcode IN (" + wherequery + ") order by wcName asc";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dtInspection.Load(myConnection.reader);
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
                        myConnection.comm.CommandText = "Select gtbarCode AS curgtbarCode, wcName AS Press_Name, Curing_Operator_Name = firstName + LastName, dtandTime AS Curing_dtandTime FROM vCuringPCr where gtbarCode IN (" + wherequery + ")";
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



                    var row = from r0w1 in dtInspection.AsEnumerable()
                              join row2 in tbmdt.AsEnumerable()
                             on r0w1.Field<string>("gtbarcode") equals row2.Field<string>("tbmgtbarCode") into p
                              from r0w2 in p.DefaultIfEmpty()
                              join r0w3 in curdt.AsEnumerable()
                                on r0w1.Field<string>("gtbarcode") equals r0w3.Field<string>("curgtbarCode") into ps
                              from r0w3 in ps.DefaultIfEmpty()

                              select r0w1.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "" }).Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" }).ToArray();

                    foreach (object[] values in row)
                        mainGVdt.Rows.Add(values);
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = (DataTable)ViewState["dt"];
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=PCRInspectionSummaryReport.xls");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PCRInspectionSummaryReport");
                ws.Cells["A1"].Value = "PCR InspectionSummaryDefect Report ";

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
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message + exp.Source, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
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
    }
}
