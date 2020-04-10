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
    /// <summary>
    /// Handler for <see cref="ShuffleCardsCommand"/>.
    /// </summary>
    class ShuffleCardsCommandHandler : IRequestHandler<ShuffleCardsCommand, IEnumerable<int>>
    {
        /// <summary>
        /// Game service.
        /// </summary>
        private GamesServices GamesServices;

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="gamesServices">Game service.</param>
        public ShuffleCardsCommandHandler(GamesServices gamesServices)
        {
            GamesServices = gamesServices;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of shuffled cards.</returns>
        public async Task<IEnumerable<int>> Handle(ShuffleCardsCommand request, CancellationToken cancellationToken)
        {
            var game = await GamesServices.GetGame(request.GameId);
            var playerCards = await game.GetCardsForPlayer(request.PlayerId);

            return playerCards.Select(card => (int) card);
        }
    }
}
