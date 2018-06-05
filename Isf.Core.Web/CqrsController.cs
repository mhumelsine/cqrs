using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Isf.Core.Web
{
    public class CqrsController : Controller
    {
        protected readonly ICommandBus commandBus;
        protected readonly IQueryBus queryBus;

        public CqrsController()
        {
            commandBus = CqrsRuntime.Current.Resolver.GetMe<ICommandBus>();
            queryBus = CqrsRuntime.Current.Resolver.GetMe<IQueryBus>();
        }        
    }
}
