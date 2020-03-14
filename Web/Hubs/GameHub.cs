using EventHandlers.Commands.ShuffleCards;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public class GameHub : Hub
    {
        private IMediator Mediator;

        public GameHub(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task GetCardsForPlayer(Guid gameId, Guid playerId)
        {
            var cards = await Mediator.Send(new ShuffleCardsCommand
            {
                GameId = gameId,
                PlayerId = playerId
            });

            await Clients.All.SendAsync($"playerCardsReceived{playerId}", cards);
        }
    }
}
