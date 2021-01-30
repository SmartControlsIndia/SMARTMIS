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
    public partial class mheInput : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGridView();
                fillWorkCenterNames();       
                fillMHECodes();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "mheWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", mheWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    mheWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                fillProductCodes(mheWCIDLabel.Text.Trim());
            }

            else if (((DropDownList)sender).ID == "mheProductCodeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from producttypeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", mheProductCodeDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    mheProductIDLabel.Text = myConnection.reader[0].ToString();
                    mheUnitIDLabel.Text = myConnection.reader[3].ToString();
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                fillUnit(mheUnitIDLabel.Text.Trim());
            }

            else if (((DropDownList)sender).ID == "mheCodeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from mheMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", mheCodeDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    mheCodeIDLabel.Text = myConnection.reader[0].ToString();                   
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "mheSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "mheCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "mheDialogOKButton")
            {
                mheIDLabel.Text = mheIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {            
           if (((ImageButton)sender).ID == "mheGridEditImageButton")
            {                
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                mheIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("mheGridIDLabel")).Text);
                mheWCNameDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("mheGridMHEWCNameLabel")).Text);

                EventArgs mheDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(mheWCNameDropDownList, mheDropDownListEventArgs);               

                mheProductCodeDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("mheGridProductNameLabel")).Text);
                DropDownList_SelectedIndexChanged(mheProductCodeDropDownList, mheDropDownListEventArgs);
                mheUnitDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("mheGridUnitNameLabel")).Text);
                mheQuantityTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("mheGridQuantityLabel")).Text);
                mheCodeDropDownList.SelectedValue = (((Label)gridViewRow.Cells[1].FindControl("mheGridMHENameLabel")).Text);
                DropDownList_SelectedIndexChanged(mheCodeDropDownList, mheDropDownListEventArgs);
              
            }
            else if (((ImageButton)sender).ID == "visGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        #endregion

        #region User Defined Function

        private void fillWorkCenterNames()
        {
            mheWCNameDropDownList.Items.Clear();
            mheWCNameDropDownList.Items.Add("");

            mheWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            mheWCNameDropDownList.DataBind();         

        }

        private void fillProductCodes(string wcID)
        {
            mheProductCodeDropDownList.Items.Clear();
            mheProductCodeDropDownList.Items.Add("");

            mheProductCodeDropDownList.DataSource = myWebService.FillDropDownList("producttypeMaster", "name", "WHERE wcID = " + wcID + "");
            mheProductCodeDropDownList.DataBind();

        }

        private void fillUnit(string unitID)
        {
            mheUnitDropDownList.Items.Clear();
            mheUnitDropDownList.Items.Add("");

            mheUnitDropDownList.DataSource = myWebService.FillDropDownList("unitMaster", "name", "WHERE iD = " + unitID + "");
            mheUnitDropDownList.DataBind();
            mheUnitDropDownList.SelectedIndex = 1;
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            mheNotifyMessageDiv.Visible = false;
            mheNotifyTimer.Enabled = false;
        }

        private void fillMHECodes()
        {
            mheCodeDropDownList.Items.Clear();
            mheCodeDropDownList.Items.Add("");

            mheCodeDropDownList.DataSource = myWebService.FillDropDownList("mheMaster", "name");
            mheCodeDropDownList.DataBind();

        }

        private void fillGridView()
        {
            mheGridView.DataSource = myWebService.fillGridView("Select iD, wcID, productTypeName, unitName, quantity, mheName, wcName from vMHEInput", ConnectionOption.SQL);
            mheGridView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in wcMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            int flag = 0;
            int notifyIcon = 0;

            if (mheIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();
                    myConnection.comm.CommandText = "Insert into mheInput (wcID, productID, unitID, mheID, quantity) values (@wcID, @productID, @unitID, @mheID, @quantity)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(mheWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productID", mheProductIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@unitID", mheUnitIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@mheID", mheCodeIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@quantity", mheQuantityTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                    Notify(1, "MHE record saved successfully");
                }              
            }

            else if (mheIDLabel.Text.Trim() != "0")
            {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update mheInput SET wcID = @wcID, productID = @productID, unitID = @unitID, mheID = @mheID, quantity = @quantity WHERE (iD = @iD)";
       
                    myConnection.comm.Parameters.AddWithValue("@iD", Convert.ToInt32(mheIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(mheWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productID", mheProductIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@unitID", mheUnitIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@mheID", mheCodeIDLabel.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@quantity", mheQuantityTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "MHE Code updated successfully");
               
            }
            return flag;
        }

        private void clearPage()
        {
            mheWCIDLabel.Text = "0";
            mheProductIDLabel.Text = "0";
            mheUnitIDLabel.Text = "0";
            mheQuantityTextBox.Text = "";
            mheCodeIDLabel.Text = "0";

            fillWorkCenterNames();
            fillMHECodes();
            fillProductCodes(mheWCIDLabel.Text.Trim());
            fillUnit(mheUnitIDLabel.Text.Trim());
            //mheProductCodeDropDownList.SelectedIndex = 0;
            //mheUnitDropDownList.SelectedIndex = 0;
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
                mheNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                mheNotifyImage.Src = "../Images/tick.png";
            }
            mheNotifyLabel.Text = notifyMessage;

            mheNotifyMessageDiv.Visible = true;
            mheNotifyTimer.Enabled = true;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in wcMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 26 March 2011
            //Date Updated  : 26 March 2011
            //Revision No.  : 01

            int flag = 0;

            if (mheWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (mheProductIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (mheCodeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (mheUnitIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            } 

            if (mheQuantityTextBox.Text.Trim() == "")
            {
                flag = 1;
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

            if (mheIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From mheInput WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", mheIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "MHE record deleted successfully");
            }

            return flag;
        }

        #endregion
    }
}
