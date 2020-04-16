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
    public class LobbyFactory
    {
        /// <summary>
        /// Factory to create a player depending on the game type.
        /// </summary>
        PlayerFactory PlayerFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="playerFactory"></param>
        public LobbyFactory(PlayerFactory playerFactory)
        {
            PlayerFactory = playerFactory;
        }

        /// <summary>
        /// Create lobby based on game type.
        /// </summary>
        /// <param name="gamesEnum">Game type.</param>
        /// <returns></returns>
        public ILobby CreateLobby(GamesEnum gamesEnum)
        {
            switch (gamesEnum)
            {
                case GamesEnum.Coinche:
                    return new CoincheLobby(PlayerFactory);

                default:
                    throw new UnknownGameTypeException(gamesEnum);
            }
        }
    }
}
