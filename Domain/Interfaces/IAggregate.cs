using Domain.Events;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IAggregate
    {
        Guid Id { get; }

        int Version { get; }

        ICollection<IDomainEvent> UncommittedEvents { get; }
    }
}
