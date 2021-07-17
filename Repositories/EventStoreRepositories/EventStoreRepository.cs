using Domain.Entities.EventStoreEntities;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using EventHandlers.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repositories.DbContexts;
using System;
using System.Collections.Generic;
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
        /// Mark the aggregate to be persisted in the store and cache.
        /// </summary>
        /// <param name="aggregate">Aggregate to save.</param>
        /// <returns></returns>
        public async Task Save(Aggregate aggregate)
        {
            EventStoreDbContext.Aggregates.Add(aggregate);

            SaveToCache(aggregate);
        }

        /// <summary>
        /// Save a list of events into BDD and cache.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public async Task Save(Aggregate aggregate, IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                aggregate.Events.Add(@event);
                EventStoreDbContext.Events.Add(@event);
            }

            SaveToCache(aggregate);
        }

        /// <summary>
        /// Persist changes into DB.
        /// </summary>
        /// <returns></returns>
        public Task SaveChanges()
        {
            return EventStoreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a aggregate as a domain instance from cache or DB.
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
        /// Get the Db Aggregate if existing, else return null.
        /// </summary>
        /// <param name="id">Aggregate ID.</param>
        /// <returns></returns>
        public async Task<Aggregate> Get(Guid id)
        {
            var fromCache = Cache.Get<Aggregate>(id);

            if(fromCache != null)
            {
                return fromCache;
            }

            var fromDb = await GetFromDataBase(id);

            if(fromDb != null)
            {
                SaveToCache(fromDb);
            }

            return fromDb;
        }

        private void SaveToCache(Aggregate aggregate)
        {
            Cache.Set(aggregate.Id, aggregate, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(DEFAULT_CACHE_DURATION)
            });
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
                .SingleOrDefaultAsync(es => es.Id == id);
        }
    }
}
