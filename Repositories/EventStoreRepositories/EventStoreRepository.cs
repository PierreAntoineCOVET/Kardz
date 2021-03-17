﻿using Domain.Entities.EventStoreEntities;
using Domain.Events;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using EventHandlers.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.DbContexts;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories.EventStoreRepositories
{
    /// <summary>
    /// Event sourcing repository.
    /// </summary>
    public class EventStoreRepository : IEventStoreRepository
    {
        /// <summary>
        /// Db context.
        /// </summary>
        private EventStoreDbContext EventStoreDbContext;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventStoreDbContext">Db context to use.</param>
        public EventStoreRepository(EventStoreDbContext eventStoreDbContext)
        {
            EventStoreDbContext = eventStoreDbContext;
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

            await EventStoreDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a aggregate from the DB.
        /// </summary>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        /// <remarks>Load all its events.</remarks>
        public async Task<T> Get<T>(Guid id) where T : IAggregate
        {
            var aggregate = await EventStoreDbContext.Aggregates
                .Include(es => es.Events)
                .SingleAsync(es => es.Id == id);

            var domainAssembly = Assembly.Load("Domain");
            var aggregateInstance = (T)Activator.CreateInstance(domainAssembly.GetType(aggregate.Type));

            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new TeamMappingConverter(),
                    new PlayerMappingConverter()
                }
            };

            foreach (var @event in aggregate.Events.OrderBy(e => e.Date))
            {
                var eventInstance = JsonSerializer.Deserialize(@event.Datas, domainAssembly.GetType(@event.Type), options);
                aggregateInstance.Apply((dynamic)eventInstance);
            }

            return aggregateInstance;
        }
    }
}
