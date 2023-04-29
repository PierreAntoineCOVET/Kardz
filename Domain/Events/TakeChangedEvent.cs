using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class TakeChangedEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public int AggregateVersion { get; set; }

        public IEnumerable<ICards> CurrentFold { get; set; }

        public IEnumerable<ICards> PreviousFold { get; set; }

        public IEnumerable<ICards> NextPlayerAvailableCards { get; set; }
    }
}
