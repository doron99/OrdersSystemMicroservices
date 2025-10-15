using Azure;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.RepositoryContracts;
namespace DataAccessLayer.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<IOrdersRepository> _logger;

        public OrdersRepository(AppDbContext ctx, ILogger<IOrdersRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<Entities.Order?> OrderAddAsync(Entities.Order order)
        {
            _ctx.Add(order);
            await _ctx.SaveChangesAsync();
            return order;
        }

        public async Task<OrderItem> OrderAddItemAsync(Guid orderId, OrderItem orderItem)
        {
            Entities.Order? orderWithItems = await OrderGetByIdAsync(orderId);
            if (orderWithItems == null)
            {
                _logger.LogInformation("Order not found");
                throw new Exception("Order not found");
            }

            orderItem.OrderId = orderWithItems.Id;
            _ctx.Add(orderItem);
            
            await _ctx.SaveChangesAsync();
            return orderItem;

        }

      

        public async Task<OrderForResponseWithItems?> OrderGetByIdWithItemsAsync(Guid id)
        {

            var order = await _ctx.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems) // Eager load the OrderItems
            .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                _logger.LogInformation("Order not found");
                throw new Exception("Order not found");
            }

            // Map the Order entity to OrderResponse
            var orderResponse = new OrderForResponseWithItems
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Sku = oi.Sku,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    OrderItemDesc = oi.OrderItemDesc,
                    ProductId = oi.ProductId
                }).ToList()
            };

            return orderResponse;
        }
        public async Task<Entities.Order?> OrderGetByIdAsync(Guid id)
        {
            var order = await _ctx.Orders
                .FirstOrDefaultAsync(o => o.Id == id); // Find the order by Id

            if (order == null)
            {
                _logger.LogInformation("Order not found");
                throw new Exception("Order not found");
            }

            return order;
        }

        public async Task<bool> OrderDeleteItemAsync(Guid orderId, Guid itemId)
        {
            var orderWithItems = await OrderGetByIdAsync(orderId);
            if (orderWithItems == null)
            {
                _logger.LogInformation("Order not found");
                throw new Exception("Order not found");
            }

            var orderItem = orderWithItems.OrderItems.FirstOrDefault(oi => oi.OrderItemId == itemId);
            if (orderItem == null)
            {
                _logger.LogInformation("Order item not found");
                throw new Exception("Order item not found");
            }

            // Remove the OrderItem from the collection
            orderWithItems.OrderItems.Remove(orderItem);

            // Save changes to the database
            await _ctx.SaveChangesAsync();

            return true;
        }
        
        public async Task<List<OrderForResponseWithItems?>> OrderGetByParamsAsync(OrderStatus? status, Guid? customerId, int page, int pageSize)
        {
            var query = _ctx.Orders.AsNoTracking()
            /// Apply filters conditionally using ConditionalWhere
            .ConditionalWhere(() => status.HasValue, o => o.Status == status!.Value)
            .ConditionalWhere(() => customerId != null, o => o.CustomerId == customerId)
                .Include(o => o.OrderItems); // Eager load the OrderItems


            // Apply pagination
            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize) 
                .ToListAsync();

            // Get the total count of orders for pagination
            //var totalCount = await query.CountAsync();

            var orderResponses = orders.Select(order => new OrderForResponseWithItems
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    Sku = oi.Sku,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    OrderItemDesc = oi.OrderItemDesc,
                    ProductId = oi.ProductId
                }).ToList()
            }).ToList();

            return orderResponses!;

        }

        public async Task<bool> OrderChangeStatusAsync(Guid id, OrderStatus orderStatus)
        {
            var existingOrder = await _ctx.Orders.FindAsync(id);
            if (existingOrder == null)
            {
                _logger.LogInformation("Order not found");
                throw new Exception("Order not found");
            }

            existingOrder.Status = orderStatus;
            existingOrder.UpdatedAt = DateTime.UtcNow;

            _ctx.Orders.Attach(existingOrder);
            _ctx.Entry(existingOrder).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                var entry = await _ctx.Orders.AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (entry != null)
                {
                    throw new Exception("The order was updated by another user. Please reload and try again. OrderId: "+ id);
                }
                return false;
            }
        }

       
    }
}
