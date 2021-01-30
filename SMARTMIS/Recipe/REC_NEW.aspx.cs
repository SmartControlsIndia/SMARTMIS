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


public partial class REC_NEW : System.Web.UI.Page
{    
    string connString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {

        System.IO.StreamReader sr = new System.IO.StreamReader("C:/Program Files/Smart/SmartSCADA/UserLevel.txt");
        string line;
        int sublen;
        string substring;

        while (sr.Peek() != -1)
        {
            line = sr.ReadLine();
            substring = Server.HtmlEncode(line);
            sublen = substring.Length;
            sublen = sublen - 8;
            TxtCreatedBy.Text = substring.Substring(2, sublen);

        }
        sr.Close();
        sr.Dispose();
        //}
        // mohan start---------------------------------------------------------

        if (!Page.IsPostBack)
        {

            TxtPreMake.Items.Clear();
            TxtCreatedOn.Text = DateTime.Now.ToShortDateString();
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();
            OleDbCommand cmd1 = new OleDbCommand("select RecCode from recipe", con);
            TxtRecCode1.Items.Add("");
            OleDbDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {

                TxtRecCode1.Items.Add(dr1[0].ToString());

            }
            dr1.Close();

            OleDbCommand cmd = new OleDbCommand("select pressmake from presses", con);
            TxtPreMake.Items.Add("");
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                TxtPreMake.Items.Add(dr[0].ToString());

            }
            dr.Close();
            con.Close();
            Label19.Text = "";
            Button1.Enabled = false;




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

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
    // mohan end===============================
    
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
        try
        {
            con.Open();
            string a;
            string b;
            a = "none";
            b = "none";
             
              if(TxtRecCode.Text != "")
                {
            OleDbCommand cmdR = new OleDbCommand("select RecCode from recipe where RecCode='" + TxtRecCode.Text + "'", con);
            
            OleDbDataReader dr = cmdR.ExecuteReader();
            if (dr.Read())
            {
              Label19.ForeColor = System.Drawing.Color.Red;
              Label19.Text = "RECIPE CODE ALLREADY EXIST ";   
            }
            else
            {
                  OleDbCommand cmd = new OleDbCommand("INSERT INTO RECIPE VALUES ('" + TxtRecCode.Text + "','" + TxtDiscription.Text + "','" + TxtCreatedBy.Text + "','" + a + "','" + TxtCreatedOn.Text + "','" + b + "','" + TxtPreMake.Text + "','" + Convert.ToInt32(val1chk1.Checked) + "','" + Convert.ToInt32(val2chk1.Checked) + "','" + Convert.ToInt32(val3chk1.Checked) + "','" + Convert.ToInt32(val4chk1.Checked) + "','" + Convert.ToInt32(val5chk1.Checked) + "','" + Convert.ToInt32(val6chk1.Checked) + "','" + Convert.ToInt32(val7chk1.Checked) + "','" + Convert.ToInt32(val8chk1.Checked) + "','" + Convert.ToInt32(val9chk1.Checked) + "','" + Convert.ToInt32(val10chk1.Checked) + "','" + Convert.ToInt32(val11chk1.Checked) + "','" + Convert.ToInt32(val12chk1.Checked) + "','" + Convert.ToInt32(val13chk1.Checked) + "','" + Convert.ToInt32(val14chk1.Checked) + "','" + Convert.ToInt32(val15chk1.Checked) + "','" + Convert.ToInt32(val16chk1.Checked) + "','" + Convert.ToInt32(val1chk2.Checked) + "','" + Convert.ToInt32(val2chk2.Checked) + "','" + Convert.ToInt32(val3chk2.Checked) + "','" + Convert.ToInt32(val4chk2.Checked) + "','" + Convert.ToInt32(val5chk2.Checked) + "','" + Convert.ToInt32(val6chk2.Checked) + "','" + Convert.ToInt32(val7chk2.Checked) + "','" + Convert.ToInt32(val8chk2.Checked) + "','" + Convert.ToInt32(val9chk2.Checked) + "','" + Convert.ToInt32(val10chk2.Checked) + "','" + Convert.ToInt32(val11chk2.Checked) + "','" + Convert.ToInt32(val12chk2.Checked) + "','" + Convert.ToInt32(val13chk2.Checked) + "','" + Convert.ToInt32(val14chk2.Checked) + "','" + Convert.ToInt32(val15chk2.Checked) + "','" + Convert.ToInt32(val16chk2.Checked) + "','" + Convert.ToInt32(val1chk3.Checked) + "','" + Convert.ToInt32(val2chk3.Checked) + "','" + Convert.ToInt32(val3chk3.Checked) + "','" + Convert.ToInt32(val4chk3.Checked) + "','" + Convert.ToInt32(val5chk3.Checked) + "','" + Convert.ToInt32(val6chk3.Checked) + "','" + Convert.ToInt32(val7chk3.Checked) + "','" + Convert.ToInt32(val8chk3.Checked) + "','" + Convert.ToInt32(val9chk3.Checked) + "','" + Convert.ToInt32(val10chk3.Checked) + "','" + Convert.ToInt32(val11chk3.Checked) + "','" + Convert.ToInt32(val12chk3.Checked) + "','" + Convert.ToInt32(val13chk3.Checked) + "','" + Convert.ToInt32(val14chk3.Checked) + "','" + Convert.ToInt32(val15chk3.Checked) + "','" + Convert.ToInt32(val16chk3.Checked) + "','" + Convert.ToInt32(val1chk4.Checked) + "','" + Convert.ToInt32(val2chk4.Checked) + "','" + Convert.ToInt32(val3chk4.Checked) + "','" + Convert.ToInt32(val4chk4.Checked) + "','" + Convert.ToInt32(val5chk4.Checked) + "','" + Convert.ToInt32(val6chk4.Checked) + "','" + Convert.ToInt32(val7chk4.Checked) + "','" + Convert.ToInt32(val8chk4.Checked) + "','" + Convert.ToInt32(val9chk4.Checked) + "','" + Convert.ToInt32(val10chk4.Checked) + "','" + Convert.ToInt32(val11chk4.Checked) + "','" + Convert.ToInt32(val12chk4.Checked) + "','" + Convert.ToInt32(val13chk4.Checked) + "','" + Convert.ToInt32(val14chk4.Checked) + "','" + Convert.ToInt32(val15chk4.Checked) + "','" + Convert.ToInt32(val16chk4.Checked) + "','" + Convert.ToInt32(val1chk5.Checked) + "','" + Convert.ToInt32(val2chk5.Checked) + "','" + Convert.ToInt32(val3chk5.Checked) + "','" + Convert.ToInt32(val4chk5.Checked) + "','" + Convert.ToInt32(val5chk5.Checked) + "','" + Convert.ToInt32(val6chk5.Checked) + "','" + Convert.ToInt32(val7chk5.Checked) + "','" + Convert.ToInt32(val8chk5.Checked) + "','" + Convert.ToInt32(val9chk5.Checked) + "','" + Convert.ToInt32(val10chk5.Checked) + "','" + Convert.ToInt32(val11chk5.Checked) + "','" + Convert.ToInt32(val12chk5.Checked) + "','" + Convert.ToInt32(val13chk5.Checked) + "','" + Convert.ToInt32(val14chk5.Checked) + "','" + Convert.ToInt32(val15chk5.Checked) + "','" + Convert.ToInt32(val16chk5.Checked) + "','" + Convert.ToInt32(val1chk6.Checked) + "','" + Convert.ToInt32(val2chk6.Checked) + "','" + Convert.ToInt32(val3chk6.Checked) + "','" + Convert.ToInt32(val4chk6.Checked) + "','" + Convert.ToInt32(val5chk6.Checked) + "','" + Convert.ToInt32(val6chk6.Checked) + "','" + Convert.ToInt32(val7chk6.Checked) + "','" + Convert.ToInt32(val8chk6.Checked) + "','" + Convert.ToInt32(val9chk6.Checked) + "','" + Convert.ToInt32(val10chk6.Checked) + "','" + Convert.ToInt32(val11chk6.Checked) + "','" + Convert.ToInt32(val12chk6.Checked) + "','" + Convert.ToInt32(val13chk6.Checked) + "','" + Convert.ToInt32(val14chk6.Checked) + "','" + Convert.ToInt32(val15chk6.Checked) + "','" + Convert.ToInt32(val16chk6.Checked) + "','" + Convert.ToInt32(val1chk7.Checked) + "','" + Convert.ToInt32(val2chk7.Checked) + "','" + Convert.ToInt32(val3chk7.Checked) + "','" + Convert.ToInt32(val4chk7.Checked) + "','" + Convert.ToInt32(val5chk7.Checked) + "','" + Convert.ToInt32(val6chk7.Checked) + "','" + Convert.ToInt32(val7chk7.Checked) + "','" + Convert.ToInt32(val8chk7.Checked) + "','" + Convert.ToInt32(val9chk7.Checked) + "','" + Convert.ToInt32(val10chk7.Checked) + "','" + Convert.ToInt32(val11chk7.Checked) + "','" + Convert.ToInt32(val12chk7.Checked) + "','" + Convert.ToInt32(val13chk7.Checked) + "','" + Convert.ToInt32(val14chk7.Checked) + "','" + Convert.ToInt32(val15chk7.Checked) + "','" + Convert.ToInt32(val16chk7.Checked) + "','" + Convert.ToInt32(val1chk8.Checked) + "','" + Convert.ToInt32(val2chk8.Checked) + "','" + Convert.ToInt32(val3chk8.Checked) + "','" + Convert.ToInt32(val4chk8.Checked) + "','" + Convert.ToInt32(val5chk8.Checked) + "','" + Convert.ToInt32(val6chk8.Checked) + "','" + Convert.ToInt32(val7chk8.Checked) + "','" + Convert.ToInt32(val8chk8.Checked) + "','" + Convert.ToInt32(val9chk8.Checked) + "','" + Convert.ToInt32(val10chk8.Checked) + "','" + Convert.ToInt32(val11chk8.Checked) + "','" + Convert.ToInt32(val12chk8.Checked) + "','" + Convert.ToInt32(val13chk8.Checked) + "','" + Convert.ToInt32(val14chk8.Checked) + "','" + Convert.ToInt32(val15chk8.Checked) + "','" + Convert.ToInt32(val16chk8.Checked) + "','" + Convert.ToInt32(val1chk9.Checked) + "','" + Convert.ToInt32(val2chk9.Checked) + "','" + Convert.ToInt32(val3chk9.Checked) + "','" + Convert.ToInt32(val4chk9.Checked) + "','" + Convert.ToInt32(val5chk9.Checked) + "','" + Convert.ToInt32(val6chk9.Checked) + "','" + Convert.ToInt32(val7chk9.Checked) + "','" + Convert.ToInt32(val8chk9.Checked) + "','" + Convert.ToInt32(val9chk9.Checked) + "','" + Convert.ToInt32(val10chk9.Checked) + "','" + Convert.ToInt32(val11chk9.Checked) + "','" + Convert.ToInt32(val12chk9.Checked) + "','" + Convert.ToInt32(val13chk9.Checked) + "','" + Convert.ToInt32(val14chk9.Checked) + "','" + Convert.ToInt32(val15chk9.Checked) + "','" + Convert.ToInt32(val16chk9.Checked) + "','" + Convert.ToInt32(val1chk10.Checked) + "','" + Convert.ToInt32(val2chk10.Checked) + "','" + Convert.ToInt32(val3chk10.Checked) + "','" + Convert.ToInt32(val4chk10.Checked) + "','" + Convert.ToInt32(val5chk10.Checked) + "','" + Convert.ToInt32(val6chk10.Checked) + "','" + Convert.ToInt32(val7chk10.Checked) + "','" + Convert.ToInt32(val8chk10.Checked) + "','" + Convert.ToInt32(val9chk10.Checked) + "','" + Convert.ToInt32(val10chk10.Checked) + "','" + Convert.ToInt32(val11chk10.Checked) + "','" + Convert.ToInt32(val12chk10.Checked) + "','" + Convert.ToInt32(val13chk10.Checked) + "','" + Convert.ToInt32(val14chk10.Checked) + "','" + Convert.ToInt32(val15chk10.Checked) + "','" + Convert.ToInt32(val16chk10.Checked) + "','" + min1.Text + "','" + min2.Text + "','" + min3.Text + "','" + min4.Text + "','" + min5.Text + "','" + min6.Text + "','" + min7.Text + "','" + min8.Text + "','" + min9.Text + "','" + min10.Text + "','" + sec1.Text + "','" + sec2.Text + "','" + sec3.Text + "','" + sec4.Text + "','" + sec5.Text + "','" + sec6.Text + "','" + sec7.Text + "','" + sec8.Text + "','" + sec9.Text + "','" + sec10.Text + "','" + ps1.Text + "','" + ps2.Text + "','" + ps3.Text + "','" + ps4.Text + "','" + ps5.Text + "','" + ps6.Text + "','" + ps7.Text + "','" + ps8.Text + "','" + ps9.Text + "','" + ps10.Text + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "')", con);
            OleDbCommand cmd1 = new OleDbCommand("INSERT INTO RECIPE_1 VALUES ('" + TxtRecCode.Text + "','" + Convert.ToInt32(val1chk11.Checked) + "','" + Convert.ToInt32(val2chk11.Checked) + "','" + Convert.ToInt32(val3chk11.Checked) + "','" + Convert.ToInt32(val4chk11.Checked) + "','" + Convert.ToInt32(val5chk11.Checked) + "','" + Convert.ToInt32(val6chk11.Checked) + "','" + Convert.ToInt32(val7chk11.Checked) + "','" + Convert.ToInt32(val8chk11.Checked) + "','" + Convert.ToInt32(val9chk11.Checked) + "','" + Convert.ToInt32(val10chk11.Checked) + "','" + Convert.ToInt32(val11chk11.Checked) + "','" + Convert.ToInt32(val12chk11.Checked) + "','" + Convert.ToInt32(val13chk11.Checked) + "','" + Convert.ToInt32(val14chk11.Checked) + "','" + Convert.ToInt32(val15chk11.Checked) + "','" + Convert.ToInt32(val16chk11.Checked) + "','" + Convert.ToInt32(val1chk12.Checked) + "','" + Convert.ToInt32(val2chk12.Checked) + "','" + Convert.ToInt32(val3chk12.Checked) + "','" + Convert.ToInt32(val4chk12.Checked) + "','" + Convert.ToInt32(val5chk12.Checked) + "','" + Convert.ToInt32(val6chk12.Checked) + "','" + Convert.ToInt32(val7chk12.Checked) + "','" + Convert.ToInt32(val8chk12.Checked) + "','" + Convert.ToInt32(val9chk12.Checked) + "','" + Convert.ToInt32(val10chk12.Checked) + "','" + Convert.ToInt32(val11chk12.Checked) + "','" + Convert.ToInt32(val12chk12.Checked) + "','" + Convert.ToInt32(val13chk12.Checked) + "','" + Convert.ToInt32(val14chk12.Checked) + "','" + Convert.ToInt32(val15chk12.Checked) + "','" + Convert.ToInt32(val16chk12.Checked) + "','" + Convert.ToInt32(val1chk13.Checked) + "','" + Convert.ToInt32(val2chk13.Checked) + "','" + Convert.ToInt32(val3chk13.Checked) + "','" + Convert.ToInt32(val4chk13.Checked) + "','" + Convert.ToInt32(val5chk13.Checked) + "','" + Convert.ToInt32(val6chk13.Checked) + "','" + Convert.ToInt32(val7chk13.Checked) + "','" + Convert.ToInt32(val8chk13.Checked) + "','" + Convert.ToInt32(val9chk13.Checked) + "','" + Convert.ToInt32(val10chk13.Checked) + "','" + Convert.ToInt32(val11chk13.Checked) + "','" + Convert.ToInt32(val12chk13.Checked) + "','" + Convert.ToInt32(val13chk13.Checked) + "','" + Convert.ToInt32(val14chk13.Checked) + "','" + Convert.ToInt32(val15chk13.Checked) + "','" + Convert.ToInt32(val16chk13.Checked) + "','" + Convert.ToInt32(val1chk14.Checked) + "','" + Convert.ToInt32(val2chk14.Checked) + "','" + Convert.ToInt32(val3chk14.Checked) + "','" + Convert.ToInt32(val4chk14.Checked) + "','" + Convert.ToInt32(val5chk14.Checked) + "','" + Convert.ToInt32(val6chk14.Checked) + "','" + Convert.ToInt32(val7chk14.Checked) + "','" + Convert.ToInt32(val8chk14.Checked) + "','" + Convert.ToInt32(val9chk14.Checked) + "','" + Convert.ToInt32(val10chk14.Checked) + "','" + Convert.ToInt32(val11chk14.Checked) + "','" + Convert.ToInt32(val12chk14.Checked) + "','" + Convert.ToInt32(val13chk14.Checked) + "','" + Convert.ToInt32(val14chk14.Checked) + "','" + Convert.ToInt32(val15chk14.Checked) + "','" + Convert.ToInt32(val16chk14.Checked) + "','" + Convert.ToInt32(val1chk15.Checked) + "','" + Convert.ToInt32(val2chk15.Checked) + "','" + Convert.ToInt32(val3chk15.Checked) + "','" + Convert.ToInt32(val4chk15.Checked) + "','" + Convert.ToInt32(val5chk15.Checked) + "','" + Convert.ToInt32(val6chk15.Checked) + "','" + Convert.ToInt32(val7chk15.Checked) + "','" + Convert.ToInt32(val8chk15.Checked) + "','" + Convert.ToInt32(val9chk15.Checked) + "','" + Convert.ToInt32(val10chk15.Checked) + "','" + Convert.ToInt32(val11chk15.Checked) + "','" + Convert.ToInt32(val12chk15.Checked) + "','" + Convert.ToInt32(val13chk15.Checked) + "','" + Convert.ToInt32(val14chk15.Checked) + "','" + Convert.ToInt32(val15chk15.Checked) + "','" + Convert.ToInt32(val16chk15.Checked) + "','" + Convert.ToInt32(val1chk16.Checked) + "','" + Convert.ToInt32(val2chk16.Checked) + "','" + Convert.ToInt32(val3chk16.Checked) + "','" + Convert.ToInt32(val4chk16.Checked) + "','" + Convert.ToInt32(val5chk16.Checked) + "','" + Convert.ToInt32(val6chk16.Checked) + "','" + Convert.ToInt32(val7chk16.Checked) + "','" + Convert.ToInt32(val8chk16.Checked) + "','" + Convert.ToInt32(val9chk16.Checked) + "','" + Convert.ToInt32(val10chk16.Checked) + "','" + Convert.ToInt32(val11chk16.Checked) + "','" + Convert.ToInt32(val12chk16.Checked) + "','" + Convert.ToInt32(val13chk16.Checked) + "','" + Convert.ToInt32(val14chk16.Checked) + "','" + Convert.ToInt32(val15chk16.Checked) + "','" + Convert.ToInt32(val16chk16.Checked) + "','" + Convert.ToInt32(val1chk17.Checked) + "','" + Convert.ToInt32(val2chk17.Checked) + "','" + Convert.ToInt32(val3chk17.Checked) + "','" + Convert.ToInt32(val4chk17.Checked) + "','" + Convert.ToInt32(val5chk17.Checked) + "','" + Convert.ToInt32(val6chk17.Checked) + "','" + Convert.ToInt32(val7chk17.Checked) + "','" + Convert.ToInt32(val8chk17.Checked) + "','" + Convert.ToInt32(val9chk17.Checked) + "','" + Convert.ToInt32(val10chk17.Checked) + "','" + Convert.ToInt32(val11chk17.Checked) + "','" + Convert.ToInt32(val12chk17.Checked) + "','" + Convert.ToInt32(val13chk17.Checked) + "','" + Convert.ToInt32(val14chk17.Checked) + "','" + Convert.ToInt32(val15chk17.Checked) + "','" + Convert.ToInt32(val16chk17.Checked) + "','" + Convert.ToInt32(val1chk18.Checked) + "','" + Convert.ToInt32(val2chk18.Checked) + "','" + Convert.ToInt32(val3chk18.Checked) + "','" + Convert.ToInt32(val4chk18.Checked) + "','" + Convert.ToInt32(val5chk18.Checked) + "','" + Convert.ToInt32(val6chk18.Checked) + "','" + Convert.ToInt32(val7chk18.Checked) + "','" + Convert.ToInt32(val8chk18.Checked) + "','" + Convert.ToInt32(val9chk18.Checked) + "','" + Convert.ToInt32(val10chk18.Checked) + "','" + Convert.ToInt32(val11chk18.Checked) + "','" + Convert.ToInt32(val12chk18.Checked) + "','" + Convert.ToInt32(val13chk18.Checked) + "','" + Convert.ToInt32(val14chk18.Checked) + "','" + Convert.ToInt32(val15chk18.Checked) + "','" + Convert.ToInt32(val16chk18.Checked) + "','" + Convert.ToInt32(val1chk19.Checked) + "','" + Convert.ToInt32(val2chk19.Checked) + "','" + Convert.ToInt32(val3chk19.Checked) + "','" + Convert.ToInt32(val4chk19.Checked) + "','" + Convert.ToInt32(val5chk19.Checked) + "','" + Convert.ToInt32(val6chk19.Checked) + "','" + Convert.ToInt32(val7chk19.Checked) + "','" + Convert.ToInt32(val8chk19.Checked) + "','" + Convert.ToInt32(val9chk19.Checked) + "','" + Convert.ToInt32(val10chk19.Checked) + "','" + Convert.ToInt32(val11chk19.Checked) + "','" + Convert.ToInt32(val12chk19.Checked) + "','" + Convert.ToInt32(val13chk19.Checked) + "','" + Convert.ToInt32(val14chk19.Checked) + "','" + Convert.ToInt32(val15chk19.Checked) + "','" + Convert.ToInt32(val16chk19.Checked) + "','" + Convert.ToInt32(val1chk20.Checked) + "','" + Convert.ToInt32(val2chk20.Checked) + "','" + Convert.ToInt32(val3chk20.Checked) + "','" + Convert.ToInt32(val4chk20.Checked) + "','" + Convert.ToInt32(val5chk20.Checked) + "','" + Convert.ToInt32(val6chk20.Checked) + "','" + Convert.ToInt32(val7chk20.Checked) + "','" + Convert.ToInt32(val8chk20.Checked) + "','" + Convert.ToInt32(val9chk20.Checked) + "','" + Convert.ToInt32(val10chk20.Checked) + "','" + Convert.ToInt32(val11chk20.Checked) + "','" + Convert.ToInt32(val12chk20.Checked) + "','" + Convert.ToInt32(val13chk20.Checked) + "','" + Convert.ToInt32(val14chk20.Checked) + "','" + Convert.ToInt32(val15chk20.Checked) + "','" + Convert.ToInt32(val16chk20.Checked) + "','" + min11.Text + "','" + min12.Text + "','" + min13.Text + "','" + min14.Text + "','" + min15.Text + "','" + min16.Text + "','" + min17.Text + "','" + min18.Text + "','" + min19.Text + "','" + min20.Text + "','" + sec11.Text + "','" + sec12.Text + "','" + sec13.Text + "','" + sec14.Text + "','" + sec15.Text + "','" + sec16.Text + "','" + sec17.Text + "','" + sec18.Text + "','" + sec19.Text + "','" + sec20.Text + "','" + ps11.Text + "','" + ps12.Text + "','" + ps13.Text + "','" + ps14.Text + "','" + ps15.Text + "','" + ps16.Text + "','" + ps17.Text + "','" + ps18.Text + "','" + ps19.Text + "','" + ps20.Text + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "','" + a + "')", con);

            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();

            
            Label19.ForeColor = System.Drawing.Color.Green;
            Label19.Text = "RECIPE CODE " + TxtRecCode.Text + " SAVED SUCCESSFULLY ";

            }
            dr.Close();
            con.Close();
          }
           else
               {
                      Label19.ForeColor = System.Drawing.Color.Red;
                      Label19.Text = " INVAILD RECIPE CODE "; 
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
    protected void TxtRecCode_TextChanged(object sender, EventArgs e)
    {

    }
    protected void TxtPreMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 1; i <= 20; i++)
        {
            TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + i));
            TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
            TextBox ctrl2 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
            ctrl.Enabled = false;
            ctrl.Text = "";
            ctrl1.Enabled = false;
            ctrl1.Text = "";
            ctrl2.Enabled = false;
            ctrl2.Text = "";

            for (int j = 1; j <= 16; j++)
            {
                CheckBox chk = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + j + "chk" + i));
                chk.Enabled = false;
                chk.Checked = false;
            }
        }

       
        int a;
        int b;
        int c;

        b = 0;
        a = 3;
        c = 0;
       
        OleDbConnection con = new OleDbConnection(connString);
        
        con.Open();

        OleDbCommand cmd1 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + TxtPreMake.SelectedItem + "'", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();
       

        while (dr1.Read())
        {
           
            b = Convert.ToInt16(dr1[1].ToString());
            c = Convert.ToInt16(dr1[2].ToString());
            for (int i = 1; i <= 16; i++)
            {
                Label ctrl = (Label)(this.FindControl("ctl00$ContentPlaceHolder1$lblValve" + i));
              ctrl.Text = dr1[a].ToString();
                a = a + 1;
            }
            for (int j = 1; j <= b; j++)
            {
                TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$min" + j));
                TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + j));
                TextBox ctrl2 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + j));
                for (int k = 1; k <= b; k++)
                {
                    for (int l = 1; l <= c; l++)
                    {
                        CheckBox chk = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + l + "chk" + k));
                        chk.Enabled = true;
                    }
                }

                ctrl.Enabled = true;
                ctrl1.Enabled = true;
                ctrl2.Enabled = true;
                
            }

        }
        Label19.Text = "";
       
        Button1.Enabled = true;

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }
        
    }
    protected void Sec_CheckedChanged(object sender, EventArgs e)
    {
        int a;
        int b;
        int c;

        b = 0;
        a = 3;
        c = 0;

        OleDbConnection con = new OleDbConnection(connString);

        con.Open();

        OleDbCommand cmd1 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + TxtPreMake.SelectedItem + "'", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();


        while (dr1.Read())
        {
            b = Convert.ToInt16(dr1[1].ToString());


            if (Sec.Checked == false)
            {
                for (int i = 1; i <= b; i++)
                {

                    TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
                    ctrl1.Enabled = false;
                    ctrl1.Text = "";


                }
            }
            if (Sec.Checked == true)
            {
                for (int i = 1; i <= b; i++)
                {

                    TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
                    ctrl1.Enabled = true;
                    ctrl1.Text = "";


                }
            }


        }

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }  
    }
    protected void PS_CheckedChanged(object sender, EventArgs e)
    {

        int a;
        int b;
        int c;

        b = 0;
        a = 3;
        c = 0;

        OleDbConnection con = new OleDbConnection(connString);

        con.Open();

        OleDbCommand cmd1 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + TxtPreMake.SelectedItem + "'", con);

        OleDbDataReader dr1 = cmd1.ExecuteReader();


        while (dr1.Read())
        {
            b = Convert.ToInt16(dr1[1].ToString());


            if (PS.Checked == false)
            {
                for (int i = 1; i <= b; i++)
                {

                    TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                    ctrl1.Enabled = false;
                    
                    ctrl1.Text = "";


                }
            }
            if (PS.Checked == true )
            {
                for (int i = 1; i <= b; i++)
                {

                    TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                    ctrl1.Enabled = true;
                    ctrl1.Text = "";


                }
            }


        }

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
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

        OleDbCommand cmd = new OleDbCommand("select * from recipe where RecCode='" + TxtRecCode1.SelectedItem + "'", con);

        OleDbDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            TxtPreMake.SelectedValue  = dr[6].ToString();
            str = dr[6].ToString();
            TxtDiscription.Text = dr[1].ToString();
            
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

        OleDbCommand cmdr2 = new OleDbCommand("select * from recipe_1 where RecCode='" + TxtRecCode1.SelectedItem + "'", con);

        OleDbDataReader drr2 = cmdr2.ExecuteReader();
        while (drr2.Read())
        {
            for (int a = 11; a <= 20; a++)
            {
                for (int b = 1; b <= 16; b++)
                {
                    CheckBox vchk1 = (CheckBox)(this.FindControl("ctl00$ContentPlaceHolder1$val" + b + "chk" + a));
                    vchk1.Checked = Convert.ToBoolean(Convert.ToInt32(drr2[kkk]));
                    kkk = kkk+ 1;
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


        OleDbCommand cmd2 = new OleDbCommand("select * from PRESSES WHERE PRESSMAKE ='" + TxtPreMake.SelectedItem + "'", con);

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
        Button1.Enabled = true ;

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    