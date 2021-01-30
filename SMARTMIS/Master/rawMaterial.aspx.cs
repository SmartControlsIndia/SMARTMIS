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
    public partial class rawMaterialMaster1 : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillWorkCenterName();
              
                fillGridView();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());



            if (((DropDownList)sender).ID == "rawMaterialWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", rawMaterialWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    rawMaterialWCIDLabel.Text = myConnection.reader[0].ToString();
                }

                fillProductTypeName();

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else if (((DropDownList)sender).ID == "rawMaterialProductCodeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from producttypeMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", rawMaterialProductCodeDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    rawMaterialProductIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "rawMaterialSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "rawMaterialCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "rawMaterialDialogOKButton")
            {
                rawMaterialIDLabel.Text = rawMaterialIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "rawMaterialGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                rawMaterialIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridIDLabel")).Text);
                rawMaterialWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridWCIDLabel")).Text);
                rawMaterialProductIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridProductTypeIDLabel")).Text);
                rawMaterialWCNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridWCNameLabel")).Text);
                EventArgs rawMaterialDropDownListEventArgs = new EventArgs();
                DropDownList_SelectedIndexChanged(rawMaterialWCNameDropDownList, rawMaterialDropDownListEventArgs);               

                rawMaterialProductCodeDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridProductLabel")).Text);
                rawMaterialNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridNameLabel")).Text);
                rawMaterialDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("rawMaterialGridDescriptionLabel")).Text);

            }
            else if (((ImageButton)sender).ID == " rawMaterialGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            rawMaterialNotifyMessageDiv.Visible = false;
            rawMaterialNotifyTimer.Enabled = false;
        }

        #endregion

        #region User Defined Function

        private void fillWorkCenterName()
        {

            //Description   : Function for filling wcNameDropDownList with Process Name
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01



            rawMaterialWCNameDropDownList.Items.Clear();
            rawMaterialWCNameDropDownList.Items.Add("");

            rawMaterialWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            rawMaterialWCNameDropDownList.DataBind();
        }

        private void fillProductTypeName()
        {
            //Description   : Function for filling productTypeNameDropDownList with Process Name
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01


            rawMaterialProductCodeDropDownList.Items.Clear();
            rawMaterialProductCodeDropDownList.Items.Add("");

            rawMaterialProductCodeDropDownList.DataSource = myWebService.FillDropDownList("producttypeMaster", "name", "where wcID='" + rawMaterialWCIDLabel.Text.ToString().Trim() + "'");

            rawMaterialProductCodeDropDownList.DataBind();
        }

        private void fillGridView()
        {

            //Description   : Function for filling rawMaterialGridView
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01


            rawMaterialGridView.DataSource = myWebService.fillGridView("Select * from vRawMaterial", "iD", smartMISWebService.order.Desc);
            rawMaterialGridView.DataBind();


        }

        private int save()
        {
            //Description   : Function for saving and updating record in rawMaterialMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01


            int flag = 0;
            int notifyIcon = 0;
            if (rawMaterialIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("rawmaterialMaster", "name", "where name='" + rawMaterialNameTextBox.Text.Trim() + "'", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into rawmaterialMaster (wcID, productTypeID, name, description) values (@wcID, @productTypeID, @name, @description)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(rawMaterialWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(rawMaterialProductIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", rawMaterialNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", rawMaterialDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "RawMaterial saved successfully");
                }

                else
                {
                    Notify(notifyIcon, "RawMaterial already exists");
                }

            }
            else if (rawMaterialIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update rawmaterialMaster SET wcID = @wcID, productTypeID = @productTypeID, name = @name, description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(rawMaterialWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@productTypeID", Convert.ToInt32(rawMaterialProductIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@name", rawMaterialNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@description", rawMaterialDescriptionTextBox.Text.Trim());

                    myConnection.comm.Parameters.AddWithValue("@iD", rawMaterialIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "RawMaterial updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in rawMaterialMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (rawMaterialIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From rawmaterialMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", rawMaterialIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "RawMaterial deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in rawMaterialMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (rawMaterialWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (rawMaterialProductIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (rawMaterialNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            rawMaterialWCIDLabel.Text = "0";
            rawMaterialProductIDLabel.Text = "0";
            rawMaterialNameTextBox.Text = "";
            fillWorkCenterName();
            fillProductTypeName();
            rawMaterialIDLabel.Text = "0";
            rawMaterialDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in rawMaterialMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                rawMaterialNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                rawMaterialNotifyImage.Src = "../Images/tick.png";
            }
            rawMaterialNotifyLabel.Text = notifyMessage;

            rawMaterialNotifyMessageDiv.Visible = true;
            rawMaterialNotifyTimer.Enabled = true;
        }

        #endregion

    
    }
}
