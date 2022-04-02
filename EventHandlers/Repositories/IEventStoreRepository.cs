using Domain.Entities.EventStoreEntities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHandlers.Repositories
{
    public interface IEventStoreRepository
    {
        /// <summary>
        /// Save the aggregate and all its events into the store.
        /// </summary>
        /// <param name="aggregate">Aggregate to save.</param>
        /// <returns></returns>
        Task Save(Aggregate aggregate);

        /// <summary>
        /// Save a list of events into BDD and cache.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        Task Save(Aggregate aggregate, IEnumerable<Event> events);


        /// <summary>
        /// Get the Db Aggregate if existing, else return null.
        /// </summary>
        /// <param name="id">Aggregate id.</param>
        /// <returns></returns>
        Task<Aggregate> Get(Guid id);

        /// <summary>
        /// Persist changes into DB.
        /// </summary>
        /// <returns></returns>
        Task SaveChanges();
    }
}
