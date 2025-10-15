using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly IMapper _mapper;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public OrdersService(IOrdersRepository ordersRepo, IMapper mapper,IRabbitMQPublisher rabbitMQPublisher)
        {
            _ordersRepo = ordersRepo;
            _mapper = mapper;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        public async Task<OrderReponse?> OrderAddAsync(OrderAddRequest orderRequest)
        {
            Order orderFromRequest = _mapper.Map<Order>(orderRequest);

            var orderCreated = await _ordersRepo.OrderAddAsync(orderFromRequest);

            OrderReponse orderToResponse = _mapper.Map<OrderReponse>(orderCreated);
            return orderToResponse;

        }

        public async Task<OrderItemReponse?> OrderAddItemAsync(Guid orderId, OrderItemAddRequest orderItemRequest)
        {
            var oiRequest = _mapper.Map<OrderItem>(orderItemRequest);
            var oi = await _ordersRepo.OrderAddItemAsync(orderId, oiRequest);

            var oiResponse = _mapper.Map<OrderItemReponse>(oi);
            return oiResponse;
        }
        

        public async Task<bool> OrderDeleteItemAsync(Guid orderId, Guid itemId)
        {
            bool isDeleted = await _ordersRepo.OrderDeleteItemAsync(orderId, itemId);

            return isDeleted;
        }
        public async Task<bool> OrderChangeStatusAsync(Guid id, OrderStatus orderStatus)
        {
            bool isChanged = await _ordersRepo.OrderChangeStatusAsync(id, orderStatus);
            string routingKey = "order.status.changed";
            var message = new
            {
                OrderId = id,
                NewStatus = orderStatus.ToString()
            };
            _rabbitMQPublisher.Publish<object>(routingKey, message);
            return isChanged;
        }
        public async Task<OrderForResponseWithItems?> OrderGetByIdAsync(Guid id)
        {
            var order = await _ordersRepo.OrderGetByIdWithItemsAsync(id);

            return order;
        }


        public async Task<List<OrderForResponseWithItems?>> OrderGetByParamsAsync(OrderStatus? status, Guid? customerId, int page, int pageSize)
        {
            var orders = await _ordersRepo.OrderGetByParamsAsync(status, customerId, page, pageSize);

            return orders;
        }
    }
}
