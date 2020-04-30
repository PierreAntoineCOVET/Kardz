using Domain.Enums;
using Domain.Exceptions;
using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using System;

namespace Domain.Factories
{
    /// <summary>
    /// Create players.
    /// </summary>
    public class PlayerFactory
    {
        /// <summary>
        /// Create a player based on game type.
        /// </summary>
        /// <param name="gamesEnum">Game type.</param>
        /// <param name="playerId">Player's id.</param>
        /// <returns></returns>
        public IPlayer CreatePlayer(GamesEnum gamesEnum, Guid playerId)
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
