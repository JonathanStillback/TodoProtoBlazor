using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Proto;
using Proto.DependencyInjection;
using Proto.Extensions;

namespace Configurations
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
            services.AddSingleton<IDBProvider<Todo>, SqliteDBProvider<Todo>>();
            services.AddSingleton(serviceProvider => new ActorSystem().WithServiceProvider(serviceProvider));
            services.AddSingleton<IProtoClient, ProtoClient>();
            services.AddScoped<INotifierStateService<Todo>, NotifierStateService<Todo>>();

            services.AddTransient<DispatcherSendActor>();
            services.AddTransient<DispatcherRequestActor>();
            services.AddTransient<TodoActor>();

            services.AddControllersWithViews();
            services.AddServerSideBlazor();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapControllerRoute("Catch all", "{*url}", new {Controller = "Home", Action = "Index"});
                // endpoints.MapFallbackToPage("/");
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
