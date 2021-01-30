using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Master
{
    public partial class ReceipeFileName : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string moduleName = "RecipeMaster";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                

                fillGridView();
            }
        }
        private void fillGridView()
        {


            recipeGridView.DataSource = myWebService.fillGridView("select [Id],[TuoFileName],[LastUpdate] from LastUpdateTBRuniformityData ", "Id", smartMISWebService.order.Desc);
            recipeGridView.DataBind();
        }
       

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = @" 
  IF @id=0
begin
 IF(NOT EXISTS(SELECT * FROM LastUpdateTBRuniformityData WHERE TuoFileName=@TuoFileName))
	BEGIN
	insert into LastUpdateTBRuniformityData (TuoFileName,LastUpdate)values(@TuoFileName,@dtandtime)
	end
end
	
 

else
	begin
	update LastUpdateTBRuniformityData set  TuoFileName=@TuoFileName ,LastUpdate=@dtandtime where Id=@id
	end 
";
                myConnection.comm.Parameters.AddWithValue("@TuoFileName", TextBox1.Text    );
                myConnection.comm.Parameters.AddWithValue("@dtandtime",Convert.ToDateTime(TextBox2.Text) );
                myConnection.comm.Parameters.AddWithValue("@id", recipeIDHidden.Value);
 



             int   flag = myConnection.comm.ExecuteNonQuery();

             if (flag > 0)
             { fillGridView(); }
             else
             { }


                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            catch (Exception ex)
            { }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
          // recipeNotifyMessageDiv1.Visible = false;
            recipeNotifyTimer.Enabled = false;
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {

            if (((ImageButton)sender).ID == "recipeGridEditImageButton")
            {
                
                    //Code for editing gridview row
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    TextBox1.Text = (((Label)gridViewRow.Cells[1].FindControl("lblTuoFileName")).Text);
                    TextBox2.Text = (((Label)gridViewRow.Cells[1].FindControl("lblLastUpdate")).Text);

                    recipeIDHidden.Value = (((HiddenField)gridViewRow.Cells[1].FindControl("hfdid")).Value);
                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(recipeCancelButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }

            
        }
    }

}
