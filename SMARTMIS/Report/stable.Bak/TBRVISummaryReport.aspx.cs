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

namespace SmartMIS.Report
{
    public partial class TBRVISummaryReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #endregion
        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery, machinenamequery, percent_sign=null;
        string faultname;
        string recipeCode;
        string faultArea;
        string recipeID ;
        string faultAreaID;
        int status, pid = -1;

        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;

        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "SecondPCRVisualInspectionReport.xlsx";
        string filepath; 


        #endregion
        #region System Defined Function
        public TBRVISummaryReport()
        {
            filepath = myWebService.getExcelPath();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                    if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        VIReportRecipeWiseMainPanel.Visible = true;
                        VIFaultWisePanel.Visible = false;
                        SizeWiseRegionPanel.Visible = false;
                    }
                    else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                    {
                        VIReportRecipeWiseMainPanel.Visible = false;
                        VIFaultWisePanel.Visible = false;
                        SizeWiseRegionPanel.Visible = true;
                    }
                    else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
                    {

                        VIFaultWisePanel.Visible = true;
                        VIReportRecipeWiseMainPanel.Visible = false;

                        SizeWiseRegionPanel.Visible = false;
                    }

                    backDiv.Visible = false;
                    dialogPanel.Visible = false;
            
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            try
            {
               if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (((GridView)sender).ID == "VIRecipeWiseGridView")
                    {
                        Label recipeIDLabel = ((Label)e.Row.FindControl("VISizeWiseTyreTypeIDLabel"));
                        recipeID = recipeIDLabel.Text;
                        Label recipeCodeLabel = ((Label)e.Row.FindControl("VISizeWiseTyreTypeLabel"));
                        
                        GridView childGridView = ((GridView)e.Row.FindControl("VIRecipeWiseChildGridView"));

                        recipeCodeLabel.Text = myWebService.getRecipeCode("recipeMaster", "iD", recipeIDLabel.Text);
                        fillChildInnerGridView("3401", "gg", childGridView, recipeIDLabel.Text.Trim(), rToDate, rFromDate, "1");
                    }

                    else if (((GridView)sender).ID == "SizeWiseRegionGridView")
                    {
                        Label recipeIDLabel = ((Label)e.Row.FindControl("VISizeFaultWiseTyreTypeIDLabel"));

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("VISizeFaultWiseTyreTypeLabel"));
                        recipeCodeLabel.Text = myWebService.getRecipeCode("recipeMaster", "iD", recipeIDLabel.Text);
                        GridView childGridView = ((GridView)e.Row.FindControl("VIRecipeFaultWiseChildGridView"));
                       
                            faultname = FaultTypeDropDownList.SelectedValue;

                            if (faultname == "REWORK")
                                faultname = "2";
                            else if (faultname=="NCMR")
                                faultname = "3";


                        fillChildInnerGridView(faultname, "gg", childGridView, recipeIDLabel.Text.Trim(), rToDate, rFromDate, "1");
                    }

                    else if (((GridView)sender).ID == "VIFaultWiseGridView")
                    {
                        Label recipeIDLabel = ((Label)e.Row.FindControl("VIFaultWiseTyreTypeIDLabel"));
                        Label recipeCodeLabel = ((Label)e.Row.FindControl("VIFaultWiseTyreTypeLabel"));
                        recipeCodeLabel.Text = myWebService.getRecipeCode("recipeMaster", "ID", recipeIDLabel.Text);

                        GridView childGridView = ((GridView)e.Row.FindControl("VIFaultWiseChildGridView"));
                        
                            faultname = FaultTypeDropDownList.SelectedValue;
                            if (faultname == "REWORK")
                                faultname = "2";
                            else if (faultname == "NCMR")
                                faultname = "3";
                            recipeID = recipeIDLabel.Text;
                        recipeCode = recipeCodeLabel.Text;
                        fillChildInnerGridView(faultname, "gg", childGridView, recipeIDLabel.Text, rToDate, rFromDate, "1");
                    }

                    else if (((GridView)sender).ID == "VIFaultWiseChildGridView")
                    {
                        Label faultAreaIDLabel = ((Label)e.Row.FindControl("VIfaltAreaIDLabel"));
                       
                        Label FaltAreaNameLabel = ((Label)e.Row.FindControl("VIfaltAreaNameLabel"));

                        if (faultAreaIDLabel.Text == "1")
                            FaltAreaNameLabel.Text = "Tread";
                        else if(faultAreaIDLabel.Text=="2")
                            FaltAreaNameLabel.Text = "SideWall";
                        else if (faultAreaIDLabel.Text == "3")
                            FaltAreaNameLabel.Text = "Bead";
                        else if (faultAreaIDLabel.Text == "4")
                            FaltAreaNameLabel.Text = "CarCass";
                        else if (faultAreaIDLabel.Text == "5")
                            FaltAreaNameLabel.Text = "Others";

                        GridView childGridView = ((GridView)e.Row.FindControl("VIFaultNameChildGridView"));

                        faultAreaID = faultAreaIDLabel.Text;
                            faultname = FaultTypeDropDownList.SelectedValue;
                            if (faultname == "REWORK")
                                faultname = "2";
                            else if (faultname == "NCMR")
                                faultname = "3";

                        faultArea = FaltAreaNameLabel.Text.Trim();

                        fillChildInnerGridView(faultname, faultAreaIDLabel.Text, childGridView, recipeID, rToDate, rFromDate, "1");
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    else if (((GridView)sender).ID == "VIFaultNameChildGridView")
                    {
                        Label faultNameLabel = ((Label)e.Row.FindControl("VIfaltFaultNameLabel"));
                        faultname = faultNameLabel.Text;

                        
 
                    }
                    if (((GridView)sender).ID == "VIRecipeWiseChildGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
                    }
                    if (((GridView)sender).ID == "VIRecipeFaultWiseChildGridView")
                    {
                        e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#9BC8F0';");  //#FDCB0A
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");
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


        private void fillRecipeWiseGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {
                if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                {
                    VIRecipeWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    VIRecipeWiseGridView.DataBind();
                }
                else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
                {
                    SizeWiseRegionGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    SizeWiseRegionGridView.DataBind();
                }
                else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
                {
                    VIFaultWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    VIFaultWiseGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void fillChildGridView(GridView childgridview, string query)
        {
            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        private void fillChildInnerGridView(string faltType, string faltArea, GridView childGridView, string recipecode, String toDate, String fromDate, String option)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {
                toDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                fromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                fromDate = formatfromDate(fromDate.Replace(" 07:00:00", ""));
                            
                if (childGridView.ID == "VIRecipeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_TBRVIReportRecipeWise_Nos", faltType, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }

                else if (childGridView.ID == "VIRecipeFaultWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_TBRVIReportRecipeFaultWise_Nos", faltType, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }
                else if (childGridView.ID == "VIFaultWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillFaltArea(faltType, faltArea, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }
                else if (childGridView.ID == "VIFaultNameChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillFaltName(faltType, faltArea, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }


            }

            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        
        private DataTable fillFaltArea(string faltType, string faltArea, string recipeCode, string rTodate, string rFromDate, ConnectionOption option)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("defectAreaID", typeof(int));
            dt.Columns.Add("FaultAreaName", typeof(string));
            string defectArea = FaultAreaDropDownList.SelectedValue.ToString();
            int defectID=0;

            switch (defectArea)
            {
                case  "Tread":
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

            //myConnection.open(ConnectionOption.SQL);
            //myConnection.comm = myConnection.conn.CreateCommand();
            //string str = null;
            if (FaultAreaDropDownList.SelectedValue == "All")
            {
                dt.Rows.Add(1, "Tread");
                dt.Rows.Add(2, "SideWall");
                dt.Rows.Add(3, "Bead");
                dt.Rows.Add(4, "Carcass");
                dt.Rows.Add(5, "Others");
 
            }
            // str = "select distinct defectAreaID ,defectName as FaultAreaName from vTBRVisualInspectionReport where status='" + faltType + "' and CuringRecipeID='" + recipeCode + "'  and dtandTime>='" + rTodate + "' and dtandTime<='" + rFromDate + "' AND wcName IN ('7001','7002')";
            else
            {
                // str = "select distinct defectAreaID ,defectName as FaultAreaName from vTBRVisualInspectionReport where status='" + faltType + "' and CuringRecipeID='" + recipeCode + "'  and defectAreaID='" + defectID + "' and  dtandTime>='" + rTodate + "' and dtandTime<='" + rFromDate + "' AND wcName IN ('7001','7002')";

                dt.Rows.Add(defectID, defectArea);
            }
            //myConnection.comm.CommandText = str;
            //System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(myConnection.comm.CommandText, myConnection.conn);
            //DataSet ds = new DataSet();
           // da.Fill(ds, "dt");

            return dt;


        }
        private DataSet fillFaltName(string faltType, string faltArea, string recipeCode, string rTodate, string rFromDate, ConnectionOption option)
        {
            DataTable dt = new DataTable();
            rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
            
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select distinct defectName as faultName from  vTBRVisualInspectionReport  where status='" + faltType + "' and CuringRecipeID='" + recipeCode + "' and defectAreaID='" + faltArea + "'  and ((dtandTime>=" + rToDate + " AND wcName IN ('7001','7002')";

            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(myConnection.comm.CommandText, myConnection.conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "dt");

            myConnection.comm.Dispose();
            
            myConnection.close(ConnectionOption.SQL);

           
            return ds;


        }


        public int totalcheckedcount = 0, okcount = 0, reworkcount = 0, ncmrcount = 0,totalrework = 0, treadfault = 0, sidewallfault = 0,beadfault = 0, carcassfault = 0, othersfault = 0;
        public DataTable fillGridView(string procedureName, string faltType, string recipeCode, string rToDate, string rFromDate, ConnectionOption option)
        {
            DataTable flag = new DataTable();
            if (getdisplaytype == "Percent")
                percent_sign = "%";
            //string type = "TBR";
            
            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01

            if (option == ConnectionOption.SQL)
            {
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter machineNameParameter = null;
                    System.Data.SqlClient.SqlParameter displayType = new System.Data.SqlClient.SqlParameter("@displayType", System.Data.SqlDbType.VarChar);
                    displayType.Direction = System.Data.ParameterDirection.Input;
                    displayType.Value = getdisplaytype;
                    
                    if (procedureName == "sp_TBRVIReportRecipeFaultWise_Nos")
                    {
                        
                        machineNameParameter = new System.Data.SqlClient.SqlParameter("@faultType", System.Data.SqlDbType.VarChar);
                        machineNameParameter.Direction = System.Data.ParameterDirection.Input;
                        machineNameParameter.Value = faltType;
                    }
                    
                    System.Data.SqlClient.SqlParameter tyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    tyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    tyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter toDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    toDateParameter.Direction = System.Data.ParameterDirection.Input;
                    toDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter fromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    fromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    fromDateParameter.Value = rFromDate;

                    //System.Data.SqlClient.SqlParameter reportType = new System.Data.SqlClient.SqlParameter("@reportType", System.Data.SqlDbType.VarChar);
                    //reportType.Direction = System.Data.ParameterDirection.Input;
                    //reportType.Value = type;

                    myConnection.comm.Parameters.Add(displayType);
                    if (procedureName == "sp_TBRVIReportRecipeFaultWise_Nos")
                    {
                        myConnection.comm.Parameters.Add(machineNameParameter);
                    }
                    myConnection.comm.Parameters.Add(tyreTypeParameter);
                    myConnection.comm.Parameters.Add(toDateParameter);
                    myConnection.comm.Parameters.Add(fromDateParameter);
                    

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter nmachineNameParameter = null;
                    System.Data.SqlClient.SqlParameter ndisplayType = new System.Data.SqlClient.SqlParameter("@displayType", System.Data.SqlDbType.VarChar);
                    ndisplayType.Direction = System.Data.ParameterDirection.Input;
                    ndisplayType.Value = "Numbers";

                    if (procedureName == "sp_TBRVIReportRecipeFaultWise_Nos")
                    {
                        nmachineNameParameter = new System.Data.SqlClient.SqlParameter("@faultType", System.Data.SqlDbType.VarChar);
                        nmachineNameParameter.Direction = System.Data.ParameterDirection.Input;
                        nmachineNameParameter.Value = faltType;
                    }
                  
                    System.Data.SqlClient.SqlParameter ntyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    ntyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    ntyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter ntoDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    ntoDateParameter.Direction = System.Data.ParameterDirection.Input;
                    ntoDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter nfromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    nfromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    nfromDateParameter.Value = rFromDate;

                    //System.Data.SqlClient.SqlParameter nreportType = new System.Data.SqlClient.SqlParameter("@reportType", System.Data.SqlDbType.VarChar);
                    //nreportType.Direction = System.Data.ParameterDirection.Input;
                    //nreportType.Value = type;

                    myConnection.comm.Parameters.Add(ndisplayType);
                    if (procedureName == "sp_TBRVIReportRecipeFaultWise_Nos")
                    {
                        myConnection.comm.Parameters.Add(nmachineNameParameter);
                    }
                    myConnection.comm.Parameters.Add(ntyreTypeParameter);
                    myConnection.comm.Parameters.Add(ntoDateParameter);
                    myConnection.comm.Parameters.Add(nfromDateParameter);
                    

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    if (myConnection.reader.HasRows)
                    {
                        myConnection.reader.Read();
                        totalcheckedcount += Convert.ToInt32(myConnection.reader["TotalChecked"].ToString());

                        if (procedureName == "sp_TBRVIReportRecipeWise_Nos")
                        {
                            okcount += Convert.ToInt32(myConnection.reader["TotalOK"].ToString());
                            reworkcount += Convert.ToInt32(myConnection.reader["TotalRepair"].ToString());
                            ncmrcount += Convert.ToInt32(myConnection.reader["TotalNCMR"].ToString());
                        }
                        else if (procedureName == "sp_TBRVIReportRecipeFaultWise_Nos")
                        {
                            totalrework += Convert.ToInt32(myConnection.reader["TotalRework"].ToString());
                            treadfault += Convert.ToInt32(myConnection.reader["TreadFault"].ToString());
                            sidewallfault += Convert.ToInt32(myConnection.reader["SideWallFault"].ToString());
                            beadfault += Convert.ToInt32(myConnection.reader["Beadfault"].ToString());
                            carcassfault += Convert.ToInt32(myConnection.reader["Carcassfault"].ToString());
                            othersfault += Convert.ToInt32(myConnection.reader["Othersfault"].ToString());
                        }
                    }
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
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

            return flag;
        }





        public string formattoDate(String date)
        {
            string flag = "";
            if (date != null)
            {
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
        public string formatfromDate(String date)
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
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (Convert.ToInt32(month) == 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-01-" + (Convert.ToInt32(year) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    else if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
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
            }
            return flag;
        }



        #endregion
        protected void FaultTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
            
            if (FaultTypeDropDownList.SelectedValue == "REWORK")
                status = 2;
            if (FaultTypeDropDownList.SelectedValue == "NCMR")
                status = 3;


            if (((DropDownList)sender).ID == "FaultTypeDropDownList")
                FaultAreaDropDownList.SelectedIndex = 0;


            if (FaultTypeDropDownList.SelectedValue == "Select" && FaultAreaDropDownList.SelectedValue == "Select")
            {
                VIReportRecipeWiseMainPanel.Visible = true;
                VIFaultWisePanel.Visible = false;
                SizeWiseRegionPanel.Visible = false;
            }
            else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue == "Select")
            {
                VIReportRecipeWiseMainPanel.Visible = false;
                VIFaultWisePanel.Visible = false;
                SizeWiseRegionPanel.Visible = true;
            }
            else if (FaultTypeDropDownList.SelectedValue != "Select" && FaultAreaDropDownList.SelectedValue != "Select")
            {

                VIFaultWisePanel.Visible = true;
                VIReportRecipeWiseMainPanel.Visible = false;

                SizeWiseRegionPanel.Visible = false;
            }

            faultType.Text = "Total" + " " + FaultTypeDropDownList.SelectedValue;
            Label11.Text = FaultTypeDropDownList.SelectedValue + "FaultArea";

            rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
            fillRecipeWiseGridView("select distinct curingRecipeID, curingRecipeName from vTBRVisualInspectionReport where ((dtandTime>=" + rToDate + " AND wcName IN ('7001','7002')");

            getdisplaytype = displayType.SelectedItem.ToString();
        }
        public int tyreCount = 0;
        public int TyreQuantity(Object FaultName)
        {
            int flag = 0;
            rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
            faultname = FaultTypeDropDownList.SelectedValue;

            if (faultname == "REWORK")
                faultname = "2";
            else if (faultname == "NCMR")
                faultname = "3";
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "select distinct count(*) from  vTBRVisualInspectionReport  where defectName='" + FaultName + "' and  CuringRecipeID='" + recipeID + "' and status='" + faultname + "' and defectAreaID='" + faultAreaID + "' and ((dtandTime>=" + rToDate + " AND wcName IN ('7001','7002')";

            myConnection.reader = myConnection.comm.ExecuteReader();
            while (myConnection.reader.Read())
            {
                if (DBNull.Value != (myConnection.reader[0]))
                {
                    flag = Convert.ToInt32(myConnection.reader[0]);
                    tyreCount += flag;

                }
                else
                    flag = 0;
            }

            myConnection.conn.Close();
            myConnection.comm.Dispose();
            myConnection.reader.Close();
            return flag;

        }
        public string getdisplaytype = "Numbers";
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            getdisplaytype = displayType.SelectedItem.ToString();
            showdata();
        }
        protected void VIRecipeWiseTotalMinor_Click(object sender, EventArgs e)
        {
            backDiv.Visible = true;
            dialogPanel.Visible = true;
            emptyMsg.Visible = false;
            totalcheckedcount = Convert.ToInt32(totalCheckedCountLabel.Text);
            okcount = Convert.ToInt32(okcountLabel.Text);
            reworkcount = Convert.ToInt32(reworkcountLabel.Text);
            ncmrcount = Convert.ToInt32(ncmrcountLabel.Text);
            if (((LinkButton)sender).ID == "VIRecipeWiseTotalMinorLink")
            {
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((LinkButton)sender).Parent).Parent;
                string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("VISizeWiseTyreTypeInnerRecipeCodeLabel")).Text);

                fillBarCodeDetailGridView(recipeCode.ToString(),"3");
            }
            else if (((LinkButton)sender).ID == "VIRecipeWiseNotOk")
            {
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((LinkButton)sender).Parent).Parent;
                string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("VISizeWiseTyreTypeInnerRecipeCodeLabel")).Text);

                fillBarCodeDetailGridView(recipeCode.ToString(), "2");
            }
            else if (((LinkButton)sender).ID == "VIRecipeWiseNotOkTotal")
            {
                fillBarCodeDetailGridView("Total", "2");
            }
            else if (((LinkButton)sender).ID == "VIRecipeWiseTotalMinorLinkTotal")
            {
                fillBarCodeDetailGridView("Total", "3");
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
                    else
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
        private string TotalprodataformatDate(String fromDate, String toDate)
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
                        flag2 = "01-01" + "-" + (Convert.ToInt32(tyear) + 1).ToString() + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else if (DateTime.DaysInMonth(Convert.ToInt32(tyear), Convert.ToInt32(tday)) != Convert.ToInt32(tmonth))
                    {
                        flag2 = tday + "-" + (Convert.ToInt32(tmonth) + 1).ToString() + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag2 = (Convert.ToInt32(tday) + 1).ToString() + "-" + "01" + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    //flag2 = tday + "-" + tmonth + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";


                    flag = "'" + flag1 + "' " + "and" + " " + "TestTime<'" + flag2 + "' ))";
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

                curdt.Columns.Add("wcName", typeof(string));
                curdt.Columns.Add("mouldNo", typeof(string));
                curdt.Columns.Add("gtbarCode", typeof(string));
                tbmdt.Columns.Add("wcName", typeof(string));
                tbmdt.Columns.Add("gtbarCode", typeof(string));
                

                dt.Columns.Add("wcname", typeof(string));
                dt.Columns.Add("description", typeof(string));
                dt.Columns.Add("gtbarcode", typeof(string));
                
                gridviewdt.Columns.Add("tbmWCName", typeof(string));
                gridviewdt.Columns.Add("curingWCName", typeof(string));
                gridviewdt.Columns.Add("mouldName", typeof(string));
                gridviewdt.Columns.Add("visualWCName", typeof(string));
                gridviewdt.Columns.Add("size", typeof(string));
                gridviewdt.Columns.Add("barcode", typeof(string));
                rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
                if (recipecode != "Total")
                    recipecode = " AND curingRecipeName='" + recipecode + "'";
                else
                    recipecode = "";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select wcname, description, gtbarCode from vTBRVisualInspectionReport where status='" + status + "' AND wcName IN ('7001','7002') AND ((dtandTime>=" + rToDate + recipecode;
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(myConnection.reader);

                myConnection.conn.Close();
                myConnection.comm.Dispose();
                myConnection.reader.Close();

                if (dt.Rows.Count != 0)
                {
                    string InQuery = "(";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        InQuery += "'" + dt.Select()[i][2].ToString() + "',";
                    }
                    InQuery = InQuery.TrimEnd(',');
                    InQuery += ")";

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT wcName, mouldNo, gtbarCode FROM vCuringtbr WHERE gtbarCode IN " + InQuery.ToString(), con);
                    var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    curdt.Load(dread);

                    con.Close();
                    cmd.Dispose();
                    dread.Close();

                    SqlConnection con1 = new SqlConnection();
                    con1.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                    con1.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT wcName, gtbarCode FROM vTbmTBR WHERE gtbarCode IN " + InQuery.ToString(), con1);
                    var dread1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);
                    tbmdt.Load(dread1);

                    con1.Close();
                    cmd1.Dispose();
                    dread1.Close();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = gridviewdt.NewRow();
                        dr[3] = dt.Select()[i][0].ToString();
                        dr[4] = dt.Select()[i][1].ToString();
                        dr[5] = dt.Select()[i][2].ToString();

                        for (int j = 0; j < curdt.Rows.Count; j++)
                        {
                            try
                            {
                                dr[1] = curdt.Select("gtbarCode='" + dt.Rows[i][2].ToString() + "'")[j][0].ToString();
                                dr[2] = curdt.Select("gtbarCode='" + dt.Rows[i][2].ToString() + "'")[j][1].ToString();
                            }
                            catch (Exception e) { }
                        }
                        for (int j = 0; j < tbmdt.Rows.Count; j++)
                        {
                            try
                            {
                                dr[0] = tbmdt.Select("gtbarCode='" + dt.Rows[i][2].ToString() + "'")[j][0].ToString();
                            }
                            catch (Exception e) { }
                        }
                        gridviewdt.Rows.Add(dr);

                    }

                    performanceReportBarcodeDetailGridView.DataSource = gridviewdt;
                    performanceReportBarcodeDetailGridView.DataBind();
                    if (performanceReportBarcodeDetailGridView.Rows.Count == 0)
                        emptyMsg.Visible = true;
                }
                totalCheckedCountLabel.Text = totalcheckedcount.ToString();
                okcountLabel.Text = okcount.ToString();
                reworkcountLabel.Text = reworkcount.ToString();
                ncmrcountLabel.Text = ncmrcount.ToString();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
            showdata();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
        
        protected void expToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable curdt = new DataTable();
                DataTable gridviewdt = new DataTable();

                gridviewdt.Columns.Add("SNo", typeof(string));
                gridviewdt.Columns.Add("PressDate", typeof(string));
                gridviewdt.Columns.Add("PressTime", typeof(string));
                gridviewdt.Columns.Add("Shift", typeof(string));
                gridviewdt.Columns.Add("TyreSize", typeof(string));
                gridviewdt.Columns.Add("PressNo", typeof(string));
                gridviewdt.Columns.Add("Cavity", typeof(string));
                gridviewdt.Columns.Add("MouldNo", typeof(string));
                gridviewdt.Columns.Add("Side", typeof(string));
                gridviewdt.Columns.Add("BuildingDate", typeof(string));
                gridviewdt.Columns.Add("BuildingTime", typeof(string));
                gridviewdt.Columns.Add("BuilderName", typeof(string));
                gridviewdt.Columns.Add("Barcode", typeof(string));
                gridviewdt.Columns.Add("DefectLocation", typeof(string));
                gridviewdt.Columns.Add("Defect", typeof(string));
                gridviewdt.Columns.Add("Disposal", typeof(string));
                gridviewdt.Columns.Add("Remark", typeof(string));
                gridviewdt.Columns.Add("Responsibility", typeof(string));
                
                rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                TimeSpan ts = DateTime.Parse(rFromDate) - DateTime.Parse(rToDate);
                int result = (int)ts.TotalDays;

                if ((int)ts.TotalDays > 3)
                {
                    ShowWarning.Visible = true;
                    ShowWarning.Text = "<table style=\"padding:4px;\"><tr><td width=20%><img src='../images/exclamation.png' height=\"30\" /></td><td width=80%><strong> <font color=#9F6000>Select Data between 3 days!!!</font></strong></td></tr></table>";
                }
                else
                {
                    rFromDate = formatfromDate(rFromDate.Replace(" 07:00:00", ""));

                    string query = "select description, wcName, CAST(dtandTime AS DATE) AS getdate, convert(char(8), dtandTime, 108) AS gettime, firstName + ' ' + lastName As builderName, gtbarcode, defectAreaName, defectName, remarks, ssORnss=(CASE WHEN ssORnss='1' THEN 'SS' WHEN ssORnss='2' THEN 'NSS' WHEN ssORnss='0' THEN '' END), shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END) from vTBRVisualInspectionReport where wcName = '7003' AND dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "'";

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = query;

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    dt.Load(myConnection.reader);
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.conn.Close();

                    ExcelLabel.Text = "<table border=1>";
                    if (dt.Rows.Count != 0)
                    {
                        string InQuery = "(";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            InQuery += "'" + dt.Select()[i][5].ToString() + "',";
                        }
                        InQuery = InQuery.TrimEnd(',');
                        InQuery += ")";

                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                        con.Open();

                        SqlCommand cmd = new SqlCommand("SELECT wcName, mouldNo, gtbarCode, CAST(dtandTime AS DATE) AS getdate, convert(char(8), dtandTime, 108) AS gettime FROM vCuringtbr WHERE gtbarCode IN " + InQuery.ToString(), con);
                        cmd.CommandTimeout = 0;
                        var dread = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        curdt.Load(dread);

                        con.Close();
                        cmd.Dispose();
                        dread.Close();

                        ExcelLabel.Text += "<tr style=\"background-color:#FFFF00;\"><th>S. No.</th><th>Press Date</th><th>Press Time</th><th>Shift</th><th>TyreSize</th><th>Press No.</th><th>Cavity</th><th>Mould No.</th><th>Side</th><th>Building Date</th><th>Building Time</th><th>Builder Name</th><th>Barcode</th><th>Defect Location</th><th>Defect</th><th>Disposal</th><th>Remark</th><th>Responsibility</th></tr>";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = gridviewdt.NewRow();
                            ExcelLabel.Text += "<tr><td>" + (i + 1) + "</td>";

                            bool flag = false;
                            for (int j = 0; j < curdt.Rows.Count; j++)
                            {
                                try
                                {
                                    ExcelLabel.Text += "<td>" + curdt.Select("gtbarCode='" + dt.Rows[i][5].ToString() + "'")[j][3].ToString() + "</td>";
                                    flag = true;
                                }
                                catch (Exception ex)
                                {
                                    if (!flag && (curdt.Rows.Count - 1) == j)
                                    { ExcelLabel.Text += "<td></td>"; flag = false; }
                                }
                                try
                                {
                                    ExcelLabel.Text += "<td>" + curdt.Select("gtbarCode='" + dt.Rows[i][5].ToString() + "'")[j][4].ToString() + "</td>";
                                    flag = true;
                                }
                                catch (Exception ex)
                                {
                                    if (!flag && (curdt.Rows.Count - 1) == j)
                                    { ExcelLabel.Text += "<td></td>"; flag = false; }
                                }

                            }
                            ExcelLabel.Text += "<td>" + dt.Rows[i][10] + "</td><td>" + dt.Rows[i][0] + "</td>";
                            flag = false;
                            for (int j = 0; j < curdt.Rows.Count; j++)
                            {
                                try
                                {
                                    ExcelLabel.Text += "<td>" + curdt.Select("gtbarCode='" + dt.Rows[i][5].ToString() + "'")[j][0].ToString() + "</td><td></td>";
                                }
                                catch (Exception ex)
                                { }

                                try
                                {
                                    ExcelLabel.Text += "<td>" + curdt.Select("gtbarCode='" + dt.Rows[i][5].ToString() + "'")[j][1].ToString() + "</td>";
                                    flag = true;
                                }
                                catch (Exception ex)
                                {
                                    if (!flag && (curdt.Rows.Count - 1) == j)
                                    { ExcelLabel.Text += "<td></td><td></td><td></td>"; flag = false; }
                                }

                            }
                            ExcelLabel.Text += "<td>" + dt.Rows[i][9] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][2] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][3] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][4] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][5] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][6] + "</td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][7] + "</td>";
                            ExcelLabel.Text += "<td></td>";
                            ExcelLabel.Text += "<td>" + dt.Rows[i][8] + "</td>";
                            ExcelLabel.Text += "<td></td>";
                            ExcelLabel.Text += "</tr>";
                        }
                        ExcelLabel.Text += "</table>";
                    }
                    ExcelPanel.Visible = true;
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=FirstTBRVisualInspectionReport.xls");
                    Response.ContentType = "application/vnd.ms-excel";

                    StringWriter stringWrite = new StringWriter();
                    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                    ExcelPanel.RenderControl(htmlWrite);

                    Response.Write(stringWrite.ToString());

                    Response.Flush();
                    Response.End();
                    ExcelPanel.Visible = false;
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public void showdata()
        {
            rToDate = TotaldtformatDate(tuoReportMasterFromDateTextBox.Text, tuoReportMasterToDateTextBox.Text);
            fillRecipeWiseGridView("select Distinct curingRecipeID, curingRecipeName from vTBRVisualInspectionReport where ((dtandTime>=" + rToDate +" AND wcName IN ('7001','7002')");

            if (getdisplaytype == "Percent")
            {
                if (totalcheckedcount != 0)
                {
                    okcount = (okcount * 100) / totalcheckedcount;
                    reworkcount = (reworkcount * 100) / totalcheckedcount;
                    ncmrcount = (ncmrcount * 100) / totalcheckedcount;
                }
            }
            
            totalCheckedCountLabel.Text = totalcheckedcount.ToString();
            okcountLabel.Text = okcount.ToString();
            reworkcountLabel.Text = reworkcount.ToString();
            ncmrcountLabel.Text = ncmrcount.ToString();
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
    }
}
