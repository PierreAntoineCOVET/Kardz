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
    public class GetPlayerCardsQueryHandler : IRequestHandler<GetPlayerCardsQuery, IEnumerable<int>>
    {
        private readonly IGenericRepository GenericRepository;

        public GetPlayerCardsQueryHandler(IGenericRepository gameRepository)
        {
            GenericRepository = gameRepository;
        }

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
