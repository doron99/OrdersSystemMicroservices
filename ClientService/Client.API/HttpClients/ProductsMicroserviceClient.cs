using Microsoft.Extensions.Caching.Distributed;
using Client.API.Models;
using System.Text.Json;
using Client.API.Dtos;

namespace Client.API.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsMicroserviceClient> _logger;
        public ProductsMicroserviceClient(HttpClient httpClient,
            ILogger<ProductsMicroserviceClient> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<List<ProductDto?>> GetProducts()
        {
            List<ProductDto?> products = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/products");

                if (response.IsSuccessStatusCode) {
                    var result = await response.Content.ReadFromJsonAsync<List<ProductDto?>>();
                    products = result ?? new List<ProductDto?>();

                    return products;
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
        public async Task<List<object?>> GetStockTrackingList()
        {
            List<object?> stockTrackingList = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/products/GetStockTrackingList");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<object?>>();
                    stockTrackingList = result ?? new List<object?>();

                    return stockTrackingList;
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
