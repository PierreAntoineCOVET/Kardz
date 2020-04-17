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
        /// Teams of the game.
        /// </summary>
        public ICollection<CoincheTeam> Teams { get; set; }
    }
}
