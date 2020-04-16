using Domain.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public class GameCreatedEvent : IDomainEvent, INotification
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public IEnumerable<ITeam> Teams { get; set; }

        public int Version { get; set; }
    }
}
