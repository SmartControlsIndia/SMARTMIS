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

public partial class REC_DELETE : System.Web.UI.Page
{
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select RecCode from recipe", con);
            TxtRecCode.Items.Add("");
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                
                TxtRecCode.Items.Add(dr[0].ToString());

            }
            dr.Close();
            con.Close();
            con.Dispose();
            for (int i = 1; i <= 20; i++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + i));
                TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
                TextBox ctrl2 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                ctrl.Enabled = false;
                ctrl1.Enabled = false;
                ctrl2.Enabled = false;

                for (int j = 1; j <= 16; j++)
                {
                    CheckBox chk = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + j + "chk" + i));
                    chk.Enabled = false;
                }
            }
            Label19.Text = "";
            btndelete.Enabled = false;
        }
    }
    protected void TxtRecCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);

        con.Open();
        int k;
        k = 7;
        int m;
        int mm;
        m = 167;
        mm = 161;
        string str;
        int kkk;
        kkk = 1;

        OleDbCommand cmd = new OleDbCommand("select * from recipe where RecCode='" + TxtRecCode.SelectedItem + "'", con);

        OleDbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            txtpressmake.Text = dr[6].ToString();
            //TxtPreMake.SelectedValue = dr[6].ToString();
            str = dr[6].ToString();
            TxtDiscription.Text = dr[1].ToString();
            TxtCreatedBy.Text = dr[2].ToString();
            TxtCreatedOn.Text = dr[4].ToString();

            for (int a = 1; a <= 10; a++)
            {
                for (int b = 1; b <= 16; b++)
                {
                    CheckBox vchk1 = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + b + "chk" + a));
                    vchk1.Checked = Convert.ToBoolean(Convert.ToInt32(dr[k]));
                    k = k + 1;
                }

            }

            for (int j = 1; j <= 10; j++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + j));
                ctrl.Text = dr[m].ToString();
                m = m + 1;
            }
            for (int l = 1; l <= 10; l++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + l));
                ctrl.Text = dr[m].ToString();
                m = m + 1;
            }
            for (int i = 1; i <= 10; i++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                ctrl.Text = dr[m].ToString();
                m = m + 1;
            }


        }
        dr.Close();

        OleDbCommand cmdr2 = new OleDbCommand("select * from recipe_1 where RecCode='" + TxtRecCode.SelectedItem + "'", con);

        OleDbDataReader drr2 = cmdr2.ExecuteReader();
        while (drr2.Read())
        {
            for (int a = 11; a <= 20; a++)
            {
                for (int b = 1; b <= 16; b++)
                {
                    CheckBox vchk1 = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + b + "chk" + a));
                    vchk1.Checked = Convert.ToBoolean(Convert.ToInt32(drr2[kkk]));
                    kkk = kkk + 1;
                }
            }


            for (int j = 11; j <= 20; j++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + j));
                ctrl.Text = drr2[mm].ToString();
                mm = mm + 1;
            }
            for (int l = 11; l <= 20; l++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + l));
                ctrl.Text = drr2[mm].ToString();
                mm = mm + 1;
            }
            for (int i = 11; i <= 20; i++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                ctrl.Text = drr2[mm].ToString();
                mm = mm + 1;
            }

        }
        drr2.Close();



        ///////////////////
        for (int i = 1; i <= 20; i++)
        {
            TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + i));
            TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
            TextBox ctrl2 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
            ctrl.Enabled = false;

            ctrl1.Enabled = false;

            ctrl2.Enabled = false;


            for (int j = 1; j <= 16; j++)
            {
                CheckBox chk = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + j + "chk" + i));
                chk.Enabled = false;

            }
        }
        int aa;
        int bb;
        int cc;

        bb = 0;
        aa = 3;
        cc = 0;


        OleDbCommand cmd2 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + txtpressmake.Text + "'", con);

        OleDbDataReader dr2 = cmd2.ExecuteReader();


        while (dr2.Read())
        {

            bb = Convert.ToInt16(dr2[1].ToString());
            cc = Convert.ToInt16(dr2[2].ToString());
            for (int i = 1; i <= 16; i++)
            {
                Label ctrl = (Label)(this.FindControl("ctl00$ContentPlaceHolder1$lblValve" + i));
                ctrl.Text = dr2[aa].ToString();
                aa = aa + 1;
            }
            for (int j = 1; j <= bb; j++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + j));
                TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + j));
                TextBox ctrl2 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + j));
                for (int kk = 1; kk <= bb; kk++)
                {
                    for (int l = 1; l <= cc; l++)
                    {
                        CheckBox chk = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + l + "chk" + kk));
                        chk.Enabled = true;
                    }
                }

                ctrl.Enabled = true;
                ctrl1.Enabled = true;
                ctrl2.Enabled = true;

            }

        }

        ////////////////////

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        Label19.Text = "";
       
       
        btndelete.Enabled = true ;
        
    
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        
       if(TxtRecCode.SelectedValue!="")
        {
            con.Open();


            OleDbCommand cmd = new OleDbCommand("DELETE FROM RECIPE WHERE RecCode='" + TxtRecCode.Text + "'", con);
            OleDbCommand cmd1 = new OleDbCommand("DELETE FROM RECIPE_1 WHERE RecCode='" + TxtRecCode.Text + "'", con);

            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
            Label19.ForeColor = System.Drawing.Color.Green;
            Label19.Text = "RECIPE CODE " + TxtRecCode.Text + " DELETE SUCCESSFULLY ";
        }
           else
           {
               Label19.ForeColor = System.Drawing.Color.Red;
               Label19.Text = "YOU ARE TRYING TO DELETE INVAILD DATA ";
           }
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
    protected void Sec_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void PS_CheckedChanged(object sender, EventArgs e)
    {

    }
}
