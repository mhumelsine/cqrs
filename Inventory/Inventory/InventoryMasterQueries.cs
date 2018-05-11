using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Inventory
{
    public class GetMasterByIdQuery : Query
    {
        public Guid AggregateRootId { get; set; }
    }

    public class GetMasterByLINQuery : Query
    {
        public string LIN { get; set; }
    }
}
