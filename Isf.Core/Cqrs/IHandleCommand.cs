using Isf.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    //TODO:  Make an attribute for filtering command handlers by customer
    public interface IHandleCommand<TCommand>
    where TCommand : Command
    {
        Task<CommandResult> HandleAsync(ICommandHandlingContext<TCommand> context);
    }
}
