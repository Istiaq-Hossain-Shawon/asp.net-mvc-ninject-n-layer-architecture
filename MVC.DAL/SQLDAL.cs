using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace MVC.DAL
{
    public class SQLDAL
    {
        private SqlConnection connection;

        public SQLDAL()
        {
            connection = new SqlConnection(GlobalConnection());
        }

        public string GlobalConnection()
        {
            string entityConnectionString = ConfigurationManager.ConnectionStrings["yourconnectinstringName"].ConnectionString;
            //string providerConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;
            return entityConnectionString;
        }



        #region Query  Execute for sql

        public Result ExecuteQuery(string SQL)
        {
            Result oResult = new Result();
            SqlCommand oCmd = null;

            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oCmd = new SqlCommand(SQL, connection);
                    oCmd.ExecuteNonQuery();
                    oResult.ExecutionState = true;
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }
        public Result ExecuteQuery(List<string> SQL)
        {
            Result oResult = new Result();
            SqlTransaction oTransaction = null;
            SqlCommand oCmd = null;

            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oTransaction = connection.BeginTransaction();
                    foreach (string s in SQL)
                    {
                        oCmd = new SqlCommand(s, connection);
                        oCmd.Transaction = oTransaction;
                        oCmd.ExecuteNonQuery();
                        oResult.ExecutionState = true;
                    }
                    oTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
                oTransaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }
        public Result Select(string SQL)
        {
            Result oResult = new Result();
            SqlCommand oCmd = null;
            try
            {

                if (connection != null)
                {
                    connection.Open();
                    oCmd = new SqlCommand(SQL, connection);
                    oCmd.CommandTimeout = 0;
                    SqlDataAdapter adapter = new SqlDataAdapter(oCmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    oResult.ExecutionState = true;
                    oResult.Data = dt;
                }
            }
            catch (Exception ex)
            {
                oResult.ExecutionState = false;
                oResult.Error = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return oResult;
        }

        #endregion


        #region Query  Execute for oracle

        public Result SelectOracle(string SQL)
        {
            DataTable dt = new DataTable();
            Result oResult = new Result();
            using (OracleConnection connection = new OracleConnection())
            {
                connection.ConnectionString = GlobalConnection();
                connection.Open();
                OracleCommand cmd = new OracleCommand("Select Distinct t.Branch_ID as BRID,t.BRANCH_NM as BRNAME From Branch_Home_Bank t order by t.branch_id");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                using (OracleDataAdapter dataAdapter = new OracleDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(dt);
                }
                connection.Close();
                connection.Dispose();
            }
            oResult.ExecutionState = true;
            oResult.Data = dt;
            return oResult;
        }

        #endregion


        //private static string GetConnectionString()
        //{
        //    string connectionString = WebConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
        //    return connectionString;
        //}
    }
}
