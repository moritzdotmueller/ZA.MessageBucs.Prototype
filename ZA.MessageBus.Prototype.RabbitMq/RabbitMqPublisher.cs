using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ZA.MessageBus.Prototype.Contracts;

namespace ZA.MessageBus.Prototype.RabbitMq
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly RabbitMqConfig config;

        public RabbitMqPublisher(RabbitMqConfig config)
        {
            this.config = config;
        }

        public void Publish(params IMessage[] messages)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = config.Host,
                Port = config.Port,
                UserName = config.User,
                Password = config.Password
            };
            using var connection = connectionFactory.CreateConnection();
            using var model = connection.CreateModel();

            model.TxSelect();
            foreach (var message in messages)
            {
                var props = model.CreateBasicProperties();
                props.Persistent = true;
                props.CorrelationId = message.Id;
                model.BasicPublish(
                    "ZA.MessageBus", 
                    message.GetType().FullName, 
                    true,
                    props, 
                    Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, message.GetType())));
            }
            model.TxCommit();
        }
    }
}
