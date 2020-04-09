using Domain.Domain.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Domain.Implementations.Coinche
{
    /// <summary>
    /// Lobby for coinche players.
    /// </summary>
    internal class CoincheLobby : BaseLobby
    {
        /// <summary>
        /// Game type.
        /// </summary>
        public override GamesEnum Game => GamesEnum.Coinche;

        /// <summary>
        /// Ensure a given game is created only once.
        /// </summary>
        private SemaphoreSlim CreateGameLock = new SemaphoreSlim(1, 1);

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
                await CreateGameLock.WaitAsync();

                if (!CanStartGame())
                    return null;

                selectedPlayers = PlayersLookingForGame
                    .Take(4)
                    .Select(kvp => kvp.Value)
                    .Cast<CoinchePlayer>()
                    .ToList();
                foreach (var player in selectedPlayers)
                {
                    PlayersLookingForGame.TryRemove(player.Id, out IPlayer _);
                    PlayersInLobby.TryRemove(player.Id, out IPlayer _);
                }
            }
            finally
            {
                CreateGameLock.Release();
            }

            var game = new CoincheGame(Guid.NewGuid());
            game.AddPlayers(selectedPlayers);
            return game;
        }

        /// <summary>
        /// Ensure there is enough players to create a game.
        /// </summary>
        /// <returns>True if there is enough players.</returns>
        public override bool CanStartGame()
        {
            return PlayersLookingForGame.Count >= 4;
        }
    }
}
