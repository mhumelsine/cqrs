using Isf.Core.Common;
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

    public interface ICommandHandlingContext<TCommand>
        where TCommand : Command
    {       
        TCommand Command { get; }
        T GetMe<T>() where T : class;
        IEnumerable<DomainEvent> UnpublishedEvents { get; }

        void PublishEvent(DomainEvent domainEvent);

        void ClearUnpublishedEvents();
    }

    public class CommandHandingContext<TCommand> : ICommandHandlingContext<TCommand>
        where TCommand : Command
    {

        private readonly IList<DomainEvent> unpublishedEvents;
        private readonly IResolver resolver;

        public TCommand Command { get; private set; }

        
        public IEnumerable<DomainEvent> UnpublishedEvents {
            get
            {
                return unpublishedEvents;
            }
        }

        public CommandHandingContext(TCommand command, IResolver resolver)
        {
            Command = command;
            unpublishedEvents = new List<DomainEvent>();
            this.resolver = resolver;
        }

        public void ClearUnpublishedEvents()
        {
            unpublishedEvents.Clear();
        }

        public T GetMe<T>() where T : class
        {
            return resolver.GetMe<T>();
        }

        public void PublishEvent(DomainEvent domainEvent)
        {
            unpublishedEvents.Add(domainEvent);
        }
    }

}
