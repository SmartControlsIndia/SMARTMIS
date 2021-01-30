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
    public partial class performanceReportTUOWise : System.Web.UI.UserControl
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        public int count;
           

        #endregion
        myConnection myConnection = new myConnection();

        #region globle variable
        public string rType, rWCID,option, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, workcentername, wcnamequery, wcIDQuery;
        string dtnadtime1 = "";

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
                
                if (!IsPostBack)
                {


                    if (Session["userID"].ToString().Trim() == "")
                    {
                        Response.Redirect("/SmartMIS/Default.aspx", true);
                    }
                    else
                    {
                        wcnamequery = wcquery();
                        reportHeader._rDate = reportMasterFromDateTextBox.Text;

                        if (QualityReportTUOWise.Checked)
                        {
                            performanceReportTUOWiseSizeDropdownlist.Enabled = false;
                            performanceReportTUOWiseRecipeDropdownlist.Enabled = false;

                            QualityReportTUOWisePanel.Visible = true;
                            QualityReportRecipeTUOWisePanel.Visible = false;
                        }
                        else
                        {
                            performanceReportTUOWiseSizeDropdownlist.Enabled = true;
                            performanceReportTUOWiseRecipeDropdownlist.Enabled = true;
                            QualityReportTUOWisePanel.Visible = false;
                            QualityReportRecipeTUOWisePanel.Visible = true;


                        }

                        //Compare the hidden field if it contains the query string or not
                    }
                }
            }
            catch (Exception exp)
            {


            }
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {

        }

        protected void Button_Click(object sender, EventArgs e)
            {

                rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);

            
                try
                {
                    if (((Button)sender).ID == "ErankViewDetailButton")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                        string wcname = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseMachineNameLabel")).Text);
                        string recipeCode = (((Label)gridViewRow.Cells[1].FindControl("performanceReportTUOWiseTyreTypeLabel")).Text);
                        fillBarCodeDetailGridView(wcname, recipeCode);

                        ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);
                    }
                    else
                    {

                    }
                    if (((Button)sender).ID == "ErankRecipeWiseViewDetailButton")
                    {
                        GridViewRow gridviewrow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                        string recipeCode = (((Label)gridviewrow.Cells[1].FindControl("performanceReportRecipeTUOWiseTyreTypeLabel")).Text);
                        fillBarCodeDetailGridView(recipeCode);

                        ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                    }

                    if (((Button)sender).ID == "AllErankDetailButton")
                    {
                        fillBarCodeDetailGridView();

                        ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                    }


                    if (((Button)sender).ID == "AllRecipeErankDetailButton")
                    {
                        fillBarCodeDetailGridView();

                        ScriptManager.RegisterClientScriptBlock(ViewButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                    }
                    else
                    { 

                    }
                }
             catch(Exception exp)
                {
                 

             }
         }

        private void fillBarCodeDetailGridView(string wcname,string recipecode)
         {
             string query = "select wcname, machinename,tireType,barCode from vproductiondataTUO where machinename='" + wcname + "' and tireType='" + recipecode + "' and uniformitygrade='E'  and testtime>='" + formattoDate(rToDate) + "' and testtime<='" + formatfromDate(rToDate) + "'";

            performanceReportBarcodeDetailGridView.DataSource= myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();
 
         }

        private void fillBarCodeDetailGridView(string recipecode)
        {



            string query = "select wcname, machinename,TireType,barCode from vproductiondataTUO where tireType='" + recipecode + "' and uniformitygrade='E'  and testtime>='"+formattoDate(rToDate)+"' and testtime<='"+formatfromDate(rToDate)+"'";

            performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            performanceReportBarcodeDetailGridView.DataBind();

        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            count = 0;
            rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);

            string dtnadtime = TotalformatDate(rToDate);
            string query;
            if (QualityReportTUOWise.Checked == true)
            {
                fillGridView("Select DISTINCT  machinename from vproductiondataTUO order by machinename asc");

            }

            else if (((DropDownList)sender).ID == "performanceReportTUOWiseRecipeDropdownlist")
            {
                performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All")); 

                 query = "select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                   showReportSizeORRecipeWise(query);
            
            }
            else if (((DropDownList)sender).ID == "performanceReportTUOWiseSizeDropdownlist")
            {
                performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All")); 

                 query = "select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                showReportSizeORRecipeWise(query);

             

 
            }
            else if (((DropDownList)sender).ID == "optionDropDownList")
            {
                if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {

                    query = "select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                    showReportSizeORRecipeWise(query);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    query = "select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  ((testTime>" + dtnadtime;
                    showReportSizeORRecipeWise(query);
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && QualityReportRecipeTUOWise.Checked == true)
                {
                    showReportRecipeWise();
 
                }


            }
            
 
        }

        private void fillBarCodeDetailGridView()
         {
             string query = "select wcname,machinename,tireType,barCode from vproductiondataTUO where uniformitygrade='E'  and testtime>='"+formattoDate(rToDate)+"' and testtime<='"+formatfromDate(rToDate)+"'";
             performanceReportBarcodeDetailGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
             performanceReportBarcodeDetailGridView.DataBind();

         }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (optionDropDownList.SelectedItem.Text == "No")
                    option = "1";
                else
                    option = "2";
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "performanceReportTUOWiseMainGridView")
                    {
                        Label wcnameLabel = ((Label)e.Row.FindControl("performanceReportTUOWiseWCNameLabel"));
                        workcentername = wcnameLabel.Text.ToString();
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTUOWiseGridView"));
                        showReport1(childGridView, workcentername, rToDate, rFromDate);
                    }


                    if (((GridView)sender).ID == "performanceReportTUOWiseGridView")
                    {
                        string[] tempValue = tyreTypeHidden.Value.Split(new char[] { '?' });

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportTUOWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportTUOWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);
                    }

                    if (((GridView)sender).ID == "performanceReportRecipeWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportSizeWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportrecipeWiseChildGridView"));

                        fillChildInnerGridView(workcentername, childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);
                    }

                    if (((GridView)sender).ID == "performanceReportRecipeTUOWiseGridView")
                    {

                        Label recipeCodeLabel = ((Label)e.Row.FindControl("performanceReportRecipeTUOWiseTyreTypeLabel"));
                        GridView childGridView = ((GridView)e.Row.FindControl("performanceReportrecipeTUOWiseChildGridView"));

                        fillChildInnerGridView("3601", childGridView, recipeCodeLabel.Text.Trim(), formattoDate(rToDate), formatfromDate(rToDate), option);

                    }
                }
                else
                {

                }

            }

            catch (Exception exp)
            {
            }
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {      
           
                 //count++;
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "ALERT", "alert('welcome');", true);
                 System.Threading.Thread.Sleep(3000);

                rToDate = myWebService.formatDate(reportMasterFromDateTextBox.Text);
                reportHeader._rDate = reportMasterFromDateTextBox.Text;
                performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

                performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));

                string dtnadtime = TotalformatDate(rToDate);

                dtnadtime1 = TotalprodataformatDate(rToDate);

                if (QualityReportTUOWise.Checked)
                {
                    fillGridView("Select DISTINCT  machinename from productiondataTUO order by machinename asc");

                    QualityReportTUOWisePanel.Visible = true;
                    QualityReportRecipeTUOWisePanel.Visible = false;
                }
                if (QualityReportRecipeTUOWise.Checked)
                {
                    fillRecipeWiseGridView("Select distinct tireType as description FROM productionDataTUO WHERE  ((testTime>" + dtnadtime);

                    QualityReportTUOWisePanel.Visible = false;
                    QualityReportRecipeTUOWisePanel.Visible = true;
                }

           

  
        }


        protected void QualityReportRecipeTUOWise_CheckedChanged(object sender, EventArgs e)
        {
            count = 0;
            performanceReportTUOWiseSizeDropdownlist.Enabled = true;
            performanceReportTUOWiseRecipeDropdownlist.Enabled = true;
            fillSizedropdownlist();
            fillDesigndropdownlist();
            QualityReportTUOWisePanel.Visible = false;
            QualityReportRecipeTUOWisePanel.Visible = true;


        }

        protected void QualityReportTUOWise_CheckedChanged(object sender, EventArgs e)
        {
            count = 0;
            performanceReportTUOWiseRecipeDropdownlist.SelectedIndex = performanceReportTUOWiseRecipeDropdownlist.Items.IndexOf(performanceReportTUOWiseRecipeDropdownlist.Items.FindByText("All"));

            performanceReportTUOWiseSizeDropdownlist.SelectedIndex = performanceReportTUOWiseSizeDropdownlist.Items.IndexOf(performanceReportTUOWiseSizeDropdownlist.Items.FindByText("All"));
            performanceReportTUOWiseSizeDropdownlist.Enabled = false;
            performanceReportTUOWiseRecipeDropdownlist.Enabled = false;
            QualityReportTUOWisePanel.Visible = true;
            QualityReportRecipeTUOWisePanel.Visible = false;

        }


        #endregion

        #region User Defined Function

        private void showReportSizeORRecipeWise(string query)
        {
           

            fillRecipeWiseGridView(query);
          
        }

        private void showReport()
        {
            string dtnadtime = TotalformatDate(rToDate);

            fillGridView("Select DISTINCT  machinename from vproductiondataTUO order by machinename asc");
            fillRecipeWiseGridView("Select distinct tireType as description FROM productionDataTUO WHERE  ((testTime>" + dtnadtime);

            
        }

        private void showReportRecipeWise()
        {
            string dtnadtime = TotalformatDate(rToDate);

            fillRecipeWiseGridView("Select distinct tireType as description FROM productionDataTUO WHERE  ((testTime>" + dtnadtime);


        }

        public void showReport1(GridView childgridview, string wcname, string rToDate, string rFromDate)
        {
            try
            {
               
                string dtnadtime = TotalformatDate(rToDate);

                fillChildGridView(childgridview, "Select distinct tireType as description,machineName FROM productionDataTUO WHERE  machinename='" + wcname + "' AND ((testTime>" + dtnadtime);

              
            }
            catch (Exception ex)
            {

            }
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 13 June 2013
            //Date Updated  : 13 June 2013
            //Revision No.  : 01
            try
            {

                performanceReportTUOWiseMainGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportTUOWiseMainGridView.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        private void fillRecipeWiseGridView(string query)
        {

            //Description   : Function for filling performanceReportSizeWiseGridView TyreType
            //Author        : Brajesh kumar
            //Date Created  : 12 July 2013
            //Date Updated  : 12 July 2013
            //Revision No.  : 01
            try
            {

                performanceReportRecipeTUOWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                performanceReportRecipeTUOWiseGridView.DataBind();
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

                if (childGridView.ID == "performanceReportTUOWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportTUOWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }
                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportTUOWise_Percent", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
                    }

                }
                if (childGridView.ID == "performanceReportrecipeTUOWiseChildGridView")
                {
                    if (option == "1")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportTUORecipeWise_Nos", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

                        childGridView.DataBind();
 
                    }


                    else if (option == "2")
                    {
                        childGridView.DataSource = fillGridView("sp_performanceReportTUORecipeWise_Percent", wcname, recipecode, toDate, fromDate, ConnectionOption.SQL);

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

        public string fillCuringWCName(Object barcode)
        {
            string  flag = "None";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select wcname from vcuringpcr where gtbarcode = '"+barcode.ToString()+"'";
               

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        flag = myConnection.reader[0].ToString();
                    else
                        flag ="NOne";
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

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ")";
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
            Double totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public Double AlltotalBQuantity()
        {
            Double flag = 0;
            Double totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public Double AlltotalCQuantity()
        {
            Double flag = 0;
            Double totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'C' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public Double AlltotalDQuantity()
        {
            Double flag = 0;
            Double totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'D' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public Double AlltotalEQuantity()
        {
            Double flag = 0;
            Double totalchecked = AlltotalcheckedQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'E' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public int AlltotalcheckedRecipeWiseQuantity()
        {
            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }
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

        public Double AlltotalRecipeWiseAQuantity()
        {
            Double flag = 0;
            Double totalchecked = AlltotalcheckedRecipeWiseQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();



                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A'and  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'A' and testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }

                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

        public Double AlltotalRecipeWiseBQuantity()
        {
            Double flag = 0;
            Double totalcheked = AlltotalcheckedRecipeWiseQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B'and  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'B' and testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalcheked;
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

        public Double AlltotalRecipeWiseCQuantity()
        {
            Double flag = 0;
            Double totalcheked = AlltotalcheckedRecipeWiseQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'C' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'C'and  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'C' and testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));



                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalcheked;
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

        public Double AlltotalRecipeWiseDQuantity()
        {
            Double flag = 0;
            Double totalcheked = AlltotalcheckedRecipeWiseQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'D' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'D'and  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'D' and testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (DBNull.Value != (myConnection.reader[0]))
                        if (option == "1")
                            flag = Convert.ToInt32(myConnection.reader[0]);
                        else
                        {
                            flag = (Convert.ToInt32(myConnection.reader[0]) * 100) / totalcheked;
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

        public Double AlltotalRecipeWiseEQuantity()
        {
            Double flag = 0;

            Double totalchecked = AlltotalcheckedRecipeWiseQuantity();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.Text == "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'E' and testTime>=@todate and testTime<=@fromdate and( " + wcnamequery + ")";
                }
                else if (performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'E'and  testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreSize='" + performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";
                }
                else if (performanceReportTUOWiseSizeDropdownlist.SelectedItem.Text == "All" && performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text != "All")
                {
                    myConnection.comm.CommandText = "select COUNT(*) from ProductionDataTUO where UniformityGrade = 'E' and testtime>=@todate and testtime<=@fromdate and( " + wcnamequery + ") and tireType in(select distinct tireType as description from productiondataTUO where tireType in(select name from recipemaster where tyreDesign='" + performanceReportTUOWiseRecipeDropdownlist.SelectedItem.Text + "')and  testtime>=@todate and testtime<=@fromdate)";

                }
                myConnection.comm.Parameters.AddWithValue("@todate", formattoDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@fromdate", formatfromDate(rToDate));


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

                flag = "'" + flag1 + "' " + "and" + " " + "testTime<'" + flag2 + "' " + ")OR" + " " + "(testTime>'" + flag3 + "'and" + " " + "testTime<" + "'" + flag4 + "'))";

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

        public string wcquery()
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "select distinct machinename from productiondatatuo";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {

                    if (flag != "")
                    {
                        flag = flag + "or" + " " + "machinename = '" + myConnection.reader[0] + "'";
                    }
                    else
                    {
                        flag = "machinename = '" + myConnection.reader[0] + "'";

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



        private void fillSizedropdownlist()

        {

            performanceReportTUOWiseSizeDropdownlist.DataSource = null;
            performanceReportTUOWiseSizeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreSize");
            performanceReportTUOWiseSizeDropdownlist.DataBind();
        }

        private void fillDesigndropdownlist()
        {

            performanceReportTUOWiseRecipeDropdownlist.DataSource = null;
            performanceReportTUOWiseRecipeDropdownlist.DataSource = FillDropDownList("recipemaster", "tyreDesign");
            performanceReportTUOWiseRecipeDropdownlist.DataBind();
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
                    if(myConnection.reader[0].ToString()!="")
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

        public ArrayList FillDropDownList(string tableName, string coloumnName, string whereClause)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                sqlQuery = "Select DISTINCT " + coloumnName + " from " + tableName + " " + whereClause + "";

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
        #endregion


    }
}