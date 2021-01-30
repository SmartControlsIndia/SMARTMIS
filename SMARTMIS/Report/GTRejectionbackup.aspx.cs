using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;



namespace SmartMIS.Report
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        myConnection myconnection = new myConnection();
        StringBuilder htmlTable;
        DataTable rejectiondt;
        DataTable tbmdt;
        DataTable fulldatatable;
        DataTable manningdt;
        DataRow row;

        smartMISWebService mywebservice = new smartMISWebService();
   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //doworkfunc();

                //ReportDocument rptdoc = new ReportDocument();
                //rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
                //rptdoc.SetDataSource(fulldatatable);
                //CrystalReportViewer1.ReportSource = rptdoc;

            }
           
           
        }
        protected void ViewButton_Click(object sender, EventArgs e)
        {
            doworkfunc();

            // ReportDocument rptdoc = new ReportDocument();
            //rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
            //rptdoc.SetDataSource(fulldatatable);
            //CrystalReportViewer1.ReportSource=rptdoc;
        }
        private void doworkfunc()
        {
            htmlTable = new StringBuilder();
            fulldatatable = new DataTable();
            tbmdt = new DataTable();
            rejectiondt = new DataTable();
            manningdt = new DataTable();

            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = "select * from manningmaster";
                myconnection.reader = myconnection.comm.ExecuteReader();
                manningdt.Load(myconnection.reader);
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);
            }
            catch (Exception exc)
            {
 
            }

            try
            {
                myconnection.open(ConnectionOption.SQL);
                myconnection.comm = myconnection.conn.CreateCommand();
                myconnection.comm.CommandText = "select wcName,type as status,defectName,firstName,lastName,gtBarcode,dtandTime from vGTRejection where  wcname='" + processDropDownList.SelectedItem.Text + "' and dtandtime>'" + mywebservice.formatDate(fromdatecalendertextbox.Text) + " " + "07:00:00" + "' and dtandtime<'" + mywebservice.formatDate(TodateCalendertextbox.Text) + " " + "07:00:00" + "'";
                myconnection.reader = myconnection.comm.ExecuteReader();
                rejectiondt.Load(myconnection.reader);
                var barcode = rejectiondt.AsEnumerable().Select(row => row.Field<string>("gtbarcode")).ToArray();
                myconnection.comm.Dispose();
                myconnection.reader.Close();
                myconnection.close(ConnectionOption.SQL);


                if (barcode.Length != 0)
                {
                    string InQuery = "(";
                    for (int i = 0; i < barcode.Length; i++)
                    {
                        InQuery += "'" + barcode[i].ToString() + "',";
                    }
                    InQuery = InQuery.TrimEnd(',');
                    InQuery += ")";

                    myconnection.open(ConnectionOption.SQL);
                    myconnection.comm = myconnection.conn.CreateCommand();
                    myconnection.comm.CommandText = "select GTbarcode, wcName,recipeCode,dtandTime as DOP,manningID,manningID2,manningID3 from vTBMPCR where gtbarcode in " + InQuery + "";
                    myconnection.reader = myconnection.comm.ExecuteReader();
                    tbmdt.Load(myconnection.reader);
                    myconnection.comm.Dispose();
                    myconnection.reader.Close();
                    myconnection.close(ConnectionOption.SQL);
                }


                htmlTable.Append("<table border='1' width=100%>");

                htmlTable.Append("<tr><th class=tableheadercolumn>Status</th><th class=tableheadercolumn>DefectName</th><th class=tableheadercolumn>InspectorName</th><th class=tableheadercolumn>GTbarcode</th><th class=tableheadercolumn>InspectionDate</th><th class=tableheadercolumn>TBM WCName</th><th class=tableheadercolumn>RecipeCode</th><th class=tableheadercolumn>Date O.P.</th><th class=tableheadercolumn>Shift</th><th class=tableheadercolumn>Builder1</th><th class=tableheadercolumn>Builder2</th><th class=tableheadercolumn>Builder3</th></tr>");
                fulldatatable.Columns.Add("Status"); fulldatatable.Columns.Add("DefectName"); fulldatatable.Columns.Add("InspectorName"); fulldatatable.Columns.Add("GTbarcode"); fulldatatable.Columns.Add("InspectionDate"); fulldatatable.Columns.Add("TBM WCName"); fulldatatable.Columns.Add("RecipeCode"); fulldatatable.Columns.Add("Date O.P."); fulldatatable.Columns.Add("Shift"); fulldatatable.Columns.Add("Builder1"); fulldatatable.Columns.Add("Builder2"); fulldatatable.Columns.Add("Builder3");


                for (int i = 0; i < rejectiondt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    row = fulldatatable.NewRow();
                    var dr = rejectiondt.Select("gtbarcode = " + barcode[i] + "");
                    
                       // row["Status"] = dr["status"];
                        htmlTable.Append("<td class=tablecolumn>" + dr[0][1] + "</td>");
                        //row["DefectName"] = dr["defectName"];
                        htmlTable.Append("<td class=tablecolumn>" + dr[0][2] + "</td>");
                        //row["InspectorName"] = dr["firstName"] + " " + dr["lastname"];

                        htmlTable.Append("<td class=tablecolumn>" + dr[0][3] + " " + dr[0][4] + "</td>");
                        //row["GTbarcode"] = dr["GTbarcode"];

                        htmlTable.Append("<td class=tablecolumn>" + dr[0][5] + "</td>");
                       // row["InspectionDate"] = dr["dtandTime"];

                        htmlTable.Append("<td class=tablecolumn>" + dr[0][6] + "</td>");
                    
                        var dr1 = tbmdt.Select("gtbarcode =" + barcode[i] + "");
                    
                        //row["TBM WCName"] = dr["wcName"];

                        htmlTable.Append("<td class=tablecolumn>" + dr1[0][1] + "</td>");
                        //row["Recipecode"] = dr["Recipecode"];

                        htmlTable.Append("<td class=tablecolumn>" + dr1[0][2] + "</td>");
                        //row["Date O.P."] = dr["DOP"];

                        htmlTable.Append("<td class=tablecolumn>" + dr1[0][3] + "</td>");
                        //row["Shift"] = " ";
                        DateTime dop =Convert.ToDateTime(dr1[0][3]);
                        htmlTable.Append("<td class=tablecolumn>" + getshift(dop) + "</td>");

                        var results = manningdt.Select("iD ="+Convert.ToInt32(dr1[0][4]));

                        //row["Builder1"] = results[0][2].ToString() + " " + results[0][3].ToString();

                        htmlTable.Append("<td class=tablecolumn>" + results[0][2].ToString() + " " + results[0][3].ToString() + "</td>");
                        if (dr1[0][5]!=DBNull.Value)
                        {
                            var results1 = manningdt.Select("iD=" + Convert.ToInt32(dr1[0][5]));
                            htmlTable.Append("<td class=tablecolumn>" + results1[0][2].ToString() + " " + results1[0][3].ToString() + "</td>");

                        }
                        else
                            htmlTable.Append("<td class=tablecolumn>"+" "+"</td>");


                       // row["Builder2"] = results1[0][2].ToString() + " " + results1[0][3].ToString();
                        if (dr1[0][6]!=DBNull.Value)
                        {
                            var results2 = manningdt.Select("iD=" + Convert.ToInt32(dr1[0][6]));
                            htmlTable.Append("<td class=tablecolumn>" + results2[0][2].ToString() + " " + results2[0][3].ToString() + "</td>");

                        }
                        else
                            htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");

                       
                        //row["Builder3"] = results2[0][2].ToString() + " " + results2[0][3].ToString();


                    
                    //fulldatatable.Rows.Add(row);
                    htmlTable.Append("</tr>");
            }
                
                htmlTable.Append("</table>");

                //ViewState["htmltable"] = htmlTable;
               // ViewState["fulldatatable"] = fulldatatable;

                GTRejectiondataplaceholder.Controls.Clear();
                GTRejectiondataplaceholder.Controls.Add(new Literal { Text = htmlTable.ToString() });

            }
            catch (Exception exc)
            {
 
            }


        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void ExportExcel_Click(object sender, EventArgs e)
        {

          //  string aa = ViewState["htmltable"].ToString();

        }
        private string getshift(DateTime dt)
        {
            string shift = "";

            if (dt.Hour > 6 && dt.Hour < 15)
                shift = "A";
            else if (dt.Hour > 14 && dt.Hour < 23)
                shift = "B";
            else if (dt.Hour == 23 || (dt.Hour >= 0 && dt.Hour < 7))
                shift = "C";
            return shift;
 
        }



    }
}
