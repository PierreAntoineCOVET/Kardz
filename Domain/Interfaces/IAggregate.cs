using Domain.Events;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Contrat de base d'un aggregate.
    /// </summary>
    public interface IAggregate
    {
        /// <summary>
        /// Id de l'aggregate.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Version actuel de l'aggregate.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Liste d'evenements non commités en BDD.
        /// </summary>
        ICollection<IDomainEvent> UncommittedEvents { get; }

        /// <summary>
        /// Application des evenements.
        /// </summary>
        /// <param name=""></param>
        void Apply(IDomainEvent @event);
    }
}
