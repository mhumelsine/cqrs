using Inventory.Commands;
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

            var runtime = new IsfCqrsRuntime(new NaiveResolver(), "Inventory");

            runtime.Start();

            ICommandBus bus = new InMemoryCommandBus();

            //temporary should be in runtime
            bus.Subscribe(typeof(CreateInventoryItemCommand), runtime);


            var result = await bus.PublishAsync(command);
        }
    }
}
