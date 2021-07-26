using ConsoleApp.BAL;
using ConsoleApp.Model;
using System.Collections.Generic;
using System;
using ConsoleApp.CustomModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ConsoleApp.Helper
{
    public class OrderHelper : IDisposable
    {
        ValidationContext validationContext;
        List<ValidationResult> results;

        public NewOrderModel GetData(int Id)
        {
            using (OrderBAL bal = new OrderBAL())
            {
                return bal.GetNewOrderModelDataFromId(Id);
            }
        }
        public List<NewOrders> GetList()
        {
            using (OrderBAL bal = new OrderBAL())
            {
                return bal.GetNewOrderList();
            }
        }
        public string SaveData(NewOrderModel model)
        {
            using (OrderBAL bal = new OrderBAL())
            {
                validationContext = new ValidationContext(model);
                results = new List<ValidationResult>();
                if (Validator.TryValidateObject(model, validationContext, results, true))
                {
                    if (bal.SaveData(model))
                        return "";
                    else
                        return string.Join(",", bal.ErrorMsg.ToArray());
                }
                else
                {
                    return string.Join(",", results.Select(p => p.ErrorMessage).ToArray());
                }
            }
        }
        public string DeleteData(int Id)
        {
            using (OrderBAL bal = new OrderBAL())
            {
                if (bal.DeleteData(Id))
                    return "";
                else
                    return string.Join(",", bal.ErrorMsg.ToArray());
            }
        }
        public void Dispose()
        {

        }
    }
}