using System.Collections.Generic;
using System.Reflection;
using EventHandlers.Behavior;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Web.Hubs;
using Microsoft.Extensions.Configuration;
using Registration;

namespace Web
{
    public class Startup
    {
        private const string CORS_AUTHORISE_LOCALHOST = "LocalHosts";

        private IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var eventHandlerAssembly = Assembly.Load("EventHandlers");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(eventHandlerAssembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(EventHandlers.Behavior.ValidationBehavior<,>))
                .AddFluentValidation(new List<Assembly> { eventHandlerAssembly });

            // cross site request from localhost:4200 to localhost:xxxx.
            services.AddCors(option =>
            {
                option.AddPolicy(
                    CORS_AUTHORISE_LOCALHOST,
                    builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddMemoryCache();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";

            });

            services.AddSignalR();

            //services.AddSignalR(options => options.EnableDetailedErrors = true);

            //        services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            //// Traduction des dates envoyés par Angular HttpClient en UTC vers local
            //.AddJsonOptions(options => options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local);

            services.AddCoincheConfiguration(Configuration);

            services.AddKardzDomain();

            services.AddKardzDbContexts(Configuration);

            services.AddKardzRopositories();

            services.AddSerializationMappers();

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CORS_AUTHORISE_LOCALHOST);

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Kardz");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LobbyHub>("/lobby");
                endpoints.MapHub<GameHub>("/game");
            });

            if (env.IsProduction())
            {
                app.UseSpaStaticFiles();
                app.UseSpa(spa => spa.Options.SourcePath = "ClientApp");
            }
        }
    }
}
