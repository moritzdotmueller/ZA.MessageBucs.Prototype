using System;
using System.Threading.Tasks;
using ZA.MessageBus.Prototype.Contracts;
using ZA.MessageBus.Prototype.Contracts.Messages;

namespace ZA.MessageBus.Prototype.ConsumerA
{
    public class CreateSettlementSummaryHandler : IHandler<CreateSettlementSummaryCommand>
    {
        private readonly IPublisher publisher;

        public CreateSettlementSummaryHandler(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public Task HandleAsync(CreateSettlementSummaryCommand command)
        {
            Console.WriteLine($"ConsumerA: {command.Message}");
            var result = new SettlementSummaryCreated
            {
                Id = command.Id,
                Timestamp = DateTime.Now,
                SomeStringProperty = "Hello World",
                SomeDecimalProperty = 17.98m
            };
            this.publisher.Publish(result);
            return Task.CompletedTask;
        }
    }
}
