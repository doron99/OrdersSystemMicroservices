using BusinessLogicLayer.DomainEvents;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure
{
    public class ApiEventPublisher : IDomainEventPublisher
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ApiEventPublisher(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
        {

                string ordersUrl = $"http://{_config["OrdersMicroserviceName"]}:{_config["OrdersMicroservicePort"]}";
                var response = await _httpClient.PostAsJsonAsync($"{ordersUrl}/api/orders/ChangeOrderStatusAndNotifyUser", @event);
                response.EnsureSuccessStatusCode();
        }
    }
}
