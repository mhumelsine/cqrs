using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IEventBus
    {
        Task PostAsync<TEvent>(DomainEvent domainEvent);
        Task PostAsync(IEnumerable<DomainEvent> domainEvents);
    }
}
