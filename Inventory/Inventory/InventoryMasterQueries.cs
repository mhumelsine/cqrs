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

    public class GetMasterByNonmicalture : Query
    {
        public string GeneralNominclature { get; set; }
    }

    public class GetTopInventoryMastersQuery : Query
    {
        public string LIN { get; set; }
        public string GeneralNominclature { get; set; }
    }
}
