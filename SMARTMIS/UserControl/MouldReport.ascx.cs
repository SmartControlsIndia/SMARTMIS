using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;

namespace SmartMIS.UserControl
{
    public partial class MouldReport : System.Web.UI.UserControl
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
                   
                    
                }
            }
            catch (Exception ex)
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
            catch (Exception exp)
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
            }
            catch (Exception ex)
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
            flag.Columns.Add("CurrentMouldCodeLH", typeof(string));

            flag.Columns.Add("CurrentMouldHeatLH", typeof(string));

            flag.Columns.Add("OldMouldCodeLH", typeof(string));

            flag.Columns.Add("OldMouldHeatLH", typeof(string));

            DataRow dr = flag.NewRow();

            String[] tempinfocurrent=getmouldcountLH(wcname).Split(new char[] {'#'});
            dr[0] = tempinfocurrent[1];
            dr[1] = Convert.ToInt32(tempinfocurrent[0]);
            String[] tempinfoold = getoldmouldcountLH(wcname).Split(new char[] {'#'});
            dr[2] = tempinfoold[1].ToString();
            dr[3] =Convert.ToInt32(tempinfoold[0]);

                flag.Rows.Add(dr);


            return flag;
        }
        private String getmouldcountLH(string wcname)
        {

            string flag=0+"#"+"NULL";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 mouldCountLH,mouldCodeLH from vCuringmouldReport Where (wcname = @wcname) and MouldCountLH!=0  order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {
                    flag = "";
                    flag = myConnection.reader[0]+"#"+myConnection.reader[1].ToString();

                }
            }
            catch (Exception exc)
            {

            }
            finally
            {
                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;

        }
        private String getoldmouldcountLH(string wcname)
        {
            string flag = 0 + "#" + "NULL";

            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 mouldCountLH,mouldCodeLH from vCuringmouldReport Where (wcname = @wcname) and  mouldCountLH!=0 order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = "";

                    flag = Convert.ToInt32(myConnection.reader[0])+"#"+myConnection.reader[1].ToString();

                }
            }
            catch (Exception exc)
            {

            }
            finally
            {

                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.open(ConnectionOption.SQL);
            }

            return flag;
        }
        private DataTable GetMouldTableRH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;

            flag.Columns.Add("CurrentMouldCodeRH", typeof(string));

            flag.Columns.Add("CurrentMouldHeatRH", typeof(string));

            flag.Columns.Add("OldMouldCodeRH", typeof(string));

            flag.Columns.Add("OldMouldHeatRH", typeof(string));

            DataRow dr = flag.NewRow();



            String[] tempinfocurrent = getmouldcountRH(wcname).Split(new char[] { '#' });
            dr[0] = tempinfocurrent[1];
            dr[1] = Convert.ToInt32(tempinfocurrent[0]);
            String[] tempinfoold = getoldmouldcountRH(wcname).Split(new char[] { '#' });
            dr[2] = tempinfoold[1].ToString();
            dr[3] = Convert.ToInt32(tempinfoold[0]);

            flag.Rows.Add(dr);


            return flag;
        }
        private String getmouldcountRH(string wcname)
        {

            String flag = 0+"#"+"NULL";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 mouldCountRH,mouldCodeRH from vCuringmouldReport Where (wcname = @wcname) and MouldCountRH!=0  order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {
                    flag = "";

                    flag = myConnection.reader[0]+"#"+myConnection.reader[1];

                }
            }
            catch (Exception exc)
            {

            }
            finally
            {
                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;

        }
        private String getoldmouldcountRH(string wcname)
        {
            String flag = 0+"#"+"NULL";
            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 mouldCountRH,mouldCodeRH from vCuringmouldReport Where (wcname = @wcname) and  mouldCountRH!=0 order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = "";
                    flag =myConnection.reader[0]+"#"+myConnection.reader[1];

                }
            }
            catch (Exception exc)
            {

            }
            finally
            {

                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.open(ConnectionOption.SQL);
            }

            return flag;
        }
        private String SetFileName()
        {
            String fileName = "";
            String[] tempDate = rToDate.Split(new char[] { '-' });

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