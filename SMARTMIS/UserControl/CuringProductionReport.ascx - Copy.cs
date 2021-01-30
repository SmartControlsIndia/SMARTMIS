using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;

namespace SmartMIS.UserControl
{
    public partial class CuringProductionReport : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable dt = new DataTable();
        DataTable uniqrecipedt = new DataTable();
        

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, rprocessID,wcName,processName, size, operatorID, query;

        public int actualQuan, planningQuan, totalPlanningQuan, totalActualQuan, shiftcountA, shiftcountB, shiftcountC, shiftcountTotal;

        public String[] workcenter;

        public String Visiblity
        {

            get
            {
                return curingProductionReportDateWisePanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                curingProductionReportDateWisePanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorMsg.Visible = false;
                                
                string temp;
                string[] tempString = magicHidden.Value.Split(new char[] { '?' });

                temp = magicHidden.Value;
                if (string.IsNullOrEmpty(magicHidden.Value))
                {

                    temp = queryStringSave.Text;
                    tempString = temp.Split(new char[] { '?' });
                }
                else
                    queryStringSave.Text = temp;  // To be used when dropdown changed


                curingProductionReportDateWiseGridView.DataSource = null;
                curingProductionReportDateWiseGridView.DataBind();
                //size = CuringProductionReportSizeDropdownlist.SelectedValue.ToString();
                //operatorID = CuringProductionReportOperatorDropdownlist.SelectedValue.ToString();

                shiftcountA = 0; shiftcountB = 0; shiftcountC = 0; shiftcountTotal = 0;

                //Compare the hidden field if it contains the query string or not

                dt.Columns.Add("recipecode", typeof(string));
                dt.Columns.Add("description", typeof(string));
                dt.Columns.Add("shift", typeof(string));

                if (tempString.Length > 1)
                {
                    rType = tempString[0];
                    rWCID = tempString[1];
                    rChoice = tempString[2];
                    rToDate = tempString[3];
                    rFromDate = tempString[4];
                    size = tempString[5];
                    operatorID = tempString[6];
                    rFromYear = tempString[7];
                    if (tempString[9] == "0")
                        processName = "Curing PCR";
                    else if (tempString[9] == "1")
                        processName = "Curing TBR";

                    //  Compare which type of report user had selected//
                    //
                    //  Plant wide = 0
                    //  Workcenter wide = 1
                    //


                    if (rType == "0")
                    {
                    }
                    else if (rType == "1")
                    {
                        //  Compare choice of report user had selected//
                        //
                        //  Daily = 0
                        //  Monthly = 1
                        //  Yearly  = 2
                        //

                        if (rChoice == "0")
                        {
                            rFromDate = formatDate(myWebService.formatDate(rFromDate));
                            rToDate = formatDate(myWebService.formatDate(tempString[3]));
                            query = createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
                            
                            showDailyReport(query);
                            magicHidden.Value = "";
                        }
                        else if (rChoice == "1")
                        {
                            rToMonth = tempString[5];
                            rToYear = tempString[6];
                           
                            query = createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
                            showMonthlyReport(query);

                        }
                        else if (rChoice == "2")
                        {
                        }

                    }
                    else if (rType == "2")
                    {

                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //showDailyReport(query);
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
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "curingProductionReportDateWiseGridView")
                    {
                         wcName = (((Label)e.Row.FindControl("curingProductionReportDateWiseWCNameLabel")).Text);
                         GridView CuringProductionchildGridView = ((GridView)e.Row.FindControl("curingProductionReportDateWiseChildGridView"));
                                                
                        fillChildGridView(CuringProductionchildGridView, new String[] { wcName }, rToDate, rFromDate);
                    }
                   
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void showDailyReport(string query)
        {
            try
            {
                shiftcountA = 0; shiftcountB = 0; shiftcountC = 0; shiftcountTotal = 0;

                if (query == "(wcID = '0')" && !string.IsNullOrEmpty(magicHidden.Value))
                {
                    ErrorMsg.Visible = true;
                    ErrorMsg.Text = "<b>Select WorkCenters!!</b><BR><BR><Center><input type=\"button\" onClick=\"closePopup()\" value=\"  OK  \" class=\"popupBut\" /></Center>";
                }
                else
                {
                    string remainingQuery = "";
                    if (size != "All")
                        remainingQuery = " AND description='" + size + "'";

                    if (operatorID != "All")
                        remainingQuery += " AND manningID='" + operatorID + "'";

                    fillGridView("Select DISTINCT wcID, wcName from vCuringProduction WHERE " + query + remainingQuery + " ORDER BY wcName ASC");
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void showMonthlyReport(string query)
        {
            try
            {
                fillGridView("Select DISTINCT wcID, wcName from vCuringProduction WHERE " + query + " ORDER BY wcName ASC");

                //curingProductionReportTotalGridView.DataSource = fillInnerGridView("sp_curingProductionReportTotalMonthWise", processName, "3333", rToMonth,rToYear, ConnectionOption.SQL);
                //curingProductionReportTotalGridView.DataBind();
            }
            catch (Exception ex)
            {

            }

 
        }

        private void fillGridView(string query)
        {
                    
            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Rohit Singh
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011
            //Revision No.  : 01z
            try
            {
                if (rType == "1" && rChoice == "0")
                {

                    curingProductionReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    curingProductionReportDateWiseGridView.DataBind();
                }

                else if (rType == "1" && rChoice == "1")
                {
                    curingProductionReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    curingProductionReportDateWiseGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private void fillChildGridView(GridView childGridView, String[] args, String toDate, String fromDate)
        {
            string query="";
            
            try
            {
                string remainingQuery = "";
                if (size != "All")
                    remainingQuery = " AND description='" + size + "'";

                if (operatorID != "All")
                    remainingQuery += " AND manningID='" + operatorID + "'";

                DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("recipecode", typeof(string));
                gridviewdt.Columns.Add("description", typeof(string));
                gridviewdt.Columns.Add("AshiftCount", typeof(string));
                gridviewdt.Columns.Add("BshiftCount", typeof(string));
                gridviewdt.Columns.Add("CshiftCount", typeof(string));
                gridviewdt.Columns.Add("Totalcount", typeof(string));
                dt.Clear();

                query = "select recipecode,description,quantity,shift from vCuringProduction where wcname='" + args[0] + "' AND convert(varchar(10), dtandtime,120)>='" + formatDate(rToDate) + "' AND convert(varchar(10), dtandtime,120)<='" + formatDate(rFromDate) + "'" + remainingQuery;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = query;

                myConnection.reader = myConnection.comm.ExecuteReader();
                dt.Load(myConnection.reader);
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();

                uniqrecipedt = GetDistinctRecords(dt, "recipecode");
                int A=0, B=0, C=0;

                for (int i = 0; i < uniqrecipedt.Rows.Count; i++)
                {
                    DataRow dr = gridviewdt.NewRow();
                    try
                    {
                        dr[0] = dt.Select("recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "'")[0][0].ToString();
                    }
                    catch (Exception ex) { }
                    try
                    {
                        dr[1] = dt.Select("recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "'")[0][1].ToString();
                    }
                    catch (Exception ex) { }
                    dr[2] = dt.Compute("Sum(quantity)", "recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  shift='A'");
                    dr[3] = dt.Compute("Sum(quantity)", "recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  shift='B'");
                    dr[4] = dt.Compute("Sum(quantity)", "recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "' AND  shift='C'");
                    dr[5] = dt.Compute("Sum(quantity)", "recipecode='" + uniqrecipedt.Rows[i][0].ToString() + "'");

                    if (!string.IsNullOrEmpty(dr[2].ToString()))
                    {
                        A = Convert.ToInt32(dr[2]);
                        shiftcountA += A;
                    }
                    if (!string.IsNullOrEmpty(dr[3].ToString()))
                    {
                        B = Convert.ToInt32(dr[3]);
                        shiftcountB += B;
                    }
                    if (!string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        C = Convert.ToInt32(dr[4]);
                        shiftcountC += C;
                    }
                    gridviewdt.Rows.Add(dr);
                                       
                }
                
                childGridView.DataSource = gridviewdt;
                childGridView.DataBind();
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }
        public int shiftAcount = 0, shiftBcount = 0, shiftCcount=0;
        public DataTable fillInnerGridView(string procedureName, string wcName, string recipeCode, string rToDate, string rFromdate, ConnectionOption option)
        {
            DataTable flag = new DataTable();

            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01

            if (option == ConnectionOption.SQL)
            {
                try
                {
                    //string query = null;
                    //if (rChoice == "0")
                    //    query = "select quantity, shift from vCuringProduction where wcname='" + wcName + "' and convert(varchar(10), dtandtime,110)='" + rToDate + "' ";
                    //else if (rChoice == "1")
                    //    query = "select quantity, shift from vCuringProduction where wcname='" + wcName + "' AND recipecode='"+recipeCode+"' and datepart(MM,dtandTime)='" + rToMonth + "' and datepart(YYYY,dtandTime)='" + rToYear + "' ";

                    ////childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    ////childgridview.DataBind();

                    //myConnection.open(ConnectionOption.SQL);
                    //myConnection.comm = myConnection.conn.CreateCommand();

                    //myConnection.comm.CommandText = query;

                    //myConnection.reader = myConnection.comm.ExecuteReader();
                    //if (myConnection.reader.HasRows)
                    //{
                    //    while (myConnection.reader.Read())
                    //    {
                    //        if (myConnection.reader["shift"].ToString().Equals("A"))
                    //            shiftAcount = myConnection.reader["quantity"].ToString();
                    //        if (myConnection.reader["shift"].ToString().Equals("B"))
                    //            shiftBcount = myConnection.reader["quantity"].ToString();
                    //        if (myConnection.reader["shift"].ToString().Equals("C"))
                    //            shiftCcount = myConnection.reader["quantity"].ToString();
                    //    }
                    //}

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = procedureName;
                    myConnection.comm.CommandType = CommandType.StoredProcedure;

                    System.Data.SqlClient.SqlParameter machineNameParameter = new System.Data.SqlClient.SqlParameter("@wcname", System.Data.SqlDbType.VarChar);
                    machineNameParameter.Direction = System.Data.ParameterDirection.Input;
                    machineNameParameter.Value = wcName;

                    System.Data.SqlClient.SqlParameter tyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
                    tyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    tyreTypeParameter.Value = recipeCode;

                    System.Data.SqlClient.SqlParameter toDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    toDateParameter.Direction = System.Data.ParameterDirection.Input;
                    toDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter fromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    fromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    fromDateParameter.Value = rFromdate;

                    myConnection.comm.Parameters.Add(machineNameParameter);
                    myConnection.comm.Parameters.Add(tyreTypeParameter);
                    myConnection.comm.Parameters.Add(toDateParameter);
                    myConnection.comm.Parameters.Add(fromDateParameter);

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);

                    if(!DBNull.Value.Equals(flag.Rows[0][0]))
                    shiftAcount += Convert.ToInt32(flag.Rows[0][0]);
                    if (!DBNull.Value.Equals(flag.Rows[0][1]))
                        shiftBcount += Convert.ToInt32(flag.Rows[0][1]);
                    if (!DBNull.Value.Equals(flag.Rows[0][2]))
                        shiftCcount += Convert.ToInt32(flag.Rows[0][2]);

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);







            //        myConnection.open(ConnectionOption.SQL);
            //        myConnection.comm = myConnection.conn.CreateCommand();

            //        myConnection.comm.CommandText = procedureName;
            //        myConnection.comm.CommandType = CommandType.StoredProcedure;

            //        System.Data.SqlClient.SqlParameter nmachineNameParameter = new System.Data.SqlClient.SqlParameter("@wcname", System.Data.SqlDbType.VarChar);
            //        nmachineNameParameter.Direction = System.Data.ParameterDirection.Input;
            //        nmachineNameParameter.Value = wcName;

            //        System.Data.SqlClient.SqlParameter ntyreTypeParameter = new System.Data.SqlClient.SqlParameter("@recipecode", System.Data.SqlDbType.VarChar);
            //        ntyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
            //        ntyreTypeParameter.Value = recipeCode;

            //        System.Data.SqlClient.SqlParameter ntoDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
            //        ntoDateParameter.Direction = System.Data.ParameterDirection.Input;
            //        ntoDateParameter.Value = rToDate;

            //        System.Data.SqlClient.SqlParameter nfromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
            //        nfromDateParameter.Direction = System.Data.ParameterDirection.Input;
            //        nfromDateParameter.Value = rFromdate;

            //        myConnection.comm.Parameters.Add(nmachineNameParameter);
            //        myConnection.comm.Parameters.Add(ntyreTypeParameter);
            //        myConnection.comm.Parameters.Add(ntoDateParameter);
            //        myConnection.comm.Parameters.Add(nfromDateParameter);

            //        myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
            //        if (myConnection.reader.HasRows)
            //        {
            //            myConnection.reader.Read();
            //                shiftAcount += Convert.ToInt32(myConnection.reader["AShiftCount"].ToString());
            //                shiftBcount += Convert.ToInt32(myConnection.reader["BShiftCount"].ToString());
            //                shiftCcount += Convert.ToInt32(myConnection.reader["CShiftCount"].ToString());
            //        }

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
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);

            flag = tempDate.ToString("yyyy-MM-dd");

            return flag;
        }
    }
}