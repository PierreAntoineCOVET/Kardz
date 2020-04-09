using Domain.Domain.Services;
using Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.AddPlayerTolobby
{
    public class AddPlayerToLobbyCommandHandler : IRequestHandler<AddPlayerToLobbyCommand, int>
    {
        private LobbiesService LobbiesService;

        public AddPlayerToLobbyCommandHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        public Task<int> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
        {
            LobbiesService.Lobby.AddPlayer(request.PlayerId);

            return Task.FromResult(LobbiesService.Lobby.NumberOfPlayers);
        }
    }
}
