using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SearchGame
{
    /// <summary>
    /// Handler for <see cref="SearchGameCommand"/>.
    /// </summary>
    public class SearchGameCommandHandler : IRequestHandler<SearchGameCommand, bool>
    {
        /// <summary>
        /// Lobbies service.
        /// </summary>
        private LobbiesService LobbiesService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lobbiesService">Lobbies service.</param>
        public SearchGameCommandHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if there is enough non idle player to start a game.</returns>
        public Task<bool> Handle(SearchGameCommand request, CancellationToken cancellationToken)
        {
            LobbiesService.Lobby.AddPlayerLookingForGame(request.PlayerId);

            return Task.FromResult(LobbiesService.Lobby.CanStartGame());
        }
    }
}
