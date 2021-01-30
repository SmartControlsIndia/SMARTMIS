using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMIS.Production
{
    public class Production
    {
        #region Constructors

        public Production()
        {

        }

        #endregion
        myConnection myConnection = new myConnection();

        #region Functions

        public void TransferData(int wcID, int recipeID, int quantity, int shiftID, DateTime dtandTime, DateTime calcDtandTime)
        {
            try
            {
                myConnection.open(ConnectionOption.SQL);
                myConnection.comm = myConnection.conn.CreateCommand();

                myConnection.comm.CommandText = "sp_ProductionActual";
                myConnection.comm.CommandType = System.Data.CommandType.StoredProcedure;

                System.Data.SqlClient.SqlParameter wcParameter = new System.Data.SqlClient.SqlParameter("@wcID", System.Data.SqlDbType.Int);
                wcParameter.Direction = System.Data.ParameterDirection.Input;
                wcParameter.Value = wcID;

                System.Data.SqlClient.SqlParameter recipeIDParameter = new System.Data.SqlClient.SqlParameter("@recipeID", System.Data.SqlDbType.Int);
                recipeIDParameter.Direction = System.Data.ParameterDirection.Input;
                recipeIDParameter.Value = recipeID;

                System.Data.SqlClient.SqlParameter quantityParameter = new System.Data.SqlClient.SqlParameter("@quantity", System.Data.SqlDbType.Int);
                quantityParameter.Direction = System.Data.ParameterDirection.Input;
                quantityParameter.Value = quantity;

                System.Data.SqlClient.SqlParameter shiftIDParameter = new System.Data.SqlClient.SqlParameter("@shiftID", System.Data.SqlDbType.Int);
                shiftIDParameter.Direction = System.Data.ParameterDirection.Input;
                shiftIDParameter.Value = shiftID;

                System.Data.SqlClient.SqlParameter dtandTimeParameter = new System.Data.SqlClient.SqlParameter("@dtandTime", System.Data.SqlDbType.DateTime);
                dtandTimeParameter.Direction = System.Data.ParameterDirection.Input;
                dtandTimeParameter.Value = dtandTime;

                System.Data.SqlClient.SqlParameter calcDtandTimeParameter = new System.Data.SqlClient.SqlParameter("@calcDtandTime", System.Data.SqlDbType.DateTime);
                calcDtandTimeParameter.Direction = System.Data.ParameterDirection.Input;
                calcDtandTimeParameter.Value = calcDtandTime;

                myConnection.comm.Parameters.Add(wcParameter);
                myConnection.comm.Parameters.Add(recipeIDParameter);
                myConnection.comm.Parameters.Add(quantityParameter);
                myConnection.comm.Parameters.Add(shiftIDParameter);
                myConnection.comm.Parameters.Add(dtandTimeParameter);
                myConnection.comm.Parameters.Add(calcDtandTimeParameter);

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

        #endregion
    }
}
