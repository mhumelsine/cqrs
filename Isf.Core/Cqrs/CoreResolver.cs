using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class CoreResolver : IResolver
    {
        private readonly IServiceProvider provider;

        public CoreResolver(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public void Register<TAbstract, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public T GetMe<T>()
        {
            return (T)provider.GetService(typeof(T));
        }

        public object GetMe(Type type)
        {
            return provider.GetService(type);
        }
    }
}
