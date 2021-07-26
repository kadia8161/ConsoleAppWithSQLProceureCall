using System;
using System.Collections.Generic;

namespace ConsoleApp.Model
{
    public partial class NewOrders
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SubmitDate { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public decimal TotalAmount { get; set; }
        public string Zipcode { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
