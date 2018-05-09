using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class InMemorySingleSubscriberBus<TMessage, TExecutionResult> : ISingleSubscriberBus<TMessage, TExecutionResult> 
        where TMessage : Message 
        where TExecutionResult : ExecutionResult
    {
        private readonly Dictionary<Type, IMessageHandler<TMessage, TExecutionResult>> subscribers;

        public InMemorySingleSubscriberBus()
        {
            this.subscribers = new Dictionary<Type, IMessageHandler<TMessage, TExecutionResult>>();
        }
        public async Task<TExecutionResult> PublishAsync(TMessage message)
        {
            if(subscribers.TryGetValue(message.GetType(), out var handler))
            {
                return await handler.HandleAsync(message);
            }

            throw new HandlerNotFoundException(typeof(TMessage));
        }

        public void Subscribe(Type messageType, IMessageHandler<TMessage, TExecutionResult> handler)
        {
            //not sure if overwrite or exception should be the correct behavior
            subscribers[messageType] = handler;
        }
    }

    public class InMemoryCommandBus : InMemorySingleSubscriberBus<Command, CommandResult>, ICommandBus { }
}
