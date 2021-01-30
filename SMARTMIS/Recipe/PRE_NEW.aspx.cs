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

public partial class PRE_NEW : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnnew_Click1(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        try
        {
            con.Open();
            if (Txpressname.Text != "")
            {
                OleDbCommand cmdr = new OleDbCommand("select pressmake from tblpressmake where PRESSMAKE='" + Txpressname.Text + "' ", con);

                OleDbDataReader dr = cmdr.ExecuteReader();
                if (dr.Read())
                {

                    Label19.ForeColor = System.Drawing.Color.Red;
                    Label19.Text = "PRESS NAME ALLREADY EXIST";

                }
                else
                {
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO tblPressMake(PRESSMAKE) VALUES ('" + Txpressname.Text + "')", con);


                    cmd.ExecuteNonQuery();


                    Label19.ForeColor = System.Drawing.Color.Green;
                    Label19.Text = "PRESS NAME " + Txpressname.Text + " SAVED SUCCESSFULLY ";
                }
                dr.Close();
                con.Close();
            }
            else
            {
                Label19.ForeColor = System.Drawing.Color.Red;
                Label19.Text = "INVAILD PRESS NAME";
            }
            
            con.Close();
            con.Dispose();
        }
        catch (Exception ex)
        {
            Label19.ForeColor = System.Drawing.Color.Red;
            Label19.Text = ex.Message;
        }
    }
}
