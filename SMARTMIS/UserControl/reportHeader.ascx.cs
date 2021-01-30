using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.UserControl
{
    public partial class reportHeader : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;
        public string _rDate;

        public String ReportDate
        {
            get { return _rDate; }
            set 
            { 
                _rDate = value;
                reportHeaderDateLabel.Text = _rDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
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
                        reportHeaderDateLabel.Text = rToDate;
                    }
                    else if (rChoice == "1")
                    {
                    }
                    else if (rChoice == "2")
                    {
                    }

                }
            }
            else
            {
                reportHeaderDateLabel.Text =ReportDate;
            }
            
            reportHeaderWCName.Text = this.Page.Title;
            
            
        }

        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }
    }
}