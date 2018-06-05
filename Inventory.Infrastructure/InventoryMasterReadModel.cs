using Inventory.Inventory;
using Isf.Core.Cqrs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class InventoryMasterReadModel :
            IHandleQuery<GetMasterByIdQuery>,
            IHandleQuery<GetMasterByLINQuery>,
            IHandleQuery<GetTopInventoryMastersQuery>,
            IHandleQuery<GetMasterByNonmicalture>
    {

        public async Task<QueryResult> HandleAsync(GetMasterByIdQuery query)
        {
            var db = CqrsRuntime.Current.Resolver.GetMe<IDomainStore>();

            var item = await db.GetByIdAsync<InventoryMaster>(query.AggregateRootId);

            return QueryResult.Success(item);
        }

        public async Task<QueryResult> HandleAsync(GetMasterByLINQuery query)
        {
            var db = CqrsRuntime.Current.Resolver.GetMe<DomainDbContext>();

            var items = await db.InventoryMasters
                .Where(x => x.LIN == query.LIN)
                .ToListAsync();

            return QueryResult.Success(items);
        }

        public async Task<QueryResult> HandleAsync(GetTopInventoryMastersQuery query)
        {
            var db = CqrsRuntime.Current.Resolver.GetMe<DomainDbContext>();

            //cheating b/c using in memory DB
            var list = await db.Set<InventoryMaster>().ToListAsync();

            return QueryResult.Success(list);
        }

        public Task<QueryResult> HandleAsync(GetMasterByNonmicalture query)
        {
            throw new NotImplementedException();
        }
    }
}
