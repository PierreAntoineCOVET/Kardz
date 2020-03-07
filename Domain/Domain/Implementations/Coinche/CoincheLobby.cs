using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheLobby : BaseLobby
    {
        public override GamesEnum Game => GamesEnum.Coinche;

        public override IGame CreateGame()
        {
            if (PlayersLookingForGame.Count < 4)
                throw new InvalidNumberOfPlayersForGameException(PlayersLookingForGame.Count, Game);

            var game = new CoincheGame(Guid.NewGuid());
            game.AddPlayers(PlayersLookingForGame.Take(4));
            GamesServices.AddGame(game);
            return game;
        }

        public override bool CanStartGame()
        {
            return PlayersLookingForGame.Count >= 4;
        }
    }
}
