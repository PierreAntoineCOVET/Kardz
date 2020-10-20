using Domain.Configuration;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Factories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Lobby for coinche players.
    /// </summary>
    internal class CoincheLobby : LobbyBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="PlayerFactory">Player creationf factory.</param>
        public CoincheLobby(PlayerFactory PlayerFactory)
            : base(PlayerFactory)
        { }

        /// <summary>
        /// Game type.
        /// </summary>
        public override GamesEnum Game => GamesEnum.Coinche;

        /// <summary>
        /// Create a game.
        /// </summary>
        /// <returns><see cref="IGame"/></returns>
        /// <remarks>Players in the game a removed from the lobby.</remarks>
        public async override Task<IGame> CreateGame()
        {
            IEnumerable<CoinchePlayer> selectedPlayers;

            try
            {
                if (!await CanStartGame())
                    throw new GameException($"Not enough player to crate a coinche game ({PlayersLookingForGame.Count}).");

                await LobbyPlayersLock.WaitAsync();

                selectedPlayers = PlayersLookingForGame
                    .Take(Consts.NUMBER_OF_PLAYERS_FOR_A_GAME)
                    .Cast<CoinchePlayer>()
                    .ToList();

                foreach (var player in selectedPlayers)
                {
                    PlayersLookingForGame.Remove(player);
                    PlayersInLobby.Remove(player);
                }
            }
            finally
            {
                LobbyPlayersLock.Release();
            }

            return new CoincheGame(Guid.NewGuid(), selectedPlayers);
        }

        /// <summary>
        /// Ensure there is enough players to create a game.
        /// </summary>
        /// <returns>True if there is enough players.</returns>
        public async override Task<bool> CanStartGame()
        {
            try
            {
                await LobbyPlayersLock.WaitAsync();

                return PlayersLookingForGame.Count >= Consts.NUMBER_OF_PLAYERS_FOR_A_GAME;
            }
            finally
            {
                LobbyPlayersLock.Release();
            }
        }
    }
}
