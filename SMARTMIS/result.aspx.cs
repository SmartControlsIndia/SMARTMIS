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
using SmartMIS.SmartWebReference;
using System.Text;

namespace SmartMIS
{
    public partial class result : System.Web.UI.Page
    {

        string clientName;
        protected void Page_Load(object sender, EventArgs e)
        {
            clientName = Request["search"].ToString();
            Getresult();
        }

        //The Method Getresult will return (Response.Write) which contains search results seprated by character "~"

        // For E.G. "Ra~Rab~Racd~Raef~Raghi"   which will going to display in search suggest box 




        private void Getresult()
        {
            DataTable dt = new DataTable();

            myConnection.open();
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select recipeName from recipeMaster WHERE recipeName LIKE @gtbarCode";
            myConnection.comm.Parameters.AddWithValue("@gtbarCode", clientName + "%");

            
            
            myConnection.adapter = new System.Data.SqlClient.SqlDataAdapter(myConnection.comm);
            myConnection.adapter.Fill(dt);
            
            StringBuilder stringBuilder = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stringBuilder.Append(dt.Rows[i].ItemArray[0].ToString() + "~");   //Create Con
                }
            }

            Response.Write(stringBuilder.ToString());

        }
    }
}
