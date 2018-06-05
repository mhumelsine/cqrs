using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Inventory
{
    public class InventoryMaster : AggregateRoot
    {        
        public int Id { get; set; }
        public string LIN { get; set; }

        public string GeneralNomenclature { get; set; }

        public bool IsGArmy { get; set; }

        public int TrackingType { get; set; }

        public int Status { get; set; }
    }
}
