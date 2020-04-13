using Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public interface IAggregate
    {
        Guid Id { get; }

        int Version { get; }

        IEnumerable<IDomainEvent> GetUncommittedEvents();
    }
}
