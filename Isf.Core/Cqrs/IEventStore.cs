using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IEventStore
    {
        Task<IEnumerable<DomainEvent>> GetAllEventsAsync(Guid aggregateRootId);

        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, int startAtSequence);

        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, DateTime startDate);

        Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, DateTime startDate, DateTime endDate);

        Task<IEnumerable<DomainEvent>> GetEventsByTypeAsync(Guid aggregateRootId, Type eventType);

        Task SaveAsync(DomainEvent domainEvent);

        Task SaveAsync(IEnumerable<DomainEvent> domainEvents);
    }
}
