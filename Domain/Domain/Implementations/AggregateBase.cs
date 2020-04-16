using Domain.Domain.Interfaces;
using Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations
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
        private List<IDomainEvent> _UncommittedEvents = new List<IDomainEvent>();

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public ICollection<IDomainEvent> UncommittedEvents { get => _UncommittedEvents; }

        protected void RaiseEvent(IDomainEvent @event)
        {
            if (!_UncommittedEvents.Any(e => e.Id == @event.Id))
            {
                @event.Version = Version + 1;
                ((dynamic)this).Apply((dynamic)@event);

                _UncommittedEvents.Add(@event);

                Version = @event.Version;
            }
        }
    }
}
