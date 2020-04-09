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
    /// Lobby factory.
    /// </summary>
    public static class LobbyFactory
    {
        /// <summary>
        /// Create lobby based on game type.
        /// </summary>
        /// <param name="gamesEnum">Game type.</param>
        /// <returns></returns>
        public static ILobby CreateLobby(GamesEnum gamesEnum)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheLobby();

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
