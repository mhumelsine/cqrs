using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Isf.Core.Cqrs
{
    public abstract class AggregateRoot
    {
        public Guid AggregateRootId { get; protected set; }

        public readonly Queue<DomainEvent> UncommittedEvents = new Queue<DomainEvent>();

        protected void Apply(DomainEvent @event)
        {
            UncommittedEvents.Enqueue(@event);
            ApplyEventToInternalState(@event);
        }

        private void ApplyEventToInternalState(DomainEvent @event)
        {
            var eventType = @event.GetType();
            var eventTypeName = eventType.Name;
            var myType = GetType();
            var eventIndex = eventTypeName.LastIndexOf("Event");
            var methodName = $"On{eventTypeName.Remove(eventIndex, 5)}";

            var methodInfo = myType.GetMethod(methodName);

            if(methodInfo == null)
            {
                throw new HandlerNotFoundException(eventType);
            }

            methodInfo.Invoke(this, new[] { @event });
        }


    }
}
