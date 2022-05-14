using System;
using System.Collections.Generic;

namespace Domain.Entities.ReadEntities
{
    /// <summary>
    /// Coinche team.
    /// </summary>
    public class CoincheTeam
    {
        /// <summary>
        /// Teams Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Team number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Team score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Team's players.
        /// </summary>
        public ICollection<CoinchePlayer> Players { get; set; }

        /// <summary>
        /// Team's game id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Team's game.
        /// </summary>
        public CoincheGame Game { get; set; }
    }
}
