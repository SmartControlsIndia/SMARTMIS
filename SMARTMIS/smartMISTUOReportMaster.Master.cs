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
    public partial class smartMISTUOReportMaster : System.Web.UI.MasterPage
    {

        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();


        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {

                retainControlState();

                if (!Page.IsPostBack)
                {
                    fillProcessDropDownList();
                   // fillWCGridView(Page.Title.Trim());
                    fillReportType();
                    fillContentGridView(Page.Title.Trim());
                    fillDate();
                    fillMonth();
                    fillYear();
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                tuoReportMasterWCAllCheckBox.Checked = false;
                if (((DropDownList)sender).ID == "tuoReportMasterReportTypeDropDownList")
                {
                    if (((DropDownList)sender).SelectedIndex == 0)
                    {
                        tuoReportMasterWCDiv.Visible = false;
                    }
                    else if (((DropDownList)sender).SelectedIndex == 1)
                    {
                        tuoReportMasterWCDiv.Visible = true;
                    }
                }
                if (((DropDownList)sender).ID == "tuoReportMasterWCProcessDropDownList")
                {
                    fillWCGridView(Page.Title.Trim(), ((DropDownList)sender).SelectedItem.ToString());
                }
            }

        #endregion

        #region User Defined Function
            private void fillProcessDropDownList()
            {
                //Description   : Function for filling reportMasterWCProcessDropDownList
                //Author        : Brajesh kumar
                //Date Created  : 07 February 2012
                //Date Updated  : 07 February 2012
                //Revision No.  : 01
                //Revision Desc : 

                tuoReportMasterWCProcessDropDownList.Items.Clear();
                tuoReportMasterWCProcessDropDownList.Items.Add("");

                tuoReportMasterWCProcessDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name","where iD in(7,8,18)");
                tuoReportMasterWCProcessDropDownList.DataBind();

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
                    tuoReportMasterReportTypeDropDownList.Style.Add(HtmlTextWriterStyle.Display, "none");
                    tuoReportMasterWCDiv.Visible = true;

                    tuoReportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster WHERE processID = 28", ConnectionOption.SQL);
                    tuoReportMasterWCGridView.DataBind();
                }
                else if (option == "Downtime Report")
                {
                    tuoReportMasterReportTypeDropDownList.SelectedIndex = 1;
                    tuoReportMasterReportTypeDropDownList.Style.Add(HtmlTextWriterStyle.Display, "none");

                    tuoReportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster", ConnectionOption.SQL);
                    tuoReportMasterWCGridView.DataBind();

                    tuoReportMasterWCDiv.Visible = true;
                }
                else
                {
                    tuoReportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, workCenterName as name from vWorkCenter WHERE processName = '" + processName + "'", ConnectionOption.SQL);
                    tuoReportMasterWCGridView.DataBind();
                }
            }

            private void fillContentGridView(string option)
            {

                //Description   : Function for filling tuoReportMasterContentGridView
                //Author        : Brajesh kumar
                //Date Created  : 17 May 2011
                //Date Updated  : 17 May 2011
                //Revision No.  : 01
                //Revision Desc :


                if (option == "Weighment Report")
                {
                    tuoReportMasterContentDiv.Visible = true;

                    tuoReportMasterContentGridView.DataSource = myWebService.fillGridView("Select iD, name from materialTypeMaster", ConnectionOption.SQL);
                    tuoReportMasterContentGridView.DataBind();
                }
                else
                {
                    
                }
            }

            private void fillDate()
            {

                //Description   : Function for filling tuoReportMasterFromDateTextBox and tuoReportMasterToDateTextBox with Default Values
                //Author        : Brajesh kumar
                //Date Created  : 02 May 2011
                //Date Updated  : 02 May 2011
                //Revision No.  : 01
                //Revision Desc :

                tuoReportMasterFromDateTextBox.Disabled = true;
                tuoReportMasterFromDateTextBox.Text = "";
                tuoReportMasterToDateTextBox.Text = "";
                tuoReportMasterDayRadioButton.Checked = false;
                tuoReportMasterMonthRadioButton.Checked = false;
                tuoReportMasterYearRadioButton.Checked = false;
            }

            private void fillReportType()
             {
                 //Description   : Function for filling tuoReportMasterReportTypeDropDownList with Report Type
                 //Author        : Brajesh kumar
                 //Date Created  : 22 April 2011
                 //Date Updated  : 22 April 2011
                 //Revision No.  : 01
                 //Revision Desc :

                 tuoReportMasterReportTypeDropDownList.Items.Clear();

                 foreach (string item in myWebService.reportType)
                 {
                     tuoReportMasterReportTypeDropDownList.Items.Add(item);
                 }
             }

            private void fillMonth()
             {
                 //Description   : Function for filling tuoReportMasterToMonthDropDownList and reportMasterFromMonthDropDownList with month Name
                 //Author        : Brajesh kumar
                 //Date Created  : 22 April 2011 
                 //Date Updated  : 22 April 2011
                 //Revision No.  : 01
                 //Revision Desc :

                 tuoReportMasterToMonthDropDownList.Items.Clear();
                 tuoReportMasterToMonthDropDownList.Items.Add("");


                 foreach (string item in myWebService.monthName)
                 {
                     tuoReportMasterToMonthDropDownList.Items.Add(item);
                 }
             }

            private void fillYear()
        {
            //Description   : Function for filling reportMasterToYearWithMonthDropDownList and reportMasterFromYearWithMonthDropDownList and tuoReportMasterFromYearDropDownList with year Name
            //Author        : Brajesh kumar
            //Date Created  : 22 April 2011 
            //Date Updated  : 22 April 2011
            //Revision No.  : 01
            //Revision Desc :

            tuoReportMasterToYearWithMonthDropDownList.Items.Clear();
            tuoReportMasterToYearWithMonthDropDownList.Items.Add("");

            tuoReportMasterFromYearDropDownList.Items.Clear();
            tuoReportMasterFromYearDropDownList.Items.Add("");

            foreach (string item in myWebService.yearName)
            {
                tuoReportMasterToYearWithMonthDropDownList.Items.Add(item);
                tuoReportMasterFromYearDropDownList.Items.Add(item);
            }
        }

            private void retainControlState()
            {
                if (tuoReportMasterDayRadioButton.Checked == true)
                {
                    tuoReportMasterFromDateTextBox.Disabled = false;

                    tuoReportMasterToMonthDropDownList.Enabled = false;
                    tuoReportMasterToYearWithMonthDropDownList.Enabled = false;

                    tuoReportMasterFromYearDropDownList.Enabled = false;
                }
                else if (tuoReportMasterMonthRadioButton.Checked == true)
                {
                    tuoReportMasterFromDateTextBox.Disabled = true;

                    tuoReportMasterToMonthDropDownList.Enabled = true;
                    tuoReportMasterToYearWithMonthDropDownList.Enabled = true;

                    tuoReportMasterFromYearDropDownList.Enabled = false;
                }
                else if (tuoReportMasterYearRadioButton.Checked == true)
                {
                    tuoReportMasterFromDateTextBox.Disabled = true;

                    tuoReportMasterToMonthDropDownList.Enabled = false;
                    tuoReportMasterToYearWithMonthDropDownList.Enabled = false;

                    tuoReportMasterFromYearDropDownList.Enabled = true;
                }
            }

        #endregion            

    }
}
