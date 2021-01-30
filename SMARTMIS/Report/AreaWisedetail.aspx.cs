using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace SmartMIS.Report
{
    public partial class AreaWisedetail : System.Web.UI.Page
    {
        #region classes
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
                #endregion
        #region globle variable
        public int totalcheckedcount = 0, okcount = 0, datatotal = 0, data_total = 0, carcass_total = 0, bead_total = 0, tread_Total = 0, s_total = 0, o_total = 0;
        int status;
        DataTable exldt; DataTable mainGVdt;
        DataTable dtInspectoon = new DataTable();
        DataTable gridviewdt = new DataTable();
        DataTable tbldt = new DataTable();
        string nfromDate;
        string ntoDate;
       
        string wcIDInQuery = "(";
        ArrayList wcID = new ArrayList(); 
        #endregion
        string getType;

        string wherequery = "";
        string duration = "";
        
        string getfromdate;
        string gettodate;
        DateTime fromDate;
        DateTime toDate;
        DataTable dt = new DataTable();
        string durationQuery = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                ShowWarning.Visible = false;
                //Label2.Visible = false;

                if (string.IsNullOrEmpty(reportMasterFromDateTextBox.Text))  // If Textbox already null, then show current Date
                {
                    reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                   // string rToDate = formatDate(tuoReportMasterFromDateTextBox.Text);
                   // string rFromDate = formatDate(tuoReportMasterToDateTextBox.Text);
                   // fillSizedropdownlist();

                }
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }

            }
            catch (Exception exp)
            {
               // myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }
        //private void fillSizedropdownlist()
        //{

        //    DataTable d_dt = new DataTable();

        //    myConnection.open(ConnectionOption.SQL);
        //    myConnection.comm = myConnection.conn.CreateCommand();
        //    myConnection.comm.CommandText = "Select DISTINCT id as rID,description from recipemaster where description != '0' and description !='' and tyreSize!=''and processID = 8";
        //    myConnection.reader = myConnection.comm.ExecuteReader();
        //    d_dt.Load(myConnection.reader);

        //    ddlRecipe.DataSource = d_dt;
        //    ddlRecipe.DataTextField = "description";
        //    ddlRecipe.DataValueField = "rID";
        //    ddlRecipe.DataBind();
        //    ddlRecipe.Items.Insert(0, new ListItem("All", "All"));
        //}
        
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            //string recipe = ddlRecipe.SelectedItem.Text;
            string duration = "";
            var datetimebt = "";
            string getfromdate = reportMasterFromDateTextBox.Text;

             toDate =  DateTime.Parse(formatDate(reportMasterFromDateTextBox.Text));
             fromDate = toDate.AddDays(1);
                  
           loadData();
           
        }
        //private void showReport()
        //{
        //    try
        //    {
        //        //loadData();
        //        DataTable dtArea = new DataTable();
        //        DataTable gridviewdt = new DataTable();
        //        DataRow dr;
        //        gridviewdt.Columns.Add("Name", typeof(string));
        //        gridviewdt.Columns.Add("Curing", typeof(string));
        //        gridviewdt.Columns.Add("nC", typeof(string));
        //        gridviewdt.Columns.Add("VI", typeof(string));
        //        gridviewdt.Columns.Add("SecondVI", typeof(string));
        //        myConnection.open(ConnectionOption.SQL);
        //        myConnection.comm = myConnection.conn.CreateCommand();

        //        myConnection.comm.CommandText = "SELECT count(*) as tbm ,(SELECT COUNT(*)as curing  from curingtbr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' ) AS Curing,(SELECT COUNT(*) as Total  from vInspectionTBR where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' ) AS VI ,(SELECT COUNT(*) as Total  from vInspectionTBR where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' and wcid in (select id from wcmaster where VIstage=5 )) AS 2VI FROM tbmtbr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' ";
        //        myConnection.reader = myConnection.comm.ExecuteReader();
        //        dtArea.Load(myConnection.reader);
        //        myConnection.reader.Close();
        //        myConnection.comm.Dispose();
        //        myConnection.close(ConnectionOption.SQL);

        //        dr = gridviewdt.NewRow();
        //        for (int i = 0; i < dtArea.Rows.Count; i++)
        //        {
        //            dr[0] = dtArea.Rows[i][0].ToString();
        //            dr[1] = dtArea.Rows[i][1].ToString();
        //            dr[2] = Convert.ToInt32( dtArea.Rows[i][0].ToString()) - Convert.ToInt32( dtArea.Rows[i][1].ToString());
        //            dr[3] = dtArea.Rows[i][2].ToString();
        //            dr[4] = dtArea.Rows[i][3].ToString();
        //        }
        //        gridviewdt.Rows.Add(dr);
        //        grdView.DataSource = gridviewdt;
        //        grdView.DataBind();



        //    }
        //    catch (Exception exp)
        //    {
        //        //myWebService.writeLogs(exp.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        //    }
        //}
        private void loadData()
        {
            DataTable dt_vi = new DataTable();
            DataTable dt_workcenter = new DataTable();
            DataTable dt_Curing = new DataTable();
            DataTable dt_TBM = new DataTable();
            DataTable gridviewdt = new DataTable();
            DataRow dr;
            gridviewdt.Columns.Add("TBM", typeof(string));
            gridviewdt.Columns.Add("Curing", typeof(string));
            gridviewdt.Columns.Add("NOt in Curing", typeof(string));
            gridviewdt.Columns.Add("VI", typeof(string));
            gridviewdt.Columns.Add("SecondVI", typeof(string));

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            if (DropDownList1.SelectedValue == "PCR")
            { 
                myConnection.comm.CommandText = "select count(*) as VI from VInspectionPCR where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' ";
            }
            else { 
                myConnection.comm.CommandText = "select count(*) as VI from VInspectionTBR where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "' ";
            }
           
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_vi.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            if (DropDownList1.SelectedValue == "PCR")
            {
                myConnection.comm.CommandText = "Select count(*) as curing from curingpcr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "'";
            }
            else
            {
                myConnection.comm.CommandText = "Select count(*) as curing from curingtbr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "'";
            }
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_Curing.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            if (DropDownList1.SelectedValue == "PCR")
            {
                myConnection.comm.CommandText = "Select count(*) as TBM from tbmpcr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "'";
            }
            else
            {
                myConnection.comm.CommandText = "Select count(*) as TBM from tbmtbr where dtandTime > '" + toDate + "' and dtandtime<'" + fromDate + "'";
            }
            myConnection.reader = myConnection.comm.ExecuteReader();
            dt_TBM.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            dr = gridviewdt.NewRow();
            if (dt_TBM.Columns.Count > 0)
            {
                dr[0] = dt_TBM.Rows[0]["TBM"].ToString();
                dr[1] = dt_Curing.Rows[0][0].ToString();
                dr[2] = Convert.ToInt32(dt_TBM.Rows[0][0].ToString()) - Convert.ToInt32(dt_Curing.Rows[0][0].ToString());
                dr[3] = dt_vi.Rows[0][0].ToString();
                dr[4] = dt_vi.Rows[0][0].ToString();
            }

            gridviewdt.Rows.Add(dr);

            grdView.DataSource = gridviewdt;
            grdView.DataBind();
           
        }
        public string formatDate(String date)
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
                    flag = day + "-" + month + "-" + year + " " + "07" + ":" + "00" + ":" + "00";

                }
                catch (Exception exp)
                {
                    //myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                }
            }
            return flag;
        }



        
    }
}
