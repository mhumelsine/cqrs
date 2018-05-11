using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class EfEventStore : IEventStore
    {
        private readonly DbContext db;
        private readonly 
       /private readonly DbSet<DomainEvent> events;

        public EfEventStore(DbContext db)
        {
            this.db = db;
            this.events = db.Set<DomainEvent>();
        }

        public async Task<IEnumerable<DomainEvent>> GetAllEventsAsync(Guid aggregateRootId)
        {
            return await events
                .Where(e => e.AggregateRootId == aggregateRootId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, int startAtSequence)
        {
            return await events
                .Where(e => e.AggregateRootId == aggregateRootId
                        && e.EventSequence >= startAtSequence)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, DateTime startDate)
        {
            return await events
                .Where(e => e.AggregateRootId == aggregateRootId
                        && e.EventTimestamp >= startDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateRootId, DateTime startDate, DateTime endDate)
        {
            return await events
                .Where(e => e.AggregateRootId == aggregateRootId
                        && e.EventTimestamp >= startDate
                        && e.EventTimestamp <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsByTypeAsync(Guid aggregateRootId, Type eventType)
        {
            return await events
                .Where(e => e.AggregateRootId == aggregateRootId
                        && e.EventName == eventType.Name)
                .ToListAsync();
        }

        public async Task SaveAsync(DomainEvent domainEvent)
        {
            await events.AddAsync(domainEvent);

            await db.SaveChangesAsync();
        }

        public async Task SaveAsync(IEnumerable<DomainEvent> domainEvents)
        {
            await events.AddRangeAsync(domainEvents);

            await db.SaveChangesAsync();
        }
    }
}
