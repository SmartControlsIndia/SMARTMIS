using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Report
{
    public partial class CuringProductionReport : System.Web.UI.Page
    {
        string queryString = null;
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            queryString = magicHidden.Value;
            string[] tempString2 = queryString.Split(new char[] { '?' });

            reportHeader.ReportDate = tempString2[3].ToString();
        }
    }
}
