using DTOs.Shared;
using MediatR;
using System;

namespace EventHandlers.Queries.GetPlayerCards
{
    /// <summary>
    /// Get cards for the given player.
    /// </summary>
    public class GetGameInformationsQuery : IRequest<GameInitDto>
    {
        /// <summary>
        /// Player's game.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Player's id.
        /// </summary>
        public Guid PlayerId { get; set; }
    }
}
