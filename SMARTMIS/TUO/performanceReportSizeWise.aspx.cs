using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.TUO
{
    public partial class performanceReportSizeWise : System.Web.UI.Page
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

                //Compare the hidden field if it contains the query string or not

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

        }      
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("MM-dd-yyyy");

            return flag;
        }
    }
}
