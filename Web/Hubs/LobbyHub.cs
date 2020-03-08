using EventHandlers.Commands.AddPlayerTolobby;
using EventHandlers.Commands.SearchGame;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public class LobbyHub : Hub
    {
        private IMediator Mediator;

        public LobbyHub(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task AddNewPlayer(Guid guid)
        {
            int numberOfPlayers = await Mediator.Send(new AddPlayerToLobbyCommand
            {
                PlayerId = guid
            });

            await Clients.All.SendAsync("playersInLobby", numberOfPlayers);
        }

        public async Task SearchGame(Guid guid)
        {
            var response = await Mediator.Send(new SearchGameCommand
            {
                PlayerId = guid
            });

            if(response.HasValue)
            {
                await Clients.All.SendAsync("gameStarted", response.Value.game);
                await Clients.All.SendAsync("playersInLobby", response.Value.numberOfPlayersInLobby);
            }
        }
    }
}
