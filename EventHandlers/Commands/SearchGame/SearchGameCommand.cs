using DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.SearchGame
{
    /// <summary>
    /// Search game command.
    /// </summary>
    public class SearchGameCommand : IRequest<bool>
    {
        /// <summary>
        /// Player id.
        /// </summary>
        public Guid PlayerId { get; set; }

        //public int GamesType { get; set; }
    }
}
