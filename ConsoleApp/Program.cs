using System;
using ConsoleApp.Helper;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                   .AddJsonFile(Environment.CurrentDirectory + "/appsettings.json", false, true)
                   .Build();

            string ConnStr = configuration.GetConnectionString("DefaultConnection");

            string scriptPath = Environment.CurrentDirectory + "/Scripts";
            List<String> lstSQLFiles = new List<string>();
            if (File.Exists(scriptPath + "/" + "Load_Customers.sql"))
            {
                //lstSQLFiles.AddRange(File.ReadAllText(scriptPath + "/" + "Load_Customers.sql", System.Text.Encoding.UTF8).Split(';'));
                lstSQLFiles.Add(File.ReadAllText(scriptPath + "/" + "Load_Customers.sql", System.Text.Encoding.UTF8));
            }
            if (File.Exists(scriptPath + "/" + "Load_NewOrders.sql") && File.Exists(scriptPath + "/" + "Load_NewOrderDetails.sql"))
            {
                // lstSQLFiles.AddRange(File.ReadAllText(scriptPath + "/" + "Load_NewOrders.sql", System.Text.Encoding.UTF8).Split(';'));
                // lstSQLFiles.AddRange(File.ReadAllText(scriptPath + "/" + "Load_NewOrderDetails.sql", System.Text.Encoding.UTF8).Split(';'));
                lstSQLFiles.Add(File.ReadAllText(scriptPath + "/" + "Load_NewOrders.sql", System.Text.Encoding.UTF8));
                lstSQLFiles.Add(File.ReadAllText(scriptPath + "/" + "Load_NewOrderDetails.sql", System.Text.Encoding.UTF8));
            }
            using (BatchExecutionHelper helpr = new BatchExecutionHelper(ConnStr))
            {
                string ErrorMsg = helpr.BatchExecutionScript(lstSQLFiles);
                if (!string.IsNullOrEmpty(ErrorMsg))
                    Console.Write(ErrorMsg);
                else
                    Console.Write("Order Processed..!");
            }

            // string useroption = ""; ;
            // Console.WriteLine("Select Option");
            // Console.WriteLine("1 - Customer");
            // Console.WriteLine("2 - Customer Order");
            // Console.WriteLine("3 - Customer Order Details");
            // useroption = Console.ReadLine();
            // switch (useroption.ToString())
            // {
            //     case "1":
            //         using (CustomerHelper helper = new CustomerHelper())
            //         {
            //             var customers = helper.GetList();

            //         }
            //         break;
            //     default:
            //         Environment.Exit(0);
            //         break;
            // }
        }

        // private static void CusotmerInfo(string inputkey)
        // {
        //     switch (inputkey)
        //     {
        //         case "1":
        //             using (CustomerHelper ch = new CustomerHelper())
        //             {
        //                 ch.GetList();
        //             }
        //             break;
        //         default:
        //             Environment.Exit(0);
        //             break;
        //     }
        // }
    }
}
