using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Client.API.HttpClients;

namespace Client.API.Controllers.API
{
    [Route("api/Orders")]
    [ApiController]
    public class _OrdersController : ControllerBase
    {
        private readonly OrdersMicroserviceClient _ordersMicroserviceClient;

        public _OrdersController(OrdersMicroserviceClient ordersMicroserviceClient)
        {
            _ordersMicroserviceClient = ordersMicroserviceClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            int? status = null,
            Guid? customerId = null,
            int page = 1,
            int pageSize = 2)
        {
            var orders = await _ordersMicroserviceClient.GetOrders(status, customerId, page, pageSize);
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
        public class AddOrderRequest
        {
            public Guid CustomerId { get; set; }
            public string CustomerName { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequest)
        {
            var res = await _ordersMicroserviceClient.AddOrder(addOrderRequest);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddOrderItem(Guid id,object addOrderItemRequest)
        {
            var res = await _ordersMicroserviceClient.AddOrderItem(id,addOrderItemRequest);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }
        [HttpDelete("{orderId}/items/{orderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(string orderId, string orderItemId)
        {
            var res = await _ordersMicroserviceClient.DeleteOrderItem(orderId,orderItemId);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }

    }
}
