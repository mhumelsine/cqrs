using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class EfDomainStore : IDomainStore
    {
        private readonly DbContext db;
        private readonly IEventStore eventStore;
        private readonly IEventBus eventBus;

        public EfDomainStore(DbContext dbContext, IEventStore eventStore, IEventBus eventBus)
        {
            this.db = dbContext;
            this.eventStore = eventStore;
            this.eventBus = eventBus;
        }

        public async Task<TAggregateRoot> GetByIdAsync<TAggregateRoot>(Guid aggregateRootId) where TAggregateRoot : AggregateRoot, new()
        {
            return await db.FindAsync<TAggregateRoot>(aggregateRootId);
        }

        public async Task<TAggregateRoot> GetExistingByIdAsync<TAggregateRoot>(Guid aggregateRootId) where TAggregateRoot : AggregateRoot, new()
        {
            var aggregateRoot = await GetByIdAsync<TAggregateRoot>(aggregateRootId);

            if(aggregateRoot == null)
            {
                throw new AggregateRootNotFoundException<TAggregateRoot>(aggregateRootId);
            }

            return aggregateRoot;
        }

        public async Task SaveAsync(AggregateRoot aggregateRoot)
        {
            var events = aggregateRoot.UncommittedEvents;

            await eventStore.SaveAsync(events);

            await db.SaveChangesAsync();

            await eventBus.PostAsync(events);
        }
    }
}
