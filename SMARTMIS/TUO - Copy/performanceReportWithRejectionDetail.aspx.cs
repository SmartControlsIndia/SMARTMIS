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
    public partial class performanceReportWithRejectionDetail : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        #endregion

        #region globle variable
        public string queryString, rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, option, workcentername, wcnamequery, wcIDQuery,machinenamequery;
        string dtnadtime1 = "";
        string query = "";
        string[] tempString2;
        string footergrid;
    

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
                    if (QualityReportTBMWise.Checked)
                    {    
                        productionReport2TBMWiseMainPanel.Visible = true;
                        productionReport2RecipeWiseMainPanel.Visible = false;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
                    }
                    else if (QualityReportRecipeWise.Checked)
                    {
                        productionReport2TBMWiseMainPanel.Visible = false;
                        productionReport2RecipeWiseMainPanel.Visible = true;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
                        //fillSizedropdownlist();
                        //fillDesigndropdownlist();
                    }
                    performanceReport2TBMWiseMainGridView.DataSource = null;
                    performanceReport2TBMWiseMainGridView.DataBind();
                    performanceReport2TBMRecipeWiseMainGridView.DataSource = null;
                    performanceReport2TBMRecipeWiseMainGridView.DataBind();
                    //Compare the hidden field if it contains the query string or not


                    //  Compare which type of report user had selected//
                    //
                    //  Plant wide = 0
                    //  Workcenter wide = 1
                    //

                    if (ReportDate != null)
                    {
                        //rToDate = myWebService.formatDate(ReportDate);
                        //rFromDate = formatfromDate(myWebService.formatDate(ReportDate));

                        //query = "(iD = '4' Or iD = '5' Or iD = '6' Or iD = '7' Or iD = '8')";
                        //wcIDQuery = "(wcID = '4' Or wcID = '5' Or wcID = '6' Or wcID = '7' Or wcID = '8')";
                        //wcnamequery = wcquery(query);
                        //showReport(query);

                        //string footergrid = "performanceReport2TBMWiseFooterGridView";
                        //fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), "1");


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

            if (tuoFilterOptionDropDownList.SelectedIndex == 1)
                option = "1";
            else if (tuoFilterOptionDropDownList.SelectedIndex == 2)
                option = "2";
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
                   
                    productionReport2TBMWiseMainPanel.Visible=true;
                    productionReport2RecipeWiseMainPanel.Visible = false;
                    showReport(query);
                   footergrid = "performanceReport2TBMWiseFooterGridView";
                    fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rFromDate), option);
                    //if (tempString2[tempString2.Length - 1].ToString() != "1")
                    //fillUnknownWiseGridView();
                }
                else if (QualityReportRecipeWise.Checked)
                {

                    productionReport2TBMWiseMainPanel.Visible = false;
                    productionReport2RecipeWiseMainPanel.Visible = true;
                    showReportRecipeWise(wcnamequery);
                    footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                    fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rFromDate), option);
                   // if (tempString2[tempString2.Length - 1].ToString() != "1")
                   // fillUnknownRecipeWiseGridView();
                }
            }
            else
            {
                performanceReport2TBMWiseMainGridView.DataSource = null;
                performanceReport2TBMWiseMainGridView.DataBind();
                performanceReport2TBMRecipeWiseMainGridView.DataSource = null;
                performanceReport2TBMRecipeWiseMainGridView.DataBind();
            }

           

        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (tuoFilterOptionDropDownList.SelectedIndex == 0)
                    option = "1";
                else if (tuoFilterOptionDropDownList.SelectedIndex == 1)
                    option = "2";


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReport2TBMWiseMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReport2TBMWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();

                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMWiseGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                    }
                    else   if (((GridView)sender).ID == "performanceReport2TBMWiseGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReport2TBMWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMWiseChildGridView"));
                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);
                    }
                    else   if (((GridView)sender).ID == "performanceReport2TBMRecipeWiseMainGridView")
                    {
                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReport2TBMRecipeWiseRecipeNameLabel"));

                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReport2TBMRecipeWiseChildGridView"));
                        fillChildInnerGridView(wcnamequery, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rFromDate), option);
                    }

                }
            }

            catch (Exception exp)
            {
            }
        }
        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tuoFilterOptionDropDownList.SelectedIndex == 1)
                option = "1";
            else if (tuoFilterOptionDropDownList.SelectedIndex == 2)
                option = "2";
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
                        footergrid = "performanceReport2TBMWiseFooterGridView";
                        fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rFromDate), option);
                       // if (tempString2[tempString2.Length - 1].ToString() != "1")
                            //fillUnknownWiseGridView();

                    }

                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                         if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                         performanceReport2TBMRecipeWiseFooterGridView.Visible = false;

                         //footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                         //fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), option);
                        //fillUnknownRecipeWiseGridView();


                    }
                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                         if (tempString2[tempString2.Length - 1].ToString() == "2")
                            fillGridViewRecipeWise("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
                         performanceReport2TBMRecipeWiseFooterGridView.Visible = false;

                         //footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                         //fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), option);
                        //fillUnknownRecipeWiseGridView();


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

                            fillGridViewRecipeWise(query);
                            performanceReport2TBMRecipeWiseFooterGridView.Visible = false;

                            //footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                            //fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), option);
                            //fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                        {

                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                query = "select distinct tireType  from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            if (tempString2[tempString2.Length - 1].ToString() == "1")
                                query = "select distinct tireType  from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;

                            fillGridViewRecipeWise(query);
                            performanceReport2TBMRecipeWiseFooterGridView.Visible = false;
                            //footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                            //fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rToDate), option);
                            //fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            footergrid = "performanceReport2TBMRecipeWiseFooterGridView";
                            fillChildInnerGridView(footergrid, "ggg", formattoDate(rToDate), formatfromDate(rFromDate), option);
                            //if (tempString2[tempString2.Length - 1].ToString() != "1")
                            //    fillUnknownRecipeWiseGridView();

                        }



                    }

                }
                else if (rType == "monthWise")
                {
                    if (QualityReportTBMWise.Checked)
                    {
                        fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                        //if (tempString2[tempString2.Length - 1].ToString() != "1")
                            //fillUnknownWiseGridView();

                    }

                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        //if (tempString2[tempString2.Length - 1].ToString() == "0")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        //else if (tempString2[tempString2.Length - 1].ToString() == "1")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        //else if (tempString2[tempString2.Length - 1].ToString() == "2")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");

                        //fillUnknownRecipeWiseGridView();


                    }
                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rFromDate);
                        //if (tempString2[tempString2.Length - 1].ToString() == "0")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        //else if (tempString2[tempString2.Length - 1].ToString() == "1")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");
                        //else if (tempString2[tempString2.Length - 1].ToString() == "2")
                        //    fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")");

                        //fillUnknownRecipeWiseGridView();


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

                            //fillRecipeWiseGridView(query);
                            //fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                        {

                            if (tempString2[tempString2.Length - 1].ToString() == "0")
                                query = "select distinct tireType  from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            if (tempString2[tempString2.Length - 1].ToString() == "1")
                                query = "select distinct tireType  from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";
                            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  (datepart(MM,testTime)=" + rToMonth + " and datepart(YYYY,testTime)=" + rToYear + ")";

                            //fillRecipeWiseGridView(query);
                            //fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            //showReportRecipeWise(wcnamequery);
                            //if (tempString2[tempString2.Length - 1].ToString() != "1")
                            //    fillUnknownRecipeWiseGridView();

                        }



                    }

                }

            }
            else
            {
                //performanceReportSizeWiseMainGridView.DataSource = null;
                //performanceReportSizeWiseMainGridView.DataBind();
                //performanceReportRecipeWiseGridView.DataSource = null;
                //performanceReportRecipeWiseGridView.DataBind();
            }
            
        }
        protected void QualityReportRecipeWise_CheckedChanged(object sender, EventArgs e)
        {

            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
            fillSizedropdownlist();
            fillDesigndropdownlist();
            //QualityReportTBMWisePanel.Visible = false;
            //QualityReportRecipeWisePanel.Visible = true;


        }
        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
            //QualityReportTBMWisePanel.Visible = true;
            //QualityReportRecipeWisePanel.Visible = false;

        }
        #endregion

        #region User Defined Function
        private void showReportRecipeWise(string query)
        {

            string todate = formattoDate(rToDate);
            string fromdate = formatfromDate(rFromDate);
            if (tempString2[tempString2.Length - 1].ToString() == "0")
                fillGridViewRecipeWise("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
            else if (tempString2[tempString2.Length - 1].ToString() == "1")
                fillGridViewRecipeWise("Select DISTINCT  tireType from productionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");
            else if (tempString2[tempString2.Length - 1].ToString() == "2")
                fillGridViewRecipeWise("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and (testTime>'" + todate + "' and testTime<='" + fromdate + "')");


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

                //string dtnadtime = TotalformatDate(rToDate);

               // fillChildGridView(childgridview, "Select distinct TireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND testTime>='" + formattoDate(rToDate) + "' and testTime<='" + formatfromDate(rToDate) + "'");
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
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseproductionDataTUO WHERE  wcname='" + wcname + "' AND  datepart(MM,testTime)=" + rToMonth + "and datepart(yyyy,testTime)=" + rToYear + "");
                    else if (tempString2[tempString2.Length - 1].ToString() == "1")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machineName='" + wcname + "'AND  datepart(MM,testTime)=" + rToMonth + "and datepart(yyyy,testTime)=" + rToYear + "");

                    else if (tempString2[tempString2.Length - 1].ToString() == "2")
                        fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND  datepart(MM,testTime)=" + rToMonth + "and datepart(yyyy,testTime)=" + rToYear + "");

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

                performanceReport2TBMWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReport2TBMWiseMainGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        private void fillGridViewRecipeWise(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 22 June 2012
            //Date Updated  : 22 June 2012
            //Revision No.  : 01
            try
            {

                performanceReport2TBMRecipeWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReport2TBMRecipeWiseMainGridView.DataBind();
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

                

                if (childGridView.ID == "performanceReport2TBMWiseChildGridView")
                {
                    if (option == "1")
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "0")                        
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2CuringSWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);                       
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")                     
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TUOSWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TBMSWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }


                    else if (option == "2")
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2CuringSWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TUOSWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TBMSWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }
                else  if (childGridView.ID == "performanceReport2TBMRecipeWiseChildGridView")
                {
                    if (option == "1")
                    {

                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2CuringRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")                      
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TUORecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);                      
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TBMRecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                            childGridView.DataBind();
                    }


                    else if (option == "2")
                    {

                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2CuringRecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TUORecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReport2TBMRecipeWise_Percentage", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);
                        childGridView.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void fillChildInnerGridView(string GridView, string recipecode, String toDate, String fromDate, String option)
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
                if (GridView == "performanceReport2TBMWiseFooterGridView")
                {
                    if (option == "1")
                    {

                        if (tempString2[tempString2.Length - 1].ToString() == "0")                        
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterCuringSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);               
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")                      
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTUOSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);                           
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTBMSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);                        
                        performanceReport2TBMWiseFooterGridView.DataBind();
                    }
                    else if (option == "2")
                    {

                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterCuringSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTUOSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            performanceReport2TBMWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTBMSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        performanceReport2TBMWiseFooterGridView.DataBind();

                    }

                }
                else   if (GridView == "performanceReport2TBMRecipeWiseFooterGridView")
                {
                    if (option == "1")
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "0")                
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterCuringSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);                      
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")                        
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTUOSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTBMSWise_Nos", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);                       
                        performanceReport2TBMRecipeWiseFooterGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        if (tempString2[tempString2.Length - 1].ToString() == "0")
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterCuringSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "1")
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTUOSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        else if (tempString2[tempString2.Length - 1].ToString() == "2")
                            performanceReport2TBMRecipeWiseFooterGridView.DataSource = fillGridView("sp_PerformanceReport2FooterTBMSWise_Percentage", "wcname", recipecode, toDate, fromDate, ConnectionOption.SQL);
                        performanceReport2TBMRecipeWiseFooterGridView.DataBind();
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
