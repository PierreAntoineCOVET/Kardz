using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Game contracts.
    /// </summary>
    public interface IGame : IAggregate
    {
        /// <summary>
        /// Game's id.
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// Game's teams.
        /// </summary>
        IEnumerable<ITeam> Teams { get; }

        /// <summary>
        /// Number of the current dealer.
        /// </summary>
        int CurrentDealer { get; }

        /// <summary>
        /// Number of the player whose turn it is.
        /// </summary>
        int CurrentPlayerNumber { get; }

        /// <summary>
        /// Return the number of the player before current.
        /// </summary>
        int LastPlayerNumber { get; }

        /// <summary>
        /// Game contract.
        /// </summary>
        IContract Contract { get; }

        /// <summary>
        /// Aplly a contract to the game.
        /// </summary>
        /// <param name="color">Contract color, if needed.</param>
        /// <param name="value">Contract value, if needed.</param>
        /// <param name="player">Contract maker.</param>
        /// <param name="game">Contract's game.</param>
        void SetGameContract(ColorEnum? color, int? value, Guid player, Guid game);
    }
}
