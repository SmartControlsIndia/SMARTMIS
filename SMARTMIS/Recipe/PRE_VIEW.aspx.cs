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
using System.Data.OleDb;

public partial class PRE_VIEW : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select pressmake from tblpressmake", con);

            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                txtpressname.Items.Add(dr[0].ToString());

            }
            dr.Close();
            con.Close();
        }
    }
    protected void txtpressname_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
