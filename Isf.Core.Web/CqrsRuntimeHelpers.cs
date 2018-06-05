using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Web
{
    //setups default configurations for a single-server environment
    public static class CqrsRuntimeHelpers
    {
        public static void AddCqrs(this IServiceCollection services)
        {
            var qb = new InMemoryQueryBus();
            var cb = new InMemoryCommandBus();

            services.AddSingleton<ICommandBus, InMemoryCommandBus>(x => cb);
            services.AddSingleton<IQueryBus, InMemoryQueryBus>(x => qb);
            services.AddSingleton<IEventBus, InMemoryEventBus>();
        }

        public static void UseCqrs(this IApplicationBuilder app, params string[] assembliesToScan)
        {
            CqrsRuntime.Start(new CoreResolver(app.ApplicationServices), assembliesToScan);

            //need to set the current resolver on each request for scoped dependencies
            app.Use(async (context, successor) =>
            {
                CqrsRuntime.Current.SetResolver(new CoreResolver(context.RequestServices));

                await successor.Invoke();
            });
        }
    }
}
