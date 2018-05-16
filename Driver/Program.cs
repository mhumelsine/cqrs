using Inventory.Infrastructure;
using Inventory.Inventory;
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

            var command = new CreateInventoryMasterCommand
            {
                LIN = "Michael's LIN",
                GeneralNomenclature = "Something I'd like to track"
            };

            var query = new GetMasterByLINQuery
            {
                LIN = "Michael's LIN"
            };

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
