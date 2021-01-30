using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Web.SessionState;
using SmartMIS.SmartWebReference;

namespace SmartMIS
{
    public partial class smartMISTBMproductionReport : System.Web.UI.MasterPage
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string processName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                {
                    fillProcessDropDownList();
                    fillContentGridView(Page.Title.Trim());

                    reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    DropDownListMonth.SelectedValue = DateTime.Now.ToString("MM");
                    DropDownListYear.SelectedValue = DateTime.Now.ToString("yyyy");
                    DropDownListYearWise.SelectedValue = DateTime.Now.ToString("yyyy");
                }
        }
        private void fillProcessDropDownList()
        {
            //Description   : Function for filling reportMasterWCProcessDropDownList
            //Author        : Brajesh kumar
            //Date Created  : 07 February 2012
            //Date Updated  : 07 February 2012
            //Revision No.  : 01
            //Revision Desc : 

            reportMasterWCProcessDropDownList.Items.Clear();
            reportMasterWCProcessDropDownList.Items.Add("");

            // reportMasterWCProcessDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name");
            reportMasterWCProcessDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name", "where iD in (4,7) ");
            reportMasterWCProcessDropDownList.DataBind();

        }
        private void fillContentGridView(string option)
        {

            //Description   : Function for filling reportMasterContentGridView
            //Author        : Brajesh kumar
            //Date Created  : 17 May 2011
            //Date Updated  : 17 May 2011
            //Revision No.  : 01
            //Revision Desc :


            if (option == "Weighment Report")
            {
                reportMasterContentDiv.Visible = true;

                reportMasterContentGridView.DataSource = myWebService.fillGridView("Select iD, name from materialTypeMaster", ConnectionOption.SQL);
                reportMasterContentGridView.DataBind();
            }
            else
            {

            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());
            DropDownListOperators.Visible = false;
            if (((DropDownList)sender).ID == "reportMasterReportTypeDropDownList")
            {
                if (((DropDownList)sender).SelectedIndex == 0)
                {
                    reportMasterWCDiv.Visible = false;
                }
                else if (((DropDownList)sender).SelectedIndex == 1)
                {
                    reportMasterWCDiv.Visible = true;
                }
            }
            if (((DropDownList)sender).ID == "reportMasterWCProcessDropDownList")
            {
                fillWCGridView(Page.Title.Trim(), ((DropDownList)sender).SelectedItem.ToString());
                reportMasterWCAllCheckBox.Checked = false;

                if (reportMasterWCProcessDropDownList.SelectedValue == "Tyre Building PCR")
                    processName = "7";  //Building PCR
                else if (reportMasterWCProcessDropDownList.SelectedValue == "Tyre Building TBR")
                    processName = "4";  //Building TBR
                selectAll.Visible = true;
                reportMasterWCPanel.Visible = true;
            }
            if (((DropDownList)sender).ID == "DropDownListType")
            {
                if (DropDownListType.SelectedItem.Value == "OperatorWise")
                {
                    DropDownListOperators.Items.Clear();

                    string sqlQuery = "";

                    ListItem litem = new ListItem("All", "All");
                    DropDownListOperators.Items.Add(litem);

                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        if (reportMasterWCProcessDropDownList.SelectedValue == "Tyre Building PCR")
                        {
                            myConnection.comm.CommandText = "Select DISTINCT sapcode  from manningmaster where areaName='TBMPCR' and deptid='15' order by sapcode asc"; 
                           // myConnection.comm.CommandText = "Select DISTINCT iD, firstName, lastName from vManning WHERE ID IN (SELECT DISTINCT manningID FROM tbmPCR) order by firstName asc";
                            //myConnection.comm.CommandText = "select distinct mm.sapCode from vTbmPCR  inner join manningmaster mm on vTbmPCR.manningID2 = mm.iD  or vTbmPCR.manningID3 = mm.iD or vTbmPCR.manningID = mm.iD order by mm.sapcode asc";
                        }
                        else if (reportMasterWCProcessDropDownList.SelectedValue == "Tyre Building TBR")
                        {
                            myConnection.comm.CommandText = "Select DISTINCT sapcode  from manningmaster where areaName='TBMTBR' and deptid='15' order by sapcode asc"; 
                            //myConnection.comm.CommandText = "Select DISTINCT iD, firstName, lastName from vManning WHERE ID IN (SELECT DISTINCT manningID FROM tbmtbr) order by firstName asc"; 
                        //}
                            //myConnection.comm.CommandText = "select distinct mm.sapCode from vTbmtbR  inner join manningmaster mm on vTbmtbR.manningID2 = mm.iD  or vTbmtbR.manningID3 = mm.iD or vTbmtbR.manningID = mm.iD order by mm.sapcode asc";
                        }
                        else
                        {
                            //myConnection.comm.CommandText = "Select DISTINCT iD, firstName, lastName from vManning order by firstName asc"; 
                            myConnection.comm.CommandText = "Select DISTINCT sapcode  from vManning order by sapcode asc"; 
                        
                        }

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        myConnection.comm.CommandTimeout = 600;
                        while (myConnection.reader.Read())
                        {
                            if (myConnection.reader[0].ToString() != "")
                            {
                                //ListItem li = new ListItem(myConnection.reader[1].ToString() + " " + myConnection.reader[2].ToString(), myConnection.reader[0].ToString());
                                ListItem li = new ListItem(myConnection.reader[0].ToString());
                                DropDownListOperators.Items.Add(li);
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    DropDownListOperators.Visible = true;
                }
            }
        }
        
        public ArrayList FillDropDownList(string tableName, string coloumnName)
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

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
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);


            return flag;
        }

        private void fillWCGridView(string option, string processName)
        {

            //Description   : Function for filling reportMasterWCGridView
            //Author        : Brajesh kumar   || Brajesh kumar
            //Date Created  : 22 April 2011 ||
            //Date Updated  : 22 April 2011 || 09-May-2011                                                  ||  01-June-2011
            //Revision No.  : 01            || 02                                                           ||  03
            //Revision Desc :               || Change the logic for filling the gridview by webpage title   ||  Add the logic for visibility of Plantwide and workcenter wide DropDownList


            if (option == "Material Consumption")
            {
                //reportMasterReportTypeDropDownList.Style.Add(HtmlTextWriterStyle.Display, "none");
                reportMasterWCDiv.Visible = true;

                reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster WHERE processID = 28", ConnectionOption.SQL);
                reportMasterWCGridView.DataBind();
            }
            else if (option == "Downtime Report")
            {
                //reportMasterReportTypeDropDownList.SelectedIndex = 1;
                //reportMasterReportTypeDropDownList.Style.Add(HtmlTextWriterStyle.Display, "none");

                reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster", ConnectionOption.SQL);
                reportMasterWCGridView.DataBind();

                reportMasterWCDiv.Visible = true;
            }
            else
            {
                reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, workCenterName as name from vWorkCenter WHERE processName = '" + processName + "'", ConnectionOption.SQL);
                reportMasterWCGridView.DataBind();
            }
        }
        private void retainControlState()
        {

        }

        protected void Export_click(object sender, EventArgs e)
        {
            string duration = DropDownListDuration.SelectedItem.Value;
            string type = DropDownListType.SelectedItem.Value;
            string getTimeDuration = "";

            Panel gvpanel = (Panel)TBMMasterContentPalceHolder.FindControl("gvpanel");
            switch (duration)
            { 
                case "Date":
                    getTimeDuration = "<b>Date :</b> " + reportMasterFromDateTextBox.Text;
                    break;
                case "Month":
                    getTimeDuration = "<b>Month :</b> " + DropDownListMonth.SelectedItem.Text + " " + DropDownListYear.SelectedItem.Value;
                    break;
                case "Year":
                    getTimeDuration = "<b>Year :</b> " + DropDownListYearWise.SelectedItem.Value;
                    break;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=TBMProductionReport" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            stringWrite.Write("<table><tr><td><b>TBM Production Report</b></td><td>" + getTimeDuration + "</td><td><b>Type :</b> " + type + "</td><td><b>" + reportMasterWCProcessDropDownList.SelectedItem.Value + "</b></td></tr></table>");
            System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
            Controls.Add(form);
            form.Controls.Add(gvpanel);
            form.RenderControl(htmlWrite);

            //gv.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();

        }
        //public override void VerifyRenderingInServerForm(Control control) //To prevent Export To Excel Error
        //{
        //}

        ArrayList wcID = new ArrayList();
        protected void viewReport_Click(object sender, EventArgs e)
        {
            //If Operatorwise selected, make operator dropdown visible true
            if (DropDownListType.SelectedItem.Value == "OperatorWise")
                DropDownListOperators.Visible = true;
            int j = 0;
            foreach (GridViewRow gvRow in reportMasterWCGridView.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("reportMasterWCCheckBox");
                if (chkSel.Checked)
                {
                    //wcNames.Add(((Label)reportMasterWCGridView.Rows[j].FindControl("reportMasterWCNameLabel")).Text);
                    wcID.Add(((Label)reportMasterWCGridView.Rows[j].FindControl("reportMasterWCIDLabel")).Text);
                    //Cells[3] is the column to get one by one rows ceslls[3] columns
                    
                }
                j++;
            }

            string duration = "";
            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    duration = "day";
                    break;
                case "Month":
                    duration = "month";
                    break;
                case "Year":
                    duration = "year";
                    break;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanelProcess, this.GetType(), "myScript", "showDuration('"+duration+"')", true);
            if (contentCallEvent != null)
                contentCallEvent(this, EventArgs.Empty);
        }
        public event EventHandler contentCallEvent;

        // For TBR/PCR
        public string reportMasterWCProcessDropDownListValue
        {
            get { return this.reportMasterWCProcessDropDownList.SelectedItem.Value; }
            set { this.DropDownListType.SelectedItem.Value = value; }
        }

        // Get List of WorkCenters
        public ArrayList reportMasterWCGridViewValue
        {
            get { return wcID; }
            //set { this.DropDownListType.Text = value; }

        }
        
        // For Type
        public string DropDownListOperatorsValue
        {
            get { return this.DropDownListOperators.SelectedItem.Value; }
            set { this.DropDownListOperators.SelectedItem.Value = value; }
        }

        // For Operators
        public string DropDownListTypeValue
        {
            get { return this.DropDownListType.Text; }
            set { this.DropDownListType.Text = value; }
        }

        // For Daywise
        public string reportMasterFromDateTextBoxValue
        {
            get { return this.reportMasterFromDateTextBox.Text; }
            set { this.reportMasterFromDateTextBox.Text = value; }
        }
        public string DropDownListDurationValue
        {
            get { return this.DropDownListDuration.SelectedItem.Value; }
            set { this.DropDownListDuration.SelectedItem.Value = value; }
        }

        // For Monthwise
        public string DropDownListMonthValue
        {
            get { return this.DropDownListMonth.SelectedItem.Value; }
            set { this.DropDownListMonth.SelectedItem.Value = value; }
        }
        public string DropDownListYearValue
        {
            get { return this.DropDownListYear.Text; }
            set { this.DropDownListYear.Text = value; }
        }

        // For Yearwise
        public string DropDownListYearWiseValue
        {
            get { return this.DropDownListYearWise.Text; }
            set { this.DropDownListYearWise.Text = value; }
        }
    }
}
