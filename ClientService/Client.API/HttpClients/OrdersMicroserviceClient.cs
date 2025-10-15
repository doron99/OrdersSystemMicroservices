using Microsoft.Extensions.Caching.Distributed;
using Client.API.Models;
using System.Text.Json;
using System.Text;

namespace Client.API.HttpClients
{
    public class OrdersMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrdersMicroserviceClient> _logger;
        public OrdersMicroserviceClient(HttpClient httpClient,
            ILogger<OrdersMicroserviceClient> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<object?> AddOrder(Guid customerId)
        {
            //List<OrderForResponseWithItems?> orderForResponseWithItems = null;
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { customerId = customerId });// orderRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync($"/api/orders", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<object?>();// List<OrderForResponseWithItems?>>();
                    //orderForResponseWithItems = result ?? new List<OrderForResponseWithItems?>();
                    var res = result ?? new { Error = "Error occoured" };
                    return res;
                }
                else
                {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }

        }
        public async Task<object?> AddOrderItem(Guid orderId, object orderItem)
        {
            //List<OrderForResponseWithItems?> orderForResponseWithItems = null;
            try
            {
                var jsonContent = JsonSerializer.Serialize(orderItem);// orderRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync($"/api/orders/{orderId}/items", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<object?>();// List<OrderForResponseWithItems?>>();
                    //orderForResponseWithItems = result ?? new List<OrderForResponseWithItems?>();
                    var res = result ?? new { Error = "Error occoured" };
                    return res;
                }
                else
                {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }

        }
        public async Task<List<OrderForResponseWithItems?>> GetOrders()
        {
            List<OrderForResponseWithItems?> orderForResponseWithItems = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/orders");

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadFromJsonAsync<List<OrderForResponseWithItems?>>();
                    orderForResponseWithItems = result ?? new List<OrderForResponseWithItems?>();

                    return orderForResponseWithItems;
                } else {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }
       

        }
        public async Task<object?> GetById(Guid id)
        {
            List<object?> orderForResponseWithItems = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/orders/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<object?>();
                    //orderForResponseWithItems = result;// new List<OrderForResponseWithItems?>();

                    return result;
                }
                else
                {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }


        }
        
        public async Task<object> OrderConfirm(string id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"/api/orders/{id}/confirm",null);

                if (response.IsSuccessStatusCode)
                {
                    //it returns empty result
                    return new { isConfirm = true};
                }
                else
                {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }

        }
        public async Task<object> OrderCancel(string id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"/api/orders/{id}/cancel", null);

                if (response.IsSuccessStatusCode)
                {
                    //it returns empty result
                    return new { isCancel = true };
                }
                else
                {
                    throw new HttpRequestException("Http response status occour:" + response.StatusCode.ToString(), null, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders.");
                throw;
            }

        }
    }
}
