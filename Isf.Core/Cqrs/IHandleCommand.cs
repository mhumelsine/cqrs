using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IHandleCommand<TCommand>
        where TCommand : Command
    {
        Task<CommandResult> HandleAsync(TCommand command);
    }

    public interface IHandleAggregateRootCommand<TAggregateRoot, TCommand>
        where TAggregateRoot : AggregateRoot
        where TCommand : CommandWithAggregateRoot<TAggregateRoot>
    {
        Task<CommandResult> HandleAsync(ICommandHandlingContext<TCommand, TAggregateRoot> handlingContext);
    }
}
