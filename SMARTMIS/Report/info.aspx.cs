using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace SmartMIS.Report
{
    public partial class info : System.Web.UI.Page
    {
        myConnection myConnection = new myConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageViewLabel.Text = "";
            string htmlStr = "";
            string htmlstrr = "";
            string lot = Request.QueryString["lot"];
            string tbl = Request.QueryString["tbl"];
            string wname = Request.QueryString["wname"];
            string dt = Request.QueryString["dtandTime"];
            string insname = Request.QueryString["inspectorname"];
            myConnection.open(ConnectionOption.SQL);

            if (!string.IsNullOrEmpty(tbl) && (tbl.Equals("vfinalfinishTBR") || tbl.Equals("vfinalfinishPCR")))
            {
                if (tbl.Equals("vfinalfinishTBR"))
                    tbl = "vfinalfinishTBR";
                else if(tbl.Equals("vfinalfinishPCR"))
                    tbl = "vfinalfinishPCR";
                try
                {
                    myConnection.comm = myConnection.conn.CreateCommand();

                    myConnection.comm.CommandText = "SELECT DISTINCT gtBarcode, wcID, name, firstName,lastName, lot, dtandTime FROM " + tbl + " WHERE lot = '" + lot + "'";
                    myConnection.reader = myConnection.comm.ExecuteReader();

                    htmlstrr += "<table id=\"table2\" class=\"innerTable\" width=\"1\"><tr><td><br/></td></tr><tr><td class=\"FinalFinishgridViewHeader\">Lot Number</td><td class=\"FinalFinishgridViewHeader\">Work station</td><td class=\"FinalFinishgridViewHeader\">Inspector</td><td class=\"FinalFinishgridViewHeader\">Date and Time</td></tr>";
                    htmlstrr += "<tr style=\"color:#333333;background-color:#F7F6F3;\"><td><span class=\"gridViewItems\">" + lot + "</span></td><td><span class=\"gridViewItems\">" + wname + "</span></td><td><span class=\"gridViewItems\">" + insname + "</span></td><td><span class=\"gridViewItems\">" + dt + "</span></td></tr><tr><td><br/><br/></td></tr></table>";
                    summarylabel.Text = htmlstrr.ToString();
                    if (myConnection.reader.HasRows)
                    {
                        htmlStr += "<table id=\"table1\" class=\"innerTable\" width=\"1\"><tr><td class=\"FinalFinishgridViewHeader\">BarCode</td><td class=\"FinalFinishgridViewHeader\">Recipe Code</td><td class=\"FinalFinishgridViewHeader\">Defect Area Name</td><td class=\"FinalFinishgridViewHeader\">Defect Name</td><td class=\"FinalFinishgridViewHeader\">Remarks</td></tr>";
                        while (myConnection.reader.Read())
                        {
                            string wcid = myConnection.reader["wcID"].ToString();
                            string gtBarcode = myConnection.reader["gtBarcode"].ToString();
                            
                            string newdata = getmydata(gtBarcode, wcid, tbl);
                            htmlStr += "<tr style=\"color:#333333;background-color:#F7F6F3;\"><td><a href=\"tyreGeneaology.aspx?gtbarcode="+gtBarcode+"\" target=\"_blank\"><span class=\"gridViewItems\">" + gtBarcode + "</span></a></td>" + newdata + "</tr>";
                        }
                        htmlStr += "</table>";
                        InfoDataViewLabel.Text = htmlStr;
                    }
                    myConnection.close(ConnectionOption.SQL);
                }
                catch (Exception exp)
                {
                    MessageViewLabel.Text = exp.StackTrace;
                }
                finally
                {
                    //myConnection.comm.Dispose();
                    //myConnection.reader.Close();
                }

            }
            else
                InfoDataViewLabel.Text = "No Data To Display";
            
        }

        public string getmydata(string barcode,string wcid,string tbl)
        {
            if (tbl.Equals("vfinalfinishTBR"))
                tbl = "TBR";
            else if (tbl.Equals("vfinalfinishPCR"))
                tbl = "PCR";
            string str = "";

            SqlConnection con = new SqlConnection();
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["mySQLConnection"].ToString();
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT recipeCode FROM vTbm" + tbl + " WHERE gtbarCode = '" + barcode + "'", con);
                var dr = cmd.ExecuteReader();

                // for reciepe column
                string recipe = "";
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        recipe = dr["recipeCode"].ToString();
                    }
                }
                str += "<td><span class=\"gridViewItems\">" + recipe.ToString() + "</span></td>";
                cmd.Dispose();
                dr.Close();
                SqlCommand ncmd;

                if(tbl=="TBR")
                ncmd = new SqlCommand("SELECT defectAreaID, defectname as defectlocation, remarks FROM vTBRVisualInspectionReport WHERE gtbarCode = '" + barcode + "' AND wcid = " + wcid + "", con);
                else
                    ncmd = new SqlCommand("SELECT defectAreaID, defectname as defectlocation, remarks FROM vVisualInspectionPCR WHERE gtbarCode = '" + barcode + "' AND wcid = " + wcid + "", con);            
                var ndr = ncmd.ExecuteReader();
                // for remark and defect status column
                string defectAreaName = "";
                string defectstatus = "";
                string remarks = "";
                int defectAreaID;
                if (ndr.HasRows)
                {
                    if (ndr.Read())
                    {
                        defectAreaID=Convert.ToInt32(ndr["defectAreaID"]);

                        switch (defectAreaID)
                        {
                            case 1:
                                defectAreaName = "Tread";
                                break;
                            case 2:
                                defectAreaName = "SideWall";
                                break;
                            case 3:
                                defectAreaName = "Bead";
                                break;
                            case 4:
                                defectAreaName = "Carcass";
                                break;
                        }

                        defectstatus = ndr["defectlocation"].ToString();
                        remarks = ndr["remarks"].ToString();
                    }
                }
                str += " <td><span class=\"gridViewItems\">" + defectAreaName + "</span></td><td><span class=\"gridViewItems\">" + defectstatus + "</span></td><td><span class=\"gridViewItems\">" + remarks + "</span></td>";
                ncmd.Dispose();
                ndr.Close();

            }
            catch (Exception exp)
            {
                MessageViewLabel.Text = exp.Source;
            }

            finally
            {
                //myConnection.comm.Dispose();
                //myConnection.reader.Close();
               
            }
            con.Close();
            return str;
        }

    }
}
