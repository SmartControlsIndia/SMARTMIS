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
    public partial class QualityperformancereportTBMWise : System.Web.UI.UserControl
    {
        myConnection myConnection = new myConnection();
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        #endregion

        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery, wcIDQuery;
        string query = "";
        public string _rDate;

        public String ReportDate
        {
            get 
            {
                return _rDate;
            }

            set
            {
                _rDate = value;
              
            }
        }
        public String Visiblity
        {

            get
            {
                return productionReport2TBMWiseMainPanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                productionReport2TBMWiseMainPanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }


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
                    string[] tempString = magicHidden.Value.Split(new char[] { '?' });

                    //Compare the hidden field if it contains the query string or not

                   
                        //  Compare which type of report user had selected//
                        //
                        //  Plant wide = 0
                        //  Workcenter wide = 1
                        //

                    if (ReportDate != null)
                    {
                        rToDate = myWebService.formatDate(ReportDate);
                        rFromDate = formatfromDate(myWebService.formatDate(ReportDate));

                        query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
                        wcIDQuery = "(wcID = '4' Or wcID = '5' Or wcID = '6' Or wcID = '7' Or wcID = '8')";
                        wcnamequery = wcquery(query);
                        showReport(query);

                        string footergrid = "performanceReport2TBMWiseFooterGridView";
                        fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), "1");
                      
                       
                    }


                                //    viewQueryHidden.Value = "False";
                                //}


                    
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
                    if (((GridView)sender).ID == "performanceReport2TBMWiseMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReport2TBMWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();

                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMWiseGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                        

                    }


                    if (((GridView)sender).ID == "performanceReport2TBMWiseGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReport2TBMWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMWiseChildGridView"));
                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), "1");
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
               
                string dtnadtime = TotalformatDate(rToDate);

                fillChildGridView(childgridview, "Select distinct TireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND testTime>='"+formattoDate(rToDate)+"' and testTime<='"+formatfromDate(rToDate)+"'");

              
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

                performanceReport2TBMWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReport2TBMWiseMainGridView.DataBind();
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
                string dtnadtime = TotalformatDate(rToDate);

                if (childGridView.ID == "performanceReport2TBMWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReport2TBMSWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
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
        private void fillChildInnerGridView( string GridView, string recipecode, String toDate, String fromDate, String option)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {
                string dtnadtime = TotalformatDate(rToDate);
                if (GridView == "performanceReport2TBMWiseFooterGridView")
                {
                    if (option == "1")
                    {
                        performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTBMSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        //else if (option == "2")
                        //childGridView.DataSource = fillGridView("sp_PerformanceReportSizeWise_Percentage",machineName, tyreType, toDate, fromDate, ConnectionOption.SQL);

                        performanceReport2TBMWiseFooterGridView.DataBind();
                    }
 
                }
               
            }
            catch (Exception ex)
            {

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
                catch (Exception ex)
                {

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

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.ToString("MM-dd-yyyy");
            flag = flag + " " + "07:00:00";

            return flag;
        }
        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });

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
            return flag;
        }
        public string TotalformatDate(String date)
        {
            string flag = "";
            string flag1 = "";
            string flag2 = "";
            string flag3 = "";
            string flag4 = "";

            string day, month, year;

            string[] tempDate = date.Split(new char[] { '-' });

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
    }
}