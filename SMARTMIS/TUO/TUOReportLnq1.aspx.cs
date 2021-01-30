using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using OfficeOpenXml;

namespace SmartMIS.TUO
{
    public partial class TUOReportLnq1 : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        int total_, A_, B_, C_, D_, E_;
        
        Double pA_, pB_, pC_, pD_, pE_;
        public Double grandtotal, grandA, grandB, grandC, grandD, grandE;
        string tablename;

        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcIDQuery, machinenamequery;
        string dtnadtime1 = "", machineStatus = "", wCenterName = "default";
        string query = "", getdisplaytype="";
        string[] tempString2;
        int rowCount = 4, pid = -1;
        DataTable gridviewdt = new DataTable();
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        DataTable dt = new DataTable();
        
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "performanceReportTUOmachineWise.xlsx";
        string filepath; 

        #endregion       
        public TUOReportLnq1()
        {
            filepath = myWebService.getExcelPath();

            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                showDownload.Text = "";
                if (!IsPostBack)
                {
                    if (Session["userID"].ToString().Trim() == "")
                    {
                        Response.Redirect("/SmartMIS/Default.aspx", true);
                    }
                    else
                    {

                        fillSizedropdownlist();
                        fillDesigndropdownlist();
            
                        reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        reportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");

                        if (QualityReportTUOWise.Checked)
                        {                            
                            QualityReportTUOWisePanel.Visible = true;
                            QualityReportRecipeTUOWisePanel.Visible = false;
                        }
                        else
                        {
                            QualityReportTUOWisePanel.Visible = false;
                            QualityReportRecipeTUOWisePanel.Visible = true;


                        }

                        //Compare the hidden field if it contains the query string or not
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }      
        protected void Button_Click(object sender, EventArgs e)
        {
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());
            try
            {

                if (((Button)sender).ID == "ErankViewDetailButton")
                {
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string wcname = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseWCNameLabel")).Text);
                    string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(wcname, recipeCode);

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);
                }
                else
                {

                }
                if (((Button)sender).ID == "ErankRecipeWiseViewDetailButton")
                {
                    GridViewRow gridviewrow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                    string recipeCode = (((Label)gridviewrow.Cells[1].FindControl("performanceReportRecipeTUOWiseTyreTypeLabel")).Text);
                    fillBarCodeDetailGridView(recipeCode);

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }

                if (((Button)sender).ID == "AllErankDetailButton")
                {
                    fillBarCodeDetailGridView();

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }


                if (((Button)sender).ID == "AllRecipeErankDetailButton")
                {
                    fillBarCodeDetailGridView();

                    ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                }
                else
                {

                }
                /*
                if (QualityReportTUOWise.Checked)
                {                    
                    QualityReportTUOWisePanel.Visible = true;
                    QualityReportRecipeTUOWisePanel.Visible = false;
                    showData();
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    QualityReportTUOWisePanel.Visible = false;
                    QualityReportRecipeTUOWisePanel.Visible = true;
                    showData();
                }*/
                Label1.Text = "false";
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void loadWCData(string dtnadtime1)
        {
            try
            {
                DataTable recipe_dt = new DataTable();
                DataTable tuo_dt = new DataTable();
                dt.Columns.Add("machinename", typeof(string));
                dt.Columns.Add("tireType", typeof(string));
                dt.Columns.Add("uniformityGrade", typeof(string));

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name, tyreSize, tyreDesign from recipeMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                recipe_dt.Load(myConnection.reader);

                myConnection.comm.CommandText = "select machinename, tireType, uniformityGrade from productiondatatuo WHERE ((testTime>" + dtnadtime1 + " ORDER BY machinename ASC";
                myConnection.reader = myConnection.comm.ExecuteReader();
                tuo_dt.Load(myConnection.reader);


                if (tuo_dt.Rows.Count == 0)
                {
                    myConnection.comm.CommandText = "select machinename, tireType, uniformityGrade from ProductionDataTUO_3de2020 WHERE ((testTime>" + dtnadtime1 + " ORDER BY machinename ASC";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    tuo_dt.Load(myConnection.reader);             
                }
                        

                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    dt = tuo_dt.Copy(); ;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                          join r0w2 in recipe_dt.AsEnumerable()
                            on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text && r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values); 
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
        }
        private void showQualityReportWise()
        {
            gridviewdt.Clear();
            gridviewdt.Columns.Add("machineName", typeof(string));
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("A", typeof(string));
            gridviewdt.Columns.Add("B", typeof(string));
            gridviewdt.Columns.Add("C", typeof(string));
            gridviewdt.Columns.Add("D", typeof(string));
            gridviewdt.Columns.Add("E", typeof(string));

            DataRow drt;
            int total = 0, A = 0, B = 0, C = 0, D = 0, E = 0;
            Double ptotal = 0, pA = 0, pB = 0, pC = 0, pD = 0, pE = 0;
            Double dtotal = 0, dA = 0, dB = 0, dC = 0, dD = 0, dE = 0;
            
                        
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            var wc_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("machinename"))
                    .Select(g => new
                    {
                        wc_id = g.Key
                    }));
            var wc_items = wc_query.ToArray();
            switch (getdisplaytype)
            {
                case "Numbers":
                    foreach (var wc_item in wc_items)
                    {
                        ptotal = 0; pA = 0; pB = 0; pC = 0; pD = 0; pE = 0;
                        var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("tireType"))
                            .Select(g => new
                            {
                                tyreType_id = g.Key
                            }));
                        var tyreType_items = tyreType_query.ToArray();
                        foreach (var tyreType_item in tyreType_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("machinename") == wc_item.wc_id && l.Field<string>("tireType") == tyreType_item.tyreType_id).Select(l => new
                            {
                                wcname = l.Field<string>("machinename"),
                                tyreType = l.Field<string>("tireType"),
                                uniformitygrade = l.Field<string>("uniformitygrade")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                total = data.Count();
                                A = data.Count(d => d.uniformitygrade == "A");
                                B = data.Count(d => d.uniformitygrade == "B");
                                C = data.Count(d => d.uniformitygrade == "C");
                                D = data.Count(d => d.uniformitygrade == "D");
                                E = data.Count(d => d.uniformitygrade == "E");

                                total_ += total;
                                A_ += A;
                                B_ += B;
                                C_ += C;
                                D_ += D;
                                E_ += E;

                                ptotal += total;
                                pA += A;
                                pB += B;
                                pC += C;
                                pD += D;
                                pE += E;
                                
                                dr[0] = data[0].wcname;
                                dr[1] = data[0].tyreType;
                                dr[2] = total;
                                dr[3] = A;
                                dr[4] = B;
                                dr[5] = C;
                                dr[6] = D;
                                dr[7] = E;
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = wc_item.wc_id;
                        drt[1] = "Total";
                        drt[2] = ptotal;
                        drt[3] = pA;
                        drt[4] = pB;
                        drt[5] = pC;
                        drt[6] = pD;
                        drt[7] = pE;
                        gridviewdt.Rows.Add(drt);
                    }
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[2] = total_;
                    drt[3] = A_;
                    drt[4] = B_;
                    drt[5] = C_;
                    drt[6] = D_;
                    drt[7] = E_;
                    gridviewdt.Rows.Add(drt);
                    break;
                case "Percent":
                    foreach (var wc_item in wc_items)
                    {
                        ptotal = 0; pA = 0; pB = 0; pC = 0; pD = 0; pE = 0;
                        var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("tireType"))
                            .Select(g => new
                            {
                                tyreType_id = g.Key
                            }));
                        var tyreType_items = tyreType_query.ToArray();
                        foreach (var tyreType_item in tyreType_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("machinename") == wc_item.wc_id && l.Field<string>("tireType") == tyreType_item.tyreType_id).Select(l => new
                            {
                                wcname = l.Field<string>("machinename"),
                                tyreType = l.Field<string>("tireType"),
                                uniformitygrade = l.Field<string>("uniformitygrade")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                total = data.Count();
                                dA = data.Count(d => d.uniformitygrade == "A");
                                dB = data.Count(d => d.uniformitygrade == "B");
                                dC = data.Count(d => d.uniformitygrade == "C");
                                dD = data.Count(d => d.uniformitygrade == "D");
                                dE = data.Count(d => d.uniformitygrade == "E");

                                total_ += total;
                                pA_ += dA;
                                pB_ += dB;
                                pC_ += dC;
                                pD_ += dD;
                                pE_ += dE;

                                ptotal += total;
                                pA += dA;
                                pB += dB;
                                pC += dC;
                                pD += dD;
                                pE += dE;

                                dr[0] = data[0].wcname;
                                dr[1] = data[0].tyreType;
                                dr[2] = data.Count();
                                dr[3] = (total != 0) ? Math.Round((dA * 100 / total), 1) + "%" : null;
                                dr[4] = (total != 0) ? Math.Round((dB * 100 / total), 1) + "%" : null;
                                dr[5] = (total != 0) ? Math.Round((dC * 100 / total), 1) + "%" : null;
                                dr[6] = (total != 0) ? Math.Round((dD * 100 / total), 1) + "%" : null;
                                dr[7] = (total != 0) ? Math.Round((dE * 100 / total), 1) + "%" : null;
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = wc_item.wc_id;
                        drt[1] = "Total";
                        drt[2] = ptotal;
                        drt[3] = (ptotal != 0) ? Math.Round((pA * 100 / ptotal), 1) + "%" : null;
                        drt[4] = (ptotal != 0) ? Math.Round((pB * 100 / ptotal), 1) + "%" : null;
                        drt[5] = (ptotal != 0) ? Math.Round((pC * 100 / ptotal), 1) + "%" : null;
                        drt[6] = (ptotal != 0) ? Math.Round((pD * 100 / ptotal), 1) + "%" : null;
                        drt[7] = (ptotal != 0) ? Math.Round((pE * 100 / ptotal), 1) + "%" : null;
                        gridviewdt.Rows.Add(drt);
                    }
                    drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[2] = total_;
                    drt[3] = (total_ != 0) ? Math.Round((pA_ * 100 / total_), 1) + "%" : null;
                    drt[4] = (total_ != 0) ? Math.Round((pB_ * 100 / total_), 1) + "%" : null;
                    drt[5] = (total_ != 0) ? Math.Round((pC_ * 100 / total_), 1) + "%" : null;
                    drt[6] = (total_ != 0) ? Math.Round((pD_ * 100 / total_), 1) + "%" : null;
                    drt[7] = (total_ != 0) ? Math.Round((pE_ * 100 / total_), 1) + "%" : null;
                    gridviewdt.Rows.Add(drt);
                    break;
            }
            performanceReportTUOWiseMainGridView.DataSource = gridviewdt;
            performanceReportTUOWiseMainGridView.DataBind();
            ViewState["dt"] = gridviewdt;
            // Select the rows where showing total & make them bold
            IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
.Where(row => row.Cells[0].Text == "Total");

            foreach (var row in rows)
                row.Font.Bold = true;

        }        
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            showData();
        }
        private void showData()
        {
            getdisplaytype = optionDropDownList.SelectedItem.Text;

            rFromDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rToDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            if (!string.IsNullOrEmpty(rToDate) && !string.IsNullOrEmpty(rFromDate))
            {
                string dtnadtime = TotalprodataformatDate(rFromDate, rToDate);

                if (QualityReportTUOWise.Checked)
                {
                    loadWCData(dtnadtime);
                    showQualityReportWise();
                    QualityReportTUOWisePanel.Visible = true;
                    QualityReportRecipeTUOWisePanel.Visible = false;
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    loadRecipeData(dtnadtime);
                    showQualityRecipeReportWise();
                    QualityReportTUOWisePanel.Visible = false;
                    QualityReportRecipeTUOWisePanel.Visible = true;
                }
                Label1.Text = "false";
            }
        }
        private void loadRecipeData(string dtnadtime1)
        {
            try
            {
                DataTable recipe_dt = new DataTable();
                DataTable tuo_dt = new DataTable();

                dt.Columns.Add("tireType", typeof(string));
                dt.Columns.Add("uniformityGrade", typeof(string));
            
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name, tyreSize, tyreDesign from recipeMaster";
                myConnection.reader = myConnection.comm.ExecuteReader();
                recipe_dt.Load(myConnection.reader);

                myConnection.comm.CommandText = "select tireType, uniformityGrade from productiondatatuo WHERE ((testTime>" + dtnadtime1;
                myConnection.reader = myConnection.comm.ExecuteReader();
                tuo_dt.Load(myConnection.reader);


                if (tuo_dt.Rows.Count == 0)
                {
                    myConnection.comm.CommandText = "select tireType, uniformityGrade from ProductionDataTUO_3de2020 WHERE ((testTime>" + dtnadtime1;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    tuo_dt.Load(myConnection.reader);
                }
                        

                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    dt = tuo_dt.Copy(); ;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    var row = from r0w1 in tuo_dt.AsEnumerable()
                              join r0w2 in recipe_dt.AsEnumerable()
                                on r0w1.Field<string>("tireType") equals r0w2.Field<string>("name")
                              where r0w2.Field<string>("tyreSize") == performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text && r0w2.Field<string>("tyreDesign") == performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text
                              select r0w1.ItemArray.ToArray();
                    foreach (object[] values in row)
                        dt.Rows.Add(values);
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
        }
        private void showQualityRecipeReportWise()
        {
            gridviewdt.Clear(); 
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("A", typeof(string));
            gridviewdt.Columns.Add("B", typeof(string));
            gridviewdt.Columns.Add("C", typeof(string));
            gridviewdt.Columns.Add("D", typeof(string));
            gridviewdt.Columns.Add("E", typeof(string));

            DataRow drt;
            int total = 0, A = 0, B = 0, C = 0, D = 0, E = 0;
            Double pA, pB, pC, pD, pE;
            
            var tyreType_query = (dt.AsEnumerable().GroupBy(l => l.Field<string>("tireType"))
                    .Select(g => new
                    {
                        tyreType_id = g.Key
                    }));
                var tyreType_items = tyreType_query.ToArray();
                switch (getdisplaytype)
                {
                    case "Numbers":
                        foreach (var tyreType_item in tyreType_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("tireType") == tyreType_item.tyreType_id).Select(l => new
                            {
                                tyreType = l.Field<string>("tireType"),
                                uniformitygrade = l.Field<string>("uniformitygrade")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                total += data.Count();
                                A += data.Count(d => d.uniformitygrade == "A");
                                B += data.Count(d => d.uniformitygrade == "B");
                                C += data.Count(d => d.uniformitygrade == "C");
                                D += data.Count(d => d.uniformitygrade == "D");
                                E += data.Count(d => d.uniformitygrade == "E");

                                DataRow dr = gridviewdt.NewRow();
                                dr[0] = data[0].tyreType;
                                dr[1] = data.Count();
                                dr[2] = data.Count(d => d.uniformitygrade == "A");
                                dr[3] = data.Count(d => d.uniformitygrade == "B");
                                dr[4] = data.Count(d => d.uniformitygrade == "C");
                                dr[5] = data.Count(d => d.uniformitygrade == "D");
                                dr[6] = data.Count(d => d.uniformitygrade == "E");
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = "Total";
                        drt[1] = total;
                        drt[2] = A;
                        drt[3] = B;
                        drt[4] = C;
                        drt[5] = D;
                        drt[6] = E;
                        gridviewdt.Rows.Add(drt);
                        break;
                    case "Percent":
                        foreach (var tyreType_item in tyreType_items)
                        {
                            var data = dt.AsEnumerable().Where(l => l.Field<string>("tireType") == tyreType_item.tyreType_id).Select(l => new
                            {
                                tyreType = l.Field<string>("tireType"),
                                uniformitygrade = l.Field<string>("uniformitygrade")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                total = data.Count();
                                pA = data.Count(d => d.uniformitygrade == "A");
                                pB = data.Count(d => d.uniformitygrade == "B");
                                pC = data.Count(d => d.uniformitygrade == "C");
                                pD = data.Count(d => d.uniformitygrade == "D");
                                pE = data.Count(d => d.uniformitygrade == "E");

                                total_ += total;
                                pA_ += pA;
                                pB_ += pB;
                                pC_ += pC;
                                pD_ += pD;
                                pE_ += pE;

                                DataRow dr = gridviewdt.NewRow();
                                dr[0] = data[0].tyreType;
                                dr[1] = total;
                                dr[2] = (total != 0) ? Math.Round((pA * 100 / total), 1) + "%" : null;
                                dr[3] = (total != 0) ? Math.Round((pB * 100 / total), 1) + "%" : null;
                                dr[4] = (total != 0) ? Math.Round((pC * 100 / total), 1) + "%" : null;
                                dr[5] = (total != 0) ? Math.Round((pD * 100 / total), 1) + "%" : null;
                                dr[6] = (total != 0) ? Math.Round((pE * 100 / total), 1) + "%" : null;
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = "Total";
                        drt[1] = total_;
                        drt[2] = (total_ != 0) ? Math.Round((pA_ * 100 / total_), 1) + "%" : null;
                        drt[3] = (total_ != 0) ? Math.Round((pB_ * 100 / total_), 1) + "%" : null;
                        drt[4] = (total_ != 0) ? Math.Round((pC_ * 100 / total_), 1) + "%" : null;
                        drt[5] = (total_ != 0) ? Math.Round((pD_ * 100 / total_), 1) + "%" : null;
                        drt[6] = (total_ != 0) ? Math.Round((pE_ * 100 / total_), 1) + "%" : null;
                        gridviewdt.Rows.Add(drt);
                        break;
                }
            performanceReportRecipeTUOWiseGridView.DataSource = gridviewdt;
            performanceReportRecipeTUOWiseGridView.DataBind();

            ViewState["dt"] = gridviewdt; 
        }        
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {   
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "performanceReportTUOWiseMainGridView")
                {
                    Label cell;
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseCheckedLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseAGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseBGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseCGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseDGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUOWiseEGradeLabel"));
                    cell.Text = NullIf(cell.Text);

                    for (int rowIndex = performanceReportTUOWiseMainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = performanceReportTUOWiseMainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = performanceReportTUOWiseMainGridView.Rows[rowIndex + 1];
                        if (((Label)performanceReportTUOWiseMainGridView.Rows[rowIndex].FindControl("performanceReportTUOWiseWCNameLabel")).Text == ((Label)performanceReportTUOWiseMainGridView.Rows[rowIndex + 1].FindControl("performanceReportTUOWiseWCNameLabel")).Text)
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
                }
                if (((GridView)sender).ID == "performanceReportRecipeTUOWiseGridView")
                {
                    if (string.IsNullOrEmpty(((Label)e.Row.FindControl("performanceReportRecipeTUOWiseTyreTypeLabel")).Text))
                        e.Row.Visible = false;
                    
                    /*Label cell;
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseCheckedLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseAGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseBGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseCGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseDGradeLabel"));
                    cell.Text = NullIf(cell.Text);
                    cell = ((Label)e.Row.FindControl("performanceReportTUORecipeWiseEGradeLabel"));
                    cell.Text = NullIf(cell.Text);*/
                }
            }
        }
        private void fillBarCodeDetailGridView(string wcname, string recipecode)
        {
            List<string> conditions = new List<string>();
            string Query = ""; DataTable flagR = null;

            if (recipecode != "Total" && recipecode != "")
                conditions.Add("tireType='" + recipecode + "'");
            if (wcname != "Total")
                conditions.Add("machinename='" + wcname + "'");

            if (conditions.Any())
                Query = " AND " + string.Join(" AND ", conditions.ToArray());
            
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E' AND ((testtime>=" + TotalprodataformatDate(rToDate, rFromDate) + Query + "";
            string query1 = "select wcname, machinename,tireType,barCode from vproductiondataTUO_old where uniformitygrade='E' AND ((testtime>=" + TotalprodataformatDate(rToDate, rFromDate) + Query + "";

            flagR = myWebService.fillGridView(query, ConnectionOption.SQL);
            if (flagR.Rows.Count == 0)
            {
                flagR = myWebService.fillGridView(query1, ConnectionOption.SQL);
            }

            performanceReportBarcodeDetailGridView.DataSource = flagR;
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillBarCodeDetailGridView(string recipecode)
        {
            DataTable flagR = null;
            if (recipecode != "Total")
                recipecode = " and tireType='" + recipecode + "'";
            else
                recipecode = "";
            
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            string query = "select wcname, machinename,TireType,barCode from vproductiondataTUO where uniformitygrade='E' " + recipecode + " and ((testtime>'" + TotalprodataformatDate(rToDate, rFromDate);

            string query1 = "select wcname, machinename,TireType,barCode from vproductiondataTUO_old where uniformitygrade='E' " + recipecode + " and ((testtime>'" + TotalprodataformatDate(rToDate, rFromDate);

            flagR = myWebService.fillGridView(query, ConnectionOption.SQL);
            if (flagR.Rows.Count == 0)
            {
                flagR = myWebService.fillGridView(query1, ConnectionOption.SQL);       
            }
            performanceReportBarcodeDetailGridView.DataSource = flagR;
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillBarCodeDetailGridView()
        {
            DataTable flagR = null;
            string query = "select wcname,machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E'  and ((testtime>" + TotalprodataformatDate(rToDate, rFromDate);
            string query1 = "select wcname,machinename,tireType,barCode from vproductiondataTUO_old where uniformitygrade='E'  and ((testtime>" + TotalprodataformatDate(rToDate, rFromDate);


            flagR = myWebService.fillGridView(query, ConnectionOption.SQL);
            if (flagR.Rows.Count == 0)
            {
                flagR = myWebService.fillGridView(query1, ConnectionOption.SQL);
            }
            performanceReportBarcodeDetailGridView.DataSource = flagR;
            performanceReportBarcodeDetailGridView.DataBind();

        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 13 June 2013
            //Date Updated  : 13 June 2013
            //Revision No.  : 01
            try
            {

                performanceReportTUOWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportTUOWiseMainGridView.DataBind();
            }
            catch (Exception exp)
            {

            }
        }
        private void fillRecipeWiseGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 12 July 2013
            //Date Updated  : 12 July 2013
            //Revision No.  : 01
            try
            {

                performanceReportRecipeTUOWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportRecipeTUOWiseGridView.DataBind();
            }
            catch (Exception exp)
            {

            }
        }
        public string fillCuringWCName(Object barcode)
        {
            string flag = "None";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select wcname from vcuringpcr where gtbarcode = '" + barcode.ToString() + "'";


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = myConnection.reader[0].ToString();
                    else
                        flag = "NOne";
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private void fillChildGridView(GridView childgridview, string query)
        {
            DataTable dt = new DataTable();
            dt = myWebService.fillGridView(query, ConnectionOption.SQL);
            if (dt.Rows.Count == 0)
                machineStatus = "<center><B>Machine is not connected with MIS</B></center>";

            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        public string formattoDate(String date)
        {
            string flag = "";
            try
            {
                DateTime tempDate = Convert.ToDateTime(date);
                flag = tempDate.ToString("MM-dd-yyyy");
                flag = flag + " " + "07:00:00";
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }
        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });
            try
            {
                day = tempDate[1].ToString().Trim();
                month = tempDate[0].ToString().Trim();
                year = tempDate[2].ToString().Trim();
                // DateTime tempDate1 = Convert.ToDateTime(date);
                if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                {
                    flag = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }
                else
                {
                    flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }
        protected void QualityReportRecipeTUOWise_CheckedChanged(object sender, EventArgs e)
        {            
            fillSizedropdownlist();
            fillDesigndropdownlist();
            showData();
            QualityReportTUOWisePanel.Visible = false;
            QualityReportRecipeTUOWisePanel.Visible = true;
            showDownload.Text = "";
        }
        protected void QualityReportTUOWise_CheckedChanged(object sender, EventArgs e)
        {            
            performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

            performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
            QualityReportTUOWisePanel.Visible = true;
            QualityReportRecipeTUOWisePanel.Visible = false;
            showData();
        }
        public string TotalformatDate(string fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string fromday, frommonth, fromyear, today, tomonth, toyear;

            string[] tempfromDate = fromDate.Split(new char[] { '-' });
            string[] temptoDate = toDate.Split(new char[] { '-' });
            try
            {
                fromday = tempfromDate[1].ToString().Trim();
                frommonth = tempfromDate[0].ToString().Trim();
                fromyear = tempfromDate[2].ToString().Trim();
                today = temptoDate[1].ToString().Trim();
                tomonth = temptoDate[0].ToString().Trim();
                toyear = temptoDate[2].ToString().Trim();

                flag1 = frommonth + "-" + fromday + "-" + fromyear + " 07:00:00";
                flag2 = tomonth + "-" + today + "-" + toyear + " 06:59:59";
                /*if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                {
                    flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }
                else
                {
                    flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                    flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                }*/

                flag = "testTime>='" + flag1 + "' and " + "testTime<='" + flag2 + "'";

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }
        public string wcquery(string query)
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select name from wcmaster where " + query + " ";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        flag = "wcname = '" + myConnection.reader[0] + "'";

                    }

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public string createQuery(String wcID, String fromDate, String toDate, String fromDateColoum, String toDateColoum)
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
        public string wcquery()
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select distinct machinename from productiondatatuo";
                myConnection.comm.CommandTimeout = 0;
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        flag = flag + "or" + " " + "machinename = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        flag = "machinename = '" + myConnection.reader[0] + "'";

                    }

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private void fillSizedropdownlist()
        {

            performanceReportTUOWiseSizeDropdownlist.DataSource = null;
            performanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            performanceReportTUOWiseSizeDropdownlist.DataBind();
        }
        private void fillDesigndropdownlist()
        {

            performanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            performanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            performanceReportTUOWiseRecipeDropdownlist.DataBind();
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
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));
                showData();

            }
            else if (ddl.ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
                showData();
            }


        }
        private string TotalprodataformatDate(String fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string fday, fmonth, fyear;
            int tday, tmonth, tyear;

            string[] ftempDate = fromDate.Split(new char[] { '-' });
            string[] ttempDate = toDate.Split(new char[] { '-' });

            try
            {
                fday = ftempDate[1].ToString().Trim();
                fmonth = ftempDate[0].ToString().Trim();
                fyear = ftempDate[2].ToString().Trim();
                tday = Convert.ToInt32(ttempDate[1].ToString().Trim());
                tmonth = Convert.ToInt32(ttempDate[0].ToString().Trim());
                tyear = Convert.ToInt32(ttempDate[2].ToString().Trim());

                flag = "'" + fyear + "-" + fmonth + "-" + fday + " 06:59:59' AND testTime<='" + formatToDate(tday, tmonth, tyear) + "'))";

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            return flag;
        }
        protected string checkDigit(int digit)
        {
            string str = "";
            if (digit.ToString().Length == 1)
                str = "0" + digit;
            else
                str = digit.ToString();
            return str;
        }
        private string formatToDate(int day, int month, int year)
        {
            string showToDate = "";
            if (month == 12 && day == 31)
                showToDate = (year + 1) + "-01-01";
            else if (day == 31 && (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10))
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else if (day == 30 && (month == 4 || month == 6 || month == 9 || month == 11))
                showToDate = year.ToString() + "-" + (checkDigit(month + 1)) + "-01";
            else if (month == 2 && (year == 2016) && (day == 29))
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else if (month == 2 && (day == 28) && year != 2016)
                showToDate = year.ToString() + "-" + checkDigit((month + 1)) + "-01";
            else
                showToDate = year + "-" + checkDigit(month) + "-" + checkDigit((day + 1));
            return showToDate + " 06:59:59";
        }
        public ArrayList FillDropDownList(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " " + whereClause + "";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }              
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=TUOReport.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TUOReport");
            ws.Cells["A1"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Medium2);
            ws.Cells.AutoFitColumns();
            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();




            ////reportHeader._rDate = reportMasterFromDateTextBox.Text;

            //if (optionDropDownList.SelectedItem.Text == "Numbers")
            //    option = "1";
            //else
            //    option = "2";

            //rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            //rFromDate = myWebService.formatDate(reportMasterToDateTextBox.Text.Trim().ToString());

            ////reportHeader._rDate = reportMasterFromDateTextBox.Text;

            //string dtnadtime = TotalformatDate(rFromDate, rToDate);
                        
            //if (QualityReportTUOWise.Checked)
            //{                
            //    QualityReportTUOWisePanel.Visible = true;
            //    QualityReportRecipeTUOWisePanel.Visible = false;
            //    excelReport(query);
            //}
            //if (QualityReportRecipeTUOWise.Checked)
            //{
            //    QualityReportTUOWisePanel.Visible = false;
            //    QualityReportRecipeTUOWisePanel.Visible = true;
            //    excelReportRecipeWise("default", rToDate, rFromDate);
            //}
            //Label1.Text = "false";
        }
       // [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
       //public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);





        public void excelReport(string query)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("Select DISTINCT  machinename from productiondataTUO order by machinename asc", con);
                cmd.CommandTimeout = 0;
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
                   // GetWindowThreadProcessId(hwnd, out pid);

                    xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                    Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

                    xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                    xlWorkSheet.Cells[1, 2] = "Performance Report TUO Machine Wise";
                    xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                    xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                    ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                    ((Excel.Range)xlWorkSheet.Cells[3, 2]).EntireColumn.ColumnWidth = "20";
                    xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.get_Range("D3", "E3").Merge(misValue);
                    xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                    xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Merge(misValue);
                    xlWorkSheet.get_Range("C3", "C4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                    xlWorkSheet.get_Range("A3", "H3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
                    xlWorkSheet.Cells[2, 2] = "To : " + formattoDate(rFromDate);
                    xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                    xlWorkSheet.Cells[3, 1] = "Machine name";
                    xlWorkSheet.Cells[3, 2] = "Tyre Type";
                    xlWorkSheet.Cells[3, 3] = "Checked";
                    xlWorkSheet.Cells[3, 4] = "OE";
                    xlWorkSheet.Cells[3, 6] = "";
                    xlWorkSheet.Cells[3, 7] = "Rep";
                    xlWorkSheet.Cells[3, 8] = "Scrap";

                    xlWorkSheet.Cells[4, 4] = "A";
                    xlWorkSheet.Cells[4, 5] = "B";
                    xlWorkSheet.Cells[4, 6] = "C";
                    xlWorkSheet.Cells[4, 7] = "D";
                    xlWorkSheet.Cells[4, 8] = "E";

                    ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                    xlWorkSheet.get_Range("A3", "C3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("D4", "H4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                    xlWorkSheet.get_Range("D3", "H3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                    xlWorkSheet.get_Range("A3", "H3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    xlWorkSheet.get_Range("A4", "H4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);

                    while (dr.Read())
                    {
                        excelReportRecipeWise(dr["machinename"].ToString().Trim(), rToDate, rFromDate);
                    }

                    xlWorkSheet.get_Range("A" + (rowCount + 1), "H" + (rowCount + 1)).Merge(misValue);
                    xlWorkSheet.Cells[rowCount + 2, 1] = "Grand Total";
                    xlWorkSheet.Cells[rowCount + 2, 3] = grandtotal;
                    xlWorkSheet.Cells[rowCount + 2, 4] = grandA;
                    xlWorkSheet.Cells[rowCount + 2, 5] = grandB;
                    xlWorkSheet.Cells[rowCount + 2, 6] = grandC;
                    xlWorkSheet.Cells[rowCount + 2, 7] = grandD;
                    xlWorkSheet.Cells[rowCount + 2, 8] = grandE;

                    xlWorkSheet.get_Range("A1", "H" + (rowCount + 2)).Font.Bold = true;
                    xlWorkSheet.get_Range("A1", "H" + (rowCount + 2)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report TUO Machine Wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                KillProcess(pid, "EXCEL");
            }
        }
        protected string getqueryWCWise(string wcNameString, string dtnadtime)
        {
            if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

            }
            return query;
        }

        protected string getqueryWCWise_old(string wcNameString, string dtnadtime)
        {
            if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE " + wcNameString + " AND ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' )and  ((testTime>" + dtnadtime;
            }
            else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
            {
                query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE " + wcNameString + " AND tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;
            }
            return query;
        }

        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        public void excelReportRecipeWise(string wcName, string rToDate, string rFromDate)
        {
            string query; string query1;
            DataTable dt = new DataTable();
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("tireType", typeof(string));
            gridviewdt.Columns.Add("Checked", typeof(string));
            gridviewdt.Columns.Add("A", typeof(string));
            gridviewdt.Columns.Add("B", typeof(string));
            gridviewdt.Columns.Add("C", typeof(string));
            gridviewdt.Columns.Add("D", typeof(string));
            gridviewdt.Columns.Add("E", typeof(string));
            dt.Columns.Add("tireType", typeof(string));
            dt.Columns.Add("uniformitygrade", typeof(string));
            int total, A, B, C, D, E;
            Double pA, pB, pC, pD, pE;

            int colCount = 1, mergeCount = 0, typeCount = 0;
            query = ""; query1 = "";
            string dtnadtime = TotalprodataformatDate(rToDate, rFromDate);

            if (QualityReportTUOWise.Checked)
            {
                typeCount = 1;
                query = getqueryWCWise("machineName='" + wcName + "'", dtnadtime);
                query1 = getqueryWCWise_old("machineName='" + wcName + "'", dtnadtime);
                xlWorkSheet.Cells[rowCount + 1, 1] = wcName;
                mergeCount = rowCount;
            }
            else if (QualityReportRecipeTUOWise.Checked)
            {
                typeCount = 0;
                xlApp = new Excel.ApplicationClass();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkBook.CheckCompatibility = false;
                xlWorkBook.DoNotPromptForConvert = true;

                //Get PID
                HandleRef hwnd = new HandleRef(xlApp, (IntPtr)xlApp.Hwnd);
               // GetWindowThreadProcessId(hwnd, out pid);

                xlApp.Visible = true; // ensure that the excel app is visible.
                xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet; // Get the current active worksheet.
                Microsoft.Office.Interop.Excel.Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2); //Get more work sheet if neccessary

                xlWorkSheet.get_Range("B1", "E1").Merge(misValue); // Heading
                xlWorkSheet.Cells[1, 2] = "Performance Report TUO Machine Wise";
                xlWorkSheet.get_Range("B1", "E1").Font.Size = 16;
                xlWorkSheet.Cells[1, 6] = DateTime.Now.ToString();
                ((Excel.Range)xlWorkSheet.Cells[1, 6]).EntireColumn.ColumnWidth = "20";
                ((Excel.Range)xlWorkSheet.Cells[1, 2]).EntireColumn.ColumnWidth = "20";

                xlWorkSheet.get_Range("B1", "E1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.get_Range("C3", "D3").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Merge(misValue);
                xlWorkSheet.get_Range("B3", "B4").Merge(misValue);
                xlWorkSheet.get_Range("A3", "A4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("B3", "B4").Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                xlWorkSheet.get_Range("A3", "G3").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                xlWorkSheet.Cells[2, 1] = "From : " + formattoDate(rToDate);
                xlWorkSheet.Cells[2, 2] = "To : " + formattoDate(rFromDate);
                xlWorkSheet.get_Range("A2", "B2").Font.Bold = true;

                xlWorkSheet.Cells[3, 1] = "Tyre Type";
                xlWorkSheet.Cells[3, 2] = "Checked";
                xlWorkSheet.Cells[3, 3] = "OE";
                xlWorkSheet.Cells[3, 5] = "";
                xlWorkSheet.Cells[3, 6] = "Rep";
                xlWorkSheet.Cells[3, 7] = "Scrap";
                xlWorkSheet.get_Range("C3", "D3").Merge(Type.Missing);
                
                xlWorkSheet.Cells[4, 3] = "A";
                xlWorkSheet.Cells[4, 4] = "B";
                xlWorkSheet.Cells[4, 5] = "C";
                xlWorkSheet.Cells[4, 6] = "D";
                xlWorkSheet.Cells[4, 7] = "E";

                ((Excel.Range)xlWorkSheet.Cells[1, 1]).EntireColumn.ColumnWidth = "25";
                xlWorkSheet.get_Range("A3", "B3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("C4", "G4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkOrange);
                xlWorkSheet.get_Range("C3", "G3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkSlateBlue);
                xlWorkSheet.get_Range("A3", "G3").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                xlWorkSheet.get_Range("A4", "G4").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                
                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM  ProductionDataTUO  WHERE ((testTime>" + dtnadtime;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "Select  tireType ,uniformityGrade FROM ProductionDataTUO WHERE tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }


                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All")
                {
                    query1 = "Select  tireType ,uniformityGrade FROM  ProductionDataTUO_3de2020  WHERE ((testTime>" + dtnadtime;
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                {

                    query1 = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query1 = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE tireType in(select name from recipeMaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    query1 = "Select  tireType ,uniformityGrade FROM ProductionDataTUO_3de2020 WHERE tireType in(select name from recipeMaster where tireType in(select name from recipeMaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "' ) AND tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "' ) and  ((testTime>" + dtnadtime;

                }
            }
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);

                if (dt.Rows.Count == 0)
                {
                    myConnection.comm.CommandText = query1;
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                }
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

            DataTable uniqrecipedt = new DataTable();
            uniqrecipedt = GetDistinctRecords(dt, "tireType");

            total_ = 0; A_ = 0; B_ = 0; C_ = 0; D_ = 0; E_ = 0;

            for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
            {
                total = 0; A = 0; B = 0; C = 0; D = 0; E = 0;
                pA = 0; pB = 0; pC = 0; pD = 0; pE = 0; rowCount++;

                total = dt.Select("tireType ='" + uniqrecipedt.Rows[i][0].ToString() + "'").Length;
                A = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='A'"));
                B = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='B'"));
                C = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='C'"));
                D = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='D'"));
                E = Convert.ToInt32(dt.Compute("count(uniformitygrade)", "tireType='" + uniqrecipedt.Rows[i][0].ToString() + "' and uniformitygrade='E'"));

                total_ = total_ + total;
                A_ = A_ + A;
                B_ = B_ + B;
                C_ = C_ + C;
                D_ = D_ + D;
                E_ = E_ + E;

                DataRow dr = gridviewdt.NewRow();
                switch(option)
                {
                    case "1" :
                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0];
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = A;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = B;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = C;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = D;
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = E;
                        break;
                    case "2" :                    
                        pA = ((total == 0) ? 0 : ((double)(A * 100) / total));
                        pB = ((total == 0) ? 0 : ((double)(B * 100) / total));
                        pC = ((total == 0) ? 0 : ((double)(C * 100) / total));
                        pD = ((total == 0) ? 0 : ((double)(D * 100) / total));
                        pE = ((total == 0) ? 0 : ((double)(E * 100) / total));

                        xlWorkSheet.Cells[rowCount, colCount + typeCount] = uniqrecipedt.Rows[i][0].ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 1] = total.ToString();
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 2] = Math.Round(pA, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 3] = Math.Round(pB, 1);

                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 4] = Math.Round(pC, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 5] = Math.Round(pD, 1);
                        xlWorkSheet.Cells[rowCount, colCount + typeCount + 6] = Math.Round(pE, 1);
                        break;
                    }

                gridviewdt.Rows.Add(dr);



            }
            DataRow ndr = gridviewdt.NewRow();
            
            gridviewdt.Rows.Add(ndr);
            DataRow tdr = gridviewdt.NewRow();
            xlWorkSheet.Cells[rowCount + 2, colCount + typeCount] = "Total";
            switch(option)
            {
                case "1" :
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = A_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = B_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = C_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = D_;
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = E_;
                    break;
                case "2" :            
                pA = ((total_ == 0) ? 0 : ((double)(A_ * 100) / total_));
                pB = ((total_ == 0) ? 0 : ((double)(B_ * 100) / total_));
                pC = ((total_ == 0) ? 0 : ((double)(C_ * 100) / total_));
                pD = ((total_ == 0) ? 0 : ((double)(D_ * 100) / total_));
                pE = ((total_ == 0) ? 0 : ((double)(E_ * 100) / total_));

                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 1] = total_.ToString();
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 2] = Math.Round(pA, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 3] = Math.Round(pB, 1);

                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 4] = Math.Round(pC, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 5] = Math.Round(pD, 1);
                xlWorkSheet.Cells[rowCount + 2, colCount + typeCount + 6] = Math.Round(pE, 1);
                break;
            }

            gridviewdt.Rows.Add(tdr);

            rowCount += 2;
            grandtotal = grandtotal + total_;
            grandA = grandA + A_;
            grandB = grandB + B_;
            grandC = grandC + C_;
            grandD = grandD + D_;
            grandE = grandE + E_;
                        
            if (QualityReportTUOWise.Checked)
            {               
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Merge(misValue);
                xlWorkSheet.get_Range("A" + (mergeCount + 1), "A" + rowCount).Cells.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
               
            }
            else if (QualityReportRecipeTUOWise.Checked)
            {
                xlWorkSheet.get_Range("A" + (rowCount + 1), "G" + (rowCount + 1)).Merge(misValue);

                xlWorkSheet.get_Range("A1", "G" + rowCount).Font.Bold = true;
                xlWorkSheet.get_Range("A1", "G" + rowCount).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                xlWorkBook.SaveAs(filepath + fileName, 51, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlShared, misValue, misValue, misValue, misValue, misValue);  //For excel 2007 and above
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                showDownload.Text = "<div id=\"backdiv\" style=\"position: fixed;top: 0;right: 0;bottom: 0;left: 0;z-index: 1040;background: rgba(0, 0, 0, 0.7);margin: 0px;padding: 0px;display: block;cursor: auto;\"><div id=\"innerdiv\" align=\"center\" style=\"width:25%;max-width:25%;height:150px;background: rgba(255, 255, 255, 1);background-color:#A9E2F3;position:fixed;z-index: 1050;top:45px;left: 500px;-moz-border-radius: 10px;-webkit-border-radius: 10px;border-radius: 5px;border:5px solid #888888;box-shadow: 0 0 5px 2px rgba( 0, 0, 0, 0.6 );\"><h3>Performance Report TUO Machine Wise</h3><BR><a href=javascript:void() onClick=\"downloadFile('../Excel/" + fileName + "')\">Click Here</a> to download Excel file  <a href=javascript:void(); title=\"Close\" onClick=\"closebox()\" class=\"close\">X</a></div></div>";
               
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
            return num == "0" ? "" : num.ToString();
        }
        protected void OnDataBound(object sender, EventArgs e)
        {
            /*try
            {
                if (QualityReportTUOWise.Checked)
                {
                    if (performanceReportTUOWiseMainGridView.Rows.Count != 0)
                    {
                        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                        TableHeaderCell cell = new TableHeaderCell();

                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "OE";
                        cell.ColumnSpan = 2;
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "Rep";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "Rejection";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "ViewDetail";
                        row.Controls.Add(cell);


                        performanceReportTUOWiseMainGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    if (performanceReportRecipeTUOWiseGridView.Rows.Count != 0)
                    {
                        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                        TableHeaderCell cell = new TableHeaderCell();

                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "OE";
                        cell.ColumnSpan = 2;
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "";
                        row.Controls.Add(cell);

                        cell = new TableHeaderCell();
                        cell.Text = "Rep";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "Scrap";
                        row.Controls.Add(cell);
                        cell = new TableHeaderCell();
                        cell.Text = "ScrapDetail";
                        row.Controls.Add(cell);

                        performanceReportRecipeTUOWiseGridView.HeaderRow.Parent.Controls.AddAt(0, row);
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }*/
        }

    }
}
