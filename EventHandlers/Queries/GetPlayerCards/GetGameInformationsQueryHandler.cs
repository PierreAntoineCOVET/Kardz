using Domain.Configuration;
using Domain.Exceptions;
using DTOs.Shared;
using EventHandlers.Repositories;
using EventHandlers.Specifications;
using MediatR;
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
        /// Coinche game configuration.
        /// </summary>
        private readonly CoincheConfiguration Configuration;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameRepository"></param>
        public GetGameInformationsQueryHandler(IGenericRepository gameRepository, CoincheConfiguration configuration)
        {
            GenericRepository = gameRepository;
            Configuration = configuration;
        }

        /// <summary>
        /// Get all the game informations relative to the player.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of cards.</returns>
        /// TODO: delete and send all infos on search game command
        public async Task<GameInitDto> Handle(GetGameInformationsQuery request, CancellationToken cancellationToken)
        {
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

            var timerEndDate = game.TurnTimerBase.AddSeconds(Configuration.TimerLengthInSecond + Configuration.NetworkOffsetInSecond);

            return new GameInitDto
            {
                PlayerCards = cards,
                Dealer = game.CurrentDealer,
                PlayerPlaying = game.CurrentPayerTurn,
                PlayerNumber = player.Number,
                TurnEndTime = timerEndDate
            };
        }
    }
}
