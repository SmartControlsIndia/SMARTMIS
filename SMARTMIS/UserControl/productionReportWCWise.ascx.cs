using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace SmartMIS.UserControl
{
    public partial class productionReportWCWise : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();


        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rSize, rToYear, rFromYear, rprocessID, operatorID, processName, query;
        public int actualQuan, planningQuan,totalPlanningQuan,totalActualQuan;

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

                productionReportDateWiseGridView.DataSource = null;
                productionReportDateWiseGridView.DataBind();
                productionReportMonthWiseGridView.DataSource = null;
                productionReportMonthWiseGridView.DataBind();

                //size = TBMProductionReportSizeDropdownlist.SelectedValue.ToString();
                operatorID = TBMProductionReportRecipeDropdownlist.SelectedValue.ToString();
                ErrorMsg.Visible = false;
                //Compare the hidden field if it contains the query string or not
                if (!IsPostBack)
                {
                   // fillSizedropdownlist();
                    //fillOperatordropdownlist();
                }
                
                if (tempString.Length > 1)
                {
                    rType = tempString[0];
                    rWCID = tempString[1];
                    rChoice = tempString[2];
                    rToDate = tempString[4];
                    rFromDate = tempString[3];
                    rSize = tempString[5];
                    rToYear = tempString[6];
                    rFromYear = tempString[7];
                    if (tempString[9] == "0")
                        processName = "7";  //Curing PCR
                    else if (tempString[9] == "1")
                        processName = "5";  //Curing TBR


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
                            
                            rToDate = formatDate(myWebService.formatDate(rToDate));
                            query = myWebService.createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
                            showDailyReport(query);
                            magicHidden.Value = "";
                        }
                        else if (rChoice == "1")
                        {
                            rToMonth = tempString[5];
                            rToYear = tempString[6];
                            string query = myWebService.createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
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
            catch(Exception ex)
            {

            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDailyReport(query);
            productionReportDatewWiseMainPanel.Visible = true;
        }
        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

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
        private void fillOperatordropdownlist()
        {

            TBMProductionReportRecipeDropdownlist.Items.Clear();

            string sqlQuery = "";

            ListItem litem = new ListItem("All", "All");
            TBMProductionReportRecipeDropdownlist.Items.Add(litem);

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT manningID, firstName, lastName from vCuringProduction";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (myConnection.reader[0].ToString() != "")
                    {
                        ListItem li = new ListItem(myConnection.reader[1].ToString() + " " + myConnection.reader[2].ToString(), myConnection.reader[0].ToString());
                        TBMProductionReportRecipeDropdownlist.Items.Add(li);
                    }
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
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
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void showDailyReport(string query)
        {
            try
            {
                if (query == "(iD = '0')" && !string.IsNullOrEmpty(magicHidden.Value))
                {
                    ErrorMsg.Visible = true;
                    ErrorMsg.Text = "<b>Select WorkCenters!!</b><BR><BR><Center><input type=\"button\" onClick=\"closePopup()\" value=\"  OK  \" class=\"popupBut\" /></Center>";
                }
                else
                {
                    fillGridView("Select DISTINCT iD, workCenterName from vWorkCenter WHERE " + query + "");
                }
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void showMonthlyReport(string query)
        {
            try
            {

                fillGridView("Select DISTINCT iD, workCenterName from vWorkCenter WHERE " + query + "");
            }
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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
            catch(Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        private void fillChart(System.Web.UI.DataVisualization.Charting.Chart objChart, String shiftAplan, String shiftBplan, String shiftCplan, String shiftAactual, String shiftBactual,String shiftCactual,String shiftAdiff,String shiftBdiff,String shiftCdiff)
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
            planSeries.SetValue(Convert.ToInt32(shiftCplan),2);
            
            actualSeries.SetValue(Convert.ToInt32(shiftAactual), 0);
            actualSeries.SetValue(Convert.ToInt32(shiftBactual),1);
            actualSeries.SetValue(Convert.ToInt32(shiftCactual),2);
            
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
                string remainingQuery = "";
                if (rSize != "All")
                    remainingQuery = " AND recipeName='" + rSize + "'";
                
                //if (operatorID != "All")
                //    remainingQuery += " AND manningID='" + operatorID + "'";
                
                if (childGridView.ID == "productionReportDateWiseChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, ProductName  from vProductionactual1 WHERE WCID = " + wcID + remainingQuery + " ", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "productionReportDateWiseInnerChildGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select DISTINCT wcID, ProductTypeID, RecipeID, RecipeName,dtandTime from vProductionactual1 WHERE (WCID = " + wcID + remainingQuery + " AND ProductTypeID = " + productTypeID + " AND CONVERT (date, dtandTime, 110) >='" + myWebService.formatDate(fromDate) + "' AND CONVERT (date, dtandTime, 110)<'" + myWebService.formatDate(toDate) + "')", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        public int plannedQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select SUM(Quantity) from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND (DtandTime>=@fromdtandTime) AND (DtandTime<@todtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);
                myConnection.comm.Parameters.AddWithValue("@fromdtandTime", myWebService.formatDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@todtandTime", myWebService.formatDate(rToDate));
                
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
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
        public int totalplannedQuantity(Object wcID, Object recipeID, Object productTypeID, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionplanning1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (DtandTime>=@fromdtandTime) AND (DtandTime<@todtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@fromdtandTime", myWebService.formatDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@todtandTime", myWebService.formatDate(rToDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
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

        public int actualQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object plandtandTime)
        {
            int flag =0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select SUM(Quantity) from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (shift = @shift) AND ( CONVERT (date, dtandTime, 110)>=@fromdtandTime) AND ( CONVERT (date, dtandTime, 110)<@todtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@shift", shift);
                myConnection.comm.Parameters.AddWithValue("@fromdtandTime", myWebService.formatDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@todtandTime", myWebService.formatDate(rToDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
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

        public int totalActualQuantity(Object wcID, Object recipeID, Object productTypeID, Object plandtandTime)
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select sum(Quantity) from vProductionactual1 Where (wcID = @wcID) AND (recipeID = @recipeID) AND (productTypeID = @productTypeID) AND (CONVERT(date, dtandTime, 110)>=@fromdtandTime) AND (CONVERT(date, dtandTime, 110)<@todtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(wcID));
                myConnection.comm.Parameters.AddWithValue("@recipeID", Convert.ToInt32(recipeID));
                myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(productTypeID));
                myConnection.comm.Parameters.AddWithValue("@fromdtandTime", myWebService.formatDate(rFromDate));
                myConnection.comm.Parameters.AddWithValue("@todtandTime", myWebService.formatDate(rToDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = Convert.ToInt32(myConnection.reader[0]);
                    else
                        flag = 0;
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
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
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

        public int differenceQuantity(Object wcID, Object recipeID, Object productTypeID, Object shift, Object dtandTime)
        {
            int flag = 0;
            actualQuan = actualQuantity(wcID, recipeID, productTypeID, shift, dtandTime);
            planningQuan = plannedQuantity(wcID, recipeID, productTypeID, shift, dtandTime);

            flag = actualQuan - planningQuan;

            return flag;
        }

        public int totalDifferenceQuantity(Object wcID,Object recipeID,Object productTypeID, Object dtandTime)
        {
            int flag = 0;
            totalActualQuan = totalActualQuantity(wcID,recipeID,productTypeID, dtandTime);
            totalPlanningQuan = totalplannedQuantity(wcID,recipeID,productTypeID,dtandTime);

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
        

    }
}