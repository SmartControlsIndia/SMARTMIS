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
    public partial class tuoFilter : System.Web.UI.UserControl
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        #endregion
        myConnection myConnection = new myConnection();
        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery;
        string dtnadtime1 = "";
        string query = "";

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

                    reportHeader._rDate = reportMasterFromDateTextBox.Text;
                   // reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    if (QualityReportTBMWise.Checked)
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
                        QualityReportTBMWisePanel.Visible = true;
                        QualityReportRecipeWisePanel.Visible = false;
                    }
                    else if(QualityReportRecipeWise.Checked)
                    {
                         QualityReportTBMWisePanel.Visible = false;
                        QualityReportRecipeWisePanel.Visible = true;

                    }

                    //Compare the hidden field if it contains the query string or not
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

            query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
            wcIDQuery = "(wcID = '4' Or wcID = '5' Or wcID = '6' Or wcID = '7' Or wcID = '8')";
            wcnamequery = wcquery(query);
            try
            {
                if (tuoFilterOptionDropDownList.SelectedItem.Text == "No")
                    option = "1";
                else
                    option = "2";
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReportSizeWiseMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportSizeWiseGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                    }


                    if (((GridView)sender).ID == "performanceReportSizeWiseGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportSizeWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);
                    }

                    if (((GridView)sender).ID == "performanceReportRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportrecipeWiseChildGridView"));

                        fillChildInnerGridView("3401", childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);
                    }

                    if (((GridView)sender).ID == "UnknownWCMainGridView")
                    {
                        GridView childgridview = ((GridView)e.Row.FindControl("performanceReportUnknownWiseGridView"));

                        fillunknownrecipewisechildgrid(childgridview);


                    }
                    if (((GridView)sender).ID == "performanceReportUnknownWiseGridView")
                    {
                        Label recipeCode = ((Label)e.Row.FindControl("performanceReportUnknownWiseTyreTypeLabel"));
                        GridView unknownchildGridView = ((GridView)e.Row.FindControl("performanceReportUnknownWiseChildGridView"));

                        fillChildInnerGridView("3401", unknownchildGridView, recipeCode.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);

                    }
                    if (((GridView)sender).ID == "UnknownRecipeMainGridView")
                    {
                        GridView childgridview = ((GridView)e.Row.FindControl("performanceReportUnknownRecipeWiseGridView"));

                        fillunknownrecipewisechildgrid(childgridview);


                    }
                    if (((GridView)sender).ID == "performanceReportUnknownRecipeWiseGridView")
                    {
                        Label recipeCode = ((Label)e.Row.FindControl("performanceReportUnknownRecipeWiseTyreTypeLabel"));
                        GridView unknownchildGridView = ((GridView)e.Row.FindControl("performanceReportUnknownRecipeWiseChildGridView"));
                        fillChildInnerGridView("3401", unknownchildGridView, recipeCode.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);

                    }
                }

            }

            catch (Exception exp)
            {
            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

           string query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
            wcnamequery = wcquery(query);

            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);

            string dtnadtime = TotalformatDate(rToDate);
           
           
            if (QualityReportTBMWise.Checked)
            {
                fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                fillUnknownWiseGridView();




            }

            else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                string todate = formattoDate(rToDate);
                string fromdate = formatfromDate(rToDate);
                fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                fillUnknownRecipeWiseGridView();


            }
            else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
            {
                tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                string todate = formattoDate(rToDate);
                string fromdate = formatfromDate(rToDate);
                fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                fillUnknownRecipeWiseGridView();


            }
            else if (((DropDownList)sender).ID == "tuoFilterOptionDropDownList")
            {

                if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {

                    query = "select distinct tireType as description from vproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                    fillRecipeWiseGridView(query);
                    fillUnknownRecipeWiseGridView();
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                    fillRecipeWiseGridView(query);
                    fillUnknownRecipeWiseGridView();
                }
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                {
                    showReportRecipeWise(wcnamequery);
                    fillUnknownRecipeWiseGridView();

                }
               


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
        private void showReportRecipeWise(string query)
        {

            string todate = formattoDate(rToDate);
            string fromdate = formatfromDate(rToDate);
            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");


        }
        private void showReportUnknownWCWise()
        {

            string todate = formattoDate(rToDate);
            string fromdate = formatfromDate(rToDate);
            fillUnknownWiseGridView();


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
                string dtnadtime = TotalprodataformatDate(rToDate);
                fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND ((testTime>" + dtnadtime);

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
        private void fillunknownrecipewisechildgrid(GridView childgridview)
        {    
            string query="";
            dtnadtime1 = TotalprodataformatDate(rToDate);

            if(QualityReportTBMWise.Checked)

                query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and   ((testTime>" + dtnadtime1;


            else if(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text=="All"&&tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text=="All")
            query="Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and   ((testTime>" + dtnadtime1;
            else if(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text=="All"&&tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text!="All")
            query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and   ((testTime>" + dtnadtime1;
            else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
            query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and   ((testTime>" + dtnadtime1;

                fillChildGridView(childgridview, query);
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

                performanceReportSizeWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportSizeWiseMainGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void fillRecipeWiseGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceReportRecipeWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportRecipeWiseGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void fillUnknownWiseGridView()
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            DataTable dt = new DataTable();
            dt.Columns.Add("WCName", typeof(string));
            DataRow dr = dt.NewRow();
            dr["WCName"] = "UnknownTBM WC";
            dt.Rows.Add(dr);

            try
            {

                UnknownWCMainGridView.DataSource = dt;
                UnknownWCMainGridView.DataBind();
               
            }
            catch (Exception ex)
            {

            }
        }

        private void fillUnknownRecipeWiseGridView()
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            DataTable dt = new DataTable();
            dt.Columns.Add("WCName", typeof(string));
            DataRow dr = dt.NewRow();
            dr["WCName"] = "UnknownTBM WC";
            dt.Rows.Add(dr);

            try
            {

               
                UnknownRecipeMainGridView.DataSource = dt;
                UnknownRecipeMainGridView.DataBind();
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

                if (childGridView.ID == "performanceReportSizeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportSizeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportSizeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                }
                else if (childGridView.ID == "performanceReportrecipeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportRecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }
                else if (childGridView.ID == "performanceReportUnknownWiseChildGridView")
                {


                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportUnknownWCWise_Nos", "unknown", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportUnkonownWCWise_Percentage", "unknown", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
 
                    }

                  
                }
                else if (childGridView.ID == "performanceReportUnknownRecipeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportUnknownWCWise_Nos", "unknown", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportUnkonownWCWise_Percentage", "unknown", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
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

       


        public int AllTotalUnknowncheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if(QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and barcode='' ";


              else if((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text=="All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text=="All"))

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and barcode='' ";
              else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
              else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";

                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));



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
        public Double AllTotalUnknownAQuantity()
        {
            Double flag = 0;
            Double totalchecked = AllTotalUnknowncheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='A' and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A'  and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";

                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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
        public Double AllTotalUnknownBQuantity()
        {
            Double flag = 0;
            Double totalchecked = AllTotalUnknowncheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='B' and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B'  and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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
        public Double AllTotalUnknownCQuantity()
        {
            Double flag = 0;
            Double totalchecked = AllTotalUnknowncheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='C' and testTime>=@todate and testTime<=@fromdate and barcode='' ";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'C'  and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));



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
        public Double AllTotalUnknownDQuantity()
        {
            Double flag = 0;
            Double totalchecked = AllTotalUnknowncheckedQuantity();


            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='D' and testTime>=@todate and testTime<=@fromdate and barcode='' ";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'D'  and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));

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
        public Double AllTotalUnknownEQuantity()
        {
            Double flag = 0;
            Double totalchecked = AllTotalUnknowncheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();


                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='E' and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'E'  and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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
            dtnadtime1 = TotalprodataformatDate(rToDate);

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if(QualityReportTBMWise.Checked)

                   myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";


                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";

                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate" ;

                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));
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
        public Double AlltotalAQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate) ";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));

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
        public Double AlltotalBQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));

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
        public Double AlltotalCQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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
        public Double AlltotalDQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D'  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                else  if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;

                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));
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
        public Double AlltotalEQuantity()
        {
            Double flag = 0;
            dtnadtime1 = TotalprodataformatDate(rToDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E' and (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));

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
            try
            {
                DateTime tempDate = Convert.ToDateTime(date);
                flag = tempDate.ToString("MM-dd-yyyy");
                flag = flag + " " + "07:00:00";
            }
            catch (Exception exp)
            {
 
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

                flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' " + ")OR" + " " + "(testTime>'" + flag3 + "'and" + " " + "testTime<" + "'" + flag4 + "'))";
            }
            catch (Exception exp)
            {
 
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
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {

            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
            fillSizedropdownlist();
            fillDesigndropdownlist();
            QualityReportTBMWisePanel.Visible = false;
            QualityReportRecipeWisePanel.Visible = true;


        }     
        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
            QualityReportTBMWisePanel.Visible = true;
            QualityReportRecipeWisePanel.Visible = false;

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
        protected void ViewButton_Click(object sender, EventArgs e)
        {

            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
            reportHeader._rDate = reportMasterFromDateTextBox.Text;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));
            
           
            query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
            wcIDQuery = "(wcID = '4' Or wcID = '5' Or wcID = '6' Or wcID = '7' Or wcID = '8')";
            wcnamequery = wcquery(query);
            dtnadtime1 = TotalprodataformatDate(rToDate);

            if (QualityReportTBMWise.Checked)
            {
                QualityReportTBMWisePanel.Visible = true;
                QualityReportRecipeWisePanel.Visible = false;
                showReport(query);
                fillUnknownWiseGridView();
            }
            else if (QualityReportRecipeWise.Checked)
            {
                QualityReportTBMWisePanel.Visible = false;
                QualityReportRecipeWisePanel.Visible = true;
                showReportRecipeWise(wcnamequery);
                fillUnknownRecipeWiseGridView();
            }

           
          
        }
        private void fillSizedropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.DataBind();
        }
        private void fillDesigndropdownlist()
        {

            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.DataBind();
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

            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);


            return flag;
        }



    }
}