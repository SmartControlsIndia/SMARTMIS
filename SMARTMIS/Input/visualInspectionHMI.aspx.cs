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
    public partial class visualInspectionHMI : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region Variables

            private String _tableName;
            private String _viewName;

        #endregion

        #region Properties

        public String TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public String ViewName
        {
            get { return _viewName; }
            set { _viewName = value; }
        }

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    viHMIWCNameLabel.Text = myWebService.setWorkCenterName("wcMaster", Convert.ToInt32(Request.QueryString[0].Trim()));

                    fillDefectCode();
                    fillRepair();
                    fillBuff();
                    fillHold();
                }

                this.TableName = Request.QueryString[1].Trim();

                if (this.TableName.ToUpper().Contains("PCR"))
                    this.ViewName = "vVisualInspectionPCR";
                else
                    this.ViewName = "vVisualInspectionTBR";


                viHMIGTBarcodeTextBox.Focus();
            }
            catch(Exception ex)
            {
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "viHMIStatusOKButton")
                {
                    viHMIGTBarcodeCodeHidden.Value = viHMIGTBarcodeTextBox.Value;
                    viHMIDefectCodeHidden.Value = "1";
                    viHMIDefectLocationHidden.Value = "1";
                    viHMIStatusHidden.Value = "OK";
                    viHMITimerStatusHidden.Value = "0";

                    TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), viHMIGTBarcodeCodeHidden.Value.Trim(), "12", "83", 1, Session["ID"].ToString(), Array.IndexOf(myWebService.status, viHMIStatusHidden.Value.Trim()), Convert.ToDateTime(DateTime.Now));

                    GetLastGTHistory(viHMIGTBarcodeCodeHidden.Value.Trim());

                    viHMIGTHistoryGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName, defectLocation, dtandTime from " + this.ViewName + " WHERE gtbarCode = '" + viHMIGTBarcodeCodeHidden.Value.Trim() + "'", ConnectionOption.SQL);
                    viHMIGTHistoryGridView.DataBind();


                    clearPage();
                    //viTimer.Enabled = true;
                }
                else if (((Button)sender).ID == "viHMICancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "viHMIGTHistoryImageButton")
                {
                    viHMIGTHistoryGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName, defectLocation, dtandTime from " + this.ViewName + " WHERE gtbarCode = '" + viHMIGTBarcodeCodeHidden.Value.Trim() + "'", ConnectionOption.SQL);
                    viHMIGTHistoryGridView.DataBind();

                    if (viHMIGTBarcodeCodeHidden.Value.Trim() == "")
                        viHMIGTBarcodeTextBox.Focus();
                    else
                        viHMIStatusOKButton.Focus();
                }
                else if (((Button)sender).ID == "viHMISaveButton")
                {
                    viHMIGTBarcodeCodeHidden.Value = viHMIGTBarcodeTextBox.Value;
                    viHMIStatusHidden.Value = "Not OK";

                    TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), viHMIGTBarcodeCodeHidden.Value.Trim(), viHMIDefectCodeHidden.Value, viHMIDefectLocationHidden.Value, 1, Session["ID"].ToString(), Array.IndexOf(myWebService.status, viHMIStatusHidden.Value.Trim()), Convert.ToDateTime(DateTime.Now));

                    GetLastGTHistory(viHMIGTBarcodeCodeHidden.Value.Trim());

                    viHMIGTHistoryGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName, defectLocation, dtandTime from " + this.ViewName + " WHERE gtbarCode = '" + viHMIGTBarcodeCodeHidden.Value.Trim() + "'", ConnectionOption.SQL);
                    viHMIGTHistoryGridView.DataBind();

                    clearPage();
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                viHMIGTHistoryGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName, defectLocation, dtandTime from " + this.ViewName + " WHERE gtbarCode = '" + viHMIGTBarcodeCodeHidden.Value.Trim() + "'", ConnectionOption.SQL);
                viHMIGTHistoryGridView.DataBind();

                viHMIGTBarcodeTextBox.Focus();
            }
            catch(Exception exp)
            {

            }
        }

        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "viHMIGTHistoryGridView")
                {
                    Label gtBarcodeLabel = ((Label)e.Row.FindControl("viHMIGridGTBarcodeLabel"));

                    GridView childGridView = ((GridView)e.Row.FindControl("viHMIGTHistoryInnerGridView"));
                    fillChildGridView(childGridView, gtBarcodeLabel.Text.Trim());
                }
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            if (((Timer)sender).ID == "viTimer")
            {
                if (viHMITimerStatusHidden.Value == "0")
                {
                    TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), viHMIGTBarcodeCodeHidden.Value.Trim(), "1", "1", 1, Session["ID"].ToString(), Array.IndexOf(myWebService.status, viHMIStatusHidden.Value.Trim()), Convert.ToDateTime(DateTime.Now));
                    GetLastGTHistory(viHMIGTBarcodeCodeHidden.Value.Trim());
                    clearPage();
                }
                viTimer.Enabled = false;
            }
        }

        #endregion

        #region User Defined Functions

        private TyreType getTyreType(string gtBarcode)
        {
            TyreType flag;
            int notifyIcon = 0;

            if (myWebService.IsRecordExist("tbmtbr", "gtbarCode", "WHERE gtbarCode = '" + gtBarcode + "'", out notifyIcon) == true)
                flag = TyreType.TBR;
            else if (myWebService.IsRecordExist("tbmpcr", "gtbarCode", "WHERE gtbarCode = '" + gtBarcode + "'", out notifyIcon) == true)
                flag = TyreType.PCR;
            else
                flag = TyreType.None;

            return flag;
        }

        private void fillDefectCode()
        {

            //Description   : Function for filling viHMIGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01


            viHMIDefectCodeListView.GroupItemCount = 1;

            //int flag = myWebService.RecordCount("defectstatusMaster", "name", "");

            //if (flag < 8)
            //{
            //    viHMIDefectCodeListView.GroupItemCount = 2;
            //}
            //else if ((flag >= 8) && (flag <= 12))
            //{
            //    viHMIDefectCodeListView.GroupItemCount = 3;
            //}
            //else
            //{
            //    viHMIDefectCodeListView.GroupItemCount = 4;
            //}
            try
            {

                viHMIDefectCodeListView.DataSource = myWebService.fillGridView("Select iD, name from defectstatusMaster WHERE name <> 'N/A' ORDER BY iD", ConnectionOption.SQL);
                viHMIDefectCodeListView.DataBind();
            }
            catch(Exception exp)
            {

            }
        }

        private void fillRepair()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            viHMIRepairListView.GroupItemCount = 5;

            //int flag = myWebService.RecordCount("defectLocationMaster", "name", "");

            //if (flag < 8)
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 2;
            //}
            //else if ((flag >= 8) && (flag <= 12))
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 3;
            //}
            //else
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 4;
            //}
            try
            {
                viHMIRepairListView.DataSource = myWebService.fillGridView("Select iD, name from defectLocationMaster WHERE defectStatusID = 1 ORDER BY iD", ConnectionOption.SQL);
                viHMIRepairListView.DataBind();
            }
            catch(Exception exp)
            {

            }
        }

        private void fillBuff()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            viHMIBuffListView.GroupItemCount = 5;

            //int flag = myWebService.RecordCount("defectLocationMaster", "name", "");

            //if (flag < 8)
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 2;
            //}
            //else if ((flag >= 8) && (flag <= 12))
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 3;
            //}
            //else
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 4;
            //}
            try
            {
                viHMIBuffListView.DataSource = myWebService.fillGridView("Select iD, name from defectLocationMaster WHERE defectStatusID = 2 ORDER BY iD", ConnectionOption.SQL);
                viHMIBuffListView.DataBind();
            }
            catch(Exception ex)
            {
            }
        }

        private void fillHold()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            viHMIHoldListView.GroupItemCount = 5;

            //int flag = myWebService.RecordCount("defectLocationMaster", "name", "");

            //if (flag < 8)
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 2;
            //}
            //else if ((flag >= 8) && (flag <= 12))
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 3;
            //}
            //else
            //{
            //    viHMIDefectLocationListView.GroupItemCount = 4;
            //}
            try
            {

                viHMIHoldListView.DataSource = myWebService.fillGridView("Select iD, name from defectLocationMaster WHERE defectStatusID = 3 ORDER BY iD", ConnectionOption.SQL);
                viHMIHoldListView.DataBind();
            }
            catch(Exception exp)
            { 
            }
        }

        private void fillChildGridView(GridView childGridView, string gtBarcode)
        {

            if (childGridView.ID == "viHMIGTHistoryInnerGridView")
            {
                try
                {
                    childGridView.DataSource = myWebService.fillGridView("Select iD, wcID, wcName , gtbarCode, manningID, sapCode, status, defectstatusID, defectStatusName, defectLocation, dtandTime from " + this.ViewName + " WHERE gtbarCode = '" + gtBarcode + "'", ConnectionOption.SQL);
                    childGridView.DataBind();
                }
                catch(Exception exp)
                {
                }

            }
        }

        public string displayStatus(Object obj)
        {

            //Description   : Function for making a decision status of viHMIGTHistoryGridView
            //Author        : Brajesh kumar
            //Date Created  : 29 August 2012
            //Date Updated  : 29 August 2012
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                flag = myWebService.statusState[Convert.ToInt32(obj)];
            }
            return flag;
        }

        private int TransferData(String tableName, int wcID, String gtBarcode, String defectStatusID, String defectLocationID, int reasonID, String manningID, int status, DateTime dtandTime)
        {
            //Description   : Function for saving and updating record in vInspectiontbr Table
            //Author        : Brajesh kumar
            //Date Created  : 12 October 2011
            //Date Updated  : 12 October 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            int tempValidation = validation();

            if (tempValidation <= 0)
            {
               //int notifyIcon = 0;

                //if (myWebService.IsRecordExist(TableName, "gtbarCode", "WHERE gtbarCode = '" + viHMIGTBarcodeTextBox.Value.Trim() + "'", out notifyIcon) == true)
                //{
                //    Notify(0, viHMIGTBarcodeTextBox.Value.Trim() + " already exists");
                //}
                //else
                //{
                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into " + this.TableName + " (wcID, gtbarCode, manningID, status, defectstatusID, defectLocationID, reasonID, dtandTime) values (@wcID, @gtbarCode, @manningID, @status, @defectstatusID, @defectLocationID, @reasonID, @dtandTime)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", wcID);
                    myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);
                    myConnection.comm.Parameters.AddWithValue("@defectstatusID", defectStatusID);
                    myConnection.comm.Parameters.AddWithValue("@defectLocationID", defectLocationID);
                    myConnection.comm.Parameters.AddWithValue("@reasonID", reasonID);
                    myConnection.comm.Parameters.AddWithValue("@manningID", manningID);
                    myConnection.comm.Parameters.AddWithValue("@status", status);
                    myConnection.comm.Parameters.AddWithValue("@dtandTime", dtandTime);

                    flag = myConnection.comm.ExecuteNonQuery();
                    Notify(1, "Visualization record saved successfully");

                    GC.Collect();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }

                 
                //}
            }
            else if (tempValidation == 1)
            {
                Notify(0, "Invalid Workcenter Name");
            }
            else if (tempValidation == 2)
            {
                Notify(0, "Invalid GT Barcode :" + viHMIGTBarcodeTextBox.Value.Trim());
            }
            else if (tempValidation == 3)
            {
                Notify(0, "Invalid Defect");
            }
            else if (tempValidation == 4)
            {
                Notify(0, "Invalid Defect Code");
            }

            return flag;
        }

        private void GetLastGTHistory(String gtBarcode)
        {
            TyreType tyreType = getTyreType(gtBarcode);

            viLastGTBarcodeLabel.Text = gtBarcode;
            viTBMWCNameLabel.Text = "";
            viCuringWCNameLabel.Text = "";

            myConnection.open(ConnectionOption.SQL);

            if (tyreType == TyreType.TBR)
            {
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName from vtbmtbr WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    viTBMWCNameLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();

                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName from vcuringtbr WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    viCuringWCNameLabel.Text = myConnection.reader[0].ToString();
                }
                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();
            }
            else if (tyreType == TyreType.PCR)
            {
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName from vtbmpcr WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    viTBMWCNameLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();

                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName from vcuringpcr WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    viCuringWCNameLabel.Text = myConnection.reader[0].ToString();
                }
                myConnection.reader.Close();
                myConnection.reader.Dispose();
                myConnection.comm.Dispose();
            }

            myConnection.close(ConnectionOption.SQL);
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in wcMessageDiv
            //Author        : Brajesh kumar
            //Date Created  : 28 February 2012
            //Date Updated  : 28 February 2012
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
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in vInspectionTBR Table
            //Author        : Brajesh kumar
            //Date Created  : 12 October 2011
            //Date Updated  : 12 October 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (viHMIWCNameLabel.Text.Trim() == "")
            {
                flag = 1;
            }

            if ((viHMIGTBarcodeCodeHidden.Value.Trim() == "") || (viHMIGTBarcodeCodeHidden.Value.Length != 10))
            {
                flag = 2;
            }

            if (viHMIDefectCodeHidden.Value.Trim() == "0")
            {
                flag = 3;
            }

            if (viHMIDefectLocationHidden.Value.Trim() == "0")
            {
                flag = 4;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh kumar
            //Date Created  : 12 October 2011
            //Date Updated  : 12 October 2011
            //Revision No.  : 01
            //Revision Desc : 

            viTimer.Enabled = false;

            viHMIGTBarcodeTextBox.Value = "";
            viHMIGTBarcodeCodeHidden.Value = "";
            viHMIStatusHidden.Value = "";
            viHMIDefectCodeHidden.Value = "0";
            viHMIDefectLocationHidden.Value = "0";
            viHMIStatusHidden.Value = "0";

        }

        #endregion
    }
}
