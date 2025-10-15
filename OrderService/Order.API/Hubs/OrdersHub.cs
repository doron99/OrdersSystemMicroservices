using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace Order.API.Hubs
{
    public class OrderHub : Hub
    {
        //public async Task SendOrderStatusUpdate(string orderId, string status)
        //{
        //    await Clients.All.SendAsync("ReceiveOrderStatusUpdate", orderId, status);
        //}
        //public async Task SendOrderCreate(OrderReponse orderResponse)
        //{
        //    await Clients.All.SendAsync("ReceiveOrderCreate", orderResponse);
        //}
    }
    
}
