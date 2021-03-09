using EventHandlers.Commands.SetGameContract;
using EventHandlers.Queries.GetPlayerCards;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Web.Hubs
{
    /// <summary>
    /// SignalR hub for all game communications.
    /// </summary>
    public class GameHub : BaseHub
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
        public GameHub(IMediator mediator, IMemoryCache memoryCache)
            : base(memoryCache)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Get radomised card for a player and a game.
        /// </summary>
        /// <param name="gameId">Guid of the game played.</param>
        /// <param name="playerId">Id of the player to serve.</param>
        /// <returns></returns>
        public async Task GetGameInformations(Guid gameId, Guid playerId)
        {
            var gameInit = await Mediator.Send(new GetGameInformationsQuery
            {
                GameId = gameId,
                PlayerId = playerId
            });

            await Clients.Client(Context.ConnectionId).SendAsync("gameInformationsReceived", gameInit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task SetGameContract(int? color, int? value, Guid gameId, Guid playerId)
        {
            var contract = await Mediator.Send(new SetGameContractCommand
            {
                GameId = gameId,
                PlayerId = playerId,
                Color = color,
                Value = value
            });

            await Clients.All.SendAsync("gameContractChanged", contract);
        }
    }
}
