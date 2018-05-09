using Inventory.Entities;
using Inventory.Queries;
using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Stores
{
    public class InventoryReadStore :
        IHandleQuery<InventoryItemQuery>
    {
        public Task<QueryResult> HandleAsync(InventoryItemQuery query)
        {
            return QueryResult
                .SuccessAsync(Enumerable.Empty<InventoryItem>());
        }
    }
}
