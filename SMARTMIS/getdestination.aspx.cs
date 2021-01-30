using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;

namespace SmartMIS
{
    public partial class getdestination : System.Web.UI.Page
    {

        public static int countreq = 0;
        public static int count2ndline = 0;
        public static int[] shearorecipeWiseCounter = new int[200];

        public static int[] recipeWiseCounter = new int[200];
        public static int[] stationwiseCounter = new int[12];
        public static int[] tempaaray;

        DataTable SecondVIConfig = new DataTable();
        DataTable NCMRConfig = new DataTable(); //created by sachin ## DateTime : 24-12-2020
        DataTable GradeShearography = new DataTable(); //created by sachin ## DateTime : 24-12-2020
        DataTable shearoConfig = new DataTable();
        DataTable ShreographyTemptable = new DataTable();
        myConnection myConnection = new myConnection();
        smartMISWebService mywebservice = new smartMISWebService();
        int stationNO = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                string live = Request.QueryString["live"];
                if (live == "1")
                {
                    Response.Write("OK");
                }

                else
                {

                    string station = Request.QueryString["station"];
                    string parcellID = Request.QueryString["ID"];
                    string gtbarcode = Request.QueryString["scan"];
                    string weight = Request.QueryString["Weight"];
                    string finaldestination = "";
                    if (weight == null)
                        weight = "";

                    if (gtbarcode != null)
                    {
                        if (gtbarcode.Length == 11)
                            gtbarcode = gtbarcode.Substring(0, 10);
                        if (station != null)
                            stationNO = Convert.ToInt32(station);

                        #region Station - 1
                        if (station == "1")
                        {
                            if (gtbarcode.Contains("?"))
                            {
                                Response.Write("station=" + station + "&ID=" + parcellID + "&destination=??");
                                finaldestination = "??";
                            }
                            else
                            {

                                finaldestination = "0" + getdata(gtbarcode, stationNO);
                                if (finaldestination.Contains("?"))
                                    finaldestination = "??";
                                Response.Write("station=" + station + "&ID=" + parcellID + "&destination=" + finaldestination + "");

                            }
                            mywebservice.saveBuddedata(gtbarcode, station, finaldestination, weight);

                        }
                        #endregion
                        #region Station - 2

                        else if (station == "2")
                        {
                           
                            weight = "";
                            if (gtbarcode.Contains("?"))
                            {
                                Response.Write("station=" + station + "&ID=" + parcellID + "&destination=??");
                                finaldestination = "??";
                            }
                           else
                            {
                                getBarcodeInNCMRArea(gtbarcode);
                                getGradeShearographyArea(gtbarcode);
                                if (NCMRConfig.Rows.Count > 0 && GradeShearography.Rows.Count == 0)
                                {
                                    // code for respective unloading station
                                    Response.Write("station=2&ID=" + parcellID + "&destination=01");
                                    finaldestination = "01";
                                    mywebservice.saveBuddedata(gtbarcode, "2", finaldestination, weight);
                                }
                               else
                                {   
                                    getShearoConfig();
                                    int aa = 0;
                                    string gtrecipe = getrecipe(gtbarcode);

                                    int tyretesting = 0;
                                    int recipewisecountervalue = 0;
                                    int count = 0;

                                    for (int i = 0; i < shearoConfig.Rows.Count; i++)
                                    {

                                        if (gtrecipe == shearoConfig.Rows[i][0].ToString())
                                        {
                                            count = i;
                                            tyretesting = Convert.ToInt32(shearoConfig.Rows[i][1]);
                                            recipewisecountervalue = shearorecipeWiseCounter[i];
                                            recipewisecountervalue++;
                                            shearorecipeWiseCounter[i] = recipewisecountervalue;
                                            break;
                                        }
                                    }

                                    if (tyretesting != 0 && (recipewisecountervalue >= 100 / tyretesting))
                                    {
                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=01");
                                        shearorecipeWiseCounter[count] = 0;
                                        finaldestination = "01";
                                    }

                                    else
                                    {
                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=00");
                                        finaldestination = "00";
                                    }
                                    mywebservice.saveBuddedata(gtbarcode, station, finaldestination, weight);

                                }

                            }
                        }
                       
                        #endregion
                        #region Station - 3
                        else if (station == "3")
                        {
                                getBarcodeInNCMRArea(gtbarcode);
                                                               
                                getGradeShearographyArea(gtbarcode);

                                if (NCMRConfig.Rows.Count > 0)
                                {
                                    if (GradeShearography.Rows.Count > 0 && (GradeShearography.Rows[0]["Grade"].ToString() == "A"))
                                    {
                                        #region
                                        //exiting code

                                        weight = "";
                                        if (gtbarcode.Contains("?"))
                                        {
                                            Response.Write("station=" + station + "&ID=" + parcellID + "&destination=11");
                                            finaldestination = "11";
                                            mywebservice.writeLogs("finaldestination=11", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                        }
                                        else
                                        {
                                            getVIConfig();
                                            int aa = 0;
                                            string gtrecipe = getrecipe(gtbarcode);
                                            mywebservice.writeLogs(gtrecipe + "_gtrecipe_" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            bool isunloadbarcode = getunloadbarcode(gtbarcode);
                                            mywebservice.writeLogs(isunloadbarcode + "_isunloadbarcode_" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            bool flag = mywebservice.IsRecordExist("BuddeScannedTyreDetail", "gtbarcode", "where gtbarcode='" + gtbarcode + "' and destination='01' and stationNo=3", out aa);
                                            mywebservice.writeLogs("flag_" + flag, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            int tyretesting = 0;
                                            int recipewisecountervalue = 0;
                                            int count = 0;
                                            if (!flag)
                                            {

                                                try
                                                {
                                                    mywebservice.writeLogs("Count_" + SecondVIConfig.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                    for (int i = 0; i < SecondVIConfig.Rows.Count; i++)
                                                    {

                                                        if (gtrecipe == SecondVIConfig.Rows[i][0].ToString().Trim())
                                                        {
                                                            mywebservice.writeLogs("gtrecipe == SecondVIConfig_" + SecondVIConfig.Rows[i][0], System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                            count = i;
                                                            tyretesting = Convert.ToInt32(SecondVIConfig.Rows[i][1]);
                                                            recipewisecountervalue = recipeWiseCounter[i];
                                                            recipewisecountervalue++;
                                                            recipeWiseCounter[i] = recipewisecountervalue;
                                                            mywebservice.writeLogs(SecondVIConfig.Rows[i][0].ToString() + "_gtrecipe_" + tyretesting, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                            break;
                                                        }
                                                    }
                                                }
                                                catch (Exception exc)
                                                {
                                                    mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                            }
                                            mywebservice.writeLogs("tyretesting=" + tyretesting + "&recipewisecountervalue=" + recipewisecountervalue + "&isunloadbarcode=" + isunloadbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                            if ((tyretesting != 0 && !flag && (recipewisecountervalue >= 100 / tyretesting)) || isunloadbarcode)
                                            {
                                                try
                                                {
                                                    Response.Write("station=" + station + "&ID=" + parcellID + "&destination=01");
                                                    recipeWiseCounter[count] = 0;
                                                    finaldestination = "01";
                                                    mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=01", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                                catch (Exception exc)
                                                {
                                                    mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                            }

                                            else
                                            {
                                                try
                                                {
                                                    int stationwisetempcount = 0;
                                                    int tempdestination = Convert.ToInt32(getdata(gtbarcode, stationNO));
                                                    int destination = tempdestination + 1;

                                                    mywebservice.writeLogs("stationNo=" + stationNO + "&destination=" + destination + "&gtbarcode" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                    if (tempdestination == 0)
                                                    {
                                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=11");
                                                        stationwisetempcount = stationwiseCounter[10];
                                                        stationwiseCounter[10] = stationwisetempcount + 1;
                                                        finaldestination = "11";
                                                        mywebservice.writeLogs("tempdestination == 0station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                    }
                                                    else if (destination < 10)
                                                    {
                                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=0" + destination + "");
                                                        stationwisetempcount = stationwiseCounter[tempdestination];
                                                        stationwiseCounter[tempdestination] = stationwisetempcount + 1;
                                                        finaldestination = "0" + destination;
                                                        mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                    }
                                                    else if (destination >= 10)
                                                    {
                                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "");
                                                        stationwisetempcount = stationwiseCounter[tempdestination];
                                                        stationwiseCounter[tempdestination] = stationwisetempcount + 1;
                                                        finaldestination = destination.ToString();
                                                        mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                    }
                                                }
                                                catch (Exception exc)
                                                {
                                                    mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }

                                            }

                                        }
                                        mywebservice.saveBuddedata(gtbarcode, station, finaldestination, weight);
#endregion
                                    }
                                    else
                                    {
                                        // second line vi

                                        // code for respective unloading station
                                        Response.Write("station=3&ID=" + parcellID + "&destination=01");
                                        finaldestination = "01";
                                        mywebservice.saveBuddedata(gtbarcode, "3", finaldestination, weight);
                                    }
                                }
                                else
                                {
                                    #region
                                    //exiting code

                                    weight = "";
                                    if (gtbarcode.Contains("?"))
                                    {
                                        Response.Write("station=" + station + "&ID=" + parcellID + "&destination=11");
                                        finaldestination = "11";
                                        mywebservice.writeLogs("finaldestination=11", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                    }
                                    else
                                    {
                                        getVIConfig();
                                        int aa = 0;
                                        string gtrecipe = getrecipe(gtbarcode);
                                        mywebservice.writeLogs(gtrecipe + "_gtrecipe_" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                        bool isunloadbarcode = getunloadbarcode(gtbarcode);
                                        mywebservice.writeLogs(isunloadbarcode + "_isunloadbarcode_" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                        bool flag = mywebservice.IsRecordExist("BuddeScannedTyreDetail", "gtbarcode", "where gtbarcode='" + gtbarcode + "' and destination='01' and stationNo=3", out aa);
                                        mywebservice.writeLogs("flag_" + flag, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                        int tyretesting = 0;
                                        int recipewisecountervalue = 0;
                                        int count = 0;
                                        if (!flag)
                                        {

                                            try
                                            {
                                                mywebservice.writeLogs("Count_" + SecondVIConfig.Rows.Count, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                for (int i = 0; i < SecondVIConfig.Rows.Count; i++)
                                                {

                                                    if (gtrecipe == SecondVIConfig.Rows[i][0].ToString().Trim())
                                                    {
                                                        mywebservice.writeLogs("gtrecipe == SecondVIConfig_" + SecondVIConfig.Rows[i][0], System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                        count = i;
                                                        tyretesting = Convert.ToInt32(SecondVIConfig.Rows[i][1]);
                                                        recipewisecountervalue = recipeWiseCounter[i];
                                                        recipewisecountervalue++;
                                                        recipeWiseCounter[i] = recipewisecountervalue;
                                                        mywebservice.writeLogs(SecondVIConfig.Rows[i][0].ToString() + "_gtrecipe_" + tyretesting, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                        break;
                                                    }
                                                }
                                            }
                                            catch (Exception exc)
                                            {
                                                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            }
                                        }
                                        mywebservice.writeLogs("tyretesting=" + tyretesting + "&recipewisecountervalue=" + recipewisecountervalue + "&isunloadbarcode=" + isunloadbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                        if ((tyretesting != 0 && !flag && (recipewisecountervalue >= 100 / tyretesting)) || isunloadbarcode)
                                        {
                                            try
                                            {
                                                Response.Write("station=" + station + "&ID=" + parcellID + "&destination=01");
                                                recipeWiseCounter[count] = 0;
                                                finaldestination = "01";
                                                mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=01", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            }
                                            catch (Exception exc)
                                            {
                                                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            }
                                        }

                                        else
                                        {
                                            try
                                            {
                                                int stationwisetempcount = 0;
                                                int tempdestination = Convert.ToInt32(getdata(gtbarcode, stationNO));
                                                int destination = tempdestination + 1;

                                                mywebservice.writeLogs("stationNo=" + stationNO + "&destination=" + destination + "&gtbarcode" + gtbarcode, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

                                                if (tempdestination == 0)
                                                {
                                                    Response.Write("station=" + station + "&ID=" + parcellID + "&destination=11");
                                                    stationwisetempcount = stationwiseCounter[10];
                                                    stationwiseCounter[10] = stationwisetempcount + 1;
                                                    finaldestination = "11";
                                                    mywebservice.writeLogs("tempdestination == 0station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                                else if (destination < 10)
                                                {
                                                    Response.Write("station=" + station + "&ID=" + parcellID + "&destination=0" + destination + "");
                                                    stationwisetempcount = stationwiseCounter[tempdestination];
                                                    stationwiseCounter[tempdestination] = stationwisetempcount + 1;
                                                    finaldestination = "0" + destination;
                                                    mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                                else if (destination >= 10)
                                                {
                                                    Response.Write("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "");
                                                    stationwisetempcount = stationwiseCounter[tempdestination];
                                                    stationwiseCounter[tempdestination] = stationwisetempcount + 1;
                                                    finaldestination = destination.ToString();
                                                    mywebservice.writeLogs("station=" + station + "&ID=" + parcellID + "&destination=" + destination + "", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                                }
                                            }
                                            catch (Exception exc)
                                            {
                                                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                                            }

                                        }

                                    }
                                    mywebservice.saveBuddedata(gtbarcode, station, finaldestination, weight);
                                    #endregion
                                }

                              
                                //if ( GradeShearography.Rows.Count > 0 && (GradeShearography.Rows[0][0].ToString() == "B" || GradeShearography.Rows[0][0].ToString() == "C"))
                                //{
                                //    // code for respective unloading station
                                //    Response.Write("station=3&ID=" + parcellID + "&destination=01");
                                //    finaldestination = "01";
                                //    mywebservice.saveBuddedata(gtbarcode, "3", finaldestination, weight);
                                //}
                                //else
                                //{
                                    
                                //}   
                         
                              }
                        #endregion
                    }
                    else
                    {
                        string msg = "Testing_" + station + "_" + parcellID + "_" + gtbarcode + "_" + weight;
                        mywebservice.writeLogs(msg, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
                    }
                }
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }

        }

        private void getBarcodeInNCMRArea(string gtbarcode)
        { 
              try
            {
                NCMRConfig.Clear();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select top 1 status from TBRVisualInspection where gtbarcode='" + gtbarcode + "' and status='35'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                NCMRConfig.Load(myConnection.reader);
                mywebservice.writeLogs("NCMRConfig", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
        
        }

        private void getGradeShearographyArea(string gtbarcode)
        {
            try
            {
                GradeShearography.Clear();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select top 1 * from ShearographyData where barcode='" + gtbarcode + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                GradeShearography.Load(myConnection.reader);
                mywebservice.writeLogs("GradeShearography", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }

        }


        private void deleteShreographyTemptable(string gtbarcode)
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "delete from Shreographytyretable where barcode='" + gtbarcode + "' ";
                myConnection.comm.ExecuteNonQuery();
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
        }
        private string getdata(string gtbarcode, int stationNO)
        {
            string flag = "0";
            int minimumvalue = 0;

            // Array.Clear(tempaaray, 0, tempaaray.Length);
            DataTable dt = new DataTable();
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "sp_BuddeTUO";
                myConnection.comm.CommandType = CommandType.StoredProcedure;
                System.Data.SqlClient.SqlParameter GTBarcodeParameter = new System.Data.SqlClient.SqlParameter("@GTBarcode", System.Data.SqlDbType.VarChar);
                GTBarcodeParameter.Direction = System.Data.ParameterDirection.Input;
                GTBarcodeParameter.Value = gtbarcode;
                System.Data.SqlClient.SqlParameter stationNOparameter = new System.Data.SqlClient.SqlParameter("@stationNo", System.Data.SqlDbType.Int);
                stationNOparameter.Direction = System.Data.ParameterDirection.Input;
                stationNOparameter.Value = stationNO;
                myConnection.comm.Parameters.Add(GTBarcodeParameter);
                myConnection.comm.Parameters.Add(stationNOparameter);
                myConnection.reader = myConnection.comm.ExecuteReader(CommandBehavior.CloseConnection);
                if (myConnection.reader.HasRows)
                {
                    dt.Load(myConnection.reader);
                    tempaaray = new int[dt.Rows.Count];
                    if (dt.Rows.Count > 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tempaaray[i] = stationwiseCounter[Convert.ToInt32(dt.Rows[i][0])];
                        }
                        minimumvalue = tempaaray.Min();
                        flag = dt.Rows[Array.IndexOf(tempaaray, minimumvalue)][0].ToString();
                    }
                    else
                        flag = dt.Rows[0][0].ToString();
                }
                else
                    flag = "0";
            }
            catch (Exception exc)
            {
                flag = "0";
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            if (flag == "" || flag == null)
                flag = "0";

            return flag;

        }

        private void getVIConfig()
        {
            try
            {
                SecondVIConfig.Clear();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select recipeCode,tyretesting from budde2ndInspectionLookup order by iD asc";
                myConnection.reader = myConnection.comm.ExecuteReader();
                SecondVIConfig.Load(myConnection.reader);
                mywebservice.writeLogs("getVIConfig", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }

        }

        private void getShearoConfig()
        {
            try
            {

                shearoConfig.Clear();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select recipeCode,tyretesting from buddeshearographyLookup order by iD asc";
                myConnection.reader = myConnection.comm.ExecuteReader();
                shearoConfig.Load(myConnection.reader);
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }

        }

        private string getrecipe(string gtbarcode)
        {
            string flag = "";
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select RecipeCode from vtbmTBR where gtbarcode='" + gtbarcode + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();
                while (myConnection.reader.Read())
                {
                    flag = myConnection.reader[0].ToString();
                }
            }
            catch (Exception exc)
            {
                flag = "";
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            mywebservice.writeLogs("getRecipeMethod"+flag, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

            return flag;
            
        }
        private void getShreographyTemptable(string recipeName)
        {
            try
            {
                ShreographyTemptable.Clear();
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
               // myConnection.comm.CommandText = "select rm.name,st.Recipeid,st.barcode from Shreographytyretable st inner join recipeMaster rm on st.Recipeid = rm.iD order by st.id desc";
                myConnection.comm.CommandText = "select top 5 rm.name,st.Recipeid,st.barcode from Shreographytyretable st inner join recipeMaster rm on st.Recipeid = rm.iD where rm.name='" + recipeName + "' order by st.id desc";
                myConnection.reader = myConnection.comm.ExecuteReader();
                ShreographyTemptable.Load(myConnection.reader);
            }
            catch (Exception exc)
            {
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.conn.Close();
            }
            // return flag;
            mywebservice.writeLogs("getShreographyTemptable", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
        }
        private bool getunloadbarcode(string gtbarcode)
        {
            bool flag = false;
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();
                myConnection.comm.CommandText = "select barcode from unloadbarcode where barcode='" + gtbarcode + "'";
                myConnection.reader = myConnection.comm.ExecuteReader();

                if (myConnection.reader.HasRows)
                {

                    while (myConnection.reader.Read())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception exc)
            {
                flag = false;
                mywebservice.writeLogs(exc.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));
            }
            finally
            {
                myConnection.reader.Close();
                myConnection.comm.Dispose();
                myConnection.close(ConnectionOption.SQL);
            }
            return flag;
            mywebservice.writeLogs("getunloadbarcode", System.Reflection.MethodBase.GetCurrentMethod().Name, Path.GetFileName(Request.Url.AbsolutePath));

        }

    }
}
