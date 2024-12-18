﻿using Domain.Configuration;
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
        IEnumerable<ITeam> GetTeams();

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
        IContract GetContract();

        /// <summary>
        /// Time at wich the current turn times out.
        /// </summary>
        DateTimeOffset CurrentTurnTimeout { get; }

        /// <summary>
        /// Aplly a contract to the game.
        /// </summary>
        /// <param name="color">Contract color, if needed.</param>
        /// <param name="value">Contract value, if needed.</param>
        /// <param name="player">Contract maker.</param>
        /// <param name="coinched">Coinched or counter coinched.</param>
        /// <returns>True if the contract applyed correctly, false if it failed.</returns>
        void SetGameContract(CoincheCardColorsEnum? color, int? value, Guid player, bool coinched);

        /// <summary>
        /// Start a new take.
        /// </summary>
        void StartNewTake();

        /// <summary>
        /// Get the list of playable cards from the current list.
        /// Validate that the given list is what was expected.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="possibleCards"></param>
        /// <returns></returns>
        IEnumerable<ICards> GetPlayableCards(Guid playerId, IEnumerable<ICards> possibleCards);
    }
}
