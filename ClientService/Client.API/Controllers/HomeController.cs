using Microsoft.AspNetCore.Mvc;
using Client.API.Models;
using System.Diagnostics;
using Client.API.Services;

namespace Client.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration config, ILogger<HomeController> logger)
        {
            _config = config;
            _logger = logger;
        }
        
        public IActionResult Index()
        {

           
            string ordersServiceUrl = Environment.GetEnvironmentVariable("ORDER_API_MICROSERVICE_BASE_URL") ?? _config["Urls:ORDER_API_MICROSERVICE_BASE_URL"]!;
            string ordersClientServiceUrl = Environment.GetEnvironmentVariable("ORDER_CLIENT_MICROSERVICE_BASE_URL") ?? _config["Urls:ORDER_CLIENT_MICROSERVICE_BASE_URL"]!;
            ViewData["Customers"] = FakeCustomerService.GetCustomers();
            ViewData["OrdersServiceUrl"] = ordersServiceUrl;
            ViewData["OrdersClientServiceUrl"] = ordersClientServiceUrl;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AddNewOrder()
        {
            ViewData["Customers"] = FakeCustomerService.GetCustomers();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
