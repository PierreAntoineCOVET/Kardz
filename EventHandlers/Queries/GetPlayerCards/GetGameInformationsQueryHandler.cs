using Domain.Exceptions;
using DTOs.Shared;
using EventHandlers.Specifications;
using MediatR;
using Repositories.ReadEntities;
using Repositories.ReadRepositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayerCards
{
    /// <summary>
    /// Handler for <see cref="GetGameInformationsQuery"/>.
    /// </summary>
    public class GetGameInformationsQueryHandler : IRequestHandler<GetGameInformationsQuery, GameInitDto>
    {
        /// <summary>
        /// Read model generic repository.
        /// </summary>
        private readonly IGenericRepository GenericRepository;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameRepository"></param>
        public GetGameInformationsQueryHandler(IGenericRepository gameRepository)
        {
            GenericRepository = gameRepository;
        }

        /// <summary>
        /// Get the cards for the given player and game.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of cards.</returns>
        public async Task<GameInitDto> Handle(GetGameInformationsQuery request, CancellationToken cancellationToken)
        {
            //var game = await GenericRepository.Query<CoincheGame>()
            //    .Include(g => g.Teams)
            //    .ThenInclude(t => t.Players)
            //    .SingleOrDefaultAsync(g => g.Id == request.GameId);
            var games = await GenericRepository.Query(new CoincheGameDatasSpecification(request.GameId));
            var game = games.SingleOrDefault();

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            var player = game.Teams.SelectMany(t => t.Players).SingleOrDefault(p => p.Id == request.PlayerId);

            if(player == null)
            {
                throw new GameException($"Player id {request.PlayerId} doesn't exist in game {request.GameId}.");
            }

            var cards = player.Cards.Split(';').Select(c => int.Parse(c));

            return new GameInitDto
            {
                PlayerCards = cards,
                Dealer = game.CurrentDealer,
                PlayerPlaying = game.CurrentPayerTurn,
                PlayerNumber = player.Number
            };
        }
    }
}
