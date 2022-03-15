using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class ContractFailedEvent : IDomainEvent, INotification
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Dictionnary of all the cards attributed to each players.
        /// </summary>
        public IDictionary<Guid, IEnumerable<CardsEnum>> CardsDistribution { get; set; }

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
