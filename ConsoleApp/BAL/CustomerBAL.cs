using ConsoleApp.Model;
using System.Collections.Generic;
using System.Linq;
using System;
namespace ConsoleApp.BAL
{
    public class CustomerBAL : BaseBAL<Customers>
    {
        public List<string> ErrorMsg { get; set; }
        public CustomerBAL()
        {
            ctx = new EcommContext();
        }
        public List<Customers> GetCustomerList()
        {
            return ctx.Customers.ToList();
        }
        public List<Customers> GetActiveList()
        {
            return ctx.Customers.Where(p => p.StatusCode == "ACTIVE").ToList();
        }
        public Customers GetDataFromId(int customerId)
        {
            return ctx.Customers.Where(p => p.CustomerId == customerId).FirstOrDefault();
        }
        public Customers GetActiveDataFromId(int customerId)
        {
            return ctx.Customers.Where(p => p.CustomerId == customerId).FirstOrDefault();
        }
        public bool SaveData(Customers model)
        {
            if (model == null)
            {
                ErrorMsg.Add("No data to save");
                return false;
            }
            else
            {
                if (model.CustomerId == 0)
                    return AddData(model);
                else
                    return UpdateData(model);
            }
        }

        protected override bool AddData(Customers model)
        {
            try
            {
                ctx.Customers.Add(model);
                if (base.SaveData() > 0)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return false;
            }
        }
        protected override bool UpdateData(Customers model)
        {
            try
            {
                ctx.Customers.Update(model);
                if (base.SaveData() > 0)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return false;
            }
        }

        public override bool DeleteData(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    Customers model = ctx.Customers.Where(predicate => predicate.CustomerId == Id).FirstOrDefault();
                    if (model != null)
                    {
                        ctx.Customers.Remove(model);
                        if (base.SaveData() > 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return false;
            }
        }
    }
}