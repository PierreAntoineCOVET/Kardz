using Domain.Enums;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Factories
{
    /// <summary>
    /// Game factory.
    /// </summary>
    public static class GameFactory
    {
        public static IGame CreateGame(GamesEnum gamesEnum, IEnumerable<IPlayer> players)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheGame(Guid.NewGuid(), players);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
