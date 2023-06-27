using EventHandlers.Queries.GetNomberOfPlayersInLobby;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayableCards
{
    public class GetPlayableCardsQueryHandler : IRequestHandler<GetPlayableCardsQuery, IEnumerable<int>>
    {
        public async Task<IEnumerable<int>> Handle(GetPlayableCardsQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(500);

            return new List<int>();
        }
    }
}
