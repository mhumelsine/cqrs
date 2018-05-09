using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class CqrsDispatcher<TMessage, TExecutionResult>
        where TMessage : Message
        where TExecutionResult : ExecutionResult
    {
        private readonly IDictionary<Type, Type> handlerMap;
        private readonly IResolver resolver;

        public CqrsDispatcher(IDictionary<Type, Type> handlerMap, IResolver resolver)
        {
            this.handlerMap = handlerMap;
            this.resolver = resolver;
        }

        public async Task<TExecutionResult> DispatchAsync(TMessage message)
        {
            try
            {
                var handler = CreateHandler(message.GetType());

                var method = typeof(IHandleCommand<>)
                    .MakeGenericType(message.GetType())
                    .GetMethod("HandleAsync");

                var result = await (Task<TExecutionResult>)
                    method.Invoke(handler, new object[] { message });

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected object CreateHandler(Type messageType)
        {
            var handler = handlerMap[messageType];

            if (handler == null)
            {
                throw new HandlerNotFoundException(typeof(TMessage));
            }

            var paramList = new List<object>();

            var parameters = handler
                .GetConstructors()
                .First()
                .GetParameters();

            foreach (var param in parameters)
            {
                paramList.Add(resolver.Resolve(param.ParameterType));
            }

            return Activator.CreateInstance(handler, paramList.ToArray());
        }

        //protected IEnumerable<object> CreateHandlers()
        //{
        //    var handlers = handlerMap[typeof(TMessage)];

        //    if(handlers == null)
        //    {
        //        throw new HandlerNotFoundException(typeof(TMessage));
        //    }

        //    foreach (var handler in handlers)
        //    {

        //        var paramList = new List<object>();

        //        var parameters = handler
        //            .GetConstructors()
        //            .First()
        //            .GetParameters();

        //        foreach (var param in parameters)
        //        {
        //            paramList.Add(resolver.Resolve(param.ParameterType));
        //        }

        //        yield return Activator.CreateInstance(handler, paramList.ToArray());
        //    }
        //}
    }
}
