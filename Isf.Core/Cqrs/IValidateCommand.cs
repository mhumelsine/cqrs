using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IValidateCommand<TCommand>
        where TCommand : Command
    {
        Task<Notification> ValidateAsync(ICommandHandlingContext<TCommand> context);
    }
}
