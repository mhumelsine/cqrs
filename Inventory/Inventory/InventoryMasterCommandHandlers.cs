using Isf.Core.Common;
using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Inventory
{
    public class CreateInventoryMasterHandler : AggregateRootCommandHandler<InventoryMaster, CreateInventoryMasterCommand>
    {        
        protected override void Handle(CommandHandlingContext handlingContext)
        {
            var command = handlingContext.Command;

            handlingContext.AggregateRoot = new InventoryMaster(
                command.LIN,
                command.GeneralNomenclature,
                command.IsGArmy,
                command.TrackingType,
                handlingContext.UsernameProvider.GetUsername());
        }
    }

    public class UpdateInventoryMasterHandler : AggregateRootCommandHandler<InventoryMaster, UpdateInventoryMasterCommand>
    {
        protected override void Handle(CommandHandlingContext handlingContext)
        {
            var command = handlingContext.Command;

            handlingContext.AggregateRoot.Edit(
                command.LIN,
                command.GeneralNomenclature,
                command.IsGArmy,
                command.TrackingType,
                handlingContext.UsernameProvider.GetUsername());
        }
    }

    public class DeleteInventoryMasterHandler : AggregateRootCommandHandler<InventoryMaster, DeleteInventoryMasterCommand>
    {
        protected override void Handle(CommandHandlingContext handlingContext)
        {
            handlingContext.AggregateRoot.Delete();
        }
    }    
}
