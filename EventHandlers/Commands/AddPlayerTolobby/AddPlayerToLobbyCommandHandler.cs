using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommandHandler : IRequestHandler<AddPlayerToLobbyCommand, int>
    {
        public Task<int> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby((GamesEnum)request.GamesType);

            lobby.AddPlayer(request.PlayerId);

            return Task.FromResult(lobby.NumberOfPlayers);
        }
    }
}
