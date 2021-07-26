using ConsoleApp.BAL;
using System.Collections.Generic;
using System;
using System.Linq;
namespace ConsoleApp.Helper
{
    public class BatchExecutionHelper : IDisposable
    {
        private string ConnString;
        public BatchExecutionHelper(string constr)
        {
            ConnString = constr;
        }
        public string BatchExecutionScript(List<string> sQueries)
        {
            using (BatchExecutionBAL bal = new BatchExecutionBAL(ConnString))
            {
                bal.ExecuteBatchQuery(sQueries);
                if (bal.ErrorMsg.Count > 0)
                {
                    if (bal.ErrorMsg.Any(p => p.Contains("'PK_NewOrders'")))
                        return "Order Already Processed..!";
                    else
                        return string.Join(",", bal.ErrorMsg.ToArray());
                }
                else
                    return "";
            }
        }

        public void Dispose()
        {

        }
    }
}