using System.Threading.Tasks;

namespace ZA.MessageBus.Prototype.Contracts
{
    public interface IHandler<TMessage> where TMessage : IMessage
    {
        Task HandleAsync(TMessage command);
    }
}
