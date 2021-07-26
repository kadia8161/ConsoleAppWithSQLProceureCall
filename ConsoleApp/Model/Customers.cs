using System;
using System.Collections.Generic;

namespace ConsoleApp.Model
{
    public partial class Customers
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string StatusCode { get; set; }
        public int MinProdQuantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
