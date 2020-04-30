using System;

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
        /// Player number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Player's game id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Player team number.
        /// </summary>
        public int TeamNumber { get; set; }

        /// <summary>
        /// Player's team.
        /// </summary>
        public CoincheTeam Team { get; set; }
    }
}
