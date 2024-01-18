using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RESTAURANT.ASYNC.Broker
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IConfiguration _configuration;

        public MessageBroker(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void publishMessage<T>(T message)
        {
            var factory = new ConnectionFactory() {
                HostName = _configuration.GetSection("MessageBroker:HostName").Value,
                Port = int.Parse(_configuration.GetSection("MessageBroker:Port").Value),
                UserName = _configuration.GetSection("MessageBroker:Username").Value,
                Password = _configuration.GetSection("MessageBroker:Password").Value
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "orders",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "",
                                    routingKey: "orders",
                                    basicProperties: null,
                                    body: body);
            }
        }
    }
}
