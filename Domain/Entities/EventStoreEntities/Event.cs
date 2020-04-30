using System;

namespace Domain.Entities.EventStoreEntities
{
    /// <summary>
    /// Event store events.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Aggregate id.
        /// </summary>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Event type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Event creation date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Event author.
        /// </summary>
        public Guid? Author { get; set; }

        /// <summary>
        /// Event datas serialised to JSON.
        /// </summary>
        public string Datas { get; set; }

        /// <summary>
        /// Aggregate.
        /// </summary>
        public Aggregate Aggregate { get; set; }
    }
}
