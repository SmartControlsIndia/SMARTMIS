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

namespace SmartMIS
{
    public partial class visualInspection : System.Web.UI.Page
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

                        fillWorkcenterName();
                        fillFaultStatus();
                        fillStatus();
                        fillFaultDirection();
                        fillStatusSearch();
                        fillUserDecision();
                        fillGridView();
                    }
                    catch(Exception exp)
                    {

                    }
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    ((DropDownList)sender).Items.Remove("".Trim());

                    if (((DropDownList)sender).ID == "viWCNameDropDownList")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                        myConnection.comm.Parameters.AddWithValue("@name", viWCNameDropDownList.SelectedItem.ToString().Trim());

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        while (myConnection.reader.Read())
                        {
                            viWCIDLabel.Text = myConnection.reader[0].ToString();
                        }

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        fillReasonName(viWCIDLabel.Text.Trim());
                    }
                    else if (((DropDownList)sender).ID == "viFaultStatusDropDownList")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Select * from defectstatusMaster Where name = @name";
                        myConnection.comm.Parameters.AddWithValue("@name", viFaultStatusDropDownList.SelectedItem.ToString().Trim());

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        while (myConnection.reader.Read())
                        {
                            viFaultStatusIDLabel.Text = myConnection.reader[0].ToString();
                        }

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                    else if (((DropDownList)sender).ID == "viReviewDialogReasonNameDropDownList")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Select * from reasonMaster Where name = @name AND wcID = @wcID";
                        myConnection.comm.Parameters.AddWithValue("@name", viReviewDialogReasonNameDropDownList.SelectedItem.ToString().Trim());
                        myConnection.comm.Parameters.AddWithValue("@wcID", viWCIDLabel.Text.Trim());

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        while (myConnection.reader.Read())
                        {
                            viReviewDialogReasonIDLabel.Text = myConnection.reader[0].ToString();
                        }

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                    else if (((DropDownList)sender).ID == "viStatusDropDownList")
                    {
                        if (((DropDownList)sender).SelectedValue == myWebService.status[0])
                        {
                            viFaultStatusDropDownList.Enabled = false;
                            viFaultDirectionDropDownList.Enabled = false;

                            viFaultStatusDropDownList.SelectedValue = "-";
                            viReviewDialogReasonNameDropDownList.SelectedValue = "-";
                            viFaultDirectionDropDownList.SelectedValue = myWebService.faultDirection[0];
                            viReviewDialogUserDecisionDropDownList.SelectedValue = myWebService.userDecision[0];

                            //*************************Calling the Events Programtically**************************************//

                            EventArgs viDropDownListEventArgs = new EventArgs();
                            DropDownList_SelectedIndexChanged(viFaultStatusDropDownList, viDropDownListEventArgs);
                            DropDownList_SelectedIndexChanged(viReviewDialogReasonNameDropDownList, viDropDownListEventArgs);

                            //***********************************************************************************************//
                        }
                        else if (((DropDownList)sender).SelectedValue == myWebService.status[1])
                        {
                            viFaultStatusDropDownList.Enabled = true;
                            fillFaultStatus();
                            viFaultDirectionDropDownList.Enabled = true;
                            fillFaultDirection();
                            viReviewDialogUserDecisionDropDownList.Enabled = true;
                            fillUserDecision();

                            viReviewDialogUserDecisionDropDownList.SelectedValue = myWebService.userDecision[0];
                            viReviewDialogReasonNameDropDownList.SelectedValue = "-";

                            //*************************Calling the Events Programtically**************************************//

                            EventArgs viDropDownListEventArgs = new EventArgs();
                            DropDownList_SelectedIndexChanged(viReviewDialogReasonNameDropDownList, viDropDownListEventArgs);

                            //***********************************************************************************************//
                        }
                    }
                    else if (((DropDownList)sender).ID == "viStatusSearchDropDownList")
                    {
                        viGridView.DataSource = myWebService.fillGridView("Select DISTINCT gtbarCode from vVisualization WHERE status = " + (((DropDownList)sender).SelectedIndex) + "", ConnectionOption.SQL);
                        viGridView.DataBind();
                    }
                }
                catch(Exception exp)
                {

                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                try
                {
                    if (((Button)sender).ID == "viSaveButton")
                    {
                        save();
                        fillGridView();
                        clearPage();
                    }
                    else if (((Button)sender).ID == "viCancelButton")
                    {
                        clearPage();
                    }
                    else if (((Button)sender).ID == "viDialogOKButton")
                    {
                        viIDLabel.Text = viIDHidden.Value; //Passing value to viIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else if (((Button)sender).ID == "viDialogOKButton")
                    {
                        viIDLabel.Text = viIDHidden.Value; //Passing value to viIDLabel because on postback hidden field retains its value
                        delete();
                        fillGridView();
                        clearPage();
                    }
                    else if (((Button)sender).ID == "viReviewDialogSaveButton")
                    {

                        //*************************Calling the Event Programtically**************************************//

                        EventArgs viDropDownListEventArgs = new EventArgs();
                        DropDownList_SelectedIndexChanged(viReviewDialogReasonNameDropDownList, viDropDownListEventArgs);

                        //***********************************************************************************************//

                        review();
                        fillGridView();
                        clearPage();
                    }
                }
                catch(Exception ex)
                {

                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (myWebService.authenticate(Session["userID"].ToString(), (((ImageButton)sender).AlternateText).Trim()) == true)
                {
                    if (((ImageButton)sender).ID == "viGridReviewImageButton")
                    {
                        //Code for Reviewing
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        viReviewDialogGTBarcodeLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("viGridHiddenGTBarcodeLabel")).Text);

                        viHiddenFaultStatusID.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridFaultStatusIDLabel")).Text);
                        viHiddenManningID.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridInspectorCodeLabel")).Text);
                        viHiddenFaultDirection.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridFaultDirectionLabel")).Text);
                        viHiddenUserDecision.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridFaultDirectionLabel")).Text);
                        viHiddenStatus.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridStatusLabel")).Text);
                        viHiddenReasonID.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridReasonIDLabel")).Text);
                        viHiddenWCID.Value = (((Label)gridViewRow.Cells[1].FindControl("viGridWCIDLabel")).Text);

                        fillReasonName(viWCIDLabel.Text.Trim());
                        fillUserDecision();

                        ScriptManager.RegisterClientScriptBlock(viMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForReview');", true);

                    }
                    else if (((ImageButton)sender).ID == "viGridEditImageButton")
                    {
                        //Code for editing gridview row
                    }
                    else if (((ImageButton)sender).ID == "visGridDeleteImageButton")
                    {
                        //Code for deleting gridview row
                    }
                    else { }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(viMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }
            }

            protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            viNotifyMessageDiv.Visible = false;
            viNotifyTimer.Enabled = false;
        }

            protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (((GridView)sender).ID == "viGridView")
                        {
                            Label gtBarcodeLabel = ((Label)e.Row.FindControl("viGridGTBarcodeLabel"));

                            GridView childGridView = ((GridView)e.Row.FindControl("viInnerGridView"));
                            fillChildGridView(childGridView, gtBarcodeLabel.Text.Trim());
                        }
                    }
                }
                catch (Exception xp)
                {
 
                }
            }

        #endregion

        #region User Defined Function

            private void fillWorkcenterName()
            {

                //Description   : Function for filling viWCNameDropDownList with Workcenter Name
                //Author        : Brajesh kumar   || Brajesh kumar                                              || Brajesh kumar
                //Date Created  : 30 March 2011                                                             
                //Date Updated  : 30 March 2011 || 01 April 2011                                            || 04 April 2011
                //Revision No.  : 01            || 02                                                       || 03
                //Revision Desc :               || Change the Logic for filling the viWCNameDropDownList    || Set The workcenter name by webservice

                viReviewDialogReasonNameDropDownList.Items.Clear();
                viReviewDialogReasonNameDropDownList.Items.Add("");

                viWCNameDropDownList.Items.Clear();
                viWCNameDropDownList.Items.Add("");

                viWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
                viWCNameDropDownList.DataBind();

                viWCNameDropDownList.SelectedValue = myWebService.setWorkCenterName("wcMaster", 89);

                //*************************Calling the Event Programtically**************************************//

                EventArgs viWCNameDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(viWCNameDropDownList, viWCNameDropDownListEventArgs);

                //***********************************************************************************************//

            }

            /// <summary>
            /// Function for retriving Reason Name according to a particular workcenter
            /// </summary>
            /// <param name="wcID">
            /// ID of workcenter
            /// </param>

            private void fillReasonName(string wcID)
            {
                //Description   : Function for filling viReasonNameDropDownList with Reason Name Workcenter wise
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 30 March 2011 
                //Date Updated  : 30 March 2011 || 01 April 2011
                //Revision No.  : 01            || 02
                //Revision Desc :               || Change the Logic for filling the xRayReasonNameDropDownList

                viReviewDialogReasonNameDropDownList.Items.Clear();
                viReviewDialogReasonNameDropDownList.Items.Add("");

                viReviewDialogReasonNameDropDownList.DataSource = myWebService.FillDropDownList("reasonMaster", "name", "WHERE wcID = " + wcID + "");
                viReviewDialogReasonNameDropDownList.DataBind();
            }

            private void fillStatus()
            {

                //Description   : Function for filling viStatusDropDownList with Status Name
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011 || 06 April 2011
                //Revision No.  : 01            || 02
                //Revision Desc :               || Adding the logic for filling viReviewDialogStatusDropDownList

                viStatusDropDownList.Items.Clear();
                viStatusDropDownList.Items.Add("");

                foreach (string item in myWebService.status)
                {
                    viStatusDropDownList.Items.Add(item);
                }
            }

            private void fillFaultStatus()
            {

                //Description   : Function for filling vifaultStatusDropDownList with Fault Status Name
                //Author        : Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011
                //Revision No.  : 01

                viFaultStatusDropDownList.Items.Clear();
                viFaultStatusDropDownList.Items.Add("");

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select name from defectstatusMaster";

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    viFaultStatusDropDownList.Items.Add(myConnection.reader[0].ToString());
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

            private void fillFaultDirection()
            {

                //Description   : Function for filling viFaultDirectionDropDownList with Fault Direction
                //Author        : Brajesh kumar
                //Date Created  : 31 March 2011
                //Date Updated  : 31 March 2011
                //Revision No.  : 01

                viFaultDirectionDropDownList.Items.Clear();
                viFaultDirectionDropDownList.Items.Add("");

                foreach (string item in myWebService.faultDirection)
                {
                    viFaultDirectionDropDownList.Items.Add(item);
                }
            }

            private void fillStatusSearch()
            {

                //Description   : Function for filling viStatusSearchDropDownList with Status Name
                //Author        : Brajesh kumar
                //Date Created  : 04 April 2011
                //Date Updated  : 04 April 2011
                //Revision No.  : 01

                viStatusSearchDropDownList.Items.Clear();
                viStatusSearchDropDownList.Items.Add("");

                foreach (string item in myWebService.status)
                {
                    viStatusSearchDropDownList.Items.Add(item);
                }
            }

            private void fillUserDecision()
            {

                //Description   : Function for filling viUserDecisionDropDownList with User Decision
                //Author        : Brajesh kumar
                //Date Created  : 05 March 2011
                //Date Updated  : 05 March 2011
                //Revision No.  : 01

                viReviewDialogUserDecisionDropDownList.Items.Clear();
                viReviewDialogUserDecisionDropDownList.Items.Add("");

                foreach (string item in myWebService.userDecision)
                {
                    viReviewDialogUserDecisionDropDownList.Items.Add(item);
                }
            }

            private void fillGridView()
            {

                //Description   : Function for filling viGridView
                //Author        : Brajesh kumar                       || Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011                     || 04 April 2011
                //Revision No.  : 01                                || 02
                //Revision Desc : Use class for filling gridview    || Changes for filling datagrid from webservice

                try
                {
                    viGridView.DataSource = myWebService.fillGridView("Select DISTINCT gtbarCode from vVisualization", ConnectionOption.SQL);
                    viGridView.DataBind();
                }
                catch(Exception exc)
                {

                }
                
            }

            /// <summary>
            /// Function for making a decision status of viGridView
            /// </summary>
            /// <param name="obj">Object for making decision</param>
            /// <returns></returns>
            /// 

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
            /// Function for displaying review option in viGridView
            /// </summary>
            /// <param name="obj">Object for enabling review option</param>
            /// <returns></returns>

            public bool displayReview(Object gtbarcodeObj, Object iD, Object statusObj)
            {

                //Description   : Function for displaying review option in xRayGridView
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 07 April 2011 ||
                //Date Updated  : 07 April 2011 || 13 April 2011                                || 18 April 2011
                //Revision No.  : 01            || 02                                           || 03
                //Revision Desc :               || Change the logic for display review button   || Change the logic for display review button

                bool flag = true;
                try
                {
                    if (!string.IsNullOrEmpty(gtbarcodeObj.ToString()))
                    {

                        if ((statusObj.ToString().Trim() == "0") || (statusObj.ToString().Trim() == "3"))
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Select gtbarCode from vVisualization WHERE gtbarCode = @gtbarCode";
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

                            myConnection.comm.CommandText = "Select MIN(iD), MAX(iD) from vVisualization WHERE gtbarCode = @gtbarCode";
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
                    
                }
                catch (Exception ex)
                {

                }
                return flag;
            }

            public bool displayReviewNotReqd(Object gtbarcodeObj)
            {

                //Description   : Function for displaying review option in viGridView
                //Author        : Brajesh kumar
                //Date Created  : 14 April 2011
                //Date Updated  : 14 April 2011
                //Revision No.  : 01
                //Revision Desc : 

                bool flag = false;
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select count(gtbarcode) from vVisualization WHERE gtbarCode = @gtbarCode";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

                    int db = Convert.ToInt32(myConnection.comm.ExecuteScalar());

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();

                    if (db == 1)
                    {
                        myConnection.comm.CommandText = "Select status from vVisualization WHERE gtbarCode = @gtBarCode2";
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
                }
                catch(Exception ex)
                {

                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                return flag;
            }

            public bool displayDelete(Object gtbarcodeObj, Object iD)
            {

                //Description   : Function for displaying delete option in viBalGridView
                //Author        : Brajesh kumar
                //Date Created  : 14 April 2011
                //Date Updated  : 14 April 2011
                //Revision No.  : 01
                //Revision Desc : 

                bool flag = true;
                try
                {

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select MIN(iD), MAX(iD) from vVisualization WHERE gtbarCode = @gtbarCode";
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtbarcodeObj.ToString());

                    myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);

                    while (myConnection.reader.Read())
                    {
                        if ((Convert.ToInt32(iD) == Convert.ToInt32(myConnection.reader[0].ToString())) || (Convert.ToInt32(iD) < Convert.ToInt32(myConnection.reader[1].ToString())))
                        {
                            flag = false;
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                return flag;
            }

            public string displayUserDecision(Object obj)
            {

                //Description   : Function for displaying user decision in viGridView
                //Author        : Brajesh kumar
                //Date Created  : 04 April 2011
                //Date Updated  : 04 April 2011
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
                //Description   : Function for saving and updating record in vInspectiontbr Table
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011 || 04 April 2011                                            || 18 April 2011
                //Revision No.  : 01            || 02                                                       || 03
                //Revision Desc :               || Change the coloum name from inspectorCode to manningID   || Remove User Decision Coloumn

                int flag = 0;
                try
                {
                    if (viIDLabel.Text.Trim() == "0")
                    {
                        if (validation() <= 0)
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into vInspectionTBR (wcID, gtbarCode, manningID, status, defectstatusID, faultDirection, reasonID, dtandTime) values (@wcID, @gtbarCode, @manningID, @status, @faultstatusID, @faultDirection, @reasonID, @dtandTime)";
                            myConnection.comm.Parameters.AddWithValue("@wcID", viWCIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@gtbarCode", viGTBarcodeTextBox.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@faultstatusID", viFaultStatusIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@faultDirection", viFaultDirectionDropDownList.SelectedValue.Trim());
                            myConnection.comm.Parameters.AddWithValue("@reasonID", viReviewDialogReasonIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                            myConnection.comm.Parameters.AddWithValue("@status", viStatusDropDownList.SelectedIndex);
                            myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now));

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Visualization record saved successfully");
                        }
                    }
                    else if (viIDLabel.Text.Trim() != "0")
                    {
                        if (validation() <= 0)
                        {
                            Notify(1, "Visualizations record updated successfully");
                        }
                    }
                }
                catch(Exception exp)
                {

                }
                return flag;
            }

            private int review()
            {
                //Description   : Function for saving reviewed record in vInspectiontbr Table
                //Author        : Brajesh kumar
                //Date Created  : 06 April 2011
                //Date Updated  : 06 April 2011
                //Revision No.  : 01           
                //Revision Desc :              

                int flag = 0;
                try
                {
                    if (viIDLabel.Text.Trim() == "0")
                    {
                        if (validationForReview() <= 0)
                        {
                            myConnection.open(ConnectionOption.SQL);
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into vInspectionTBR (wcID, gtbarCode, manningID, status, faultstatusID, reasonID, faultDirection, dtandTime) values (@wcID, @gtbarCode, @manningID, @status, @faultstatusID, @reasonID, @faultDirection, @dtandTime)";
                            myConnection.comm.Parameters.AddWithValue("@wcID", viWCIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@gtbarCode", viReviewDialogGTBarcodeLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@manningID", Session["ID"].ToString());
                            myConnection.comm.Parameters.AddWithValue("@status", myWebService.getStatusState(viReviewDialogUserDecisionDropDownList.SelectedValue.ToString().Trim()));
                            myConnection.comm.Parameters.AddWithValue("@faultstatusID", viHiddenFaultStatusID.Value.Trim());
                            myConnection.comm.Parameters.AddWithValue("@faultDirection", viHiddenFaultDirection.Value.Trim());
                            myConnection.comm.Parameters.AddWithValue("@reasonID", viReviewDialogReasonIDLabel.Text.Trim());
                            myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now));

                            flag = myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);

                            Notify(1, "Visualization record saved successfully");
                        }
                    }
                    else if (viIDLabel.Text.Trim() != "0")
                    {
                        if (validation() <= 0)
                        {
                            Notify(1, "Visualizations record updated successfully");
                        }
                    }
                }
                catch(Exception exp)
                {

                }

                return flag;
            }

            private int delete()
            {

                //Description   : Function for deleting record in vInspectionTBR Table
                //Author        : Brajesh kumar
                //Date Created  : 31 March 2011
                //Date Updated  : 31 March 2011
                //Revision No.  : 01

                int flag = 0;
                try
                {
                    if (viIDLabel.Text.Trim() != "0")
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Delete From vInspectionTBR WHERE (iD = @iD)";
                        myConnection.comm.Parameters.AddWithValue("@iD", viIDLabel.Text.Trim());

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);

                        Notify(1, "Visualization record deleted successfully");
                    }
                }
                catch(Exception exp)
                {

                }

                return flag;
            }

            public string isAuthenticate(string roleID)
            {
                return myWebService.authenticate(Session["userID"].ToString(), roleID).ToString();
            }

            private int validation()
            {

                //Description   : Function for validation of data for inserting and deleting record in vInspectionTBR Table
                //Author        : Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011
                //Revision No.  : 01
                //Revision Desc : 

                int flag = 0;

                if (viWCIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (viGTBarcodeTextBox.Text.Trim() == "")
                {
                    flag = 1;
                }

                if (viFaultStatusIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (viReviewDialogReasonIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }
                if (viFaultDirectionDropDownList.SelectedValue.Trim() == "")
                {
                    flag = 1;
                }
                if (viStatusDropDownList.SelectedValue.Trim() == "")
                {
                    flag = 1;
                }

                return flag;
            }

            private int validationForReview()
            {

                //Description   : Function for validation for review of data for inserting and deleting record in vInspectionTBR Table
                //Author        : Brajesh kumar
                //Date Created  : 06 April 2011
                //Date Updated  : 06 April 2011
                //Revision No.  : 01
                //Revision Desc : 

                int flag = 0;

                if (viWCIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (viReviewDialogReasonIDLabel.Text.Trim() == "0")
                {
                    flag = 1;
                }

                if (viReviewDialogUserDecisionDropDownList.SelectedValue.Trim() == "")
                {
                    flag = 1;
                }


                return flag;
            }

            private void clearPage()
            {

                //Description   : Function for clearing controls and variables of Page
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 30 March 2011
                //Date Updated  : 30 March 2011 || 31 March 2011
                //Revision No.  : 01            || 02
                //Revision Desc :               || Add the logic to clear the viFaultDirectionDropDownList

                viIDLabel.Text = "0";
                viGTBarcodeTextBox.Text = "";
                viFaultStatusIDLabel.Text = "0";
                viReviewDialogReasonIDLabel.Text = "0";

                fillWorkcenterName();
                fillFaultStatus();
                fillStatus();
                fillFaultDirection();
            }

            private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in xrayMessageDiv
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

            private void fillChildGridView(GridView childGridView, string gtBarcode)
            {

                if (childGridView.ID == "viInnerGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName as faultStatusCode, faultDirection, reasonID, reasonName, dtandTime from vVisualization WHERE gtbarCode = '" + gtBarcode + "'", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
            }

        #endregion
    }
}
