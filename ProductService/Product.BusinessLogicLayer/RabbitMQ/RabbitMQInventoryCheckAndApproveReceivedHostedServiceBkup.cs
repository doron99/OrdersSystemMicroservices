using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoriesContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQInventoryCheckAndApproveReceivedHostedServiceBkup : IHostedService, IDisposable
    {
        private readonly ILogger<IHostedService> _logger;
        private readonly IConfiguration _config;
        private readonly IProductsService _productsService;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;


        public RabbitMQInventoryCheckAndApproveReceivedHostedServiceBkup(
            ILogger<IHostedService> logger, 
            IConfiguration config,
            IProductsService productsService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            //_productsService = productsService;
            ConnectionFactory Factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ_HostName"]!,
                UserName = _config["RabbitMQ_UserName"]!,
                Password = _config["RabbitMQ_Password"]!,
                Port = int.Parse(_config["RabbitMQ_Port"]!)
            };
            _connection = Factory.CreateConnection();
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var migrator = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
                //await migrator.WaitForMigrationAsync(stoppingToken);
            }
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
                if (message.StartsWith("\"") && message.EndsWith("\""))
                {
                    message = message.Trim('"'); // Remove the outer quotes
                }
                if (!string.IsNullOrEmpty(message))
                {
                    OrderToApprove orderToApprove = JsonConvert.DeserializeObject<OrderToApprove>(message);
                    // Process the received message
                    _logger.LogInformation("Received products: " + message);

                    if (orderToApprove.Products == null || !orderToApprove.Products.Any())
                    {
                        Console.WriteLine("No products to validate.");
                        return;
                    }
                    List<string> skus = orderToApprove.Products
                        .Select(p => p.Sku)
                        .ToList();
                    List<Product> products = await _productsService.GetProductsByListOfSkus(skus);
                    var x = 1;
                    //convert the list to List<product> i mean do the mapping
                    //List<Product> productsToApprove = orderToApprove.Products
                    //    .Select(p => new Product
                    //    {
                    //        ProductId = p.ProductId,
                    //        Name = p.Name,
                    //        Quantity = p.Quantity
                    //    })
                    //    .ToList();



                    // Here you can add logic to handle the received products, e.g., update database, etc.
                }

            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return Task.CompletedTask;
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
