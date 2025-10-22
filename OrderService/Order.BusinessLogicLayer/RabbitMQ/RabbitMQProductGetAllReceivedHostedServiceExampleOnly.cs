using Microsoft.Extensions.Configuration;
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
    
    public class RabbitMQProductGetAllReceivedHostedServiceExampleOnly : IHostedService, IDisposable
    {
        private readonly ILogger<IHostedService> _logger;
        private readonly IConfiguration _config;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        public RabbitMQProductGetAllReceivedHostedServiceExampleOnly(ILogger<IHostedService> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
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

            string routingKey = "product.all.retrieved";
            string queueName = "orders.product.all.retrieved.queue";

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

            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                if (!string.IsNullOrEmpty(message))
                {
                    var products = JsonConvert.DeserializeObject<object>(message);
                    // Process the received message
                    Console.WriteLine(" [x] Received message with products: {0}", message);
                    _logger.LogInformation("Received products: " + message);
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
