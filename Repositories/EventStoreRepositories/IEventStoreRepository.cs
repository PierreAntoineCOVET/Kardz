using Repositories.EventStoreEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EventStoreRepositories
{
    public interface IEventStoreRepository
    {
        Task AddEvent(Event @event);

        Task AddAggregate(Aggregate eventSource);

        Task<Aggregate> GetAggregate(Guid id);

        Task SaveChanges();
    }
}
