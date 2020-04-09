using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetNomberOfPlayersInLobby
{
    /// <summary>
    /// Handler for <see cref="GetNumberOfPlayersInLobbyQuery"/>.
    /// </summary>
    public class GetNumberOfPlayersInLobbyQueryHandler : IRequestHandler<GetNumberOfPlayersInLobbyQuery, int>
    {
        private LobbiesService LobbiesService;

        public GetNumberOfPlayersInLobbyQueryHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        public Task<int> Handle(GetNumberOfPlayersInLobbyQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(LobbiesService.Lobby.NumberOfPlayers);
        }
    }
}
