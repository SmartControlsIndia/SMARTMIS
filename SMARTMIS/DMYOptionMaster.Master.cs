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
    public partial class DMYOptionMaster : System.Web.UI.MasterPage
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string processName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                fillContentGridView(Page.Title.Trim());

                reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                DropDownListMonth.SelectedValue = DateTime.Now.ToString("MM");
                DropDownListYear.SelectedValue = DateTime.Now.ToString("yyyy");
                DropDownListYearWise.SelectedValue = DateTime.Now.ToString("yyyy");
            }
        }
        private void fillRecipeCodeDropDownList()
        {
            //Description   : Function for filling reportMasterWCProcessDropDownList
            //Author        : Brajesh kumar
            //Date Created  : 07 February 2012
            //Date Updated  : 07 February 2012
            //Revision No.  : 01
            //Revision Desc : 

            DropDownListType.Items.Clear();

            // reportMasterWCProcessDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name");
            //fillWCGridView(Page.Title.Trim(), ((DropDownList)sender).SelectedItem.ToString());

            if(reportMasterWCProcessDropDownList.SelectedValue=="TBR")
            DropDownListType.DataSource = FillDropDownList("BuddeScannedTyreDetail", "recipeCode");
            else if (reportMasterWCProcessDropDownList.SelectedValue=="PCR")
                DropDownListType.DataSource = FillDropDownList("PCRBuddeScannedTyreDetail", "recipeCode");

            DropDownListType.DataBind();
        }
        private void fillContentGridView(string option)
        {

            //Description   : Function for filling reportMasterContentGridView
            //Author        : Brajesh kumar
            //Date Created  : 17 May 2011
            //Date Updated  : 17 May 2011
            //Revision No.  : 01
            //Revision Desc :

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

        private void retainControlState()
        {

        }

        protected void Export_click(object sender, EventArgs e)
        {
            if (contentCallEvent1 != null)
                contentCallEvent1(this, EventArgs.Empty);

        }
        //public override void VerifyRenderingInServerForm(Control control) //To prevent Export To Excel Error
        //{
        //}

        ArrayList wcID = new ArrayList();
        protected void viewReport_Click(object sender, EventArgs e)
        {
            //If Operatorwise selected, make operator dropdown visible true
            //if (DropDownListType.SelectedItem.Value == "OperatorWise")
            //    DropDownListOperators.Visible = true;
            int j = 0;
            //foreach (GridViewRow gvRow in reportMasterWCGridView.Rows)
            //{
            //    CheckBox chkSel = (CheckBox)gvRow.FindControl("reportMasterWCCheckBox");
            //    if (chkSel.Checked)
            //    {
            //        //wcNames.Add(((Label)reportMasterWCGridView.Rows[j].FindControl("reportMasterWCNameLabel")).Text);
            //        wcID.Add(((Label)reportMasterWCGridView.Rows[j].FindControl("reportMasterWCIDLabel")).Text);
            //        //Cells[3] is the column to get one by one rows ceslls[3] columns

            //    }
            //    j++;
            //}

            string duration = "";
            switch (DropDownListDuration.SelectedItem.Value)
            {
                case "Date":
                    duration = "Day";
                    break;
                case "Month":
                    duration = "Month";
                    break;
                case "Year":
                    duration = "Year";
                    break;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanelProcess, this.GetType(), "myScript", "showDuration('" + duration + "','hideWCIDDiv')", true);
            if (contentCallEvent != null)
                contentCallEvent(this, EventArgs.Empty);
        }
        public event EventHandler contentCallEvent;
        public event EventHandler contentCallEvent1;


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

        //// For Type
        //public string DropDownListOperatorsValue
        //{
        //    get { return this.DropDownListOperators.SelectedItem.Value; }
        //    set { this.DropDownListOperators.SelectedItem.Value = value; }
        //}

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

        protected void reportMasterWCProcessDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRecipeCodeDropDownList();
        }

   
    }
}
