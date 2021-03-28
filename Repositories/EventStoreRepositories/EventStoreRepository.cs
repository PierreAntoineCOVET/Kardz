using Domain.Entities.EventStoreEntities;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using EventHandlers.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repositories.DbContexts;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.EventStoreRepositories
{
    /// <summary>
    /// Event sourcing repository.
    /// </summary>
    public class EventStoreRepository : IEventStoreRepository
    {
        /// <summary>
        /// Default cache duration in minutes.
        /// </summary>
        private const int DEFAULT_CACHE_DURATION = 5;

        /// <summary>
        /// Db context.
        /// </summary>
        private EventStoreDbContext EventStoreDbContext;

        /// <summary>
        /// Json serializer.
        /// </summary>
        private JsonSerializer JsonSerializer;

        /// <summary>
        /// Memory cache.
        /// </summary>
        private IMemoryCache Cache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreDbContext">Db context to use.</param>
        /// <param name="jsonSerializer">Json serializer.</param>
        public EventStoreRepository(EventStoreDbContext eventStoreDbContext, JsonSerializer jsonSerializer, IMemoryCache cache)
        {
            EventStoreDbContext = eventStoreDbContext;
            JsonSerializer = jsonSerializer;
            Cache = cache;
        }

        /// <summary>
        /// Save the aggregate and all its events into the store.
        /// </summary>
        /// <param name="aggregate">Aggregate to save.</param>
        /// <returns></returns>
        public async Task Save(Aggregate aggregate)
        {
            if(!await EventStoreDbContext.Aggregates.AnyAsync(a => a.Id == aggregate.Id))
            {
                EventStoreDbContext.Aggregates.Add(aggregate);
            }

            foreach (var @event in aggregate.Events)
            {
                EventStoreDbContext.Events.Add(@event);
            }

            Cache.Set(aggregate.Id, aggregate, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(DEFAULT_CACHE_DURATION)
            });

            await EventStoreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a aggregate from cache or DB.
        /// </summary>
        /// <typeparam name="T">Type of the object to get.</typeparam>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        public async Task<T> Get<T>(Guid id) where T : IAggregate
        {
            var aggregate = await Cache.GetOrCreateAsync(
                id,
                (entry) =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(DEFAULT_CACHE_DURATION);
                    return GetFromDataBase(id);
                });

            var domainAssembly = Assembly.Load("Domain");
            var aggregateInstance = (T)Activator.CreateInstance(domainAssembly.GetType(aggregate.Type));

            foreach (var @event in aggregate.Events.OrderBy(e => e.Date))
            {
                var eventInstance = JsonSerializer.Deserialize(aggregate.Type, @event);
                aggregateInstance.Apply((dynamic)eventInstance);
            }

            return aggregateInstance;
        }

        /// <summary>
        /// Get a aggregate from the DB.
        /// </summary>
        /// <typeparam name="T">Type of the object to get.</typeparam>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        /// <remarks>Load all its events.</remarks>
        private Task<Aggregate> GetFromDataBase(Guid id)
        {
            return EventStoreDbContext.Aggregates
                .Include(es => es.Events)
                .SingleAsync(es => es.Id == id);
        }
    }
}
