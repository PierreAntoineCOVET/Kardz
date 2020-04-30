using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ReadEntities
{
    /// <summary>
    /// Coinche game being played.
    /// </summary>
    public class CoincheGame
    {
        /// <summary>
        /// Game Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Cards on the board.
        /// </summary>
        public string CurrentCards { get; set; }

        /// <summary>
        /// Number of the current card's dealer.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Number of the player whose turn it is.
        /// </summary>
        public int CurrentPayerTurn { get; set; }

        /// <summary>
        /// Teams of the game.
        /// </summary>
        public ICollection<CoincheTeam> Teams { get; set; }
    }
}
