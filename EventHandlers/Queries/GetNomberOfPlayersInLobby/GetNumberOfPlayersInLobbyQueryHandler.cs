using Domain.Enums;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetNomberOfPlayersInLobby
{
    /// <summary>
    /// Handler for <see cref="GetNumberOfPlayersInLobbyQuery"/>.
    /// </summary>
    public class GetNumberOfPlayersInLobbyQueryHandler : IRequestHandler<GetNumberOfPlayersInLobbyQuery, int>
    {
        /// <summary>
        /// Lobby service.
        /// </summary>
        private LobbiesService LobbiesService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lobbiesService">Lobby service.</param>
        public GetNumberOfPlayersInLobbyQueryHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of idle players in lobby.</returns>
        public Task<int> Handle(GetNumberOfPlayersInLobbyQuery request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby(request.GameType);

            return Task.FromResult(lobby.NumberOfPlayers);
        }
    }
}
