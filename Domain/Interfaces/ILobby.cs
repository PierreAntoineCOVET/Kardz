using Domain.Enums;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
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
        /// Search a game for the given player.
        /// Return null if no games are available.
        /// </summary>
        /// <param name="id"></param>
        Task<IGame> SearchGame(Guid id);

        /// <summary>
        /// Get the number of players in the lobby.
        /// </summary>
        int NumberOfPlayers { get; }
    }
}
