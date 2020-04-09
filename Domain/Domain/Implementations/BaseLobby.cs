using Domain.Domain.Factories;
using Domain.Domain.Implementations;
using Domain.Domain.Implementations.Coinche;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Implementations
{
    /// <summary>
    /// Base lobby class.
    /// </summary>
    internal abstract class BaseLobby : ILobby
    {
        /// <summary>
        /// Players idling in lobby.
        /// </summary>
        private protected ConcurrentDictionary<Guid, IPlayer> PlayersInLobby = new ConcurrentDictionary<Guid, IPlayer>();

        /// <summary>
        /// Players looking for a game.
        /// </summary>
        private protected ConcurrentDictionary<Guid, IPlayer> PlayersLookingForGame = new ConcurrentDictionary<Guid, IPlayer>();

        /// <summary>
        /// Game type.
        /// </summary>
        public abstract GamesEnum Game { get; }

        /// <summary>
        /// Add a player to the lobby.
        /// </summary>
        /// <param name="id">Player's Id to add.</param>
        public void AddPlayer(Guid id)
        {
            if (PlayersInLobby.Any(p => p.Key == id))
                throw new PlayerAlreadyInLobbyException(id);

            var player = PlayerFactory.CreatePlayer(Game, id);
            PlayersInLobby.TryAdd(id, player);
        }

        /// <summary>
        /// Number of idle players.
        /// </summary>
        public int NumberOfPlayers => PlayersInLobby.Count;

        /// <summary>
        /// Change a player from idle to looking for a game.
        /// </summary>
        /// <param name="id"></param>
        public void AddPlayerLookingForGame(Guid id)
        {
            if (!PlayersInLobby.Any(p => p.Key == id))
                throw new PlayerSearchGameWhileNotInLobbyException(id);

            if (!PlayersLookingForGame.TryAdd(id, PlayersInLobby[id]))
                throw new PlayerAlreadyLookingForAGameException(id);
        }

        /// <summary>
        /// Define if a game can start.
        /// </summary>
        /// <returns></returns>
        public abstract bool CanStartGame();

        /// <summary>
        /// Create a game.
        /// </summary>
        /// <returns></returns>
        public abstract Task<IGame> CreateGame();
    }
}
