using Microsoft.EntityFrameworkCore;
using Repositories.DbContexts;
using Repositories.EventStoreEntities;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Add a new event.
        /// </summary>
        /// <param name="event">Event to add.</param>
        /// <returns></returns>
        public async Task AddEvent(Event @event)
        {
            await EventStoreDbContext.Events.AddAsync(@event);
        }

        /// <summary>
        /// Add an aggregate.
        /// </summary>
        /// <param name="eventSource"></param>
        /// <returns></returns>
        public async Task AddAggregate(Aggregate eventSource)
        {
            await EventStoreDbContext.Aggregates.AddAsync(eventSource);
        }

        /// <summary>
        /// Get a aggregate from the DB.
        /// </summary>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        /// <remarks>Load all its events.</remarks>
        public Task<Aggregate> GetAggregate(Guid id)
        {
            return EventStoreDbContext.Aggregates
                .Include(es => es.Events)
                .SingleAsync(es => es.Id == id);
        }

        /// <summary>
        /// Savechange (unit of work).
        /// </summary>
        /// <returns></returns>
        public async Task SaveChanges()
        {
            await EventStoreDbContext.SaveChangesAsync();
        }
    }
}
