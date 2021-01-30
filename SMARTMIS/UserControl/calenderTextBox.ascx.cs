using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.UserControl
{
    public partial class calenderTextBox : System.Web.UI.UserControl
    {
        

        public String Text
        {
            get
            {
                return calenderUserControlTextBox.Value; 
            }
            set 
            {
                calenderUserControlTextBox.Value = value;

            }
        }

        public Boolean Disabled
        {
            get
            {
                return calenderUserControlTextBox.Disabled;
            }
            set
            {
                calenderUserControlTextBox.Disabled = value;

            }
        }

        public String Visiblity
        {

            get
            {
                return calenderUserControlTextBox.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                calenderUserControlTextBox.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        public String Width
        {

            get
            {
                return calenderUserControlTextBox.Style[HtmlTextWriterStyle.Width];
            }
            set
            {
                calenderUserControlTextBox.Style.Add(HtmlTextWriterStyle.Width, value);

            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }
    }
}