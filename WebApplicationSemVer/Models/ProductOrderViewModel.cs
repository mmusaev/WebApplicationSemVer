using System;

namespace WebApplicationSemVer.Models
{
    public class ProductOrderViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int TotalPrice { get; set; }
    }
}
