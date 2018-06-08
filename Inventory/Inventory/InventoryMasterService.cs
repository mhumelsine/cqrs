using Isf.Core.Utils;
using Isf.Core.Cqrs;
using System.Threading.Tasks;

namespace Inventory.Inventory
{
    public class InventoryMasterService :
        IHandleCommand<CreateInventoryMasterCommand>,
        IHandleCommand<UpdateInventoryMasterCommand>,
        IHandleCommand<DeleteInventoryMasterCommand>
    {    

        public async Task<CommandResult> HandleAsync(ICommandHandlingContext<UpdateInventoryMasterCommand> context)
        {
            var db = context.GetMe<IDomainStore>();
            var command = context.Command;
            var usernameProvider = context.GetMe<IUsernameProvider>();

            var master = await db.GetExistingByIdAsync<InventoryMaster>(command.AggregateRootId);

            //i want to see when the lin changes

            if (master.LIN != command.LIN)
            {
                context.PublishEvent(new LinChangedEvent(
                    master.AggregateRootId,
                    command.LIN,
                    usernameProvider.GetUsername()));
            }

                master.LIN = command.LIN;
            master.GeneralNomenclature = command.GeneralNomenclature;
            master.IsGArmy = command.IsGArmy;
            master.TrackingType = command.TrackingType;
            master.Status = command.Status;            

            await db.SaveAsync(master);

            //want to track created event            

            context.PublishEvent(new InventoryMasterUpdatedEvent(
                master.AggregateRootId,
                command.LIN,
                command.GeneralNomenclature,
                command.IsGArmy,
                command.TrackingType,
                usernameProvider.GetUsername()));

            return CommandResult.Success();
        }

        public async Task<CommandResult> HandleAsync(ICommandHandlingContext<DeleteInventoryMasterCommand> context)
        {
            var db = context.GetMe<IDomainStore>();
            var command = context.Command;

            await db.DeleteAsync<InventoryMaster>(command.AggregateRootId);

            return CommandResult.Success();
        }

        public async Task<CommandResult> HandleAsync(ICommandHandlingContext<CreateInventoryMasterCommand> context)
        {
            var db = context.GetMe<IDomainStore>();
            var command = context.Command;

            var master = new InventoryMaster
            {
                GeneralNomenclature = command.GeneralNomenclature,
                IsGArmy = command.IsGArmy,
                LIN = command.LIN,
                Status = command.Status,
                TrackingType = command.TrackingType
            };

            await db.SaveAsync(master);

            //want to track created event
            var usernameProvider = context.GetMe<IUsernameProvider>();

            context.PublishEvent(new InventoryMasterCreatedEvent(
                master.AggregateRootId,
                command.LIN,
                command.GeneralNomenclature,
                command.IsGArmy,
                command.TrackingType,
                usernameProvider.GetUsername()));

            return CommandResult.Success();
        }        
    }   
}
