using Domain.Enums;
using Domain.Exceptions;
using Domain.Factories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.GamesLogic
{
    /// <summary>
    /// Base lobby class.
    /// </summary>
    internal abstract class LobbyBase : ILobby
    {
        /// <summary>
        /// Create player based on game type.
        /// </summary>
        PlayerFactory PlayerFactory;

        /// <summary>
        /// Game factory.
        /// </summary>
        GameFactory GameFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerFactory"></param>
        public LobbyBase(PlayerFactory playerFactory, GameFactory gameFactory)
        {
            PlayerFactory = playerFactory;
            GameFactory = gameFactory;
        }

        /// <summary>
        /// Players idling in lobby.
        /// </summary>
        public List<IPlayer> PlayersInLobby = new List<IPlayer>();

        /// <summary>
        /// Players looking for a game.
        /// </summary>
        public List<IPlayer> PlayersLookingForGame = new List<IPlayer>();

        /// <summary>
        /// Game type.
        /// </summary>
        public abstract GamesEnum Game { get; }

        /// <summary>
        /// Number of players in the lobby.
        /// </summary>
        public int NumberOfPlayers => PlayersInLobby.Count;

        /// <summary>
        /// Ensure a given game is created only once.
        /// </summary>
        protected SemaphoreSlim LobbyPlayersLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Add a player to the lobby.
        /// </summary>
        /// <param name="id">Player's Id to add.</param>
        public async Task AddPlayer(Guid id)
        {
            try
            {
                await LobbyPlayersLock.WaitAsync();

                if (PlayersInLobby.Any(p => p.Id == id))
                    throw new LobbyException($"Player {id} is already registered in the lobby");

                var player = PlayerFactory.CreatePlayer(Game, id);
                PlayersInLobby.Add(player);
            }
            finally
            {
                LobbyPlayersLock.Release();
            }
        }

        /// <summary>
        /// Change a player from idle to looking for a game.
        /// </summary>
        /// <param name="id"></param>
        public async Task<IGame> SearchGame(Guid id)
        {
            try
            {
                var playersForANewGame = new List<IPlayer>();

                await LobbyPlayersLock.WaitAsync();

                if (!PlayersInLobby.Any(p => p.Id == id))
                    throw new LobbyException($"Player {id} is not registered in the lobby.");

                if (PlayersLookingForGame.Any(p => p.Id == id))
                    throw new LobbyException($"Player {id} is already looking for a game.");

                var requiredNumberOfPlayersWaiting = NumberOfPlayerForANewGame - 1;

                if (PlayersLookingForGame.Count == requiredNumberOfPlayersWaiting)
                {
                    playersForANewGame.AddRange(PlayersLookingForGame);
                    playersForANewGame.Add(PlayersInLobby.Single(p => p.Id == id));

                    PlayersLookingForGame.Clear();
                    foreach (var player in playersForANewGame)
                    {
                        PlayersInLobby.Remove(player);
                    }

                    return GameFactory.CreateGame(Game, playersForANewGame);
                }
                else if(PlayersLookingForGame.Count < requiredNumberOfPlayersWaiting)
                {
                    PlayersLookingForGame.Add(PlayersInLobby.Single(p => p.Id == id));

                    return null;
                }
                else
                {
                    throw new LobbyException($"The number of players waiting for a game is {PlayersLookingForGame.Count}");
                }
            }
            finally
            {
                LobbyPlayersLock.Release();
            }
        }

        /// <summary>
        /// Define the number of players needed to start the game.
        /// Is set on the derived class.
        /// </summary>
        /// <returns></returns>
        public abstract int NumberOfPlayerForANewGame { get; }
    }
}
