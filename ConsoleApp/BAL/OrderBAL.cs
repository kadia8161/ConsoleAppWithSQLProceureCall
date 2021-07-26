using ConsoleApp.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using ConsoleApp.CustomModel;
namespace ConsoleApp.BAL
{
    public class OrderBAL : BaseBAL<NewOrders>
    {

        CustomerBAL customerBAL = new CustomerBAL();
        OrderDetailsBAL orderDetailsBAL = new OrderDetailsBAL();
        public List<string> ErrorMsg { get; set; }
        public OrderBAL()
        {
            ctx = new EcommContext();
        }
        public List<NewOrders> GetNewOrderList()
        {
            return ctx.NewOrders.ToList();
        }
        public NewOrders GetNewOrderDataFromId(int orderId)
        {
            return ctx.NewOrders.Where(p => p.OrderId == orderId).FirstOrDefault();
        }

        public NewOrderModel GetNewOrderModelDataFromId(int orderId)
        {
            NewOrderModel model = new NewOrderModel();
            return ctx.NewOrders.Where(p => p.OrderId == orderId).Select
            (p =>
                new NewOrderModel()
                {
                    OrderId = p.OrderId,
                    CustomerId = p.CustomerId,
                    StatusCode = p.StatusCode,
                    StatusDescription = p.StatusDescription,
                    TotalAmount = p.TotalAmount,
                    Zipcode = p.Zipcode,
                    SubmitDate = p.SubmitDate,
                    OrderDetails = ctx.NewOrderDetails.Where(x => x.OrderId == p.OrderId).ToList()
                }
            )
            .FirstOrDefault();
        }
        public bool SaveData(NewOrderModel model)
        {
            if (model == null)
            {
                ErrorMsg.Add("No data to save");
                return false;
            }
            else if (model.CustomerId == 0)
            {
                ErrorMsg.Add("Cutomer Missing");
                return false;
            }
            else
            {
                if (customerBAL.GetActiveDataFromId(model.CustomerId) != null)
                {
                    bool IsCommit = true;
                    NewOrders ordermodel = new NewOrders();
                    if (model.OrderId == 0)
                    {
                        ordermodel = SetModel(true, model);
                        using (var dbcxtransaction = ctx.Database.BeginTransaction())
                        {
                            if (AddData(ordermodel))
                            {
                                foreach (NewOrderDetails dtlmodel in model.OrderDetails)
                                {
                                    dtlmodel.OrderId = ordermodel.OrderId;
                                    if (!orderDetailsBAL.SaveData(dtlmodel))
                                    {
                                        ErrorMsg.Add("Problem in Order Detail Entry");
                                        IsCommit = false;
                                        break;
                                    }
                                }
                                if (IsCommit)
                                    dbcxtransaction.Commit();
                                else
                                    dbcxtransaction.Rollback();
                            }
                        }
                    }
                    else
                    {
                        ordermodel = SetModel(false, model);
                        using (var dbcxtransaction = ctx.Database.BeginTransaction())
                        {
                            if (UpdateData(ordermodel))
                            {
                                foreach (NewOrderDetails dtlmodel in model.OrderDetails)
                                {
                                    dtlmodel.OrderId = ordermodel.OrderId;
                                    {
                                        ErrorMsg.Add("Problem in Order Detail Entry");
                                        IsCommit = false;
                                        break;
                                    }
                                }
                                if (IsCommit)
                                    dbcxtransaction.Commit();
                                else
                                    dbcxtransaction.Rollback();
                            }
                        }
                    }
                    return IsCommit;
                }
                else
                {
                    ErrorMsg.Add("Cutomer Inactive");
                    return false;
                }
            }
        }

        private NewOrders SetModel(bool IsAdd, NewOrderModel model)
        {
            NewOrders ordermodel = new NewOrders();
            if (IsAdd)
            {
                ordermodel.OrderId = 0;
            }
            else
            {
                ordermodel = ctx.NewOrders.Where(p => p.OrderId == model.OrderId).FirstOrDefault();
                ordermodel.ModifiedBy = model.ModifiedBy;
                ordermodel.ModifiedDate = System.DateTime.Now;
            }
            ordermodel.CustomerId = model.CustomerId;
            ordermodel.StatusCode = model.StatusCode;
            ordermodel.StatusDescription = model.StatusDescription;
            ordermodel.SubmitDate = model.SubmitDate;
            ordermodel.TotalAmount = model.OrderDetails.Sum(p => p.Price);
            ordermodel.Zipcode = model.Zipcode;
            return ordermodel;
        }
        protected override bool AddData(NewOrders model)
        {
            try
            {
                ctx.NewOrders.Add(model);
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
        protected override bool UpdateData(NewOrders model)
        {
            try
            {
                ctx.NewOrders.Update(model);
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
                        var OrdDtlList = ctx.NewOrderDetails.Where(p => p.OrderId == Id).ToList();
                        ctx.NewOrderDetails.RemoveRange(OrdDtlList);
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

        public new void Dispose()
        {
            if (customerBAL != null)
                customerBAL.Dispose();
            if (orderDetailsBAL != null)
                orderDetailsBAL.Dispose();
            base.Dispose();
        }
    }
}