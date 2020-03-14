using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.ShuffleCards
{
    class ShuffleCardsCommandHandler : IRequestHandler<ShuffleCardsCommand, IEnumerable<CardsEnum>>
    {
        public Task<IEnumerable<CardsEnum>> Handle(ShuffleCardsCommand request, CancellationToken cancellationToken)
        {
            var game = GamesServices.GetGame(request.GameId);

            return Task.FromResult(game.GetCardsForPlayer(request.PlayerId));
        }
    }
}
