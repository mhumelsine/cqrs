using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public class Dispatcher<TMessage, TResult>
            where TMessage : Message
            where TResult : ExecutionResult
    {
        private readonly Dictionary<Type, Type> handlerMap;
        private readonly string methodName;
        private readonly Type genericInterfaceType;

        public Dispatcher(
            string methodName,
            Type genericInterfaceType,
            Dictionary<Type, Type> handlerMap)
        {
            this.methodName = methodName;
            this.genericInterfaceType = genericInterfaceType;
            this.handlerMap = handlerMap;
        }

        protected virtual object CreateHandler(Type messageType)
        {
            var handler = handlerMap[messageType];

            if (handler == null)
            {
                throw new HandlerNotFoundException(messageType);
            }

            return Activator.CreateInstance(handler);
        }

        protected virtual MethodInfo GetHandlerMethod(Type messageType)
        {
            var handleMethod = genericInterfaceType
                .MakeGenericType(messageType)
                .GetMethod(methodName);

            return handleMethod;
        }

        protected virtual object[] GetHandleMethodParameters(TMessage message)
        {
            return new object[] { message };
        }

        public virtual async Task<TResult> DispatchAsync(TMessage message)
        {
            var messageType = message.GetType();
            var handler = CreateHandler(messageType);
            var handleMethod = GetHandlerMethod(messageType);
            var parameters = GetHandleMethodParameters(message);

            var result = await (Task<TResult>)
                    handleMethod.Invoke(handler, parameters);

            return result;
        }
    }
}
