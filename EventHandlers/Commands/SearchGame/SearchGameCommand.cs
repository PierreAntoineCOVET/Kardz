using Domain.Enums;
using DTOs;
using MediatR;
using System;

namespace EventHandlers.Commands.SearchGame
{
    /// <summary>
    /// Search game command.
    /// </summary>
    public class SearchGameCommand : IRequest<GameDto>
    {
        /// <summary>
        /// Player id.
        /// </summary>
        public Guid PlayerId { get; set; }

        public GamesEnum GameType { get; set; }
    }
}
