using Domain.Configuration;
using Domain.Exceptions;
using Domain.Factories;
using Domain.Interfaces;
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
        /// DB event store repository.
        /// </summary>
        private readonly IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Construct game object from DB agregate.
        /// </summary>
        private readonly GameFactory GameFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameRepository"></param>
        public GetGameInformationsQueryHandler(IEventStoreRepository eventStoreRepository, GameFactory gameFactory)
        {
            EventStoreRepository = eventStoreRepository;
            GameFactory = gameFactory;
        }

        /// <summary>
        /// Get all the game informations relative to the player.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of cards.</returns>
        public async Task<GameInitDto> Handle(GetGameInformationsQuery request, CancellationToken cancellationToken)
        {
            var gameAggregate = await EventStoreRepository.Get(request.GameId);
            var game = GameFactory.LoadFromAggregate<IGame>(gameAggregate);

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            var player = game.GetTeams().SelectMany(t => t.GetPlayers()).SingleOrDefault(p => p.Id == request.PlayerId);

            if(player == null)
            {
                throw new GameException($"Player id {request.PlayerId} doesn't exist in game {request.GameId}.");
            }

            var cards = player.GetCards().Select(c => (int)c.Card);

            return new GameInitDto
            {
                PlayerCards = cards,
                Dealer = game.CurrentDealer,
                CurrentPlayerNumber = game.CurrentPlayerNumber,
                LocalPlayerNumber = player.Number,
                TurnEndTime = game.CurrentTurnTimeout
            };
        }
    }
}
