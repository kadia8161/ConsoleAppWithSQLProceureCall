using ConsoleApp.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using ConsoleApp.CustomModel;
namespace ConsoleApp.BAL
{
    public class OrderDetailsBAL : BaseBAL<NewOrderDetails>
    {
        public List<string> ErrorMsg { get; set; }
        public OrderDetailsBAL()
        {
            ctx = new EcommContext();
        }
        public List<NewOrderDetails> GetOrderDetailList(int orderId)
        {
            return ctx.NewOrderDetails.Where(p => p.OrderId == orderId).ToList();
        }
        // public Customers GetCustomerDataFromId(int customerId)
        // {
        //     return ctx.Customers.Where(p => p.CustomerId == customerId).FirstOrDefault();
        // }
        public bool SaveData(NewOrderDetails model)
        {
            if (model == null)
            {
                ErrorMsg.Add("No data to save");
                return false;
            }
            else
            {
                if (model.DetailId == 0)
                    return AddData(model);
                else
                    return UpdateData(model);
            }
        }

        protected override bool AddData(NewOrderDetails model)
        {
            try
            {
                ctx.NewOrderDetails.Add(model);
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
        protected override bool UpdateData(NewOrderDetails model)
        {
            try
            {
                ctx.NewOrderDetails.Update(model);
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
                    NewOrderDetails model = ctx.NewOrderDetails.Where(predicate => predicate.DetailId == Id).FirstOrDefault();
                    if (model != null)
                    {
                        ctx.NewOrderDetails.Remove(model);
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