using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing;
using System.Web.UI.HtmlControls;


namespace SmartMIS.Input
{
    public partial class curingOperatorPlanning : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string moduleName = "CuringOperatorPlanning";
        ArrayList Assignedwcname=new ArrayList();
       static string shift="";


        #region System Defined Function

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        A_Shift.Checked = true;
                        fillGridView("curingOperatorPlanningGridView");

                        if (A_Shift.Checked == true)
                            shift = "A";
                        else if (B_Shift.Checked == true)
                            shift = "B";
                        else if (C_Shift.Checked == true)
                            shift = "C";


                    }
                    catch(Exception exp)
                    {

                    }
                }
            }

            protected void Button_Click(object sender, EventArgs e)
            {
                try
                {
                    if (((Button)sender).ID == "curingOperatorPlanningOperatorNameButton")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;
                      
                    }
                    else if (((Button)sender).ID == "curingOperatorPlanningWCAssignButton")
                    {
                        if (myWebService.uservalidation(Session["userID"].ToString(), this.moduleName, 2) == true)
                        {
                            getselectedWorkcenter();

                            if (curingOperatorPlanningWCIDHidden.Value.Trim() != "")
                            {
                                LoginUser(curingOperatorPlanningManningIDHidden.Value, curingOperatorPlanningWCIDHidden.Value, "0");
                            }
                            else
                                LogoffUser(curingOperatorPlanningManningIDHidden.Value);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(curingOperatorPlanningMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForAuthentication');", true);
                        }

                    }
                    else if (((Button)sender).ID == "curingOperatorPlanningWCLogOutButton")
                    {
                        GridViewRow gridViewRow = (GridViewRow)((DataControlFieldCell)((Button)sender).Parent).Parent;

                        string manningID = (((Label)gridViewRow.Cells[1].FindControl("curingOperatorPlanningManningIDLabel")).Text);
                        curingOperatorPlanningManningIDHidden.Value = (((Label)gridViewRow.Cells[1].FindControl("curingOperatorPlanningManningIDLabel")).Text);

                        curingOperatorPlannedWcID.Value = workcenterStatus(Convert.ToInt32(curingOperatorPlanningManningIDHidden.Value));
                        fillGridView("curingOperatorPlanningWCGridView");
                        ScriptManager.RegisterClientScriptBlock(curingOperatorPlanningMagicButton, this.GetType(), "myScript", "javascript:revealModal('modalPageForWorkCenter');", true);

                    }

                    fillGridView("curingOperatorPlanningGridView");
                }
                catch(Exception ex)
                {

                }
            }

            protected void ListView_ItemDataBound(object sender, ListViewItemEventArgs e)
            {
                String[] wcname;
                wcname = curingOperatorPlannedWcID.Value.Split(new char[] { ',' });
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    HtmlInputControl CountryLabel = (HtmlInputControl)e.Item.FindControl("curingOperatorPlanningWCButton");
                    CheckBox CB = (CheckBox)e.Item.FindControl("selectwccheckbox");
                    for (int i = 0; i < wcname.Length; i++)
                    {
                        if (CountryLabel.Value.ToString().Equals(wcname[i].TrimStart()))
                        {
                            CountryLabel.Style.Add(HtmlTextWriterStyle.BackgroundColor, "red");
                            CB.Checked = true;

                        }
                      
                       
                    }
                  
                }
            }

        #endregion

        #region User Defined Function

            private void fillGridView(string id)
            {
                try
                {
                    if (id == "curingOperatorPlanningGridView")
                    {
                        curingOperatorPlanningGridView.DataSource = myWebService.fillGridView("SELECT DiSTINCT iD, firstName, lastName, sapCode FROM vManning WHERE deptID = 2 ORDER BY sapCode", ConnectionOption.SQL);
                        curingOperatorPlanningGridView.DataBind();
                    }
                    else if (id == "curingOperatorPlanningWCGridView")
                    {
                        curingOperatorPlanningWCGridView.DataSource = myWebService.fillGridView("SELECT DiSTINCT iD, name FROM wcMaster WHERE  processID = 5 OR processID = 8 and id not in(select wcID from wclogIn where manningID!="+curingOperatorPlanningManningIDHidden.Value+" and shift='"+shift+"') ", ConnectionOption.SQL);
                        curingOperatorPlanningWCGridView.DataBind();
                    }
                }
                catch( Exception ex)
                {
                }
            }

            private void LoginUser(string manningID, string wcID, string status)
            {
                
                LogoffUser(manningID);

                string[] tempWCID = wcID.Split(new char[] { '#' });
                try
                {
                    myConnection.open(ConnectionOption.SQL);

                    foreach (string workcenterID in tempWCID)
                    {
                        if (workcenterID.Trim() != "")
                        {
                            myConnection.comm = myConnection.conn.CreateCommand();

                            myConnection.comm.CommandText = "Insert into wcLogin (manningID, wcID, status,shift) values (@manningID, @wcID, @status,@shift)";
                            myConnection.comm.Parameters.AddWithValue("@manningID", manningID);
                            myConnection.comm.Parameters.AddWithValue("@wcID", workcenterID);
                            myConnection.comm.Parameters.AddWithValue("@status", status);
                            myConnection.comm.Parameters.AddWithValue("@shift", shift);
                            
                            myConnection.comm.ExecuteNonQuery();

                            myConnection.comm.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    myConnection.close(ConnectionOption.SQL);
                }
            }

            private void LogoffUser(string manningID)
            {        

                try
                {
                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "DELETE from wcLogin WHERE manningID = @manningID";
                    myConnection.comm.Parameters.AddWithValue("@manningID", manningID);

                    myConnection.comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {

                    myConnection.comm.Dispose();
                    myConnection.close(ConnectionOption.SQL);
                }
            }

            public string displayWorkCenter(Object obj)
            {

                //Description   : Function for displaying workcenters
                //Author        : Brajesh kumar
                //Date Created  : 20 June 2012
                //Date Updated  : 20 June 2012
                //Revision No.  : 01
                //Revision Desc : 

                string flag = string.Empty;
                string flag1 = string.Empty;
                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    try
                    {
                        myConnection.open(ConnectionOption.SQL);
                        myConnection.comm = myConnection.conn.CreateCommand();

                        myConnection.comm.CommandText = "Select workCenterName,shift from vWCLogin WHERE manningID = @manningID";
                        myConnection.comm.Parameters.AddWithValue("@manningID", obj.ToString());

                        myConnection.reader = myConnection.comm.ExecuteReader();

                        while (myConnection.reader.Read())
                        {
                            flag = flag + myConnection.reader[0].ToString() + ", ";
                            flag1 = myConnection.reader[1].ToString();
                        }
                        if(flag1!="")
                        flag = flag + " Planned For " + "SHIFT " +flag1;
                        else
                        flag = " No WorkCenter Planned ";


                        if (flag.Trim() != "")
                            flag = flag.Trim().Substring(0, flag.Length - 2);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {

                        myConnection.reader.Close();
                        myConnection.comm.Dispose();
                        myConnection.close(ConnectionOption.SQL);
                    }
                }
                return flag;
            }

            private string workcenterStatus(int manningID)
              {
                  string tempshift = getshift(manningID);

                  string flag = string.Empty;

                  if (manningID>0)
                  {
                      try
                      {
                          myConnection.open(ConnectionOption.SQL);
                          myConnection.comm = myConnection.conn.CreateCommand();

                          myConnection.comm.CommandText = "Select workCenterName from vWCLogin WHERE manningID = @manningID and shift='"+tempshift+"' ";
                          myConnection.comm.Parameters.AddWithValue("@manningID", manningID);

                          myConnection.reader = myConnection.comm.ExecuteReader();

                          while (myConnection.reader.Read())
                          {
                              flag = flag + myConnection.reader[0].ToString() + ", ";
                          }

                          if (flag.Trim() != "")
                              flag = flag.Trim().Substring(0, flag.Length - 2);
                      }
                      catch (Exception ex)
                      {

                      }
                      finally
                      {

                          myConnection.reader.Close();
                          myConnection.comm.Dispose();
                          myConnection.close(ConnectionOption.SQL);
                      }
                  }
                  return flag;
              }


            private string getshift(int manningID)
            {
                string flag = string.Empty;

                  if (manningID>0)
                  {
                      try
                      {
                          myConnection.open(ConnectionOption.SQL);
                          myConnection.comm = myConnection.conn.CreateCommand();

                          myConnection.comm.CommandText = "Select distinct shift from vWCLogin WHERE manningID = @manningID";
                          myConnection.comm.Parameters.AddWithValue("@manningID", manningID);

                          myConnection.reader = myConnection.comm.ExecuteReader();

                          while (myConnection.reader.Read())
                          {
                              flag = flag + myConnection.reader[0].ToString();
                          }

                         
                      }
                      catch (Exception ex)
                      {

                      }
                      finally
                      {

                          myConnection.reader.Close();
                          myConnection.comm.Dispose();
                          myConnection.close(ConnectionOption.SQL);
                      }
                  }
                  return flag;
              }
 
            
            private void getselectedWorkcenter()
            {
                curingOperatorPlanningWCIDHidden.Value = "";
                string tempiD;
                if (curingOperatorPlanningWCGridView.Items.Count > 0)
                {
                    for (int i = 0; i < curingOperatorPlanningWCGridView.Items.Count; i++)
                    {
                        HtmlInputButton ib = (HtmlInputButton)curingOperatorPlanningWCGridView.Items[i].FindControl("curingOperatorPlanningWCButton");

                        CheckBox CB = (CheckBox)curingOperatorPlanningWCGridView.Items[i].FindControl("selectwccheckbox");
                            if(CB.Checked)
                            {
                              tempiD= myWebService.getID("wcMaster", "name", ib.Value);
                                curingOperatorPlanningWCIDHidden.Value = curingOperatorPlanningWCIDHidden.Value + "#" +tempiD;
                            }

                      
                    
                    }
 
                }
            }

         



        #endregion

            //protected void selectWC_CheckedChanged(object sender, EventArgs e)
            //{

            //    CheckBox CB = (CheckBox)((CheckBox)sender).FindControl("selectwccheckbox");
            //    CheckBox cb = (CheckBox)sender;
            //    ListViewItem item = (ListViewItem)cb.NamingContainer;
            //    ListViewDataItem dataItem = (ListViewDataItem)item;
            //    int index = dataItem.DataItemIndex;


            //    if (CB.Checked)
            //    {
            //        HtmlInputButton ib = (HtmlInputButton)curingOperatorPlanningWCGridView.Items[index].FindControl("curingOperatorPlanningWCButton");

            //        ib.Style.Add(HtmlTextWriterStyle.BackgroundColor, "red");
            //    }
            //}

            protected void Shift_CheckedChanged(object sender, EventArgs e)
            {
                if (((CheckBox)sender).ID == "A_Shift")
                    shift = "A";
                else if (((CheckBox)sender).ID == "B_Shift")
                    shift = "B";
                else if (((CheckBox)sender).ID == "C_Shift")
                    shift = "C";

            }
           


    }
}
