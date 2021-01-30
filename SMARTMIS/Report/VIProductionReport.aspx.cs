using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;


namespace SmartMIS.Report
{
    public partial class VIProductionReport : System.Web.UI.Page
    {
        smartMISWebService mywebservice = new smartMISWebService();
        myConnection myconnection = new myConnection();
        DateTime fromDate, toDate;
        int ProID = -1;
        string fileName = DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + "VIProduction.xlsx";
       // string filepath = @"D:\JKSMARTMIS\JKSMARTMIS\SMARTMIS\SMARTMIS\Excel\";
        DataTable gridviewdt = new DataTable();
        int getMonth, getYear;
        //string filepath;

        //public VIProductionReport()
        //{
        //    filepath = mywebservice.getExcelPath();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
              MessageLabel.Text = "";
            if (!IsPostBack)
            {
               // BindGridView();
                DD_Monthbind();
                int _currentyear;
                const int _year = 2010;
                _currentyear = DateTime.Now.Year;
                for (int s = _year; s <= _currentyear; s++)
                {
                    ddlYear.Items.Add(new ListItem((s).ToString(), (s).ToString()));
                }
                ddlYear.DataBind();
                DailyselectionPanel.Visible = true;
                MonthWisePanel.Visible = false;
                MonthlyselectionPanel.Visible = false;
                lblNoRecord.Visible = true;
                cadatetextbox.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                ArrayList storeCurrentname = new ArrayList();
                storeCurrentname.Add("ALL");
               
                try
                {
                    DataTable currentdt = new DataTable();

                    myconnection.open(ConnectionOption.SQL);
                    myconnection.comm = myconnection.conn.CreateCommand();
                    DateTime dtValue = Convert.ToDateTime(cadatetextbox.Text);
                    var fromDate = dtValue.ToString("yyyy-MM-dd");
                    var ToDate = dtValue.AddDays(1).ToString("yyyy-MM-dd");
                    if (ddlProcess.SelectedValue == "PCR")
                    {
                        myconnection.comm.CommandText = ("SELECT distinct firstname+' '+lastname as InspectorName fROM manningMaster where areaName='PCRCuring' order by InspectorName");
                    }
                    else 
                    { 
                    myconnection.comm.CommandText = ("SELECT distinct firstname+' '+lastname as InspectorName fROM manningMaster where areaName='TBRCuring' order by InspectorName");
                    //myconnection.comm.CommandText = ("SELECT distinct firstName + '_' + lastName As InspectorName FROM vInspectionManningTBR WHERE dtandTime> '" + fromDate + " " + "07:00:00" + "' and dtandTime< '" + ToDate + " " + "07:00:00" + "'");
                    }


                    myconnection.reader = myconnection.comm.ExecuteReader();
                    while (myconnection.reader.Read())
                    {
                        if (myconnection.reader.ToString() == "")
                        {
                            MachineDropdown.Items.Clear();
                        }
                        else
                        {

                            storeCurrentname.Add(myconnection.reader[0].ToString());
                        }
                    }
                    MachineDropdown.Items.Clear();
                    MachineDropdown.DataSource = storeCurrentname;
                    MachineDropdown.DataBind();
                    myconnection.reader.Close();


                }
                catch (Exception ex)
                {
                    mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
               

            }
        }
      
        protected void ViewMachineButton_Click(object sender, EventArgs e)
        {

            DataTable maindt = new DataTable();
            maindt.Columns.Add("Date", typeof(string));
            maindt.Columns.Add("wcName", typeof(string));
            maindt.Columns.Add("Shift", typeof(string));
           
            maindt.Columns.Add("sapcode", typeof(string));
            maindt.Columns.Add("InspectorName", typeof(string));
            maindt.Columns.Add("tcheckedtyre", typeof(int));


            //Label1.Text = "<script type='text/javascript'>calledFn()</script>";
            DataTable griddt = new DataTable();
            griddt.Columns.Add("wcid", typeof(int));
            griddt.Columns.Add("id", typeof(int));
            griddt.Columns.Add("shift", typeof(string));
            griddt.Columns.Add("Date", typeof(string));

            griddt.Columns.Add("wcMasterID", typeof(int));
            griddt.Columns.Add("wcName", typeof(string));
            griddt.Columns.Add("manningID", typeof(int));
          
            griddt.Columns.Add("sapcode", typeof(string));
            griddt.Columns.Add("InspectorName", typeof(string));
            griddt.Columns.Add("tcheckedtyre", typeof(string));
            string getDuration = ddlmonthselection.SelectedItem.Text;
            int getMonth = ddlMonth.SelectedIndex;
            int getyear = Convert.ToInt32(ddlYear.SelectedItem.Text);
            if (getDuration == "Monthly")
            {
                showDurationReportMonthWise(getMonth, getyear);
                MonthNameLabelId.Text = "Month : " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(getMonth) + " " + getyear;
            }
            
            DataTable manningdt = new DataTable();
            DataTable wdt = new DataTable();
            DataTable viewdt = new DataTable();
            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                DateTime dtValue = Convert.ToDateTime(cadatetextbox.Text);
                var fromDate = dtValue.ToString("yyyy-MM-dd");
                var fromDateto = fromDate;
                var ToDate = dtValue.AddDays(1).ToString("yyyy-MM-dd");
                if (ddlshift.SelectedValue == "1")
                {
                     
                    
                    fromDate = fromDate + " 07:00:00";
                    ToDate = fromDateto + " 15:00:00 ";
                   
                }
                else if (ddlshift.SelectedValue == "2")
                {
                    fromDate = fromDate + " 15:00:00";
                    ToDate = fromDateto + " 22:00:00 ";

                }
                else if (ddlshift.SelectedValue == "3")
                {
                    fromDate = fromDate + " 22:59:59";
                    ToDate = ToDate + " 07:00:00 ";
                }
                else
                {
                    fromDate = fromDate + " 07:00:00 ";
                    ToDate = ToDate + " 07:00:00 ";
                }

                string tableName = "";
                if (ddlProcess.SelectedValue == "PCR")
                {
                    tableName = "vInspectionPcr";

                }
                else
                {
                    tableName = "vTBRVisualInspection";
                }

                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = ("SELECT wcid,manningid,shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),  convert(char(10), dtandTime, 110) AS getdate fROM " + tableName + " where manningId in (select id from manningmaster) and dtandTime> '" + fromDate + "' and dtandTime< '" + ToDate + "' ");
                mywebservice.writeLogs(myconnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            
                myconnection.reader = myconnection.comm.ExecuteReader();
                viewdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);



                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                {
                    var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                    var firstName = names[0];
                    var lastName = names[1];
                    myconnection.comm.CommandText = ("SELECT id,sapcode,firstname+' '+lastname as InspectorName fROM manningMaster where firstName='" + firstName + "' and lastName='" + lastName + "'");
                }
                else
                {
                    myconnection.comm.CommandText = ("SELECT id,sapcode,firstname+' '+lastname as InspectorName fROM manningMaster ");
                }

                myconnection.reader = myconnection.comm.ExecuteReader();
                manningdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);



                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = ("SELECT id as wcMasterID, name from wcmaster");
                myconnection.reader = myconnection.comm.ExecuteReader();
                wdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);

                var rOw = from r0w1 in viewdt.AsEnumerable()
                          join r0w2 in wdt.AsEnumerable()
                            on r0w1.Field<int>("wcID") equals r0w2.Field<int>("wcMasterID")
                          join r0w3 in manningdt.AsEnumerable()
                            on r0w1.Field<int?>("manningid") equals r0w3.Field<int?>("id") into ps
                          from r0w3 in ps.DefaultIfEmpty()
                          select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray : new object[] { 0, 0, 0 })).ToArray();

                foreach (object[] values in rOw)
                    griddt.Rows.Add(values);

                griddt.Columns.Remove("wcMasterID");
                griddt.Columns.Remove("id");

                //if (ddlProcess.SelectedValue == "PCR")
                //{
                //    if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                //    {
                //        var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                //        var firstName = names[0];
                //        var lastName = names[1];
                //        myconnection.comm.CommandText = ("SELECT manningID,wcname,sapcode, shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),  convert(char(10), dtandTime, 110) AS getdate,firstName + ' ' + lastName As InspectorName FROM vVisualInspectionPCR where firstName='" + firstName + "' and lastName='" + lastName + "'  and dtandTime> '" + fromDate + "' and dtandTime< '" + ToDate + "' order by firstName, shift asc ");
                //    }
                //    else
                //    {
                //        myconnection.comm.CommandText = ("SELECT manningID,wcname,sapcode, shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),  convert(char(10), dtandTime, 110) AS getdate,firstName + ' ' + lastName As InspectorName FROM vVisualInspectionPCR where dtandTime> '" + fromDate + "' and dtandTime< '" + ToDate + "' order by firstName, shift asc");
                //    }
                //}
                //if (ddlProcess.SelectedValue == "TBR")
                //{
                //    if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                //    {
                //        var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                //        var firstName = names[0];
                //        var lastName = names[1];
                //        myconnection.comm.CommandText = ("SELECT manningID,name as wcname,sapcode, shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),  convert(char(10), dtandTime, 110) AS getdate,firstName + ' ' + lastName As InspectorName FROM vTBRVisualInspection where firstName='" + firstName + "' and lastName='" + lastName + "'  and dtandTime> '" + fromDate + "' order by firstName, shift asc");
                //    }
                //    else
                //    {
                //        myconnection.comm.CommandText = ("SELECT manningID,name as wcname,sapcode, shift=(CASE WHEN convert(char(8), dtandTime, 108) >= '07:00:00 AM' AND convert(char(8), dtandTime, 108) <= '14:59:59.999' THEN 'A' WHEN convert(char(8), dtandTime, 108) >= '15:00:00.000' AND convert(char(8), dtandTime, 108) <= '22:59:59.999' THEN 'B' WHEN ((convert(char(8), dtandTime, 108) >= '23:00:00.000' AND convert(char(8), dtandTime, 108) <= '23:59:59.999') or (convert(char(8), dtandTime, 108) >= '00:00:01.000' AND convert(char(8), dtandTime, 108) <= '06:59:59.999')) THEN 'C' END),  convert(char(10), dtandTime,110 ) AS getdate,firstName + ' ' + lastName As InspectorName FROM vTBRVisualInspection where dtandTime> '" + fromDate + "' and dtandTime< '" + ToDate + "' order by firstName, shift asc");
                //    }
                //}
                //myconnection.comm.CommandTimeout = 0;
                //myconnection.reader = myconnection.comm.ExecuteReader();
                //viewdt.Load(myconnection.reader);
                //myconnection.comm.Dispose();
                //myconnection.reader.Close();
                //myconnection.close(ConnectionOption.SQL);
                if (getDuration == "Daily")
                {

                    string[] TobeDistinctID = { "manningid", "shift", "wcName" };
                    DataTable dtDistinct = new DataTable();
                    dtDistinct = GetDistinctRecords(griddt, TobeDistinctID);
                    for (int i = 0; i < dtDistinct.Rows.Count; i++)
                    {
                        DataRow dr = maindt.NewRow();
                        int mid = Convert.ToInt32(dtDistinct.Rows[i][0]);
                        string Shift = dtDistinct.Rows[i][1].ToString();
                        string MachineName = dtDistinct.Rows[i][2].ToString();
                        var manningData = griddt.Select("manningID=" + mid + "");
                        int totalchecked1 = griddt.AsEnumerable().Count(p => p.Field<int>("manningID") == mid && p.Field<string>("shift") == Shift && p.Field<string>("wcName") == MachineName);

                        dr[0] = manningData[0][2].ToString();
                        dr[1] = MachineName;
                       // dr[1] = manningData[0][1].ToString();
                        //dr[2] = manningData[0][3].ToString();
                        dr[2] = Shift;
                        dr[3] = manningData[0][5].ToString();
                        dr[4] = manningData[0][6].ToString();
                        dr[5] = totalchecked1.ToString();
                        maindt.Rows.Add(dr);
                    }
                    GridViewproduction.DataSource = maindt;
                    GridViewproduction.DataBind();
                }
                if (GridViewproduction.Rows.Count > 0)
                {
                    lblNoRecord.Visible =false ;
                }
                else
                {
                    lblNoRecord.Visible = true;
                }
               
            }
            catch (Exception ex)
            {
                mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        protected void calbutton_Click(object sender, EventArgs e)
        {
            Calnder1.Visible = true;

        }
        
        protected void Calnder11_SelectionChanged(object sender, EventArgs e)
        {
            cadatetextbox.Text = Calnder1.SelectedDate.ToShortDateString();
            Calnder1.Visible = false;
            //ArrayList storeValueAll = new ArrayList();
            //storeValueAll.Add("ALL");
            //try
            //{
            //    DataTable vdt = new DataTable();

            //    myconnection.open(ConnectionOption.SQL);
            //    myconnection.comm = myconnection.conn.CreateCommand();
            //    DateTime dtValue = Convert.ToDateTime(cadatetextbox.Text);
            //    var fromDate = dtValue.ToString("yyyy-MM-dd");
            //    var ToDate = dtValue.AddDays(1).ToString("yyyy-MM-dd");
            //    if (ddlProcess.SelectedValue == "PCR")
            //    {
            //        myconnection.comm.CommandText = ("SELECT distinct firstName + '_' + lastName As InspectorName FROM vVisualInspectionPCR WHERE dtandTime> '" + fromDate + " " + "07:00:00" + "' and dtandTime< '" + ToDate + " " + "07:00:00" + "'");

            //    }
            //    else
            //    {
            //        myconnection.comm.CommandText = ("SELECT distinct firstName + '_' + lastName As InspectorName FROM vTBRVisualInspection WHERE dtandTime> '" + fromDate + " " + "07:00:00" + "' and dtandTime< '" + ToDate + " " + "07:00:00" + "'");

            //    }
            //    myconnection.reader = myconnection.comm.ExecuteReader();
            //    while (myconnection.reader.Read())
            //    {
            //        if (myconnection.reader.ToString() == "")
            //        {
            //            MachineDropdown.Items.Clear();
            //        }
            //        else
            //        {

            //            storeValueAll.Add(myconnection.reader[0].ToString());
            //        }
            //    }
            //    MachineDropdown.Items.Clear();
            //    MachineDropdown.DataSource = storeValueAll;
            //    MachineDropdown.DataBind();
            //    myconnection.reader.Close();
                //MachineDropdown.Items.Insert(1, new ListItem("ALL"));
               
               
            //}
            //catch (Exception ex)
            //{
            //    mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            //}

        }
        protected void MachineDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
                DataTable mdt = new DataTable();
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                {
                    var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                    var firstName = names[0];
                    var lastName = names[1];
                    myconnection.comm.CommandText = ("SELECT distinct id FROM manningmaster where firstName='" + firstName + "' and lastName='" + lastName + "'");
                myconnection.reader = myconnection.comm.ExecuteReader();
                mdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);
                ManningLabel.Text = "";
                ManningLabel.DataBind();
                MachineDropdown.SelectedItem.Selected = true;
               // MachineDropdown.Items.Insert(1, new ListItem("ALL"));
               }
        }
        //Following function will return Distinct records.
        private DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        //forn distinct ManningID
        private DataTable GetDistinctNameRecords(DataTable dtName, string[] Columns)
        {
            DataTable dtNameRecords = new DataTable();
            dtNameRecords = dtName.DefaultView.ToTable(true, Columns);
            return dtNameRecords;
        }
        //for dynamic bound month fields
        //Done by sarita
        //Created AT 20 sep 2015
        private void DD_Monthbind()
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            for (int i = 1; i < 13; i++)
            {
                //Response.Write(info.GetMonthName(i) + "<br />");

                ddlMonth.Items.Add(new ListItem(info.GetMonthName(i), i.ToString()));
            }

        }
        //for Month wise Report
        protected void showDurationReportMonthWise(int getMonth, int getYear)
        {
            DataTable maindt = new DataTable();

            maindt.Columns.Add("wcid", typeof(int));
            maindt.Columns.Add("id", typeof(string));
            maindt.Columns.Add("Date", typeof(DateTime));
            maindt.Columns.Add("wcmasterid", typeof(int));
            maindt.Columns.Add("MachineName", typeof(string));
            maindt.Columns.Add("manningid", typeof(int));
            maindt.Columns.Add("EmployeeNo", typeof(string));
            maindt.Columns.Add("InspectorName", typeof(string));
           
                DataTable viewmonthdt = new DataTable();
                DataTable manningdt = new DataTable();
                DataTable wdt = new DataTable();
                int totaldaysinMonth = 0;
               
               // DataTable gridviewdt = new DataTable();
                gridviewdt.Columns.Add("InspectorName", typeof(string));

                if (getMonth != Convert.ToInt32(DateTime.Now.Month.ToString()))
                {
                     totaldaysinMonth = DateTime.DaysInMonth(getYear, getMonth);
                    
                }
                else
                {

                    totaldaysinMonth = Convert.ToInt32(System.DateTime.Now.Day);
                }
                var fromDate = getYear + "-" + getMonth.ToString() + "-01 07:00:00";
                int month = 0;
                if (getMonth + 1 == 13)
                { month = 01;
                getYear = getYear + 1;
                }
                else
                { month = getMonth + 1;
                }
                var toDate = getYear + "-" + (month).ToString() + "-01 07:00:00";
                for (int i = 1; i <= (totaldaysinMonth); i++)
                gridviewdt.Columns.Add(i.ToString(), typeof(int));
              try
              {
                  string tableName = "";
                  if (ddlProcess.SelectedValue == "PCR")
                  {
                      tableName = "vInspectionPcr";
                      
                  }
                  else
                  {
                      tableName = "vTBRVisualInspection"; 
                  }

                  myconnection.open(ConnectionOption.SQL);
                  myconnection.comm = myconnection.conn.CreateCommand();
                  myconnection.comm.CommandText = ("SELECT wcid,manningid,dtandtime fROM " + tableName + " where manningId in (select id from manningmaster) and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");
                  mywebservice.writeLogs(myconnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); 
                  myconnection.reader = myconnection.comm.ExecuteReader();
                  viewmonthdt.Load(myconnection.reader);
                  myconnection.comm.Dispose();
                  myconnection.reader.Close();
                  myconnection.close(ConnectionOption.SQL);



                   myconnection.open(ConnectionOption.SQL);
                   myconnection.comm = myconnection.conn.CreateCommand();
                   if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                   {
                       var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                       var firstName = names[0];
                       var lastName = names[1];
                       myconnection.comm.CommandText = ("SELECT id,sapcode,firstname+' '+lastname as InspectorName fROM manningMaster where firstName='" + firstName + "' and lastName='" + lastName + "' '");
                   }
                   else 
                   {
                       myconnection.comm.CommandText = ("SELECT id,sapcode,firstname+' '+lastname as InspectorName fROM manningMaster "); 
                   }
                  
                   myconnection.reader = myconnection.comm.ExecuteReader();
                   manningdt.Load(myconnection.reader);
                   myconnection.comm.Dispose();
                   myconnection.reader.Close();
                   myconnection.close(ConnectionOption.SQL);

                  

                   myconnection.open(ConnectionOption.SQL);
                   myconnection.comm = myconnection.conn.CreateCommand();
                   myconnection.comm.CommandText = ("SELECT id as wcMasterID, name from wcmaster");
                   myconnection.reader = myconnection.comm.ExecuteReader();
                   wdt.Load(myconnection.reader);
                   myconnection.comm.Dispose();
                   myconnection.reader.Close();
                   myconnection.close(ConnectionOption.SQL);

                   var rOw = from r0w1 in viewmonthdt.AsEnumerable()
                             join r0w2 in wdt.AsEnumerable()
                               on r0w1.Field<int>("wcID") equals r0w2.Field<int>("wcMasterID")
                             join r0w3 in manningdt.AsEnumerable()
                               on r0w1.Field<int?>("manningid") equals r0w3.Field<int?>("id") into ps
                             from r0w3 in ps.DefaultIfEmpty()
                             select r0w1.ItemArray.Concat(r0w2.ItemArray.Concat(r0w3 != null ? r0w3.ItemArray : new object[] { 0, 0, 0 })).ToArray();

                   foreach (object[] values in rOw)
                       maindt.Rows.Add(values);


                //myconnection.open(ConnectionOption.SQL);
                //myconnection.comm = myconnection.conn.CreateCommand();
                //if (ddlProcess.SelectedValue == "PCR")
                //{
                //    if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                //    {
                //        var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                //        var firstName = names[0];
                //        var lastName = names[1];



                //        myconnection.comm.CommandText = ("SELECT sapcode,firstname+' '+lastname as InspectorName,dtandtime FROM vVisualInspectionPCR where firstName='" + firstName + "' and lastName='" + lastName + "'  and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");

                //    }
                //    else
                //    {
                //        myconnection.comm.CommandText = ("SELECT sapcode,firstname+' '+lastname as InspectorName,dtandtime FROM vVisualInspectionPCR where firstname ! = 'null' and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");
                //    }
                //}
                //if (ddlProcess.SelectedValue == "TBR")
                //{
                //    if (MachineDropdown.SelectedValue != "ALL" && MachineDropdown.SelectedValue != "" && MachineDropdown.SelectedValue != "0")
                //    {
                //        var names = MachineDropdown.SelectedValue.Split(new char[] { '_' });
                //        var firstName = names[0];
                //        var lastName = names[1];
                //        myconnection.comm.CommandText = ("SELECT sapcode,firstname+' '+lastname as InspectorName,dtandtime FROM vTBRVisualInspection where firstName='" + firstName + "' and lastName='" + lastName + "'  and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");
                //    }
                //    else
                //    {
                //        myconnection.comm.CommandText = ("SELECT sapcode,firstname+' '+lastname as InspectorName,dtandtime FROM vTBRVisualInspection where firstname ! = 'null' and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");
                //    }
                //} 
                //myconnection.reader = myconnection.comm.ExecuteReader();
                //viewmonthdt.Load(myconnection.reader);
                //myconnection.comm.Dispose();
                //myconnection.reader.Close();
                //myconnection.close(ConnectionOption.SQL);

                var totalCount = maindt.AsEnumerable().GroupBy(p => new { day = p.Field<DateTime>("Date").AddHours(-7).Day, InspectorName = p.Field<string>("InspectorName") })
               .Select(e => new { InspectorName = e.Key.InspectorName, day = e.Key.day, countno = e.Distinct().Count() });
                //var tdt = DateTime.Today.Day.ToString();
                var aAAray = totalCount.ToArray();
                 
                string[] TobeDistinctUser = { "InspectorName" };
                DataTable dtDistinctUser = new DataTable();
                dtDistinctUser = GetDistinctRecordsName(maindt, TobeDistinctUser);
                for (int i = 0; i < dtDistinctUser.Rows.Count; i++)
                {
                    DataRow dr = gridviewdt.NewRow();
                    // dr[0] = aAAray[i].InspectorName.ToString();
                    dr[0] = dtDistinctUser.Rows[i][0].ToString();
                    if (dtDistinctUser.Rows.Count > 0)
                    {
                        for (int k = 1; k <= (totaldaysinMonth); k++)
                        {
                            int count = maindt.AsEnumerable().Count(row => row.Field<DateTime>("Date").AddHours(-7).Day.Equals(k) && row.Field<string>("InspectorName").Equals(dtDistinctUser.Rows[i][0]));
                            dr[k] = count.ToString();
                        }
                    }
                    //dr[MonthArray[i].day.ToString()] = MonthArray[i].countno.ToString();
                    gridviewdt.Rows.Add(dr);
                }
               MonthwiseGridView.DataSource = gridviewdt;
               MonthwiseGridView.DataBind();
               if (MonthwiseGridView.Rows.Count > 0)
               {
                   InnerLabel.Visible = false;
               }
               else
               {
                   InnerLabel.Visible = true;
               }
            }
            catch (Exception ex)
             { }
        }
        protected void ddlmonthselection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmonthselection.SelectedValue == "Daily")
            {
                pnlshift.Visible = true;
                DailyselectionPanel.Visible = true;
                MonthlyselectionPanel.Visible = false;
                MonthWisePanel.Visible = false;
                LabelDive.Visible = true;
                gridDive.Visible = true;
            }
            else
            {
                pnlshift.Visible = false;
                MonthlyselectionPanel.Visible = true;
                DailyselectionPanel.Visible = false;
                MonthWisePanel.Visible = true;
                LabelDive.Visible = false;
                gridDive.Visible = false;

            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);
       protected void ExcelMachineButton_Click(object sender, EventArgs e)
        {

            string duration = ddlmonthselection.SelectedItem.Value;
            string type = ddlProcess.SelectedItem.Value;
            string getTimeDuration = "";
            switch (duration)
            {
                case "Daily":
                    getTimeDuration = "<b>Date :</b> " + cadatetextbox.Text;
                    break;
                case "Monthly":
                    getTimeDuration = "<b>Month :</b> " + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Value;
                    break;
            }



            Response.Clear();
            string filename = "VisualInspectionProduction Report_" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            stringWrite.Write("<table><tr><td><b>Visual Inspection Production Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + ddlProcess.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr></table>");

            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            Table tb = new Table();

            TableRow tr1 = new TableRow();
            TableCell cell1 = new TableCell();
            cell1.Text = "&nbsp";
            tr1.Cells.Add(cell1);

            TableRow tr2 = new TableRow();
            TableCell cell2 = new TableCell();
            cell2.Controls.Add(GridViewproduction);
            tr2.Cells.Add(cell2);

            TableCell cell4 = new TableCell();
            cell4.Controls.Add(MonthwiseGridView);

            TableCell cell3 = new TableCell();
            cell3.Text = "&nbsp;";
            TableRow tr3 = new TableRow();
            tr3.Cells.Add(cell3);

            tb.Rows.Add(tr3);
            if (ddlmonthselection.SelectedValue == "Monthly")
            {

                TableRow tr4 = new TableRow();
                tr4.Cells.Add(cell4);
                tb.Rows.Add(tr4);
            }
            else
            {
                tb.Rows.Add(tr2);
            }
            tb.RenderControl(htmlWrite);

            Response.Write(stringWrite.ToString());
            Response.End(); 
            }
          
            
        private void stopExcelProcee(int proID, string ProcessName)
         {
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(ProcessName);
            foreach (System.Diagnostics.Process p in process)
            {
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                }
            }
        }
        private DataTable GetDistinctRecordsName(DataTable dt, string[] Columns)
        {
            DataTable dtUniqUserRecords = new DataTable();
            dtUniqUserRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqUserRecords;
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
                DataTable dtMonth = new DataTable();
                int getMonth = ddlMonth.SelectedIndex;
                ArrayList storeALLField = new ArrayList();
                int getyear = Convert.ToInt32(ddlYear.SelectedItem.Text);
                var fromDate = getyear + "-" + getMonth.ToString() + "-01 07:00:00";
                var toDate = getyear + "-" + (getMonth + 1).ToString() + "-01 07:00:00";
                storeALLField.Add("ALL");
               
             try
             {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                if (ddlProcess.SelectedValue == "PCR")
                {
                    myconnection.comm.CommandText = ("SELECT distinct firstname+'_'+lastname as InspectorName FROM vVisualInspectionPCR where firstname ! = 'null' and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");
                }
                else
                {
                    myconnection.comm.CommandText = ("SELECT distinct firstname+'_'+lastname as InspectorName FROM vTBRVisualInspection where firstname ! = 'null' and dtandTime> '" + fromDate + "' and dtandTime< '" + toDate + "'");

                }
                     // MachineDropdown.DataSource = myconnection.comm.ExecuteReader();
                 myconnection.reader = myconnection.comm.ExecuteReader();
                 while (myconnection.reader.Read())
                 {
                     if (myconnection.reader.ToString() == "")
                       {
                           MachineDropdown.Items.Clear();
                        }
                     else
                     {

                         storeALLField.Add(myconnection.reader[0].ToString());
                     }
                 }
                 
                MachineDropdown.Items.Clear();
               // MachineDropdown.Items.Add("ALL");
                MachineDropdown.DataSource = storeALLField;
                //MachineDropdown.DataTextField = "";
                //MachineDropdown.DataValueField = "InspectorName";
                MachineDropdown.DataBind();
                myconnection.reader.Close();

              }
            catch(Exception ex)
            {}

        }
        protected void MonthwiseGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
               e.Row.Cells[0].Width = new Unit("180px");
                //e.Row.Cells[1].Width = new Unit("250px");
            }
            foreach (GridViewRow row in MonthwiseGridView.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Attributes.CssStyle["text-align"] = "center";
                }
            }
        }
        protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList storeCurrentname = new ArrayList();
            storeCurrentname.Add("ALL");
               
            if (ddlProcess.SelectedValue == "PCR")
            {
                ddlmonthselection.SelectedValue = "Daily";
                DailyselectionPanel.Visible = true;
                MonthlyselectionPanel.Visible = false;
                MonthwiseGridView.DataSource = null;
                MonthwiseGridView.DataBind();
                MachineDropdown.Items.Clear();

                try
                {
                    DataTable currentdt = new DataTable();

                    myconnection.open(ConnectionOption.SQL);
                    myconnection.comm = myconnection.conn.CreateCommand();
                    DateTime dtValue = Convert.ToDateTime(cadatetextbox.Text);
                    var fromDate = dtValue.ToString("yyyy-MM-dd");
                    var ToDate = dtValue.AddDays(1).ToString("yyyy-MM-dd");
                       myconnection.comm.CommandText = ("SELECT distinct firstname+' '+lastname as InspectorName fROM manningMaster where areaName='PCRCuring' order by InspectorName");
                        //"SELECT distinct firstName + '_' + lastName As InspectorName FROM vInspectionManningPCR WHERE dtandTime> '" + fromDate + " " + "07:00:00" + "' and dtandTime< '" + ToDate + " " + "07:00:00" + "'");
                    
                    myconnection.reader = myconnection.comm.ExecuteReader();
                    while (myconnection.reader.Read())
                    {
                        if (myconnection.reader.ToString() == "")
                        {
                            MachineDropdown.Items.Clear();
                        }
                        else
                        {

                            storeCurrentname.Add(myconnection.reader[0].ToString());
                        }
                    }
                    MachineDropdown.Items.Clear();
                    MachineDropdown.DataSource = storeCurrentname;
                    MachineDropdown.DataBind();
                    myconnection.reader.Close();


                }
                catch (Exception ex)
                {
                    mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

            }
            if (ddlProcess.SelectedValue == "TBR")
            {
                ddlmonthselection.SelectedValue = "Daily";
                DailyselectionPanel.Visible = true;
                //MonthWisePanel.Visible = false;
                MonthlyselectionPanel.Visible = false;
                MonthwiseGridView.DataSource = null;
                MonthwiseGridView.DataBind();
                MachineDropdown.Items.Clear();
                try
                {
                    DataTable currentdt = new DataTable();

                    myconnection.open(ConnectionOption.SQL);
                    myconnection.comm = myconnection.conn.CreateCommand();
                    DateTime dtValue = Convert.ToDateTime(cadatetextbox.Text);
                    var fromDate = dtValue.ToString("yyyy-MM-dd");
                    var ToDate = dtValue.AddDays(1).ToString("yyyy-MM-dd");
                    myconnection.comm.CommandText = ("SELECT distinct firstname+' '+lastname as InspectorName fROM manningMaster where areaName='TBRCuring' order by InspectorName");
                    //myconnection.comm.CommandText = ("SELECT distinct firstName + '_' + lastName As InspectorName FROM vInspectionManningTBR WHERE dtandTime> '" + fromDate + " " + "07:00:00" + "' and dtandTime< '" + ToDate + " " + "07:00:00" + "'");


                    myconnection.reader = myconnection.comm.ExecuteReader();
                    while (myconnection.reader.Read())
                    {
                        if (myconnection.reader.ToString() == "")
                        {
                            MachineDropdown.Items.Clear();
                        }
                        else
                        {

                            storeCurrentname.Add(myconnection.reader[0].ToString());
                        }
                    }
                    MachineDropdown.Items.Clear();
                    MachineDropdown.DataSource = storeCurrentname;
                    MachineDropdown.DataBind();
                    myconnection.reader.Close();


                }
                catch (Exception ex)
                {
                    mywebservice.writeLogs(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        { }
      }
}

