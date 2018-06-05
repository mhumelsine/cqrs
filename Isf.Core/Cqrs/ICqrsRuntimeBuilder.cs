using Isf.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Isf.Core.Cqrs
{
    public interface ICqrsRuntimeBuilder
    {
        Dictionary<Type, Type> GetAllCommandHandlers();
        Dictionary<Type, Type[]> GetAllCommandValidators();
        Dictionary<Type, Type> GetAllQueryHandlers();
        Dictionary<Type, Type[]> GetAllEventHandlers();
        CommandDispatcher<Command, CommandResult> GetCommandDispatcher();
        Dispatcher<Query, QueryResult> GetQueryDispatcher();
    }

    public class CqrsRuntimeBuilder : ICqrsRuntimeBuilder
    {
        private const string METHOD_NAME = "HandleAsync";

        private readonly AssemblyHelper assemblyHelper;
        private readonly Dictionary<Type, Type> commandHandlerMap = new Dictionary<Type, Type>();
        private Dictionary<Type, Type[]> commandValidatorMap = new Dictionary<Type, Type[]>();
        private Dictionary<Type, Type> queryHandlerMap = new Dictionary<Type, Type>();
        private Dictionary<Type, Type[]> eventHandlerMap = new Dictionary<Type, Type[]>();

        public CqrsRuntimeBuilder(AssemblyHelper assemblyHelper)
        {
            this.assemblyHelper = assemblyHelper;

            DiscoverSingle(typeof(Command), typeof(IHandleCommand<>), commandHandlerMap);
            DiscoverSingle(typeof(Query), typeof(IHandleQuery<>), queryHandlerMap);

            DiscoverMultiple(typeof(Command), typeof(IValidateCommand<>), commandValidatorMap);
            DiscoverMultiple(typeof(DomainEvent), typeof(IHandleDomainEvent<>), eventHandlerMap);
        }

        private IEnumerable<Type> GetHandlersForType(Type type, IEnumerable<Type> handlers)
        {
            return handlers
                .Where(h => h.GetInterfaces()
                .Any(i => i.GetGenericArguments()
                    .Any(a => a == type)));
        }

        private void RegisterSingle(Type message, IEnumerable<Type> handlers, Dictionary<Type, Type> handlerMap)
        {
            var foundHandlers = GetHandlersForType(message, handlers);

            var handler = foundHandlers.FirstOrDefault();

            if (foundHandlers == null || foundHandlers.Count() == 0)
            {
                throw new HandlerNotFoundException(message);
            }

            if (foundHandlers.Count() > 1)
            {
                throw new DuplicateHandlerException(message, handlerMap[message], handler);
            }

            //throw exception if the handler already exists, should only have 1 handler per request type
            if (!handlerMap.TryAdd(message, handler))
            {
                throw new DuplicateHandlerException(message, handlerMap[message], handler);
            }
        }

        private void DiscoverSingle(Type baseType, Type handlerType, Dictionary<Type, Type> handlerMap)
        {
            var messages = assemblyHelper.GetAllConcreteDerivedTypes(baseType);
            var messageHandlers = assemblyHelper.GetAllGenericInterfaceImplementations(handlerType);

            foreach (var message in messages)
            {
                RegisterSingle(message, messageHandlers, handlerMap);
            }
        }

        private void RegisterMultiple(Type message, IEnumerable<Type> handlers, Dictionary<Type, Type[]> handlerMap)
        {
            var foundHandlers = GetHandlersForType(message, handlers);

            //events might not have any handlers
            //this is not an exception

            if (foundHandlers.Count() > 0)
            {
                handlerMap.Add(message, foundHandlers.ToArray());
            }
        }

        private void DiscoverMultiple(Type baseType, Type handlerType, Dictionary<Type, Type[]> handlerMap)
        {
            var messages = assemblyHelper.GetAllConcreteDerivedTypes(baseType);
            var messageHandlers = assemblyHelper.GetAllGenericInterfaceImplementations(handlerType);

            foreach (var message in messages)
            {
                RegisterMultiple(message, messageHandlers, handlerMap);
            }
        }

        public Dictionary<Type, Type> GetAllCommandHandlers()
        {
            return commandHandlerMap;
        }

        public Dictionary<Type, Type[]> GetAllCommandValidators()
        {
            return commandValidatorMap;
        }

        public Dictionary<Type, Type[]> GetAllEventHandlers()
        {
            return eventHandlerMap;
        }

        public Dictionary<Type, Type> GetAllQueryHandlers()
        {
            return queryHandlerMap;
        }

        public CommandDispatcher<Command, CommandResult> GetCommandDispatcher()
        {
            var dispatcher = new CommandDispatcher<Command, CommandResult>(
                METHOD_NAME,
                typeof(IHandleCommand<>),
                GetAllCommandHandlers(),
                GetAllCommandValidators());

            return dispatcher;
        }

        public Dispatcher<Query, QueryResult> GetQueryDispatcher()
        {
            var dispatcher = new Dispatcher<Query, QueryResult>(
                METHOD_NAME,
                typeof(IHandleQuery<>),
                GetAllQueryHandlers());

            return dispatcher;
        }
    }
}
