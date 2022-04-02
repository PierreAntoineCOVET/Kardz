using Domain.Configuration;
using Domain.Enums;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Domain.Factories
{
    /// <summary>
    /// Game factory.
    /// </summary>
    public class GameFactory
    {
        /// <summary>
        /// Coinche game configuration.
        /// </summary>
        private readonly CoincheConfiguration Configuration;

        public GameFactory(CoincheConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IGame CreateGame(GamesEnum gamesEnum, IEnumerable<IPlayer> players)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheGame(Guid.NewGuid(), players, Configuration);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
