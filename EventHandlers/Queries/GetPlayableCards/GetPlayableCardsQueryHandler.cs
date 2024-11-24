using Domain.Enums;
using Domain.Exceptions;
using Domain.Factories;
using Domain.Interfaces;
using EventHandlers.Mappers;
using EventHandlers.Queries.GetNomberOfPlayersInLobby;
using EventHandlers.Repositories;
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
        /// <param name="eventStoreRepository"></param>
        /// <param name="gameFactory"></param>
        public GetPlayableCardsQueryHandler(IEventStoreRepository eventStoreRepository, GameFactory gameFactory)
        {
            EventStoreRepository = eventStoreRepository;
            GameFactory = gameFactory;
        }

        public async Task<IEnumerable<int>> Handle(GetPlayableCardsQuery request, CancellationToken cancellationToken)
        {
            var gameAggregate = await EventStoreRepository.Get(request.GameId);
            var game = GameFactory.LoadFromAggregate<IGame>(gameAggregate);

            if (game == null)
            {
                throw new GameException($"Game id {request.GameId} not found.");
            }

            game.GetPlayableCards(request.PlayerId, request.PossibleCards.Select(c => CardFactory.CreateGameCard(c.ToDtoEnum())));

            return new List<int>();
        }
    }
}
