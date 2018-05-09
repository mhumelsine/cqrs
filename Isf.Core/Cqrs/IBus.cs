using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface ISingleSubscriberBus<TMessage, TExecutionResult>
        where TMessage : Message
        where TExecutionResult : ExecutionResult
    {
        Task<TExecutionResult> PublishAsync(TMessage message);

        void Subscribe(Type messageType, IMessageHandler<TMessage, TExecutionResult> handler);
    }

    public interface ICommandBus : ISingleSubscriberBus<Command, CommandResult> { }
    public interface IQueryBus : ISingleSubscriberBus<Query, QueryResult> { }
}


