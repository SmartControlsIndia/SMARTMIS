using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace SmartMIS.Master
{
    public partial class Unloadbarcode : System.Web.UI.Page
    { 
         smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        DataTable tbldt = new DataTable();
        string moduleName = "SecondLineAdmin";
        DataTable wcdt = new DataTable();
        string fromdate = "", todate = "";
        string wherequery = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    bindGridview();
                    //int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text.Trim());
                    //int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                    //if (frombarcode.ToString().Length == 8)
                    //    wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray());
                    //else if (frombarcode.ToString().Length == 7)
                    //    wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());

                }
                catch (Exception ex)
                { }
            }
            
        }
        private int validation()
        {

            int flag = 0;


            if ((BarcodeFromTextBox.Text == "") || (BarcodeFromTextBox.Text.Length != 10))
                flag = 1;
            //if((defectlocationID==0)||(defectstatusID==0))
            //    flag =2;


            return flag;
        }
         protected void ViewButton_Click(object sender, EventArgs e)
        {
            if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
            {
                try
                {
                    DataTable dt = new DataTable();
                    string savebarcode = "";
                    int saveFlag = 1;
                    int flag = 0;
                    flag = validation();
                    if (flag == 0)
                    {
                        ArrayList list = new ArrayList();
                        var listData = "";
                        int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text.Trim());
                        int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                        if (barcodeToTextBox.Text != "0")
                        {
                            if (frombarcode.ToString().Length == 8)
                                wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray()).Trim();
                            else if (frombarcode.ToString().Length == 7)
                                wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 9)
                                wherequery = "'0" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 10)
                                wherequery = "'" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'" + (frombarcode + i) + "'").ToArray());
                        }
                        else 
                        {

                            if (frombarcode.ToString().Length == 8)
                                wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + 0) + "'").ToArray()).Trim();
                            else if (frombarcode.ToString().Length == 7)
                                wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + 0) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 9)
                                wherequery = "'0" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + 0) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 10)
                                wherequery = "'" + frombarcode + "'";
                        }
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();
                        myConnection.comm.CommandText = " select barcode from UnloadBarcode where barcode in (" + wherequery + " )";

                        myConnection.reader = myConnection.comm.ExecuteReader();
                        if (myConnection.reader.HasRows)
                        {
                            while (myConnection.reader.Read())
                            {
                                savebarcode = myConnection.reader[0].ToString();

                            }
                            saveFlag = 0;
                            //flag = -1;
                        }

                        myConnection.reader.Close();
                        if ((validation() <= 0) && (saveFlag == 1))
                        // if (saveFlag == 1)
                        {
                            for (int i = 0; i <= tocount; i++)
                            {
                                list.AddRange(wherequery.Split(new char[] { ',' }));

                                myConnection.comm.CommandText = "insert into UnloadBarcode (barcode) values(" + list[i] + ")";
                                flag = myConnection.comm.ExecuteNonQuery();

                            }
                            myConnection.comm.Dispose();
                            myConnection.close(ConnectionOption.SQL);
                            Notify(1, "Record Inserted successfully");
                            bindGridview();
                            BarcodeFromTextBox.Text = "";
                            barcodeToTextBox.Text = "";
                        }
                        else 
                        
                        {
                            Notify(1, "Records already Exists.");
                        }
                    }


                }

                catch (Exception exc)
                { //myWebService.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath)); }

                }
               
            }
            else
            {
                Notify(1, "Please login with Valid Role.");
            }

           
            }
         private void bindGridview()
         {
             try
             {
                 MainGridView.DataSource = "";
                 MainGridView.DataBind();

                 myConnection.open(ConnectionOption.SQL);
                 myConnection.comm = myConnection.conn.CreateCommand();
                 myConnection.comm.CommandText = "Select barcode as Barcode from UnloadBarcode";
                 myConnection.reader = myConnection.comm.ExecuteReader();
                 tbldt.Load(myConnection.reader);
             }
             catch (Exception exc)
             { }
             finally
             {
                 if (tbldt.Rows.Count > 0)
                 {
                     if (!myConnection.reader.IsClosed)
                         myConnection.reader.Close();
                     myConnection.comm.Dispose();

                     MainGridView.DataSource = tbldt;
                     MainGridView.DataBind();
                     gvpanel.Visible = true;
                 }
                 else
                 {

                 }
             }
         }
         private void Notify(int notifyIcon, string notifyMessage)
         {

             if (notifyIcon == 0)
             {
                 rNotifyImage.Src = "../Images/notifyCircle.png";
             }
             else if (notifyIcon == 1)
             {
                 rNotifyImage.Src = "../Images/tick.png";
             }
             rNotifyLabel.Text = notifyMessage;

             oemNotifyMessageDiv.Visible = true;
             oemNotifyTimer.Enabled = true;

         }

         protected void NotifyTimer_Tick(object sender, EventArgs e)
         {
             oemNotifyMessageDiv.Visible = false;

         }

         protected void deleteButton_Click(object sender, EventArgs e)
         {
             if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 4) == true)
             {
                 try
                 {
                     int flag = 0;

                     int frombarcode = Convert.ToInt32(BarcodeFromTextBox.Text.Trim());
                     int tocount = Convert.ToInt32(barcodeToTextBox.Text.Trim());
                     if (barcodeToTextBox.Text != "0")
                     {
                     if (frombarcode.ToString().Length == 8)
                         wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + i) + "'").ToArray()).Trim();
                     else if (frombarcode.ToString().Length == 7)
                         wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray()).Trim();
                     else if (frombarcode.ToString().Length == 9)
                         wherequery = "'0" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + i) + "'").ToArray());
                     else if (frombarcode.ToString().Length == 10)
                         wherequery = "'" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'" + (frombarcode + i) + "'").ToArray());
                     }
                        else 
                        {

                            if (frombarcode.ToString().Length == 8)
                                wherequery = "'00" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'00" + (frombarcode + 0) + "'").ToArray()).Trim();
                            else if (frombarcode.ToString().Length == 7)
                                wherequery = "'000" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + 0) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 9)
                                wherequery = "'0" + frombarcode + "'," + string.Join(",", Enumerable.Range(1, tocount).Select(i => "'000" + (frombarcode + 0) + "'").ToArray());
                            else if (frombarcode.ToString().Length == 10)
                                wherequery = "'" + frombarcode + "'";
                        }
                     myConnection.open(ConnectionOption.SQL);
                     myConnection.comm = myConnection.conn.CreateCommand();

                     myConnection.comm.CommandText = "Delete from UnloadBarcode where barcode in (" + wherequery + ")";
                     myConnection.comm.ExecuteNonQuery();
                     myConnection.comm.Dispose();
                     myConnection.close(ConnectionOption.SQL);

                     Notify(1, "Record deleted successfully");
                     bindGridview();
                     BarcodeFromTextBox.Text = "";
                     barcodeToTextBox.Text = "";
                 }
                 catch (Exception exc)
                 { }

                 
             }
         }

         
             
    
    }
}
