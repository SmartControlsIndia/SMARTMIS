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
using System.Xml;

namespace SmartMIS
{
    public partial class autoComplete : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlTextWriter xmldoc;

            Response.Clear();
            Response.ContentType = "text/xml";

            xmldoc = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);
            xmldoc.WriteStartDocument(true);
            xmldoc.WriteStartElement("root");
 
            findwords(xmldoc);
            xmldoc.WriteEndElement();
            xmldoc.WriteEndDocument();
            xmldoc.Close();
            Response.End();
        }

        private String findwords(XmlWriter xmldoc)
        {
            String flag = "--";

            myConnection.open(ConnectionOption.Historian);
            myConnection.oComm = myConnection.oConn.CreateCommand();

            myConnection.oComm.CommandText = "SELECT value from ihRawData WHERE tagname = TVI1.CalculationTag AND samplingmode=CurrentValue AND quality=Good*";

            myConnection.oReader = myConnection.oComm.ExecuteReader();

            while (myConnection.oReader.Read())
            {
                flag = Convert.ToString(myConnection.oReader[0].ToString());
                xmldoc.WriteElementString("viHMIGTBarcodeTextBox", flag);
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();

            return flag;
        }
    }
}
