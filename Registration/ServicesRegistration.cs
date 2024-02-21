using Domain.Factories;
using Domain.Services;
using Domain.Tools.Serialization;
using EventHandlers.Behavior;
using EventHandlers.Repositories;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.DbContexts;
using Repositories.EventStoreRepositories;
using Repositories.ReadRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Registration
{
    /// <summary>
    /// Extention for Kards projet DI registration.
    /// </summary>
    public static class ServicesRegistration
    {
        /// <summary>
        /// Add domain services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddKardzDomain(this IServiceCollection services)
        {
            services.AddSingleton<LobbiesService>();

            services.AddSingleton<LobbyFactory>();
            services.AddSingleton<GameFactory>();
            services.AddSingleton<PlayerFactory>();
        }

        /// <summary>
        /// Add repositories
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddKardzRopositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEventStoreRepository), typeof(EventStoreRepository));
            services.AddScoped(typeof(IGenericRepository), typeof(GenericRepository));
        }

        /// <summary>
        /// Add EF DbContext.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration">Ref to AppSetting JSon file.</param>
        public static void AddKardzDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventStoreDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("EventStoreDbSqLite"));
                //options.UseSqlServer(configuration.GetConnectionString("EventStoreDb"));
            });

            services.AddDbContext<ReadDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("ReadDbSqLite"));
                //options.UseSqlServer(configuration.GetConnectionString("ReadDb"));
            });
        }

        public static void AddSerializationMappers(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(InterfaceResolverAttribute));
            var interfaceResolverAttributes = assembly.GetTypes()
                .SelectMany(t => t.GetCustomAttributes<InterfaceResolverAttribute>());

            foreach (InterfaceResolverAttribute interfaceResolverAttribute in interfaceResolverAttributes)
            {
                JsonSerializer.Register(interfaceResolverAttribute.Aggregate, interfaceResolverAttribute.Event, interfaceResolverAttribute.Converter);
            }

            services.AddTransient<JsonSerializer>();
        }

        public static void AddMediatR(this IServiceCollection services)
        {

            var eventHandlerAssembly = Assembly.Load("EventHandlers");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(eventHandlerAssembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(EventHandlers.Behavior.ValidationBehavior<,>))
                .AddFluentValidation(new List<Assembly> { eventHandlerAssembly });
        }
    }
}
