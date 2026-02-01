using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationSemVer.Models;

namespace WebApplicationSemVer.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database.ProductService _productService;
        private readonly Database2.OrderService _orderService;

        public HomeController()
        {
            _productService = new Database.ProductService();
            _orderService = new Database2.OrderService();
        }

        public ActionResult Index()
        {
            var products = _productService.GetProducts();
            var orders = _orderService.GetOrders();

            // Join products with orders
            var productOrders = from order in orders
                               join product in products on order.ProductId equals product.Id
                               select new ProductOrderViewModel
                               {
                                   OrderId = order.OrderId,
                                   ProductId = product.Id,
                                   ProductName = product.Name,
                                   Category = product.Category,
                                   Price = product.Price,
                                   CustomerName = order.CustomerName,
                                   Quantity = order.Quantity,
                                   OrderDate = order.OrderDate,
                                   Status = order.Status,
                                   TotalPrice = product.Price * order.Quantity
                               };

            return View(productOrders.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}