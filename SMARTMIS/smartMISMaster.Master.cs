using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS
{
    public partial class smartMISMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"].ToString().Trim() == "")
            {
                Response.Redirect("/SmartMIS/Default.aspx", true);
            }
            else
            {
                masterWelcomeLabel.Text = "Welcome, " + Session["userName"];
            }
        }


    }
}
