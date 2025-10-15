using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Client.API.HttpClients;

namespace Client.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersMicroserviceClient _ordersMicroserviceClient;

        public OrdersController(OrdersMicroserviceClient ordersMicroserviceClient)
        {
            _ordersMicroserviceClient = ordersMicroserviceClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersMicroserviceClient.GetOrders();
            if (orders == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(orders);
        }
        [HttpGet("OrderConfirm/{id}")]
        public async Task<IActionResult> OrderConfirm(string id)
        {
            var res = await _ordersMicroserviceClient.OrderConfirm(id);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }
        [HttpGet("OrderCancel/{id}")]
        public async Task<IActionResult> OrderCancel(string id)
        {
            var res = await _ordersMicroserviceClient.OrderCancel(id);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }

    }
}
