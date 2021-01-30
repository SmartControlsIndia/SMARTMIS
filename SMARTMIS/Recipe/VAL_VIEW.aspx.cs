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

public partial class VAL_VIEW : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnnew_Click1(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        try
        {
            con.Open();

            if (Txvalvesname.Text != "")
            {
                OleDbCommand cmdR = new OleDbCommand("select VALVENAME from VALVES where valvename='" + Txvalvesname.Text  + "'", con);

                OleDbDataReader dr = cmdR.ExecuteReader();
                if (dr.Read())
                {

                    Label19.ForeColor = System.Drawing.Color.Red;
                    Label19.Text = "VALVE NAME ALLREADY EXIST";

                }
                else
                {
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO valves(valvename) VALUES ('" + Txvalvesname.Text + "')", con);


                    cmd.ExecuteNonQuery();


                    Label19.ForeColor = System.Drawing.Color.Green;
                    Label19.Text = "VALVE NAME " + Txvalvesname.Text + " SAVED SUCCESSFULLY ";
                }
                dr.Close();
        //        con.Close();
            }
            else
            {
                Label19.ForeColor = System.Drawing.Color.Red;
                Label19.Text = "INVAILD VALVE NAME";
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
            Label19.ForeColor = System.Drawing.Color.Red;
            Label19.Text = ex.Message;
        }
    }
}
