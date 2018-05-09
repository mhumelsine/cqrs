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
}
