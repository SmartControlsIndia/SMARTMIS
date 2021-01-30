using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;

namespace SmartMIS.UserControl
{
    public partial class mould : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, rprocessID;
        public int actualQuan, planningQuan, totalPlanningQuan, totalActualQuan;
        public String[] workcenter;

        public String Visiblity
        {

            get
            {
                return mouldReportDateWisePanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                mouldReportDateWisePanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string[] tempString = magicHidden.Value.Split(new char[] { '?' });

                mouldReportDateWiseGridView.DataSource = null;
                mouldReportDateWiseGridView.DataBind();

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
                            rFromDate = formatDate(myWebService.formatDate(rFromDate));
                            string query = myWebService.createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");

                            showDailyReport(query);
                            magicHidden.Value = "";
                        }
                        else if (rChoice == "1")
                        {
                            rToMonth = tempString[5];
                            rToYear = tempString[6];
                            string query = myWebService.createQuery(rWCID, rFromDate, rToDate, "dtandTime", "dtandTime");
                            //showMonthlyReport(query);

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
            catch (Exception ex)
            {
 
            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "mouldReportDateWiseGridView")
                    {
                        String wcName = (((Label)e.Row.FindControl("mouldReportDateWiseWCNameLabel")).Text);
                        GridView childGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseChildGridViewLH"));
                        GridView childGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseChildGridViewRH"));


                        fillChildGridView(childGridViewLH, new String[] { wcName }, rToDate, rFromDate);
                        fillChildGridView(childGridViewRH, new String[] { wcName }, rToDate, rFromDate);
                    }
                    else if (((GridView)sender).ID == "mouldReportDateWiseChildGridViewLH")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((GridView)sender).Parent).Parent;
                        String wcName = (((Label)gridViewRow.Cells[1].FindControl("mouldReportDateWiseWCNameLabel")).Text);

                        String mouldNameLH = (((Label)e.Row.FindControl("mouldReportDateWiseChildMouldNameLabelLH")).Text);
                        GridView childGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseBladderChildGridViewLH"));
                        fillChildGridView(childGridViewLH, new String[] { wcName, mouldNameLH }, rToDate, rFromDate);
                    }
                    else if (((GridView)sender).ID == "mouldReportDateWiseChildGridViewRH")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((GridView)sender).Parent).Parent;
                        String wcName = (((Label)gridViewRow.Cells[1].FindControl("mouldReportDateWiseWCNameLabel")).Text);
                        String mouldNameRH = (((Label)e.Row.FindControl("mouldReportDateWiseChildMouldNameLabelRH")).Text);
                        GridView childGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseBladderChildGridViewRH"));
                        fillChildGridView(childGridViewRH, new String[] { wcName, mouldNameRH }, rToDate, rFromDate);

                    }
                    else if (((GridView)sender).ID == "mouldReportDateWiseBladderChildGridViewLH")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((GridView)sender).Parent).Parent;
                        String mouldNameLH = (((Label)gridViewRow.Cells[1].FindControl("mouldReportDateWiseChildMouldNameLabelLH")).Text);

                        GridViewRow parentGridViewRow = (GridViewRow)((DataControlFieldCell)(((GridView)sender).Parent.Parent.Parent).Parent.Parent).Parent;
                        String wcName = (((Label)parentGridViewRow.Cells[1].FindControl("mouldReportDateWiseWCNameLabel")).Text);

                        String bladderNameLH = (((Label)e.Row.FindControl("mouldReportDateWiseChildBladderNameLabelLH")).Text);
                        GridView childRecipeGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseRecipeChildGridViewLH"));
                        GridView childTotalGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseTotalChildGridViewLH"));

                        GridView childShiftAGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftAChildGridViewLH"));
                        GridView childShiftBGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftBChildGridViewLH"));
                        GridView childShiftCGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftCChildGridViewLH"));

                        fillChildGridView(childRecipeGridViewLH, new String[] { wcName, mouldNameLH, bladderNameLH }, rToDate, rFromDate);

                        fillChildGridView(childShiftAGridViewLH, new String[] { wcName, mouldNameLH, bladderNameLH }, rToDate, rFromDate);
                        fillChildGridView(childShiftBGridViewLH, new String[] { wcName, mouldNameLH, bladderNameLH }, rToDate, rFromDate);
                        fillChildGridView(childShiftCGridViewLH, new String[] { wcName, mouldNameLH, bladderNameLH }, rToDate, rFromDate);
                        fillChildGridView(childTotalGridViewLH, new String[] { wcName, mouldNameLH, bladderNameLH }, rToDate, rFromDate);
                    }
                    else if (((GridView)sender).ID == "mouldReportDateWiseBladderChildGridViewRH")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((GridView)sender).Parent).Parent;
                        String mouldNameRH = (((Label)gridViewRow.Cells[1].FindControl("mouldReportDateWiseChildMouldNameLabelRH")).Text);

                        GridViewRow parentGridViewRow = (GridViewRow)((DataControlFieldCell)(((GridView)sender).Parent.Parent.Parent).Parent.Parent).Parent;
                        String wcName = (((Label)parentGridViewRow.Cells[1].FindControl("mouldReportDateWiseWCNameLabel")).Text);

                        String bladderNameRH = (((Label)e.Row.FindControl("mouldReportDateWiseChildBladderNameLabelRH")).Text);
                        GridView childRecipeGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseRecipeChildGridViewRH"));

                        GridView childShiftAGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftAChildGridViewRH"));
                        GridView childShiftBGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftBChildGridViewRH"));
                        GridView childShiftCGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseShiftCChildGridViewRH"));
                        GridView childTotalGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseTotalChildGridViewRH"));

                        fillChildGridView(childRecipeGridViewRH, new String[] { wcName, mouldNameRH, bladderNameRH }, rToDate, rFromDate);
                        fillChildGridView(childShiftAGridViewRH, new String[] { wcName, mouldNameRH, bladderNameRH }, rToDate, rFromDate);
                        fillChildGridView(childShiftBGridViewRH, new String[] { wcName, mouldNameRH, bladderNameRH }, rToDate, rFromDate);
                        fillChildGridView(childShiftCGridViewRH, new String[] { wcName, mouldNameRH, bladderNameRH }, rToDate, rFromDate);
                        fillChildGridView(childTotalGridViewRH, new String[] { wcName, mouldNameRH, bladderNameRH }, rToDate, rFromDate);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void showDailyReport(string query)
        {
            try
            {
                fillGridView("Select DISTINCT iD, workCenterName from vWorkCenter WHERE " + query + "");
            }
            catch(Exception ex)
            {

            }
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Rohit Singh
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011
            //Revision No.  : 01z
            try
            {
                if (rType == "1" && rChoice == "0")
                {

                    mouldReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    mouldReportDateWiseGridView.DataBind();
                }

                else if (rType == "1" && rChoice == "1")
                {
                    mouldReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    mouldReportDateWiseGridView.DataBind();
                }
            }
            catch(Exception exp)
            {

            }
        }

        private void fillChildGridView(GridView childGridView, String[] args, String toDate, String fromDate)
        {
            try
            {
                if (childGridView.ID == "mouldReportDateWiseChildGridViewLH")
                {
                    childGridView.DataSource = GetMouldTableLH(args[0]);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseChildGridViewRH")
                {
                    childGridView.DataSource = GetMouldTableRH(args[0]);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseBladderChildGridViewLH")
                {
                    childGridView.DataSource = GetBladderTableLH(args[0], new string[] { args[1] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseBladderChildGridViewRH")
                {
                    childGridView.DataSource = GetBladderTableRH(args[0], new string[] { args[1] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseRecipeChildGridViewLH")
                {
                    childGridView.DataSource = GetRecipeTableLH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseRecipeChildGridViewRH")
                {
                    childGridView.DataSource = GetRecipeTableRH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftAChildGridViewLH")
                {
                    childGridView.DataSource = GetAShiftCountLH(args[0], new string[] { args[1], args[2]});
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftAChildGridViewRH")
                {
                    childGridView.DataSource = GetAShiftCountRH(args[0], new string[] { args[1], args[2]});
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftBChildGridViewLH")
                {
                    childGridView.DataSource = GetBShiftCountLH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftBChildGridViewRH")
                {
                    childGridView.DataSource = GetBShiftCountRH(args[0], new string[] { args[1], args[2]});
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftCChildGridViewLH")
                {
                    childGridView.DataSource = GetCShiftCountLH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseShiftCChildGridViewRH")
                {
                    childGridView.DataSource = GetCShiftCountRH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseTotalChildGridViewLH")
                {
                    childGridView.DataSource = GetBladderCountLH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseTotalChildGridViewRH")
                {
                    childGridView.DataSource = GetBladderCountRH(args[0], new string[] { args[1], args[2] });
                    childGridView.DataBind();
                }
            }
            catch(Exception ex)

            {
            }
        }

        public String objName(Object objMouldName)
        {
            String flag = Convert.ToString(objMouldName);

            if (String.Empty == flag)
                flag = "";

            return flag;
        }

        private DataTable GetMouldTableLH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("mouldCodeLH", typeof(string));
             DataRow dr=flag.NewRow();

             try
             {
                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();

                 myConnection.comm.CommandText = "Select distinct mouldCodeLH from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime)";
                 myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                 myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                 myConnection.reader = myConnection.comm.ExecuteReader();
                 while (myConnection.reader.Read())
                 {
                     dr = flag.NewRow();
                   
                     if (DBNull.Value != (myConnection.reader[0]))
                     {
                        
                         dr[0] = myConnection.reader[0].ToString();
                     }

                     flag.Rows.Add(dr);
                 }
               
             }
             catch (Exception exp)
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

        private DataTable GetMouldTableRH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("mouldCodeRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select distinct  mouldCodeRH from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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


        private DataTable GetBladderTableLH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("bladderCodeLH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select distinct bladderCodeLH from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeLH=@mouldCodeLH)";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeLH", value[0].ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetRecipeTableLH(string TableName, string[] value)
        {
             DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("recipeName", typeof(string));
             DataRow dr=flag.NewRow();

             try
             {
                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();

                 myConnection.comm.CommandText = "Select distinct recipename from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeLH=@mouldCodeLH) and (bladderCodeLH=@bladderCodeLH)";
                 myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                 myConnection.comm.Parameters.AddWithValue("@mouldCodeLH",value[0].ToString());
                 myConnection.comm.Parameters.AddWithValue("@bladderCodeLH",value[1].ToString());
                 myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                 myConnection.reader = myConnection.comm.ExecuteReader();
                 while (myConnection.reader.Read())
                 {
                     dr = flag.NewRow();
                   
                     if (DBNull.Value != (myConnection.reader[0]))
                     {
                        
                         dr[0] = myConnection.reader[0].ToString();
                     }

                     flag.Rows.Add(dr);
                 }
               
             }
             catch (Exception exp)
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

        private DataTable GetAShiftCountLH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("AshiftCountLH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountLH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeLH=@mouldCodeLH) and (bladderCodeLH=@bladderCodeLH)  and shift='A'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeLH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeLH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetBShiftCountLH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("BshiftCountLH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountLH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeLH=@mouldCodeLH) and (bladderCodeLH=@bladderCodeLH)  and shift='B'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeLH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeLH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetCShiftCountLH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CshiftCountLH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountLH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeLH=@mouldCodeLH) and (bladderCodeLH=@bladderCodeLH)  and shift='C'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeLH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeLH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    else
                    {
                        dr[0] = 0;
                    }
                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetBladderTableRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("bladderCodeRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select distinct bladderCodeRH from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeRH=@mouldCodeRH)";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeRH", value[0].ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetRecipeTableRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("recipeName", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select distinct recipename from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeRH=@mouldCodeRH) and (bladderCodeRH=@bladderCodeRH)";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeRH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeRH", value[1].ToString());
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetAShiftCountRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("AshiftCountRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountRH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeRH=@mouldCodeRH) and (bladderCodeRH=@bladderCodeRH)  and shift='A'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeRH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeRH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }

                    else
                    {
                        dr[0] = 0;
                    }



                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetBShiftCountRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("BshiftCountRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountRH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeRH=@mouldCodeRH) and (bladderCodeRH=@bladderCodeRH)  and shift='B'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeRH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeRH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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
        private DataTable GetCShiftCountRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CshiftCountRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountRH) from vCuringmouldData Where (wcname = @wcname) and (DtandTime=@dtandTime) and (mouldCodeRH=@mouldCodeRH) and (bladderCodeRH=@bladderCodeRH)  and shift='C'";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@mouldCodeRH", value[0].ToString());
                myConnection.comm.Parameters.AddWithValue("@bladderCodeRH", value[1].ToString());

                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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

        private DataTable GetBladderCountRH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("TotalBladderCountRH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountRH) from vCuringmouldData Where (wcname = @wcname) and (bladderCodeRH=@bladderCodeRH) and (dtandTime=@dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@bladderCodeRH", value[1].ToString());
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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
        private DataTable GetBladderCountLH(string TableName, string[] value)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("TotalBladderCountLH", typeof(string));
            DataRow dr = flag.NewRow();

            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select  sum(bladderCountLH) from vCuringmouldData Where (wcname = @wcname) and (bladderCodeLH=@bladderCodeLH) and (dtandTime=@dtandTime) ";
                myConnection.comm.Parameters.AddWithValue("@wcname", Convert.ToInt32(wcname));
                myConnection.comm.Parameters.AddWithValue("@bladderCodeLH", value[1].ToString());
                myConnection.comm.Parameters.AddWithValue("@dtandTime", myWebService.formatDate(rToDate));


                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    dr = flag.NewRow();

                    if (DBNull.Value != (myConnection.reader[0]))
                    {

                        dr[0] = myConnection.reader[0].ToString();
                    }
                    else
                    {
                        dr[0] = 0;
                    }

                    flag.Rows.Add(dr);
                }

            }
            catch (Exception exp)
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


        private String SetFileName()
        {
            String fileName = "";
            String[] tempDate = rToDate.Split(new char[] {'-'});

            fileName = tempDate[0] + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("MMM") + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("yy");
                
            return fileName;
        }

        private String SetDayName(String day)
        {
            String flag = "";

            if (day.Length < 2)
                flag = "0" + day;
            else
                flag = day;

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

       