using System;
using System.Collections.Generic;

namespace Domain.Entities.ReadEntities
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
        /// Cards of the current turn.
        /// </summary>
        public string CurrentTurnCards { get; set; }

        /// <summary>
        /// Cards on the board.
        /// </summary>
        public string LastTurnCards { get; set; }

        /// <summary>
        /// Number of the current card's dealer.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Number of the current player.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Current turn timeout limit.
        /// </summary>
        public DateTimeOffset CurrentTurnTimeout { get; set; }

        /// <summary>
        /// Teams of the game.
        /// </summary>
        public ICollection<CoincheTeam> Teams { get; set; }

        /// <summary>
        /// List of all takes of the game.
        /// </summary>
        public ICollection<CoincheTake> Takes { get; set; }
    }
}
