using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace ConsoleApp.BAL
{
    public class BatchExecutionBAL : IDisposable
    {

        // private EcommContext ctx = new EcommContext();
        string SQLCon = "";
        public BatchExecutionBAL(string strCon)
        {
            SQLCon = strCon;
            ErrorMsg = new List<string>();
        }
        public List<string> ErrorMsg { get; set; }
        public void ExecuteBatchQuery(List<string> sQueries)
        {
            using (SqlConnection con = new SqlConnection(SQLCon))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd;
                    foreach (string qry in sQueries)
                    {
                        try
                        {
                            cmd = new SqlCommand(qry, con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception innerEx)//Duplicate PK in customer script
                        {
                            if (innerEx.Message.Contains("'PK_Customers'"))
                                continue;
                            else
                            {
                                throw innerEx;
                            }
                        }
                    }
                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "prcProcessOrder";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    ErrorMsg.Add(ex.Message);
                    tran.Rollback();
                }
                finally
                {
                    if (con.State == ConnectionState.Open) con.Close();
                }
            }
        }

        public void Dispose()
        {

        }
    }
}