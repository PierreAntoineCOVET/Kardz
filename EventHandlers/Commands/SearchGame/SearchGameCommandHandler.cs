using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using DTOs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventHandlers.Commands.SearchGame
{
    public class SearchGameCommandHandler : IRequestHandler<SearchGameCommand, (GameDto game, int numberOfPlayersInLobby)?>
    {
        public async Task<(GameDto game, int numberOfPlayersInLobby)?> Handle(SearchGameCommand request, CancellationToken cancellationToken)
        {
            var lobby = LobbiesService.GetLobby((GamesEnum)request.GamesType);

            lobby.AddPlayerLookingForGame(request.PlayerId);

            if(lobby.CanStartGame())
            {
                var game = await lobby.CreateGame();
                return (game?.ToGameDto(), lobby.NumberOfPlayers);
            }

            return null;
        }
    }
}
