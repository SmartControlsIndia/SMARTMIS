using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace SmartMIS.Input
{
    public partial class AddXRayOperator : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        int processID;
        protected void Page_Load(object sender, EventArgs e)
        {
            processID = 17;
            if (!IsPostBack)
            {
                fillWCGroupdropdownlist();
                fillXrayop();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = @"  
 IF(NOT EXISTS(SELECT XrayWcId FROM XrayOpLogin WHERE XrayWcId=@XrayWcId ))
	BEGIN
 insert into XrayOpLogin(XrayWcId,ManningId) values(@XrayWcId,@ManningId)
 END
 else
 begin
 update XrayOpLogin set ManningId=@ManningId where XrayWcId=@XrayWcId
 end";
            myConnection.comm.Parameters.AddWithValue("@XrayWcId", dpdxray.SelectedValue);

            myConnection.comm.Parameters.AddWithValue("@ManningId", dpdop1.SelectedValue);


            myConnection.comm.ExecuteNonQuery();

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            fillXrayop();

        }
        private void fillWCGroupdropdownlist()
        {
            dpdop1.DataSource = null;
            DataTable dt = new DataTable();
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = @"select id,(firstname + ' ' +  lastname)name from vmanning where  areaName='XRAYTBR' order by firstname,lastname asc ";


            myConnection.reader = myConnection.comm.ExecuteReader();
            dt.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            dpdop1.DataSource = dt;
            dpdop1.DataValueField = "id";
            dpdop1.DataTextField = "name";
            dpdop1.DataBind();



        }
        private void fillXrayop()
        {

            DataTable dt = new DataTable();
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = @"SELECT    
     wc.name as WCName,(MM.firstName+  ' '+MM.lastName)Operator
  FROM  XrayOpLogin XrayL
  inner join manningMaster MM on MM.iD=XrayL.ManningId
  inner join wcMaster WC on WC.id=XrayL.XraywcId where areaName='XRAYTBR'";


            myConnection.reader = myConnection.comm.ExecuteReader();
            dt.Load(myConnection.reader);

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
            grdOpXray.DataSource = dt;

            grdOpXray.DataBind();



        }
    }
}
