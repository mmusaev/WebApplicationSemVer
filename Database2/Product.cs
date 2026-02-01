using System;
using System.Collections.Generic;

namespace Database2
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
    }

    public class ProductService
    {
        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999, Category = "Electronics" },
                new Product { Id = 2, Name = "Mouse", Price = 29, Category = "Electronics" },
                new Product { Id = 3, Name = "Keyboard", Price = 79, Category = "Electronics" },
                new Product { Id = 4, Name = "Monitor", Price = 299, Category = "Electronics" },
                new Product { Id = 5, Name = "Desk Chair", Price = 199, Category = "Furniture" }
            };
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }

    public class OrderService
    {
        public List<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order { OrderId = 1001, ProductId = 1, CustomerName = "John Smith", Quantity = 2, OrderDate = new DateTime(2026, 1, 15), Status = "Shipped" },
                new Order { OrderId = 1002, ProductId = 2, CustomerName = "Sarah Johnson", Quantity = 5, OrderDate = new DateTime(2026, 1, 16), Status = "Processing" },
                new Order { OrderId = 1003, ProductId = 3, CustomerName = "Mike Brown", Quantity = 3, OrderDate = new DateTime(2026, 1, 17), Status = "Delivered" },
                new Order { OrderId = 1004, ProductId = 1, CustomerName = "Emily Davis", Quantity = 1, OrderDate = new DateTime(2026, 1, 18), Status = "Shipped" },
                new Order { OrderId = 1005, ProductId = 4, CustomerName = "David Wilson", Quantity = 2, OrderDate = new DateTime(2026, 1, 19), Status = "Processing" },
                new Order { OrderId = 1006, ProductId = 5, CustomerName = "Lisa Anderson", Quantity = 4, OrderDate = new DateTime(2026, 1, 20), Status = "Delivered" },
                new Order { OrderId = 1007, ProductId = 2, CustomerName = "Tom Martinez", Quantity = 10, OrderDate = new DateTime(2026, 1, 21), Status = "Shipped" },
                new Order { OrderId = 1008, ProductId = 3, CustomerName = "Anna Taylor", Quantity = 2, OrderDate = new DateTime(2026, 1, 22), Status = "Processing" }
            };
        }
    }
}
