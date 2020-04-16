using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetPlayerCards
{
    public class GetPlayerCardsQuery : IRequest
    {
        public Guid GameId { get; set; }

        public Guid PlayerId { get; set; }
    }
}
