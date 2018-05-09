using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Events
{
    public class InventoryItemCreatedEvent : Event
    {
        public readonly string LIN;
        public readonly string Description;
        public readonly Guid InventoryId;

        public InventoryItemCreatedEvent(Guid inventoryId, string LIN, string description)
        {
            this.InventoryId = inventoryId;
            this.LIN = LIN;
            this.Description = description;
        }
    }
}
