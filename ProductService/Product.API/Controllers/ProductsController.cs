using BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
       
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductsService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductsService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("GetStockTrackingList")]
        public async Task<IActionResult> GetStockTrackingList()
        {
            var products = await _productService.GetStockTrackingList();
            return Ok(products);
        }


    }
}
