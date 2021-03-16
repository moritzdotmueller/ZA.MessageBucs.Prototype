using RabbitMQ.Client;

namespace ZA.MessageBus.Prototype.RabbitMq
{
    public class ConnectionContext
    {
        public IConnection Connection { get; init; }
        public IModel Model { get; init; }
        public string ConsumerTag { get; init; }
    }
}