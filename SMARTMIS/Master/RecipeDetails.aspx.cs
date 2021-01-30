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
using System.Collections.Generic;
using System.IO;


namespace SmartMIS.Master
{
    public partial class RecipeDetails : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable recipedt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillSizedropdownlist();
                if (Session["userID"].ToString().Trim() == "")
                {
                    Response.Redirect("/SmartMIS/Default.aspx", true);
                }
                else
                {
                    //reportMasterFromDateTextBox.Text = DateTime.Now.ToString("dd-MM-yyyy");
                }
            }
        }
         private void fillSizedropdownlist()
        {
            DataTable d_dt = new DataTable();
           
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "Select DISTINCT id as rID,name from recipemaster where  processID  in ('5','8','4','7' ) order by name desc";
                
            myConnection.reader = myConnection.comm.ExecuteReader();
            d_dt.Load(myConnection.reader);
            
            ddlRecipe.DataSource = d_dt;
            ddlRecipe.DataTextField = "name";
            ddlRecipe.DataValueField = "rID";
            ddlRecipe.DataBind();
            ddlRecipe.Items.Insert(0, new ListItem("All", "All"));
            if (!myConnection.reader.IsClosed)
                myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            //ddlRecipe.DataSource = null;
            //ddlRecipe.DataSource = FillDropDownList("recipemaster", "name");
            //ddlRecipe.DataBind();
        }
         protected void ViewButton_Click(object sender, EventArgs e)
         {
             myWebService.writeLogs("click", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); 
             try
             {
                 string recipe = ddlRecipe.SelectedItem.Text;
                 myWebService.writeLogs(recipe, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();
                 if (recipe != "All")
                 {
                     myConnection.comm.CommandText = "SELECT name,recipeNo ,tyreSize ,description,oemID,tyreDesign,DBMCode,TBRPCode,TUOEnable,SpecWeight,TrimNo,excludeTUO,SAPMaterialCode,TBRTUOCode,upperWeight ,lowerWeight,curingCapacity   FROM recipeMaster where name='" + recipe + "' and  processID  in ('5','8','4','7' ) order by name desc";
                 }
                 else
                 {
                     myConnection.comm.CommandText = "SELECT name,recipeNo ,tyreSize ,description,oemID,tyreDesign,DBMCode,TBRPCode,TUOEnable,SpecWeight,TrimNo,excludeTUO,SAPMaterialCode,TBRTUOCode,upperWeight ,lowerWeight,curingCapacity   FROM recipeMaster where processID  in ('5','8','4','7' ) order by name desc";
                 }
                 
                 myConnection.reader = myConnection.comm.ExecuteReader();
                 recipedt.Load(myConnection.reader);
                 grdRecipe.DataSource = recipedt;
                 grdRecipe.DataBind();
             }
             catch (Exception ex)
             { 
                 myWebService.writeLogs(ex.StackTrace, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); 
             }
             finally
             {
                 if (!myConnection.reader.IsClosed)
                     myConnection.reader.Close();
                 myConnection.comm.Dispose();
                 myConnection.close(ConnectionOption.SQL);
             }

         }

         protected void Export_click(object sender, EventArgs e)
         {
             Response.Clear();
             Response.AddHeader("content-disposition", "attachment;filename=RecipeDetails" + DateTime.Now.ToString("yyyy_MM_ddThh_mm_ss_") + ".xls");
             Response.Charset = "";
             Response.ContentType = "application/vnd.xls";
             System.IO.StringWriter stringWrite = new System.IO.StringWriter();
             System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
             stringWrite.Write("<table><tr><td><b>Recipe </b></td><td><b>" + ddlRecipe.SelectedItem.Text + "</b></td></tr></table>");
             System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
             Controls.Add(form);
             form.Controls.Add(manningPanel);
             form.RenderControl(htmlWrite);

             //gv.RenderControl(htmlWrite);
             Response.Write(stringWrite.ToString());
             Response.End();
         }
    }
}
