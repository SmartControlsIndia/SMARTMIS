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

public partial class REC_STA_NEW : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        //OleDbConnection con = new OleDbConnection(connString);

        //con.Open();

        //OleDbCommand cmd = new OleDbCommand("select pressmake from tblpressmake", con);

        //OleDbDataReader dr = cmd.ExecuteReader();
        //while (dr.Read())
        //{

        //    txtpressname.Items.Add(dr[0].ToString());

        //}
        //dr.Close();
        
        //con.Close();
        //for (int i = 1; i <= 12; i++)
        //{
        //    DropDownList ctrl = (DropDownList)(this.FindControl("txtvalve" + i));
        //    ctrl.Enabled = false;
            
        //}
        if (!Page.IsPostBack)
        {
            Button1.Enabled = false;
            Label19.Text = "";
            txtvalve.Enabled = false;
            for (int j = 1; j <= 16; j++)
            {
                DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + j));
                ctrl.Items.Clear();
                ctrl.Enabled = false;

            }
            fillPressMakeDropDown();
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

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
        for (int j = m+1; j <= 16; j++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + j));
            ctrl.Items.Clear();
            
        }
        dr1.Close();

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
    }
    protected void txtvalve_SelectedIndexChanged(object sender, EventArgs e)
    {
        Button1.Enabled = true;

        for (int j = 1; j <= 16; j++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + j));
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
        for (int j = m + 1; j <= 16; j++)
        {
            DropDownList ctrl = (DropDownList)(this.FindControl("ctl00$ContentPlaceHolder1$txtvalve" + j));
            ctrl.Items.Clear();
            ctrl.Enabled = false ;

        }
        dr1.Close();

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        Button1.Enabled = true;
        Label19.Text = "";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        try
       {
            con.Open();
            if (txtpressname.Text != "")
            {
                OleDbCommand cmdr = new OleDbCommand("select pressmake from presses where PRESSMAKE ='" + txtpressname.Text + "'", con);

                OleDbDataReader dr = cmdr.ExecuteReader();


                if (dr.Read())
                {

                    Label19.ForeColor = System.Drawing.Color.Red;
                    Label19.Text = "PRESS ALLREADY EXIST ";

                }
                else
                {
                    //OleDbCommand cmd = new OleDbCommand("UPDATE PRESSES SET NOSteps='" + txtstep.SelectedItem + "',NOValves='" + txtvalve.SelectedItem + "',Valve01='" + txtvalve1.SelectedItem + "',Valve02='" + txtvalve2.SelectedItem + "',Valve03='" + txtvalve3.SelectedItem + "',Valve04='" + txtvalve4.SelectedItem + "',Valve05='" + txtvalve5.SelectedItem + "',Valve06='" + txtvalve6.SelectedItem + "',Valve07='" + txtvalve7.SelectedItem + "',Valve08='" + txtvalve8.SelectedItem + "',Valve09='" + txtvalve9.SelectedItem + "',Valve10='" + txtvalve10.SelectedItem + "',Valve11='" + txtvalve11.SelectedItem + "',Valve12='" + txtvalve12.SelectedItem + "' WHERE PRESSMAKE='"+txtpressname.SelectedItem +"'", con);
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO PRESSES VALUES ('" + txtpressname.Text + "','" + txtstep.SelectedItem + "','" + txtvalve.SelectedItem + "','" + txtvalve1.SelectedItem + "','" + txtvalve2.SelectedItem + "','" + txtvalve3.SelectedItem + "','" + txtvalve4.SelectedItem + "','" + txtvalve5.SelectedItem + "','" + txtvalve6.SelectedItem + "','" + txtvalve7.SelectedItem + "','" + txtvalve8.SelectedItem + "','" + txtvalve9.SelectedItem + "','" + txtvalve10.SelectedItem + "','" + txtvalve11.SelectedItem + "','" + txtvalve12.SelectedItem + "','" + txtvalve13.SelectedItem + "','" + txtvalve14.SelectedItem + "','" + txtvalve15.SelectedItem + "','" + txtvalve16.SelectedItem + "')", con);

                    cmd.ExecuteNonQuery();



                    Label19.ForeColor = System.Drawing.Color.Green;
                    Label19.Text = "PRESS " + txtpressname.Text + " SAVED SUCCESSFULLY ";
                }
                dr.Close();
                con.Close();
            }
            else
            {
                Label19.ForeColor = System.Drawing.Color.Red;
                Label19.Text = " INVAILD PRESS  ";
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
    protected void txtstep_SelectedIndexChanged(object sender, EventArgs e)
    {
        Button1.Enabled = true;
        Label19.Text = "";
        txtvalve.Enabled = true;
    }
    public void fillPressMakeDropDown()
    {
        OleDbConnection mycon = new OleDbConnection(connString);
        try{
        
        mycon.Open();
        OleDbCommand cmd1 = new OleDbCommand("Select distinct PressMake from tblPressMake", mycon);
        OleDbDataReader dr1=cmd1.ExecuteReader();
        if(dr1.HasRows)
        {
            while(dr1.Read())
            {
                txtpressname.Items.Add(dr1[0].ToString().Trim());
            }
        }
        dr1.Close();
        if (mycon.State == ConnectionState.Open)
        {
            mycon.Close();
            mycon.Dispose();
        }
        }
        catch(Exception e)
        {
            if (mycon.State == ConnectionState.Open)
            {
                mycon.Close();
                mycon.Dispose();
            }
           
            Label19.Text=e.Message.ToString();
            
        }

    }
}