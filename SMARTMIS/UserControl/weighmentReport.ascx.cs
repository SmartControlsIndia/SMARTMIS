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
using SmartMIS.SmartWebReference;

namespace SmartMIS.UserControl
{
    public partial class weighmentReport : System.Web.UI.UserControl
    {
        smartMISWebService myWebService = new smartMISWebService();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear;
        public string rMatType;

        protected void Page_Load(object sender, EventArgs e)
        {

            string[] tempString = magicHidden.Value.Split(new char[] { '?' });

            weighmentReportGridView.DataSource = null;
            weighmentReportGridView.DataBind();

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
                rMatType = tempString[8];

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
                        rFromDate = formatDate(myWebService.formatDate(rFromDate));
                        string query = myWebService.createQuery(rWCID, rToDate, rFromDate, "dtandTime", "dtandTime");
                        showWeighmentReport(query);
                        magicHidden.Value = "";
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

        private void fillGridView(string query)
        {

            //Description   : Function for filling weighmentReport WorkCenter
            //Author        : Brajesh kumar
            //Date Created  : 16 May 2011
            //Date Updated  : 16 May 2011
            //Revision No.  : 01


            weighmentReportGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            weighmentReportGridView.DataBind();
        }

        protected void showWeighmentReport(string query)
        {
            fillGridView("SELECT name, recipeName, rawMaterialName, batches, setValue, loLimit, hiLimit, average, sigma, threeSigma, sixSigma FROM vMaterialWeighment1 WHERE " + query + " And " + createQuery(rMatType) + "");
        }

        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }

        public string createQuery(String matID)
        {
            string query = "";
            string or = "";
            string[] tempMatID = matID.Split(new char[] { '#' });

            foreach (string items in tempMatID)
            {
                if (items.Trim() != "")
                {
                    query = query + or + "materialTypeID = '" + items + "'";
                    or = " Or ";
                }

            }

            query = "(" + query + ")";

            return query;
        }
    }
}