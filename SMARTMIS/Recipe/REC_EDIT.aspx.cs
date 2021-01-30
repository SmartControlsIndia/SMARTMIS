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

public partial class REC_EDIT : System.Web.UI.Page
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
            TxtEditBy.Text = substring.Substring(2, sublen);
        }
        sr.Close();
        sr.Dispose();
        if (!Page.IsPostBack)
        {
            
            TxtPreMake.Items.Clear();
            TxtCreatedOn.Text = DateTime.Now.ToShortDateString();
            OleDbConnection con = new OleDbConnection(connString);

            con.Open();

            OleDbCommand cmd = new OleDbCommand("select RecCode from recipe", con);

            OleDbDataReader dr = cmd.ExecuteReader();
            TxtRecCode.Items.Add("");
            while (dr.Read())
            {
               
                TxtRecCode.Items.Add(dr[0].ToString());

            }
            dr.Close();
            dr.Dispose();
            OleDbCommand cmd1 = new OleDbCommand("select pressmake from presses", con);
            TxtPreMake.Items.Add("");
            OleDbDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                
                TxtPreMake.Items.Add(dr1[0].ToString());

            }
            dr1.Close();
            dr.Dispose();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
            Label19.Text = "";
            Button1.Enabled = false;
	    TxtPreMake.Enabled = false;

        }
       
    }


    protected void Timer1_Tick(object sender, EventArgs e)
    {


    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        OleDbConnection con = new OleDbConnection(connString);
      // try
       // {
           if (TxtRecCode.SelectedValue!="" && TxtPreMake.SelectedValue!="")
           {
            con.Open();
            OleDbCommand cmd = new OleDbCommand("UPDATE RECIPE SET DESCRIPTION='" + TxtDiscription.Text + "',EDITEDBY='" + TxtEditBy.Text + "',EDITEDON='" + TxtCreatedOn.Text + "',PRESSMAKE='" + TxtPreMake.SelectedValue + "',S1V1='" + Convert.ToInt32(val1chk1.Checked) + "',S1V2='" + Convert.ToInt32(val2chk1.Checked) + "',S1V3='" + Convert.ToInt32(val3chk1.Checked) + "',S1V4='" + Convert.ToInt32(val4chk1.Checked) + "',S1V5='" + Convert.ToInt32(val5chk1.Checked) + "',S1V6='" + Convert.ToInt32(val6chk1.Checked) + "',S1V7='" + Convert.ToInt32(val7chk1.Checked) + "',S1V8='" + Convert.ToInt32(val8chk1.Checked) + "',S1V9='" + Convert.ToInt32(val9chk1.Checked) + "',S1V10='" + Convert.ToInt32(val10chk1.Checked) + "',S1V11='" + Convert.ToInt32(val11chk1.Checked) + "',S1V12='" + Convert.ToInt32(val12chk1.Checked) + "',S1V13='" + Convert.ToInt32(val13chk1.Checked) + "',S1V14='" + Convert.ToInt32(val14chk1.Checked) + "',S1V15='" + Convert.ToInt32(val15chk1.Checked) + "',S1V16='" + Convert.ToInt32(val16chk1.Checked) + "',S2V1='" + Convert.ToInt32(val1chk2.Checked) + "',S2V2='" + Convert.ToInt32(val2chk2.Checked) + "',S2V3='" + Convert.ToInt32(val3chk2.Checked) + "',S2V4='" + Convert.ToInt32(val4chk2.Checked) + "',S2V5='" + Convert.ToInt32(val5chk2.Checked) + "',S2V6='" + Convert.ToInt32(val6chk2.Checked) + "',S2V7='" + Convert.ToInt32(val7chk2.Checked) + "',S2V8='" + Convert.ToInt32(val8chk2.Checked) + "',S2V9='" + Convert.ToInt32(val9chk2.Checked) + "',S2V10='" + Convert.ToInt32(val10chk2.Checked) + "',S2V11='" + Convert.ToInt32(val11chk2.Checked) + "',S2V12='" + Convert.ToInt32(val12chk2.Checked) + "',S2V13='" + Convert.ToInt32(val13chk2.Checked) + "',S2V14='" + Convert.ToInt32(val14chk2.Checked) + "',S2V15='" + Convert.ToInt32(val15chk2.Checked) + "',S2V16='" + Convert.ToInt32(val16chk2.Checked) + "',S3V1='" + Convert.ToInt32(val1chk3.Checked) + "',S3V2='" + Convert.ToInt32(val2chk3.Checked) + "',S3V3='" + Convert.ToInt32(val3chk3.Checked) + "',S3V4='" + Convert.ToInt32(val4chk3.Checked) + "',S3V5='" + Convert.ToInt32(val5chk3.Checked) + "',S3V6='" + Convert.ToInt32(val6chk3.Checked) + "',S3V7='" + Convert.ToInt32(val7chk3.Checked) + "',S3V8='" + Convert.ToInt32(val8chk3.Checked) + "',S3V9='" + Convert.ToInt32(val9chk3.Checked) + "',S3V10='" + Convert.ToInt32(val10chk3.Checked) + "',S3V11='" + Convert.ToInt32(val11chk3.Checked) + "',S3V12='" + Convert.ToInt32(val12chk3.Checked) + "',S3V13='" + Convert.ToInt32(val13chk3.Checked) + "',S3V14='" + Convert.ToInt32(val14chk3.Checked) + "',S3V15='" + Convert.ToInt32(val15chk3.Checked) + "',S3V16='" + Convert.ToInt32(val16chk3.Checked) + "',S4V1='" + Convert.ToInt32(val1chk4.Checked) + "',S4V2='" + Convert.ToInt32(val2chk4.Checked) + "',S4V3='" + Convert.ToInt32(val3chk4.Checked) + "',S4V4='" + Convert.ToInt32(val4chk4.Checked) + "',S4V5='" + Convert.ToInt32(val5chk4.Checked) + "',S4V6='" + Convert.ToInt32(val6chk4.Checked) + "',S4V7='" + Convert.ToInt32(val7chk4.Checked) + "',S4V8='" + Convert.ToInt32(val8chk4.Checked) + "',S4V9='" + Convert.ToInt32(val9chk4.Checked) + "',S4V10='" + Convert.ToInt32(val10chk4.Checked) + "',S4V11='" + Convert.ToInt32(val11chk4.Checked) + "',S4V12='" + Convert.ToInt32(val12chk4.Checked) + "',S4V13='" + Convert.ToInt32(val13chk4.Checked) + "',S4V14='" + Convert.ToInt32(val14chk4.Checked) + "',S4V15='" + Convert.ToInt32(val15chk4.Checked) + "',S4V16='" + Convert.ToInt32(val16chk4.Checked) + "',S5V1='" + Convert.ToInt32(val1chk5.Checked) + "',S5V2='" + Convert.ToInt32(val2chk5.Checked) + "',S5V3='" + Convert.ToInt32(val3chk5.Checked) + "',S5V4='" + Convert.ToInt32(val4chk5.Checked) + "',S5V5='" + Convert.ToInt32(val5chk5.Checked) + "',S5V6='" + Convert.ToInt32(val6chk5.Checked) + "',S5V7='" + Convert.ToInt32(val7chk5.Checked) + "',S5V8='" + Convert.ToInt32(val8chk5.Checked) + "',S5V9='" + Convert.ToInt32(val9chk5.Checked) + "',S5V10='" + Convert.ToInt32(val10chk5.Checked) + "',S5V11='" + Convert.ToInt32(val11chk5.Checked) + "',S5V12='" + Convert.ToInt32(val12chk5.Checked) + "',S5V13='" + Convert.ToInt32(val13chk5.Checked) + "',S5V14='" + Convert.ToInt32(val14chk5.Checked) + "',S5V15='" + Convert.ToInt32(val15chk5.Checked) + "',S5V16='" + Convert.ToInt32(val16chk5.Checked) + "',S6V1='" + Convert.ToInt32(val1chk6.Checked) + "',S6V2='" + Convert.ToInt32(val2chk6.Checked) + "',S6V3='" + Convert.ToInt32(val3chk6.Checked) + "',S6V4='" + Convert.ToInt32(val4chk6.Checked) + "',S6V5='" + Convert.ToInt32(val5chk6.Checked) + "',S6V6='" + Convert.ToInt32(val6chk6.Checked) + "',S6V7='" + Convert.ToInt32(val7chk6.Checked) + "',S6V8='" + Convert.ToInt32(val8chk6.Checked) + "',S6V9='" + Convert.ToInt32(val9chk6.Checked) + "',S6V10='" + Convert.ToInt32(val10chk6.Checked) + "',S6V11='" + Convert.ToInt32(val11chk6.Checked) + "',S6V12='" + Convert.ToInt32(val12chk6.Checked) + "',S6V13='" + Convert.ToInt32(val13chk6.Checked) + "',S6V14='" + Convert.ToInt32(val14chk6.Checked) + "',S6V15='" + Convert.ToInt32(val15chk6.Checked) + "',S6V16='" + Convert.ToInt32(val16chk6.Checked) + "',S7V1='" + Convert.ToInt32(val1chk7.Checked) + "',S7V2='" + Convert.ToInt32(val2chk7.Checked) + "',S7V3='" + Convert.ToInt32(val3chk7.Checked) + "',S7V4='" + Convert.ToInt32(val4chk7.Checked) + "',S7V5='" + Convert.ToInt32(val5chk7.Checked) + "',S7V6='" + Convert.ToInt32(val6chk7.Checked) + "',S7V7='" + Convert.ToInt32(val7chk7.Checked) + "',S7V8='" + Convert.ToInt32(val8chk7.Checked) + "',S7V9='" + Convert.ToInt32(val9chk7.Checked) + "',S7V10='" + Convert.ToInt32(val10chk7.Checked) + "',S7V11='" + Convert.ToInt32(val11chk7.Checked) + "',S7V12='" + Convert.ToInt32(val12chk7.Checked) + "',S7V13='" + Convert.ToInt32(val13chk7.Checked) + "',S7V14='" + Convert.ToInt32(val14chk7.Checked) + "',S7V15='" + Convert.ToInt32(val15chk7.Checked) + "',S7V16='" + Convert.ToInt32(val16chk7.Checked) + "' WHERE RecCode = '" + TxtRecCode.SelectedValue + "'", con);
            OleDbCommand cmd1 = new OleDbCommand("UPDATE RECIPE SET S8V1='" + Convert.ToInt32(val1chk8.Checked) + "',S8V2='" + Convert.ToInt32(val2chk8.Checked) + "',S8V3='" + Convert.ToInt32(val3chk8.Checked) + "',S8V4='" + Convert.ToInt32(val4chk8.Checked) + "',S8V5='" + Convert.ToInt32(val5chk8.Checked) + "',S8V6='" + Convert.ToInt32(val6chk8.Checked) + "',S8V7='" + Convert.ToInt32(val7chk8.Checked) + "',S8V8='" + Convert.ToInt32(val8chk8.Checked) + "',S8V9='" + Convert.ToInt32(val9chk8.Checked) + "',S8V10='" + Convert.ToInt32(val10chk8.Checked) + "',S8V11='" + Convert.ToInt32(val11chk8.Checked) + "',S8V12='" + Convert.ToInt32(val12chk8.Checked) + "',S8V13='" + Convert.ToInt32(val13chk8.Checked) + "',S8V14='" + Convert.ToInt32(val14chk8.Checked) + "',S8V15='" + Convert.ToInt32(val15chk8.Checked) + "',S8V16='" + Convert.ToInt32(val16chk8.Checked) + "',S9V1='" + Convert.ToInt32(val1chk9.Checked) + "',S9V2='" + Convert.ToInt32(val2chk9.Checked) + "',S9V3='" + Convert.ToInt32(val3chk9.Checked) + "',S9V4='" + Convert.ToInt32(val4chk9.Checked) + "',S9V5='" + Convert.ToInt32(val5chk9.Checked) + "',S9V6='" + Convert.ToInt32(val6chk9.Checked) + "',S9V7='" + Convert.ToInt32(val7chk9.Checked) + "',S9V8='" + Convert.ToInt32(val8chk9.Checked) + "',S9V9='" + Convert.ToInt32(val9chk9.Checked) + "',S9V10='" + Convert.ToInt32(val10chk9.Checked) + "',S9V11='" + Convert.ToInt32(val11chk9.Checked) + "',S9V12='" + Convert.ToInt32(val12chk9.Checked) + "',S9V13='" + Convert.ToInt32(val13chk9.Checked) + "',S9V14='" + Convert.ToInt32(val14chk9.Checked) + "',S9V15='" + Convert.ToInt32(val15chk9.Checked) + "',S9V16='" + Convert.ToInt32(val16chk9.Checked) + "',S10V1='" + Convert.ToInt32(val1chk10.Checked) + "',S10V2='" + Convert.ToInt32(val2chk10.Checked) + "',S10V3='" + Convert.ToInt32(val3chk10.Checked) + "',S10V4='" + Convert.ToInt32(val4chk10.Checked) + "',S10V5='" + Convert.ToInt32(val5chk10.Checked) + "',S10V6='" + Convert.ToInt32(val6chk10.Checked) + "',S10V7='" + Convert.ToInt32(val7chk10.Checked) + "',S10V8='" + Convert.ToInt32(val8chk10.Checked) + "',S10V9='" + Convert.ToInt32(val9chk10.Checked) + "',S10V10='" + Convert.ToInt32(val10chk10.Checked) + "',S10V11='" + Convert.ToInt32(val11chk10.Checked) + "',S10V12='" + Convert.ToInt32(val12chk10.Checked) + "',S10V13='" + Convert.ToInt32(val13chk10.Checked) + "',S10V14='" + Convert.ToInt32(val14chk10.Checked) + "',S10V15='" + Convert.ToInt32(val15chk10.Checked) + "',S10V16='" + Convert.ToInt32(val16chk10.Checked) + "',TimeMM_1='" + min1.Text + "',TimeMM_2='" + min2.Text + "',TimeMM_3='" + min3.Text + "',TimeMM_4='" + min4.Text + "',TimeMM_5='" + min5.Text + "',TimeMM_6='" + min6.Text + "',TimeMM_7='" + min7.Text + "',TimeMM_8='" + min8.Text + "',TimeMM_9='" + min9.Text + "',TimeMM_10='" + min10.Text + "',TimeSS_1='" + sec1.Text + "',TimeSS_2='" + sec2.Text + "',TimeSS_3='" + sec3.Text + "',TimeSS_4='" + sec4.Text + "',TimeSS_5='" + sec5.Text + "',TimeSS_6='" + sec6.Text + "',TimeSS_7='" + sec7.Text + "',TimeSS_8='" + sec8.Text + "',TimeSS_9='" + sec9.Text + "',TimeSS_10='" + sec10.Text + "',Spare_1='" + ps1.Text + "',Spare_2='" + ps2.Text + "',Spare_3='" + ps3.Text + "',Spare_4='" + ps4.Text + "',Spare_5='" + ps5.Text + "',Spare_6='" + ps6.Text + "',Spare_7='" + ps7.Text + "',Spare_8='" + ps8.Text + "',Spare_9='" + ps9.Text + "',Spare_10='" + ps10.Text + "' WHERE RecCode = '" + TxtRecCode.SelectedValue + "'", con);
            OleDbCommand cmd2 = new OleDbCommand("UPDATE RECIPE_1 SET S11V1='" + Convert.ToInt32(val1chk11.Checked) + "',S11V2='" + Convert.ToInt32(val2chk11.Checked) + "',S11V3='" + Convert.ToInt32(val3chk11.Checked) + "',S11V4='" + Convert.ToInt32(val4chk11.Checked) + "',S11V5='" + Convert.ToInt32(val5chk11.Checked) + "',S11V6='" + Convert.ToInt32(val6chk11.Checked) + "',S11V7='" + Convert.ToInt32(val7chk11.Checked) + "',S11V8='" + Convert.ToInt32(val8chk11.Checked) + "',S11V9='" + Convert.ToInt32(val9chk11.Checked) + "',S11V10='" + Convert.ToInt32(val10chk11.Checked) + "',S11V11='" + Convert.ToInt32(val11chk11.Checked) + "',S11V12='" + Convert.ToInt32(val12chk11.Checked) + "',S11V13='" + Convert.ToInt32(val13chk11.Checked) + "',S11V14='" + Convert.ToInt32(val14chk11.Checked) + "',S11V15='" + Convert.ToInt32(val15chk11.Checked) + "',S11V16='" + Convert.ToInt32(val16chk11.Checked) + "',S12V1='" + Convert.ToInt32(val1chk12.Checked) + "',S12V2='" + Convert.ToInt32(val2chk12.Checked) + "',S12V3='" + Convert.ToInt32(val3chk12.Checked) + "',S12V4='" + Convert.ToInt32(val4chk12.Checked) + "',S12V5='" + Convert.ToInt32(val5chk12.Checked) + "',S12V6='" + Convert.ToInt32(val6chk12.Checked) + "',S12V7='" + Convert.ToInt32(val7chk12.Checked) + "',S12V8='" + Convert.ToInt32(val8chk12.Checked) + "',S12V9='" + Convert.ToInt32(val9chk12.Checked) + "',S12V10='" + Convert.ToInt32(val10chk12.Checked) + "',S12V11='" + Convert.ToInt32(val11chk12.Checked) + "',S12V12='" + Convert.ToInt32(val12chk12.Checked) + "',S12V13='" + Convert.ToInt32(val13chk12.Checked) + "',S12V14='" + Convert.ToInt32(val14chk12.Checked) + "',S12V15='" + Convert.ToInt32(val15chk12.Checked) + "',S12V16='" + Convert.ToInt32(val16chk12.Checked) + "',S13V1='" + Convert.ToInt32(val1chk13.Checked) + "',S13V2='" + Convert.ToInt32(val2chk13.Checked) + "',S13V3='" + Convert.ToInt32(val3chk13.Checked) + "',S13V4='" + Convert.ToInt32(val4chk13.Checked) + "',S13V5='" + Convert.ToInt32(val5chk13.Checked) + "',S13V6='" + Convert.ToInt32(val6chk13.Checked) + "',S13V7='" + Convert.ToInt32(val7chk13.Checked) + "',S13V8='" + Convert.ToInt32(val8chk13.Checked) + "',S13V9='" + Convert.ToInt32(val9chk13.Checked) + "',S13V10='" + Convert.ToInt32(val10chk13.Checked) + "',S13V11='" + Convert.ToInt32(val11chk13.Checked) + "',S13V12='" + Convert.ToInt32(val12chk13.Checked) + "',S13V13='" + Convert.ToInt32(val13chk13.Checked) + "',S13V14='" + Convert.ToInt32(val14chk13.Checked) + "',S13V15='" + Convert.ToInt32(val15chk13.Checked) + "',S13V16='" + Convert.ToInt32(val16chk13.Checked) + "',S14V1='" + Convert.ToInt32(val1chk14.Checked) + "',S14V2='" + Convert.ToInt32(val2chk14.Checked) + "',S14V3='" + Convert.ToInt32(val3chk14.Checked) + "',S14V4='" + Convert.ToInt32(val4chk14.Checked) + "',S14V5='" + Convert.ToInt32(val5chk14.Checked) + "',S14V6='" + Convert.ToInt32(val6chk14.Checked) + "',S14V7='" + Convert.ToInt32(val7chk14.Checked) + "',S14V8='" + Convert.ToInt32(val8chk14.Checked) + "',S14V9='" + Convert.ToInt32(val9chk14.Checked) + "',S14V10='" + Convert.ToInt32(val10chk14.Checked) + "',S14V11='" + Convert.ToInt32(val11chk14.Checked) + "',S14V12='" + Convert.ToInt32(val12chk14.Checked) + "',S14V13='" + Convert.ToInt32(val13chk14.Checked) + "',S14V14='" + Convert.ToInt32(val14chk14.Checked) + "',S14V15='" + Convert.ToInt32(val15chk14.Checked) + "',S14V16='" + Convert.ToInt32(val16chk14.Checked) + "',S15V1='" + Convert.ToInt32(val1chk15.Checked) + "',S15V2='" + Convert.ToInt32(val2chk15.Checked) + "',S15V3='" + Convert.ToInt32(val3chk15.Checked) + "',S15V4='" + Convert.ToInt32(val4chk15.Checked) + "',S15V5='" + Convert.ToInt32(val5chk15.Checked) + "',S15V6='" + Convert.ToInt32(val6chk15.Checked) + "',S15V7='" + Convert.ToInt32(val7chk15.Checked) + "',S15V8='" + Convert.ToInt32(val8chk15.Checked) + "',S15V9='" + Convert.ToInt32(val9chk15.Checked) + "',S15V10='" + Convert.ToInt32(val10chk15.Checked) + "',S15V11='" + Convert.ToInt32(val11chk15.Checked) + "',S15V12='" + Convert.ToInt32(val12chk15.Checked) + "',S15V13='" + Convert.ToInt32(val13chk15.Checked) + "',S15V14='" + Convert.ToInt32(val14chk15.Checked) + "',S15V15='" + Convert.ToInt32(val15chk15.Checked) + "',S15V16='" + Convert.ToInt32(val16chk15.Checked) + "',S16V1='" + Convert.ToInt32(val1chk16.Checked) + "',S16V2='" + Convert.ToInt32(val2chk16.Checked) + "',S16V3='" + Convert.ToInt32(val3chk16.Checked) + "',S16V4='" + Convert.ToInt32(val4chk16.Checked) + "',S16V5='" + Convert.ToInt32(val5chk16.Checked) + "',S16V6='" + Convert.ToInt32(val6chk16.Checked) + "',S16V7='" + Convert.ToInt32(val7chk16.Checked) + "',S16V8='" + Convert.ToInt32(val8chk16.Checked) + "',S16V9='" + Convert.ToInt32(val9chk16.Checked) + "',S16V10='" + Convert.ToInt32(val10chk16.Checked) + "',S16V11='" + Convert.ToInt32(val11chk16.Checked) + "',S16V12='" + Convert.ToInt32(val12chk16.Checked) + "',S16V13='" + Convert.ToInt32(val13chk16.Checked) + "',S16V14='" + Convert.ToInt32(val14chk16.Checked) + "',S16V15='" + Convert.ToInt32(val15chk16.Checked) + "',S16V16='" + Convert.ToInt32(val16chk16.Checked) + "',S17V1='" + Convert.ToInt32(val1chk17.Checked) + "',S17V2='" + Convert.ToInt32(val2chk17.Checked) + "',S17V3='" + Convert.ToInt32(val3chk17.Checked) + "',S17V4='" + Convert.ToInt32(val4chk17.Checked) + "',S17V5='" + Convert.ToInt32(val5chk17.Checked) + "',S17V6='" + Convert.ToInt32(val6chk17.Checked) + "',S17V7='" + Convert.ToInt32(val7chk17.Checked) + "',S17V8='" + Convert.ToInt32(val8chk17.Checked) + "',S17V9='" + Convert.ToInt32(val9chk17.Checked) + "',S17V10='" + Convert.ToInt32(val10chk17.Checked) + "',S17V11='" + Convert.ToInt32(val11chk17.Checked) + "',S17V12='" + Convert.ToInt32(val12chk17.Checked) + "',S17V13='" + Convert.ToInt32(val13chk17.Checked) + "',S17V14='" + Convert.ToInt32(val14chk17.Checked) + "',S17V15='" + Convert.ToInt32(val15chk17.Checked) + "',S17V16='" + Convert.ToInt32(val16chk17.Checked) + "' WHERE RecCode = '" + TxtRecCode.SelectedValue + "'", con);
            OleDbCommand cmd3 = new OleDbCommand("UPDATE RECIPE_1 SET S18V1='" + Convert.ToInt32(val1chk18.Checked) + "',S18V2='" + Convert.ToInt32(val2chk18.Checked) + "',S18V3='" + Convert.ToInt32(val3chk18.Checked) + "',S18V4='" + Convert.ToInt32(val4chk18.Checked) + "',S18V5='" + Convert.ToInt32(val5chk18.Checked) + "',S18V6='" + Convert.ToInt32(val6chk18.Checked) + "',S18V7='" + Convert.ToInt32(val7chk18.Checked) + "',S18V8='" + Convert.ToInt32(val8chk18.Checked) + "',S18V9='" + Convert.ToInt32(val9chk18.Checked) + "',S18V10='" + Convert.ToInt32(val10chk18.Checked) + "',S18V11='" + Convert.ToInt32(val11chk18.Checked) + "',S18V12='" + Convert.ToInt32(val12chk18.Checked) + "',S18V13='" + Convert.ToInt32(val13chk18.Checked) + "',S18V14='" + Convert.ToInt32(val14chk18.Checked) + "',S18V15='" + Convert.ToInt32(val15chk18.Checked) + "',S18V16='" + Convert.ToInt32(val16chk18.Checked) + "',S19V1='" + Convert.ToInt32(val1chk19.Checked) + "',S19V2='" + Convert.ToInt32(val2chk19.Checked) + "',S19V3='" + Convert.ToInt32(val3chk19.Checked) + "',S19V4='" + Convert.ToInt32(val4chk19.Checked) + "',S19V5='" + Convert.ToInt32(val5chk19.Checked) + "',S19V6='" + Convert.ToInt32(val6chk19.Checked) + "',S19V7='" + Convert.ToInt32(val7chk19.Checked) + "',S19V8='" + Convert.ToInt32(val8chk19.Checked) + "',S19V9='" + Convert.ToInt32(val9chk19.Checked) + "',S19V10='" + Convert.ToInt32(val10chk19.Checked) + "',S19V11='" + Convert.ToInt32(val11chk19.Checked) + "',S19V12='" + Convert.ToInt32(val12chk19.Checked) + "',S19V13='" + Convert.ToInt32(val13chk19.Checked) + "',S19V14='" + Convert.ToInt32(val14chk19.Checked) + "',S19V15='" + Convert.ToInt32(val15chk19.Checked) + "',S19V16='" + Convert.ToInt32(val16chk19.Checked) + "',S20V1='" + Convert.ToInt32(val1chk20.Checked) + "',S20V2='" + Convert.ToInt32(val2chk20.Checked) + "',S20V3='" + Convert.ToInt32(val3chk20.Checked) + "',S20V4='" + Convert.ToInt32(val4chk20.Checked) + "',S20V5='" + Convert.ToInt32(val5chk20.Checked) + "',S20V6='" + Convert.ToInt32(val6chk20.Checked) + "',S20V7='" + Convert.ToInt32(val7chk20.Checked) + "',S20V8='" + Convert.ToInt32(val8chk20.Checked) + "',S20V9='" + Convert.ToInt32(val9chk20.Checked) + "',S20V10='" + Convert.ToInt32(val10chk20.Checked) + "',S20V11='" + Convert.ToInt32(val11chk20.Checked) + "',S20V12='" + Convert.ToInt32(val12chk20.Checked) + "',S20V13='" + Convert.ToInt32(val13chk20.Checked) + "',S20V14='" + Convert.ToInt32(val14chk20.Checked) + "',S20V15='" + Convert.ToInt32(val15chk20.Checked) + "',S20V16='" + Convert.ToInt32(val16chk20.Checked) + "',TimeMM_11='" + min11.Text + "',TimeMM_12='" + min12.Text + "',TimeMM_13='" + min13.Text + "',TimeMM_14='" + min14.Text + "',TimeMM_15='" + min15.Text + "',TimeMM_16='" + min16.Text + "',TimeMM_17='" + min17.Text + "',TimeMM_18='" + min18.Text + "',TimeMM_19='" + min19.Text + "',TimeMM_20='" + min20.Text + "',TimeSS_11='" + sec11.Text + "',TimeSS_12='" + sec12.Text + "',TimeSS_13='" + sec13.Text + "',TimeSS_14='" + sec14.Text + "',TimeSS_15='" + sec15.Text + "',TimeSS_16='" + sec16.Text + "',TimeSS_17='" + sec17.Text + "',TimeSS_18='" + sec18.Text + "',TimeSS_19='" + sec19.Text + "',TimeSS_20='" + sec20.Text + "',Spare_1='" + ps11.Text + "',Spare_2='" + ps12.Text + "',Spare_3='" + ps13.Text + "',Spare_4='" + ps14.Text + "',Spare_5='" + ps15.Text + "',Spare_6='" + ps16.Text + "',Spare_7='" + ps17.Text + "',Spare_8='" + ps18.Text + "',Spare_9='" + ps19.Text + "',Spare_10='" + ps20.Text + "' WHERE RecCode = '" + TxtRecCode.SelectedValue + "'", con);
            cmd.ExecuteNonQuery();
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            cmd3.ExecuteNonQuery();
            
            con.Close();
            con.Dispose();
            Label19.ForeColor = System.Drawing.Color.Green;
            Label19.Text = "RECIPE " + TxtRecCode.SelectedValue + " EDIT SUCCESSFULLY ";
           }
           else
           {
               Label19.ForeColor = System.Drawing.Color.Red;
               Label19.Text = "YOU ARE TRYING TO EDIT INVAILD DATA ";
           }
       // }
       //catch (Exception ex)
       //{
       //    Label19.ForeColor = System.Drawing.Color.Red;
       //    Label19.Text = ex.Message;
       //}
        

    }
    protected void TxtRecCode_TextChanged(object sender, EventArgs e)
    {
         TxtPreMake.Enabled = true;
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
            TxtPreMake.SelectedValue = dr[6].ToString();
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
        dr.Dispose();
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
        drr2.Dispose();


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
        Button1.Enabled = true;

    }
    protected void TxtPreMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 1; i <= 17; i++)
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

            for (int j = 1; j <= 12; j++)
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
            for (int i = 1; i <= 12; i++)
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
        a = 177;
        c = 171;

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
                    ctrl1.Enabled = true ;
                    


                }
            }


        }
        OleDbCommand cmd2 = new OleDbCommand("select * from RECIPE WHERE RecCode ='" + TxtRecCode.SelectedValue + "'", con);

        OleDbDataReader dr2 = cmd2.ExecuteReader();


        while (dr2.Read())
        {
            if (Sec.Checked == true)
            {
                for (int i = 1; i <= 10; i++)
                {
                    TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + i));
                    ctrl.Text = dr2[a].ToString();
                    a = a + 1;
                }
            }

        }

        OleDbCommand cmd3 = new OleDbCommand("select * from RECIPE_1 WHERE RecCode ='" + TxtRecCode.SelectedValue + "'", con);

        OleDbDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
            if (Sec.Checked == true)
            {

                for (int j = 11; j <= 20; j++)
                {
                    TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$sec" + j));
                    ctrl.Text = dr3[c].ToString();
                    c = c + 1;
                }
            }
        }
        dr1.Close();

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
        a = 187;
        c = 181;

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
            if (PS.Checked == true)
            {
                for (int i = 1; i <= b; i++)
                {

                    TextBox ctrl1 = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                    ctrl1.Enabled = true;

                }
            }


        }
        dr1.Close();
        OleDbCommand cmd2 = new OleDbCommand("select * from RECIPE WHERE RecCode ='" + TxtRecCode.SelectedValue + "'", con);

        OleDbDataReader dr2 = cmd2.ExecuteReader();


        while (dr2.Read())
        {
            if (PS.Checked == true)
            {
                for (int i = 1; i <= 10; i++)
                {
                    TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + i));
                    ctrl.Text = dr2[a].ToString();
                    a = a + 1;
                }
            }

        }

        OleDbCommand cmd3 = new OleDbCommand("select * from RECIPE_1 WHERE RecCode ='" + TxtRecCode.SelectedValue + "'", con);

        OleDbDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
            if (PS.Checked == true)
            {

                for (int j = 11; j <= 20; j++)
                {
                    TextBox ctrl = (TextBox)(this.FindControl("ctl00$ContentPlaceHolder1$ps" + j));
                    ctrl.Text = dr3[c].ToString();
                    c = c + 1;
                }
            }
        }

        if (con.State == ConnectionState.Open)
        {
            con.Close();
            con.Dispose();
        }

    }
    protected void TxtCreatedBy_TextChanged(object sender, EventArgs e)
    {

    }

    
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             