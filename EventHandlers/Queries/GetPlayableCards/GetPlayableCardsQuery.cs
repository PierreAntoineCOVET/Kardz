using DTOs.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayableCards
{
    public class GetPlayableCardsQuery: IRequest<IEnumerable<int>>
    {
        public Guid PlayerId { get; set; }

        public Guid GameId { get; set; }

        public IEnumerable<CardsEnum> PossibleCards { get; set; }
    }
}
