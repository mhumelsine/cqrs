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
            var command = new CreateInventoryItemCommand
            {
                LIN = "Michael's"
            };

            var query = new InventoryItemQuery("LIN", string.Empty);

            var runtime = new IsfCqrsRuntime(new NaiveResolver(), "Inventory");

            runtime.Start();

            ICommandBus bus = new InMemoryCommandBus();
            IQueryBus qBus = new InMemoryQueryBus();

            //temporary should be in runtime
            bus.Subscribe(typeof(CreateInventoryItemCommand), runtime);
            qBus.Subscribe(typeof(InventoryItemQuery), runtime);

            var result = await bus.PublishAsync(command);

            var qResult = await qBus.PublishAsync(query);

            return;
        }
    }
}
