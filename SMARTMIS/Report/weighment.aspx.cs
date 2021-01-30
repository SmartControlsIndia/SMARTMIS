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

namespace SmartMIS.Report
{
    public partial class weighment : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void magicButton_Click(object sender, EventArgs e)
        {

        }
    }
}
