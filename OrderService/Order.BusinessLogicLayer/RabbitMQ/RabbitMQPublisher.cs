using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConfiguration _config;

        public RabbitMQPublisher(IConfiguration config)
        {
            _config = config;
        }

      

       
        public void Publish<T>(string routingKey, T message)
        {
            ConnectionFactory Factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ_HostName"]!,
                UserName = _config["RabbitMQ_UserName"]!,
                Password = _config["RabbitMQ_Password"]!,
                Port = int.Parse(_config["RabbitMQ_Port"]!)
            };

            var connection = Factory.CreateConnection();
            using var channel = connection.CreateModel();

            string msgJson = JsonConvert.SerializeObject(message);
            byte[] msgBodyInBytes = Encoding.UTF8.GetBytes(msgJson);

            string exchangeName = _config["RabbitMQ_Products_Exchange"]!;
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);

            channel.BasicPublish(exchange: exchangeName,
                                  routingKey: routingKey,
                                  basicProperties: null,
                                  body: msgBodyInBytes);
        }
    }
}
