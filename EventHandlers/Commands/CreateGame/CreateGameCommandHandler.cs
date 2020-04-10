using Domain.Domain.Services;
using Domain.Events;
using DTOs;
using EventHandlers.Notifications.Game;
using MediatR;
using System;
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

        private IServiceProvider ServiceProvider;

        /// <summary>
        /// Cosntructor.
        /// </summary>
        /// <param name="lobbiesService">Lobby service.</param>
        /// <param name="gamesServices">Game service.</param>
        public CreateGameCommandHandler(IMediator mediator, LobbiesService lobbiesService, IServiceProvider serviceProvider)
        {
            Mediator = mediator;
            LobbiesService = lobbiesService;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Handler for game creation.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="GameDto"/></returns>
        public async Task<GameDto> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var game = await LobbiesService.Lobby.CreateGame();

            if (game == null)
                return null;

            var createGameDomainEvent = new CreateGameEvent(ServiceProvider, game);
            await Mediator.Publish(new GameEventNotification { DomainEvent = createGameDomainEvent });

            await game.ShuffleCards();

            var shuffleCardsDomainEvent = new ShuffleCardsEvent(ServiceProvider, game);
            await Mediator.Publish(new GameEventNotification { DomainEvent = shuffleCardsDomainEvent });

            return game.ToGameDto();
        }
    }
}
