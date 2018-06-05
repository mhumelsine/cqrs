using Isf.Core.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Inventory
{
    public class MustHaveUniqueLinValidator :
        IValidateCommand<CreateInventoryMasterCommand>,
        IValidateCommand<UpdateInventoryMasterCommand>
    {
        public async Task<Notification> ValidateAsync(ICommandHandlingContext<CreateInventoryMasterCommand> context)
        {
            return await EnsureUniqueLIN(context.Command.AggregateRootId, context.Command.LIN, context.GetMe<IQueryBus>());
        }

        public async Task<Notification> ValidateAsync(ICommandHandlingContext<UpdateInventoryMasterCommand> context)
        {
            return await EnsureUniqueLIN(context.Command.AggregateRootId, context.Command.LIN, context.GetMe<IQueryBus>());
        }

        private async Task<Notification> EnsureUniqueLIN(Guid id, string lin, IQueryBus queryBus)
        {
            var result = await queryBus.PublishAsync(new GetMasterByLINQuery { LIN = lin });

            var master = (result.Result as IEnumerable<InventoryMaster>)
                .FirstOrDefault(x => x.AggregateRootId != id);

            var notification = new Notification();

            if(master != null)
            {
                notification.AddError($"The LIN '{lin}' is already in use by item '{master.GeneralNomenclature}'", "LIN");
            }

            return notification;
        }
    }
}
