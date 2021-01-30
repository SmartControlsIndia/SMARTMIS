using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.TUO
{
    public partial class performanceReport2SizeWise : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }
            else
            {

                string[] tempString = magicHidden.Value.Split(new char[] { '?' });
                performanceReport2.ReportDate = reportMasterFromDateTextBox.Text;
                performanceReport3.ReportDate = reportMasterFromDateTextBox.Text;
                reportHeader.ReportDate = reportMasterFromDateTextBox.Text;
                 
                //Compare the hidden field if it contains the query string or not
                performanceReport2.Visiblity = "Block";
                performanceReport3.Visiblity = "None";

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
                            //PerformanceReportMachineWiseRadioButton.Checked = true;
                            
                            // Setting the date from 7:00:00 AM //

                        }
                        else if (rChoice == "1")
                        {
                        }
                        else if (rChoice == "2")
                        {

                        }

                    }
                }
            }
          
        }
        protected void magicButton_Click(object sender, EventArgs e)
        {
            //performanceReport3.Visible = false;

        }
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("MM-dd-yyyy");

            return flag;
        }
      
        protected void PerformanceReportMachineWiseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            performanceReport2.Visiblity = "Block";
            performanceReport3.Visiblity = "None";

        }

        protected void PerformanceReportRecipeWiseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            performanceReport2.Visiblity = "None";
            performanceReport3.Visiblity = "Block";
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {
            performanceReport2.ReportDate = reportMasterFromDateTextBox.Text;
            performanceReport3.ReportDate = reportMasterFromDateTextBox.Text;
            reportHeader.ReportDate = reportMasterFromDateTextBox.Text;
            if (PerformanceReportMachineWiseRadioButton.Checked)
            {
                performanceReport2.Visiblity = "Block";
                performanceReport3.Visiblity = "None";
            }
            else
            {
                performanceReport2.Visiblity = "None";
                performanceReport3.Visiblity = "Block";
            }


        }

      
    }
}
