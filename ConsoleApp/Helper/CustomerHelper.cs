using ConsoleApp.BAL;
using ConsoleApp.Model;
using System.Collections.Generic;
using System;
namespace ConsoleApp.Helper
{
    public class CustomerHelper : IDisposable
    {
        public Customers GetData(int Id)
        {
            using (CustomerBAL cb = new CustomerBAL())
            {
                return cb.GetActiveDataFromId(Id);
            }
        }
        public List<Customers> GetList()
        {
            using (CustomerBAL cb = new CustomerBAL())
            {
                return cb.GetCustomerList();
            }
        }
        public string SaveData(Customers model)
        {
            using (CustomerBAL cb = new CustomerBAL())
            {
                if (cb.SaveData(model))
                    return "";
                else
                    return string.Join(",", cb.ErrorMsg.ToArray());
            }
        }
        public string DeleteData(int Id)
        {
            using (CustomerBAL cb = new CustomerBAL())
            {
                if (cb.DeleteData(Id))
                    return "";
                else
                    return string.Join(",", cb.ErrorMsg.ToArray());
            }
        }

        public void Dispose()
        {

        }
    }
}