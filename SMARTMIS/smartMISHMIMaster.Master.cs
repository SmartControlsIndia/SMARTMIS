using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS
{
    public partial class smartMISHMIMaster : System.Web.UI.MasterPage
    {
        myConnection myConnection = new myConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"].ToString().Trim() == "")
            {
                if (hmiMasterManningIDHidden.Value.Trim() == "")
                    Response.Redirect("/SmartMIS/Default.aspx?page=" + Server.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri));
                else
                    Session["userID"] = hmiMasterManningIDHidden.Value;
            }
            else
            {
                masterWelcomeLabel.Text = "Welcome, " + Session["userName"];
                hmiMasterManningIDHidden.Value = Session["userID"].ToString().Trim();
            }
        }


    }
}
