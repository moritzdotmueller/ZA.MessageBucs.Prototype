using System;

namespace ZA.MessageBus.Prototype.Contracts.Messages
{
    public class CreateSettlementSummaryCommand : MessageBase
    {
        public Guid ArchiveId { get; init; }
    }
}
