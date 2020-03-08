using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheLobby : BaseLobby
    {
        public override GamesEnum Game => GamesEnum.Coinche;
        private SemaphoreSlim CreateGameLock = new SemaphoreSlim(1, 1);

        public async override Task<IGame> CreateGame()
        {
            IEnumerable<Player> selectedPlayers;

            try
            {
                await CreateGameLock.WaitAsync();

                if (!CanStartGame())
                    return null;

                selectedPlayers = PlayersLookingForGame.Take(4).Select(kvp => kvp.Value).ToList();
                foreach (var player in selectedPlayers)
                {
                    PlayersLookingForGame.TryRemove(player.Id, out Player _);
                    PlayersInLobby.TryRemove(player.Id, out Player _);
                }
            }
            finally
            {
                CreateGameLock.Release();
            }

            var game = new CoincheGame(Guid.NewGuid());
            game.AddPlayers(selectedPlayers);
            GamesServices.AddGame(game);
            return game;
        }

        public override bool CanStartGame()
        {
            return PlayersLookingForGame.Count >= 4;
        }
    }
}
