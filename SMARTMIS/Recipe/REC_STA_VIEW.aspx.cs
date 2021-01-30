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

public partial class REC_STA_VIEW : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select pressmake from presses", con);

            OleDbDataReader dr = cmd.ExecuteReader();

            txtpressname.Items.Add("");
            while (dr.Read())
            {

                txtpressname.Items.Add(dr[0].ToString());

            }
            dr.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        
    }
    protected void txtvalve_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    
    protected void txtpressname_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstep.Items.Clear();
        txtvalve.Items.Clear();
        for (int a = 1; a <= 16; a++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + a));
            ctrl.Items.Clear();

        }

        OleDbConnection con = new OleDbConnection(connString);
        int m;
        
        m = 3;
        
        con.Open();

        OleDbCommand cmd1 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + txtpressname.Text + "'", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();

        while (dr1.Read())
        {
            txtstep.Items.Add(dr1[1].ToString());
            txtvalve.Items.Add(dr1[2].ToString());

            for (int i = 1; i <= 16; i++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + i));
                ctrl.Items.Add(dr1[m].ToString());
                m = m + 1;
            }

        }
        dr1.Close();
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        
    }
    protected void txtvalve1_SelectedIndexChanged(object sender, EventArgs e)
    {

        for (int a = 1; a <= 16; a++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + a));
            ctrl.Items.Clear();

        }
        OleDbConnection con = new OleDbConnection(connString);
      //  Label19.Text = "aaa";
        con.Open();
        OleDbCommand cmd1 = new OleDbCommand("select valvename from valves ", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {


            txtvalve1.Items.Add(dr1[0].ToString());

        }
        dr1.Close();
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
    }
}
