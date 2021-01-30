using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Data;
using SmartMIS.SmartWebReference;

namespace SmartMIS.Master
{
    public partial class manPower1 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    fillGridView();
                }
            }

            protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "manPowerSapCodeGridView")
                    {
                        Label sapCode = ((Label)e.Row.FindControl("manPowerGridSapCodeLabel"));

                        //string status = attendanceStatus(sapCode.Text);

                      //  ((Image)e.Row.FindControl("manPowerGridStatusImage")).ImageUrl = myWebService.attendanceImagePath[attendanceImage(status)];

                        GridView childGridView = ((GridView)e.Row.FindControl("manPowerInnerGridView"));
                        fillChildGridView(childGridView, sapCode.Text.Trim());
                    }
                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                if (((Button)sender).ID == "manPowerReviewDialogOKButton")
                {
                    //*************************Calling the Event Programtically**************************************//

                    EventArgs manPowerDropDownListEventArgs = new EventArgs();

                    DropDownList_SelectedIndexChanged(manPowerWCNameDropDownList, manPowerDropDownListEventArgs);
                    DropDownList_SelectedIndexChanged(manPowerShiftDropDownList, manPowerDropDownListEventArgs);

                    //***********************************************************************************************//

                    if (save(manPowerWCIDLabel.Text.Trim(), manPowerShiftDropDownList.SelectedItem.Text) > 0)
                    {
                        fillGridView();
                    }
                }
                else if (((Button)sender).ID == "manPowerDialogOKButton")
                {
                    delete(manPowerIDHidden.Value.Trim());
                    fillGridView();
                }
            }

            protected void ImageButton_Click(object sender, ImageClickEventArgs e)
            {
                if (myWebService.authenticate(Session["userID"].ToString(), (((ImageButton)sender).AlternateText).Trim()) == true)
                {
                    if (((ImageButton)sender).ID == "manPowerGridAddImageButton")
                    {
                        clearPage();

                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        manPowerManningIDHidden.Value = (((Label)gridViewRow.Cells[1].FindControl("manPowerGridManningIDLabel")).Text);

                        fillWorkcenter();
                        fillShift();


                        ScriptManager.RegisterClientScriptBlock(manPowerMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForReview');", true);
                    }
                    else if (((ImageButton)sender).ID == "manPowerInnerGridDeleteImageButton")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                        GridView tempGridView = (GridView)gridViewRow.Cells[1].FindControl("manPowerInnerGridView");

                        manPowerIDHidden.Value = innerGridID(tempGridView);

                        ScriptManager.RegisterClientScriptBlock(manPowerMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPage');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(manPowerMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                }
            }

            protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (((DropDownList)sender).ID == "manPowerWCNameDropDownList")
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                    myConnection.comm.Parameters.AddWithValue("@name", ((DropDownList)sender).SelectedItem.ToString().Trim());

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    while (myConnection.reader.Read())
                    {
                        manPowerWCIDLabel.Text = myConnection.reader[0].ToString();
                    }

                    myConnection.reader.Close();
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }

        #endregion

        #region User Defined Function

            private void fillGridView()
                {

                    //Description   : Function for filling manPowerSapCodeGridView with SAP Code
                    //Author        : Brajesh kumar
                    //Date Created  : 10 June 2011
                    //Date Updated  : 10 June 2011
                    //Revision No.  : 01

                    manPowerSapCodeGridView.DataSource = myWebService.fillGridView("Select * from vManning ORDER BY sapCode", ConnectionOption.SQL);
                    manPowerSapCodeGridView.DataBind();
                }

            private void fillChildGridView(GridView childGridView, string sapCode)
            {

                if (childGridView.ID == "manPowerInnerGridView")
                {
                    childGridView.DataSource = myWebService.fillGridView("Select iD, wcID, name, shiftName from vManPowerMaster WHERE sapCode = '" + sapCode + "'", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
            }

            private void fillWorkcenter()
            {

                //Description   : Function for filling manPowerWCNameDropDownList with Workcenter Name
                //Author        : Brajesh kumar
                //Date Created  : 11 June 2011
                //Date Updated  : 11 June 2011
                //Revision No.  : 01

                manPowerWCNameDropDownList.Items.Clear();

                manPowerWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
                manPowerWCNameDropDownList.DataBind();
            }

            private void fillShift()
            {

                //Description   : Function for filling manPowerShiftDropDownList with Shift
                //Author        : Brajesh kumar
                //Date Created  : 11 June 2011
                //Date Updated  : 11 June 2011
                //Revision No.  : 01

                manPowerShiftDropDownList.Items.Clear();

                manPowerShiftDropDownList.DataSource = myWebService.FillDropDownList("shiftMaster", "shiftName");
                manPowerShiftDropDownList.DataBind();
                
            }

            private int save(string wcID, string shiftName)
            {
                //Description   : Function for saving and updating record in manPowerMaster Table
                //Author        : Brajesh kumar   || Brajesh kumar
                //Date Created  : 11 June 2011  ||
                //Date Updated  : 11 June 2011  || 13 June 2011 
                //Revision No.  : 01            || 02
                //Revision Desc :               || Change the logic for checkin whether the existing data is present or not

                int flag = 0;

                if (wcID == "0")
                {
                }
                else
                {
                    string shiftID = myWebService.getID("shiftMaster", "shiftName", shiftName);

                    if (recordExists(manPowerManningIDHidden.Value.Trim(), manPowerWCIDLabel.Text.Trim(), shiftID) == true)
                    {
                        //Show custom message in this block
                    }
                    else
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Insert into manPowerMaster (manningID, wcID, shiftID, dtandTime) values (@manningID, @wcID, @shiftID, @dtandTime)";
                        myConnection.comm.Parameters.AddWithValue("@manningID", manPowerManningIDHidden.Value.Trim());
                        myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(manPowerWCIDLabel.Text.Trim()));
                        myConnection.comm.Parameters.AddWithValue("@shiftID", Convert.ToInt32(shiftID));
                        myConnection.comm.Parameters.AddWithValue("@dtandTime", Convert.ToDateTime(DateTime.Now));

                        flag = myConnection.comm.ExecuteNonQuery();

                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }

                }



                return flag;
            }

            private void clearPage()
            {

                //Description   : Function for clearing controls and variables of Page
                //Author        : Brajesh kumar
                //Date Created  : 11 June 2011
                //Date Updated  : 11 June 2011
                //Revision No.  : 01


                manPowerManningIDHidden.Value = "";
            }

            public string isAuthenticate(string roleID)
            {
                return myWebService.authenticate(Session["userID"].ToString(), roleID).ToString();
            }

            private int delete(string ID)
            {

                //Description   : Function for deleting record in manPowerMaster Table
                //Author        : Brajesh kumar       || Brajesh kumar
                //Date Created  : 11 June 2011      ||
                //Date Updated  : 11 June 2011      || 13 June 2011
                //Revision No.  : 01                || 02
                //Revision Desc :                   || Logic Changed for multiple deletion in manPowerInnerGridView

                int flag = 0;

                if (manPowerIDHidden.Value.Trim() != "0")
                {

                    string query = "";
                    string or = "";
                    string[] tempID = ID.Split(new char[] { '#' });

                    foreach (string items in tempID)
                    {
                        if (items.Trim() != "")
                        {
                            query = query + or + "iD = '" + items + "'";
                            or = " Or ";
                        }

                    }

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Delete From manPowerMaster WHERE " + query;

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

                return flag;
            }

            private string innerGridID(GridView childGridView)
            {
                string flag = "";

                foreach (GridViewRow row in childGridView.Rows)
                {
                    CheckBox tempCheckbox = (CheckBox)row.FindControl("manPowerInnerGridCheckBox");

                    //Check if the checkbox is checked.
                    if (tempCheckbox.Checked)
                    {
                        flag += ((Label)row.FindControl("manPowerInnerGridIDLabel")).Text + "#";
                    }
                }

                return flag;
            }

            //private string attendanceStatus(string eCode)
            //{
            //    string flag = myWebService.attendanceKeyWord[0].ToString();

            //    string tempFlag_1 = myWebService.attendanceKeyWord[0].ToString();
            //    string tempFlag_2 = myWebService.attendanceKeyWord[0].ToString();

            //    myConnection.open(ConnectionOption.MySQL);
            //    myConnection.msComm = myConnection.msConn.CreateCommand();

            //    myConnection.msComm.CommandText = "Select Status1, Status2 from arpit_mondata WHERE (ECODE = @ECODE AND POST_DATE = @POST_DATE)";
            //    myConnection.msComm.Parameters.AddWithValue("@ECODE", eCode);
            //    myConnection.msComm.Parameters.AddWithValue("@POST_DATE", DateTime.Today);

            //    myConnection.msReader = myConnection.msComm.ExecuteReader();

            //    while (myConnection.msReader.Read())
            //    {
            //        tempFlag_1 = myConnection.msReader[0].ToString();
            //        tempFlag_2 = myConnection.msReader[1].ToString();

            //        if ((tempFlag_1 == myWebService.attendanceKeyWord[0].ToString()) || (tempFlag_2 == myWebService.attendanceKeyWord[1].ToString()))
            //        {
            //            flag = myWebService.attendanceKeyWord[1].ToString();
            //        }
            //    }

            //    myConnection.msComm.Dispose();
            //    myConnection.close(ConnectionOption.MySQL);

            //    return flag;
            //}

            private int attendanceImage(string attendanceStatus)
            {
                return Array.IndexOf(myWebService.attendanceKeyWord, attendanceStatus);
            }

            private bool recordExists(string manningID, string wcID, string shiftID)
            {
                bool flag = false;

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from manPowerMaster WHERE (manningID = @manningID AND wcID = @wcID And shiftID = @shiftID)";
                myConnection.comm.Parameters.AddWithValue("@manningID", manningID);
                myConnection.comm.Parameters.AddWithValue("@wcID", wcID);
                myConnection.comm.Parameters.AddWithValue("@shiftID", shiftID);

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = true;
                }

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                return flag;
            }

        #endregion
    }
}
