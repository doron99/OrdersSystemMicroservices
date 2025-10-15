using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher,IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IModel _channel;
        private readonly IConnection? _connection;
        public RabbitMQPublisher(IConfiguration config)
        {
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

        public void Publish<T>(string routingKey, T message)
        {
            
            string msgJson = JsonConvert.SerializeObject(message);
            byte[] msgBodyInBytes = Encoding.UTF8.GetBytes(msgJson);

            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

            _channel.BasicPublish(exchange: exchangeName,
                                  routingKey: routingKey,
                                  basicProperties: null,
                                  body: msgBodyInBytes);
        }
    }
}
