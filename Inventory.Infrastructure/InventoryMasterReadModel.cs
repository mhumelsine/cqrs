using Inventory.Inventory;
using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class InventoryMasterReadModel :
            IHandleQuery<GetMasterByIdQuery>,
            IHandleQuery<GetMasterByLINQuery>
    {        

        public async Task<QueryResult> HandleAsync(GetMasterByIdQuery query)
        {
            var db = IsfCqrsRuntime.Resolver.Resolve<IDomainStore>();

            var item = await db.GetByIdAsync<InventoryMaster>(query.AggregateRootId);

            return QueryResult.Success(item);
        }

        public Task<QueryResult> HandleAsync(GetMasterByLINQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
