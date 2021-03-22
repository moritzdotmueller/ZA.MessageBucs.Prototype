using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ZA.MessageBus.Prototype.Contracts;

namespace ZA.MessageBus.Prototype.RabbitMq
{
    public interface IConsumer
    {
        void Start();
    }

    public class Consumer : IConsumer
    {
        private readonly object lockObject = new();
        private ConnectionContext connectionContext;
        private readonly MessageBusConsumerConfig config;
        private readonly IHandlerFactory handlerFactory;

        public Consumer(
            MessageBusConsumerConfig config,
            IHandlerFactory handlerFactory)
        {
            this.config = config;
            this.handlerFactory = handlerFactory;
        }

        private ConnectionContext createContext()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = config.RabbitMqConfig.Host,
                Port = config.RabbitMqConfig.Port,
                UserName = config.RabbitMqConfig.User,
                Password = config.RabbitMqConfig.Password,
                AutomaticRecoveryEnabled = true,
                DispatchConsumersAsync = true
            };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            var consumer = new AsyncEventingBasicConsumer(model);
            consumer.Received += onMessageReceived;
            var consumerTag = model.BasicConsume(consumer, $"ZA.{config.ConsumerName}.queue", false);
            return new ConnectionContext
            {
                Connection = connection,
                Model = model,
                ConsumerTag = consumerTag
            };
        }

        private async Task onMessageReceived(object sender, BasicDeliverEventArgs @event)
        {
            var consumer = (AsyncEventingBasicConsumer)sender;
            try
            {
                await this.invokeHandler(@event);
                consumer.Model.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception e)
            {
                consumer.Model.BasicReject(@event.DeliveryTag, false);
                //Logging
                //Reject -> Dead-Letter-Exchange
            }
        }

        private async Task invokeHandler(BasicDeliverEventArgs @event)
        {
            var messageType = typeof(IMessage).Assembly.GetType(@event.RoutingKey);
            var messageAsJObject = JObject.Parse(Encoding.UTF8.GetString(@event.Body.ToArray()));
            messageAsJObject["Id"] ??= @event.BasicProperties.CorrelationId ?? Guid.NewGuid().ToString();
            var message = messageAsJObject.ToObject(messageType);
            var handler = this.handlerFactory.Get(messageType);
            var methods = handler.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var matchingMethod = methods.SingleOrDefault(
                m => m.Name == nameof(IHandler<IMessage>.HandleAsync)
                     && m.GetParameters().SingleOrDefault(p => p.ParameterType == messageType) != null);
            if (matchingMethod != null)
            {
                var task = (Task) matchingMethod.Invoke(handler, new object[] {message});
                await task;
            }
        }

        public void Start()
        {
            if (connectionContext != null)
            {
                return;
            }

            lock (lockObject)
            {
                connectionContext ??= createContext();
            }
        }
    }
}
