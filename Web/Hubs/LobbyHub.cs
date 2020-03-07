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

        public async Task NewPlayer(Guid guid)
        {
            int numberOfPlayers = await Mediator.Send(new AddPlayerToLobbyCommand
            {
                PlayerId = guid
            });

            await Clients.All.SendAsync("newPlayer", numberOfPlayers);
        }

        public async Task SearchGame(Guid guid)
        {
            var game = await Mediator.Send(new SearchGameCommand
            {
                PlayerId = guid
            });

            if(game != null)
                await Clients.All.SendAsync("gameStarted", game);
        }
    }
}
