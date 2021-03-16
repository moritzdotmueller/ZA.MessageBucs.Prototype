using System;

namespace ZA.MessageBus.Prototype.Contracts
{
    public interface IHandlerFactory
    {
        object Get(Type messageType);
    }
}
