using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.GamesLogic
{
    /// <summary>
    /// Base class for all aggragates.
    /// </summary>
    internal class AggregateBase
    {
        /// <summary>
        /// Aggregate id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Aggregate current version.
        /// </summary>
        public int Version { get; set; } = -1;

        /// <summary>
        /// List of uncommited events.
        /// </summary>
        /// <returns></returns>
        public ICollection<IDomainEvent> UncommittedEvents { get; } = new List<IDomainEvent>();

        /// <summary>
        /// Apply given domain event.
        /// Use of dynamic cast to call the function Apply(TrueDomainEventType).
        /// </summary>
        /// <param name="event"></param>
        public void Apply(IDomainEvent @event)
        {
            ((dynamic)this).Apply((dynamic)@event);
        }

        /// <summary>
        /// Apply a domain event en register it inside _UncommittedEvents.
        /// </summary>
        /// <param name="event"></param>
        protected virtual void RaiseEvent(IDomainEvent @event)
        {
            if (!UncommittedEvents.Any(e => e.Id == @event.Id))
            {
                Apply(@event);

                @event.AggregateVersion = ++Version;

                UncommittedEvents.Add(@event);
            }
        }
    }
}
