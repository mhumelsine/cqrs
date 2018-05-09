using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isf.Core.Cqrs
{
    public interface IDispatcher<TMessage, TResult>
    {
        Task<TResult> ExecuteAsync(TMessage message);
    }

    public class Dispatcher<TMessage, TResult> : IDispatcher<TMessage, TResult>
    {
        private readonly string methodName;
        private readonly Type genericInterfaceType;
        private readonly IDictionary<Type, Type> handlerMap;
        private readonly IResolver resolver;

        public Dispatcher(
            string dispatchMethodName, 
            Type genericInterfaceType,
            IDictionary<Type, Type> handlerMap,
            IResolver resolver)
        {
            this.methodName = dispatchMethodName;
            this.genericInterfaceType = genericInterfaceType;
            this.handlerMap = handlerMap;
            this.resolver = resolver;
        }

        public async Task<TResult> ExecuteAsync(TMessage message)
        {
            var messageType = message.GetType();
            var handler = CreateHandler(messageType);

            var handleMethod = genericInterfaceType
                    .MakeGenericType(messageType)
                    .GetMethod(methodName);

            var result = await (Task<TResult>)
                    handleMethod.Invoke(handler, new object[] { message });

            return result;
        }

        protected object CreateHandler(Type commandType)
        {
            var handler = handlerMap[commandType];

            if (handler == null)
            {
                throw new HandlerNotFoundException(commandType);
            }

            var paramList = new List<object>();

            var parameters = handler
                .GetConstructors()
                .First() //propably not a good idea
                .GetParameters();

            foreach (var param in parameters)
            {
                paramList.Add(resolver.Resolve(param.ParameterType));
            }

            return Activator.CreateInstance(handler, paramList.ToArray());
        }
    }
}
