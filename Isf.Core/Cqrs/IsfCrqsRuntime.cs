using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class IsfCqrsRuntime :
        IMessageHandler<Command, CommandResult>,
        IMessageHandler<Query, QueryResult>
    {

        private const string
            COMMAND_SUFFIX = "Command",
            QUERY_SUFFIX = "Query",
            EVENT_SUFFIX = "Event";

        private IDispatcher<Command, CommandResult> commandDispatcher;
        private IDispatcher<Query, QueryResult> queryDispatcher;
        public readonly IResolver resolver;
        private readonly string[] assembliesToScan;
        private Dictionary<Type, Type> commandHandlerMap = new Dictionary<Type, Type>();
        private Dictionary<Type, Type> queryHandlerMap = new Dictionary<Type, Type>();
        private Dictionary<Type, Type[]> eventHandlerMap = new Dictionary<Type, Type[]>();

        private IEnumerable<Type> allTypes;
        private IEnumerable<Type> AllTypes
        {
            get
            {
                if (allTypes == null)
                {
                    allTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => assembliesToScan.Any(asm => x.FullName.StartsWith(asm)))
                        .SelectMany(assembly => assembly.GetTypes());
                }

                return allTypes;
            }
        }

        public IsfCqrsRuntime(IResolver resolver, params string[] assembliesToScan)
        {
            this.resolver = resolver;
            this.assembliesToScan = assembliesToScan;
        }


        public async Task<CommandResult> HandleAsync(Command message)
        {
            return await commandDispatcher.ExecuteAsync(message);
        }

        public async Task<QueryResult> HandleAsync(Query message)
        {
            return await queryDispatcher.ExecuteAsync(message);
        }

        public void Start()
        {
            RegisterCommandsAndHandlers();
            RegisterQueriesAndHandlers();
            RegisterEventsAndHandlers();

            //register all the handlers on the bus
            var commandBus = resolver.Resolve<ICommandBus>();
            var queryBus = resolver.Resolve<IQueryBus>();

            foreach(var commandType in commandHandlerMap.Keys)
            {
                commandBus.Subscribe(commandType, this);
            }

            foreach (var queryType in queryHandlerMap.Keys)
            {
                queryBus.Subscribe(queryType, this);
            }

            this.commandDispatcher = new Dispatcher<Command, CommandResult>(
                "HandleAsync", 
                typeof(IHandleCommand<>),
                commandHandlerMap,
                resolver);

            this.queryDispatcher = new Dispatcher<Query, QueryResult>(
                "HandleAsync", 
                typeof(IHandleQuery<>),
                queryHandlerMap,
                resolver);
        }

        private IEnumerable<Type> GetAllCommandHandlers()
        {
            return GetAllGenericInterfaceImplementations(typeof(IHandleCommand<>));
        }

        private IEnumerable<Type> GetAllQueryHandlers()
        {
            return GetAllGenericInterfaceImplementations(typeof(IHandleQuery<>));
        }

        private IEnumerable<Type> GetAllEventHandlers()
        {
            return GetAllGenericInterfaceImplementations(typeof(IHandleDomainEvent<>));
        }

        private IEnumerable<Type> GetAllGenericInterfaceImplementations(Type genericInterfaceType)
        {
            return AllTypes
                .Where(x => x.IsClass)
                .Where(x => x.GetInterfaces()
                    .Where(iface => iface.IsGenericType)
                        .Any(iface => genericInterfaceType == iface.GetGenericTypeDefinition()));
        }

        private IEnumerable<Type> GetAllCommands()
        {
            return AllTypes
                .Where(x => x.Name.EndsWith(COMMAND_SUFFIX));
        }

        private IEnumerable<Type> GetAllQueries()
        {
            return AllTypes
                .Where(x => x.Name.EndsWith(QUERY_SUFFIX));
        }

        private IEnumerable<Type> GetAllEvents()
        {
            return AllTypes
                .Where(x => x.Name.EndsWith(EVENT_SUFFIX));
        }

        private IEnumerable<Type> GetHandlersForType(Type type, IEnumerable<Type> handlers)
        {
            return handlers.Where(h => h.GetInterfaces()
                .Any(i => i.GetGenericArguments()
                    .Any(a => a == type)));
        }

        private void RegisterSingle(Type dto, IEnumerable<Type> handlers, Dictionary<Type, Type> handlerMap)
        {
            var foundHandlers = GetHandlersForType(dto, handlers);

            var handler = foundHandlers.FirstOrDefault();

            if (foundHandlers == null || foundHandlers.Count() == 0)
            {
                throw new HandlerNotFoundException(dto);
            }

            if (foundHandlers.Count() > 1)
            {
                throw new DuplicateHandlerException(dto, handlerMap[dto], handler);
            }

            //throw exception if the handler already exists, should only have 1 handler per request type
            if (!handlerMap.TryAdd(dto, handler))
            {
                throw new DuplicateHandlerException(dto, handlerMap[dto], handler);
            }
        }

        private void RegisterMultiple(Type dto, IEnumerable<Type> handlers, Dictionary<Type, Type[]> handlerMap)
        {
            var foundHandlers = GetHandlersForType(dto, handlers);

            //events might not have any handlers
            //this is not an exception

            if(foundHandlers.Count() > 0)
            {
                handlerMap.Add(dto, foundHandlers.ToArray());
            }
            
        }

        private void RegisterCommandsAndHandlers()
        {
            foreach (var command in GetAllCommands()) {
                RegisterSingle(command, GetAllCommandHandlers(), commandHandlerMap);
            }
        }

        private void RegisterQueriesAndHandlers()
        {
            foreach (var query in GetAllQueries())
            {
                RegisterSingle(query, GetAllQueryHandlers(), queryHandlerMap);
            }
        }

        private void RegisterEventsAndHandlers()
        {
            foreach (var e in GetAllEvents())
            {
                RegisterMultiple(e, GetAllEventHandlers(), eventHandlerMap);
            }
        }
    }
}
