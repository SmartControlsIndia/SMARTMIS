using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartMIS.Input
{
    public partial class curingHMI : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        #region Variables

        private int _processID;

        #endregion

        #region Property

        public int ProcessID
        {
            get { return _processID; }
            set { _processID = value; }
        }

        #endregion

        #region System Defined Function

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Timeout = (60 * 8);
            if (!Page.IsPostBack)
            {
                fillGridView("curingHMIGridView");
            }

            if (Request.QueryString[0].Trim() == "TBR")
                this.ProcessID = 5;
            else if (Request.QueryString[0].Trim() == "PCR")
                this.ProcessID = 8;
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            curingHMIloginErrorLabel.Text = "";

            if (((Button)sender).ID == "curingHMIWCLoginButton")
            {
                curingHMIPasswordTextBox.Value = "";
                ScriptManager.RegisterClientScriptBlock(curingHMIMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForLogin');", true);

                curingHMIPasswordTextBox.Focus();
            }
            else if (((Button)sender).ID == "curingHMILoginButton")
            {
                /// 0 = Login
                /// 1 = Already Login
                /// 2 = Invalid Login

                int loginStatus = authenticateUser(curingHMIPasswordTextBox.Value.Trim());

                if (loginStatus == 0)
                {
                    LoginUser(curingHMIManningIDHidden.Value);
                }
                else if (loginStatus == 1)
                {
                    curingHMIloginErrorLabel.Text = "Already Logged in";
                    ScriptManager.RegisterClientScriptBlock(curingHMIMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForLogin');", true);
                }
                else if (loginStatus == 2)
                {
                    curingHMIloginErrorLabel.Text = "Invalid E-Code";
                    ScriptManager.RegisterClientScriptBlock(curingHMIMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForLogin');", true);
                }
            }
            else if (((Button)sender).ID == "curingHMIWCLogOutButton")
            {
                GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;

                string manningID = (((Label)gridViewRow.Cells[1].FindControl("curingHMIManningIDLabel")).Text);
                LogoffUser(manningID);
            }

            fillGridView("curingHMIGridView");
        }

        #endregion

        #region User Defined Function

        private int authenticateUser(string sapCode)
        {

            /// 0 = Login
            /// 1 = Already Login
            /// 2 = Invalid Login

            int flag = 2;

            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "Select * from [vUserDetails] where [sapCode]= @sapCode";
            myConnection.comm.Parameters.AddWithValue("@sapCode", sapCode);

            myConnection.reader = myConnection.comm.ExecuteReader();

            while (myConnection.reader.Read())
            {
                curingHMIManningIDHidden.Value = myConnection.reader[5].ToString();
                flag = 0;
            }

            myConnection.reader.Close();
            myConnection.comm.Dispose();

            if (flag == 0)
            {
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select * from [vWCLogin] where [manningID]= @manningID and [status] = 1";
                myConnection.comm.Parameters.AddWithValue("@manningID", curingHMIManningIDHidden.Value);

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = 1;
                }

                myConnection.reader.Close();
                myConnection.comm.Dispose();
            }
            else
            {
            }

            myConnection.close(ConnectionOption.SQL);

            return flag;
        }

        private void LoginUser(string manningID)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "UPDATE wcLogin SET status = 2 WHERE manningID = @manningID";
            myConnection.comm.Parameters.AddWithValue("@manningID", manningID);

            myConnection.comm.ExecuteNonQuery();

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        private void LogoffUser(string manningID)
        {
            myConnection.open(ConnectionOption.SQL);
            myConnection.comm = myConnection.conn.CreateCommand();

            myConnection.comm.CommandText = "UPDATE wcLogin SET status = 0 WHERE manningID = @manningID";
            myConnection.comm.Parameters.AddWithValue("@manningID", manningID);

            myConnection.comm.ExecuteNonQuery();

            myConnection.comm.Dispose();
            myConnection.close(ConnectionOption.SQL);
        }

        private void fillGridView(string id)
        {
            if (id == "curingHMIGridView")
            {
                curingHMIGridView.DataSource = myWebService.fillGridView("SELECT DiSTINCT manningID, firstName, lastName FROM vWCLogin WHERE status = 2", ConnectionOption.SQL);
                curingHMIGridView.DataBind();
            }
        }

        public string displayWorkCenter(Object obj)
        {

            //Description   : Function for displaying workcenters
            //Author        : Brajesh kumar
            //Date Created  : 18 June 2012
            //Date Updated  : 18 June 2012
            //Revision No.  : 01
            //Revision Desc : 

            string flag = string.Empty;

            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select workCenterName from vWCLogin WHERE manningID = @manningID";
                myConnection.comm.Parameters.AddWithValue("@manningID", obj.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = flag + myConnection.reader[0].ToString() + ", ";
                }

                flag = flag.Trim().Substring(0, flag.Length - 2);


                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            return flag;
        }

        #endregion
    }
}
