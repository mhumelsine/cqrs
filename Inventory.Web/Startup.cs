using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Infrastructure;
using Inventory.Inventory;
using Isf.Core.Common;
using Isf.Core.Cqrs;
using Isf.Core.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            var command = new CreateInventoryMasterCommand();

            services.AddDbContext<EventDbContext>(options =>
                options.UseSqlite("Data Source = events.db"));
            //options.UseSqlServer(Configuration.GetConnectionString("EventContext")));

            services.AddDbContext<DomainDbContext>(options =>
            options.UseSqlite("Data Source = domain.db"));
            //options.UseSqlServer(Configuration.GetConnectionString("DomainContext")));          

            services.AddScoped<IEventStore, EfEventStore>();
            services.AddScoped<IDomainStore, DomainStore>();
            services.AddScoped<IUsernameProvider, StaticUsernameProvider>();

            //add the singleton services
            services.AddCqrs();
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

            //start the CQRS runtime
            app.UseCqrs("Inventory");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Inventory}/{action=Index}/{id?}");
            });
        }
    }
}
