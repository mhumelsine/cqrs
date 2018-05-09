using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IEventBus
    {
        Task PostEventAsync<TEvent>(TEvent @event) where TEvent : Event;
    }
}
