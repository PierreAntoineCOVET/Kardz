using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Interfaces
{
    /// <summary>
    /// Common interface for all games lobby.
    /// </summary>
    public interface ILobby
    {
        /// <summary>
        /// Game type managed by the lobby.
        /// </summary>
        GamesEnum Game { get; }

        /// <summary>
        /// Add a new player in the lobby.
        /// </summary>
        /// <param name="id"></param>
        Task AddPlayer(Guid id);

        /// <summary>
        /// Add a player to the list of those looking for a game.
        /// </summary>
        /// <param name="id"></param>
        Task AddPlayerLookingForGame(Guid id);

        /// <summary>
        /// Indicate wether we can start a new game or not.
        /// </summary>
        /// <returns>true if we can start a new game</returns>
        /// <remarks>Check is not thread safe !</remarks>
        Task<bool> CanStartGame();

        /// <summary>
        /// Create a new game.
        /// </summary>
        /// <returns><see cref="IGame"/></returns>
        /// <remarks>PLayers in game are removed from the lobby.</remarks>
        Task<IGame> CreateGame();

        /// <summary>
        /// Get the number of players in the lobby.
        /// </summary>
        int NumberOfPlayers { get; }
    }
}
