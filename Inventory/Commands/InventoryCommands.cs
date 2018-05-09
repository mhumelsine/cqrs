using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Commands
{
    public class CreateInventoryItemCommand : Command
    {
        public string LIN { get; set; }
        public string Description { get; set; }
    }

    public class UpdateInventoryItemCommand : Command
    {
        public string LIN { get; set; }
    }
}
