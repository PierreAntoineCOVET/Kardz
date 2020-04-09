using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.ShuffleCards
{
    class ShuffleCardsCommandHandler : IRequestHandler<ShuffleCardsCommand, IEnumerable<int>>
    {
        private GamesServices GamesServices;

        public ShuffleCardsCommandHandler(GamesServices gamesServices)
        {
            GamesServices = gamesServices;
        }

        public async Task<IEnumerable<int>> Handle(ShuffleCardsCommand request, CancellationToken cancellationToken)
        {
            var game = await GamesServices.GetGame(request.GameId);
            var playerCards = await game.GetCardsForPlayer(request.PlayerId);

            return playerCards.Select(card => (int) card);
        }
    }
}
