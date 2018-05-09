using Inventory.DTOs;
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
        IHandleQuery<InventoryItemQuery, IEnumerable<InventoryItem>>
    {
        public Task<QueryResult<IEnumerable<InventoryItem>>> HandleAsync(InventoryItemQuery query)
        {
            return QueryResult<IEnumerable<InventoryItem>>
                .SuccessAsync(Enumerable.Empty<InventoryItem>());
        }
    }
}
