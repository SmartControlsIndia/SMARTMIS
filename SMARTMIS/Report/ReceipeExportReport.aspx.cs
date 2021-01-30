using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System.IO;

namespace SmartMIS.Report
{
    public partial class ReceipeExportReport : System.Web.UI.Page
    {
        smartMISWebService myWebService = new smartMISWebService();
        myConnection myConnection = new myConnection();

        string moduleName = "RecipeMaster";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGridView();
            }
        }
        DataTable exldt;
        private void fillGridView()
        {

            //Description   : Function for filling wcGridView
            //Author        : Brajesh Kumar  ||
            //Date Created  : 10 feb 2012 ||
            //Date Updated  : 10 feb 2012 || 11 April
            //Revision No.  : 01            || 02
            //Revision Desc :               || Change the logic for filling wcGridView by webservice
            DataTable gridviewdt = new DataTable();
            gridviewdt.Columns.Add("iD", typeof(int));
            gridviewdt.Columns.Add("recipeName", typeof(string));
            gridviewdt.Columns.Add("SAPMaterialCode", typeof(string));
            gridviewdt.Columns.Add("description", typeof(string));
            DataTable tbldt = myWebService.fillGridView("select iD ,recipeName,SAPMaterialCode,description  from vRecipeMaster", "iD", smartMISWebService.order.Desc);
            recipeGridView.DataSource = tbldt;
            recipeGridView.DataBind();

            exldt = tbldt.Copy();
            exldt.Columns[0].DataType = typeof(int);
            exldt.Load(gridviewdt.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);




            ViewState["dt"] = exldt;
        }
        protected void expToExcel_Click(object sender, EventArgs e)
        {


            // getdisplaytype = optionDropDownList.SelectedItem.Text;

            DataTable dt = (DataTable)ViewState["dt"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Receipe.xls");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ReceipeMaster");
            ws.Cells["A1"].Value = "Receipe Master";

            using (ExcelRange r = ws.Cells["A1:I1"])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Italic));
                r.Style.Font.Color.SetColor(Color.White);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
            }


            ws.Cells["A3"].LoadFromDataTable((DataTable)ViewState["dt"], true, OfficeOpenXml.Table.TableStyles.Light1);
            ws.Cells.AutoFitColumns();


           




            var ms = new MemoryStream();
            pck.SaveAs(ms);
            ms.WriteTo(Response.OutputStream);

            Response.Flush();
            Response.End();


        }
    }
}
