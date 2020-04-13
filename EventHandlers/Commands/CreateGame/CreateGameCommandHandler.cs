using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using DTOs;
using MediatR;
using Repositories.EventStoreRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.CreateGame
{
    /// <summary>
    /// Handler for <see cref="CreateGameCommand"/>.
    /// </summary>
    class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameDto>
    {
        /// <summary>
        /// Lobby service.
        /// </summary>
        private LobbiesService LobbiesService;

        private IEventStoreRepository EventStoreRepository;

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="lobbiesService">Lobby service.</param>
        /// <param name="gameFactory">Game factory.</param>
        public CreateGameCommandHandler(LobbiesService lobbiesService, IEventStoreRepository eventStoreRepository)
        {
            LobbiesService = lobbiesService;
            EventStoreRepository = eventStoreRepository;
        }

        /// <summary>
        /// Handler for game creation.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="GameDto"/></returns>
        public async Task<GameDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby((GamesEnum)request.GameType);

            var game = await lobby.CreateGame();

            game.ShuffleCards();

            await EventStoreRepository.Save(game.ToAggregate());

            return game.ToGameDto();
        }
    }
}
