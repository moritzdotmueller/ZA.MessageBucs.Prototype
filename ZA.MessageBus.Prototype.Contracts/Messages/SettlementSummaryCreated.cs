namespace ZA.MessageBus.Prototype.Contracts.Messages
{
    public class SettlementSummaryCreated : MessageBase
    {
        public string SomeStringProperty { get; init; }
        public decimal SomeDecimalProperty { get; init; }
    }
}
