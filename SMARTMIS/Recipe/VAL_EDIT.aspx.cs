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

public partial class VAL_EDIT : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select valvename from valves", con);
            txtvalve.Items.Add("");
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                txtvalve.Items.Add(dr[0].ToString());

            }
            dr.Close();
            con.Close();
            con.Dispose();
            Label19.Text = "";
            Button1.Enabled = false;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
       try
        {
            con.Open();

           if(txtvalve.SelectedValue!="")
           {
            OleDbCommand cmd = new OleDbCommand("UPDATE VALVES SET VALVENAME='" + TextBox1.Text  + "' WHERE VALVENAME = '" + txtvalve.SelectedValue + "'", con);


            cmd.ExecuteNonQuery();

            
            Label19.ForeColor = System.Drawing.Color.Green;
            Label19.Text = "VALVE NAME EDIT SUCCESSFULLY ";
           }
           else
           {
               Label19.ForeColor = System.Drawing.Color.Red;
               Label19.Text = "YOU ARE TRYING TO EDIT INVAILD DATA ";
           }
           con.Close();
           con.Dispose();
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
    protected void txtvalve_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label19.Text = "";
        Button1.Enabled = true;
    }
}
