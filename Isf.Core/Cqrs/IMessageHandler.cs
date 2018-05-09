using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IMessageHandler<TMessage, TExecutionResult>
        where TMessage : Message
        where TExecutionResult : ExecutionResult
    {
        Task<TExecutionResult> HandleAsync(TMessage message);
    }
}
