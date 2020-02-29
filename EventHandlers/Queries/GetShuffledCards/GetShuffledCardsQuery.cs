using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetShuffledCards
{
    /// <summary>
    /// Query for getting a shuffled deck of cards for a given game.
    /// </summary>
    public class GetShuffledCardsQuery : IRequest<IEnumerable<CardsEnum>>
    {
        /// <summary>
        /// Game to play <see cref="GamesEnum"/>
        /// </summary>
        public int GameType { get; set; }
    }
}
