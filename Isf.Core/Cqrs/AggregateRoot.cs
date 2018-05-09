using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class AggregateRoot
    {
        public Guid Id { get; set; }

        private Queue<Event> uncommittedEvents = new Queue<Event>();

        protected void Apply(Event @event)
        {
            uncommittedEvents.Enqueue(@event);
            ApplyEventToInternalState(@event);
        }

        private void ApplyEventToInternalState(Event @event)
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
