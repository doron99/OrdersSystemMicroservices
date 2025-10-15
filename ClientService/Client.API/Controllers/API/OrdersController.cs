﻿using Microsoft.AspNetCore.Http;
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
        public class AddOrderRequest
        {
            public Guid CustomerId { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequest)
        {
            var res = await _ordersMicroserviceClient.AddOrder(addOrderRequest.CustomerId);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }
        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddOrder(Guid id,object addOrderItemRequest)
        {
            var res = await _ordersMicroserviceClient.AddOrderItem(id,addOrderItemRequest);
            if (res == null)
            {
                return BadRequest("error occoured.");
            }
            return Ok(res);
        }

    }
}
