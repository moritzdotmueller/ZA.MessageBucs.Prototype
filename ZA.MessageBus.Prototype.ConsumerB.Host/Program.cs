using System;
using System.Threading.Tasks;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using ZA.MessageBus.Prototype.Contracts;
using ZA.MessageBus.Prototype.Contracts.Messages;
using ZA.MessageBus.Prototype.RabbitMq;

namespace ZA.MessageBus.Prototype.ConsumerB.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = await Container.BuildAsync(s =>
            {
                s.Scan(a =>
                {
                    a.AssembliesFromApplicationBaseDirectory(assembly => assembly.GetName().Name.StartsWith("ZA"));
                    a.RegisterConcreteTypesAgainstTheFirstInterface();
                    
                });

                s.For<IHandler<CreateSettlementSummaryCommand>>().Use<MultiHandler>();
                s.For<IHandler<SettlementSummaryCreated>>().Use<MultiHandler>();

                s.ForSingletonOf<RabbitMqConfig>().Use(new RabbitMqConfig
                {
                    Host = "localhost",
                    Port = 5672,
                    User = "guest",
                    Password = "guest"
                });
                s.ForSingletonOf<MessageBusConsumerConfig>().Use(c => new MessageBusConsumerConfig
                {
                    ConsumerName = "ConsumerB",
                    Topics = new[]
                    {
                        typeof(CreateSettlementSummaryCommand).FullName,
                        typeof(SettlementSummaryCreated).FullName
                    },
                    RabbitMqConfig = c.GetInstance<RabbitMqConfig>()
                });
            });

            var consumerConfig = container.GetService<MessageBusConsumerConfig>();
            var consumerCreator = container.GetService<IMessageBusConsumerCreator>();
            consumerCreator.Setup(consumerConfig);

            var consumer = container.GetService<IConsumer>();
            consumer.Start();
            Console.ReadLine();
        }
    }

    public class LamarHandlerFactory : IHandlerFactory
    {
        private readonly IContainer container;

        public LamarHandlerFactory(IContainer container)
        {
            this.container = container;
        }

        //Can one service implement a handler interface multiple times?
        public object Get(Type messageType)
        {
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            return container.GetRequiredService(handlerType);
        }
    }
}
