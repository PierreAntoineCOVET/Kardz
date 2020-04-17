using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.ReadEntities;
using Repositories.ReadRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayerCards
{
    /// <summary>
    /// Handler for <see cref="GetPlayerCardsQuery"/>.
    /// </summary>
    public class GetPlayerCardsQueryHandler : IRequestHandler<GetPlayerCardsQuery, IEnumerable<int>>
    {
        /// <summary>
        /// Read model generic repository.
        /// </summary>
        private readonly IGenericRepository GenericRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameRepository"></param>
        public GetPlayerCardsQueryHandler(IGenericRepository gameRepository)
        {
            GenericRepository = gameRepository;
        }

        /// <summary>
        /// Get the cards for the given player and game.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of cards.</returns>
        public async Task<IEnumerable<int>> Handle(GetPlayerCardsQuery request, CancellationToken cancellationToken)
        {
            var game = await GenericRepository.Query<CoincheGame>()
                .Include(g => g.Teams)
                .ThenInclude(t => t.Players)
                .SingleOrDefaultAsync(g => g.Id == request.GameId);

            if(game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            var player = game.Teams.SelectMany(t => t.Players).SingleOrDefault(p => p.Id == request.PlayerId);

            if(player == null)
            {
                throw new GameException($"Player id {request.PlayerId} doesn't exist in game {request.GameId}.");
            }

            return player.Cards.Split(';').Select(c => int.Parse(c));
        }
    }
}
