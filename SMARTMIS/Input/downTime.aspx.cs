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
    public partial class downTime : System.Web.UI.Page
    {

        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                fillWCGridView();
                if (!Page.IsPostBack)
                {
                    
                }
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "downTimeDialogOKButton")
            {
                downTimeIDLabel.Text = downTimeIDHidden.Value; //Passing value to downTimeIDLabel because on postback hidden field retains its value
                delete();
                clearPage();

                //*************************Calling the Events Programtically**************************************//

                EventArgs magicButtonEventArgs = new EventArgs();
                magicButton_Click(magicButton, magicButtonEventArgs);
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (myWebService.authenticate(Session["userID"].ToString(), (((ImageButton)sender).AlternateText).Trim()) == true)
            {
                if (((ImageButton)sender).ID == "downTimeAddImageButton")
                {
                    //Code for adding reason

                    double dtDuration = 0;
                    
                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    string downTimeEventID = (((Label)gridViewRow.Cells[1].FindControl("downTimeChildIDLabel"))).Text.Trim();
                    string reasonID = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonIDLabel"))).Text.Trim();

                    string totalDuration = (((Label)gridViewRow.Cells[1].FindControl("downTimeDurationLabel"))).Text.Trim();
                    string leftDuration = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonLeftDurationLabel"))).Text.Trim();

                    string duration = (((TextBox)gridViewRow.Cells[1].FindControl("downTimeDurationTextBox"))).Text.Trim();
                    int durationType = (((DropDownList)gridViewRow.Cells[1].FindControl("downTimeDurationDropDownList"))).SelectedIndex;

                    if (leftDuration.Trim() == "")
                    {
                        leftDuration = "0";
                    }

                    if (durationType == 0)
                    {
                        dtDuration = Convert.ToDouble(duration);
                    }
                    else if (durationType == 1)
                    {
                        dtDuration = Convert.ToDouble(duration) * 60;
                    }
                    else if (durationType == 2)
                    {
                        dtDuration = Convert.ToDouble(duration) * 1440;
                    }

                    if (Convert.ToDouble(totalDuration) < (Convert.ToDouble(leftDuration) + Convert.ToDouble(dtDuration)))
                    {
                        ScriptManager.RegisterClientScriptBlock(magicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAlert');", true);
                    }
                    else
                    {
                        addReason(downtimeReasonID(), downTimeEventID, Convert.ToInt32(dtDuration), Convert.ToInt32(reasonID));

                        //*************************Calling the Events Programtically**************************************//

                        EventArgs magicButtonEventArgs = new EventArgs();
                        magicButton_Click(magicButton, magicButtonEventArgs);
                    }
                }
                else if (((ImageButton)sender).ID == "downTimeCancelImageButton")
                {
                    //Code for canceling gridview row

                    GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                    DropDownList reasonDropDownList = (((DropDownList)gridViewRow.Cells[1].FindControl("downTimeReasonIDDropDownList")));
                    Label reasonID = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonIDLabel")));
                    Label reasonDesc = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonDescLabel")));
                    TextBox duration = (((TextBox)gridViewRow.Cells[1].FindControl("downTimeDurationTextBox")));
                    DropDownList reasonDurationDropDownList = (((DropDownList)gridViewRow.Cells[1].FindControl("downTimeDurationDropDownList")));

                    reasonDropDownList.SelectedIndex = 0;
                    reasonDesc.Text = "";
                    reasonID.Text = "0";
                    duration.Text = "";
                    reasonDurationDropDownList.SelectedIndex = 0;
                    

                }
                else if (((ImageButton)sender).ID == "visGridDeleteImageButton")
                {
                    //Code for deleting gridview row
                }
                else { }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(magicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int reasonID = 0;
            string description = "";

            GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((DropDownList)sender).Parent).Parent;

            DropDownList dropDown = (((DropDownList)gridViewRow.Cells[1].FindControl("downTimeReasonIDDropDownList")));
            Label idLabel = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonIDLabel")));
            Label wcIDLabel = (((Label)gridViewRow.Cells[1].FindControl("downTimeChildWCIDLabel")));
            Label descLabel = (((Label)gridViewRow.Cells[1].FindControl("downTimeReasonDescLabel")));


            getReasonID(dropDown.SelectedValue, Convert.ToInt32(wcIDLabel.Text.Trim()), ref reasonID, ref description);

            idLabel.Text = reasonID.ToString();
            descLabel.Text = description;
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "downTimeGridView")
                {
                    Label wcIDLabel = ((Label)e.Row.FindControl("downTimeWCIDLabel"));
                    GridView childGridView = ((GridView)e.Row.FindControl("downTimeChildGridView"));

                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()));
                }
                else if (((GridView)sender).ID == "downTimeChildGridView")
                {
                    Label iDLabel = ((Label)e.Row.FindControl("downTimeChildIDLabel"));
                    Label wcIDLabel = ((Label)e.Row.FindControl("downTimeChildWCIDLabel"));
                    Label downEventLabel = ((Label)e.Row.FindControl("downTimedownEventLabel"));
                    Label upEventLabel = ((Label)e.Row.FindControl("downTimeUpEventLabel"));

                    GridView childGridView = ((GridView)e.Row.FindControl("downTimeChildReasonGridView"));

                    DropDownList childDropDownList = ((DropDownList)e.Row.FindControl("downTimeReasonIDDropDownList"));
                    DropDownList childDurationDropDownList = ((DropDownList)e.Row.FindControl("downTimeDurationDropDownList"));

                    fillChildGridView(childGridView, Convert.ToInt32(wcIDLabel.Text.Trim()), downEventLabel.Text.Trim(), upEventLabel.Text.Trim(), iDLabel.Text.Trim());

                    fillReasonDropDown(childDropDownList, Convert.ToInt32(wcIDLabel.Text.Trim()));
                    fillDurationDropDown(childDurationDropDownList);
                }
            }
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            string rWCID, rChoice, rFromDate, rToDate, rType;

            string[] tempString = magicHidden.Value.Split(new char[] { '?' });

            downTimeGridView.DataSource = null;
            downTimeGridView.DataBind();

            //Compare the hidden field if it contains the query string or not

            if (tempString.Length > 1)
            {
                rWCID = tempString[0];
                rChoice = tempString[1];
                rFromDate = tempString[2];
                rToDate = tempString[3];
                rType = tempString[4];

                //  Compare choice of report user had selected//
                //
                //  Range = 0
                //  Pending = 1
                //

                if (rChoice == "0")
                {
                    string query = myWebService.createQuery(rWCID, rFromDate, rToDate, "downdtandTime", "downdtandTime");
                    showDownTime(query);
                }
                else if (rChoice == "1")
                {
                }
                else if (rChoice == "2")
                {
                }

            }

        }

        private void fillWCGridView()
        {

            //Description   : Function for filling downTimeWCGridView
            //Author        : Brajesh kumar 
            //Date Created  : 05 May 2011
            //Date Updated  : 05 May 2011
            //Revision No.  : 01
            //Revision Desc :

            downTimeWCGridView.DataSource = myWebService.fillGridView("Select iD, name from wcMaster", ConnectionOption.SQL);
            downTimeWCGridView.DataBind();
        }

        private void fillGridView(string query)
        {

            //Description   : Function for filling downTimeGridView
            //Author        : Brajesh kumar   ||  Brajesh kumar 
            //Date Created  : 05 May 2011   ||  08 May 2011
            //Date Updated  : 05 May 2011   ||  08 May 2011
            //Revision No.  : 01            ||  02
            //Revision Desc :               || Change the name of the view for fecthing records

            downTimeGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            downTimeGridView.DataBind();
        }

        private void fillChildGridView(GridView childGridView, int wcID)
        {

            if (childGridView.ID == "downTimeChildGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("Select DISTINCT iD, wcID, downEvent, convert(varchar(8), downdtandtime, 114) as downdtandtime, upEvent, convert(varchar(8), updtandtime, 114) as updtandtime, duration from vDownTimeEvents WHERE (WCID = " + wcID + ")", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        private void fillChildGridView(GridView childGridView, int wcID, string downEvent, string upEvent, string iD)
        {

            if (childGridView.ID == "downTimeChildReasonGridView")
            {
                childGridView.DataSource = myWebService.fillGridView("SELECT DISTINCT reasonID, downTimeReasonID, reasonName, description, downDuration FROM vDownTimeReason WHERE (WCID = " + wcID + " AND downEvent = '" + downEvent + "' AND upEvent = '" + upEvent + "' AND iD = '" + iD + "')", ConnectionOption.SQL);
                childGridView.DataBind();
            }
        }

        private void fillReasonDropDown(DropDownList downtimeReasonDropDownList, int wcID)
        {

            downtimeReasonDropDownList.Items.Clear();
            downtimeReasonDropDownList.Items.Add("");
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT name FROM dbo.reasonMaster WHERE (wcID = " + wcID + ")";
            myConnection.comm.Connection = myConnection.conn;

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                downtimeReasonDropDownList.Items.Add(myConnection.reader[0].ToString());
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

        }

        private void fillDurationDropDown(DropDownList downtimeDurationDropDownList)
        {

            downtimeDurationDropDownList.Items.Clear();

            foreach (string item in myWebService.duration)
            {
                downtimeDurationDropDownList.Items.Add(item);
            }

        }

        /// <summary>
        /// Function for displaying leftDuration for reasons
        /// </summary>
        /// <param name="objWCID">Object for WCID</param>
        /// <param name="objDownEvent">Object for Down Event Name</param>
        /// <param name="objUpEvent">Object for Up Event Name</param>
        /// <returns></returns>

        public string displayLeftDuration(Object objWCID, Object objID)
        {

            //Description   : Function for displaying leftDuration for reasons
            //Author        : Brajesh kumar
            //Date Created  : 07 May 2011
            //Date Updated  : 07 May 2011
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT DISTINCT SUM(downDuration) AS leftTime FROM dbo.vDownTimeReason WHERE (wcID = " + Convert.ToInt32(objWCID) + ") AND (iD = '" + Convert.ToString(objID) + "')";
            myConnection.comm.Connection = myConnection.conn;

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                flag = myConnection.reader[0].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        private void getReasonID(string reasonName, int wcID, ref int reasonID, ref string description)
        {

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT iD, name,  description, wcID FROM dbo.reasonMaster WHERE (name = '" + reasonName + "') AND (wcID = " + wcID + ")";
            myConnection.comm.Connection = myConnection.conn;

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
               reasonID = Convert.ToInt32(myConnection.reader[0].ToString());
               description = myConnection.reader[2].ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

        }

        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }

        public string isAuthenticate(string roleID)
        {
            return myWebService.authenticate(Session["userID"].ToString(), roleID).ToString();
        }

        private int addReason(string iD, string downEventID, int duration, int reasonID)
        {
            int flag = 0;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Insert into downtimeReason (iD, downtimeeventID, downDuration, reasonID) values (" + iD + ", '" + downEventID + "', " + duration + ", " + reasonID + ")";

            flag = myConnection.comm.ExecuteNonQuery();

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        protected void showDownTime(string query)
        {
            fillGridView("Select DISTINCT wcID, name from vDownTimeEvents WHERE " + query + "");
        }

        private string downtimeReasonID()
        {
            string flag = "0";

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();
            myConnection.comm.CommandText = "SELECT iD FROM downtimeReason ORDER BY CAST(ID AS INT)";
            myConnection.comm.Connection = myConnection.conn;

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                flag = (Convert.ToInt32(myConnection.reader[0]) + 1).ToString();
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);

            return flag;

        }

        private int delete()
        {

            //Description   : Function for deleting record in downtimeReason Table
            //Author        : Brajesh kumar
            //Date Created  : 07 May 2011
            //Date Updated  : 07 May 2011
            //Revision No.  : 01

            int flag = 0;

            if (downTimeIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From downtimeReason WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", downTimeIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                //Notify(1, "Visualization record deleted successfully");
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh kumar 
            //Date Created  : 07 May 2011
            //Date Updated  : 07 May 2011
            //Revision No.  : 01
            //Revision Desc :

            downTimeIDLabel.Text = "0";
            downTimeIDHidden.Value = "";
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             