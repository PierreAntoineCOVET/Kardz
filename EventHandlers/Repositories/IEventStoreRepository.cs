using Domain.Entities.EventStoreEntities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace EventHandlers.Repositories
{
    public interface IEventStoreRepository
    {
        Task Save(Aggregate aggregate);

        Task<T> Get<T>(Guid id) where T : IAggregate;
    }
}
