using Domain.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public class GameCreatedEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public IEnumerable<IPlayer> Players { get; set; }

        public int Version { get; set; }
    }
}
