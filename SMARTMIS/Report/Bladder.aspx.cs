using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;

namespace SmartMIS.Report
{
    public partial class Bladder : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();
        string minHeat = "", maxHeat = "", bladderSize = "";
        DataTable maindt = new DataTable();

        public string rType, rWCID, rChoice, rToDate, rFromDate, rToMonth, rToYear, rFromYear, rprocessID;
        public int actualQuan, planningQuan, totalPlanningQuan, totalActualQuan;
        public String[] workcenter;

        public String Visiblity
        {

            get
            {
                return bladderReportDateWisePanel.Style[HtmlTextWriterStyle.Display];
            }
            set
            {
                bladderReportDateWisePanel.Style.Add(HtmlTextWriterStyle.Display, value);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                mSize.Items.Clear();
                fillsizedropdownlist();
                curingDropDownList.SelectedIndex = 2;
            }
            bladderSize = mSize.SelectedItem.Value;


        }
        protected void curingDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bladderReportDateWiseGridView.DataSource = null;
            bladderReportDateWiseGridView.DataBind();
            mSize.Items.Clear();
            fillsizedropdownlist();
            fillGridView();
        }
        protected void mSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bladderReportDateWiseGridView.DataSource = null;
            bladderReportDateWiseGridView.DataBind();
            mSize.SelectedValue = bladderSize;
            fillGridView();
        }
        private void fillsizedropdownlist()
        {
            ArrayList flag = new ArrayList();
            string sqlQuery = "";

            flag.Add("All");
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                string dataname = curingDropDownList.SelectedItem.Value.ToString();

                dataname = (dataname == "Curing TBR") ? "5" : ((dataname == "Curing PCR") ? "8" : "8");

                sqlQuery = "Select DISTINCT sizeName from vCuringBladderReport WHERE processID='" + dataname + "' ORDER BY sizeName";

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

            mSize.DataSource = null;
            mSize.DataSource = flag;
            mSize.DataBind();

        }
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((GridView)sender).ID == "bladderReportDateWiseGridView")
                    {
                        String wcName = (((Label)e.Row.FindControl("bladderReportDateWiseWCNameLabel")).Text);
                        GridView childGridViewLH = ((GridView)e.Row.FindControl("bladderReportDateWiseChildGridViewLH"));
                        GridView childGridViewRH = ((GridView)e.Row.FindControl("bladderReportDateWiseChildGridViewRH"));


                        fillChildGridView(childGridViewLH, new String[] { wcName }, rToDate, rFromDate);
                        fillChildGridView(childGridViewRH, new String[] { wcName }, rToDate, rFromDate);
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
            try
            {
                bladderReportDateWiseGridView.DataSource = myWebService.fillGridView(query, ConnectionOption.SQL);
                bladderReportDateWiseGridView.DataBind();

            }
            catch (Exception exp)
            {
                myWebService.writeLogs(exp.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private void fillChildGridView(GridView childGridView, String[] args, String toDate, String fromDate)
        {
            try
            {
                if (childGridView.ID == "bladderReportDateWiseChildGridViewLH")
                {
                    childGridView.DataSource = GetBladderTableLH(args[0]);
                    childGridView.DataBind();
                }
                else if (childGridView.ID == "bladderReportDateWiseChildGridViewRH")
                {
                    childGridView.DataSource = GetBladderTableRH(args[0]);
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
        private DataTable GetBladderTableLH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CurrentBladderCodeLH", typeof(string));
            flag.Columns.Add("CurrentBladderSizeLH", typeof(string));
            flag.Columns.Add("CurrentBladderHeatLH", typeof(string));
            flag.Columns.Add("BladderTargetLifeLH", typeof(string));
            flag.Columns.Add("OldBladderCodeLH", typeof(string));
            flag.Columns.Add("OldBladderSizeLH", typeof(string));
            flag.Columns.Add("OldBladderHeatLH", typeof(string));

            DataRow dr = flag.NewRow();

            String[] tempinfocurrent = getBladdercountLH(wcname).Split(new char[] { '#' });
            dr[0] = tempinfocurrent[1];
            dr[2] = Convert.ToInt32(tempinfocurrent[0]);
            dr[1] = tempinfocurrent[2];
            String[] tempinfoold = getoldBladdercountLH(wcname).Split(new char[] { '#' });
            dr[3] = tempinfocurrent[3].ToString();
            dr[4] = tempinfoold[1].ToString();
            dr[6] = Convert.ToInt32(tempinfoold[0]);
            dr[5] = tempinfoold[2];

            flag.Rows.Add(dr);


            return flag;
        }
        private string getBladdercountLH(string wcname)
        {

            string flag = 0 + "#" + "NULL" + "#" + "NULL", whereHeatis = "";

            try
            {
                bladderSize = mSize.SelectedItem.Value.ToString();
                if (bladderSize != "All")
                    whereHeatis += " AND sizeName='" + bladderSize + "'";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 bladderCountLH,bladderCodeLH,sizeName,TargetLifeLH from vCuringbladderReport Where (wcname = @wcname) and bladderCountLH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {

                    flag = myConnection.reader[0].ToString() + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString() + "#" + myConnection.reader[3].ToString();


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
        private string getoldBladdercountLH(string wcname)
        {
            string flag = 0 + "#" + "NULL" + "#" + "NULL", whereHeatis = "";
            try
            {
                bladderSize = mSize.SelectedItem.Value.ToString();
                if (bladderSize != "All")
                    whereHeatis += " AND sizeName='" + bladderSize + "'";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 bladderCountLH,bladderCodeLH,sizeName,TargetLifeLH from vCuringbladderReport Where (wcname = @wcname) and  bladderCountLH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString() + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString() + "#" + myConnection.reader[3].ToString();
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
        private DataTable GetBladderTableRH(string TableName)
        {
            DataTable flag = new DataTable(TableName);
            string wcname = TableName;
            flag.Columns.Add("CurrentBladderCodeRH", typeof(string));
            flag.Columns.Add("CurrentBladderSizeRH", typeof(string));
            flag.Columns.Add("CurrentBladderHeatRH", typeof(string));
            //flag.Columns.Add("BladderTargetLifeLH", typeof(string));
            flag.Columns.Add("BladderTargetLifeRH", typeof(string));
            flag.Columns.Add("OldBladderCodeRH", typeof(string));
            flag.Columns.Add("OldBladderSizeRH", typeof(string));
            flag.Columns.Add("OldBladderHeatRH", typeof(string));

            DataRow dr = flag.NewRow();

            String[] tempinfocurrent = getBladdercountRH(wcname).Split(new char[] { '#' });
            dr[0] = tempinfocurrent[1];
            dr[2] = Convert.ToInt32(tempinfocurrent[0]);
            dr[1] = tempinfocurrent[2];
            String[] tempinfoold = getoldBladdercountRH(wcname).Split(new char[] { '#' });
            dr[3] = tempinfocurrent[3].ToString();
            dr[4] = tempinfoold[1].ToString();
            dr[6] = Convert.ToInt32(tempinfoold[0]);
            dr[5] = tempinfoold[2];

            flag.Rows.Add(dr);


            return flag;
        }
        private string getBladdercountRH(string wcname)
        {

            string flag = 0 + "#" + "NULL" + "#" + "NULL", whereHeatis = "";

            try
            {
                bladderSize = mSize.SelectedItem.Value.ToString();
                if (bladderSize != "All")
                    whereHeatis += " AND sizeName='" + bladderSize + "'";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select top 1 bladderCountRH,bladderCodeRH,sizeName,TargetLifeRH from vCuringBladderReport Where (wcname = @wcname) and bladderCountRH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();



                while (myConnection.reader.Read())
                {

                    flag = myConnection.reader[0].ToString() + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString() + "#" + myConnection.reader[3].ToString();

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
        private string getoldBladdercountRH(string wcname)
        {
            string flag = 0 + "#" + "NULL" + "#" + "NULL", whereHeatis = "";
            try
            {
                bladderSize = mSize.SelectedItem.Value.ToString();
                if (bladderSize != "All")
                    whereHeatis += " AND sizeName='" + bladderSize + "'";

                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "Select Top 2 bladderCountRH,bladderCodeRH,sizeName,TargetLifeRH from vCuringBladderReport Where (wcname = @wcname) and  bladderCountRH!=0 " + whereHeatis + " order by dtandTime desc";
                myConnection.comm.Parameters.AddWithValue("@wcname", wcname.ToString());

                myConnection.reader = myConnection.comm.ExecuteReader();

                while (myConnection.reader.Read())
                {

                    flag = myConnection.reader[0].ToString() + "#" + myConnection.reader[1].ToString() + "#" + myConnection.reader[2].ToString() + "#" + myConnection.reader[3].ToString();


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
            String[] tempDate = rToDate.Split(new char[] { '-' });

            fileName = tempDate[0] + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("MMM") + Convert.ToDateTime(tempDate[1] + "-" + tempDate[0] + "-" + tempDate[2]).ToString("yy");

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

        protected void fillGridView()
        {
            DataTable gridviewdt = new DataTable();
            string dataname = "", whereHeatis = "", bladderSize = "", iD = "", wcName = "", bladderCountLH = "0", bladderCountRH = "0", BladderTargetLifeLH = "0", BladderTargetLifeRH = "0";
            dataname = curingDropDownList.SelectedItem.Value.ToString();
            minHeat = mininumHeat.Text.Trim().ToString();
            maxHeat = maximumHeat.Text.Trim().ToString();
            bladderSize = mSize.SelectedItem.Value.ToString();

            try
            {
                if (bladderSize != "All")
                    whereHeatis += " AND sizeName='" + bladderSize + "'";

                reportHeader.ReportDate = DateTime.Now.ToString("dd-MM-yyyy");
                dataname = (dataname == "Curing TBR") ? "5" : ((dataname == "Curing PCR") ? "8" : "");

                if (dataname != "Select")
                {
                    maindt.Columns.Add("iD", typeof(string));
                    maindt.Columns.Add("workCenterName", typeof(string));
                    maindt.Columns.Add("bladderCountLH", typeof(string));
                    maindt.Columns.Add("bladderCountRH", typeof(string));
                    maindt.Columns.Add("BladderTargetLifeLH", typeof(string));
                    maindt.Columns.Add("BladderTargetLifeRH", typeof(string));
                    gridviewdt.Columns.Add("iD", typeof(string));
                    gridviewdt.Columns.Add("workCenterName", typeof(string));
                    gridviewdt.Columns.Add("bladderCountLH", typeof(string));
                    gridviewdt.Columns.Add("bladderCountRH", typeof(string));
                    gridviewdt.Columns.Add("TargetLifeLH", typeof(string));
                    gridviewdt.Columns.Add("TargetLifeRH", typeof(string));

                    myConnection.open(ConnectionOption.SQL);
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "Select wcID AS iD, wcName AS workCenterName, bladderCountLH, bladderCountRH from vCuringBladderReport WHERE processID='" + dataname + "'  " + whereHeatis + " order by dtandTime DESC";

                    myConnection.reader = myConnection.comm.ExecuteReader();
                    maindt.Load(myConnection.reader);

                    DataTable uniqwcGroupdt = new DataTable();
                    uniqwcGroupdt = GetDistinctRecords(maindt, "ID");
                    DataRow dr = gridviewdt.NewRow();

                    for (int i = 0; i < uniqwcGroupdt.Rows.Count; i++)
                    {
                        try
                        {
                            bladderCountLH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND bladderCountLH<>0")[0][2].ToString();
                        }
                        catch
                        { }
                        try
                        {
                            bladderCountRH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND bladderCountRH<>0")[0][3].ToString();
                        }
                        catch
                        { }
                        try
                        {
                            BladderTargetLifeLH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND TargetLifeLH<>0")[0][4].ToString();
                        }
                        catch
                        { }
                        try
                        {
                            BladderTargetLifeRH = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "' AND TargetLifeRH<>0")[0][5].ToString();
                        }
                        catch
                        { }
                        try
                        {
                            wcName = maindt.Select("iD ='" + uniqwcGroupdt.Rows[i][0].ToString() + "'")[0][1].ToString();
                        }
                        catch
                        { }
                        DataRow drow = gridviewdt.NewRow(); int flag = 0;

                        if (minHeat != "" && maxHeat != "")
                        {
                            if ((Convert.ToInt32(bladderCountLH) >= Convert.ToInt32(minHeat) || Convert.ToInt32(bladderCountRH) >= Convert.ToInt32(minHeat)) && ((Convert.ToInt32(bladderCountLH) <= Convert.ToInt32(maxHeat) || Convert.ToInt32(bladderCountRH) <= Convert.ToInt32(maxHeat))))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = bladderCountLH;
                                drow[3] = bladderCountRH;
                                drow[4] = BladderTargetLifeLH;
                                drow[5] = BladderTargetLifeRH;
                                flag = 1;
                            }
                        }
                        else if (minHeat != "")
                        {
                            if (Convert.ToInt32(bladderCountLH) >= Convert.ToInt32(minHeat) || Convert.ToInt32(bladderCountRH) >= Convert.ToInt32(minHeat))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = bladderCountLH;
                                drow[3] = bladderCountRH;
                                drow[4] = BladderTargetLifeLH;
                                drow[5] = BladderTargetLifeRH;
                                flag = 1;
                            }
                        }
                        else if (maxHeat != "")
                        {
                            if (Convert.ToInt32(bladderCountLH) <= Convert.ToInt32(maxHeat) || Convert.ToInt32(bladderCountRH) <= Convert.ToInt32(maxHeat))
                            {
                                drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                                drow[1] = wcName;
                                drow[2] = bladderCountLH;
                                drow[3] = bladderCountRH;
                                drow[4] = BladderTargetLifeLH;
                                drow[5] = BladderTargetLifeRH;
                                flag = 1;
                            }
                        }
                        else
                        {
                            drow[0] = uniqwcGroupdt.Rows[i]["iD"];
                            drow[1] = wcName;
                            drow[2] = bladderCountLH;
                            drow[3] = bladderCountRH;
                            drow[4] = BladderTargetLifeLH;
                            drow[5] = BladderTargetLifeRH;
                            flag = 1;
                        }
                        if (flag == 1) gridviewdt.Rows.Add(drow);

                    }
                    bladderReportDateWiseGridView.DataSource = gridviewdt;
                    bladderReportDateWiseGridView.DataBind();
                }
                else
                {
                    bladderReportDateWiseGridView.DataSource = null;
                    bladderReportDateWiseGridView.DataBind();
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
        //Created by sarita
        //Date: 28-11-2015
        protected void exptoexcel_Click(object sender, EventArgs e)
        {
            string type = curingDropDownList.SelectedItem.Value;
            string size = mSize.SelectedItem.Value;

            Response.Clear();
            string filename = "BladderReport_" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            //stringWrite.Write("<table><tr><td><b>Bladder  Report</b></td><td><b>Bladder  Size :</b>" + mSize.SelectedItem.Value + "</td><td><b>Type :</b> " + Bladder curingDropDownList.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr></table>");
            stringWrite.Write("<table><tr><td><b>Bladder Report</b></td><td><b>Bladder Size :</b>" + mSize.SelectedItem.Value + "</td><td><b>Type :</b> " + curingDropDownList.SelectedItem.Value + "</td><td><b>" + DateTime.Now.ToString() + "</b></td></tr><tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr><tr style=\"background-color:orange;\"><td><b>Work center Name</b></td><td><b>Side :</b></td><td><b>Current Bladder  Code  :</b></td><td><b>Current Bladder  Size</b></td><td><b>Current Bladder  Heat</b></td><td><b>OLD Bladder  Code</b></td><td><b>OLD Bladder  Size</b></td><td><b>OLD Bladder  Heat</b></td> </tr></table>");


            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            bladderReportDateWiseGridView.RenderControl(htmlWrite);
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
