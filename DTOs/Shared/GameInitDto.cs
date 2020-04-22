using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.Shared
{
    /// <summary>
    /// PLayers informations.
    /// </summary>
    public class GameInitDto
    {
        /// <summary>
        /// List of cards for the requesting player.
        /// </summary>
        public IEnumerable<int> PlayerCards { get; set; }

        /// <summary>
        /// Number of the requiting player.
        /// </summary>
        public int PlayerNumber { get; set; }

        /// <summary>
        /// Number of the player that is the current dealer.
        /// </summary>
        public int Dealer { get; set; }

        /// <summary>
        /// Number of the player whose turn it is to play.
        /// </summary>
        public int PlayerPlaying { get; set; }
    }
}
