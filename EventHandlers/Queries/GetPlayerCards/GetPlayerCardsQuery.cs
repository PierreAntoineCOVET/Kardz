﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetPlayerCards
{
    /// <summary>
    /// Get cards for the given player.
    /// </summary>
    public class GetPlayerCardsQuery : IRequest<IEnumerable<int>>
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