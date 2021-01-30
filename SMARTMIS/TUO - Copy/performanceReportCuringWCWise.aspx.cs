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
    public partial class performanceReportCuringWCWise : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        #endregion
        myConnection myConnection = new myConnection();

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery,machinenamequery;
        string dtnadtime1 = "";
        string query = "";
        string[] tempString2;
    

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

                   // reportHeader._rDate = reportMasterFromDateTextBox.Text;
                    // reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    if (QualityReportTBMWise.Checked)
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
                        QualityReportTBMWisePanel.Visible = true;
                        QualityReportRecipeWisePanel.Visible = false;
                    }
                    else if (QualityReportRecipeWise.Checked)
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

            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);
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
                    rToDate =myWebService.formatDate(tempString2[3]);
                    rFromDate = myWebService.formatDate(tempString2[4]);
                }

                tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
                tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));



                wcnamequery = wcquery(query);
                if (rType == "dayWise")
                    dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);

                if (QualityReportTBMWise.Checked)
                {
                    QualityReportTBMWisePanel.Visible = true;
                    QualityReportRecipeWisePanel.Visible = false;
                    showReport(query);
                    if (tempString2[tempString2.Length - 1].ToString() != "1")
                        fillUnknownWiseGridView();
                }
                else if (QualityReportRecipeWise.Checked)
                {
                    QualityReportTBMWisePanel.Visible = false;
                    QualityReportRecipeWisePanel.Visible = true;
                    showReportRecipeWise(wcnamequery);
                    if (tempString2[tempString2.Length - 1].ToString() != "1")
                        fillUnknownRecipeWiseGridView();
                }
            }
            else
            {
                performanceReportSizeWiseMainGridView.DataSource = null;
                performanceReportSizeWiseMainGridView.DataBind();
                performanceReportRecipeWiseGridView.DataSource = null;
                performanceReportRecipeWiseGridView.DataBind();
            }

            //reportHeader.ReportDate = tempString2[3].ToString();
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

           
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


                    else if (((GridView)sender).ID == "performanceReportSizeWiseGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportSizeWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);
                    }

                    else   if (((GridView)sender).ID == "performanceReportRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportrecipeWiseChildGridView"));

                        fillChildInnerGridView("3401", childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);
                    }

                    else   if (((GridView)sender).ID == "UnknownWCMainGridView")
                    {
                        GridView childgridview = ((GridView)e.Row.FindControl("performanceReportUnknownWiseGridView"));

                        fillunknownrecipewisechildgrid(childgridview);


                    }
                    else  if (((GridView)sender).ID == "performanceReportUnknownWiseGridView")
                    {
                        Label recipeCode = ((Label)e.Row.FindControl("performanceReportUnknownWiseTyreTypeLabel"));
                        GridView unknownchildGridView = ((GridView)e.Row.FindControl("performanceReportUnknownWiseChildGridView"));

                        fillChildInnerGridView("3401", unknownchildGridView, recipeCode.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);

                    }
                    else   if (((GridView)sender).ID == "UnknownRecipeMainGridView")
                    {
                        GridView childgridview = ((GridView)e.Row.FindControl("performanceReportUnknownRecipeWiseGridView"));

                        fillunknownrecipewisechildgrid(childgridview);


                    }
                    else  if (((GridView)sender).ID == "performanceReportUnknownRecipeWiseGridView")
                    {
                        Label recipeCode = ((Label)e.Row.FindControl("performanceReportUnknownRecipeWiseTyreTypeLabel"));
                        GridView unknownchildGridView = ((GridView)e.Row.FindControl("performanceReportUnknownRecipeWiseChildGridView"));
                        fillChildInnerGridView("3401", unknownchildGridView, recipeCode.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);

                    }
                }

            }

            catch (Exception exp)
            {
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            tempString2 = queryString.Split(new char[] { '?' });
            query = createQuery(tempString2[1]);
            wcIDQuery = createwcIDQuery(tempString2[1]);
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
                    rToDate =myWebService.formatDate(tempString2[3]);
                    rFromDate = myWebService.formatDate(tempString2[4]);
                }
                wcnamequery = wcquery(query);
                string dtnadtime = TotalprodataformatDate(rToDate,rFromDate);
                if (rType == "dayWise")
                {
                    if (QualityReportTBMWise.Checked)
                    {
                        fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                        if (tempString2[tempString2.Length - 1].ToString() != "1")
                            fillUnknownWiseGridView();

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
                        if (tempString2[tempString2.Length - 1].ToString() != "1")
                        fillUnknownRecipeWiseGridView();


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
                        if (tempString2[tempString2.Length - 1].ToString() != "1")
                        fillUnknownRecipeWiseGridView();


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
                            if (tempString2[tempString2.Length - 1].ToString() != "1")
                            fillUnknownRecipeWiseGridView();
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
                            if (tempString2[tempString2.Length - 1].ToString() != "1")
                            fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            if (tempString2[tempString2.Length - 1].ToString() != "1")
                                fillUnknownRecipeWiseGridView();

                        }



                    }

                }
                else if (rType == "monthWise")
                {
                    if (QualityReportTBMWise.Checked)
                    {
                        fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                        if (tempString2[tempString2.Length - 1].ToString() != "1")
                            fillUnknownWiseGridView();

                    }

                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        if(tempString2[tempString2.Length-1].ToString()!="1")
                        fillUnknownRecipeWiseGridView();


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
                        if(tempString2[tempString2.Length-1].ToString()!="1")
                        fillUnknownRecipeWiseGridView();


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
                            fillUnknownRecipeWiseGridView();
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
                            if (tempString2[tempString2.Length - 1].ToString() != "1")
                            fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            if (tempString2[tempString2.Length - 1].ToString() != "1")
                                fillUnknownRecipeWiseGridView();

                        }



                    }

                }

            }
            else
            {
                performanceReportSizeWiseMainGridView.DataSource = null;
                performanceReportSizeWiseMainGridView.DataBind();
                performanceReportRecipeWiseGridView.DataSource = null;
                performanceReportRecipeWiseGridView.DataBind();
            }

        }

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

        #endregion

        #region User Defined Function

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
        private void showReportUnknownWCWise()
        {

            string todate = formattoDate(rToDate);
            string fromdate = formatfromDate(rFromDate);
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
                if (rType == "dayWise")
                {
                    string dtnadtime = TotalprodataformatDate(rToDate,rFromDate);
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseproductionDataTUO WHERE  wcname='" + wcname + "' AND ((testTime>" + dtnadtime);
                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machineName='" + wcname + "' AND ((testTime>" + dtnadtime);

                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND ((testTime>" + dtnadtime);
                }
                else if (rType == "monthWise")
                {
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseproductionDataTUO WHERE  wcname='" + wcname + "' AND  datepart(MM,testTime)="+rToMonth+"and datepart(yyyy,testTime)="+rToYear+"");
                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machineName='" + wcname + "'AND  datepart(MM,testTime)=" + rToMonth + "and datepart(yyyy,testTime)=" + rToYear + "");

                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND  datepart(MM,testTime)=" + rToMonth + "and datepart(yyyy,testTime)=" + rToYear + "");
 
                }
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
            string query = "";
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            if (rType == "dayWise")
            {
                if (QualityReportTBMWise.Checked)
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and   ((testTime>" + dtnadtime1;
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and   ((testTime>" + dtnadtime1;
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and   ((testTime>" + dtnadtime1;
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and   ((testTime>" + dtnadtime1;
            }
            if (rType == "monthWise")
            {
                if (QualityReportTBMWise.Checked)
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode=''  and   (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and   (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All")
                    query = "Select distinct tireType as description FROM productionDataTUO WHERE barcode='' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and   (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";

            }
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
                if (rType == "dayWise")
                {
                    string dtnadtime = TotalformatDate(rToDate,rFromDate);

                    if (childGridView.ID == "performanceReportSizeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportCuringWCWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportTUOWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportSizeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportCuringWCWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportTUOWise_percent", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportSizeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);


                            childGridView.DataBind();
                        }
                    }
                    else if (childGridView.ID == "performanceReportrecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_CuringWisePerformanceReportRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportTUORecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_CuringWisePerformanceReportRecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_PerformanceReportTUORecipeWise_Percent", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
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
                else if (rType == "monthWise")
                {
                    if (childGridView.ID == "performanceReportSizeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportCuringWCWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportTUOWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportSizeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);

                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportCuringWCWise_Percentage", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportTUOWise_percent", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportSizeWise_Percentage", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);


                            childGridView.DataBind();
                        }
                    }
                    else if (childGridView.ID == "performanceReportrecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_monthCuringWisePerformanceReportRecipeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportTUORecipeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportRecipeWise_Nos", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                childGridView.DataSource = fillGridView("sp_monthCuringWisePerformanceReportRecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportTUORecipeWise_Percent", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);

                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                childGridView.DataSource = fillGridView("sp_monthPerformanceReportRecipeWise_Percentage", wcname, recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                    }
                    else if (childGridView.ID == "performanceReportUnknownWiseChildGridView")
                    {


                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_monthPerformanceReportUnknownWCWise_Nos", "unknown", recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            childGridView.DataSource = fillGridView("sp_monthPerformanceReportUnkonownWCWise_Percentage", "unknown", recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();

                        }


                    }
                    else if (childGridView.ID == "performanceReportUnknownRecipeWiseChildGridView")
                    {
                        if (option == "1")
                        {
                            childGridView.DataSource = fillGridView("sp_monthPerformanceReportUnknownWCWise_Nos", "unknown", recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
                        else if (option == "2")
                        {
                            childGridView.DataSource = fillGridView("sp_monthPerformanceReportUnkonownWCWise_Percentage", "unknown", recipecode, rToMonth, rToYear, ConnectionOption.SQL);
                            childGridView.DataBind();
                        }
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
                if (QualityReportTBMWise.Checked)

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and barcode='' ";


                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";
                else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate and barcode='' ";

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
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


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
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


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
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));



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
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate)";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and testTime>=@todate and testTime<=@fromdate";

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
                else if (rType == "monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+"";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ")  and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+"";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + "";

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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate) ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate) ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and (testTime>=@todate and testTime<=@fromdate) ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;

                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ") ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+"";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+"";
                    }
                    if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ") ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='A' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ") ";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='A' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";

                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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

                else if(rType=="monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcIDQuery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='B' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")) and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='B' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


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
                else if(rType=="monthWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='C' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")) and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='C' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));


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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();

            try
            {
                if(rType=="dayWise")
                {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='D' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                }

                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                }
                else if (tempString2[tempString2.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D'  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
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
                else if(rType=="monthWise")
                {

                     myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString2[tempString2.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='D' and  (" + wcnamequery + ")  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                }

                else if (tempString2[tempString2.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                }
                else if (tempString2[tempString2.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)

                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D'  (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='D' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='D' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)="+rToMonth+" and datepart(YYYY,testTime)="+rToYear+")";
                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
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
            dtnadtime1 = TotalprodataformatDate(rToDate,rFromDate);
            Double totalchecked = AlltotalcheckedQuantity();


            try
            {
                if (rType == "dayWise")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='E' and  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E'  (" + wcnamequery + ")  and testTime>=@todate and testTime<=@fromdate";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (testTime>=@todate and testTime<=@fromdate)) and testTime>=@todate and testTime<=@fromdate";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and ((testTime>" + dtnadtime1;
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and ((testTime>" + dtnadtime1;
                    }
                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));
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
                    if (tempString2[tempString2.Length - 1].ToString() == "0")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='E' and (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                    }

                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                    }
                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                    {
                        if (QualityReportTBMWise.Checked)

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E' and (" + wcnamequery + ")  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))

                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade='E' and  tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcIDQuery + ") and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")) and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                        else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                            myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  UniformityGrade='E' and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                    }

                    myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                    myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rFromDate));

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
        public string TotalprodataformatDate(String fromDate, String toDate)
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


                    flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' ))";
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
                        if(tempString2[tempString2.Length-1].ToString()=="1")
                            flag = flag + "or" + " " + "machineName = '" + myConnection.reader[0] + "'";
                        else
                        flag = flag + "or" + " " + "wcname = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        if(tempString2[tempString2.Length-1].ToString()=="1")
                            flag = "machineName = '" + myConnection.reader[0] + "'";
                        else

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
        #endregion

    }
}
