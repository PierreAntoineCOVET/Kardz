using Domain.Domain.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    /// <summary>
    /// Coinche player.
    /// </summary>
    internal class CoinchePlayer : IPlayer
    {
        /// <summary>
        /// Player's Id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Player's number in the game.
        /// Used for cards distribution.
        /// </summary>
        /// <remarks>Can be replaced with id ?</remarks>
        public int Number { get; set; }

        /// <summary>
        /// List of cards.
        /// </summary>
        public IEnumerable<CardsEnum> Cards { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Player's Id.</param>
        public CoinchePlayer(Guid id)
        {
            this.Id = id;
        }
    }
}
