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
        int CurrentDealer { get; set; }

        /// <summary>
        /// Number of the player whose turn it is.
        /// </summary>
        int CurrentPlayer { get; set; }
    }
}
