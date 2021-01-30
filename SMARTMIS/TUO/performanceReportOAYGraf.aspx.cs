using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;


namespace SmartMIS.TUO
{
    public partial class performanceReportOAYGraf : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        #endregion

        #region globle variable
        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery,option, query, wcIDQuery;
        DateTime dtnadtime;
        Double totalchecked;
        string[] tempString;
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
                    if (QualityReportTBMWise.Checked)
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;

                        QualityReportOAYGRAFTBMWisePanel.Visible = true;
                        QualityReportOAYGRAFRecipeWisePanel.Visible = false;
                    }
                    else if (QualityReportRecipeWise.Checked)
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
                        QualityReportOAYGRAFTBMWisePanel.Visible = false;
                        QualityReportOAYGRAFRecipeWisePanel.Visible = true;
                    }
                    //Compare the hidden field if it contains the query string or not

                   

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
                                //rToDate = myWebService.formatDate(rToDate);
                                //rFromDate = formatfromDate(myWebService.formatDate(rFromDate));
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
            
            catch (Exception ex)
            {

            }
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            tempString = magicHidden.Value.Split(new char[] { '?' });

                   if (tempString.Length > 1)
                    {
                        rType = tempString[0];
                        rWCID = tempString[1];
                        rChoice = tempString[2];
                        rToDate = tempString[3];
                        rFromDate = tempString[3];
                        rToMonth = tempString[5];
                        rToYear = tempString[7];
                       }

                   if (rChoice == "2")
                   {
                       notifyLabel.Visible = false;
                       if (QualityReportTBMWise.Checked)
                           QualityReportOAYGRAFTBMWisePanel.Visible = true;
                       else
                           QualityReportOAYGRAFRecipeWisePanel.Visible = true;
                       string query = myWebService.createQuery(rWCID);
                       wcnamequery = myWebService.wcquery(query, tempString[tempString.Length - 1]);
                       showReport(query);
                       showReportRecipeWise(wcnamequery);
                       fillchart();

                   }
                   else
                   {
                       notifyLabel.Visible = true;
                       QualityReportOAYGRAFTBMWisePanel.Visible = false;
                   }



        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (tuoFilterOptionDropDownList.SelectedItem.Text == "No")
                    option = "1";
                else
                    option = "2";
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

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), rToDate, rToDate, option);
                    }

                    if (((GridView)sender).ID == "performanceReportOAYGRAFRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportOAYGRAFRecipeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportOAYGRAFrecipeWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), rToDate, rToDate, option);
                    }
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

           
                if(tempString[tempString.Length-1]=="0")
           fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + query + ") and DATEPART(yyyy,testTime)=" + rToYear + "");

            else if(tempString[tempString.Length-1]=="1")
           fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + query + ") and DATEPART(yyyy,testTime)=" + rToYear + "");

            else if(tempString[tempString.Length-1]=="2")

           fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + query + ") and DATEPART(yyyy,testTime)=" + rToYear + "");


        }


        public void showReport1(GridView childgridview, string wcname, string rToDate, string rFromDate)
        {
            try
            {
               
                if(tempString[tempString.Length-1]=="0")
              fillChildGridView(childgridview, "Select distinct tireType as description FROM vCuringWiseProductionDataTUO WHERE  wcname='" + wcname + "' AND DATEPART(yyyy,testTime)=" + rToYear + "");

                else if(tempString[tempString.Length-1]=="1")
              fillChildGridView(childgridview, "Select distinct tireType as description FROM productionDataTUO WHERE  machinename='" + wcname + "' AND DATEPART(yyyy,testTime)=" + rToYear + "");

                else if(tempString[tempString.Length-1]=="2")
              fillChildGridView(childgridview, "Select distinct tireType as description FROM vproductionDataTUO WHERE  wcname='" + wcname + "' AND DATEPART(yyyy,testTime)=" + rToYear + "");

             
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

                if (childGridView.ID == "performanceReportOAYGRAFWiseChildGridView")
                {
                    if (option == "1")
                    {      if(tempString[tempString.Length-1]=="0")
                        childGridView.DataSource = fillGridView("sp_PerformanceReportOAYCuringWise_Nos", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if(tempString[tempString.Length-1]=="1")
                        childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTUOWise_Nos", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if(tempString[tempString.Length-1]=="2")
                        childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTBMWise_Nos", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        if (tempString[tempString.Length - 1] == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYCuringWise_Percentage", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if (tempString[tempString.Length - 1] == "1")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTUOWise_Percentage", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if (tempString[tempString.Length - 1] == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTBMWise_Percentage", wcname, recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }

                }
                if (childGridView.ID == "performanceReportOAYGRAFrecipeWiseChildGridView")
                {
                    if (option == "1")
                    {
                        if (tempString[tempString.Length - 1] == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYCuringRecipeWise_Nos", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if (tempString[tempString.Length - 1] == "1")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTUORecipeWise_Nos", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);
                        else if (tempString[tempString.Length - 1] == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYRecipeWise_Nos", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);
                    }
                    else if (option == "2")
                    {
                        if (tempString[tempString.Length - 1] == "0")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYCuringRecipeWise_Percentage", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);

                        else if (tempString[tempString.Length - 1] == "1")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYTUORecipeWise_Percentage", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);
                        else if (tempString[tempString.Length - 1] == "2")
                            childGridView.DataSource = fillGridView("sp_PerformanceReportOAYRecipeWise_Percentage", "aaa", recipecode, rToYear, rToYear, ConnectionOption.SQL);
                    }
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

                    System.Data.SqlClient.SqlParameter machineNameParameter = new System.Data.SqlClient.SqlParameter("@wcName", System.Data.SqlDbType.VarChar);
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
            string[] xvalues = { "YTD", "jan", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };



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
            if (option == "1")
            {
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
            }
            else if (option == "2")
            {
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
            }

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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";

                }
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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";

                }


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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'";

                }
             


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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'";

                }
               
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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'";

                }
              

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
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'";

                }
               

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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'";

                }              


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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'";

                }                


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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'";

                }              

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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'";

                }               

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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'";

                }               

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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'";

                }            

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

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  (" + wcnamequery + ") and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')) and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  (" + wcnamequery + ")  and UniformityGrade in('A','B') and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'))  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'";

                }           


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
        public Double AlltotalYTDQuantity()
        {
            Double flag = 0;
            totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + ")  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and datepart(YYYY,testTime)=" + rToYear + "";

                }

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalJANQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01'";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='01')";

                }
             

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalFEBQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";

                }               


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalMARQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='03')";

                }               

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalAPRQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='04')";

                }               
               

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalMAYQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='05')";

                }               
               


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalJUNQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='06')";

                }               
                


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalJULQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='07')";

                }               
               

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalAUGQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='08')";

                }               
               

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalSEPQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='09')";

                }               


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalOCTQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')) and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')) and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='10')";

                }               
             

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalNOVQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')) and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='02')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='11')";

                }               
              

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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
        public Double AlltotalDECQuantity()
        {
            Double flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                if (tempString[tempString.Length - 1].ToString() == "0")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12') ";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vCuringWiseProductionDataTUO WHERE (" + wcnamequery + ") and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'))  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and  (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vCuringWiseProductionDataTUO where  UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                }

                else if (tempString[tempString.Length - 1].ToString() == "1")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from ProductionDataTUO WHERE (" + wcnamequery + ") and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')) and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                }
                else if (tempString[tempString.Length - 1].ToString() == "2")
                {
                    if (QualityReportTBMWise.Checked)
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  (" + wcnamequery + ")  and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in (Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12'))  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";
                    else if ((tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All") && (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All"))
                        myConnection.comm.CommandText = "select COUNT(*) from vProductionDataTUO where UniformityGrade in('A','B') and  tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')  and (datepart(YYYY,testTime)=" + rToYear + " and datepart(MM,testTime)='12')";

                }               
              

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalchecked;
                            flag = Math.Round(flag, 2);
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

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.ToString("MM-dd-yyyy");
            flag = flag + " " + "07:00:00";

            return flag;
        }

        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
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
            if (date != null)
            {
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
            QualityReportOAYGRAFTBMWisePanel.Visible = false;
            QualityReportOAYGRAFRecipeWisePanel.Visible = true;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = true;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = true;
            fillSizedropdownlist();
            fillDesigndropdownlist();

        }

        protected void QualityReportTBMWise_CheckedChanged(object sender, EventArgs e)
        {
            QualityReportOAYGRAFTBMWisePanel.Visible = true;
            QualityReportOAYGRAFRecipeWisePanel.Visible = false;
            tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Enabled = false;
            tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Enabled = false;

        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tempString = magicHidden.Value.Split(new char[] { '?' });
            query =myWebService.createQuery(tempString[1]);
            wcIDQuery =myWebService.createwcIDQuery(tempString[1]);
            if (tempString[1] != "0")
            {
                rToYear = tempString[7];
                wcnamequery =myWebService.wcquery(query,tempString[tempString.Length-1]);
                if (QualityReportTBMWise.Checked)
                {
                    fillGridView("Select DISTINCT  workCenterName from vWorkCenter WHERE " + query + "");
                    fillchart();
                }
              
                else   if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseRecipeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                        string todate = formattoDate(rToDate);
                        string fromdate = formatfromDate(rToDate);
                        if (tempString[tempString.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "");
                        else if (tempString[tempString.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "");
                        else if (tempString[tempString.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)=" + rToYear + "");
                      

                    }
                    else if (((DropDownList)sender).ID == "tuoFilterPerformanceReportTUOWiseSizeDropdownlist")
                    {
                        tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedIndex = tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                        if (tempString[tempString.Length - 1].ToString() == "0")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vCuringWiseproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)="+rToYear+"");
                        else if (tempString[tempString.Length - 1].ToString() == "1")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from productionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)="+rToYear+"");
                        else if (tempString[tempString.Length - 1].ToString() == "2")
                            fillRecipeWiseGridView("Select DISTINCT  tireType from vproductionDataTUO WHERE (" + wcnamequery + ") and tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "') and datepart(YYYY,testTime)="+rToYear+"");
                       

                    }
                    else if (((DropDownList)sender).ID == "tuoFilterOptionDropDownList")
                    {

                        if (tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                        {
                            if (tempString[tempString.Length - 1].ToString() == "0")

                                query = "select distinct tireType from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  datepart(YYYY,testTime)="+rToYear+"";
                            else if (tempString[tempString.Length - 1].ToString() == "1")

                                query = "select distinct tireType from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  datepart(YYYY,testTime)="+rToYear+"";
                            else if (tempString[tempString.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  datepart(YYYY,testTime)="+rToYear+"";

                            fillRecipeWiseGridView(query);
                            //if (tempString[tempString.Length - 1].ToString() != "1")
                            //    fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                        {

                            if (tempString[tempString.Length - 1].ToString() == "0")
                                query = "select distinct tireType  from vCuringWiseproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            if (tempString[tempString.Length - 1].ToString() == "1")
                                query = "select distinct tireType  from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                            else if (tempString[tempString.Length - 1].ToString() == "2")
                                query = "select distinct tireType from vproductiondataTUO where tireType in(select name from recipemaster where tyreSize='" + tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;

                            fillRecipeWiseGridView(query);
                            //if (tempString[tempString.Length - 1].ToString() != "1")
                            //    fillUnknownRecipeWiseGridView();
                        }
                        else if (tuoFilterPerformanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && tuoFilterPerformanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeWise.Checked)
                        {
                            showReportRecipeWise(wcnamequery);
                            //if (tempString[tempString.Length - 1].ToString() != "1")
                            //    fillUnknownRecipeWiseGridView();

                        }



                    

                }
               

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
