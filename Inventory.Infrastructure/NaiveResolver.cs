//using Isf.Core.Common;
//using Isf.Core.Cqrs;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Inventory.Infrastructure
//{
//    public class NaiveResolver : IResolver
//    {

//        private ICommandBus cBus;
//        private IQueryBus qBus;
//        private EventDbContext dbContext;
//        private DomainDbContext domainDbContext;
//        private IEventBus eventBus;
//        private IEventStore eventStore;
//        private IDomainStore domainStore;
//        private IUsernameProvider usernameProvider;

//        public NaiveResolver(EventDbContext eventDbContext, DomainDbContext domainDbContext)
//        {
//            cBus = new InMemoryCommandBus();
//            qBus = new InMemoryQueryBus();

//            //var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();            
//            //optionsBuilder.UseInMemoryDatabase("EventsDB");

//            //var optionsBuilderD = new DbContextOptionsBuilder<DomainDbContext>();
//            //optionsBuilderD.UseInMemoryDatabase("DomainDB");

//            //dbContext = new EventDbContext(optionsBuilder.Options);
//            //domainDbContext = new DomainDbContext(optionsBuilderD.Options);

//            eventBus = new InMemoryEventBus();
//            eventStore = new EfEventStore(eventDbContext);
//            domainStore = new EfDomainStore(domainDbContext, eventStore, eventBus);
//            usernameProvider = new StaticUsernameProvider();
//        }

//        public void Register<TAbstract, TConcrete>()
//        {
//            throw new NotImplementedException();
//        }

//        public T Resolve<T>()
//        {
//            var type = typeof(T);

//            if (type == typeof(ICommandBus))
//            {
//                return (T)cBus;
//            }

//            if (type == typeof(IQueryBus))
//            {
//                return (T)qBus;
//            }

//            if (type == typeof(IEventStore))
//            {
//                return (T)eventStore;
//            }

//            if (type == typeof(IDomainStore))
//            {
//                return (T)domainStore;
//            }

//            if (type == typeof(IEventBus))
//            {
//                return (T)eventBus;
//            }

//            if (type == typeof(IUsernameProvider))
//            {
//                return (T)usernameProvider;
//            }

//            if (type == typeof(DomainDbContext))
//            {
//                return (T)(object)domainDbContext;
//            }

//            throw new InvalidOperationException();
//        }

//        public object Resolve(Type type)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
