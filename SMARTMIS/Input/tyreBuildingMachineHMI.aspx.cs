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
using System.IO;
using SmartMIS.SmartWebReference;

namespace SmartMIS.Input
{
    public partial class tyreBuildingMachineHMI : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        SmartMIS.Production.Production myProduction = new SmartMIS.Production.Production();

        #region Variables

            private String _tableName;

        #endregion

        #region Property

            public String TableName
            {
                get { return _tableName; }
                set { _tableName = value; }
            }

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    tbmHMIWCNameLabel.Text = myWebService.setWorkCenterName("wcMaster", Convert.ToInt32(Request.QueryString[0].Trim()));

                    tbmHMIRecipeIDHidden.Value = Session["tbmRecipeID"].ToString();
                    tbmHMIRecipeCodeHidden.Value = Session["tbmRecipeCode"].ToString();
                }

                if (Request.QueryString[1].Trim() == "tbmtbr")
                {
                    this.TableName = Request.QueryString[1].Trim();
                    fillRecipeCode("4");
                }
                else
                {
                    this.TableName = "tbmpcr";
                    fillRecipeCode("7");
                }

                tbmHMIRecipeCodeButton.Value = tbmHMIRecipeCodeHidden.Value;
                tbmHMIGTBarcodeTextBox.Focus();
            }
            catch (Exception exc)
            {
                Response.Write(exc.Message);
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "tbmHMIOKButton")
            {
                save();
                clearPage();
            }
            else if (((Button)sender).ID == "tbmHMINotOKButton")
            {
                clearPage();
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            tbmNotifyMessageDiv.Visible = false;
            tbmNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Functions

        private void fillRecipeCode(String processID)
        {

            //Description   : Function for filling tbmHMIGridView with Recipe Name
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            tbmHMIDefectCodeListView.DataSource = myWebService.fillGridView("Select distinct iD, recipename,imagepath from vrecipeMaster WHERE processID = "+ processID +" ORDER BY iD", ConnectionOption.SQL);
            tbmHMIDefectCodeListView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in vInspectiontbr Table
            //Author        : Brajesh kumar
            //Date Created  : 12 October 2011
            //Date Updated  : 12 October 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (validation() <= 0)
            {
                int wcID = 0;
                int recipeID = 0;
                int manningID = 0;
                int shiftID = 0;

                int notifyIcon = 0;

                Session["tbmRecipeID"] = tbmHMIRecipeIDHidden.Value.Trim();
                Session["tbmRecipeCode"] = tbmHMIRecipeCodeHidden.Value;

                wcID = Convert.ToInt32(Request.QueryString[0].Trim());
                manningID = Convert.ToInt32(Session["ID"].ToString());
                recipeID = Convert.ToInt32(tbmHMIRecipeIDHidden.Value.Trim());

                shiftID = myWebService.GetShiftID(DateTime.Now);

                if (myWebService.IsRecordExist(TableName, "gtbarCode", "WHERE gtbarCode = '" + tbmHMIGTBarcodeTextBox.Value.Trim() + "'", out notifyIcon) == true)
                {
                    Notify(4, tbmHMIGTBarcodeTextBox.Value.Trim() + " already exists");
                }
                else
                {
                    TransferData(TableName, wcID, tbmHMIGTBarcodeTextBox.Value.Trim(), "", recipeID, tbmHMIRecipeCodeHidden.Value.Trim(), 0, manningID, DateTime.Now);
                    myProduction.TransferData(wcID, recipeID, 1, shiftID, DateTime.Now, myWebService.GetTimeStamp(DateTime.Now));

                    Notify(1, tbmHMIGTBarcodeTextBox.Value.Trim() + " record saved successfully");

                    GC.Collect();
                }
            }
            else
            {
                Notify(4, "Invalid GT Barcode :" + tbmHMIGTBarcodeTextBox.Value.Trim());
            }

            return flag;
        }

        public void TransferData(String tableName, int wcID, String gtBarCode, String mheID, int recipeID, String recipeCode, Double gtWeight, int manningID, DateTime dtandTime)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Insert into " + tableName + " (wcID, gtbarCode, mheID, recipeID, recipeCode, gtWeight, manningID, dtandTime) values (@wcID, @gtbarCode, @mheID, @recipeID, @recipeCode, @gtWeight, @manningID, @dtandTime)";

            myConnection.comm.Parameters.AddWithValue("@wcID", wcID);
            myConnection.comm.Parameters.AddWithValue("@gtbarCode", gtBarCode);
            myConnection.comm.Parameters.AddWithValue("@mheID", mheID);
            myConnection.comm.Parameters.AddWithValue("@recipeID", recipeID);
            myConnection.comm.Parameters.AddWithValue("@recipeCode", recipeCode);
            myConnection.comm.Parameters.AddWithValue("@gtWeight", gtWeight);
            myConnection.comm.Parameters.AddWithValue("@manningID", manningID);
            myConnection.comm.Parameters.AddWithValue("@dtandTime", dtandTime);

            myConnection.comm.ExecuteNonQuery();

            myConnection.comm.Dispose();
            //myConnection.close(ConnectionOption.SQL);
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
            //Condition 4   : Error

            if (notifyIcon == 0)
            {
                tbmNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                tbmNotifyImage.Src = "../Images/tick.png";
            }
            else if (notifyIcon == 4)
            {
                tbmNotifyImage.Src = "../Images/notifyCircle.png";
            }
            tbmNotifyLabel.Text = notifyMessage;

            tbmNotifyMessageDiv.Visible = true;
            tbmNotifyTimer.Enabled = true;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in tbmtbr Table
            //Author        : Brajesh kumar
            //Date Created  : 28 February 2011
            //Date Updated  : 28 February 2011
            //Revision No.  : 01
            //Revision Desc : 

            int flag = 0;

            if (tbmHMIWCNameLabel.Text.Trim() == "")
                flag = 1;
            if ((tbmHMIGTBarcodeTextBox.Value.Trim() == "") || (tbmHMIGTBarcodeTextBox.Value.Length != 10))
                flag = 1;
            if (tbmHMIRecipeCodeHidden.Value.Trim() == "")
                flag = 1;

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh kumar
            //Date Created  : 28 February 2011
            //Date Updated  : 28 February 2011
            //Revision No.  : 01
            //Revision Desc : 

            tbmHMIGTBarcodeTextBox.Value = "";
            tbmHMIGTBarcodeTextBox.Focus();
        }

        #endregion
    }
}
