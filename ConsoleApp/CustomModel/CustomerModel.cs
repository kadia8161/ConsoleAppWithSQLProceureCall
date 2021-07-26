using ConsoleApp.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ConsoleApp.CustomModel
{
    public class CustomerModel : Customers
    {

    }

    public class NewOrderModel : NewOrders
    {
        [Required]
        public new int CustomerId { get; set; }
        public List<NewOrderDetails> OrderDetails { get; set; }
    } 

}