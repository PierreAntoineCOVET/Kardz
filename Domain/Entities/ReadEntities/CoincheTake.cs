using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ReadEntities
{
    /// <summary>
    /// Coinche take.
    /// </summary>
    public class CoincheTake
    {
        /// <summary>
        /// DB ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Linked game ID.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Linked game object.
        /// </summary>
        public CoincheGame Game { get; set; }

        /// <summary>
        /// Linked player Id.
        /// </summary>
        public Guid CurrentPlayerId { get; set; }

        /// <summary>
        /// Linked player object.
        /// </summary>
        public CoinchePlayer CurrentPlayer { get; set; }

        /// <summary>
        /// Current fold to display.
        /// </summary>
        public string CurrentFold { get; set; }

        /// <summary>
        /// Last fold to display.
        /// </summary>
        public string PreviousFold { get; set; }

        /// <summary>
        /// Playable card by the current player for this fold.
        /// </summary>
        public string CurrentPlayerPlayableCards { get; set; }
    }
}
