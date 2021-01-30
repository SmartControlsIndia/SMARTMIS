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
                doworkfunc();

                ReportDocument rptdoc = new ReportDocument();
                rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
                rptdoc.SetDataSource(fulldatatable);
                CrystalReportViewer1.ReportSource = rptdoc;

            }
           
           
        }

        protected void ViewButton_Click(object sender, EventArgs e)
        {
            doworkfunc();

             ReportDocument rptdoc = new ReportDocument();
            rptdoc.Load(Server.MapPath("~/crystalreport/gtrejectioncrpt.rpt"));
            rptdoc.SetDataSource(fulldatatable);
            CrystalReportViewer1.ReportSource=rptdoc;
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
                foreach (DataRow dr in rejectiondt.Select("gtbarcode = " + barcode[i] + ""))
                {
                    row["Status"] = dr["status"];
                    htmlTable.Append("<td class=tablecolumn>" + dr["status"] + "</td>");
                    row["DefectName"] = dr["defectName"];
                    htmlTable.Append("<td class=tablecolumn>" + dr["defectName"] + "</td>");
                    row["InspectorName"] = dr["firstName"] + " " + dr["lastname"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["firstName"] + " " + dr["lastname"] + "</td>");
                    row["GTbarcode"] = dr["GTbarcode"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["GTbarcode"] + "</td>");
                    row["InspectionDate"] = dr["dtandTime"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["dtandTime"] + "</td>");
                }
                foreach (DataRow dr in tbmdt.Select("gtbarcode =" + barcode[i] + ""))
                {
                    row["TBM WCName"] = dr["wcName"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["wcName"] + "</td>");
                    row["Recipecode"] = dr["Recipecode"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["Recipecode"] + "</td>");
                    row["Date O.P."] = dr["DOP"];

                    htmlTable.Append("<td class=tablecolumn>" + dr["DOP"] + "</td>");
                    row["Shift"] = " ";
                    htmlTable.Append("<td class=tablecolumn>" + " " + "</td>");

                    var results = (from DataRow myRow in manningdt.Rows
                                  where (int)myRow["ID"] ==(int)dr["manningID"]
                                  select myRow).ToArray();


                    row["Builder1"] = results[0][2].ToString() + " " + results[0][3].ToString();

                    htmlTable.Append("<td class=tablecolumn>" + results[0][2].ToString() + " " + results[0][3].ToString() + "</td>");
                    var results1 = (from DataRow myRow in manningdt.Rows
                                  where (int)myRow["ID"] == (int)dr["manningID"]
                                  select myRow).ToArray();


                    row["Builder2"] = results1[0][2].ToString() + " " + results1[0][3].ToString();

                    htmlTable.Append("<td class=tablecolumn>" + results1[0][2].ToString() + " " + results1[0][3].ToString() + "</td>");
                    var results2 = (from DataRow myRow in manningdt.Rows
                                   where (int)myRow["ID"] == (int)dr["manningID"]
                                   select myRow).ToArray();

                    row["Builder3"] = results2[0][2].ToString() + " " + results2[0][3].ToString();

                    htmlTable.Append("<td class=tablecolumn>" + results2[0][2].ToString() + " " + results2[0][3].ToString() + "</td>");

                }
                fulldatatable.Rows.Add(row);
                htmlTable.Append("</tr>");

            }
            htmlTable.Append("</table>");

            ViewState["htmltable"] = htmlTable;
            ViewState["fulldatatable"] = fulldatatable;

            GTRejectiondataplaceholder.Controls.Clear();
            GTRejectiondataplaceholder.Controls.Add(new Literal { Text = htmlTable.ToString() });

         


        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void ExportExcel_Click(object sender, EventArgs e)
        {

            string aa = ViewState["htmltable"].ToString();

        }



    }
}
