using System;

namespace ZA.MessageBus.Prototype.Contracts
{
    public interface IMessage
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
    }

    public abstract class MessageBase : IMessage
    {
        public Guid Id { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
