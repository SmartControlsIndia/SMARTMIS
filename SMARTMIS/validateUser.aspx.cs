using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS
{
    public partial class validateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[0] == "1")
            {
                Session.Abandon();
                if (Request.QueryString.Count > 1)
                    Response.Redirect("/SMARTMIS/Default.aspx?page=" + Server.UrlEncode(HttpContext.Current.Request.QueryString.ToString().Substring(14)), true);
                else
                    Response.Redirect("/SMARTMIS/Default.aspx", true);
            }
        }
    }
}
