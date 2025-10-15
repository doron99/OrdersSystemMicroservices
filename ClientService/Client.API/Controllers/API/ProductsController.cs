using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Client.API.HttpClients;

namespace Client.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsMicroserviceClient _productsMicroserviceClient;

        public ProductsController(ProductsMicroserviceClient productsMicroserviceClient)
        {
            _productsMicroserviceClient = productsMicroserviceClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var orders = await _productsMicroserviceClient.GetProducts();
            if (orders == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(orders);
        }
       

    }
}
