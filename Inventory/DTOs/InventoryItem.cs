using Inventory.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DTOs
{
    public class InventoryItem
    {
        public Guid Id { get; set; }
        public string LIN { get; set; }
        public string Description { get; set; }

        public Task OnInventoryItemCreated(InventoryItemCreatedEvent e)
        {
            Id = Guid.NewGuid();


            System.Diagnostics.Debug.WriteLine($"Handled Event {e.GetType().Name}");

            return Task.CompletedTask;
        }
    }
}
