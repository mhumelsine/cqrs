using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (subscribers.TryGetValue(message.GetType(), out var handler))
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

    public class InMemoryMultiSubscriberBus<TMessage, TExecutionResult> : ISingleSubscriberBus<TMessage, TExecutionResult>
    where TMessage : Message
    where TExecutionResult : ExecutionResult
    {
        private readonly Dictionary<Type, HashSet<IMessageHandler<TMessage, TExecutionResult>>> subscribers;

        public InMemoryMultiSubscriberBus()
        {
            this.subscribers = new Dictionary<Type, HashSet<IMessageHandler<TMessage, TExecutionResult>>>();
        }
        public async Task<TExecutionResult> PublishAsync(TMessage message)
        {
            if (subscribers.TryGetValue(message.GetType(), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    return await handler.HandleAsync(message);
                }
            }

            throw new HandlerNotFoundException(typeof(TMessage));
        }

        public void Subscribe(Type messageType, IMessageHandler<TMessage, TExecutionResult> handler)
        {
            HashSet<IMessageHandler<TMessage, TExecutionResult>> set;

            if (!subscribers.TryGetValue(messageType, out set))
            {
                set = new HashSet<IMessageHandler<TMessage, TExecutionResult>>();

                subscribers.Add(messageType, set);
            }

            if (set.Contains(handler))
            {
                var handlerType = handler.GetType();
                throw new DuplicateHandlerException(messageType, handlerType, handlerType);
            }

            set.Add(handler);
        }
    }

    public class InMemoryCommandBus : InMemorySingleSubscriberBus<Command, CommandResult>, ICommandBus { }
    public class InMemoryQueryBus : InMemorySingleSubscriberBus<Query, QueryResult>, IQueryBus { }

    public class InMemoryEventBus : InMemoryMultiSubscriberBus<DomainEvent, ExecutionResult>, IEventBus
    {
        public async Task PostAsync<TEvent>(DomainEvent domainEvent)
        {
            await base.PublishAsync(domainEvent);
        }

        public async Task PostAsync(IEnumerable<DomainEvent> domainEvents)
        {
            foreach(var e in domainEvents)
            {
                await PostAsync<DomainEvent>(e);
            }
        }
    }
}
