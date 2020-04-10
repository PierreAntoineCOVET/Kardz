using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.ShuffleCards
{
    /// <summary>
    /// Shuffle cards command.
    /// </summary>
    public class ShuffleCardsCommand : IRequest<IEnumerable<int>>
    {
        /// <summary>
        /// Game to shuffle cards.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Player to serve shuffled cards.
        /// </summary>
        public Guid PlayerId { get; set; }
    }
}
