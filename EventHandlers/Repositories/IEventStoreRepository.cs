using Domain.Entities.EventStoreEntities;
using System;
using System.Threading.Tasks;

namespace EventHandlers.Repositories
{
    public interface IEventStoreRepository
    {
        Task Save(Aggregate aggregate);

        Task<Aggregate> Get(Guid id);
    }
}
