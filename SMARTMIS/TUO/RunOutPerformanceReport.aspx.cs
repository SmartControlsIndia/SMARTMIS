using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Globalization;

namespace SmartMIS.TUO
{
    public partial class RunOutPerformanceReport : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option="1", workcentername, wcnamequery, wcIDQuery, machinenamequery;
        string dtnadtime1 = "";
        string query = "";
        string[] tempString2;


        #endregion

        #region System Defined Function
               
        protected void Page_Load(object sender, EventArgs e)
        {
            rToDate = formattoDate(myWebService.formatDate(reportMasterDateWiseTextBox.Text));
            rFromDate = formatfromDate(myWebService.formatDate(reportMasterDateWiseTextBox.Text));

            rToMonth = month.SelectedItem.Value;
            rToYear = year.SelectedValue;
            try
            {
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {


                    reportHeader._rDate = reportMasterDateWiseTextBox.Text.ToString();

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }


        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (!string.IsNullOrEmpty(query))
                wcnamequery = wcquery(query);
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {



                    if (((GridView)sender).ID == "RunOutRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("RunOutSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("RunOutRecipeWiseChildGridView"));

                        fillChildInnerGridView("3401", childGridView, recipeCodeLabel.Text.Trim(), rToDate, rFromDate, "1");
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

                RunOutRecipeWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                RunOutRecipeWiseGridView.DataBind();
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
                if (rType == "dayWise")

                {
                    string dtnadtime = TotalformatDate(rToDate);

                    if (childGridView.ID == "RunOutRecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_RunOutReportRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                    }

                }


                if (rType == "monthWise")
                {

                    if (childGridView.ID == "RunOutRecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_RunOutReportMonthRecipeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                    }

                }


            }










            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public DataTable fillGridView(string procedureName, string wcName, string recipeCode, string rToDate, string rFromDate, ConnectionOption option)
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
                    fromDateParameter.Value = rFromDate;

                    myConnection.comm.Parameters.Add(machineNameParameter);
                    myConnection.comm.Parameters.Add(tyreTypeParameter);
                    myConnection.comm.Parameters.Add(toDateParameter);
                    myConnection.comm.Parameters.Add(fromDateParameter);

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                    flag.Load(myConnection.reader);
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

        public int AlltotalcheckedQuantity()
        {
            int flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where dtandTime>=@todate and dtandTime<=@fromdate";



                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))

                            flag = Convert.ToInt32(myConnection.reader[0]);

                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + "";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))

                            flag = Convert.ToInt32(myConnection.reader[0]);

                        else
                            flag = 0;
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
        public Double AlltotalAQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='A'  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='A'   and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";

                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalBQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='B'  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }

                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='B'  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalCQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='C' and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);


                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")

                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='C'  and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);


                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")

                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalDQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='D'  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }

                }
                else if (rType == "monthWise")
                {

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();


                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='D' and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
        public Double AlltotalEQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='E'  and dtandTime>=@todate and dtandTime<=@fromdate";


                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
                    }
                }
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select COUNT(*) from unibalrunouttbr where RORank='E' and (datepart(MM,dtandTime)=" + rToMonth + " and datepart(YYYY,dtandTime)=" + rToYear + ")";



                    myConnection.comm.Parameters.AddWithValue("@todate", rToDate);
                    myConnection.comm.Parameters.AddWithValue("@fromdate", rFromDate);

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (DBNull.Value != (myConnection.reader[0]))
                        {
                            if (option == "1")
                                flag = Convert.ToInt32(myConnection.reader[0]);
                            else
                            {
                                flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                                flag = Math.Round(flag, 2);
                            }
                        }
                        else
                            flag = 0;
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
            }
            return flag;
        }
        public string TotalformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";
            if (date != null)
            {
                string day, month, year;

                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();

                    flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }

                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' " + ")OR" + " " + "(dtandTime>'" + flag3 + "'and" + " " + "dtandTime<" + "'" + flag4 + "'))";

                }

                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        public string TotalprodataformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";
            if (date != null)
            {
                string day, month, year;

                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();

                    flag1 = month + "-" + day + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    flag2 = month + "-" + day + "-" + year + " " + "23" + ":" + "59" + ":" + "59";
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag3 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else
                    {
                        flag3 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "00" + ":" + "00" + ":" + "00";
                        flag4 = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }

                    flag = "'" + flag1 + "' " + "and" + " " + "dtandTime<'" + flag2 + "' " + ")OR" + " " + "(dtandTime>'" + flag3 + "'and" + " " + "dtandTime<" + "'" + flag4 + "'))";
                }
                catch (Exception exp)
                {
                    myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
                        if (tempString2[tempString2.Length - 1].ToString() == "1")
                            flag = flag + "or" + " " + "machineName = '" + myConnection.reader[0] + "'";
                        else
                            flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "1")
                            flag = "machineName = '" + myConnection.reader[0] + "'";
                        else

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

        public string createQuery(String wcID)
        {
            string query = "";
            string or = "";
            string[] tempWCID = wcID.Split(new char[] { '#' });

            foreach (string items in tempWCID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "ID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
        public string createwcIDQuery(String wcID)
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
        #endregion

        protected void show_Click(object sender, EventArgs e)
        {
            
            rType = searchType.SelectedItem.ToString();
            if (rType == "dayWise")
            {
                rToDate = formattoDate(myWebService.formatDate(reportMasterDateWiseTextBox.Text));
                rFromDate = formatfromDate(myWebService.formatDate(reportMasterDateWiseTextBox.Text));

              //  string str = "Select DISTINCT  tyreType from unibalrunouttbr WHERE (dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "')";
                fillRecipeWiseGridView("Select DISTINCT  tyreType from unibalrunouttbr WHERE (dtandTime>'" + rToDate + "' and dtandTime<='" + rFromDate + "')");

            }
            else if (rType == "monthWise")
            {

                rToMonth = month.SelectedItem.Value;
                rToYear = year.SelectedValue;
                fillRecipeWiseGridView("Select DISTINCT  tyreType from unibalrunouttbr WHERE (datepart(MM,dtandTime)='" + rToMonth + "' and datepart(YYYY,dtandTime)='" + rToYear + "')");


            }
        }

        protected void searchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            rType = searchType.SelectedItem.ToString();

            if (rType == "dayWise")
            {
                reportMasterDateWiseTextBox.Visible = true;
                reportMasterDateWiseTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                monthlypanel.Visible = false;
            }
            else if (rType == "monthWise")
            {
                reportMasterDateWiseTextBox.Visible = false;

                string mon = DateTime.Now.Month.ToString();

                DateTimeFormatInfo dinfo = new DateTimeFormatInfo();
                month.SelectedItem.Text = dinfo.GetMonthName(Convert.ToInt16(mon));
                year.SelectedValue = DateTime.Now.Year.ToString();
                monthlypanel.Visible = true;
            }
        }
    }
}
