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

        public async Task<TAggregateRoot> GetByKeyAsync<TAggregateRoot>(params object[] keys) where TAggregateRoot : AggregateRoot, new()
        {
            return await db.FindAsync<TAggregateRoot>(keys);
        }

        public async Task<TAggregateRoot> GetExistingByIdAsync<TAggregateRoot>(params object[] keys) where TAggregateRoot : AggregateRoot, new()
        {
            var aggregateRoot = await GetByKeyAsync<TAggregateRoot>(keys);

            if(aggregateRoot == null)
            {
                throw new AggregateRootNotFoundException<TAggregateRoot>(keys);
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
