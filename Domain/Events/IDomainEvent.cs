using System;

namespace Domain.Events
{
    /// <summary>
    /// Contract for domain events link to any aggregates.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Id of the event.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Version of the aggregate after the event was applyed.
        /// </summary>
        int AggregateVersion { get; set; }
    }
}
