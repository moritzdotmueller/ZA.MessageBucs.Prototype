using System;
using System.Threading.Tasks;
using ZA.MessageBus.Prototype.Contracts;
using ZA.MessageBus.Prototype.Contracts.Messages;

namespace ZA.MessageBus.Prototype.ConsumerA
{
    public class CreateSettlementSummaryHandler : IHandler<CreateSettlementSummaryCommand>
    {
        public Task HandleAsync(CreateSettlementSummaryCommand command)
        {
            Console.WriteLine($"ConsumerA: {command.Message}");
            return Task.CompletedTask;
        }
    }
}
