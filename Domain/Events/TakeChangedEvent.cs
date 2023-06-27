using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class TakeChangedEvent : IDomainEvent, INotification
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public int AggregateVersion { get; set; }

        public IEnumerable<ICards> CurrentFold { get; set; }

        public IEnumerable<ICards> PreviousFold { get; set; }

        public IEnumerable<ICards> CurrentPlayerPlayableCards { get; set; }

        public IPlayer CurrentPlayer { get; set; }
    }
}
