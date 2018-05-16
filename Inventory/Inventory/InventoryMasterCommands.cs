using Isf.Core.Cqrs;
using System;

namespace Inventory.Inventory
{
    public class CreateInventoryMasterCommand : CommandWithAggregateRootId
    {
        public string LIN { get; set; }

        public string GeneralNomenclature { get; set; }

        public bool IsGArmy { get; set; }

        public int TrackingType { get; set; }

        public int Status { get; set; }
    }

    public class UpdateInventoryMasterCommand : CommandWithAggregateRootId
    {
        public string LIN { get; set; }

        public string GeneralNomenclature { get; set; }

        public bool IsGArmy { get; set; }

        public int TrackingType { get; set; }

        public int Status { get; set; }
    }

    public class DeleteInventoryMasterCommand : CommandWithAggregateRootId
    {
    }
}
