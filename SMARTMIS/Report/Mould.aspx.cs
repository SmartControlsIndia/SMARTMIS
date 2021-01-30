using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Configuration;

using System.Web.UI.WebControls.WebParts;

namespace SmartMIS.Report
{
    public partial class Mould : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string minHeat = "", maxHeat = "", mouldSize = "";
        DataTable maindt = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mSize.Items.Clear();
                fillsizedropdownlist();
                mouldcuringDropDownList.SelectedIndex = 2;
            } 
            mouldSize = mSize.SelectedItem.Value;
        }

        protected void magicButton_Click(object sender, EventArgs e)
        {
            
        }

        protected void fillGridView()
        {
            DataTable gridviewdt = new DataTable();
            string dataname = "", whereHeatis = "", mouldSize = "", wcName = "", mouldCountLH = "0", mouldCountRH = "0";

            dataname = mouldcuringDropDownList.SelectedItem.Value.ToString();
            minHeat = mininumHeat.Text.Trim().ToString();
            maxHeat = maximumHeat.Text.Trim().ToString();
            mouldSize = mSize.SelectedItem.Value.ToString();

            mSize.SelectedItem.Value = mouldSize;
            try
            {
                if (mouldSize != "All")
                    whereHeatis += " AND sizeName='" + mouldSize + "'";
                reportHeader.ReportDate = DateTime.Now.ToString("dd-MM-yyyy");

                dataname = (dataname == "Curing TBR") ? "5" : ((dataname == "Curing PCR") ? "8" : "");

                if (dataname != "Select")
                {
                    maindt.Columns.Add("iD", typeof(string));
                    maindt.Columns.Add("workCenterName", typeof(string));
                    maindt.Columns.Add("mouldCountLH", typeof(string));
                    maindt.Columns.Add("mouldCountRH", typeof(string));
                    gridviewdt.Columns.Add("iD", typeof(string));
                    gridviewdt.Columns.Add("workCenterName", typeof(string));
                    gridviewdt.Columns.Add("mouldCountLH", typeof(string));
                    gridviewdt.Columns.Add("mouldCountRH", typeof(string));

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select wcID AS iD, wcName AS workCenterName, mouldCountLH, mouldCountRH from vCuringmouldReport WHERE processID='" + dataname + "' " + whereHeatis + " order by dtandTime DESC";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    maindt.Load(myConnection.reader);

                    DataTable uniqwcGroupdt = new DataTable();
                    uniqwcGroupdt = GetDistinctRecords(maindt, "iD");
                    DataRow dr = gridviewdt.NewRow();

                    for (int i = 0; i < uniqwcGroupdt.Rows.Count; i++)
                    {
                        try
                        {
                            //mouldCountLH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND mouldCountLH<>0")[0][2].ToString();
                            mouldCountLH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND mouldCountLH<>0").AsEnumerable().Take(1).ToArray()[0][2].ToString();

                        }
                        catch { }
                        try
                        {
                            //mouldCountRH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND mouldCountRH<>0")[0][3].ToString();
                            mouldCountRH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND mouldCountRH<>0").AsEnumerable().Take(1).ToArray()[0][3].ToString();
                        }
                        catch { }
                        try
                        {
                            //wcName = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "'")[0][1].ToString();
                            wcName = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "'").AsEnumerable().Take(1).ToArray()[0][1].ToString();
                        }
                        catch { }
                        DataRow drow = gridviewdt.NewRow(); int flag = 0;

                        if (!string.IsNullOrEmpty(minHeat) && !string.IsNullOrEmpty(maxHeat))
                        {
                            if ((Convert.ToInt32(mouldCountLH) >= Convert.ToInt32(minHeat) || Convert.ToInt32(mouldCountRH) >= Convert.ToInt32(minHeat)) && ((Convert.ToInt32(mouldCountLH) <= Convert.ToInt32(maxHeat) || Convert.ToInt32(mouldCountRH) <= Convert.ToInt32(maxHeat))))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = mouldCountLH;
                                drow[3] = mouldCountRH;
                                flag = 1;
                            }

                        }
                        else if (!string.IsNullOrEmpty(minHeat))
                        {
                            if (Convert.ToInt32(mouldCountLH) >= Convert.ToInt32(minHeat) || Convert.ToInt32(mouldCountRH) >= Convert.ToInt32(minHeat))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = mouldCountLH;
                                drow[3] = mouldCountRH;
                                flag = 1;
                            }

                        }
                        else if (!string.IsNullOrEmpty(maxHeat))
                        {
                            if (Convert.ToInt32(mouldCountLH) <= Convert.ToInt32(maxHeat) || Convert.ToInt32(mouldCountRH) <= Convert.ToInt32(maxHeat))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = mouldCountLH;
                                drow[3] = mouldCountRH;
                                flag = 1;
                            }

                        }
                        else
                        {
                            drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                            drow[1] = wcName;
                            drow[2] = mouldCountLH;
                            drow[3] = mouldCountRH;
                            flag = 1;
                        }
                        if (flag == 1) gridviewdt.Rows.Add(drow);

                    }
                    mouldReportDateWiseGridView.DataSource = gridviewdt;
                    mouldReportDateWiseGridView.DataBind();
                }
                else
                {
                    mouldReportDateWiseGridView.DataSource = null;
                    mouldReportDateWiseGridView.DataBind();

                }
                mininumHeat.Text = minHeat.ToString();
                maximumHeat.Text = maxHeat.ToString();

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            fillGridView();
        }
        private static DataTable GetDistinctRecords(DataTable dt, string Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dt.DefaultView.Sort = "workCenterName";
            dt = dt.DefaultView.ToTable();

            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }
        private void fillsizedropdownlist()
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";
            
            //Description   : Function for returning values of coloums of a table in an ArrayList
            //Author        : Brajesh kumar
            //Date Created  : 19 AUG 2014
            //Date Updated  : 19 AUG 2014
            //Revision No.  : 01

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                string dataname = mouldcuringDropDownList.SelectedItem.Value.ToString();
            
                dataname = (dataname == "Curing TBR") ? "5" : ((dataname == "Curing PCR") ? "8" : "8");

                sqlQuery = "Select DISTINCT sizeName from vCuringMouldReport WHERE processID='"+dataname+"' ORDER BY sizeName";

                myConnection.comm.CommandText = sqlQuery;

                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    if (myConnection.reader[0].ToString() == "" || myConnection.reader[0].ToString() == "NULL")
                    {
                    }
                    else
                        flag.Add(myConnection.reader[0].ToString());
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {

                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            mSize.Items.Clear();
            mSize.DataSource = null;
            mSize.DataSource = flag;
            mSize.DataBind();
            
            
        }
        protected void mouldcuringDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mouldReportDateWiseGridView.DataSource = null;
            mouldReportDateWiseGridView.DataBind();
            mSize.Items.Clear();
            fillsizedropdownlist();
            fillGridView();
        }
        protected void mSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            mouldReportDateWiseGridView.DataSource = null;
            mouldReportDateWiseGridView.DataBind();
            mSize.SelectedValue = mouldSize;
            fillGridView();
        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "mouldReportDateWiseGridView")
                    {
                        String wcName = (((Label)e.Row.FindControl("mouldReportDateWiseWCNameLabel")).Text);
                        GridView childGridViewLH = ((GridView)e.Row.FindControl("mouldReportDateWiseChildGridViewLH"));
                        GridView childGridViewRH = ((GridView)e.Row.FindControl("mouldReportDateWiseChildGridViewRH"));


                        fillChildGridView(childGridViewLH, new String[] { wcName });
                        fillChildGridView(childGridViewRH, new String[] { wcName });
                    }


                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void fillGridView(string query)
        {

            //Description   : Function for filling productionReportDateWiseGridView WorkCenter
            //Author        : Rohit Singh
            //Date Created  : 30 April 2011
            //Date Updated  : 30 April 2011
            //Revision No.  : 01z
            mouldReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
            mouldReportDateWiseGridView.DataBind();
            
        }
        private void fillChildGridView(GridView childGridView, String[] args)
        {
            try
            {
                if (childGridView.ID == "mouldReportDateWiseChildGridViewLH")
                {
                    childGridView.DataSource = GetMouldTableLH(args[0]);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "mouldReportDateWiseChildGridViewRH")
                {
                    childGridView.DataSource = GetMouldTableRH(args[0]);
                    childGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        public String objName(Object objMouldName)
        {
            String flag = Convert.ToString(objMouldName);

            if (String.Empty == flag)
                flag = "";

            return flag;
        }
        private DataTable GetMouldTableLH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CurrentMouldCodeLH", typeof(string));
            flag.Columns.Add("CurrentSizeLH", typeof(string));
            
            flag.Columns.Add("CurrentMouldHeatLH", typeof(string));

            flag.Columns.Add("OldMouldCodeLH", typeof(string));
            flag.Columns.Add("OldSizeLH", typeof(string));
            
            flag.Columns.Add("OldMouldHeatLH", typeof(string));

            DataRow dr = flag.NewRow();

            String[] tempinfocurrent = getmouldcountLH(wcname).Split(new char[] { '#' });
            dr[0] = tempinfocurrent[1];
            dr[2] = Convert.ToInt32(tempinfocurrent[0]);
            dr[1] = tempinfocurrent[2];
            String[] tempinfoold = getoldmouldcountLH(wcname).Split(new char[] { '#' });
            dr[3] = tempinfoold[1].ToString();
            dr[5] = Convert.ToInt32(tempinfoold[0]);
            dr[4] = tempinfoold[2];
            flag.Rows.Add(dr);


            return flag;
        }
        private String getmouldcountLH(string wcname)
        {

            string flag = 0 + "#" + "NULL" + "#" + "NULL", whereHeatis = "";
            try
            {

                if (minHeat != "")
                    whereHeatis = " AND mouldCountLH>='"+minHeat+"'";
                if (maxHeat != "")
                    whereHeatis += " AND mouldCountLH<='" + maxHeat + "'";

                mouldSize = mSize.SelectedItem.Value.ToString();
                if (mouldSize != "All")
                    whereHeatis += " AND sizeName='" + mouldSize + "'"; 

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 mouldCountLH,mouldCodeLH, sizeName from vCuringmouldReport Where (wcname = @wcname) and MouldCountLH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = "";
                    flag = myConnection.reader[0] + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString();

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;

        }
        private String getoldmouldcountLH(string wcname)
        {
            string flag = 0 + "#NULL#NULL", whereHeatis = "";
            try
            {
                if (minHeat != "")
                    whereHeatis = " AND mouldCountLH>='" + minHeat + "'";
                if (maxHeat != "")
                    whereHeatis += " AND mouldCountLH<='" + maxHeat + "'";

                mouldSize = mSize.SelectedItem.Value.ToString();
                if (mouldSize != "All")
                    whereHeatis += " AND sizeName='" + mouldSize + "'"; 

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 mouldCountLH,mouldCodeLH,sizeName from vCuringmouldReport Where (wcname = @wcname) and  mouldCountLH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = "";

                    flag = Convert.ToInt32(myConnection.reader[0]) + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString();

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {

                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.open(ConnectionOption.SQL);
            }

            return flag;
        }
        private DataTable GetMouldTableRH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;

            flag.Columns.Add("CurrentMouldCodeRH", typeof(string));
            flag.Columns.Add("CurrentSizeRH", typeof(string));
            
            flag.Columns.Add("CurrentMouldHeatRH", typeof(string));

            flag.Columns.Add("OldMouldCodeRH", typeof(string));
            flag.Columns.Add("OldSizeRH", typeof(string));
            
            flag.Columns.Add("OldMouldHeatRH", typeof(string));

            DataRow dr = flag.NewRow();

            String[] tempinfocurrent = getmouldcountRH(wcname).Split(new char[] { '#' });
            dr[0] = tempinfocurrent[1];
            dr[2] = Convert.ToInt32(tempinfocurrent[0]);
            dr[1] = tempinfocurrent[2];
            String[] tempinfoold = getoldmouldcountRH(wcname).Split(new char[] { '#' });
            dr[3] = tempinfoold[1].ToString();
            dr[5] = Convert.ToInt32(tempinfoold[0]);
            dr[4] = tempinfoold[2];
            flag.Rows.Add(dr);


            return flag;
        }
        private String getmouldcountRH(string wcname)
        {

            String flag = 0 + "#NULL#NULL", whereHeatis = "";
            try
            {

                if (minHeat != "")
                    whereHeatis = " AND mouldCountRH>='" + minHeat + "'";
                if (maxHeat != "")
                    whereHeatis += " AND mouldCountRH<='" + maxHeat + "'";

                mouldSize = mSize.SelectedItem.Value.ToString();
                if (mouldSize != "All")
                    whereHeatis += " AND sizeName='" + mouldSize + "'"; 

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 mouldCountRH,mouldCodeRH,sizeName from vCuringmouldReport Where (wcname = @wcname) and MouldCountRH!=0 "+whereHeatis+" order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {
                    flag = "";

                    flag = myConnection.reader[0] + "#" + myConnection.reader[1] + "#" + myConnection.reader[2];

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.close(ConnectionOption.SQL);

            }
            return flag;

        }
        private String getoldmouldcountRH(string wcname)
        {
            String flag = 0 + "#NULL#NULL", whereHeatis = "";
            try
            {
                if (minHeat != "")
                    whereHeatis = " AND mouldCountRH>='" + minHeat + "'";
                if (maxHeat != "")
                    whereHeatis += " AND mouldCountRH<='" + maxHeat + "'";

                mouldSize = mSize.SelectedItem.Value.ToString();
                if (mouldSize != "All")
                    whereHeatis += " AND sizeName='" + mouldSize + "'"; 

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 mouldCountRH,mouldCodeRH,sizeName from vCuringmouldReport Where (wcname = @wcname) and  mouldCountRH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = "";
                    flag = myConnection.reader[0] + "#" + myConnection.reader[1] + "#" + myConnection.reader[2];

                }
            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {

                myConnection.comm.Dispose();
                myConnection.reader.Close();
                myConnection.open(ConnectionOption.SQL);
            }

            return flag;
        }
        private String SetFileName()
        {
            String fileName = "";
            //String[] tempDate = rToDate.Split(new char[] { '-' });

            //fileName = tempDate[0] + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("MMM") + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("yy");

            return fileName;
        }
        private String SetDayName(String day)
        {
            String flag = "";

            if (day.Length < 2)
                flag = "0" + day;
            else
                flag = day;

            return flag;
        }
        public string formatDate(String date)
        {
            string flag = "";

            DateTime tempDate = Convert.ToDateTime(date);
            flag = tempDate.AddDays(1).ToString("dd-MM-yyyy");

            return flag;
        }
        protected void exptoexcel_Click(object sender, EventArgs e)
        {
              string type = mouldcuringDropDownList.SelectedItem.Value;
                string size = mSize.SelectedItem.Value;

                Response.Clear();
                string filename = "MouldReport_" + DateTime.Now.ToString() + ".xls";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                //stringWrite.Write("<table><tr><td><b>Mould Report</b></td><td><b>Mould Size :</b>" + mSize.SelectedItem.Value + "</td><td><b>Type :</b> " + mouldcuringDropDownList.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr></table>");
                stringWrite.Write("<table><tr><td><b>Mould Report</b></td><td><b>Mould Size :</b>" + mSize.SelectedItem.Value + "</td><td><b>Type :</b> " + mouldcuringDropDownList.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr><tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr><tr style=\"background-color:orange;\"><td><b>Work center Name</b></td><td><b>Side :</b></td><td><b>Current Mould Code  :</b></td><td><b>Current Mould Size</b></td><td><b>Current Mould Heat</b></td><td><b>OLD Mould Code</b></td><td><b>OLD Mould Size</b></td><td><b>OLD Mould Heat</b></td> </tr></table>");

                                      
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
               
                 mouldReportDateWiseGridView.RenderControl(htmlWrite);
                 Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                        "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                 
                Response.Write(stringWrite.ToString());
               

                Response.End(); 
              
           
           }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /*Tell the compiler that the control is rendered
             * explicitly by overriding the VerifyRenderingInServerForm event.*/
        }
           

    }
}
