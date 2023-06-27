using System;
using System.Collections.Generic;

namespace Domain.Entities.ReadEntities
{
    /// <summary>
    /// Coinche player.
    /// </summary>
    public class CoinchePlayer
    {
        /// <summary>
        /// PLayer id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Player cards.
        /// </summary>
        public string Cards { get; set; }

        /// <summary>
        /// Player playable cards in the current turn.
        /// </summary>
        public string PlayableCards { get; set; }

        /// <summary>
        /// Player number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Team Id.
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Player's team.
        /// </summary>
        public CoincheTeam Team { get; set; }

        /// <summary>
        /// List of all takes of the player.
        /// </summary>
        public ICollection<CoincheTake> Takes { get; set; }
    }
}
