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


namespace SmartMIS.Input
{

    public partial class Input : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
      

        string Uid = "tbmuser";
        string upassword = "smart";// new pwd= jk@12345
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillOperatorNames();
               
            }

        }
        private void fillWorkCenterNames()
        {
            
            bdWCNameDropDownList.Items.Clear();
            bdWCNameDropDownList.Items.Add("");
            if (processDropDownList.SelectedItem.Text == "TBMTBR")
            {
                bdWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "WHERE ProcessId = '28' ");
            }
            else
            {
                bdWCNameDropDownList.DataSource = myWebService.FillDropDownList("wcMaster", "name", "WHERE ProcessId = '29'");
            } 
            
            bdWCNameDropDownList.DataBind();

        }
        private void fillRecipeCodes()
        {
            bdrecipeCodeDropDownList.Items.Clear();
            bdrecipeCodeDropDownList.Items.Add("");
            if (processDropDownList.SelectedItem.Text == "TBMTBR")
            {
                bdrecipeCodeDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "name", "WHERE ProcessId = '4'");
            }
            else
            {
                bdrecipeCodeDropDownList.DataSource = myWebService.FillDropDownList("recipeMaster", "name", "WHERE ProcessId = '7'");
            }
            bdrecipeCodeDropDownList.DataBind();

        }
        private void fillOperatorNames()
        {
           bdOperatorDropDownList.Items.Clear();
           bdOperatorDropDownList.Items.Add("");

           bdOperatorDropDownList.DataSource = myWebService.FillDropDownList("manningMaster", "firstname");
           bdOperatorDropDownList.DataBind();




        }
        protected void Button_Click(object sender, EventArgs e)
        {

            if (((Button)sender).ID == "bdSaveButton")

            {

                if (Session["userID"].ToString() == "tbmuser") //tbmuser
                {
                    save();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('You are not Authenticate User');", true);
                    //int temp = Convert.ToInt32(bdBarcodeTextBox.Text);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please provide number only');", true);
                    //MessageBox.Show("Please provide number only");
                    //Response.Write("alert('You are not authenticate User');");
                }              
            }
            else if (((Button)sender).ID == "bdCancelButton")
            {
                clearPage();
            }            
        }
        
        private int save()
        {
            int flag = 0;
            int notifyIcon = 0;

            try
            {
                if (processDropDownList.SelectedItem.Text == "TBMTBR")
                {
                    if ((validation() <= 0) && (myWebService.IsRecordExist("tbmtbr", "gtbarcode", "where( gtbarcode='" + bdBarcodeTextBox.Text.Trim() + "')", out notifyIcon) == false))
                    {
                        if (Convert.ToInt32(bdBarcodeToTextBox.Text) <= 50)
                        {
                            // RIGHT('00000000000000'+MM.MaterialPartNumber,14)
                            for (int i = 0; i <= Convert.ToInt32(bdBarcodeToTextBox.Text); i++)
                            {

                                // string x = i.ToString().PadLeft(4, '0');
                                double bdcode = Convert.ToDouble(bdBarcodeTextBox.Text);
                                bdcode = bdcode + i;
                                int bdleft = bdcode.ToString().Length;
                                string a = bdcode.ToString().PadLeft(10, '0');
                                myConnection.open(ConnectionOption.SQL);
                                myConnection.comm = myConnection.conn.CreateCommand();
                                myConnection.comm.CommandText = "Insert into tbmtbr (wcID,mheID,recipeId,recipecode,gtweight, manningID,manningID2,manningID3, gtbarcode,dtandtime) values (@wcID,@mheID, @recipeID, @recipecode,@weight, @manningID,@manningID,@manningID,@barcode,@dtandtime)";
                                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(WCIDLabel.Text.Trim()));
                                myConnection.comm.Parameters.AddWithValue("@mheID", '1');
                                myConnection.comm.Parameters.AddWithValue("@recipeID", bdrecipeIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@recipecode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@weight", '0');
                                myConnection.comm.Parameters.AddWithValue("@manningID", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@manningID2", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@manningID3", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@barcode", a.ToString());
                                myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
         
                                flag = myConnection.comm.ExecuteNonQuery();

                                myConnection.comm.Dispose();
                                myConnection.close(ConnectionOption.SQL);
                                Notify(1, "Record saved successfully");
                            }
                        }
                        else
                        {
                            Notify(notifyIcon, "Limit Exceed " + bdBarcodeToTextBox.Text.Trim());
                        }

                    }
                    else
                    {

                        Notify(notifyIcon, "Record already exists for " + bdBarcodeTextBox.Text.Trim());
                    }
                }
                else if (processDropDownList.SelectedItem.Text == "TBMPCR")
                {
                    if ((myWebService.IsRecordExist("tbmpcr", "gtbarcode", "where( gtbarcode='" + bdBarcodeTextBox.Text.Trim() + "')", out notifyIcon) == false))
                    {
                        if (Convert.ToInt32(bdBarcodeToTextBox.Text) <= 50)
                        {
                            // RIGHT('00000000000000'+MM.MaterialPartNumber,14)
                            for (int i = 0; i <= Convert.ToInt32(bdBarcodeToTextBox.Text); i++)
                            {

                                // string x = i.ToString().PadLeft(4, '0');
                                double bdcode = Convert.ToDouble(bdBarcodeTextBox.Text);
                                bdcode = bdcode + i;
                                int bdleft = bdcode.ToString().Length;
                                string a = bdcode.ToString().PadLeft(10, '0');
                                myConnection.open(ConnectionOption.SQL);
                                myConnection.comm = myConnection.conn.CreateCommand();
                                myConnection.comm.CommandText = "Insert into tbmpcr (wcID,mheID,recipeId,recipecode,gtweight, manningID,manningID2,manningID3, gtbarcode,dtandtime) values (@wcID,@mheID, @recipeID, @recipecode,@weight, @manningID,@manningID,@manningID,@barcode,@dtandtime)";
                                myConnection.comm.Parameters.AddWithValue("@wcID", Convert.ToInt32(WCIDLabel.Text.Trim()));
                                myConnection.comm.Parameters.AddWithValue("@mheID", '1');
                                myConnection.comm.Parameters.AddWithValue("@recipeID", bdrecipeIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@recipecode", bdrecipeCodeDropDownList.SelectedItem.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@weight", '0');
                                myConnection.comm.Parameters.AddWithValue("@manningID", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@manningID2", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@manningID3", OpIDLabel.Text.Trim());
                                myConnection.comm.Parameters.AddWithValue("@barcode", a.ToString());
                                myConnection.comm.Parameters.AddWithValue("@dtandtime", DateTime.Now);
                                myWebService.writeLogs(myConnection.comm.CommandText, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
         
                                flag = myConnection.comm.ExecuteNonQuery();

                                myConnection.comm.Dispose();
                                myConnection.close(ConnectionOption.SQL);
                                Notify(1, "Record saved successfully");
                            }
                        }

                    }
                    else
                    {

                        Notify(notifyIcon, "Record already exists for " + bdBarcodeTextBox.Text.Trim());
                    }
                }
            }
            catch (Exception exp)
            {
              myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
         
            }
          
            
            //}
           return flag;
        
        }
        private void Notify(int notifyIcon, string notifyMessage)
        {
            if (notifyIcon == 0)
            {
                bdNotifyImage.Src = "../Images/notifyCircle.png";
            }
            else if (notifyIcon == 1)
            {
                bdNotifyImage.Src = "../Images/tick.png";
            }
            bdNotifyLabel.Text = notifyMessage;

            bdNotifyMessageDiv.Visible = true;
            bdNotifyTimer.Enabled = true;
        }
        protected void NotifyTimer_Tick(object sender, EventArgs e)
        {
            bdNotifyMessageDiv.Visible = false;
            bdNotifyTimer.Enabled = false;
        }
        private int validation()
        {

            int flag = 0;

            if (WCIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (bdrecipeIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }

            if (OpIDLabel.Text.Trim() == "0")
            {
                flag = 1;
            }



            if (bdBarcodeTextBox.Text.Trim() == "")
            {
                flag = 1;
            }

            return flag;
        }
        private void clearPage()
        {
            WCIDLabel.Text = "0";
            bdrecipeIDLabel.Text = "0";
            bdBarcodeTextBox.Text = "0";
            OpIDLabel.Text = "";
            bdWCNameDropDownList.SelectedIndex = 0;
            bdBarcodeToTextBox.Text = "0";
            bdrecipeCodeDropDownList.SelectedIndex = 0;
            bdOperatorDropDownList.SelectedIndex = 0;
           
        }
        protected void bdWCNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "bdWCNameDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from wcMaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", bdWCNameDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    WCIDLabel.Text = myConnection.reader[0].ToString();
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

                //fillRecipeCodes(WCIDLabel.Text.Trim());
            }
             if (((DropDownList)sender).ID == "bdrecipeCodeDropDownList")
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from recipemaster Where name = @name";
                myConnection.comm.Parameters.AddWithValue("@name", bdrecipeCodeDropDownList.SelectedItem.ToString().Trim());

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    bdrecipeIDLabel.Text = myConnection.reader[0].ToString();
                   
                }
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);

               
            }
             if (((DropDownList)sender).ID == "bdOperatorDropDownList")
             {
                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();

                 myConnection.comm.CommandText = "Select * from manningMaster Where firstname = @name";
                 myConnection.comm.Parameters.AddWithValue("@name", bdOperatorDropDownList.SelectedItem.ToString().Trim());

                 myConnection.reader = myConnection.comm.ExecuteReader();
                 while (myConnection.reader.Read())
                 {
                     OpIDLabel.Text = myConnection.reader[0].ToString();

                 }
                 myConnection.reader.Close();
                 myConnection.comm.Dispose();
                 myConnection.close(ConnectionOption.SQL);


             }
        }

        protected void processDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                fillWorkCenterNames();
                fillRecipeCodes();
                bdBarcodeToTextBox.Text = "0";
                bdBarcodeTextBox.Text = "0";
                fillOperatorNames();
           
        }
      
    }
}
