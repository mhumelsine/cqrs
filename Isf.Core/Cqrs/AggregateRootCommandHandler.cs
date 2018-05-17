using Isf.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public abstract class AggregateRootCommandHandler<TAggregateRoot, TCommand> : IHandleCommand<TCommand>
        where TAggregateRoot : AggregateRoot, new()
        where TCommand : Command
    {
        private readonly IDomainStore store;
        public readonly IUsernameProvider usernameProvider;

        public AggregateRootCommandHandler()
        {
            this.store = IsfCqrsRuntime.Current.Resolver.Resolve<IDomainStore>();
            this.usernameProvider = IsfCqrsRuntime.Current.Resolver.Resolve<IUsernameProvider>();
        }

        protected abstract void Handle(CommandHandlingContext handlingContext);

        public async Task<CommandResult> HandleAsync(TCommand command)
        {
            CommandWithAggregateRootId commandWithAggregateRootId = command as CommandWithAggregateRootId;

            TAggregateRoot aggregateRoot = null;

            if (commandWithAggregateRootId != null && commandWithAggregateRootId.AggregateRootId != Guid.Empty)
            {
                aggregateRoot = await store.GetExistingByIdAsync<TAggregateRoot>(commandWithAggregateRootId.AggregateRootId);
            }

            var validationResult = await ValidateAsync(aggregateRoot, command);

            if (validationResult.HasErrors)
            {
                return new CommandResult(ExecutionStatus.ValidationFailed, validationResult);
            }

            var context = new CommandHandlingContext(aggregateRoot, command, usernameProvider);

            Handle(context);

            await store.SaveAsync(context.AggregateRoot);

            //set after DB write
            commandWithAggregateRootId.AggregateRootId = context.AggregateRoot.AggregateRootId;

            return CommandResult.Success();
        }

        protected virtual Task<Notification> ValidateAsync(TAggregateRoot aggregateRoot, TCommand command)
        {
            return Task.FromResult(Notification.OK);
        }

        protected class CommandHandlingContext
        {
            public readonly IUsernameProvider UsernameProvider;
            public TAggregateRoot AggregateRoot { get; set; }
            public TCommand Command { get; set; }

            public CommandHandlingContext(TAggregateRoot aggregateRoot, TCommand command, IUsernameProvider usernameProvider)
            {
                this.AggregateRoot = aggregateRoot;
                this.Command = command;
                this.UsernameProvider = usernameProvider;
            }
        }
    }
}
