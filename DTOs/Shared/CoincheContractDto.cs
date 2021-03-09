using System;

namespace DTOs.Shared
{
    /// <summary>
    /// Game contract informations.
    /// </summary>
    public class CoincheContractDto
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
        public bool HasLastPLayerPassed { get; set; }

        /// <summary>
        /// Id of the player that will update the contract.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Time at wich the current turn ends.
        /// </summary>
        public DateTimeOffset TurnEndTime { get; set; }
    }
}
