using Inventory.Commands;
using Inventory.Entities;
using Inventory.Events;
using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class InventoryService :
        IHandleCommand<CreateInventoryItemCommand>,
        IHandleCommand<UpdateInventoryItemCommand>
    {

       

        public InventoryService()
        {

        }

        public Task<CommandResult> HandleAsync(CreateInventoryItemCommand command)
        {
            System.Diagnostics.Debug.WriteLine($"Processed command {command.GetType().Name}");

            //await eventBus.PostEventAsync(
            //    new InventoryItemCreatedEvent(
            //        Guid.NewGuid(),
            //        command.LIN,
            //        command.Description));

            var item = new InventoryItem(
                command.Id,
                command.LIN,
                command.Description);

            return CommandResult.SuccessAsync();
        }

        public Task<CommandResult> HandleAsync(UpdateInventoryItemCommand command)
        {
            return CommandResult.SuccessAsync();
        }
    }
}
