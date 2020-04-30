using Domain.Enums;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    /// <summary>
    /// AddPlayerToLobbyCommand handler.
    /// </summary>
    public class AddPlayerToLobbyCommandHandler : IRequestHandler<AddPlayerToLobbyCommand, int>
    {
        /// <summary>
        /// Games lobby services.
        /// </summary>
        private LobbiesService LobbiesService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lobbiesService">Games lobby services.</param>
        public AddPlayerToLobbyCommandHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of player in lobby.</returns>
        public Task<int> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby((GamesEnum)request.GameType);

            lobby.AddPlayer(request.PlayerId);

            return Task.FromResult(lobby.NumberOfPlayers);
        }
    }
}
