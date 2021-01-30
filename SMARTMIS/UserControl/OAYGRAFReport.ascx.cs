using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

namespace SmartMIS.UserControl
{
    public partial class OAYGRAFReport : System.Web.UI.UserControl
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        #endregion
        myConnection myConnection = new myConnection();

        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery;
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

                    QualityReportOAYGRAFTBMWisePanel.Visible = true;
                    QualityReportOAYGRAFRecipeWisePanel.Visible = false;
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
                                rFromDate = formatfromDate(myWebService.formatDate(rFromDate));
                                //tyreTypeHidden.Value = getTyreType("productionDataTUO", "TireType", " WHERE TestTime >= '" + rToDate + "' AND TestTime <= '" + rFromDate + "'");

                                //string r1ToDate = rToDate + " 07:00:00 AM";
                                //string r1FromDate = rFromDate + " 07:00:00 AM";

                                // For preventing tuoFilterNOMDropDownList from filling again and again on postback//
                                //if (tyreTypeHidden.Value != "")
                                //{


                                //}
                                //else
                                //{
                                //    performanceReportSizeWiseMainGridView.DataSource = null;
                                //    performanceReportSizeWiseMainGridView.DataBind();
                                //}


                                // For preventing tuoFilterNOMDropDownList from filling again and again on postback//

                                //if (viewQueryHidden.Value == "True")
                                //{
                                string query = myWebService.createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
                                wcnamequery = wcquery(query);
                                showReport(query);
                                showReportRecipeWise(wcnamequery);
                                fillchart();

                                //    viewQueryHidden.Value = "False";
                                //}


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

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReportOAYGRAFWiseMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportOAYGRAFWiseGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                    }


                    if (((GridView)sender).ID == "performanceReportOAYGRAFWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportOAYGRAFWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportOAYGRAFWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), "1");
                    }
                }
                if (((GridView)sender).ID == "performanceReportOAYGRAFRecipeWiseGridView")
                {

                    Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportOAYGRAFRecipeWiseTyreTypeLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseChildGridView"));

                    fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), "2");
                }
            }

            catch (Exception exp)
            {
            }
        }

        #endregion

        #region User Defined Function



        private void showReport(string query)
        {
            fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
        }

        private void showReportRecipeWise(string query)
        {

            DateTime tempdate = Convert.ToDateTime(rToDate);
            string dtandtime = tempdate.ToString("yyyy");

            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and DATEPART(yyyy,testTime)="+dtandtime+"");


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
                DateTime tempdate = Convert.ToDateTime(rToDate);
                string dtandtime = tempdate.ToString("yyyy");

                fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND DATEPART(yyyy,testTime)="+dtandtime+"");

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

                performanceReportOAYGRAFWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportOAYGRAFWiseMainGridView.DataBind();
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

                performanceReportOAYGRAFRecipeWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportOAYGRAFRecipeWiseGridView.DataBind();
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

                if (childGridView.ID == "performanceReportOAYGRAFWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTBMSWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                }
                if (childGridView.ID == "performanceReportOAYGRAFrecipeWiseChildGridView")
                {

                    childGridView.DataSource = fillGridView("sp_PerformanceReportOAYRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                    childGridView.DataBind();
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
        
        private void fillchart()
        {    
            
            double[] TotalCheckedSeries = new double[13];
            double[] ABgradeSeries = new double[13];
            string[] xvalues = { "YTD","jan","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC" };
             


            TotalCheckedSeries.SetValue(AlltotalcheckedQuantity(), 0);
            TotalCheckedSeries.SetValue(AlltotalJANCheckedQuantity(), 1);
            TotalCheckedSeries.SetValue(AlltotalFEBCheckedQuantity(), 2);
            TotalCheckedSeries.SetValue(AlltotalMARCheckedQuantity(), 3);
            TotalCheckedSeries.SetValue(AlltotalAPRCheckedQuantity(), 4);
            TotalCheckedSeries.SetValue(AlltotalMAYCheckedQuantity(), 5);
            TotalCheckedSeries.SetValue(AlltotalJUNCheckedQuantity(), 6);
            TotalCheckedSeries.SetValue(AlltotalJULCheckedQuantity(), 7);
            TotalCheckedSeries.SetValue(AlltotalAUGCheckedQuantity(), 8);
            TotalCheckedSeries.SetValue(AlltotalSEPCheckedQuantity(), 9);
            TotalCheckedSeries.SetValue(AlltotalOCTCheckedQuantity(), 10);
            TotalCheckedSeries.SetValue(AlltotalNOVCheckedQuantity(), 11);
            TotalCheckedSeries.SetValue(AlltotalDECCheckedQuantity(), 12);

            ABgradeSeries.SetValue(AlltotalYTDQuantity(), 0);
            ABgradeSeries.SetValue(AlltotalJANQuantity(), 1);
            ABgradeSeries.SetValue(AlltotalFEBQuantity(), 2);

            ABgradeSeries.SetValue(AlltotalMARQuantity(), 3);
            ABgradeSeries.SetValue(AlltotalAPRQuantity(), 4);
            ABgradeSeries.SetValue(AlltotalMAYQuantity(), 5);
            ABgradeSeries.SetValue(AlltotalJUNQuantity(), 6);
            ABgradeSeries.SetValue(AlltotalJULQuantity(), 7);
            ABgradeSeries.SetValue(AlltotalAUGQuantity(), 8);

            ABgradeSeries.SetValue(AlltotalSEPQuantity(), 9);
            ABgradeSeries.SetValue(AlltotalOCTQuantity(), 10);
            ABgradeSeries.SetValue(AlltotalNOVQuantity(), 11);
            ABgradeSeries.SetValue(AlltotalDECQuantity(), 12);


            performanceReportOAYGrafTBMChart.Series["TotalCheckedSeries"].Points.DataBindXY(xvalues, TotalCheckedSeries);
            performanceReportOAYGrafTBMChart.Series["ABgradeSeries"].Points.DataBindXY(xvalues, ABgradeSeries);

        }


        public int AlltotalcheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and( " + wcnamequery + ")";
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
        public int AlltotalJANCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='01' and( " + wcnamequery + ")";
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
        public int AlltotalFEBCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='02' and( " + wcnamequery + ")";
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
        public int AlltotalMARCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='03' and( " + wcnamequery + ")";
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
        public int AlltotalAPRCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='04' and( " + wcnamequery + ")";
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
        public int AlltotalMAYCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='05' and( " + wcnamequery + ")";
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
        public int AlltotalJUNCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='06' and( " + wcnamequery + ")";
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
        public int AlltotalJULCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='07' and( " + wcnamequery + ")";
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
        public int AlltotalAUGCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='08' and( " + wcnamequery + ")";
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
        public int AlltotalSEPCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='09' and( " + wcnamequery + ")";
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
        public int AlltotalOCTCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='10' and( " + wcnamequery + ")";
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
        public int AlltotalNOVCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='11' and( " + wcnamequery + ")";
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
        public int AlltotalDECCheckedQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where DATEPART(yyyy, testTime)= DATEPART(yyyy,@todate) and datepart(MM,testTime)='12' and( " + wcnamequery + ")";
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


        public int AlltotalYTDQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)=DATEPART(yyyy,@todate) and( " + wcnamequery + ")";
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

        public int AlltotalJANQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='01' and( " + wcnamequery + ")";
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

        public int AlltotalFEBQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='02' and( " + wcnamequery + ")";
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

        public int AlltotalMARQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='03' and( " + wcnamequery + ")";
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

        public int AlltotalAPRQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='04' and( " + wcnamequery + ")";
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

        public int AlltotalMAYQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='05' and( " + wcnamequery + ")";
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

        public int AlltotalJUNQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='06' and( " + wcnamequery + ")";
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

        public int AlltotalJULQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='07' and( " + wcnamequery + ")";
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

        public int AlltotalAUGQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='08' and( " + wcnamequery + ")";
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

        public int AlltotalSEPQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandTime)='09' and( " + wcnamequery + ")";
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

        public int AlltotalOCTQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='10' and( " + wcnamequery + ")";
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

        public int AlltotalNOVQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='11' and( " + wcnamequery + ")";
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

        public int AlltotalDECQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and DATEPART(yyyy,dtandTime)= DATEPART(yyyy, @todate) and DATEPART(MM, dtandtime)='12' and( " + wcnamequery + ")";
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

        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {
            QualityReportOAYGRAFTBMWisePanel.Visible = false;
            QualityReportOAYGRAFRecipeWisePanel.Visible = true;

        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {
            QualityReportOAYGRAFTBMWisePanel.Visible = true;
            QualityReportOAYGRAFRecipeWisePanel.Visible = false;

        }
    }
}