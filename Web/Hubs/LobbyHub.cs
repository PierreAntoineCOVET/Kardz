using EventHandlers.Commands.AddPlayerTolobby;
using EventHandlers.Commands.CreateGame;
using EventHandlers.Commands.SearchGame;
using EventHandlers.Queries.GetNomberOfPlayersInLobby;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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
                PlayerId = guid
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
            var canCreateGame = await Mediator.Send(new SearchGameCommand
            {
                PlayerId = guid
            });

            if(canCreateGame)
            {
                var game = await Mediator.Send(new CreateGameCommand());

                if (game != null)
                {
                    var playersId = game.PlayersId.Select(pid => pid).ToList();
                    var playersConnectionsId = GetPlayerConnectionId(playersId).ToList();
                    await Clients.Clients(playersConnectionsId).SendAsync("gameStarted", game.Id);

                    var numberOfPlayersLeft = await Mediator.Send(new GetNumberOfPlayersInLobbyQuery());
                    await Clients.All.SendAsync("playersInLobby", numberOfPlayersLeft);
                }
            }
        }
    }
}
