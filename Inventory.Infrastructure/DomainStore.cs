using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure
{
    public class DomainStore : EfDomainStore
    {
        public DomainStore(DomainDbContext dbContext, IEventStore eventStore, IEventBus eventBus)
            :base(dbContext, eventStore, eventBus)
        {

        }
    }
}
