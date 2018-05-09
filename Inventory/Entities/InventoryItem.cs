using Inventory.Events;
using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Entities
{
    public class InventoryItem : AggregateRoot
    {
        public string LIN { get; set; }
        public string Description { get; set; }

        public InventoryItem(Guid id, string LIN, string description)
        {
            Apply(new InventoryItemCreatedEvent(id, LIN, description));
        }

        public void OnInventoryItemCreated(InventoryItemCreatedEvent e)
        {
            Id = e.Id;
            LIN = e.LIN;
            Description = e.Description;

            System.Diagnostics.Debug.WriteLine($"Handled Event {e.GetType().Name}");
        }


    }
}
