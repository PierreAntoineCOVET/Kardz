﻿using System;

namespace DTOs.Shared
{
    /// <summary>
    /// Game contract informations.
    /// </summary>
    public class GameContractDto
    {
        /// <summary>
        /// Contract value.
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Contract color.
        /// </summary>
        public int? Color { get; set; }

        /// <summary>
        /// Id of the player that last made the contract.
        /// </summary>
        public int LastPlayerNumber { get; set; }

        /// <summary>
        /// True if the last player did not bet
        /// </summary>
        public bool HasContractFailed { get; set; }

        /// <summary>
        /// Id of the player that will update the contract.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Time at wich the current turn ends.
        /// </summary>
        public DateTimeOffset TurnEndTime { get; set; }

        /// <summary>
        /// Last player voted color.
        /// </summary>
        public int? LastColor { get; set; }

        /// <summary>
        /// LAst player voted value.
        /// </summary>
        public int? LastValue { get; set; }
    }
}
