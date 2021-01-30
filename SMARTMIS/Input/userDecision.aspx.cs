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

namespace SmartMIS.Input
{
    public partial class userDecision : System.Web.UI.Page
    {
        string[] status = new string[] { "OK", "Not OK" };
        smartMISWebService myWebService = new smartMISWebService();
        string gtbarCode, statusID = null;       
        string[] userdecision = new string[] { "N/A", "Scrap", "Rework" };

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                inputDataSet.createDataSet();
                fillGridView();
                fillStatus();
                fillStatusSearch();
                fillReasonName();
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {

            if (((Button)sender).ID == "udDialogDelOKButton")
            {
                udIDLabel.Text = udIDHidden.Value;
                delete();
                fillGridView();
                clearPage();

            }

            else if (((Button)sender).ID == "udDialogOKButton")
            {
                udIDLabel.Text = udIDHidden.Value; //Passing value to areaIDLabel because on postback hidden field retains its value
                save();
                fillGridView();
                clearPage();
            }
        }

        protected void DropDown_IndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());

            if (((DropDownList)sender).ID == "modalReasonDropDownList")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from reasonMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", modalReasonDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    udReasonIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close();
            }

            else if (((DropDownList)sender).ID == "udStatusChangeDropDown")
            {
                userDecisionGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName, gtbarCode, faultstatusID, faultName, reasonID, reasonName, manningID, status from vTyreXray where status=" + (((DropDownList)sender).SelectedIndex) + "");
                userDecisionGridView.DataBind();           
            }

            else if (((DropDownList)sender).ID=="modalStatusDropDownList")
            {
                if (((DropDownList)sender).SelectedValue == status[0])
                {
                    ModalUserDecisionDropDownList.Enabled = false;
                    ModalUserDecisionDropDownList.SelectedValue = "N/A";
                }
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "udGridDecisionImageButton")
            {
                //Code for Reviewing
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                modalGTBarCodeLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("udGridGTBarcodeLabel")).Text);
                
               
                fillUserDecision();

                ScriptManager.RegisterClientScriptBlock(viMagicButton, this.GetType(), "BlockName", "javascript:revealModal('ModalPage2');", true);
            }
        }


        protected void xRayNotifyTimer_Tick(object sender, EventArgs e)
        {
            viNotifyMessageDiv.Visible = false;
            viNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private int delete()
        {

            //Description   : Function for deleting record in areaMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (udIDLabel.Text.Trim() != "0")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From tyreXRay WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", udIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close();

                Notify(1, "Record deleted successfully");
            }

            return flag;
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in xrayMessageDiv
            //Author        : Rohit Singh
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
                viNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                viNotifyImage.Src = "../Images/tick.png";
            }
            viNotifyLabel.Text = notifyMessage;

            viNotifyMessageDiv.Visible = true;
            viNotifyTimer.Enabled = true;
        }

        private void clearPage()
        {
                fillGridView();
                fillStatus();
                fillStatusSearch();
                fillReasonName();
            }

        public string displayStatus(Object obj)
        {

            //Description   : Function for making a decision status of viGridView
            //Author        : Rohit Singh
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                flag = status[Convert.ToInt32(obj)];
            }
            return flag;
        }

        private void fillUserDecision()
        {

            //Description   : Function for filling viUserDecisionDropDownList with User Decision
            //Author        : Rohit Singh
            //Date Created  : 05 March 2011
            //Date Updated  : 05 March 2011
            //Revision No.  : 01

            ModalUserDecisionDropDownList.Items.Clear();
            ModalUserDecisionDropDownList.Items.Add("");

            foreach (string item in userdecision)
            {
                ModalUserDecisionDropDownList.Items.Add(item);
            }
        }

        private int save()
        {
            //Description   : Function for saving and updating record in tyreXRay Table
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 28 March 2011
            //Date Updated  : 28 March 2011 || 29 March 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || change the logic for row[9]

            int flag = 0;

            if (udIDLabel.Text.Trim() == "0")
            {
                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Insert into tyreXRay (wcID, gtbarCode, faultstatusID, reasonID, manningID, recipeCode, status, dtandTime, userDecision) values (@wcID, @gtbarCode, @faultstatusID, @reasonID, @manningID, @recipeCode, @status, @dtandTime, @userDecision)";
                myConnection.comm.Parameters.AddWithValue("@wcID", "86");
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", modalGTBarCodeLabel.Text.Trim());
                myConnection.comm.Parameters.AddWithValue("@faultstatusID", "21");
                myConnection.comm.Parameters.AddWithValue("@reasonID", udReasonIDLabel.Text.Trim());
                myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                myConnection.comm.Parameters.AddWithValue("@recipeCode", "R19C29");
                myConnection.comm.Parameters.AddWithValue("@status", modalStatusDropDownList.SelectedIndex);
                myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now.ToLongDateString()));
                myConnection.comm.Parameters.AddWithValue("@userDecision", ModalUserDecisionDropDownList.SelectedIndex-1);

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close();

                Notify(1, "Record added successfully");
            }

             //   Notify(1, "X-Ray record saved successfully");

                 

            return flag;
        } 

        private void fillReasonName()
        {
            //Description   : Function for filling viReasonNameDropDownList with Reason Name Workcenter wise
            //Author        : Rohit Singh   || Rohit Singh
            //Date Created  : 30 March 2011 
            //Date Updated  : 30 March 2011 || 01 April 2011
            //Revision No.  : 01            || 02
            //Revision Desc :               || Change the Logic for filling the xRayReasonNameDropDownList

            modalReasonDropDownList.Items.Clear();
            modalReasonDropDownList.Items.Add("");

            modalReasonDropDownList.DataSource = myWebService.FillDropDownList("reasonMaster", "name", "WHERE wcID = 86");
            modalReasonDropDownList.DataBind();
        }

        private void fillStatus()
        {

            //Description   : Function for filling xRayStatusDropDownList with Status Name
            //Author        : Rohit Singh
            //Date Created  : 29 March 2011
            //Date Updated  : 29 March 2011
            //Revision No.  : 01

            modalStatusDropDownList.Items.Clear();
            modalStatusDropDownList.Items.Add("");

            foreach (string item in status)
            {
                modalStatusDropDownList.Items.Add(item);
            }
        }

        private void fillStatusSearch()
        {

            //Description   : Function for filling viStatusSearchDropDownList with Status Name
            //Author        : Rohit Singh
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01

            udStatusChangeDropDown.Items.Clear();
            udStatusChangeDropDown.Items.Add("");

            foreach (string item in status)
            {
                udStatusChangeDropDown.Items.Add(item);
            }
        }
        
        private void fillGridView()
        {

            userDecisionGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName, gtbarCode, faultstatusID, faultName, reasonID, reasonName, manningID, status from vTyreXray");
            userDecisionGridView.DataBind();
        }

        public bool displayReview(Object gtbarcodeObj, Object statusObj)
        {

            //Description   : Function for displaying review option in viGridView
            //Author        : Rohit Singh
            //Date Created  : 04 April 2011
            //Date Updated  : 04 April 2011
            //Revision No.  : 01
            //Revision Desc : 

            bool flag = true;

            if (!string.IsNullOrEmpty(statusObj.ToString()))
            {
                //if (statusObj.ToString() == "1")
                //{
                //    flag = true;
                //}
                //else
                //{
                //    flag = false;
                //}

                myConnection.open();
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from vtyreXRay WHERE gtbarCode = @gtbarCode AND Status = @Status";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());
                myConnection.comm.Parameters.AddWithValue("@Status", Convert.ToInt32("0"));

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = false;
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close();

            }

            return flag;
        }

        #endregion     


    }
}
