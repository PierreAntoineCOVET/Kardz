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
        // TODO: remove and use persistance DB or events
        private static readonly ConcurrentDictionary<Guid, IGame> Games = new ConcurrentDictionary<Guid, IGame>();

        public static void AddGame(IGame game)
        {
            if (!Games.TryAdd(game.Id, game))
                throw new GameAlreadyExistException(game.Id);
        }
    }
}
