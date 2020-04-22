using Domain.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;

namespace Domain.Events
{
    /// <summary>
    /// Game created event.
    /// </summary>
    public class GameCreatedEvent : IDomainEvent, INotification
    {
        /// <summary>
        /// Event's id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Game's id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Game's teams.
        /// </summary>
        public IEnumerable<ITeam> Teams { get; set; }

        /// <summary>
        /// Player number for the dealer.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Player who needs to play.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }
    }
}
