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
    public partial class BladderReport : System.Web.UI.UserControl
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
                return bladderReportDateWisePanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                bladderReportDateWisePanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string[] tempString = magicHidden.Value.Split(new char[] { '?' });

                bladderReportDateWiseGridView.DataSource = null;
                bladderReportDateWiseGridView.DataBind();

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
                    if (((GridView)sender).ID == "bladderReportDateWiseGridView")
                    {
                        String wcName = (((Label)e.Row.FindControl("bladderReportDateWiseWCNameLabel")).Text);
                        GridView childGridViewLH = ((GridView)e.Row.FindControl("bladderReportDateWiseChildGridViewLH"));
                        GridView childGridViewRH = ((GridView)e.Row.FindControl("bladderReportDateWiseChildGridViewRH"));


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

                    bladderReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    bladderReportDateWiseGridView.DataBind();
                }

                else if (rType == "1" && rChoice == "1")
                {
                    bladderReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                    bladderReportDateWiseGridView.DataBind();
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
                if (childGridView.ID == "bladderReportDateWiseChildGridViewLH")
                {
                    childGridView.DataSource = GetBladderTableLH(args[0]);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "bladderReportDateWiseChildGridViewRH")
                {
                    childGridView.DataSource = GetBladderTableRH(args[0]);
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
        private DataTable GetBladderTableLH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CurrentBladderHeatLH", typeof(string));

            flag.Columns.Add("OldBladderHeatLH", typeof(string));

            DataRow dr = flag.NewRow();


            dr[0] = getBladdercountLH(wcname);
            dr[1] = getoldBladdercountLH(wcname);

            flag.Rows.Add(dr);


            return flag;
        }
        private int getBladdercountLH(string wcname)
        {

            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 bladderCountLH from vCuringbladderReport Where (wcname = @wcname) and bladderCountLH!=0  order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {

                    flag = Convert.ToInt32(myConnection.reader[0]);

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
        private int getoldBladdercountLH(string wcname)
        {
            int flag = 0;
            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 bladderCountLH from vCuringbladderReport Where (wcname = @wcname) and  bladderCountLH!=0 order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {

                    flag = Convert.ToInt32(myConnection.reader[0]);

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
        private DataTable GetBladderTableRH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CurrentBladderHeatRH", typeof(string));

            flag.Columns.Add("OldBladderHeatRH", typeof(string));

            DataRow dr = flag.NewRow();


            dr[0] = getBladdercountRH(wcname);
            dr[1] = getoldBladdercountRH(wcname);

            flag.Rows.Add(dr);


            return flag;
        }
        private int getBladdercountRH(string wcname)
        {

            int flag = 0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 bladderCountRH from vCuringBladderReport Where (wcname = @wcname) and bladderCountRH!=0  order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {

                    flag = Convert.ToInt32(myConnection.reader[0]);

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
        private int getoldBladdercountRH(string wcname)
        {
            int flag = 0;
            try
            {

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 bladderCountRH from vCuringBladderReport Where (wcname = @wcname) and  bladderCountRH!=0 order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {

                    flag = Convert.ToInt32(myConnection.reader[0]);

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