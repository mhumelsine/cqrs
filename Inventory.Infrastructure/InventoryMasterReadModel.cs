using Inventory.Inventory;
using Isf.Core.Cqrs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var db = IsfCqrsRuntime.Current.Resolver.Resolve<IDomainStore>();

            var item = await db.GetByIdAsync<InventoryMaster>(query.AggregateRootId);

            return QueryResult.Success(item);
        }

        public Task<QueryResult> HandleAsync(GetMasterByLINQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult> HandleAsync(GetTopInventoryMastersQuery query)
        {
            var db = IsfCqrsRuntime.Current.Resolver.Resolve<DomainDbContext>();

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
