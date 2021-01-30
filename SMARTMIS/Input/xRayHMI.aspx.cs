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
    public partial class xRayHMI : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region Variables

        private String _tableName;
        private String _gtBarcode;

        #endregion

        #region Properties

        public String TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public String GTBarcode
        {
            get { return _gtBarcode; }
            set { _gtBarcode = value; }
        }

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {

                    xRayHMIWCNameLabel.Text = myWebService.setWorkCenterName("wcMaster", Convert.ToInt32(Request.QueryString[0].Trim()));

                    fillDefectCode();
                    fillBodyPly();
                    fillBelt();
                    fillBead();
                    fillSteelChipper();
                    fillOther();

                }

                this.TableName = Request.QueryString[1].Trim();
                this.GTBarcode = Request.QueryString[2].Trim();

                xRayHMIGTBarcodeTextBox.Value = this.GTBarcode;
                xRayHMIGTBarcodeTextBox.Focus();

            }
            catch(Exception exp)
            {

            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).ID == "xRayHMIStatusOKButton")
                {
                    xRayHMIGTBarcodeCodeHidden.Value = xRayHMIGTBarcodeTextBox.Value;
                    xRayHMIDefectCodeHidden.Value = "1";
                    xRayHMIStatusHidden.Value = "OK";
                    xRayHMITimerStatusHidden.Value = "0";

                    TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), xRayHMIGTBarcodeCodeHidden.Value.Trim(), "", Session["ID"].ToString(), Array.IndexOf(myWebService.status, xRayHMIStatusHidden.Value.Trim()), xRayHMIDefectCodeHidden.Value, Convert.ToDateTime(DateTime.Now));

                    GetLastGTHistory(xRayHMIGTBarcodeCodeHidden.Value.Trim());


                    clearPage();
                    //viTimer.Enabled = true;
                }
                else if (((Button)sender).ID == "xRayHMICancelButton")
                {
                    clearPage();
                }
                else if (((Button)sender).ID == "xRayHMISaveButton")
                {
                    xRayHMIGTBarcodeCodeHidden.Value = xRayHMIGTBarcodeTextBox.Value;
                    xRayHMIStatusHidden.Value = "Not OK";

                    TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), xRayHMIGTBarcodeCodeHidden.Value.Trim(), "", Session["ID"].ToString(), Array.IndexOf(myWebService.status, xRayHMIStatusHidden.Value.Trim()), xRayHMIDefectCodeHidden.Value, Convert.ToDateTime(DateTime.Now));

                    GetLastGTHistory(xRayHMIGTBarcodeCodeHidden.Value.Trim());

                    clearPage();
                }
            }
            catch(Exception exp)
            {
            }
        }

        //protected void NotifyTimer_Tick(object sender, EventArgs e)
        //{
        //    if (((Timer)sender).ID == "xRayTimer")
        //    {
        //        if (xRayHMITimerStatusHidden.Value == "0")
        //        {
        //            TransferData(this.TableName, Convert.ToInt32(Request.QueryString[0].ToString()), xRayHMIGTBarcodeCodeHidden.Value.Trim(), "", Session["ID"].ToString(), Array.IndexOf(myWebService.status, xRayHMIStatusHidden.Value.Trim()), xRayHMIDefectCodeHidden.Value, Convert.ToDateTime(DateTime.Now));
        //            GetLastGTHistory(xRayHMIGTBarcodeCodeHidden.Value.Trim());
        //            clearPage();
        //        }
        //        xRayTimer.Enabled = false;
        //    }
        //}

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

            //Description   : Function for filling xRayHMIGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01


            xRayHMIDefectCodeListView.GroupItemCount = 1;

            //int flag = myWebService.RecordCount("defectstatusMaster", "name", "");

            //if (flag < 8)
            //{
            //    xRayHMIDefectCodeListView.GroupItemCount = 2;
            //}
            //else if ((flag >= 8) && (flag <= 12))
            //{
            //    xRayHMIDefectCodeListView.GroupItemCount = 3;
            //}
            //else
            //{
            //    xRayHMIDefectCodeListView.GroupItemCount = 4;
            //}

            xRayHMIDefectCodeListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectMaster ORDER BY iD", ConnectionOption.SQL);
            xRayHMIDefectCodeListView.DataBind();
        }

        private void fillBodyPly()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            xRayHMIBodyPlyListView.GroupItemCount = 5;

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

            xRayHMIBodyPlyListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectLocationMaster WHERE xRayDefectStatusID = 1 ORDER BY iD", ConnectionOption.SQL);
            xRayHMIBodyPlyListView.DataBind();
        }

        private void fillBelt()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            xRayHMIBeltListView.GroupItemCount = 5;

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

            xRayHMIBeltListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectLocationMaster WHERE xRayDefectStatusID = 2 ORDER BY iD", ConnectionOption.SQL);
            xRayHMIBeltListView.DataBind();
        }

        private void fillBead()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            xRayHMIBeadListView.GroupItemCount = 5;

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

            xRayHMIBeadListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectLocationMaster WHERE xRayDefectStatusID = 3 ORDER BY iD", ConnectionOption.SQL);
            xRayHMIBeadListView.DataBind();
        }

        private void fillSteelChipper()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            xRayHMISteelChipperListView.GroupItemCount = 5;

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

            xRayHMISteelChipperListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectLocationMaster WHERE xRayDefectStatusID = 4 ORDER BY iD", ConnectionOption.SQL);
            xRayHMISteelChipperListView.DataBind();
        }

        private void fillOther()
        {

            //Description   : Function for filling viHMIDefectLocationGridView with Fault Status Name
            //Author        : Brajesh kumar
            //Date Created  : 29 July 2011
            //Date Updated  : 29 July 2011
            //Revision No.  : 01

            xRayHMIOtherListView.GroupItemCount = 5;

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

            xRayHMIOtherListView.DataSource = myWebService.fillGridView("Select iD, name from xRayDefectLocationMaster WHERE xRayDefectStatusID = 5 ORDER BY iD", ConnectionOption.SQL);
            xRayHMIOtherListView.DataBind();
        }

        private int TransferData(String tableName, int wcID, String gtBarcode, String reportfilePath, String manningID, int status, String xRayDefectStatusID, DateTime dtandTime)
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

                //if (myWebService.IsRecordExist(TableName, "gtbarCode", "WHERE gtbarCode = '" + xRayHMIGTBarcodeTextBox.Value.Trim() + "'", out notifyIcon) == true)
                //{
                //    Notify(0, xRayHMIGTBarcodeTextBox.Value.Trim() + " already exists");
                //}
                //else
                //{
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Insert into " + this.TableName + " (wcID, gtbarCode, reportfilePath, manningID, status, xRayDefectStatusID, dtandTime) values (@wcID, @gtbarCode, @reportfilePath, @manningID, @status, @xRayDefectStatusID, @dtandTime)";
                myConnection.comm.Parameters.AddWithValue("@wcID", wcID);
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);
                myConnection.comm.Parameters.AddWithValue("@reportfilePath", reportfilePath);
                myConnection.comm.Parameters.AddWithValue("@manningID", manningID);
                myConnection.comm.Parameters.AddWithValue("@xRayDefectStatusID", xRayDefectStatusID);
                myConnection.comm.Parameters.AddWithValue("@status", status);
                myConnection.comm.Parameters.AddWithValue("@dtandTime", dtandTime);

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "X-Ray record saved successfully");

                GC.Collect();
                //}
            }
            else if (tempValidation == 1)
            {
                Notify(0, "Invalid Workcenter Name");
            }
            else if (tempValidation == 2)
            {
                Notify(0, "Invalid GT Barcode :" + xRayHMIGTBarcodeTextBox.Value.Trim());
            }
            else if (tempValidation == 3)
            {
                Notify(0, "Invalid Defect");
            }

            return flag;
        }

        private void GetLastGTHistory(String gtBarcode)
        {
            TyreType tyreType = getTyreType(gtBarcode);

            xRayLastGTBarcodeLabel.Text = gtBarcode;
            xRayTBMWCNameLabel.Text = "";
            xRayCuringWCNameLabel.Text = "";

            myConnection.open(ConnectionOption.SQL);

            if (tyreType == TyreType.TBR)
            {
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select wcName from vtbmtbr WHERE gtbarCode = @gtbarCode";
                myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarcode);

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    xRayTBMWCNameLabel.Text = myConnection.reader[0].ToString();
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
                    xRayCuringWCNameLabel.Text = myConnection.reader[0].ToString();
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
                    xRayTBMWCNameLabel.Text = myConnection.reader[0].ToString();
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
                    xRayCuringWCNameLabel.Text = myConnection.reader[0].ToString();
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
                xRayNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                xRayNotifyImage.Src = "../Images/tick.png";
            }
            xRayNotifyLabel.Text = notifyMessage;

            xRayNotifyMessageDiv.Visible = true;
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

            if (xRayHMIWCNameLabel.Text.Trim() == "")
            {
                flag = 1;
            }

            if ((xRayHMIGTBarcodeCodeHidden.Value.Trim() == "") || (xRayHMIGTBarcodeCodeHidden.Value.Length != 10))
            {
                flag = 2;
            }

            if (xRayHMIDefectCodeHidden.Value.Trim() == "0")
            {
                flag = 3;
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

            //xRayTimer.Enabled = false;

            xRayHMIGTBarcodeTextBox.Value = "";
            xRayHMIGTBarcodeCodeHidden.Value = "";
            xRayHMIStatusHidden.Value = "";
            xRayHMIDefectCodeHidden.Value = "0";
            xRayHMIStatusHidden.Value = "0";

        }

        #endregion
    }
}
