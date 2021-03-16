namespace ZA.MessageBus.Prototype.Contracts
{
    public interface IPublisher
    {
        void Publish(params IMessage[] messages);
    }
}
