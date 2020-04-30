using System;
using System.Collections.Generic;

namespace Domain.Entities.EventStoreEntities
{
    /// <summary>
    /// Aggreate of events.
    /// </summary>
    public class Aggregate
    {
        /// <summary>
        /// Aggregate ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Aggregate concerte type (full name).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Aggregate version (last event).
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// List of events.
        /// </summary>
        public ICollection<Event> Events { get; set; }
    }
}
