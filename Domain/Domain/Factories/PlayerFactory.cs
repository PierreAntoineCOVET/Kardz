using Domain.Domain.Implementations.Coinche;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Factories
{
    /// <summary>
    /// Create players.
    /// </summary>
    public static class PlayerFactory
    {
        /// <summary>
        /// Create a player based on game type.
        /// </summary>
        /// <param name="gamesEnum">Game type.</param>
        /// <param name="playerId">Player's id.</param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(GamesEnum gamesEnum, Guid playerId)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoinchePlayer(playerId);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
