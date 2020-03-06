using EventHandlers.Commands.AddPlayerTolobby;
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
                Guid = guid
            });

            await Clients.All.SendAsync("newPlayer", numberOfPlayers);
        }
    }
}
