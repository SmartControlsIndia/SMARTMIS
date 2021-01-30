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
    public partial class smartMISCuringReportMaster : System.Web.UI.MasterPage
    {

        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string whereQuery;

        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {

                retainControlState();

                if (!Page.IsPostBack)
                {
                    fillProcessDropDownList();
                    fillReportType();
                    fillContentGridView(Page.Title.Trim());
                    fillDate();
                    
                    reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    reportMasterToDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                if (((DropDownList)sender).ID == "reportMasterReportTypeDropDownList")
                {
                    if (((DropDownList)sender).SelectedIndex == 0)
                    {
                        //reportMasterWCDiv.Visible = false;
                    }
                    else if (((DropDownList)sender).SelectedIndex == 1)
                    {
                        //reportMasterWCDiv.Visible = true;
                    }
                }
                if (((DropDownList)sender).ID == "reportMasterWCProcessDropDownList")
                {
                    fillWCGridView(Page.Title.Trim(), ((DropDownList)sender).SelectedItem.ToString());
                    reportMasterWCAllCheckBox.Checked = false;

                    if (reportMasterWCProcessDropDownList.SelectedValue == "Curing PCR")
                        whereQuery = "Where processID=8";  //Curing PCR
                    else if (reportMasterWCProcessDropDownList.SelectedValue == "Curing TBR")
                        whereQuery = "Where processID<=5";  //Curing TBR

                    fillSizedropdownlist();
                    fillOperatordropdownlist();

                }
            }

        #endregion

        #region User Defined Function

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
            private void fillSizedropdownlist()
            {
                CuringProductionReportSizeDropdownlist.Items.Clear();
                CuringProductionReportSizeDropdownlist.DataSource = null;
                CuringProductionReportSizeDropdownlist.DataSource = FillDropDownList("vCuringProduction " + whereQuery, "description");
                CuringProductionReportSizeDropdownlist.DataBind();
            }
            private void fillOperatordropdownlist()
            {

                CuringProductionReportOperatorDropdownlist.Items.Clear();

                string sqlQuery = "";

                ListItem litem = new ListItem("All", "All");
                CuringProductionReportOperatorDropdownlist.Items.Add(litem);

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    sqlQuery = "Select DISTINCT manningID, firstName, lastName from vCuringProduction " + whereQuery;

                    myConnection.comm.CommandText = sqlQuery;

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        if (myConnection.reader[0].ToString() != "")
                        {
                            ListItem li = new ListItem(myConnection.reader[1].ToString() + " " + myConnection.reader[2].ToString(), myConnection.reader[0].ToString());
                            CuringProductionReportOperatorDropdownlist.Items.Add(li);
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
                reportMasterWCProcessDropDownList.DataSource = myWebService.FillDropDownList("processMaster", "name", "where iD in (5,8) ");
                reportMasterWCProcessDropDownList.DataBind();

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
                    //reportMasterWCDiv.Visible = true;

                    reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster WHERE processID = 28", ConnectionOption.SQL);
                    reportMasterWCGridView.DataBind();
                }
                else if(option == "Downtime Report")
                {
                    //reportMasterReportTypeDropDownList.SelectedIndex = 1;
                    //reportMasterReportTypeDropDownList.Style.Add(HtmlTextWriterStyle.Display, "none");

                    reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster", ConnectionOption.SQL);
                    reportMasterWCGridView.DataBind();

                    //reportMasterWCDiv.Visible = true;
                }
                else
                {
                    reportMasterWCGridView.DataSource = myWebService.fillGridView("Select iD, workCenterName as name from vWorkCenter WHERE processName = '" + processName + "'", ConnectionOption.SQL);
                    reportMasterWCGridView.DataBind();
                }
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

            private void fillDate()
            {

                //Description   : Function for filling reportMasterFromDateTextBox and reportMasterToDateTextBox with Default Values
                //Author        : Brajesh kumar
                //Date Created  : 02 May 2011
                //Date Updated  : 02 May 2011
                //Revision No.  : 01
                //Revision Desc :

            }

            private void fillReportType()
             {
                 //Description   : Function for filling reportMasterReportTypeDropDownList with Report Type
                 //Author        : Brajesh kumar
                 //Date Created  : 22 April 2011
                 //Date Updated  : 22 April 2011
                 //Revision No.  : 01
                 //Revision Desc :

                 //reportMasterReportTypeDropDownList.Items.Clear();

                 //foreach (string item in myWebService.reportType)
                 //{
                 //    reportMasterReportTypeDropDownList.Items.Add(item);
                 //}
             }

            private void retainControlState()
            {
                
            }

        #endregion            

    }
}
