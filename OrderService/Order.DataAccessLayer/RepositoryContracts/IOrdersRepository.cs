using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryContracts
{
    public interface IOrdersRepository
    {
        //POST / api / orders → create draft order
        Task<Entities.Order?> OrderAddAsync(Entities.Order order);
        // GET / api / orders /{ id}
        Task<Entities.Order?> OrderGetByIdAsync(Guid id);
        Task<Entities.OrderForResponseWithItems?> OrderGetByIdWithItemsAsync(Guid id);
        //Task<List<Order?>> GetAllOrdersAsync();
        //Task<List<Order?>> GetAllOrdersFilteredAsync();
        // POST / api / orders /{ id}/ confirm
        // POST / api / orders /{ id}/ cancel
        Task<bool> OrderChangeStatusAsync(Guid id, OrderStatus orderStatus);
        // POST /api/orders/{id}/ items → add item
        Task<OrderItem> OrderAddItemAsync(Guid orderId ,OrderItem orderItem);
        // DELETE /api/orders/{id}/ items /{ itemId}
        Task<bool> OrderDeleteItemAsync(Guid orderId, Guid itemId);
        // GET / api / orders ? status = &customerId = &page = &pageSize =
        Task<List<OrderForResponseWithItems?>> OrderGetByParamsAsync(OrderStatus? status, Guid? customerId, int page, int pageSize);






    }
}
