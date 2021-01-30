using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace SmartMIS
{
    public partial class dailySummaryReportControl : System.Web.UI.UserControl
    {
      #region Global Variables
        DataTable dTable;
        smartMISWebService myWebService = new smartMISWebService();
        int totDPlan, totDactual, totDDiff, totMPlan, totMActual, totMDiff, totYPlan, totYActual, totYDiff;
        #endregion
        myConnection myConnection = new myConnection();

      #region System Defined Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            fillGridView();      
        }

        protected void Gridview_RowBound(object sender, GridViewRowEventArgs e)
        {            
           if (e.Row.RowType == DataControlRowType.DataRow)
           {
                Label dayPlan=new Label();
                Label dayActual = new Label();
                Label dayDiff = new Label();
                Label monthPlan = new Label();
                Label monthActual = new Label();
                Label monthDiff = new Label();
                Label yearPlan = new Label();
                Label yearActual = new Label();
                Label yearDiff = new Label();
               
                Chart prodSummaryChart = ((Chart)e.Row.FindControl("prodSummaryReportDailyChart"));
                Label type = ((Label)e.Row.FindControl("prodSummaryTypeLabel"));
                dayPlan = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildDayPlanLabel"));
                dayActual = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildDayActualLabel"));
                dayDiff = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildDayDifferenceLabel"));
                monthPlan = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildMonthPlanLabel"));
                monthActual = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildMonthActualLabel"));
                monthDiff = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildMonthDifferenceLabel"));
                yearPlan = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildYearPlanLabel"));
                yearActual = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildYearActualLabel"));
                yearDiff = ((Label)e.Row.FindControl("productionReportDateWiseInnerChildYearDifferenceLabel"));            

                double[] plan = { Convert.ToDouble(dayPlan.Text.Trim()), Convert.ToDouble(monthPlan.Text.Trim()), Convert.ToDouble(yearPlan.Text.Trim()) };
                double[] actual = { Convert.ToDouble(dayActual.Text.Trim()), Convert.ToDouble(monthActual.Text.Trim()), Convert.ToDouble(yearActual.Text.Trim()) };
                double[] diff = { Convert.ToDouble(dayDiff.Text.Trim()), Convert.ToDouble(monthDiff.Text.Trim()), Convert.ToDouble(yearDiff.Text.Trim()) };

                formatChart(prodSummaryChart, plan, actual, diff);
            }
        }

        protected void Gridview_RowTotalBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label dayPlan = new Label();
                Label dayActual = new Label();
                Label dayDiff = new Label();
                Label monthPlan = new Label();
                Label monthActual = new Label();
                Label monthDiff = new Label();
                Label yearPlan = new Label();
                Label yearActual = new Label();
                Label yearDiff = new Label();

                Chart prodSummaryChart = ((Chart)e.Row.FindControl("prodSummaryReportTotalChart"));
                Label type = ((Label)e.Row.FindControl("prodSummaryTotalTypeLabel"));
                dayPlan = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildDayPlanLabel"));
                dayActual = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildDayActualLabel"));
                dayDiff = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildDayDifferenceLabel"));
                monthPlan = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildMonthPlanLabel"));
                monthActual = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildMonthActualLabel"));
                monthDiff = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildMonthDifferenceLabel"));
                yearPlan = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildYearPlanLabel"));
                yearActual = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildYearActualLabel"));
                yearDiff = ((Label)e.Row.FindControl("productionReportDateWiseTotalInnerChildYearDifferenceLabel"));

                double[] plan = { Convert.ToDouble(dayPlan.Text.Trim()), Convert.ToDouble(monthPlan.Text.Trim()), Convert.ToDouble(yearPlan.Text.Trim()) };
                double[] actual = { Convert.ToDouble(dayActual.Text.Trim()), Convert.ToDouble(monthActual.Text.Trim()), Convert.ToDouble(yearActual.Text.Trim()) };
                double[] diff = { Convert.ToDouble(dayDiff.Text.Trim()), Convert.ToDouble(monthDiff.Text.Trim()), Convert.ToDouble(yearDiff.Text.Trim()) };

                formatChart(prodSummaryChart, plan, actual, diff);
            }
        }

    #endregion

      #region User Defined Function

          public string formatDate()
        {
            string flag = "";
            string month, day, year;
            string[] tempDate = DateTime.Now.ToShortDateString().Split(new char[] { '/' });
            month = tempDate[0].ToString();
            day = tempDate[1].ToString();
            year = tempDate[2].ToString();
            flag = month + "-" + day + "-" + year;
            return flag;
        }

          public string plannedQuantityDay(object type)
        {
                    string flag = "";
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where PlanDtandTime >= '"+DateTime.Now.ToShortDateString()+"' and PlanDtandTime<'"+DateTime.Now.AddDays(1).ToShortDateString()+"' and processName like '%"+type+"%' and ActualDtandTime>='"+DateTime.Now.ToShortDateString()+"'  and ActualDtandTime <'"+DateTime.Now.AddDays(1).ToShortDateString()+"'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        flag = myConnection.reader[0].ToString();
                    }
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();  

                    if (flag.Equals(""))
                    {
                        flag = "0";
                    }
                  
                    return flag;
        }

          public string plannedQuantityMonth(object type)
            {
                    string flag = "";
             
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where Month(PlanDtandTime) = '"+DateTime.Now.Month+"' and Year(PlanDtandTime)='"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Month(ActualDtandTime)='"+DateTime.Now.Month+"' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        flag = myConnection.reader[0].ToString();
                    }
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    if (flag.Equals(""))
                    {
                        flag = "0";
                    }
                    return flag;
             }

          public string plannedQuantityYear(object type)
         {
                    string flag = "";
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where Year(PlanDtandTime) = '"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        flag = myConnection.reader[0].ToString();
                    }
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    if (flag.Equals(""))
                    {
                        flag = "0";
                    }

                    return flag;
            }

          public string actualQuantityDay(object type)
          {
              string flag = "";
              myConnection.open(ConnectionOption.SQL);
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where PlanDtandTime >= '"+DateTime.Now.ToShortDateString()+"' and PlanDtandTime<'"+DateTime.Now.AddDays(1).ToShortDateString()+"' and processName like '%"+type+"%' and ActualDtandTime>='"+DateTime.Now.ToShortDateString()+"'  and ActualDtandTime <'"+DateTime.Now.AddDays(1).ToShortDateString()+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string actualQuantityMonth(object type)
          {
              string flag = "";

              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where Month(PlanDtandTime) = '"+DateTime.Now.Month+"' and Year(PlanDtandTime)='"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Month(ActualDtandTime)='"+DateTime.Now.Month+"' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string actualQuantityYear(object type)
          {
              string flag = "";
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where Year(PlanDtandTime) = '"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              myConnection.close(ConnectionOption.SQL);
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string differenceQuantityDay(object type)
          {
              string flag = "";
              myConnection.open(ConnectionOption.SQL);
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where PlanDtandTime >= '"+DateTime.Now.ToShortDateString()+"' and PlanDtandTime<'"+DateTime.Now.AddDays(1).ToShortDateString()+"' and processName like '%"+type+"%' and ActualDtandTime>='"+DateTime.Now.ToShortDateString()+"'  and ActualDtandTime <'"+DateTime.Now.AddDays(1).ToShortDateString()+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string differenceQuantityMonth(object type)
          {
              string flag = "";

              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where Month(PlanDtandTime) = '"+DateTime.Now.Month+"' and Year(PlanDtandTime)='"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Month(ActualDtandTime)='"+DateTime.Now.Month+"' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string differenceQuantityYear(object type)
          {
              string flag = "";
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where Year(PlanDtandTime) = '"+DateTime.Now.Year+"' and processName like '%"+type+"%' and Year(ActualDtandTime)='"+DateTime.Now.Year+"'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              myConnection.close(ConnectionOption.SQL);
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string plannedQuantityTotalDay(object type)
          {
              string flag = "";
              myConnection.open(ConnectionOption.SQL);
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where PlanDtandTime >= '" + DateTime.Now.ToShortDateString() + "' and PlanDtandTime<'" + DateTime.Now.AddDays(1).ToShortDateString() + "'  and ActualDtandTime>='" + DateTime.Now.ToShortDateString() + "'  and ActualDtandTime <'" + DateTime.Now.AddDays(1).ToShortDateString() + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string plannedQuantityTotalMonth(object type)
          {
              string flag = "";

              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where Month(PlanDtandTime) = '" + DateTime.Now.Month + "' and Year(PlanDtandTime)='" + DateTime.Now.Year + "' and Month(ActualDtandTime)='" + DateTime.Now.Month + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string plannedQuantityTotalYear(object type)
          {
              string flag = "";
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(PlannedQty) from vProduction2 where Year(PlanDtandTime) = '" + DateTime.Now.Year + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              myConnection.close(ConnectionOption.SQL);
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string actualQuantityTotalDay(object type)
          {
              string flag = "";
              myConnection.open(ConnectionOption.SQL);
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where PlanDtandTime >= '" + DateTime.Now.ToShortDateString() + "' and PlanDtandTime<'" + DateTime.Now.AddDays(1).ToShortDateString() + "' and ActualDtandTime>='" + DateTime.Now.ToShortDateString() + "'  and ActualDtandTime <'" + DateTime.Now.AddDays(1).ToShortDateString() + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string actualQuantityTotalMonth(object type)
          {
              string flag = "";

              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where Month(PlanDtandTime) = '" + DateTime.Now.Month + "' and Year(PlanDtandTime)='" + DateTime.Now.Year + "'  and Month(ActualDtandTime)='" + DateTime.Now.Month + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string actualQuantityTotalYear(object type)
          {
              string flag = "";
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(ActualQty) from vProduction2 where Year(PlanDtandTime) = '" + DateTime.Now.Year + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              myConnection.close(ConnectionOption.SQL);
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string differenceQuantityTotalDay(object type)
          {
              string flag = "";
              myConnection.open(ConnectionOption.SQL);
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where PlanDtandTime >= '" + DateTime.Now.ToShortDateString() + "' and PlanDtandTime<'" + DateTime.Now.AddDays(1).ToShortDateString() + "' and ActualDtandTime>='" + DateTime.Now.ToShortDateString() + "'  and ActualDtandTime <'" + DateTime.Now.AddDays(1).ToShortDateString() + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

          public string differenceQuantityTotalMonth(object type)
          {
              string flag = "";

              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where Month(PlanDtandTime) = '" + DateTime.Now.Month + "' and Year(PlanDtandTime)='" + DateTime.Now.Year + "' and Month(ActualDtandTime)='" + DateTime.Now.Month + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              if (flag.Equals(""))
              {
                  flag = "0";
              }
              return flag;
          }

          public string differenceQuantityTotalYear(object type)
          {
              string flag = "";
              myConnection.comm = myConnection.conn.CreateCommand();
              myConnection.comm.CommandText = "Select sum(Difference) from vProduction2 where Year(PlanDtandTime) = '" + DateTime.Now.Year + "' and Year(ActualDtandTime)='" + DateTime.Now.Year + "'";
              myConnection.reader = myConnection.comm.ExecuteReader();
              while (myConnection.reader.Read())
              {
                  flag = myConnection.reader[0].ToString();
              }
              myConnection.reader.Close();
              myConnection.comm.Dispose();
              myConnection.close(ConnectionOption.SQL);
              if (flag.Equals(""))
              {
                  flag = "0";
              }

              return flag;
          }

        private void fillGridView()
        {
            myConnection.open(ConnectionOption.SQL);
            string query = "Select distinct substring(processName,0,4) as Type from vProduction2";
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = query;
            SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            prodSummaryReportGridView.DataSource = ds;
            prodSummaryReportGridView.DataBind();
            da.Dispose();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            
            //prodSummaryReportGridView.DataSource = myWebService.fillGridView("Select distinct substring(processName,0,4) as Type from vProduction2");
            //prodSummaryReportGridView.DataBind();

            DataTable totDTable = new DataTable();
            totDTable.Columns.Add("Type");
            DataRow dRow = totDTable.NewRow();
            dRow[0] = "Total";
            totDTable.Rows.Add(dRow);
            totalGridView.DataSource = totDTable;
            totalGridView.DataBind();
        }

        private void createDataTable()
        {
            // Adding Columns to VirtualTable
            dTable = new DataTable();
            dTable.Columns.Add("Type");
            dTable.Columns.Add("Day");
            dTable.Columns.Add("Month");
            dTable.Columns.Add("Year");           
        }   

        private void bindDataTable()
        {
            ArrayList processNames = new ArrayList();
            processNames.Add("TBR");
            processNames.Add("PCR");

            string date = formatDate();           

            for (int i = 0; i < processNames.Count; i++)
            {
                int dayPlan, dayActual, dayDiff, mthPlan, mthActual, mthDiff, yearPlan, yearActual, yearDiff;
                
                #region bindDayWiseData

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "Select SUM(PlannedQty) as Planned, SUM(ActualQty) as Actual, SUM(Difference) as Diff from vProduction2 where PlanDtandTime >= '04-21-2011' and PlanDtandTime<'04-22-2011' and processName like '%tbr%' and ActualDtandTime>='04-21-2011'  and ActualDtandTime <'04-22-2011'";           
                SqlDataAdapter da = new SqlDataAdapter(myConnection.comm);
                DataSet ds = new DataSet();
                da.Fill(ds);               

                    if ((!(ds.Tables[0].Rows[0].IsNull("Planned")))) 
                    {
                        dayPlan  = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    } else{dayPlan=0;}

                    if((!(ds.Tables[0].Rows[0].IsNull("Actual"))))
                    {
                        dayActual = Convert.ToInt32(ds.Tables[0].Rows[0][1]);                        
                    } else{dayActual=0;}

                    if((!(ds.Tables[0].Rows[0].IsNull("Diff"))))
                    {   
                      dayDiff = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                    } else {dayDiff = 0;}

                    totDPlan = totDPlan + dayPlan;
                    totDactual = totDactual + dayActual;
                    totDDiff = totDDiff + dayDiff;

                    myConnection.comm.Dispose();
                    da.Dispose();
                    ds.Dispose();

                    #endregion

                #region bindMonthWiseData

                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select sum(plannedQty) as Planned , sum(actualQty) as Actual, sum(Difference) as Diff from vProduction2 where Month(PlandtandTime) ='" + Convert.ToDateTime(date).Month + "' and Month(ActualdtandTime) ='" + Convert.ToDateTime(date).Month + "' and processName like '%" + processNames[i].ToString() + "%'";
                    da = new SqlDataAdapter(myConnection.comm);
                    ds = new DataSet();
                    da.Fill(ds);

                    if ((!(ds.Tables[0].Rows[0].IsNull("Planned"))))
                    {
                        mthPlan = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                
                    } else { mthPlan = 0;}

                    if (!(ds.Tables[0].Rows[0].IsNull("Actual")))
                    {
                        mthActual = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                    }
                    else { mthActual = 0; }

                    if (!(ds.Tables[0].Rows[0].IsNull("Diff")))
                    {
                        mthDiff = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
   
                    } else{ mthDiff=0; }

                    totMPlan = totMPlan + mthPlan;
                    totMActual = totMActual + mthActual;
                    totMDiff = totMDiff + mthDiff;

                    da.Dispose();
                    ds.Dispose();
                    myConnection.comm.Dispose();
                    #endregion

                #region bindYearWiseData

                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "select sum(plannedQty) as Planned , sum(actualQty) as Actual, sum(Difference) as Diff from vProduction2 where Year(PlandtandTime) ='" + Convert.ToDateTime(date).Year  + "' and Year(ActualdtandTime) ='" + Convert.ToDateTime(date).Year+ "' and processName like '%" + processNames[i].ToString() + "%'";
                    da = new SqlDataAdapter(myConnection.comm);
                    ds = new DataSet();
                    da.Fill(ds);

                    if (!(ds.Tables[0].Rows[0].IsNull("Planned"))) 
                    {
                        yearPlan = Convert.ToInt32(ds.Tables[0].Rows[0][0]);                      
                        
                    } else { yearPlan = 0;}

                    if (!(ds.Tables[0].Rows[0].IsNull("Actual")))
                    {
                        yearActual = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                    } else { yearActual = 0; }

                    if(!(ds.Tables[0].Rows[0].IsNull("Diff")))
                    {
                        yearDiff = Convert.ToInt32(ds.Tables[0].Rows[0][2]);

                    } else{ yearDiff=0; }
                
                    totYPlan = totYPlan + yearPlan;
                    totYActual = totYActual + yearActual;
                    totYDiff = totYDiff + yearDiff;
                    da.Dispose();
                    ds.Dispose();
                    myConnection.comm.Dispose();

                    #endregion

                #region bindDataToTable
               
                    DataRow dRow1 = dTable.NewRow();                    
                    dRow1["Type"] = processNames[i].ToString();
                    dRow1["Day"] = dayPlan;
                    dRow1["Month"] = mthPlan;
                    dRow1["Year"] = yearPlan;
                    dTable.Rows.Add(dRow1);
                    DataRow dRow2 = dTable.NewRow();
                    dRow2["Type"] = processNames[i].ToString();
                    dRow2["Day"] = dayActual;
                    dRow2["Month"] = mthActual;
                    dRow2["Year"] = yearActual;
                    dTable.Rows.Add(dRow2);
                    DataRow dRow3 = dTable.NewRow();
                    dRow3["Type"] = "";
                    dRow3["Day"] = dayActual;
                    dRow3["Month"] = mthActual;
                    dRow3["Year"] = yearActual;
                    dTable.Rows.Add(dRow2); 
                    

                    //dRow["MonthPlan"] = mthPlan;
                    //dRow["MonthActual"] = mthActual;
                    //dRow["MonthDiff"] = mthDiff;
                    //dRow["YearPlan"] = yearPlan;
                    //dRow["YearActual"] = yearActual;
                    //dRow["YearDiff"] = yearDiff;
                     
                }
                 //Binding Total Values to Virtual Table

                //DataRow dRowTotal = dTable.NewRow();
                //dRowTotal["Type"] = "Total";
                //dRowTotal["DayPlan"] = totDPlan;
                //dRowTotal["DayActual"] = totDactual;
                //dRowTotal["DayDiff"] = totDDiff;
                //dRowTotal["MonthPlan"] = totMPlan;
                //dRowTotal["MonthActual"] = totMActual;
                //dRowTotal["MonthDiff"] = totMDiff;
                //dRowTotal["YearPlan"] = totYPlan;
                //dRowTotal["YearActual"] = totYActual;
                //dRowTotal["YearDiff"] = totYDiff;
                //dTable.Rows.Add(dRowTotal);
                //prodSummaryReportGridView.DataSource = dTable;
                //prodSummaryReportGridView.DataBind();

                #endregion
            }      

        private void formatChart(Chart prodSummaryChart, double[] plan, double[] actual, double[] diff)
        {
            ArrayList xVal=new ArrayList();
            xVal.Add("Day");
            xVal.Add("Month");
            xVal.Add("Year");
                       
            for (int i = 0; i < prodSummaryChart.Series.Count;i++)
            {
                prodSummaryChart.Series[i]["DrawingStyle"] = "Cylinder";
                prodSummaryChart.Series[i]["PointWidth"] = "0.6";
                prodSummaryChart.Series[i].ChartType = SeriesChartType.Column;   
            }

            prodSummaryChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            prodSummaryChart.Series[0].Points.DataBindXY(xVal, plan);
            prodSummaryChart.Series[1].Points.DataBindXY(xVal, actual);
            prodSummaryChart.Series[2].Points.DataBindXY(xVal, diff);
            prodSummaryChart.ChartAreas[0].AxisY.Title = "Production";
        }

      #endregion

    }
 }


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   