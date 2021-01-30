using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace SmartMIS.UserControl
{
    public partial class tuoFilterRep2 : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;
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

                    if (tempString.Length > 1)
                    {
                        rType = tempString[0];
                        rWCID = tempString[1];
                        rChoice = tempString[2];
                        rToDate = tempString[3];
                        rFromDate = tempString[3];
                        rToMonth = tempString[5];
                        rToYear = tempString[6];
                        rFromYear = tempString[7];

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
                                rToDate = myWebService.formatDate(rToDate);
                                rFromDate = formatDate(myWebService.formatDate(rFromDate));
                                tyreTypeHidden.Value = getTyreType("productionDataTUO", "TireType", " WHERE TestTime >= '" + rToDate + "' AND TestTime <= '" + rFromDate + "'");


                                string r1ToDate = rToDate + " 07:00:00 AM";
                                string r1FromDate = rFromDate + " 07:00:00 AM";

                                // For preventing tuoFilterNOMDropDownList from filling again and again on postback//
                                if (tyreTypeHidden.Value != "")
                                {


                                }
                                else
                                {
                                    performanceReport2SizeWiseGridView.DataSource = null;
                                    performanceReport2SizeWiseGridView.DataBind();
                                }


                                // For preventing tuoFilterNOMDropDownList from filling again and again on postback//

                                if (viewQueryHidden.Value == "True")
                                {
                                    fillNOMDropDownList();

                                    viewQueryHidden.Value = "False";
                                }

                                if (tuoFilterRep2SizeDropDownList.SelectedValue != "")
                                    tuoFilterRep2SizeDropDownList.Items.Remove("");
                            }
                            else if (rChoice == "1")
                            {
                            }
                            else if (rChoice == "2")
                            {
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void magicButton_Click(object sender, EventArgs e)
        {

        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                if (((DropDownList)sender).ID == "tuoFilterRep2NOMDropDownList")
                {
                    fillDesignDropDownList();

                    tuoFilterRep2SizeDropDownList.DataSource = null;
                    tuoFilterRep2SizeDropDownList.DataBind();
                }
                else if (((DropDownList)sender).ID == "tuoFilterRep2DesignDropDownList")
                {
                    fillSizeDropDownList();

                    tuoFilterRep2SizeDropDownList.DataSource = null;
                    tuoFilterRep2SizeDropDownList.DataBind();
                }
                else if (((DropDownList)sender).ID == "tuoFilterRep2SizeDropDownList")
                {
                    if (tuoFilterRep2SizeDropDownList.SelectedValue == "All")
                    {
                       tyreTypeHidden.Value = getTyreType("vDesignMaster", "description", " where design='" + tuoFilterRep2DesignDropDownList.SelectedValue + "'");
                    }
                    else
                    {
                       tyreTypeHidden.Value = getTyreType("vDesignMaster", "description", " WHERE size='" + tuoFilterRep2SizeDropDownList.SelectedValue + "'");
                    }
                    int resultIndex = Convert.ToInt32(tuoFilterRep2ResultDropDownList.SelectedIndex);
                    resultIndex++;
                    tyreTypeHidden.Value = tyreTypeHidden.Value + "?" + resultIndex;
                    showReport(tyreTypeHidden.Value, rToDate, rFromDate);
                }
                else if (((DropDownList)sender).ID == "tuoFilterRep2ResultDropDownList")
                {
                    if (tuoFilterRep2SizeDropDownList.SelectedValue == "All")
                    {
                       tyreTypeHidden.Value = getTyreType("vDesignMaster", "description", " where design='" + tuoFilterRep2DesignDropDownList.SelectedValue + "'");
                    }
                    else
                    {
                      tyreTypeHidden.Value = getTyreType("vDesignMaster", "description", " WHERE size='" + tuoFilterRep2SizeDropDownList.SelectedValue + "'");
                    }
                    int resultIndex = Convert.ToInt32(tuoFilterRep2ResultDropDownList.SelectedIndex);
                    resultIndex++;
                   tyreTypeHidden.Value = tyreTypeHidden.Value + "?" + resultIndex;
                    showReport(tyreTypeHidden.Value, rToDate, rFromDate);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void fillNOMDropDownList()
        {
            //Description   : Function for filling tuoFilterNOMDropDownList WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 21 June 2012
            //Date Updated  : 21 June 2012
            //Revision No.  : 01
            try
            {
                tuoFilterRep2SizeDropDownList.Items.Clear();
                tuoFilterRep2DesignDropDownList.Items.Clear();

                if (tyreTypeHidden.Value.Trim() != "")
                {
                    String whereClause = "";
                    String[] tempTyreType = tyreTypeHidden.Value.Split(new char[] { '#' });
                    if (tempTyreType.Length > 1)
                    {
                        foreach (String item in tempTyreType)
                        {
                            if (whereClause.Length > 0)
                                whereClause += " OR ";

                            whereClause += "description = '" + item + "'";
                        }
                        tuoFilterRep2NOMDropDownList.DataSource = FillDropDownList("vDesignMaster", "nom", " WHERE " + whereClause);
                        tuoFilterRep2NOMDropDownList.DataBind();
                    }
                    else
                    {
                        tuoFilterRep2NOMDropDownList.DataSource = FillDropDownList("vDesignMaster", "nom", " WHERE description = '" +tyreTypeHidden.Value + "'");
                        tuoFilterRep2NOMDropDownList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public void fillDesignDropDownList()
        {
            //Description   : Function for filling tuoFilterDesignDropDownList WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 21 June 2012
            //Date Updated  : 21 June 2012
            //Revision No.  : 01
            try
            {
                tuoFilterRep2SizeDropDownList.Items.Clear();

                if (tyreTypeHidden.Value.Trim() != "")
                {
                    String whereClause = "";
                    String[] tempTyreType = tyreTypeHidden.Value.Split(new char[] { '#' });
                    if (tempTyreType.Length > 1)
                    {
                        foreach (String item in tempTyreType)
                        {
                            if (whereClause.Length > 0)
                                whereClause += " OR ";

                            whereClause += "description = '" + item + "'";
                        }
                        if (tuoFilterRep2NOMDropDownList.SelectedValue == "All")
                        {
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "design", " WHERE (" + whereClause + ")");
                            tuoFilterRep2DesignDropDownList.DataBind();
                        }
                        else
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "design", " WHERE nom = '" + tuoFilterRep2NOMDropDownList.SelectedItem.Value + "'");
                              tuoFilterRep2DesignDropDownList.DataBind();

                    }
                    else
                    {
                        if (tuoFilterRep2NOMDropDownList.SelectedValue == "All")
                        {
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "design", " WHERE  description = '" + tyreTypeHidden.Value + "'");
                            tuoFilterRep2DesignDropDownList.DataBind();
                        }
                        else
                        {
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "design", " WHERE nom = '" + tuoFilterRep2NOMDropDownList.SelectedItem.Value + "'");
                            tuoFilterRep2DesignDropDownList.DataBind();

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void fillSizeDropDownList()
        {
            //Description   : Function for filling tuoFilterDesignDropDownList WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 21 June 2012
            //Date Updated  : 21 June 2012
            //Revision No.  : 01
            try
            {
                if (tyreTypeHidden.Value.Trim() != "")
                {
                    String whereClause = "";
                    String[] tempTyreType = tyreTypeHidden.Value.Split(new char[] { '#' });
                    if (tempTyreType.Length > 1)
                    {
                        foreach (String item in tempTyreType)
                        {
                            if (whereClause.Length > 0)
                                whereClause += " OR ";

                            whereClause += "description = '" + item + "'";
                        }
                        if (tuoFilterRep2DesignDropDownList.SelectedValue == "All")
                        {
                            tuoFilterRep2SizeDropDownList.DataSource = FillDropDownList("vDesignMaster", "size", " WHERE  (" + whereClause + ")");
                            tuoFilterRep2SizeDropDownList.DataBind();
                        }
                        else
                        {
                            tuoFilterRep2SizeDropDownList.DataSource = FillDropDownList("vDesignMaster", "size", " WHERE design = '" + tuoFilterRep2DesignDropDownList.SelectedItem.Text.Trim() + "' AND (" + whereClause + ")");
                            tuoFilterRep2SizeDropDownList.DataBind();
                        }
                    }
                    else
                    {
                        if (tuoFilterRep2DesignDropDownList.SelectedValue == "All")
                        {
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "size", " WHERE  description = '" + tyreTypeHidden.Value + "'");
                            tuoFilterRep2DesignDropDownList.DataBind();
                        }
                        else
                        {
                            tuoFilterRep2DesignDropDownList.DataSource = FillDropDownList("vDesignMaster", "size", " WHERE  design = '" + tuoFilterRep2DesignDropDownList.SelectedItem.Text.Trim() + "' AND description = '" +tyreTypeHidden.Value + "'");
                            tuoFilterRep2DesignDropDownList.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
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
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReport2SizeWiseGridView")
                    {
                        string[] tempValue =tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label tyreTypeLabel = ((Label)e.Row.FindControl("performanceReport2SizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2SizeWiseChildGridView"));

                        fillChildGridView(childGridView, tyreTypeLabel.Text.Trim(), rToDate, rFromDate, tempValue[1]);
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }
        public void showReport(string tyreType, string rToDate, string rFromDate)
        {
            try
            {
                if (tuoFilterRep2SizeDropDownList.SelectedValue == "All")
                {
                    fillGridView("Select  description FROM vDesignMaster WHERE Design='" + tuoFilterRep2DesignDropDownList.SelectedValue + "'");

                }
                if (tuoFilterRep2SizeDropDownList.SelectedValue == "All" && tuoFilterRep2DesignDropDownList.SelectedValue == "All")
                {
                    fillGridView("Select  description FROM vDesignMaster WHERE nom='" + tuoFilterRep2NOMDropDownList.SelectedValue + "'");

                }
                if (tuoFilterRep2SizeDropDownList.SelectedValue == "All" && tuoFilterRep2DesignDropDownList.SelectedValue == "All" && tuoFilterRep2NOMDropDownList.SelectedValue == "All")
                {
                    fillGridView("Select distinct TireType as description FROM productionDataTUO WHERE TestTime >= '" + rToDate + "' AND TestTime <= '" + rFromDate + "'");

                }

                if (tuoFilterRep2SizeDropDownList.SelectedValue != "All")
                {
                    fillGridView("Select  description FROM vDesignMaster WHERE size='" + tuoFilterRep2SizeDropDownList.SelectedValue + "'");
                }
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

                performanceReport2SizeWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReport2SizeWiseGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void fillChildGridView(GridView childGridView, string tyreType, String toDate, String fromDate, String option)
        {
            //Description   : Function for filling ChildGridView
            //Author        : Brajesh kumar
            //Date Created  : 23 June 2012
            //Date Updated  : 23 June 2012
            //Revision No.  : 01
            //Description   :
            try
            {
                if (childGridView.ID == "performanceReport2SizeWiseChildGridView")
                {
                    if (option == "1")
                        childGridView.DataSource = fillGridView("sp_performanceReport2SizeWise_Nos", tyreType, toDate, fromDate, ConnectionOption.SQL);
                    else if (option == "2")
                        childGridView.DataSource = fillGridView("sp_PerformanceReport2SizeWise_Percentage", tyreType, toDate, fromDate, ConnectionOption.SQL);

                    childGridView.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable fillGridView(string procedureName, string tyreType, string rToDate, string rFromDate, ConnectionOption option)
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

                    System.Data.SqlClient.SqlParameter tyreTypeParameter = new System.Data.SqlClient.SqlParameter("@tyreType", System.Data.SqlDbType.VarChar);
                    tyreTypeParameter.Direction = System.Data.ParameterDirection.Input;
                    tyreTypeParameter.Value = tyreType;

                    System.Data.SqlClient.SqlParameter toDateParameter = new System.Data.SqlClient.SqlParameter("@toDate", System.Data.SqlDbType.VarChar);
                    toDateParameter.Direction = System.Data.ParameterDirection.Input;
                    toDateParameter.Value = rToDate;

                    System.Data.SqlClient.SqlParameter fromDateParameter = new System.Data.SqlClient.SqlParameter("@fromDate", System.Data.SqlDbType.VarChar);
                    fromDateParameter.Direction = System.Data.ParameterDirection.Input;
                    fromDateParameter.Value = rFromDate;

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
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("MM-dd-yyyy");

            return flag;
        }
    }
}