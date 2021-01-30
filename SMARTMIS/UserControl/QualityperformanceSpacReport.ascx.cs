using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SmartMIS.UserControl
{
    public partial class QualityperformanceSpacReport : System.Web.UI.UserControl
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        #endregion
        myConnection myConnection = new myConnection();

        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery, parameter, spec, wcIDQuery,query;
        float value;
        #endregion

        #region System Defined Function
        protected void Page_Load(object sender, EventArgs e)
        {   
            try
            {
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    reportHeader.ReportDate = reportMasterFromDateTextBox.Text;
                              rToDate = reportMasterFromDateTextBox.Text;


                                rToDate = myWebService.formatDate(rToDate);
                                rFromDate = formatfromDate(rFromDate);


                               
                                //    viewQueryHidden.Value = "False";                     
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void magicButton_Click(object sender, EventArgs e)
        {
           
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReportTBMWiseSpecMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportTBMWiseSpecWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTBMWiseSpecGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                    }


                    if (((GridView)sender).ID == "performanceReportTBMWiseSpecGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });
                        
                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportTBMWiseSpecTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTBMWiseSpecChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), "1");
                    }
                }
            }

            catch (Exception exp)
            {
            }
        }
        #endregion

        #region User Defined Function

        public string getTyreType(string tableName, string coloumnName, string whereClause)
        {
            string flag = "";

            string sqlQuery = "";

            //Description   : Function for returning TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tableName != "vDesignMaster")
                    sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + "" + whereClause;
                else
                    sqlQuery = "Select " + coloumnName + " from " + tableName + "" + whereClause;

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (flag.Length > 0)
                        flag += "#";

                    flag += myConnection.reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }
        public ArrayList FillDropDownList(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 21 June 2012
            //Date Updated  : 21 June 2012
            //Revision No.  : 01
            flag.Add("");
            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + "" + whereClause;

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            return flag;
        }
        private void showReport(string query)
        {
            fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
        }
        public void showReport1(GridView childgridview, string wcname, string rToDate, string rFromDate)
        {
            try
            {
                //if (tuoFilterSizeDropDownList.SelectedValue == "All")
                //{
                //    fillChildGridView(childgridview,"Select  description FROM vDesignMaster WHERE Design='" + tuoFilterDesignDropDownList.SelectedValue + "'");

                //}
                //if (tuoFilterSizeDropDownList.SelectedValue == "All" && tuoFilterDesignDropDownList.SelectedValue == "All")
                //{
                //    fillChildGridView(childgridview,"Select  description FROM vDesignMaster WHERE nom='" + tuoFilterNOMDropDownList.SelectedValue + "'");

                //}
                //if (tuoFilterSizeDropDownList.SelectedValue == "All" && tuoFilterDesignDropDownList.SelectedValue == "All" && tuoFilterNOMDropDownList.SelectedValue == "All")
                //{
                string dtnadtime = TotalformatDate(rToDate,rFromDate);

                fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND testTime>='"+formattoDate(rToDate)+"' and testTime<='"+formatfromDate(rFromDate)+"'");

                //}

                //if (tuoFilterSizeDropDownList.SelectedValue != "All")
                //{
                //    fillChildGridView(childgridview,"Select  description FROM vDesignMaster WHERE size='" + tuoFilterSizeDropDownList.SelectedValue + "'");
                //}
            }
            catch (Exception ex)
            {

            }
        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceReportTBMWiseSpecMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportTBMWiseSpecMainGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void fillChildGridView(GridView childgridview, string query)
        {
            childgridview.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            childgridview.DataBind();
        }
        private void fillChildInnerGridView(string wcname, GridView childGridView, string recipecode, String toDate, String fromDate, String option)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {
                string dtnadtime = TotalformatDate(rToDate,rFromDate);

                if (childGridView.ID == "performanceReportTBMWiseSpecChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportSizeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        //else if (option == "2")
                        //childGridView.DataSource = fillGridView("sp_PerformanceReportSizeWise_Percentage",machineName, tyreType, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable fillGridView(string procedureName, string wcName, string recipeCode, string rToDate, string rFromDate,  ConnectionOption option)
        {
            DataTable flag = new DataTable();

            //Description   : Function for returning Datatable on the basis of SQL Query
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            flag.Columns.Add("checked",typeof(string));
            flag.Columns.Add("A", typeof(string));
            flag.Columns.Add("B", typeof(string));
            flag.Columns.Add("specPlus10", typeof(string));
            flag.Columns.Add("SpecPlus20", typeof(string));
            flag.Columns.Add("SpecPlus30", typeof(string));
            flag.Columns.Add("Specgreaterthan30", typeof(string));

            DataRow dr = flag.NewRow();
            dr[0] = AllcheckedQuantity(recipeCode);
            dr[1] = AllAQuantity(recipeCode);
            dr[2] = AllBQuantity(recipeCode);
            dr[3] = All10PlusQuantity(recipeCode);
            dr[4] = All20PlusQuantity(recipeCode);
            dr[5] = All30PlusQuantity(recipeCode);
            dr[6] = All30greaterQuantity(recipeCode);

            flag.Rows.Add(dr);     

            return flag;
        }
        private int AllcheckedQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and wcname= '"+workcentername+"' and tireType=@recipecode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int AllAQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {
                
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int AllBQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int All10PlusQuantity( string recipeCode )
        {
            int flag = 0;
            try
            {
               float value1 = value + 2;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where ("+parameter+">"+value +" and "+parameter+"<"+value1+") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int All20PlusQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {
                float value1 = value+ 2;
                float value2 = value + 4;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int All30PlusQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {
                float value1 =  value+4;
                float value2 = value+ 6;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        private int All30greaterQuantity(string recipeCode)
        {
            int flag = 0;
            try
            {
                float value1 = value +6;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int AlltotalcheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int AlltotalAQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int AlltotalBQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int Alltotal10PlusQuantity()
        {
            int flag = 0;
            float value1 = value +2;

            try
            {    
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));



                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int Alltotal20PlusQuantity()
        {
            int flag = 0;
            float value1 = value + 2;
            float value2 = value + 4;

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int Alltotal30PlusQuantity()
        {
            int flag = 0;
            float value1 = value + 4;
            float value2 = value + 6;

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and dtandTime>=@todate and dtandTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int Alltotal30GreaterQuantity()
        {
            int flag = 0;

            float value1 = value + 6;

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + " and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

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

                }
            }
            return flag;
        }
        public string formatfromDate(String date)
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

                }
            }
            return flag;
        }
        public string TotalformatDate(String fromDate, string toDate)
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

                    flag1 = fmonth + "-" + fday + "-" + fyear + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = tmonth + "-" + tday + "-" + tyear + " " + "07" + ":" + "00" + ":" + "00";


                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' ))";
                }
                catch (Exception exp)
                {

                }

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
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }

        #endregion

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void showreport_Click(object sender, EventArgs e)
        {
            try
            {
                query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
                wcIDQuery = "(wcID = '4' Or wcID = '5' Or wcID = '6' Or wcID = '7' Or wcID = '8')";
                wcnamequery = wcquery(query);

                parameter = tuoFilterSpecParameterDropDownList.SelectedValue;
                value = Convert.ToInt32(SpecTextBox.Text);
                showReport(query);
            }
            catch (Exception exp)
            {
              
            }

        }

    }
}