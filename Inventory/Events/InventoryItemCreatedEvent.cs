using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Events
{
    public class InventoryItemCreatedEvent : Event
    {
        public readonly Guid ItemId;
        public readonly string LIN;
        public readonly string Description;

        public InventoryItemCreatedEvent(Guid itemId, string LIN, string description)
        {
            this.ItemId = itemId;
            this.LIN = LIN;
            this.Description = description;
        }
    }
}
