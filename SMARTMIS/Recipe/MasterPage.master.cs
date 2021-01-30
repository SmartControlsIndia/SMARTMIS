using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = " © Smart Controls India Ltd. All rights reserved ";
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
      //  Label1.Text = " © Smart Controls India Ltd. " + DateTime.Now.ToString(string.Format("yyyy")) + " - All rights reserved ";

    }
}
