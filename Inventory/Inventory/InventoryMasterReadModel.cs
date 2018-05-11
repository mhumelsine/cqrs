using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Inventory
{
    public class InventoryMasterReadModel :
        IHandleQuery<GetMasterByIdQuery>,
        IHandleQuery<GetMasterByLINQuery>
    {
        public Task<QueryResult> HandleAsync(GetMasterByIdQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult> HandleAsync(GetMasterByLINQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
