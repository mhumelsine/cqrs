using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Queries
{

    public class InventoryItemQuery : Query
    {
        public readonly string LIN;
        public readonly string Description;

        public InventoryItemQuery(string LIN, string description)
        {
            this.LIN = LIN;
            this.Description = description;
        }
    }
}
