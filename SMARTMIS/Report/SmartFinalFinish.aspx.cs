using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data.SqlClient;

namespace SmartMIS.Report
{
    public partial class SmartFinalFinish : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
          string tblname,mydatetime,htmlStr,htmlcountStr,status = "";
          string todatetime;
          int totalTyre;
        smartMISWebService mywebservice = new smartMISWebService();
        protected void Page_Load(object sender, EventArgs e)
        {
            myConnection.open(ConnectionOption.SQL);

        }
        
        protected void Button2_Click(object sender, EventArgs e)
        {
            DataViewLabel.Text = "";
            SummaryDataView.Text = "";
            messageLabel.Text = "";
           
           
            if (!string.IsNullOrEmpty(DropDownList1.SelectedValue))
            {
                mydatetime = reportMasterFromDateTextBox.Text.Trim().ToString();
                todatetime = Convert.ToDateTime(mywebservice.formatDate(reportMasterToDateTextBox.Text), CultureInfo.InvariantCulture).AddDays(1).ToString("MM/dd/yyyy");
                
                

              

                // Show data
                try
                {
                    myConnection.open(ConnectionOption.SQL);

                    if (DropDownList1.SelectedValue == "TBR")
                        tblname = "vfinalfinishTBR";
                    else if (DropDownList1.SelectedValue == "PCR")
                        tblname = "vfinalfinishPCR";

                    status = DropDownList2.SelectedItem.Value;
                    if (status.Equals("OK"))
                        status = "status = '1'";
                    else if (status.Equals("Not Ok"))
                        status = "status = '2'";
                    else
                        status = "1=1";
                    if (string.IsNullOrEmpty(mydatetime))
                        mydatetime = "";

                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select lot,name, status, dtandTime ,firstName, lastName FROM " + tblname + " WHERE " + status + " AND iD IN (SELECT MIN(iD) FROM " + tblname + " GROUP BY lot) AND dtandTime>='" + mywebservice.formatDate(mydatetime) + " 07:00:00 AM' AND dtandTime<='" + todatetime + " 06:59:59 AM'";
                    string test = myConnection.comm.CommandText;
                    myConnection.reader = myConnection.comm.ExecuteReader();

                    if (myConnection.reader.HasRows)
                    {
                        htmlStr += "<table id=\"table1\" class=\"innerTable\" width=\"1\">";
                        while (myConnection.reader.Read())
                        {
                            string lot = myConnection.reader["lot"].ToString();

                            int lotcount = getlotcount(lot);
                            totalTyre += lotcount;
                            string wname = myConnection.reader["name"].ToString();
                            string newstatus = myConnection.reader["status"].ToString();
                            if (newstatus == "1")
                                newstatus = "OK";
                            else if (newstatus == "2")
                                newstatus = "Not Ok";

                            string dtandTime = myConnection.reader["dtandTime"].ToString();
                            string inspectorname = myConnection.reader["firstName"] + " " + myConnection.reader["lastName"];
                            htmlStr += "<tr style=\"color:#333333;background-color:#F7F6F3;\"><td width=\"30%\"><span class=\"gridViewItems\">" + wname + "</span></td><td width=\"15%\"><span class=\"gridViewItems\">"+lotcount+"</span></td><td width=\"10%\"><span class=\"gridViewItems\"><a href=\"info.aspx?lot=" + lot + "&tbl=" + tblname + "&wname=" + wname + "&dtandTime=" + dtandTime + "&inspectorname=" + inspectorname + "\" target=\"_blank\">" + lot + "</a></span></td><td width=\"20%\"><span class=\"gridViewItems\">" + newstatus + "</span></td><td width=\"30%\"><span class=\"gridViewItems\">" + dtandTime + "</span></td></tr>";
                        }
                        htmlStr += "</table>";

                    }

                    else
                        htmlStr = "No Data Available";
                    DataViewLabel.Text = htmlStr.ToString();

                }
                catch (Exception exc)
                {
                    messageLabel.Text = exc.Message;
                }
                finally 
                {

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.conn.Close();
                }




                try
                {

                   

                    //Show count
                    myConnection.open(ConnectionOption.SQL);

                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "select lot, status FROM " + tblname + " WHERE iD IN (SELECT MIN(iD) FROM " + tblname + " GROUP BY lot) AND dtandTime>='" + mywebservice.formatDate(mydatetime) + " 07:00:00 AM' AND dtandTime<='" + todatetime + " 06:59:59 AM'";
                    myConnection.reader = myConnection.comm.ExecuteReader();

                    int lotcount = 0, okcount = 0, notokcount = 0;
                    float percent;
                    if (myConnection.reader.HasRows)
                    {
                        htmlcountStr += "<table id=\"table1\" class=\"innerTable\" width=\"1\"><tr><td><br/></td></tr><tr> <td class=\"FinalFinishgridViewHeader\">Total Tyre</td><td class=\"FinalFinishgridViewHeader\">Total Lots</td><td class=\"FinalFinishgridViewHeader\">OK Lots</td><td class=\"FinalFinishgridViewHeader\">Not OK Lot</td><td class=\"FinalFinishgridViewHeader\">Not Ok Percent</td></tr>";
                        while (myConnection.reader.Read())
                        {
                            string mystatus = myConnection.reader[1].ToString();
                            if (mystatus.Equals("2"))
                                notokcount++;
                            else if (mystatus.Equals("1"))
                                okcount++;
                            lotcount++;
                        }
                        percent = (float)notokcount / lotcount * 100;
                        decimal m = Convert.ToDecimal(percent);
                        decimal newpercent = Math.Round(m, 2);
                        htmlcountStr += "<tr style=\"color:#333333;background-color:#F7F6F3;\"><td width=\"120px\"><span class=\"gridViewItems\">" + totalTyre + "</span></td> <td width=\"120px\"><span class=\"gridViewItems\">" + lotcount + "</span></td><td width=\"120px\"><span class=\"gridViewItems\">" + okcount + "</span></td><td width=\"120px\"><span class=\"gridViewItems\">" + notokcount + "</span></td><td width=\"120px\"><span class=\"gridViewItems\">" + newpercent.ToString() + " %</span></td></tr></table>";
                    }

                    SummaryDataView.Text = htmlcountStr;
                }
                catch (Exception exc)
                {
                    messageLabel.Text = exc.Message;
                }
                finally
                {
                    myConnection.reader.Close();
                    myConnection.comm.Dispose();

                }


                 
            }
            else
                DataViewLabel.Text = "No Data Available";
        }
        public int getlotcount(string lot)
        {
            SqlConnection con = new SqlConnection();
            int lotcount = 0;
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("select COUNT(*) AS lotcount FROM " + tblname + " WHERE lot = '" + lot + "'", con);
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        lotcount = (int)dr["lotcount"];
                        
                    }
                }
                dr.Close();
                cmd.Dispose();
                con.Close();
            
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            
            return lotcount;
        }

    }
}
