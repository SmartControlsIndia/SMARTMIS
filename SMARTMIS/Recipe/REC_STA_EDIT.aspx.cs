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

public partial class REC_STA_EDIT : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select pressmake from presses", con);
            // OleDbCommand cmd1 = new OleDbCommand("select valvename from valves", con);
            OleDbDataReader dr = cmd.ExecuteReader();
            // OleDbDataReader dr1 = cmd1.ExecuteReader();
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
            Label19.Text = "";
            Button1.Enabled = false;
            txtstep.Enabled = false;
            txtvalve.Enabled = false;
          
        }
    }
    protected void txtvalve_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int a = 1; a <= 16; a++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + a));
            ctrl.Items.Clear();

        }
        OleDbConnection con = new OleDbConnection(connString);
        int m;
        m = Convert.ToInt16(txtvalve.Text);

        con.Open();
        OleDbCommand cmd1 = new OleDbCommand("select VALVENAME from VALVES", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {


            for (int i = 1; i <= m; i++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + i));
                ctrl.Items.Add(dr1[0].ToString());
                ctrl.Enabled = true;
            }

        }
        for (int j = m + 1; j <= 12; j++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + j));
            ctrl.Items.Clear();
            ctrl.Enabled = false;

        }
        dr1.Close();

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        Label19.Text = "";
        Button1.Enabled = true;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        try
        {
            con.Open();
            if(txtpressname.SelectedValue!="")
            {

                OleDbCommand cmd = new OleDbCommand("UPDATE PRESSES SET NOSteps='" + txtstep.SelectedItem + "',NOValves='" + txtvalve.SelectedItem + "',Valve01='" + txtvalve1.SelectedItem + "',Valve02='" + txtvalve2.SelectedItem + "',Valve03='" + txtvalve3.SelectedItem + "',Valve04='" + txtvalve4.SelectedItem + "',Valve05='" + txtvalve5.SelectedItem + "',Valve06='" + txtvalve6.SelectedItem + "',Valve07='" + txtvalve7.SelectedItem + "',Valve08='" + txtvalve8.SelectedItem + "',Valve09='" + txtvalve9.SelectedItem + "',Valve10='" + txtvalve10.SelectedItem + "',Valve11='" + txtvalve11.SelectedItem + "',Valve12='" + txtvalve12.SelectedItem + "',Valve13='" + txtvalve13.SelectedItem + "',Valve14='" + txtvalve14.SelectedItem + "',Valve15='" + txtvalve15.SelectedItem + "',Valve16='" + txtvalve16.SelectedItem + "' WHERE PRESSMAKE='" + txtpressname.SelectedItem + "'", con);
           // OleDbCommand cmd = new OleDbCommand("INSERT INTO PRESSES VALUES ('" + txtpressname.Text + "','" + txtstep.SelectedItem + "','" + txtvalve.SelectedItem + "','" + txtvalve1.SelectedItem + "','" + txtvalve2.SelectedItem + "','" + txtvalve3.SelectedItem + "','" + txtvalve4.SelectedItem + "','" + txtvalve5.SelectedItem + "','" + txtvalve6.SelectedItem + "','" + txtvalve7.SelectedItem + "','" + txtvalve8.SelectedItem + "','" + txtvalve9.SelectedItem + "','" + txtvalve10.SelectedItem + "','" + txtvalve11.SelectedItem + "','" + txtvalve12.SelectedItem + "')", con);

            cmd.ExecuteNonQuery();


            con.Close();
            Label19.ForeColor = System.Drawing.Color.Green;
            Label19.Text = "PRESS " + txtpressname.Text + " EDIT SUCCESSFULLY ";
            }
           else
           {
               Label19.ForeColor = System.Drawing.Color.Red;
               Label19.Text = "YOU ARE TRYING TO EDIT INVAILD DATA ";
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
    protected void txtpressname_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int a = 1; a <= 16; a++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + a));
            ctrl.Items.Clear();

        }
       
        OleDbConnection con = new OleDbConnection(connString);
        int m;
        con.Open();
        //m = Convert.ToInt16(txtvalve.Text);
        m = 3;
        OleDbCommand cmd = new OleDbCommand("select VALVENAME from VALVES", con);
        OleDbDataReader dr = cmd.ExecuteReader();

        for (int b = 1; b <= 16; b++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + b));
            ctrl.Items.Add("");

        }
        while (dr.Read())
        {
            for (int a = 1; a <= 16; a++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + a));
                ctrl.Items.Add(dr[0].ToString());

            }
        }
        dr.Close();
        

        OleDbCommand cmd1 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + txtpressname.Text  + "'", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();

        while (dr1.Read())
        {

            int x = Convert.ToInt16(dr1[2].ToString());
            txtstep.Text = dr1[1].ToString();
            txtvalve.Text = dr1[2].ToString();
           
         
            for (int i = 1; i <= x; i++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + i));
                ctrl.SelectedValue = dr1[m].ToString();
                ctrl.Enabled = true;
                m = m + 1;
            }

        }
       
        dr1.Close();
        OleDbCommand cmd2 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + txtpressname.Text + "'", con);

        OleDbDataReader dr2 = cmd1.ExecuteReader();

        while (dr2.Read())
        {


            int y;

            y = Convert.ToInt16(dr2[2].ToString());

            for (int i = y+1; i <= 16; i++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + i));
                ctrl.Items.Clear();
                ctrl.Enabled = false;
               
            }

        }

        dr2.Close();
        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        Label19.Text = "";
        Button1.Enabled = true;
        txtstep.Enabled = true;
        txtvalve.Enabled = true;
    }
    protected void txtvalve1_SelectedIndexChanged(object sender, EventArgs e)
    {

        
    }



    protected void txtvalve2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve2.SelectedValue  == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve1_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (txtvalve1.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve3.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve4_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve4.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve5_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve5.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve6_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve6.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve7_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve7.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve8_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve8.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve9_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve9.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve10_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve10.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve11_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve11.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtvalve12_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve12.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }
    }
    protected void txtstep_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label19.Text = "";
       
    }
    protected void txtvalve13_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve13.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }

    }
    protected void txtvalve14_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve14.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }

    }
    protected void txtvalve15_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve15.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }

    }
    protected void txtvalve16_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtvalve16.SelectedValue == "")
        {
            Label19.Text = "BLANK IS NOT A VAILD FIELD";
            Button1.Enabled = false;
        }
        else
        {
            Label19.Text = "";
            Button1.Enabled = true;
        }

    }
}

