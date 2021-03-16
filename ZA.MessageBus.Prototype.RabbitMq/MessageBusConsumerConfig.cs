using System;

namespace ZA.MessageBus.Prototype.RabbitMq
{
    public class MessageBusConsumerConfig
    {
        public RabbitMqConfig RabbitMqConfig { get; set; }
        public string ConsumerName { get; init; }
        public string[] Topics { get; init; } = Array.Empty<string>();
    }

}
