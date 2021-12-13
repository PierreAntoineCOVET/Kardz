using Domain.Enums;
using Domain.Services;
using DTOs;
using EventHandlers.Mappers;
using EventHandlers.Notifications.Aggregate;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SearchGame
{
    /// <summary>
    /// Handler for <see cref="SearchGameCommand"/>.
    /// </summary>
    public class SearchGameCommandHandler : IRequestHandler<SearchGameCommand, GameDto>
    {
        /// <summary>
        /// Lobbies service.
        /// </summary>
        private readonly LobbiesService LobbiesService;

        /// <summary>
        /// Mediator.
        /// </summary>
        private readonly IMediator Mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lobbiesService">Lobbies service.</param>
        /// <param name="mediator">Mediator service.</param>
        public SearchGameCommandHandler(LobbiesService lobbiesService, IMediator mediator)
        {
            LobbiesService = lobbiesService;
            Mediator = mediator;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if there is enough non idle player to start a game.</returns>
        public async Task<GameDto> Handle(SearchGameCommand request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby((GamesEnum)request.GameType);

            var newGame = await lobby.SearchGame(request.PlayerId);

            if(newGame != null)
            {
                await Mediator.Publish(new AggregateSaveNotification
                {
                    Aggregate = newGame
                });
            }

            return newGame?.ToGameDto();
        }
    }
}
