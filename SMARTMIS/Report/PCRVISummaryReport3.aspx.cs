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

namespace SmartMIS.Report
{
    public partial class PCRVISummaryReport3 : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign=null;
        public int totalcheckedcount = 0, okcount = 0, reworkcount = 0, ncmrcount = 0,erraticHoldCount=0,majorHoldcount=0,pass3count=0,pass4count=0, totalrework = 0, treadfault = 0, sidewallfault = 0, beadfault = 0, carcassfault = 0, othersfault = 0;
        int status;

        DataTable wc_name_dt = new DataTable();
        DataTable dt = new DataTable();
               
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "TBRVisualInspectionReport.xlsx";
        string filepath; 


        #endregion
        #region System Defined Function
        public PCRVISummaryReport3()
        {
            filepath = myWebService.getExcelPath();

            dt.Columns.Add("wcID", typeof(int));
            dt.Columns.Add("curingRecipeID", typeof(int));
            dt.Columns.Add("status", typeof(int));
            dt.Columns.Add("defectID", typeof(int));
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("wc_name", typeof(int));
            dt.Columns.Add("recipe_id", typeof(int));
            dt.Columns.Add("recipe_name", typeof(string));
            dt.Columns.Add("recipe_description", typeof(string));
            dt.Columns.Add("defect_id", typeof(int));
            dt.Columns.Add("defectAreaID", typeof(int));
            dt.Columns.Add("defectName", typeof(string));
                      
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                backDiv.Visible = false;
                dialogPanel.Visible = false;
                ShowWarning.Visible = false;
                                       
                if (string.IsNullOrEmpty(tuoReportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
                {
                    tuoReportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    tuoReportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    
                }
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    
                    rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
                                        
                    if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        
                    }
                    else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        
                    }
                    else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
                    {

                    }

                    backDiv.Visible = false;
            
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void loadData()
        {
            DataTable dt_vi = new DataTable();
                        DataTable dt_workcenter = new DataTable();
                        DataTable dt_recipe = new DataTable();
                        DataTable dt_defect = new DataTable();
                        rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        //myConnection.comm.CommandText = "select wcID, curingRecipeID, status, defectStatusID As defectID from VInspectionPCR where ((dtandTime>=" + rToDate + " AND wcID in (select iD from wcmaster where vistage=3 and processID=9) and status in (55,56,57,58)";

                        myConnection.comm.CommandText = @"select wcID,curingRecipeID,status,defectID from (select wcID,gtbarCode, curingRecipeID, status, defectStatusID As defectID ,row_number() over (partition by gtbarCode order by dtandTime desc) as rono from VInspectionPCR where ((dtandTime>=" + rToDate + " AND wcID in (select iD from wcmaster where vistage=3 and processID=9) and status in (55,56,57,58)) as t where rono = 1";
                         myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_vi.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select Distinct iD, name from wcMaster where vistage=3 and processID=9";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_workcenter.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select iD AS recipe_id, name, description from recipeMaster where processID = 8";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_recipe.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "select ID AS defect_id, faultAreaID As defectAreaID, name As defectName from PCRDefectMaster";
                        myConnection.reader = myConnection.comm.ExecuteReader();
                        dt_defect.Load(myConnection.reader);

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        var row = from r0w1 in dt_vi.AsEnumerable()
                                  join r0w2 in dt_workcenter.AsEnumerable()
                                    on r0w1.Field<int>("wcID") equals r0w2.Field<int>("iD")
                                  join r0w3 in dt_recipe.AsEnumerable()
                                    on r0w1.Field<int?>("curingRecipeID") equals r0w3.Field<int?>("recipe_id")
                                  join r0w4 in dt_defect.AsEnumerable()
                                    on r0w1.Field<int?>("defectID") equals r0w4.Field<int?>("defect_id") into ps
                                  from r0w4 in ps.DefaultIfEmpty()
                                  select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3.ItemArray.Concat(r0w4 != null ? r0w4.ItemArray : new object[] { }))).ToArray();

                        foreach (object[] values in row)
                            dt.Rows.Add(values);
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    
                    if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;                    
                    }
                    else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    }
                    
                    for (int rowIndex = MainGridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow gvRow = MainGridView.Rows[rowIndex];
                        GridViewRow gvPreviousRow = MainGridView.Rows[rowIndex + 1];
                        for (int cellCount = 0; cellCount < 2 /*gvRow.Cells.Count*/; cellCount++)
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
        #endregion
        #region User Defined Function
                        
        public string formatfromDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(formatDate(date));
                    flag = tempDate.ToString("MM-dd-yyyy");
                    flag = flag + " " + "07:00:00";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message+ date, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
                try
                {
                    DateTime tempDate = Convert.ToDateTime(formatDate(date)).AddDays(1);
                    flag = tempDate.ToString("MM-dd-yyyy");
                    flag = flag + " " + "07:00:00";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
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
        public string formattoDate11(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    month = tempDate[1].ToString().Trim();
                    day = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (Convert.ToInt32(month) == 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-05-" + (Convert.ToInt32(year) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    else if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = month + "-" + (Convert.ToInt32(day)).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "05" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }



        #endregion
        
        protected void FaultTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
            int result = (int)ts.TotalDays;
            if ((int)ts.TotalDays < 0)
            {
                ShowWarning.Visible = true;
                ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>From Date cannot be greater than To Date!!!</font></strong></td></tr></table>";
            }
            else
            {
                if (FaultTypeDropDownList.SelectedValue == "ERRATIC HOLD")
                    status = 55;
                if (FaultTypeDropDownList.SelectedValue == "MAJOR HOLD")
                    status = 56;
                if (FaultTypeDropDownList.SelectedValue == "PASS-3")
                    status = 57;
                if (FaultTypeDropDownList.SelectedValue == "PASS-4")
                    status = 58;

                if (((DropDownList)sender).ID == "FaultTypeDropDownList")
                    FaultAreaDropDownList.SelectedIndex = 0;

                if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                {
                    showVIReport();
                }
                else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                {
                    showCategoryType();
                }
                else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
                {
                    showCategoryFaultType();
                }


                getdisplaytype = displayType.SelectedItem.ToString();
            }
        }
        private void showCategoryFaultType()
        {
            try
            {
                loadData();
                MainGridView.Visible = true;
                VIRecipeWiseGridView.Visible = false;


                string[] defect_name = { "Tread", "SideWall", "Bead", "Carcass", "Others" };
                string defectArea = FaultAreaDropDownList.SelectedValue.ToString();
                int defectID = 0, total_defects = 0;

                switch (defectArea)
                {
                    case "Tread":
                        defectID = 1;
                        break;
                    case "SideWall":
                        defectID = 2;
                        break;
                    case "Bead":
                        defectID = 3;
                        break;
                    case "Carcass":
                        defectID = 4;
                        break;
                    case "Others":
                        defectID = 5;
                        break;

                }
                string defect_type = "";
               // if (status == 32)
                  //  defect_type = "HOLDFaultArea";
               // else if (status == 33)
                   // defect_type = "SCRAPFaultArea";

                if (status == 55)
                    defect_type = "Total_Erratic_Hold";
                else if (status == 56)
                    defect_type = "Total_Major_Hold";
                else if (status == 57)
                    defect_type = "Total_Pass-3";
                else if (status == 58)
                    defect_type = "Total_Pass-4";
                getdisplaytype = displayType.SelectedItem.ToString();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("TyreType", typeof(string));
                gridviewdt.Columns.Add(defect_type, typeof(string));
                gridviewdt.Columns.Add("FaultName", typeof(string));
                gridviewdt.Columns.Add("Tyre_Count", typeof(string));

                createGridView(gridviewdt, MainGridView);

                DataTable fault_dt = new DataTable();
                rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
                               
                var query = (dt.AsEnumerable().GroupBy(l => l.Field<int?>("curingRecipeID"))
                    .Select(g => new
                    {
                        curing_id = g.Key
                    }));
                int num_of_curing = query.Count();
                var items = query.ToArray();


                foreach (var item in items)
                {
                    var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                    {
                        description = l.Field<string>("recipe_description"),
                        defectAreaID = l.Field<int?>("defectAreaID"),
                        status = l.Field<int?>("status")
                    }).ToArray();

                    if (defectArea == "All")
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var inner_data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id && l.Field<int?>("status") == status && l.Field<int?>("defectAreaID").Equals(i + 1))
                                .GroupBy(x => x.Field<string>("defectName"))
                                .Select(l => new
                            {
                                defectName = l.Key,
                                defectcount = l.Count()
                            }).ToArray();


                            foreach (var inner_item in inner_data)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                dr[0] = data[0].description;
                                dr[1] = defect_name[i].ToString();
                                dr[2] = inner_item.defectName;
                                dr[3] = inner_item.defectcount;
                                total_defects += Convert.ToInt32(dr[3]);
                                gridviewdt.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        var inner_data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id && l.Field<int?>("status") == status && l.Field<int?>("defectAreaID").Equals(defectID))
                                .GroupBy(x => x.Field<string>("defectName"))
                                .Select(l => new
                                {
                                    defectName = l.Key,
                                    defectcount = l.Count()
                                }).ToArray();


                            foreach (var inner_item in inner_data)
                            {
                                DataRow dr = gridviewdt.NewRow();
                                dr[0] = data[0].description;
                                dr[1] = defectArea.ToString();
                                dr[2] = inner_item.defectName;
                                dr[3] = inner_item.defectcount;
                                total_defects += Convert.ToInt32(dr[3]);
                                gridviewdt.Rows.Add(dr);
                            }                       
                    }
                }
                DataRow drt = gridviewdt.NewRow();
                drt[0] = "Total";
                drt[3] = total_defects;
                gridviewdt.Rows.Add(drt);

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // Select the rows where showing total & make them bold
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }
        private void showCategoryType()
        {
            try
            {
                loadData();
                MainGridView.Visible = true;
                VIRecipeWiseGridView.Visible = false;

                getdisplaytype = displayType.SelectedItem.ToString();
                DataTable gridviewdt = new DataTable();
                string defect_type = "";
                if (status == 55)
                    defect_type = "Total_Erratic_Hold";
                else if (status == 56)
                    defect_type = "Total_Major_Hold";
                else if (status == 57)
                    defect_type = "Total_Pass-3";
                else if (status == 58)
                    defect_type = "Total_Pass-4";
                
                gridviewdt.Columns.Add("TyreType", typeof(string));
                gridviewdt.Columns.Add("Total_checked", typeof(string));
                gridviewdt.Columns.Add(defect_type, typeof(string));
                gridviewdt.Columns.Add("Tread", typeof(string));
                gridviewdt.Columns.Add("Sidewall", typeof(string));
                gridviewdt.Columns.Add("Bead", typeof(string));
                gridviewdt.Columns.Add("Carcass", typeof(string));
                gridviewdt.Columns.Add("Others", typeof(string));

                createGridView(gridviewdt, MainGridView);

                var query = (dt.AsEnumerable().GroupBy(l => l.Field<int?>("curingRecipeID"))
                    .Select(g => new
                    {
                        curing_id = g.Key
                    }));
                int num_of_curing = query.Count();
                var items = query.ToArray();

                //
                if (getdisplaytype == "Numbers")
                {
                    foreach (var item in items)
                    {
                        DataRow dr = gridviewdt.NewRow();

                        var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                        {
                            description = l.Field<string>("recipe_description"),
                            defectAreaID = l.Field<int?>("defectAreaID"),
                            status = l.Field<int?>("status")
                        }).ToArray();

                        if (data.Count() != 0)
                        {
                            totalcheckedcount += data.Count(); ;
                            reworkcount += data.Count(d => d.status == status); ;
                            treadfault += data.Count(d => d.defectAreaID == 1 && d.status == status); ;
                            sidewallfault += data.Count(d => d.defectAreaID == 2 && d.status == status);
                            beadfault += data.Count(d => d.defectAreaID == 3 && d.status == status);
                            carcassfault += data.Count(d => d.defectAreaID == 4 && d.status == status);
                            othersfault += data.Count(d => d.defectAreaID == 5 && d.status == status);

                            dr[0] = data[0].description;
                            dr[1] = data.Count();
                            dr[2] = data.Count(d => d.status == status);
                            dr[3] = data.Count(d => d.defectAreaID == 1 && d.status == status);
                            dr[4] = data.Count(d => d.defectAreaID == 2 && d.status == status);
                            dr[5] = data.Count(d => d.defectAreaID == 3 && d.status == status);
                            dr[6] = data.Count(d => d.defectAreaID == 4 && d.status == status);
                            dr[7] = data.Count(d => d.defectAreaID == 5 && d.status == status);
                        }
                        gridviewdt.Rows.Add(dr);
                    }
                    DataRow drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[1] = totalcheckedcount;
                    drt[2] = reworkcount;
                    drt[3] = treadfault;
                    drt[4] = sidewallfault;
                    drt[5] = beadfault;
                    drt[6] = carcassfault;
                    drt[7] = othersfault;
                    gridviewdt.Rows.Add(drt);
                }
                else
                {
                    foreach (var item in items)
                    {
                        DataRow dr = gridviewdt.NewRow();

                        var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                        {
                            description = l.Field<string>("recipe_description"),
                            defectAreaID = l.Field<int?>("defectAreaID"),
                            status = l.Field<int?>("status")
                        }).ToArray();

                        if (data.Count() != 0)
                        {
                            totalcheckedcount += data.Count(); ;
                            reworkcount += data.Count(d => d.status == status); ;
                            treadfault += data.Count(d => d.defectAreaID == 1 && d.status == status); ;
                            sidewallfault += data.Count(d => d.defectAreaID == 2 && d.status == status);
                            beadfault += data.Count(d => d.defectAreaID == 3 && d.status == status);
                            carcassfault += data.Count(d => d.defectAreaID == 4 && d.status == status);
                            othersfault += data.Count(d => d.defectAreaID == 5 && d.status == status);


                            dr[0] = data[0].description;
                            dr[1] = data.Count();
                            dr[2] = (data.Count(d => d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[3] = (data.Count(d => d.defectAreaID == 1 && d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[4] = (data.Count(d => d.defectAreaID == 2 && d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[5] = (data.Count(d => d.defectAreaID == 3 && d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[6] = (data.Count(d => d.defectAreaID == 4 && d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[7] = (data.Count(d => d.defectAreaID == 5 && d.status == status) * 100 / Convert.ToInt32(dr[1])) + "%";
                        }
                        gridviewdt.Rows.Add(dr);
                    }
                    DataRow drt = gridviewdt.NewRow();
                    drt[0] = "Total";
                    drt[1] = totalcheckedcount;
                    drt[2] = (reworkcount * 100 / totalcheckedcount) + "%";
                    drt[3] = (treadfault * 100 / totalcheckedcount) + "%";
                    drt[4] = (sidewallfault * 100 / totalcheckedcount) + "%";
                    drt[5] = (beadfault * 100 / totalcheckedcount) + "%";
                    drt[6] = (carcassfault * 100 / totalcheckedcount) + "%";
                    drt[7] = (othersfault * 100 / totalcheckedcount) + "%";
                    gridviewdt.Rows.Add(drt);
                }

                MainGridView.DataSource = gridviewdt;
                MainGridView.DataBind();

                // Select the rows where showing total & make them bold
                IEnumerable<GridViewRow> rows = MainGridView.Rows.Cast<GridViewRow>()
    .Where(row => row.Cells[0].Text == "Total");

                foreach (var row in rows)
                    row.Font.Bold = true;

                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }             public string getdisplaytype = "Numbers";
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
            rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
            TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
            int result = (int)ts.TotalDays;
            if ((int)ts.TotalDays < 0)
            {
                ShowWarning.Visible = true;
                ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>From Date cannot be greater than To Date!!!</font></strong></td></tr></table>";
            }
            else
                showVIReport();   
        }
        private void showVIReport()
        {
            try
            {
                loadData();
                VIRecipeWiseGridView.Visible = true;
                MainGridView.Visible = false;
                
                getdisplaytype = displayType.SelectedItem.ToString();
                DataTable gridviewdt = new DataTable();
                DataRow drt;
                gridviewdt.Columns.Add("curingRecipeName", typeof(string));
                gridviewdt.Columns.Add("TotalChecked", typeof(string));
                gridviewdt.Columns.Add("TotalErraticHOLD", typeof(string));
                gridviewdt.Columns.Add("TotalMAJORHOLD", typeof(string));
                gridviewdt.Columns.Add("TotalPASS-3", typeof(string));
               // gridviewdt.Columns.Add("TotalPASS-4", typeof(string));

                //createGridView(gridviewdt);

                var query = (dt.AsEnumerable().GroupBy(l => l.Field<int?>("curingRecipeID"))
                    .Select(g => new
                    {
                        curing_id = g.Key
                    }));
                int num_of_curing = query.Count();
                var items = query.ToArray();

                //
                switch(getdisplaytype)
                {
                    case "Numbers":
                        foreach (var item in items)
                        {
                            DataRow dr = gridviewdt.NewRow();

                            var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                            {
                                description = l.Field<string>("recipe_description"),
                                status = l.Field<int?>("status")
                            }).ToArray();

                            if (data.Count() != 0)
                            {
                                totalcheckedcount += data.Count();
                                erraticHoldCount += data.Count(d => d.status == 55); //Last time 31
                                majorHoldcount += data.Count(d => d.status == 56); //Last time 32
                                pass3count += data.Count(d => d.status == 57); //Last time 33
                                pass4count += data.Count(d => d.status == 58); //Last time 33

                                dr[0] = data[0].description;
                                dr[1] = data.Count();
                                dr[2] = data.Count(d => d.status == 55);
                                dr[3] = data.Count(d => d.status == 56);
                                dr[4] = data.Count(d => d.status == 57);
                               // dr[5] = data.Count(d => d.status == 58);
                            }
                            gridviewdt.Rows.Add(dr);

                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = "TOTAL";
                        drt[1] = totalcheckedcount;
                        drt[2] = erraticHoldCount;
                        drt[3] = majorHoldcount;
                        drt[4] = pass3count;
                       // drt[5] = pass4count;
                        gridviewdt.Rows.Add(drt);
                        break;
                    case "Percent":
                        foreach (var item in items)
                        {
                            DataRow dr = gridviewdt.NewRow();

                            var data = dt.AsEnumerable().Where(l => l.Field<int?>("curingRecipeID") == item.curing_id).Select(l => new
                            {
                                description = l.Field<string>("recipe_description"),
                                status = l.Field<int?>("status")
                            }).ToArray();

                            totalcheckedcount += data.Count();
                            //okcount += data.Count(d => d.status == 31);
                           // reworkcount += data.Count(d => d.status == 32);
                            //ncmrcount += data.Count(d => d.status == 33);

                            erraticHoldCount += data.Count(d => d.status == 55); //Last time 31
                            majorHoldcount += data.Count(d => d.status == 56); //Last time 32
                            pass3count += data.Count(d => d.status == 57); //Last time 33
                            pass4count += data.Count(d => d.status == 58); //Last time 33

                            dr[0] = data[0].description;
                            dr[1] = data.Count();
                            dr[2] = (data.Count(d => d.status == 55) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[3] = (data.Count(d => d.status == 56) * 100 / Convert.ToInt32(dr[1])) + "%";
                            dr[4] = (data.Count(d => d.status == 57) * 100 / Convert.ToInt32(dr[1])) + "%";
                           // dr[5] = (data.Count(d => d.status == 58) * 100 / Convert.ToInt32(dr[1])) + "%";

                            gridviewdt.Rows.Add(dr);
                        }
                        drt = gridviewdt.NewRow();
                        drt[0] = "TOTAL";
                        drt[1] = totalcheckedcount;
                        drt[2] = (erraticHoldCount * 100 / totalcheckedcount) + "%";
                        drt[3] = (majorHoldcount * 100 / totalcheckedcount) + "%";
                        drt[4] = (pass3count * 100 / totalcheckedcount) + "%";
                       // drt[5] = (pass4count * 100 / totalcheckedcount) + "%";
                        gridviewdt.Rows.Add(drt);
                        break;
                }
                VIRecipeWiseGridView.DataSource = gridviewdt;
                VIRecipeWiseGridView.DataBind();

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
        protected void VIRecipeWiseTotalMinor_Click(object sender, EventArgs e)
        {
           if (((LinkButton)sender).ID == "VIRecipeWiseTotalMinorLink")
            {
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((LinkButton)sender).Parent).Parent;
                string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("VISizeWiseTyreTypeLabel")).Text);

                fillBarCodeDetailGridView(recipeCode.ToString(),"57");
            }
            else if (((LinkButton)sender).ID == "VIRecipeWiseNotOk")
            {
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((LinkButton)sender).Parent).Parent;
                string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("VISizeWiseTyreTypeLabel")).Text);

                fillBarCodeDetailGridView(recipeCode.ToString(), "56");
            }
           else if (((LinkButton)sender).ID == "VIRecipeWiseTotalMinorLink123")
            {
                fillBarCodeDetailGridView("Total", "58");
            }
            else if (((LinkButton)sender).ID == "VIRecipeWiseTotalMinorLinkTotal")
            {
                fillBarCodeDetailGridView("Total", "33");
            }
        }
        private string TotaldtformatDate(String fromDate, String toDate)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";

            if (fromDate != null)
            {
                string fday, fmonth, fyear;
                string tday, tmonth, tyear;

                string[] ftempDate = fromDate.Split(new char[] { '-' });
                string[] ttempDate = toDate.Split(new char[] { '-' });

                try
                {
                    fday = ftempDate[1].ToString().Trim();
                    fmonth = ftempDate[0].ToString().Trim();
                    fyear = ftempDate[2].ToString().Trim();
                    tday = ttempDate[1].ToString().Trim();
                    tmonth = ttempDate[0].ToString().Trim();
                    tyear = ttempDate[2].ToString().Trim();

                    flag1 = fday + "-" + fmonth + "-" + fyear + " " + "07" + ":" + "00" + ":" + "00";

                    if (Convert.ToInt32(tday) == 12 && Convert.ToInt32(tmonth) == 31)
                    {
                        flag2 = "01-01-" + (Convert.ToInt32(tyear) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    if (DateTime.DaysInMonth(Convert.ToInt32(tyear), Convert.ToInt32(tday)) != Convert.ToInt32(tmonth))
                    {
                        flag2 = tday + "-" + (Convert.ToInt32(tmonth) + 1).ToString() + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else if (Convert.ToInt32(tday) < 12 )
                    {
                        flag2 = (Convert.ToInt32(tday) + 1).ToString() + "-" + "01" + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    //flag2 = tday + "-" + tmonth + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";


                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' ))";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

            }
            return flag;
        }
        private void fillBarCodeDetailGridView(string recipecode,string status)
        {
            try
            {
                DataTable gridviewdt = new DataTable();
                DataTable dt = new DataTable();
                DataTable curdt = new DataTable();
                DataTable tbmdt = new DataTable();
                string from_date = formatfromDate(tuoReportMasterFromDateTextBox.Text),
                to_date = formattoDate(tuoReportMasterToDateTextBox.Text);
                string tempfromdate = Convert.ToDateTime(from_date).AddDays(-10).ToString();
                curdt.Columns.Add("wcName", typeof(string));
                curdt.Columns.Add("mouldNo", typeof(string));
                curdt.Columns.Add("cur_gtbarCode", typeof(string));
                tbmdt.Columns.Add("wcName", typeof(string));
                tbmdt.Columns.Add("tbm_gtbarCode", typeof(string));
                

                dt.Columns.Add("wcname", typeof(string));
                dt.Columns.Add("description", typeof(string));
                dt.Columns.Add("vi_gtbarcode", typeof(string));

                gridviewdt.Columns.Add("visualWCName", typeof(string));
                gridviewdt.Columns.Add("size", typeof(string));
                gridviewdt.Columns.Add("vi_barcode", typeof(string));

                gridviewdt.Columns.Add("mouldName", typeof(string));
                gridviewdt.Columns.Add("cur_barcode", typeof(string));
                gridviewdt.Columns.Add("curingWCName", typeof(string));

                gridviewdt.Columns.Add("tbm_barcode", typeof(string));
                gridviewdt.Columns.Add("tbmWCName", typeof(string));
                rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
                if (recipecode != "Total")
                    recipecode = " AND description='" + recipecode + "'";
                else
                    recipecode = "";
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select wcname, description, gtbarCode AS vi_gtbarCode from vVisualInspectionPCR where status='" + status + "' AND   wcID in (select iD from wcmaster where vistage=3 and processID=9) AND dtandTime>='" + from_date + "' AND dtandTime<'" + to_date + "'" + recipecode;
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(myConnection.reader);
                myConnection.conn.Close();
                myConnection.comm.Dispose();
                myConnection.reader.Close();

                string barcodeInQuery = "(";
                int TotalRow = dt.Rows.Count;

                string[] res = new string[TotalRow];
                for (int i = 0; i < TotalRow; i++)
                {
                    res[i] = dt.Rows[i]["vi_gtbarCode"].ToString();
                    barcodeInQuery += "'" + res[i].ToString() + "',";
                }

                barcodeInQuery = barcodeInQuery.TrimEnd(',');
                barcodeInQuery += ")";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "SELECT wcName as curingWCName, mouldNo, gtbarcode AS cur_gtbarCode FROM vCuringpcr WHERE dtandTime>='" + tempfromdate + "' AND dtandTime<'" + to_date + "' and gtbarcode in " + barcodeInQuery.ToString() + "";
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                curdt.Load(myConnection.reader);
                myConnection.conn.Close();
                myConnection.comm.Dispose();
                myConnection.reader.Close();

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "SELECT wcName as tbmWCName, gtbarcode AS tbm_gtbarCode FROM vTbmpcr WHERE dtandTime>='" + tempfromdate + "' AND dtandTime<'" + to_date + "' and gtbarcode in " + barcodeInQuery.ToString() + "";
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                tbmdt.Load(myConnection.reader);
                myConnection.conn.Close();
                myConnection.comm.Dispose();
                myConnection.reader.Close();

                //if (dt.Rows.Count != 0)
                {
                    var row = from r0w1 in dt.AsEnumerable()
                              join r0w2 in curdt.AsEnumerable()
                                on r0w1.Field<string>("vi_gtbarcode") equals r0w2.Field<string>("cur_gtbarcode") into p
                              from r0w2 in p.DefaultIfEmpty()
                              join r0w3 in tbmdt.AsEnumerable()
                                on r0w1.Field<string>("vi_gtbarcode") equals r0w3.Field<string>("tbm_gtbarcode") into ps
                              from r0w3 in ps.DefaultIfEmpty()
                              select r0w1.ItemArray.Concat(r0w2 != null ? r0w2.ItemArray.Skip(1) : new object[] { "", "", "" })
                                  .Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", ""}).ToArray();


                    foreach (object[] values in row)
                        gridviewdt.Rows.Add(values);

                    gridviewdt.Columns.Remove("cur_barcode");
                    gridviewdt.Columns.Remove("vi_barcode");
                    
                    performanceReportBarcodeDetailGridView.DataSource = gridviewdt;
                    performanceReportBarcodeDetailGridView.DataBind();
                    performanceReportBarcodeDetailGridView.Visible = true;
                    backDiv.Visible = true;
                    dialogPanel.Visible = true;
                    emptyMsg.Visible = false;
            
                    /*if (performanceReportBarcodeDetailGridView.Rows.Count == 0)
                        emptyMsg.Visible = true;*/
                }
                
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
        protected void displayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            if (FaultTypeDropDownList.SelectedValue == "HOLD")
                status = 32;
            if (FaultTypeDropDownList.SelectedValue == "SCRAP")
                status = 33;

            if (((DropDownList)sender).ID == "FaultTypeDropDownList")
                FaultAreaDropDownList.SelectedIndex = 0;

            if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
            {
                showVIReport();
            }
            else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
            {
                showCategoryType();
            }
            else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
            {
                showCategoryFaultType();
            }


            getdisplaytype = displayType.SelectedItem.ToString();
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
                int result = (int)ts.TotalDays;

                if ((int)ts.TotalDays < 0)
                {
                    ShowWarning.Visible = true;
                    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>From Date cannot be greater than To Date!!!</font></strong></td></tr></table>";
                }
                else
                {
                   // rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));


                    DataTable gridviewdt = new DataTable();


                    DataTable dt = new DataTable();
                    DataTable curdt = new DataTable();
                    DataTable tbmdt = new DataTable();
                    string from_date = formatfromDate(tuoReportMasterFromDateTextBox.Text),
                           to_date = formattoDate(tuoReportMasterToDateTextBox.Text);
                    //string from_date = formatfromDate(tuoReportMasterFromDateTextBox.Text),
                    //to_date = formattoDate(tuoReportMasterToDateTextBox.Text);

                    string[] head_array = new string[] { "S. No.", "visualWCName", "TyreSize", "Barcode", "Status", "DefectAreaName", "Defectname", "Remark", "shift", "InspectorName", "VIDate", "VITime", "PressNo", "cavityName", "MouldNo", "Cure_Date", "Cure_Time", "tbmWCName", "TBM_Date", "TBM_Time", "BuilderName", "BuilderName2", "BuilderName3" };
                    foreach (var arr in head_array)
                        gridviewdt.Columns.Add(arr, typeof(string));

                    createGridView(gridviewdt, ExcelGridView);

                    rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
//                    myConnection.comm.CommandText = @"select wcname AS visualWCName, description AS TyreSize, gtbarCode AS BarCode,
//                    StatusName AS Status, defectLocationName as defectAreaName, defectname, remarks, 
//                    shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND 
//                    convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
//                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
//                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
//                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),firstname as InspectorName,convert(char(10), dtandTime, 105) AS VIDate, convert(char(8), dtandTime, 108) AS VITime
//                    from vvisualInspectionPCR where  wcID in (select iD from wcmaster where vistage=3 and processID=9) and status in (55,56,57) AND dtandTime>='" + from_date + "' AND dtandTime<'" + to_date + "' order by VITime asc";

                    myConnection.comm.CommandText = @"select visualWCName,TyreSize,BarCode,Status,defectAreaName,defectname,remarks,shift,InspectorName,VIDate, VITime,dtandTime from ( select wcname AS visualWCName, description AS TyreSize, gtbarCode AS BarCode, StatusName AS Status, defectLocationName as defectAreaName, defectname, dtandTime,remarks, 
                    shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN 
                    convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' 
                    THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999')
                    or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),firstname as InspectorName,convert(char(10), dtandTime, 105) AS VIDate, convert(char(8), dtandTime, 108) AS VITime ,row_number() over (partition by gtbarCode order by dtandTime asc) as rono
                    from vvisualInspectionPCR where  wcID in (select iD from wcmaster where vistage=3 and processID=9) and status in (55,56,57) AND dtandTime>='" + from_date + "' AND dtandTime<'" + to_date + "') as t where rono = 1  order by dtandTime asc";
                    
                    
                  
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    dt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    dt.Columns.RemoveAt(11);

                    string tempfromdt = Convert.ToDateTime(from_date).AddDays(-10).ToString();
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "SELECT gtbarCode AS cur_gtbarcode, wcName As PressNo,RIGHT(pressbarcode,8) as cavityNo, case when  pressbarCode like'%L%' then  SUBSTRING(mouldNo, 0, CHARINDEX('#', mouldNo)) when pressbarCode like'%R%' then  SUBSTRING(mouldNo, CHARINDEX('#', mouldNo)  + 1, LEN(mouldNo)) end as  mouldNo, convert(char(10), dtandTime, 105) AS Cure_Date, convert(char(8), dtandTime, 108) AS Cure_Time FROM vCuringpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    curdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "SELECT gtbarCode AS tbm_gtbarcode, wcName AS tbmWCName,convert(char(10), dtandTime, 105) AS TBM_Date, convert(char(8), dtandTime, 108) AS TBM_Time, firstName + ' ' + lastName As BuilderName, isnull( (select firstName from manningMaster where iD= manningID2),'unknown') as BuilderName2, isnull( (select firstName from manningMaster where iD= manningID3),'unknown') as BuilderName3 FROM vTbmpcr WHERE dtandTime>='" + tempfromdt + "' AND dtandTime<'" + to_date + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    tbmdt.Load(myConnection.reader);
                    myConnection.conn.Close();
                    myConnection.comm.Dispose();
                    myConnection.reader.Close();

                    if (dt.Rows.Count != 0)
                    {
                        int serial_number = 1;
                        var row = from r0w1 in dt.AsEnumerable()
                                  join r0w2 in curdt.AsEnumerable()
                                    on r0w1.Field<string>("Barcode") equals r0w2.Field<string>("cur_gtbarcode") into p
                                  from r0w2 in p.DefaultIfEmpty()
                                  join r0w3 in tbmdt.AsEnumerable()
                                    on r0w1.Field<string>("Barcode") equals r0w3.Field<string>("tbm_gtbarCode") into ps
                                  from r0w3 in ps.DefaultIfEmpty()
                                  select new string[] { serial_number++.ToString() }
                                  .Concat(r0w1.ItemArray.Concat
                                  (r0w2 != null ? r0w2.ItemArray.Skip(1): new object[] { "", "", "", "","" })
                                  .Concat(r0w3 != null ? r0w3.ItemArray.Skip(1) : new object[] { "", "", "", "" })).ToArray();

                        foreach (object[] values in row)
                            gridviewdt.Rows.Add(values);

                        ExcelGridView.DataSource = gridviewdt;
                        ExcelGridView.DataBind();

                        ExcelPanel.Visible = true;
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=PCRMajorReport.xls");
                        Response.ContentType = "application/vnd.ms-excel";

                        StringWriter stringWrite = new StringWriter();
                        HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                        ExcelPanel.RenderControl(htmlWrite);

                        Response.Write(stringWrite.ToString());

                        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                        //Response.Flush();
                        //Response.End();
                        ExcelPanel.Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message+ exp.Source, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        
    }
}
