using System;
using System.Collections.Generic;

namespace ConsoleApp.Model
{
    public partial class NewOrderDetails
    {
        public int OrderId { get; set; }
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
