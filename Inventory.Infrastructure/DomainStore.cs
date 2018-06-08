using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure
{
    public class DomainStore : EfDomainAndEventStore
    {
        public DomainStore(DomainDbContext dbContext, IEventBus eventBus)
            :base(dbContext, eventBus)
        {

        }
    }
}
