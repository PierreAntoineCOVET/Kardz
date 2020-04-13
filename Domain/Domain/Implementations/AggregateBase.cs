using Domain.Domain.Interfaces;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations
{
    internal class AggregateBase
    {
        public int Version { get; set; } = -1;

        private List<IDomainEvent> UncommittedEvents = new List<IDomainEvent>();

        public IEnumerable<IDomainEvent> GetUncommittedEvents() => UncommittedEvents;

        protected void RaiseEvent(IDomainEvent @event)
        {
            if (!UncommittedEvents.Any(e => e.Id == @event.Id))
            {
                @event.Version = Version + 1;
                ((dynamic)this).Apply((dynamic)@event);

                UncommittedEvents.Add(@event);

                Version = @event.Version;
            }
        }
    }
}
