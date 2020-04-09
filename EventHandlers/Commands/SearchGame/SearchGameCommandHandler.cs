using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SearchGame
{
    public class SearchGameCommandHandler : IRequestHandler<SearchGameCommand, bool>
    {
        private LobbiesService LobbiesService;

        public SearchGameCommandHandler(LobbiesService lobbiesService)
        {
            LobbiesService = lobbiesService;
        }

        public Task<bool> Handle(SearchGameCommand request, CancellationToken cancellationToken)
        {
            LobbiesService.Lobby.AddPlayerLookingForGame(request.PlayerId);

            return Task.FromResult(LobbiesService.Lobby.CanStartGame());
        }
    }
}
