using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

namespace SmartMIS.TUO
{
    public partial class performanceReportSpacWise : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        #endregion
        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery, parameter, spec, wcIDQuery,query;
        float value;
        string queryString;
        String[] tempString2;
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
                    if (QualityReportRecipeWise.Checked)
                    {
                        performanceReportSpecRcipeWisePanel.Visible = true;
                        PerformaceReportSpecMachinewisePanel.Visible = false;
                    }
                    else if (QualityReportTBMWise.Checked)
                    {
                        performanceReportSpecRcipeWisePanel.Visible = false;
                        PerformaceReportSpecMachinewisePanel.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void magicButton_Click(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query = myWebService.createQuery(tempString2[1]);
            wcIDQuery = myWebService.createwcIDQuery(tempString2[1]);
            wcnamequery = myWebService.wcquery(query, tempString2[tempString2.Length - 1]);
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("Select"));
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("Select"));


            if (tempString2[3] == "0")
            {
                rType = "monthWise";
                rToMonth = tempString2[5];
                rToYear = tempString2[6];
            }
            else if (tempString2[3] != "0")
            {
                rType = "dayWise";
                rToDate = myWebService.formatDate(tempString2[3]);
                rFromDate = myWebService.formatDate(tempString2[4]);

            }
            try
            {


                parameter = tuoFilterSpecParameterDropDownList.SelectedValue;
                reportHeader.ReportDate = rToDate;


                if (SpecTextBox.Text.Length > 0)
                    value = Convert.ToInt32(SpecTextBox.Text);
                else
                {
                    value = 0;
                    SpecTextBox.Text = "0";
                }

                if (QualityReportTBMWise.Checked)
                {
                    PerformaceReportSpecMachinewisePanel.Visible = true;
                    performanceReportSpecRcipeWisePanel.Visible = false;

                    showReport(query);
                }
                else if (QualityReportRecipeWise.Checked)
                {
                    performanceReportSpecRcipeWisePanel.Visible = true;
                    PerformaceReportSpecMachinewisePanel.Visible = false;
                    showReportRecipeWise(wcnamequery);
                }
            }
            catch(Exception ex)
            {

            }


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


                    else  if (((GridView)sender).ID == "performanceReportTBMWiseSpecGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });
                        
                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportTBMWiseSpecTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTBMWiseSpecChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), "1");
                    }


                    else if (((GridView)sender).ID == "performanceReportRecipeWiseSpecGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportRecipeWiseSpecTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportRecipeWiseSpecChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), "1");
                    }
                }
            }

            catch (Exception exp)
            {
            }
        }
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {
            performanceReportSpecRcipeWisePanel.Visible = true;
            PerformaceReportSpecMachinewisePanel.Visible = false;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
            fillSizedropdownlist();
            fillDesigndropdownlist();
           
        }
        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {
            performanceReportSpecRcipeWisePanel.Visible = false;
            PerformaceReportSpecMachinewisePanel.Visible = true;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
          
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query =myWebService.createQuery(tempString2[1]);
            wcIDQuery =myWebService.createwcIDQuery(tempString2[1]);
            parameter = tuoFilterSpecParameterDropDownList.SelectedItem.Text;
            if (tempString2[1] != "0")
            {
                if (tempString2[3] == "0")
                {
                    rType = "monthWise";
                    rToMonth = tempString2[5];
                    rToYear = tempString2[6];
                }
                else if (tempString2[3] != "0")
                {
                    rType = "dayWise";
                    rToDate = myWebService.formatDate(tempString2[3]);
                    rFromDate = myWebService.formatDate(tempString2[4]);
                }
                wcnamequery =myWebService.wcquery(query,"1");
                string dtnadtime =myWebService.TotalprodataformatDate(rToDate);
                if (rType == "dayWise")
                {
                    if (QualityReportTBMWise.Checked)
                    {
                        fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                        
                    }

                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                      


                    }
                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                     

                    }
                    else if (((DropDownList)sender).ID == "tuoFilterOptionDropDownList")
                    {

                        if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")

                                query = "select distinct tireType from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")

                                query = "select distinct tireType from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;

                            fillRecipeWiseGridView(query);
                           
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                        {

                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                query = "select distinct tireType  from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            if (tempString2[tempString2.Length - 1].ToString() == "1")
                                query = "select distinct tireType  from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;

                            fillRecipeWiseGridView(query);
                       
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            

                        }



                    }

                }
                else if (rType == "monthWise")
                {
                    if (QualityReportTBMWise.Checked)
                    {
                        fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                       

                    }

                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rToDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                     

                    }
                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                       

                    }
                    else if (((DropDownList)sender).ID == "tuoFilterOptionDropDownList")
                    {

                        if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")

                                query = "select distinct tireType from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")

                                query = "select distinct tireType from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                            fillRecipeWiseGridView(query);
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                        {

                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                query = "select distinct tireType  from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            if (tempString2[tempString2.Length - 1].ToString() == "1")
                                query = "select distinct tireType  from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                            fillRecipeWiseGridView(query);
                           
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            
                        }



                    }

                }

            }
            else
            {
              
            }

        }
        #endregion
        #region User Defined Function
        private void showReportRecipeWise(string query)
        {
            if (rType == "dayWise")
            {
                string todate = formattoDate(rToDate);
                string fromdate = formatfromDate(rFromDate);
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");

                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");

                else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
            }
            else if (rType == "monthWise")
            {
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + query + ") and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')");

                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + query + ") and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)<='" + rToYear + "')");

                else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')");
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

                performanceReportRecipeWiseSpecGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportRecipeWiseSpecGridView.DataBind();
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
        private void showReport(string query)
        {
            fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
        }
        public void showReport1(GridView childgridview, string wcname, string rToDate, string rFromDate)
        {
            try
            {
                if (rType == "dayWise")
                {
                    string dtnadtime = TotalformatDate(rToDate,rFromDate);
                    if (tempString2[tempString2.Length - 1] == "0")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseProductionDataTUO WHERE  wcname='" + wcname + "' AND testTime>='" + formattoDate(rToDate) + "' and testTime<='" + formatfromDate(rFromDate) + "'");
                    else if (tempString2[tempString2.Length - 1] == "1")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machinename='" + wcname + "' AND testTime>='" + formattoDate(rToDate) + "' and testTime<='" + formatfromDate(rFromDate) + "'");
                    else if (tempString2[tempString2.Length - 1] == "2")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND testTime>='" + formattoDate(rToDate) + "' and testTime<='" + formatfromDate(rFromDate) + "'");
                }
                else if (rType == "monthWise")
                {
                    if (tempString2[tempString2.Length - 1] == "0")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseProductionDataTUO WHERE  wcname='" + wcname + "' AND datepart(MM,testTime)='" +rToMonth + "' and datepart(YYYY,testTime)='" +rToYear + "'");
                    else if (tempString2[tempString2.Length - 1] == "1")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machinename='" + wcname + "' AND datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'");
                    else if (tempString2[tempString2.Length - 1] == "2")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'");
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

                else if (childGridView.ID == "performanceReportRecipeWiseSpecChildGridView")
                {
                    if (option == "1")
                    
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportSizeWise_Nos", "aaa", recipecode, toDate, fromDate, ConnectionOption.SQL);
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
                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and  tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  testTime>=@todate and testTime<=@fromdate  and tireType=@recipecode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'  and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'  and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "' and tireType=@recipecode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and wcname= '" + workcentername + "' and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and machineName= '" + workcentername + "' and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and wcname= '" + workcentername + "' and tireType=@recipecode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "' and wcname= '" + workcentername + "' and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "' and machineName= '" + workcentername + "' and tireType=@recipecode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "' and wcname= '" + workcentername + "' and tireType=@recipecode";
                    }
                }
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

                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformitygrade='A' and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformitygrade='A' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformitygrade='A' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate) and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformitygrade='A' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformitygrade='A' and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformitygrade='A' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformitygrade='A' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                }
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
                if (QualityReportRecipeWise.Checked)
                {

                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate) and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformityGrade='B' and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where uniformityGrade='B' and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                }
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
                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                    }
                }
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

                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                    }
                    if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "')  and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                    if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                }

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
                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and (datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                }
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
                if (QualityReportRecipeWise.Checked)
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate)  and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and tireType=@recipeCode";
                    }
                }
                else
                {
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + "  and ( testTime>=@todate and testTime<=@fromdate) and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and machineName= '" + workcentername + "' and tireType=@recipeCode";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + "  and ( datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "') and wcname= '" + workcentername + "' and tireType=@recipeCode";
                    }
                }
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

                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                        {
                             if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "1")
                        {
                             if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "2")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";

                    }
                
               
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
               
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO UniformityGrade = 'A' and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'A' and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "1")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade = 'A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade = 'A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "2")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and  testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'A' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'A' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    }
                
               
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
               
                    if (rType == "dayWise")
                    {
                        
                            if (tempString2[tempString2.Length - 1] == "0")
                            {
                                if (QualityReportTBMWise.Checked)
                                    myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO UniformityGrade = 'B' and testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'B' and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                            }
                            else if (tempString2[tempString2.Length - 1] == "1")
                            {
                                if (QualityReportTBMWise.Checked)
                                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade = 'B' and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and tireType  in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade = 'B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                            }
                            else if (tempString2[tempString2.Length - 1] == "2")
                            {
                                if (QualityReportTBMWise.Checked)
                                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and  testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                    myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                            }
                        
                    }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade = 'B' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade = 'B' and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    }
                
               
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
               
                    if (rType == "dayWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "1")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType  in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                        else if (tempString2[tempString2.Length - 1] == "2")
                        {
                            if (QualityReportTBMWise.Checked)
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and  testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                            else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                                myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                        }
                    
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value1 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    }
                }
               
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
                if (rType == "dayWise")
                {
                    if (tempString2[tempString2.Length - 1] == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType  in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                }

                else if (rType == "monthWise")
                {
                    if (tempString2[tempString2.Length - 1] == "0")
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    else if (tempString2[tempString2.Length - 1] == "1")
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    else if (tempString2[tempString2.Length - 1] == "2")
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                }
                
               
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
                if (rType == "dayWise")
                {
                    if (tempString2[tempString2.Length - 1] == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO (" + parameter + ">" + value + " and " + parameter + "<" + value2 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value2 + ") and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value + " and " + parameter + "<" + value2 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where   (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                }

                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where (" + parameter + ">" + value1 + " and " + parameter + "<" + value2 + ") and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                    }
              
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
                if (rType == "dayWise")
                {
                    if (tempString2[tempString2.Length - 1] == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO ("+ parameter + ">" + value1 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + ") and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where (" + parameter + ">" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where ("+ parameter + ">" + value1 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where ("+ parameter + ">" + value1 + ") and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value1 + ") and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE  (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + parameter + ">" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where   (" + parameter + ">" + value1 + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1] == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where ("+ parameter + ">" + value1 + ") and  testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where ("+ parameter + ">" + value1 + ") and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where ("+ parameter + ">" + value1 + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "Select") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "Select"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where ("+ parameter + ">" + value1 + ") and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                }
                    else if (rType == "monthWise")
                    {
                        if (tempString2[tempString2.Length - 1] == "0")
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where " + parameter + ">" + value1 + " and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "1")
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where " + parameter + ">" + value1 + " and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";
                        else if (tempString2[tempString2.Length - 1] == "2")
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where " + parameter + ">" + value1 + " and datepart(MM,testTime)='" + rToMonth + "' and datepart(YYYY,testTime)='" + rToYear + "'";

                    }
               
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

            flag.Add("Select");
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

