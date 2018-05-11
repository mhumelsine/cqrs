using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public interface ICommandHandlingContext<TCommand, TAggregateRoot>
        where TCommand : Command
    {
        CommandResult Result { get; }
        TCommand Command { get; }
        TAggregateRoot AggregateRoot { get; set; }
    }
}
