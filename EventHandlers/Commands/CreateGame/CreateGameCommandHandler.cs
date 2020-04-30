using Domain.Enums;
using Domain.Services;
using DTOs;
using EventHandlers.Notifications.Aggregate;
using MediatR;
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

        private IMediator Mediator;

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="lobbiesService">Lobby service.</param>
        /// <param name="gameFactory">Game factory.</param>
        public CreateGameCommandHandler(LobbiesService lobbiesService, IMediator mediator)
        {
            LobbiesService = lobbiesService;
            Mediator = mediator;
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

            await Mediator.Publish(new AggregateSaveNotification
            {
                Aggregate = game
            });

            return game.ToGameDto();
        }
    }
}
