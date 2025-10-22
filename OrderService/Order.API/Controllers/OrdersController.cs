﻿using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Order.API.DTOs;
using Order.API.Hubs;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrdersService ordersService, IHubContext<OrderHub> hubContext, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _hubContext = hubContext;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderAddRequest orderRequest)
        {
            var orderCreated = await _ordersService.OrderAddAsync(orderRequest);
            if (orderCreated == null)
            {
                return BadRequest("Order could not be created.");
            }
            
            await _hubContext.Clients.All.SendAsync(OrdersHubMethods.ReceiveOrderCreate, orderCreated);

            return Ok(orderCreated);
            //return CreatedAtAction(nameof(GetOrderById), new { id = orderCreated.Id }, orderCreated);
        }

        [HttpPost("testOnly")]
        public async Task<IActionResult> testOnly([FromBody] OrderToApproveMessageResponse otam)
        {
            //string d = obj["res"];
            //string d1 = obj["orderId"];

            //todo: fix here
            //Guid orderId = Guid.NewGuid();
            if (otam.Success) {
                var isOrderStatusChanged = await _ordersService.OrderChangeStatusAsync(otam.OrderId,
                orderStatus: DataAccessLayer.Entities.OrderStatus.Confirmed);
                await _hubContext.Clients.All.SendAsync(
                    OrdersHubMethods.ReceiveOrderStatusUpdate,
                    otam.OrderId.ToString(),
                    OrderStatus.Confirmed,
                    "success"
                    );
            } else {
                var isOrderStatusChanged = await _ordersService.OrderChangeStatusAsync(otam.OrderId,
                orderStatus: DataAccessLayer.Entities.OrderStatus.Draft);
                await _hubContext.Clients.All.SendAsync(
                    OrdersHubMethods.ReceiveOrderStatusUpdate,
                    otam.OrderId.ToString(), 
                    OrderStatus.Draft,
                    otam.ErrorMessage);
            }



            return Ok("Order success");
        }
        [HttpPost("{id}/items")]
        //add order item
        public async Task<IActionResult> AddOrderItem(Guid id,[FromBody] OrderItemAddRequest orderItemRequest)
        {
            var orderItemCreated = await _ordersService.OrderAddItemAsync(id, orderItemRequest);
            if (orderItemCreated == null)
            {
                return BadRequest("Order could not add an item.");
            }

            return Ok(orderItemCreated);
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id, Guid itemId)
        {
            var isItemDeleted = await _ordersService.OrderDeleteItemAsync(id, itemId);
            if (!isItemDeleted)
            {
                return BadRequest("Order could not delete current item.");
            }
            return Ok("Item deleted Successfully");
        }
        //[HttpPost("{id}/backToDraft")]
        //public async Task<IActionResult> backToDraft(Guid id)
        //{
        //    var isOrderStatusChanged = await _ordersService.OrderChangeStatusAsync(id,
        //        orderStatus: DataAccessLayer.Entities.OrderStatus.Draft);
        //    if (!isOrderStatusChanged)
        //    {
        //        return BadRequest("Order status could not be changed");
        //    }

        //    await _hubContext.Clients.All.SendAsync(
        //        OrdersHubMethods.ReceiveOrderStatusUpdate, 
        //        id, 
        //        OrderStatus.Draft);
        //    return Ok();
        //}
        
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> OrderConfirm(Guid id)
        {
            var isOrderStatusChanged = await _ordersService.OrderChangeStatusAsync(id, 
                orderStatus: DataAccessLayer.Entities.OrderStatus.Confirmed,true);
            if (!isOrderStatusChanged)
            {
                return BadRequest("Order status could not be changed");
            }
            await _hubContext.Clients.All.SendAsync(
                OrdersHubMethods.ReceiveOrderStatusUpdate, 
                id, 
                OrderStatus.PendingApproval, 
                OrdersHubMethods.MsgConfirmAndWaitingApproval);

            return Ok();
        }
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> OrderCancel(Guid id)
        {
            var isOrderStatusChanged = await _ordersService.OrderChangeStatusAsync(id, 
                orderStatus: DataAccessLayer.Entities.OrderStatus.Cancelled);
            if (!isOrderStatusChanged)
            {
                return BadRequest("Order status could not be changed");
            }

            await _hubContext.Clients.All.SendAsync(
                OrdersHubMethods.ReceiveOrderStatusUpdate, 
                id, 
                OrderStatus.Cancelled,
                OrdersHubMethods.MsgCanceled);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> OrderGetById(Guid id)
        {
            var orderResponse = await _ordersService.OrderGetByIdAsync(id);
            if (orderResponse == null)
            {
                return BadRequest("Order not found");
            }
            return Ok(orderResponse);
        }
        [HttpGet]
        public async Task<IActionResult> OrderGetList(
            int? status = null,
            Guid? customerId = null,
            int page = 1,
            int pageSize = 2)
        {
            OrderStatus? orderStatus = status.HasValue ? (OrderStatus?)status.Value : null;
            var orderListResponse = await _ordersService.OrderGetByParamsAsync(orderStatus, customerId, page, pageSize);
            //if (orderResponse == null)
            //{
            //    return BadRequest("Order not found");
            //}
           

            return Ok(orderListResponse);
        }
    }
}
//POST / api / orders → create draft order
// POST /api/orders/{id}/ items → add item
// DELETE /api/orders/{id}/ items /{ itemId}
// POST / api / orders /{ id}/ confirm
// POST / api / orders /{ id}/ cancel
// GET / api / orders /{ id}
// GET / api / orders ? status = &customerId = &page = &pageSize =
// GET / health / ready, GET / health / live
