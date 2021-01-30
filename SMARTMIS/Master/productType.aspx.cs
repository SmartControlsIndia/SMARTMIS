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

namespace SmartMIS.Master
{
    public partial class productType : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillWorkCenterName();
                fillUnitName();
                fillGridView();
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Remove("".Trim());



            if (((DropDownList)sender).ID == "productTypeWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", productTypeWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    productTypeWCIDLabel.Text = myConnection.reader[0].ToString();
                }

              

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            else if (((DropDownList)sender).ID == "productTypeUnitNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from unitMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", productTypeUnitNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    productTypeUnitIdLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }

        }

        protected void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "productTypeSaveButton")
            {
                save();
                fillGridView();
                clearPage();
            }
            else if (((Button)sender).ID == "productTypeCancelButton")
            {
                clearPage();
            }
            else if (((Button)sender).ID == "productTypeDialogOKButton")
            {
                productTypeIDLabel.Text = productTypeIDHidden.Value; //Passing value to wcIDLabel because on postback hidden field retains its value
                delete();
                fillGridView();
                clearPage();
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            if (((ImageButton)sender).ID == "productTypeGridEditImageButton")
            {
                //Code for editing gridview row
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((ImageButton)sender).Parent).Parent;

                productTypeIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridIDLabel")).Text);
                productTypeWCIDLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridWCIDLabel")).Text);

                productTypeUnitIdLabel.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGriUnitIDLabel")).Text);
                productTypeWCNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridWCNameLabel")).Text);

               
                productTypeUnitNameDropDownList.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridUnitNameLabel")).Text);
                productTypeNameTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridNameLabel")).Text);
                productTypeDescriptionTextBox.Text = (((Label)gridViewRow.Cells[1].FindControl("productTypeGridDescriptionLabel")).Text);

            }
            else if (((ImageButton)sender).ID == " productTypeGridDeleteImageButton")
            {
                //Code for deleting gridview row

            }
            else
            {
            }
        }

        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            productTypeNotifyMessageDiv.Visible = false;
            productTypeNotifyTimer.Enabled = false;
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

            productTypeWCNameDropDownList.Items.Clear();
            productTypeWCNameDropDownList.Items.Add("");

            productTypeWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name");
            productTypeWCNameDropDownList.DataBind();
        }

        private void fillUnitName()
        {
            //Description   : Function for filling productTypeNameDropDownList with Process Name
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01


            productTypeUnitNameDropDownList.Items.Clear();
            productTypeUnitNameDropDownList.Items.Add("");

            productTypeUnitNameDropDownList.DataSource = myWebService.FillDropDownList("unitMaster","name");

            productTypeUnitNameDropDownList.DataBind();
        }

        private void fillGridView()
        {
            //Description   : Function for filling productTypeGridView
            //Author        : Brajesh Kumar
            //Date Created  : 02 April 2011
            //Date Updated  : 02 April 2011
            //Revision No.  : 01

            productTypeGridView.DataSource = myWebService.fillGridView("Select * from vproductType", "iD", smartMISWebService.order.Desc);
            productTypeGridView.DataBind();
        }

        private int save()
        {
            //Description   : Function for saving and updating record in productTypeMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 2 April 2011
            //Date Updated  : 2 April 2011
            //Revision No.  : 01


            int flag = 0;
            int notifyIcon = 0;
            if (productTypeIDLabel.Text.Trim() == "0")
            {
                if ((validation() <= 0) && (myWebService.IsRecordExist("producttypeMaster", "name", "where((name='" + productTypeNameTextBox.Text.Trim() + "') AND (wcID='" + productTypeWCIDLabel.Text.Trim() + "'))", out notifyIcon) == false))
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Insert into producttypeMaster ( name,wcID, unitID, description) values ( @name,@wcID, @unitID, @description)";
                   
                    myConnection.comm.Parameters.AddWithValue("@name", productTypeNameTextBox.Text.Trim());
                     myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(productTypeWCIDLabel.Text.Trim()));
                     myConnection.comm.Parameters.AddWithValue("@unitID", Convert.ToInt32(productTypeUnitIdLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@description", productTypeDescriptionTextBox.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "productType saved successfully");
                }

                else
                {
                    Notify(notifyIcon, "productTypeName already exists for " + productTypeWCNameDropDownList.SelectedValue.ToString().Trim());
                }

            }
            else if (productTypeIDLabel.Text.Trim() != "0")
            {
                if (validation() <= 0)
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Update producttypeMaster SET name = @name, wcID = @wcID, unitID = @unitID,  description = @description WHERE (iD = @iD)";
                    myConnection.comm.Parameters.AddWithValue("@name", productTypeNameTextBox.Text.Trim());
                    myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(productTypeWCIDLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@unitID", Convert.ToInt32(productTypeUnitIdLabel.Text.Trim()));
                    myConnection.comm.Parameters.AddWithValue("@description", productTypeDescriptionTextBox.Text.Trim());

                    myConnection.comm.Parameters.AddWithValue("@iD", productTypeIDLabel.Text.Trim());

                    flag = myConnection.comm.ExecuteNonQuery();

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);

                    Notify(1, "productType updated successfully");
                }
            }

            return flag;
        }

        private int delete()
        {

            //Description   : Function for deleting record in productTypeMaster Table
            //Author        : Brajesh Kumar
            //Date Created  : 2 April 2011
            //Date Updated  : 2 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (productTypeIDLabel.Text.Trim() != "0")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Delete From producttypeMaster WHERE (iD = @iD)";
                myConnection.comm.Parameters.AddWithValue("@iD", productTypeIDLabel.Text.Trim());

                flag = myConnection.comm.ExecuteNonQuery();

                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                Notify(1, "productType deleted successfully");
            }

            return flag;
        }

        private int validation()
        {

            //Description   : Function for validation of data for inserting and deleting record in productTypeMaster Table
            //Author        : Brajesh kumar
            //Date Created  : 2 April 2011
            //Date Updated  : 2 April 2011
            //Revision No.  : 01

            int flag = 0;

            if (productTypeWCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (productTypeUnitIdLabel.Text.Trim() == "0")
            {
                flag = 1;
            }
            if (productTypeNameTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }

        private void clearPage()
        {

            //Description   : Function for clearing controls and variables of Page
            //Author        : Brajesh Kumar
            //Date Created  : 2 April 2011
            //Date Updated  : 2 April 2011
            //Revision No.  : 01

            productTypeWCIDLabel.Text = "0";
            productTypeUnitIdLabel.Text = "0";
            productTypeNameTextBox.Text = "";
            fillWorkCenterName();
            fillUnitName();
            
            productTypeIDLabel.Text = "0";
            productTypeDescriptionTextBox.Text = "";
        }

        private void Notify(int notifyIcon, string notifyMessage)
        {

            //Description   : Function for showing notify information in productTypeMessageDiv
            //Author        : Brajesh Kumar
            //Date Created  : 2 April 2011
            //Date Updated  : 2 April 2011
            //Revision No.  : 01

            //Condition 0   : Nothing
            //Condition 1   : Insertion
            //Condition 2   : Updation
            //Condition 3   : Deletion
            //Condition 3   : Error

            if (notifyIcon == 0)
            {
                productTypeNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                productTypeNotifyImage.Src = "../Images/tick.png";
            }
            productTypeNotifyLabel.Text = notifyMessage;

            productTypeNotifyMessageDiv.Visible = true;
            productTypeNotifyTimer.Enabled = true;
        }

        #endregion
    }
}
