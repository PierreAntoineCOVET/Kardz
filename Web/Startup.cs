using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventHandlers.Behavior;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Web.Hubs;

namespace Web
{
    public class Startup
    {
        private const string CORS_AUTHORISE_LOCALHOST = "LocalHosts";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var eventHandlerAssembly = Assembly.Load("EventHandlers");
            services.AddMediatR(eventHandlerAssembly)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(EventHandlers.Behavior.ValidationBehavior<,>))
                .AddFluentValidation(new List<Assembly> { eventHandlerAssembly });

            services.AddCors(option =>
            {
                option.AddPolicy(
                    CORS_AUTHORISE_LOCALHOST,
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";

            });

            services.AddSignalR();

                //        services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //// Traduction des dates envoyés par Angular HttpClient en UTC vers local
                //.AddJsonOptions(options => options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net();

            app.UseCors(CORS_AUTHORISE_LOCALHOST);

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LobbyHub>("/lobby");
            });

            if (env.IsProduction())
            {
                app.UseSpaStaticFiles();
                app.UseSpa(spa => spa.Options.SourcePath = "ClientApp");
            }
        }
    }
}
