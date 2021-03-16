using System;
using System.Threading.Tasks;
using ZA.MessageBus.Prototype.Contracts;
using ZA.MessageBus.Prototype.Contracts.Messages;

namespace ZA.MessageBus.Prototype.ConsumerB
{
    public class MultiHandler : IHandler<CreateSettlementSummaryCommand>, IHandler<SettlementSummaryCreated>
    {
        public Task HandleAsync(CreateSettlementSummaryCommand command)
        {
            Console.WriteLine($"ConsumerB: {command.Message}");
            return Task.CompletedTask;
        }

        public Task HandleAsync(SettlementSummaryCreated command)
        {
            Console.WriteLine($"ConsumerB: {command.SomeStringProperty}, {command.SomeDecimalProperty}");
            return Task.CompletedTask;
        }
    }
}
