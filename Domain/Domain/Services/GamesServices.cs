using Domain.Domain.Interfaces;
using Domain.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Services
{
    public static class GamesServices
    {
        private static readonly List<IGame> Games = new List<IGame>();

        public static void AddGame(IGame game)
        {
            if (Games.Any(g => g.Id == game.Id))
                throw new GameAlreadyExistException(game.Id);

            Games.Add(game);
        }
    }
}
