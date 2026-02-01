using System.Collections.Generic;

namespace Database
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
}
