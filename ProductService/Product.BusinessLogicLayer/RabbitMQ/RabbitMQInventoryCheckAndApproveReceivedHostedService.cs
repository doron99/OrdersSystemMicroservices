using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQInventoryCheckAndApproveReceivedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<IHostedService> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMQInventoryCheckAndApproveReceivedHostedService(
            ILogger<IHostedService> logger, 
            IConfiguration config,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
            ConnectionFactory Factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ_HostName"]!,
                UserName = _config["RabbitMQ_UserName"]!,
                Password = _config["RabbitMQ_Password"]!,
                Port = int.Parse(_config["RabbitMQ_Port"]!)
            };
            _connection = Factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;

            string routingKey = "inventory.check.approve.retrieved";
            string queueName = "product.inventory.check.approve.queue";

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routingKey);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                
                if (!string.IsNullOrEmpty(message))
                {
                    OrderToApproveMessageResponse otam = new OrderToApproveMessageResponse();

                    OrderToApprove orderToApprove = JsonConvert.DeserializeObject<OrderToApprove>(message);
                    // Process the received message
                    _logger.LogInformation("Received products: " + message);

                    if (orderToApprove.Products == null || !orderToApprove.Products.Any())
                    {
                        Console.WriteLine("No products to validate.");
                        otam.Success = false;
                        otam.ErrorMessage = "No products to validate.";
                        otam.OrderId = orderToApprove.OrderId;
                        await Task.Delay(5000);
                        _ = SendDataToHttpMicroserviceAsync(otam, _config);// Use `_ =` to ignore the task and avoid waiting
                        return;
                    }

                    List<string> Skus = orderToApprove.Products
                        .Select(p => p.Sku)
                        .ToList();
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var productsRepo = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
                        List<Product> products = await productsRepo.GetProductsByListOfSkus(Skus);

                        string emptyResOrErrorMessage = ValidateAndSubtractQuantities(products, orderToApprove.Products);
                        otam.OrderId = orderToApprove.OrderId;
                        otam.ErrorMessage = emptyResOrErrorMessage;
                        if (emptyResOrErrorMessage == "") //success
                        {
                            await WithdrawStockByGivenProductsAsync(productsRepo, products, orderToApprove.Products, orderToApprove.OrderId);

                                
                            otam.Success = true;
                            Console.WriteLine("Inventory check passed and quantities subtracted successfully.");
                        }
                        else
                        {
                            otam.Success = false;
                            Console.WriteLine("Inventory check failed: " + emptyResOrErrorMessage);
                        }
                        //sleep for 5 seconds to simulate processing time
                        await Task.Delay(5000);
                        _ = SendDataToHttpMicroserviceAsync(otam,_config);// Use `_ =` to ignore the task and avoid waiting

                    }
                }

            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task WithdrawStockByGivenProductsAsync(IProductsRepository productsRepo, List<Product> products, List<ProductToApprove> productsToApprove,Guid orderId)
        {
            foreach (var prod in productsToApprove)
            {
                var existingProduct = products.FirstOrDefault(p => p.Sku == prod.Sku);
                await productsRepo.WithdrawStockAsync(
                        prod.Sku,
                        existingProduct.Stock,
                        prod.Quantity,
                        "Withdraw",
                        orderId);
            }
        }

        private async Task SendDataToHttpMicroserviceAsync(OrderToApproveMessageResponse data, IConfiguration _config)
        {
            using var client = new HttpClient();
            string ordersMicroserviceUrl = $"http://{_config["OrdersMicroserviceName"]}:{_config["OrdersMicroservicePort"]}";

            client.BaseAddress = new Uri(ordersMicroserviceUrl + "/api/orders/testOnly"); // Adjust the URL

            // Serialize your data to JSON
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send the request asynchronously
            var response = await client.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Data sent successfully.");
            }
            else
            {
                Console.WriteLine($"Error sending data: {response.StatusCode}");
            }
        }
        /// <summary>
        /// empty string is success
        /// </summary>
        /// <param name="products"></param>
        /// <param name="productsWithQuantityToMinus"></param>
        /// <returns></returns>
        public string ValidateAndSubtractQuantities(List<Product> products, List<ProductToApprove> productsWithQuantityToMinus)
        {
            foreach (var productToMinus in productsWithQuantityToMinus)
            {
                // Find the matching product in the products list
                var existingProduct = products.FirstOrDefault(p => p.Sku == productToMinus.Sku);

                if (existingProduct != null)
                {
                    // Check if there is enough quantity to subtract
                    if (existingProduct.Stock - productToMinus.Quantity < 0)
                    {
                        return $"{existingProduct.Sku} does not have enough stock. Available: {existingProduct.Stock}, Requested: {productToMinus.Quantity}.";
                    }
                }
                else
                {
                    return $"Product with Sku {productToMinus.Sku} not found.";
                }
            }
            return "";
        }
        //public void ValidateAndSubtractQuantities(List<Product> products, List<ProductToApprove> productsWithQuantityToMinus)
        //{
        //    foreach (var productToMinus in productsWithQuantityToMinus)
        //    {
        //        // Find the matching product in the products list
        //        var existingProduct = products.FirstOrDefault(p => p.Sku == productToMinus.Sku);

        //        if (existingProduct != null)
        //        {
        //            // Check if there is enough quantity to subtract
        //            if (existingProduct.Stock >= productToMinus.Quantity)
        //            {
        //                // Subtract the quantity
        //                existingProduct.Quantity -= productToMinus.Quantity;
        //                Console.WriteLine($"Subtracted {productToMinus.Quantity} from {existingProduct.ProductDesc}. New quantity: {existingProduct.Quantity}.");
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Not enough stock for {existingProduct.ProductDesc}. Available: {existingProduct.Quantity}, Requested: {productToMinus.Quantity}.");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Product with ID {productToMinus.ProductId} not found.");
        //        }
        //    }
        //}

        private async Task CunsumeEvents(IProductsService productsService)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Dispose();
            }
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
