using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Commands;
using Isf.Core.Cqrs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //need the assembly to be loaded, need a better way
            var command = new CreateInventoryItemCommand();

            var resolver = new NaiveResolver();
            var runtime = new Runtime(resolver, "Inventory");
            var bus = new InMemoryCommandBus(runtime);

            runtime.Start();

            services.AddSingleton<IResolver>(resolver);

            services.AddSingleton(runtime);

            services.AddSingleton<IEventBus>(bus);
            services.AddSingleton<ICommandBus>(bus);
            services.AddSingleton<IQueryBus>(bus);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
