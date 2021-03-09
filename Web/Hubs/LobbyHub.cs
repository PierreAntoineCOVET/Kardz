using EventHandlers.Commands.AddPlayerTolobby;
using EventHandlers.Commands.SearchGame;
using EventHandlers.Queries.GetNomberOfPlayersInLobby;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs
{
    /// <summary>
    /// SignalR hub for all lobby communications.
    /// </summary>
    public class LobbyHub : BaseHub
    {
        /// <summary>
        /// MediatR query manager.
        /// </summary>
        private IMediator Mediator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mediator">MediatR hub.</param>
        /// <param name="memoryCache">Internal memory cache for players connection mapping.</param>
        public LobbyHub(IMediator mediator, IMemoryCache memoryCache)
            : base(memoryCache)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Add a new player to the lobby.
        /// </summary>
        /// <param name="guid">Guid of the player to add.</param>
        /// <returns></returns>
        public async Task AddNewPlayer(Guid guid)
        {
            int numberOfPlayers = await Mediator.Send(new AddPlayerToLobbyCommand
            {
                PlayerId = guid,
                GameType = 0
            });

            await Clients.All.SendAsync("playersInLobby", numberOfPlayers);
        }

        /// <summary>
        /// Mark a player as searching for a game.
        /// </summary>
        /// <param name="guid">Guid of the player.</param>
        /// <returns></returns>
        public async Task SearchGame(Guid guid)
        {
            var newGame = await Mediator.Send(new SearchGameCommand
            {
                PlayerId = guid,
                GameType = 0
            });

            if(newGame != null)
            {
                var playersId = newGame.PlayersId.Select(pid => pid).ToList();
                var playersConnectionsId = GetPlayerConnectionId(playersId).ToList();
                await Clients.Clients(playersConnectionsId).SendAsync("gameStarted", newGame.Id);

                var numberOfPlayersLeft = await Mediator.Send(new GetNumberOfPlayersInLobbyQuery());
                await Clients.All.SendAsync("playersInLobby", numberOfPlayersLeft);
            }
        }
    }
}
