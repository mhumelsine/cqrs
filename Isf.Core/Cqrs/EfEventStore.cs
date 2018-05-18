using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class EfEventStore : IEventStore
    {
        private readonly EventDbContext db;
        private readonly DbSet<DomainEvent> events;

        public EfEventStore(EventDbContext db)
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
            var events = new List<DomainEvent>();

            foreach (var e in domainEvents)
            {
                events.Add(new DomainEvent(e.AggregateRootId, e.EventSequence, e.UserCreated)
                {
                    EventData = JsonConvert.SerializeObject(GetOwnPropsEvent(e))
                });
            }

            await db.Set<DomainEvent>().AddRangeAsync(events);

            await db.SaveChangesAsync();
        }

        private object GetOwnPropsEvent(DomainEvent e)
        {
            IDictionary<string, object> dynamicEvent = new ExpandoObject();

            var props = e.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in props)
            {
                dynamicEvent[property.Name] = property.GetValue(e);
            }

            return dynamicEvent;
        }
    }
}
