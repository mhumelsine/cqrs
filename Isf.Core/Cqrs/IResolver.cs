using Microsoft.EntityFrameworkCore;
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

        private ICommandBus cBus;
        private IQueryBus qBus;
        private DbContext dbContext;
        private IEventBus eventBus;
        private IEventStore eventStore;
        private IDomainStore domainStore;

        public NaiveResolver()
        {
            cBus = new InMemoryCommandBus();
            qBus = new InMemoryQueryBus();

            var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();
            optionsBuilder.UseSqlite("Data Source=events.db");

            dbContext = new EventDbContext(optionsBuilder.Options);
            eventStore = new EfEventStore(dbContext);
            //domainStore = new EfDomainStore(dbContext, eventStore, eventBus);
        }

        public void Register<TAbstract, TConcrete>()
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            var type = typeof(T);

            if (type == typeof(ICommandBus))
            {
                return (T)cBus;
            }

            if (type == typeof(IQueryBus))
            {
                return (T)qBus;
            }

            if (type == typeof(IEventStore))
            {
                return (T)eventStore;
            }

            if (type == typeof(IDomainStore))
            {
                return (T)domainStore;
            }

            if(type == typeof(IEventBus))
            {
                return (T)eventBus;
            }

            throw new InvalidOperationException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
