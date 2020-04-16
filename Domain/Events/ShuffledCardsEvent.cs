using Domain.Domain.Interfaces;
using Domain.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class ShuffledCardsEvent : IDomainEvent, INotification
    {
        public Guid Id { get; set; }

        public IEnumerable<CardsEnum> ShuffledCards { get; set; }

        public int Version { get; set; }
    }
}
