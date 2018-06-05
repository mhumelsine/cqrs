using Isf.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class CqrsRuntime :
            IMessageHandler<Command, CommandResult>,
            IMessageHandler<Query, QueryResult>
    {
        public static CqrsRuntime Current;
        public IResolver Resolver;

        private readonly CommandDispatcher<Command, CommandResult> commandDispatcher;
        private readonly Dispatcher<Query, QueryResult> queryDispatcher;

        public CqrsRuntime(ICqrsRuntimeBuilder builder)
        {
            commandDispatcher = builder.GetCommandDispatcher();
            queryDispatcher = builder.GetQueryDispatcher();
        }

        public static void Start(IResolver resolver, params string[] assembliesToScan)
        {
            //skip if already started
            if (Current != null)
            {
                return;
            }

            var assemblyHelper = new AssemblyHelper(assembliesToScan);

            var builder = new CqrsRuntimeBuilder(assemblyHelper);

            Current = new CqrsRuntime(builder);

            Current.SetResolver(resolver);

            Current.SubscribeToBus(builder);
        }

        private void SubscribeToBus(ICqrsRuntimeBuilder builder)
        {
            SubscribeToCommands(builder);
            SubscribeToQueries(builder);
            //SubscribeToEvents(builder);
        }

        private void SubscribeToQueries(ICqrsRuntimeBuilder builder)
        {
            var queryBus = Resolver.GetMe<IQueryBus>();

            foreach (var query in builder.GetAllQueryHandlers().Keys)
            {
                queryBus.Subscribe(query, this);
            }
        }

        private void SubscribeToCommands(ICqrsRuntimeBuilder builder)
        {
            var commandBus = Resolver.GetMe<ICommandBus>();

            foreach (var command in builder.GetAllCommandHandlers().Keys)
            {
                commandBus.Subscribe(command, this);
            }
        }

        //TODO:Event subscriptions
        //private void SubscribeToEvents(ICqrsRuntimeBuilder builder)
        //{
        //    var eventBus = Resolver.GetMe<IEventBus>();

        //    foreach (var domainEvent in builder.GetAllEventHandlers().Keys)
        //    {
        //        eventBus.Subscribe(domainEvent, this);
        //    }
        //}

        public void SetResolver(IResolver resolver)
        {
            Resolver = resolver;
        }

        public async Task<CommandResult> HandleAsync(Command message)
        {
            return await commandDispatcher.DispatchAsync(message);
        }

        public async Task<QueryResult> HandleAsync(Query message)
        {
            return await queryDispatcher.DispatchAsync(message);
        }
    }
}
