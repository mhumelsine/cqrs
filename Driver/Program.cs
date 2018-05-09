using Inventory.Commands;
using Inventory.Queries;
using Isf.Core.Cqrs;
using System;
using System.Threading.Tasks;

namespace Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.WaitAll(Try());

            Console.WriteLine("Hello World!");
        }

        async static Task Try()
        {
            var resolver = new NaiveResolver();

            var command = new CreateInventoryItemCommand
            {
                LIN = "Michael's",
                Description = "Something I'd like to track"
            };

            var query = new InventoryItemQuery("LIN", string.Empty);

            var runtime = new IsfCqrsRuntime(resolver, "Inventory");

            runtime.Start();

            ICommandBus bus = resolver.Resolve<ICommandBus>();
            IQueryBus qBus = resolver.Resolve<IQueryBus>();

            var result = await bus.PublishAsync(command);

            var qResult = await qBus.PublishAsync(query);

            return;
        }
    }
}
