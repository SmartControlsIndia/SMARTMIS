using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;

namespace SmartMIS.Report
{
    public partial class xrayreport : System.Web.UI.Page
    {
        smartMISWebService mywebservice = new smartMISWebService();
        myConnection myConnection = new myConnection();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { fillProcessDropDownList(); }
            if (string.IsNullOrEmpty(reportMasterFromDate.Text))  // If Textbox already null, then show current Date
            {
                reportMasterFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                string showToDate = "";
                int month = DateTime.Now.Month, year = DateTime.Now.Year;

                if (DateTime.Now.Month == 12 && DateTime.Now.Day == 31)
                    showToDate = "01-01" + "-" + (DateTime.Now.Year + 1);
                else if (DateTime.Now.Day == 31 && (DateTime.Now.Month == 1 || DateTime.Now.Month == 3 || DateTime.Now.Month == 5 || DateTime.Now.Month == 7 || DateTime.Now.Month == 8 || DateTime.Now.Month == 10))
                    showToDate = "01-" + checkDigit((DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else if (DateTime.Now.Day == 30 && (DateTime.Now.Month == 4 || DateTime.Now.Month == 6 || DateTime.Now.Month == 9 || DateTime.Now.Month == 11))
                    showToDate = "01-" + (checkDigit(DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else if (DateTime.Now.Month == 2)
                    showToDate = "01-" + checkDigit((DateTime.Now.Month + 1)) + "-" + DateTime.Now.Year.ToString();
                else
                    showToDate = checkDigit((DateTime.Now.Day + 1)) + "-" + checkDigit(DateTime.Now.Month) + "-" + DateTime.Now.Year;

                reportMasterToDate.Text = showToDate.ToString();

            }
        }

        protected string checkDigit(int digit)
        {
            string str = "";
            if (digit.ToString().Length == 1)
                str = "0" + digit;
            else
                str = digit.ToString();
            return str;
        }
        public string formatfromDate(String date)
        {
            string flag = "";

            string day, month, year;
            if (date != null)
            {
                string[] tempDate = date.Split(new char[] { '-' });
                try
                {
                    day = tempDate[1].ToString().Trim();
                    month = tempDate[0].ToString().Trim();
                    year = tempDate[2].ToString().Trim();
                    // DateTime tempDate1 = Convert.ToDateTime(date);
                    if (Convert.ToInt32(month) == 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-01-" + (Convert.ToInt32(year) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    if (Convert.ToInt32(month) < 12 && Convert.ToInt32(day) == 31)
                    {
                        flag = "01-01-" + (Convert.ToInt32(month) + 1).ToString() + " 07" + ":" + "00" + ":" + "00";
                    }
                    if (DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month)) != Convert.ToInt32(day))
                    {
                        flag = month + "-" + (Convert.ToInt32(day) + 1).ToString() + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                    else if (Convert.ToInt32(month) < 12 )
                    {
                        flag = (Convert.ToInt32(month) + 1).ToString() + "-" + "01" + "-" + year + " " + "07" + ":" + "00" + ":" + "00";
                    }
                }
                catch (Exception exp)
                {
                    mywebservice.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }
        private void fillProcessDropDownList()
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("id", typeof(string));
            dt1.Columns.Add("Name", typeof(string));
            dt1.Rows.Add("ALL", "ALL");
            dt1.Rows.Add("7201", "Xray1");
            dt1.Rows.Add("Xray2", "Xray2");

            dpdxray.DataSource = dt1;// mywebservice.FillDropDownListXray("wcMaster", "name", "where  processID in (17) ");
            dpdxray.DataTextField = "Name";
            dpdxray.DataValueField = "id";
            dpdxray.DataBind();
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string fromdate,newfromdate,todate;
            fromdate = reportMasterFromDate.Text.Trim().ToString();
            todate = reportMasterToDate.Text.Trim().ToString();
            //newfromdate = Convert.ToDateTime(mywebservice.formatDate(fromdate)).AddDays(1).ToString("dd-MM-yyyy");

            myConnection.open(ConnectionOption.SQL);
            // Count Rows Shift wise
            myConnection.comm = myConnection.conn.CreateCommand();
            if (dpdxray.SelectedIndex == 1)
            {
                myConnection.comm.CommandText = @"select WCNAME, dtandTime, myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' 
AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND 
convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND
convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108)
<= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray WHERE wcname='" + dpdxray.SelectedValue + "' AND dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + @"' 
            order by dtandTime asc";
            }
            else if (dpdxray.SelectedIndex == 2)
            {
                myConnection.comm.CommandText = @"select WCNAME, dtandTime, myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' 
AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND 
convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND
convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108)
<= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray2 WHERE wcname='" + dpdxray.SelectedValue + "' AND dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + @"' 
            order by dtandTime asc";
            }
            else 
            {
                myConnection.comm.CommandText = @"select WCNAME, dtandTime, myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108)<= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray2 WHERE dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", ""))+"' Union select WCNAME, dtandTime, myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108)<= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray WHERE dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + @"' 
            order by dtandTime asc";
            }
            
            myConnection.reader = myConnection.comm.ExecuteReader();

            int shifta = 0, shiftb = 0, shiftc = 0;
            if (myConnection.reader.HasRows)
            {
                 while (myConnection.reader.Read())
                 {
                     string shift = myConnection.reader["myDate"].ToString();
                     if (shift.Equals("Shift A"))
                         shifta++;
                     else if (shift.Equals("Shift B"))
                         shiftb++;
                     else if (shift.Equals("Shift C"))
                         shiftc++;
                 }
            }

            // Display Date
            if (!string.IsNullOrEmpty(fromdate))
            {
                try
                {

                    if (dpdxray.SelectedIndex == 1 )
                    {
                        GridView1.DataSource = mywebservice.fillGridView(@"SELECT WCNAME, gtbarCode, firstName+' '+lastName AS name,convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],
                    myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND
convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray WHERE  wcname='" + dpdxray.SelectedValue + "' AND  dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + "' order by dtandTime asc", ConnectionOption.SQL);
                        GridView1.DataBind();
                    }
                    else if (dpdxray.SelectedIndex == 2)
                    {
                        GridView1.DataSource = mywebservice.fillGridView(@"SELECT WCNAME, gtbarCode, firstName+' '+lastName AS name,convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],
                    myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND
convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray2 WHERE wcname='" + dpdxray.SelectedValue + "' AND dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + "' order by dtandTime asc", ConnectionOption.SQL);
                        GridView1.DataBind();
                    }
                   else
                    {
                        GridView1.DataSource = mywebservice.fillGridView(@"SELECT WCNAME, gtbarCode, firstName+' '+lastName AS name,convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],
                    myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray2 WHERE dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + "' union  SELECT WCNAME, gtbarCode, firstName+' '+lastName AS name,convert(char(10), dtandTime, 105) AS Date,CONVERT(VARCHAR(8) , dtandTime , 108) AS [Time],myDate=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'Shift A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'Shift B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'Shift C' END) FROM vTyreXray WHERE dtandTime>='" + mywebservice.formatDate(fromdate) + " 07:00:00'  AND dtandTime<'" + formatfromDate(mywebservice.formatDate(todate).Replace(" 07:00:00", "")) + "' order by date,Time asc", ConnectionOption.SQL);
                        GridView1.DataBind();
                    }
                    
                    showcount.Text = "Total : " + GridView1.Rows.Count + " Shift A : " + shifta + " Shift B : " + shiftb + " Shift C : " + shiftc;
                }
                catch (Exception exp)
                {
                    Response.Write(exp.Message);
                }
            }
        }

        protected void Export_click(object sender, EventArgs e)
        {
            //string duration = DropDownListDuration.SelectedItem.Value;
            //string type = DropDownListType.SelectedItem.Value;
            //string getTimeDuration = "";

            //Panel gvpanel = (Panel)CurMasterContentPlaceHolder.FindControl("gvpanel");
            //switch (duration)
            //{
            //    case "Date":
            //        getTimeDuration = "<b>Date :</b> " + reportMasterFromDateTextBox.Text;
            //        break;
            //    case "Month":
            //        getTimeDuration = "<b>Month :</b> " + DropDownListMonth.SelectedItem.Text + " " + DropDownListYear.SelectedItem.Value;
            //        break;
            //    case "Year":
            //        getTimeDuration = "<b>Year :</b> " + DropDownListYearWise.SelectedItem.Value;
            //        break;
            //}

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=XRAYReport" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            stringWrite.Write("<table><tr><td><b>X-Ray Report</b></td><td>" + reportMasterFromDate.Text + "</td><td><b>Type :</b> " + reportMasterToDate + "</td><td><b>" + dpdxray.SelectedItem.Value + "</b></td></tr></table>");
            System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
            Controls.Add(form);
            form.Controls.Add(Panel1);
            form.RenderControl(htmlWrite);

            //gv.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                e.Row.Cells[0].Text = "WCNAME";
                e.Row.Cells[1].Text = "Barcode";
                e.Row.Cells[2].Text = "Name";
                e.Row.Cells[3].Text = "Date & Time";
                e.Row.Cells[4].Text = "Shift";
            }
        }            
    }
}
