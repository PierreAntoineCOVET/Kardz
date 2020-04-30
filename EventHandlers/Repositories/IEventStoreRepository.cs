using Repositories.EventStoreEntities;
using System;
using System.Threading.Tasks;

namespace Repositories.EventStoreRepositories
{
    public interface IEventStoreRepository
    {
        Task Save(Aggregate aggregate);

        Task<Aggregate> Get(Guid id);
    }
}
