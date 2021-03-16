using RabbitMQ.Client;

namespace ZA.MessageBus.Prototype.RabbitMq
{
    public interface IMessageBusConsumerCreator
    {
        void Setup(MessageBusConsumerConfig config);
    }

    public class MessageBusConsumerCreator : IMessageBusConsumerCreator
    {
        public void Setup(MessageBusConsumerConfig config)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = config.RabbitMqConfig.Host,
                Port = config.RabbitMqConfig.Port,
                UserName = config.RabbitMqConfig.User,
                Password = config.RabbitMqConfig.Password
            };
            using var connection = connectionFactory.CreateConnection();
            using var model = connection.CreateModel();

            model.ExchangeDeclare($"ZA.{config.ConsumerName}.exchange", "topic", true, false);
            model.QueueDeclare($"ZA.{config.ConsumerName}.queue", true, false, false);
            model.QueueBind($"ZA.{config.ConsumerName}.queue", $"ZA.{config.ConsumerName}.exchange", "#");

            //ToDo: Update bindings
            foreach (var topic in config.Topics)
            {
                model.ExchangeBind($"ZA.{config.ConsumerName}.exchange", "ZA.MessageBus", topic);
            }
        }
    }

}
