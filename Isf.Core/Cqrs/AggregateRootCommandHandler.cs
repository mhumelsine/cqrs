﻿using Isf.Core.Common;
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

        public AggregateRootCommandHandler()
        {
            this.store = new EfDomainStore();
        }

        protected abstract void Handle(CommandHandlingContext handlingContext);

        public async Task<CommandResult> HandleAsync(TCommand command)
        {
            CommandWithAggregateRootId commandWithAggregateRootId = command as CommandWithAggregateRootId;

            TAggregateRoot aggregateRoot = null;

            if(commandWithAggregateRootId != null)
            {
                aggregateRoot = await store.GetExistingByIdAsync<TAggregateRoot>(commandWithAggregateRootId.AggregateRootId);
            }            

            var validationResult = await ValidateAsync(aggregateRoot, command);

            if (validationResult.HasErrors)
            {
                return new CommandResult(ExecutionStatus.ValidationFailed, validationResult);
            }

            var context = new CommandHandlingContext(aggregateRoot, command);

            Handle(context);

            await store.SaveAsync(context.AggregateRoot);

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

            public CommandHandlingContext(TAggregateRoot aggregateRoot, TCommand command)
            {
                this.AggregateRoot = aggregateRoot;
                this.Command = command;
            }
        }
    }
}