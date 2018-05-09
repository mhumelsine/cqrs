using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isf.Core.Cqrs
{
    public interface IResolver
    {
        void Register<TAbstract, TConcrete>();
        T Resolve<T>();
        object Resolve(Type type);
    }

    public class NaiveResolver : IResolver
    {

        private ICommandBus cBus = new InMemoryCommandBus();
        private IQueryBus qBus = new InMemoryQueryBus();
        public void Register<TAbstract, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            var type = typeof(T);

            if(type == typeof(ICommandBus))
            {
                return (T)cBus;
            }

            if(type == typeof(IQueryBus))
            {
                return (T)qBus;
            }


            throw new InvalidOperationException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
