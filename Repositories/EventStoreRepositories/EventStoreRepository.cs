using Domain.Entities.EventStoreEntities;
using EventHandlers.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repositories.DbContexts;
using System;
using System.Collections.Generic;
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
        /// Memory cache.
        /// </summary>
        private IMemoryCache Cache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreDbContext">Db context to use.</param>
        /// <param name="jsonSerializer">Json serializer.</param>
        public EventStoreRepository(EventStoreDbContext eventStoreDbContext, IMemoryCache cache)
        {
            EventStoreDbContext = eventStoreDbContext;
            Cache = cache;
        }

        /// <summary>
        /// Mark the aggregate to be persisted in the store and cache.
        /// </summary>
        /// <param name="aggregate">Aggregate to save.</param>
        /// <returns></returns>
        public Task Save(Aggregate aggregate)
        {
            EventStoreDbContext.Aggregates.Add(aggregate);

            SaveToCache(aggregate);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Save a list of events into BDD and cache.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public Task Save(Aggregate aggregate, IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                aggregate.Events.Add(@event);
                EventStoreDbContext.Events.Add(@event);
            }

            SaveToCache(aggregate);

            return Task.CompletedTask;
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
        /// Get the Db Aggregate if existing, else return null.
        /// </summary>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        public Task<Aggregate> Get(Guid id)
        {
            return Cache.GetOrCreateAsync(
                id,
                (entry) =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(DEFAULT_CACHE_DURATION);
                    return GetFromDataBase(id);
                });
        }

        /// <summary>
        /// Save aggregate to cache form <see cref="DEFAULT_CACHE_DURATION"/> minutes.
        /// </summary>
        /// <param name="aggregate"></param>
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
