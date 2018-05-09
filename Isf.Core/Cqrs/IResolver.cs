using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isf.Core.Cqrs
{
    public interface IResolver
    {
        void Register<TAbstract, TConcrete>();
        object Resolve(Type type);
    }

    public class NaiveResolver : IResolver
    {
        public void Register<TAbstract, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
            //return InMemoryCommandBus.Current;
        }
    }
}
