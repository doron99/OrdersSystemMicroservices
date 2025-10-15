using Microsoft.AspNetCore.Mvc;
using Client.API.Models;
using System.Diagnostics;
using Client.API.Services;
using Client.API.HttpClients;

namespace Client.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HomeController> _logger;
        private readonly ProductsMicroserviceClient _productsMicroserviceClient;
        private readonly OrdersMicroserviceClient _ordersMicroserviceClient;

        public HomeController(IConfiguration config, ILogger<HomeController> logger, 
            ProductsMicroserviceClient productsMicroserviceClient, OrdersMicroserviceClient ordersMicroserviceClient)
        {
            _config = config;
            _logger = logger;
            _productsMicroserviceClient = productsMicroserviceClient;
            _ordersMicroserviceClient = ordersMicroserviceClient;
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
        public async Task<IActionResult> AddNewOrder()
        {
            ViewData["Customers"] = FakeCustomerService.GetCustomers();
            ViewData["Products"] = await _productsMicroserviceClient.GetProducts();

            return View();
        }
        [HttpGet("home/editorder/{id}")]
        public async Task<IActionResult> EditOrder(string id)
        {
            ViewData["Customers"] = FakeCustomerService.GetCustomers();
            ViewData["Products"] = await _productsMicroserviceClient.GetProducts();
            ViewData["Order"] = await _ordersMicroserviceClient.GetById(Guid.Parse(id));

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
