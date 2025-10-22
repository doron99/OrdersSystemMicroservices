using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IOrdersService
    {
        Task<OrderReponse?> OrderAddAsync(OrderAddRequest orderRequest);
        Task<OrderItemReponse?> OrderAddItemAsync(Guid orderId, OrderItemAddRequest orderItemRequest);
        Task<bool> OrderDeleteItemAsync(Guid orderId, Guid itemId);

        Task<bool> OrderChangeStatusAsync(Guid id, OrderStatus orderStatus, bool sendToInventoryApprove = false);
        Task<OrderForResponseWithItems?> OrderGetByIdAsync(Guid id);
        Task<List<OrderForResponseWithItems?>> OrderGetByParamsAsync(OrderStatus? status, Guid? customerId, int page, int pageSize);
    }
}
