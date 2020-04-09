using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.ShuffleCards
{
    public class ShuffleCardsCommand : IRequest<IEnumerable<int>>
    {
        public Guid GameId { get; set; }

        public Guid PlayerId { get; set; }
    }
}
