using Isf.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class CommandDispatcher<TMessage, TResult> : Dispatcher<TMessage, TResult>
            where TMessage : Command
            where TResult : CommandResult
    {
        private readonly Dictionary<Type, Type[]> validatorMap;

        public CommandDispatcher(
            string methodName,
            Type genericInterfaceType,
            Dictionary<Type, Type> handlerMap,
            Dictionary<Type, Type[]> validatorMap)

            : base(
                methodName,
                genericInterfaceType,
                handlerMap)
        {
            this.validatorMap = validatorMap;
        }

        protected virtual object[] CreateValidators(Type messageType)
        {
            if (validatorMap.TryGetValue(messageType, out var validators))
            {
                var handlers = new object[validators.Length];

                for (int i = 0; i < validators.Length; i++)
                {
                    handlers[i] = Activator.CreateInstance(validators[i]);
                }

                return handlers;
            }

            return Array.Empty<object>();
        }

        public async override Task<TResult> DispatchAsync(TMessage message)
        {
            var messageType = message.GetType();
            var handlerType = typeof(CommandHandingContext<>)
                .MakeGenericType(messageType);


            var handler = CreateHandler(messageType);
            var handleMethod = GetHandlerMethod(messageType);

            dynamic context = Activator.CreateInstance(handlerType, new object[] { message, CqrsRuntime.Current.Resolver });

            //var context = Convert.ChangeType(handlingContext, handlerType) as ICommandHandlingContext<TMessage>;


            //var context = new CommandHandingContext<TMessage>(message, CqrsRuntime.Current.Resolver);

            try
            {
                //validate the command

                var validationNotification = await RunAllValidatorsAsync(context, CreateValidators(messageType));

                if (validationNotification.HasErrors)
                {
                    return (TResult)new CommandResult(ExecutionStatus.ValidationFailed, validationNotification);
                }


                //start transaction here

                var result = await (Task<TResult>)handleMethod.Invoke(handler, new object[] { context });

                var eventStore = context.GetMe<IEventStore>();

                await eventStore.SaveAsync(context.UnpublishedEvents);

                //var eventBus = context.GetMe<IEventBus>();



                //commit transaction here

                context.ClearUnpublishedEvents();

                return result;
            }
            catch (Exception ex)
            {
                //TODO:  Log this also
                var notification = new Notification();

                notification.AddError(ex.Message);

                return (TResult)new CommandResult(ExecutionStatus.Failed, notification);
            }
        }

        private async Task<Notification> RunAllValidatorsAsync(dynamic context, dynamic[] validators)
        {
            var notifcations = new Notification[validators.Length];

            for (int i = 0; i < validators.Length; i++)
            {
                notifcations[i] = await validators[i].ValidateAsync(context);
            }

            return Notification.Join(notifcations);
        }
    }
}
