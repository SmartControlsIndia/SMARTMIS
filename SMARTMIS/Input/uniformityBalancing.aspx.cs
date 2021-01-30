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

namespace SmartMIS.Input
{
    public partial class uniformityBalancing : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        fillStatusSearch();
                        fillGridView();
                    }
                    catch(Exception exp)
                    {

                    }
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                ((DropDownList)sender).Items.Remove("".Trim());

                if (((DropDownList)sender).ID == "uniBalStatusSearchDropDownList")
                {
                    try
                    {
                        uniBalGridView.DataSource = myWebService.fillGridView("Select DISTINCT gtbarCode, recipeCode FROM vUniBalTBR WHERE status = " + (((DropDownList)sender).SelectedIndex) + "", ConnectionOption.SQL);
                        uniBalGridView.DataBind();
                    }
                    catch (Exception ex)
                    { 
                    }
                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                if (((Button)sender).ID == "uniBalSaveButton")
                {

                }
                else if (((Button)sender).ID == "uniBalCancelButton")
                {

                }
                else if (((Button)sender).ID == "uniBalDialogOKButton")
                {
                    try
                    {
                        uniBalIDLabel.Text = uniBalIDHidden.Value; //Passing value to viIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    catch(Exception ex)
                    {
                    }
                }
                else if (((Button)sender).ID == "uniBalReviewDialogSaveButton")
                {
                    try
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (myWebService.authenticate(Session["userID"].ToString(), (((ImageButton)sender).AlternateText).Trim()) == true)
                {
                    if (((ImageButton)sender).ID == "uniBalGridReviewImageButton")
                    {
                        //Code for Reviewing
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        uniBalReviewDialogGTBarcodeLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridHiddenGTBarcodeLabel")).Text);
                        uniBalHiddenRecipeCode.Value = (((Label)gridViewRow.Cells[1].FindControl("uniBalGridHiddenRecipeCodeLabel")).Text);

                        uniBalHiddenRecipeCode.Value = myWebService.getID("recipeMaster", "name", uniBalHiddenRecipeCode.Value);

                        fillUserDecision();

                        ScriptManager.RegisterClientScriptBlock(uniBalMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForReview');", true);
                    }

                    else if (((ImageButton)sender).ID == "uniBalGridEditImageButton")
                    {
                        //Code for editing gridview row
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;
                    }

                    else if (((ImageButton)sender).ID == "uniBalGridDeleteImageButton")
                    {
                        //Code for deleting gridview row
                    }

                    else
                    { }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(uniBalMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
            {
                uniBalNotifyMessageDiv.Visible = false;
                uniBalNotifyTimer.Enabled = false;
            }

            protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "uniBalGridView")
                    {
                        try
                        {
                            Label gtBarcodeLabel = ((Label)e.Row.FindControl("uniBalGridGTBarcodeLabel"));

                            GridView childGridView = ((GridView)e.Row.FindControl("uniBalInnerGridView"));
                            fillChildGridView(childGridView, gtBarcodeLabel.Text.Trim());
                        }
                        catch(Exception exp)
                        {
                        }
                    }
                }
            }

        #endregion

        #region User Defined Function

        private void fillStatusSearch()
        {

            //Description   : Function for filling tyreXRay with Status Name
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01

            uniBalStatusSearchDropDownList.Items.Clear();
            uniBalStatusSearchDropDownList.Items.Add("");

            foreach (string item in myWebService.status)
            {
                uniBalStatusSearchDropDownList.Items.Add(item);
            }
        }

        public bool displayReviewNotReqd(Object gtbarcodeObj)
        {

            //Description   : Function for displaying review option in xRayGridView
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            bool flag = false;
            int db=0;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select count(gtbarcode) from vUniBalTBR WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

                 db = Convert.ToInt32(myConnection.comm.ExecuteScalar());
            }
            catch (Exception ex)
            {
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
            }

            if (db == 1)
            {

                myConnection.comm.CommandText = "Select status from vUniBalTBR WHERE gtbarCode = @gtBarCode2";
                myConnection.comm.Parameters.AddWithValue("@gtBarCode2", gtbarcodeObj.ToString());
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    string val = myConnection.reader[0].ToString();
                    if (val.Equals("0"))
                    {
                        flag = true;
                    }
                }
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        public bool displayDelete(Object gtbarcodeObj, Object iD)
        {

            //Description   : Function for displaying delete option in uniBalGridView
            //Author        : Brajesh kumar
            //Date Created  : 14 April 2011
            //Date Updated  : 14 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            bool flag = true;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select MIN(iD), MAX(iD) from vUniBalTBR WHERE gtbarCode = @gtbarCode";
            myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

            myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

            while (myConnection.reader.Read())
            {
                if ((Convert.ToInt32(iD) == Convert.ToInt32(myConnection.reader[0].ToString())) || (Convert.ToInt32(iD) < Convert.ToInt32(myConnection.reader[1].ToString())))
                {
                    flag = false;
                }
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        private void fillUserDecision()
        {

            //Description   : Function for filling uniBalUserDecisionDropDownList with User Decision
            //Author        : Brajesh kumar
            //Date Created  : 07 March 2011
            //Date Updated  : 07 March 2011
            //Revision No.  : 01

            uniBalReviewDialogUserDecisionDropDownList.Items.Clear();
            uniBalReviewDialogUserDecisionDropDownList.Items.Add("");

            foreach (string item in myWebService.userDecision)
            {
                uniBalReviewDialogUserDecisionDropDownList.Items.Add(item);
            }
        }

        private void fillGridView()
        {

            //Description   : Function for filling uniBalGridView
            //Author        : Brajesh kumar   || Brajesh kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011                || 07 April 2011
            //Revision No.  : 01            || 02                           || 03
            //Revision Desc :               || change the logic for row[9]  || change the logic for filling the uniBalGridView by webservice

            uniBalGridView.DataSource = myWebService.fillGridView("Select DISTINCT gtbarCode, recipeCode from vUniBalTBR", ConnectionOption.SQL);
            uniBalGridView.DataBind();
        }

        public string displayStatus(Object obj)
        {

            //Description   : Function for making a decision status of viGridView
            //Author        : Brajesh kumar
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                flag = myWebService.statusState[Convert.ToInt32(obj)];
            }

            return flag;
        }

        /// <summary>
        /// Function for displaying review option in uniBalGridView
        /// </summary>
        /// <param name="obj">Object for enabling review option</param>
        /// <returns></returns>

        public bool displayReview(Object gtbarcodeObj, Object iD, Object statusObj)
        {

            //Description   : Function for displaying review option in xRayGridView
            //Author        : Brajesh kumar   || Brajesh kumar
            //Date Created  : 07 April 2011 ||
            //Date Updated  : 07 April 2011 || 14 April 2011                                || 18 April 2011
            //Revision No.  : 01            || 02                                           || 03
            //Revision Desc :               || Change the logic for display review button   || Change the logic for display review button

            bool flag = true;

            if (!string.IsNullOrEmpty(gtbarcodeObj.ToString()))
            {

                if ((statusObj.ToString().Trim() == "0") || (statusObj.ToString().Trim() == "3"))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select gtbarCode from vUniBalTBR WHERE gtbarCode = @gtbarCode";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

                    myConnection.reader = myConnection.comm.ExecuteReader();

                    while (myConnection.reader.Read())
                    {
                        flag = false;
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
                else if ((statusObj.ToString().Trim() == "1") || (statusObj.ToString().Trim() == "2"))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select MIN(iD), MAX(iD) from vUniBalTBR WHERE gtbarCode = @gtbarCode";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

                    myConnection.reader = myConnection.comm.ExecuteReader();

                    while (myConnection.reader.Read())
                    {
                        if ((Convert.ToInt32(iD) < Convert.ToInt32(myConnection.reader[1].ToString())))
                        {
                            flag = false;
                        }
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }
            return flag;
        }

        public string displayUserDecision(Object obj)
        {

            //Description   : Function for displaying user decision in viGridView
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = "";

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                flag = myWebService.userDecision[Convert.ToInt32(obj)];
            }

            return flag;
        }

        private int save()
        {
            //Description   : Function for saving reviewed record in unibalrunouttbr Table
            //Author        : Brajesh kumar
            //Date Created  : 06 April 2011
            //Date Updated  : 06 April 2011
            //Revision No.  : 01           
            //Revision Desc :              

            int flag = 0;

            if (uniBalIDLabel.Text.Trim() == "0")
            {
                if (validationForReview() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Insert into unibalrunouttbr (wcID, gtbarCode, recipeID, manningID, status, action,  dtandTime) values (@wcID, @gtbarCode, @recipeID, @manningID, @status, @action, @dtandTime)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", uniBalWCIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", uniBalReviewDialogGTBarcodeLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@recipeID", uniBalHiddenRecipeCode.Value.Trim());
                    myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                    myConnection.comm.Parameters.AddWithValue("@status", myWebService.getStatusState(uniBalReviewDialogUserDecisionDropDownList.SelectedValue.ToString().Trim()));
                    myConnection.comm.Parameters.AddWithValue("@action", uniBalReviewDialogActionTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now));

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "Uniformity record saved successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in tyreXray Table
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (uniBalIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From unibalrunouttbr WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", uniBalIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "Uniformity record deleted successfully");
            }

            return flag;
        }

        public string isAuthenticate(string roleID)
        {
            return myWebService.authenticate(Session["userID"].ToString(), roleID).ToString();
        }

        private int validationForReview()
        {

            //Description   : Function for validation for review of data for inserting and deleting record in tyreXRay Table
            //Author        : Brajesh kumar
            //Date Created  : 07 April 2011
            //Date Updated  : 07 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (uniBalWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (uniBalReviewDialogUserDecisionDropDownList.SelectedValue.Trim() == "")
            {
                flag = 1;
            }
            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh kumar
            //Date Created  : 30 March 2011
            //Date Updated  : 30 March 2011
            //Revision No.  : 01
            //Revision Desc : 

            uniBalIDLabel.Text = "0";
            uniBalReviewDialogActionTextBox.Text = "";
        }

        private void fillChildGridView(GridView childGridView, string gtBarcode)
        {

            if (childGridView.ID == "uniBalInnerGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName, gtbarCode, recipeCode, sapCode, status, action, dtandTime from vUniBalTBR WHERE gtbarCode = '" + gtBarcode + "'", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in uniBalMessageDiv
            //Author        : Brajesh kumar
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                uniBalNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                uniBalNotifyImage.Src = "../Images/tick.png";
            }
            uniBalNotifyLabel.Text = notifyMessage;

            uniBalNotifyMessageDiv.Visible = true;
            uniBalNotifyTimer.Enabled = true;
        }

        #endregion
    }
}