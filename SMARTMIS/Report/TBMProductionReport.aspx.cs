using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Report
{
    public partial class TBMProductionReport : System.Web.UI.Page
    {

        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, rprocessID;
        public int actualQuan, planningQuan, totalPlanningQuan, totalActualQuan;

        public String Visiblity
        {

            get
            {
                return productionReportDatewWiseMainPanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                productionReportDatewWiseMainPanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    productionReportDateWiseGridView.DataSource = null;
                    productionReportDateWiseGridView.DataBind();
                    productionReportMonthWiseGridView.DataSource = null;
                    productionReportMonthWiseGridView.DataBind();
                }
            
                    
            }
            catch (Exception ex)
            {

            }
        }

        public int shift_a_count = 0, shift_b_count = 0, shift_c_count = 0;
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "productionReportDateWiseGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseWCIDLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("productionReportDateWiseChildGridView"));

                        fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), 0, rToDate, rFromDate);
                    }
                    else if (((GridView)sender).ID == "productionReportDateWiseChildGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildWCIDLabel"));
                        Label productTypeIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildProductTypeIDLabel"));
                        Label recipeIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseChildRecipeIDLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("productionReportDateWiseInnerChildGridView"));

                        fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), Convert.ToInt32(productTypeIDLabel.Text.Trim()), rToDate, rFromDate);
                    }
                    else if (((GridView)sender).ID == "productionReportDateWiseInnerChildGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildWCIDLabel"));
                        Label productTypeIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildProductTypeIDLabel"));
                        Label recipeIDLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildRecipeIDLabel"));
                        Label productionReportDateWiseInnerChildShiftAPlanLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftAPlanLabel"));
                        Label productionReportDateWiseInnerChildShiftBPlanLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftBPlanLabel"));
                        Label productionReportDateWiseInnerChildShiftCPlanLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftCPlanLabel"));
                        Label productionReportDateWiseInnerChildShiftAActualLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftAActualLabel"));
                        Label productionReportDateWiseInnerChildShiftBActualLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftBActualLabel"));
                        Label productionReportDateWiseInnerChildShiftCActualLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftCActualLabel"));
                        Label productionReportDateWiseInnerChildShiftADifferenceLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftADifferenceLabel"));
                        Label productionReportDateWiseInnerChildShiftBDifferenceLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftBDifferenceLabel"));
                        Label productionReportDateWiseInnerChildShiftCDifferenceLabel = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildShiftCDifferenceLabel"));

                        shift_a_count += Convert.ToInt32(productionReportDateWiseInnerChildShiftAActualLabel.Text);
                        shift_b_count += Convert.ToInt32(productionReportDateWiseInnerChildShiftBActualLabel.Text);
                        shift_c_count += Convert.ToInt32(productionReportDateWiseInnerChildShiftCActualLabel.Text);

                        System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("productionReportDailyChart");
                        fillChart(childChart, productionReportDateWiseInnerChildShiftAPlanLabel.Text.Trim(), productionReportDateWiseInnerChildShiftBPlanLabel.Text.Trim(), productionReportDateWiseInnerChildShiftCPlanLabel.Text.Trim(), productionReportDateWiseInnerChildShiftAActualLabel.Text.Trim(), productionReportDateWiseInnerChildShiftBActualLabel.Text.Trim(), productionReportDateWiseInnerChildShiftCActualLabel.Text.Trim(), productionReportDateWiseInnerChildShiftADifferenceLabel.Text.Trim(), productionReportDateWiseInnerChildShiftBDifferenceLabel.Text.Trim(), productionReportDateWiseInnerChildShiftCDifferenceLabel.Text.Trim());
                    }
                    else if (((GridView)sender).ID == "productionReportMonthWiseGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseWCIDLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("productionReportMonthWiseChildGridView"));

                        fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), 0, rToMonth, rToYear);
                    }

                    else if (((GridView)sender).ID == "productionReportMonthWiseChildGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseChildWCIDLabel"));
                        Label productTypeIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseChildProductTypeIDLabel"));
                        Label recipeIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseChildRecipeIDLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("productionReportMonthWiseInnerChildGridView"));

                        fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), Convert.ToInt32(productTypeIDLabel.Text.Trim()), rToMonth, rToYear);
                    }

                    else if (((GridView)sender).ID == "productionReportMonthWiseInnerChildGridView")
                    {
                        Label wcIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildWCIDLabel"));
                        Label productTypeIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildProductTypeIDLabel"));
                        Label recipeIDLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildRecipeIDLabel"));
                        Label productionReportMonthWiseInnerChildShiftAPlanLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftAPlanLabel"));
                        Label productionReportMonthWiseInnerChildShiftBPlanLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftBPlanLabel"));
                        Label productionReportMonthWiseInnerChildShiftCPlanLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftCPlanLabel"));
                        Label productionReportMonthWiseInnerChildShiftAActualLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftAActualLabel"));
                        Label productionReportMonthWiseInnerChildShiftBActualLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftBActualLabel"));
                        Label productionReportMonthWiseInnerChildShiftCActualLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftCActualLabel"));
                        Label productionReportMonthWiseInnerChildShiftADifferenceLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftADifferenceLabel"));
                        Label productionReportMonthWiseInnerChildShiftBDifferenceLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftBDifferenceLabel"));
                        Label productionReportMonthWiseInnerChildShiftCDifferenceLabel = ((Label)e.Row.FindControl("productionReportMonthWiseInnerChildShiftCDifferenceLabel"));


                        System.Web.UI.DataVisualization.Charting.Chart childChart = (System.Web.UI.DataVisualization.Charting.Chart)e.Row.FindControl("productionReportMonthlyChart");
                        fillChart(childChart, productionReportMonthWiseInnerChildShiftAPlanLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftBPlanLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftCPlanLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftAActualLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftBActualLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftCActualLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftADifferenceLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftBDifferenceLabel.Text.Trim(), productionReportMonthWiseInnerChildShiftCDifferenceLabel.Text.Trim());
                    }

                }
            }
            catch (Exception exp)
            {
            }
        }

        protected void showDailyReport(string query)
        {
            try
            {
                fillGridView("Select DISTINCT iD, workCenterName from vWorkCenter WHERE " + query + "");
            }
            catch (Exception ex)
            {
            }
        }

        protected void showMonthlyReport(string query)
        {
            try
            {

                fillGridView("Select DISTINCT iD, workCenterName from vWorkCenter WHERE " + query + "");
            }
            catch (Exception exp)
            {

            }
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011
            //Revision No.  : 01z
            try
            {
                if (rType == "1" && rChoice == "0")
                {
                    productionReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    productionReportDateWiseGridView.DataBind();
                }

                else if (rType == "1" && rChoice == "1")
                {
                    productionReportMonthWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    productionReportMonthWiseGridView.DataBind();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void fillChart(System.Web.UI.DataVisualization.Charting.Chart objChart, String shiftAplan, String shiftBplan, String shiftCplan, String shiftAactual, String shiftBactual, String shiftCactual, String shiftAdiff, String shiftBdiff, String shiftCdiff)
        {

            //Description   : Function for creating productionReportDailyChart chart
            //Author        : Brajesh kumar
            //Date Created  : 04 May 2011
            //Date Updated  : 04 May 2011
            //Revision No.  : 01

            double[] planSeries = new double[3];
            double[] actualSeries = new double[3];
            double[] differenceSeries = new double[3];

            planSeries.SetValue(Convert.ToInt32(shiftAplan), 0);
            planSeries.SetValue(Convert.ToInt32(shiftBplan), 1);
            planSeries.SetValue(Convert.ToInt32(shiftCplan), 2);

            actualSeries.SetValue(Convert.ToInt32(shiftAactual), 0);
            actualSeries.SetValue(Convert.ToInt32(shiftBactual), 1);
            actualSeries.SetValue(Convert.ToInt32(shiftCactual), 2);

            differenceSeries.SetValue(Convert.ToInt32(shiftAdiff), 0);
            differenceSeries.SetValue(Convert.ToInt32(shiftBdiff), 1);
            differenceSeries.SetValue(Convert.ToInt32(shiftCdiff), 2);


            objChart.Series[0].Points.DataBindXY(myWebService.shift, planSeries);
            objChart.Series[1].Points.DataBindXY(myWebService.shift, actualSeries);
            objChart.Series[2].Points.DataBindXY(myWebService.shift, differenceSeries);
        }
        private void fillChildGridView(GridView childGridView, int wcID, int productTypeID, String toDate, String fromDate)
        {
            try
            {
                if (childGridView.ID == "productionReportDateWiseChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, ProductName  from vProductionactual1 WHERE WCID = " + wcID + " ", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "productionReportDateWiseInnerChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, RecipeID, RecipeName,dtandTime from vProductionactual1 WHERE (WCID = " + wcID + " AND ProductTypeID = " + productTypeID + " AND dtandTime ='" + myWebService.formatDate(toDate) + "')", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "productionReportMonthWiseChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, ProductName  from vProductionactual1 WHERE WCID = " + wcID + " ", ConnectionOption.SQL);
                    childGridView.DataBind();
                }

                else if (childGridView.ID == "productionReportMonthWiseInnerChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, RecipeID, RecipeName from vProductionactual1 WHERE (WCID = " + wcID + " AND ProductTypeID = " + productTypeID + " AND datepart(mm,dtandTime) ='" + rToMonth + "' and datepart(yyyy,dtandTime)='" + rToYear + "')", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public int plannedQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Quantity from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND (DtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));



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
        public int totalplannedQuantity(Object wcID, Object recipeID, Object productTypeID, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (DtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));

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
        public int monthPlannedQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND datepart(mm,dtandTime)='" + rToMonth + "' and datepart(yyyy,dtandTime)='" + rToYear + "'";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);




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
        public int totalMonthPlannedQuantity(Object wcID, Object recipeID, Object productTypeID)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND datepart(mm,dtandTime)='" + rToMonth + "' and datepart(yyyy,dtandTime)='" + rToYear + "'";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));

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
        public int actualQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Quantity from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND(DtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));

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
        public int totalActualQuantity(Object wcID, Object recipeID, Object productTypeID, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND(DtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));

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
        public int monthActualQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift)AND datepart(mm,dtandTime)='" + rToMonth + "' and datepart(yyyy,dtandTime)='" + rToYear + "'";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);

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

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;
        }
        public int totalMonthActualQuantity(Object wcID, Object recipeID, Object productTypeID)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND datepart(mm,dtandTime)='" + rToMonth + "' and datepart(yyyy,dtandTime)='" + rToYear + "'";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));

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
        public int differenceQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object dtandTime)
        {
            int flag = 0;
            actualQuan = actualQuantity(wcID, recipeID, productTypeID, shift, dtandTime);
            planningQuan = plannedQuantity(wcID, recipeID, productTypeID, shift, dtandTime);

            flag = actualQuan - planningQuan;

            return flag;
        }

        public int totalDifferenceQuantity(Object wcID, Object recipeID, Object productTypeID, Object dtandTime)
        {
            int flag = 0;
            totalActualQuan = totalActualQuantity(wcID, recipeID, productTypeID, dtandTime);
            totalPlanningQuan = totalplannedQuantity(wcID, recipeID, productTypeID, dtandTime);

            flag = totalActualQuan - totalPlanningQuan;

            return flag;
        }

        public int monthDifferenceQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift)
        {
            int flag = 0;
            actualQuan = monthActualQuantity(wcID, recipeID, productTypeID, shift);
            planningQuan = monthPlannedQuantity(wcID, recipeID, productTypeID, shift);

            flag = actualQuan - planningQuan;

            return flag;
        }

        public int totalMonthDifferenceQuantity(Object wcID, Object recipeID, Object productTypeID)
        {
            int flag = 0;
            totalActualQuan = totalMonthActualQuantity(wcID, recipeID, productTypeID);
            totalPlanningQuan = totalMonthPlannedQuantity(wcID, recipeID, productTypeID);

            flag = totalActualQuan - totalPlanningQuan;

            return flag;
        }

        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {

        }
        
    }
}
