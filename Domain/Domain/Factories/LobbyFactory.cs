using Domain.Domain.Implementations.Coinche;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Factories
{
    public static class LobbyFactory
    {
        public static ILobby GetLobby(GamesEnum gamesEnum)
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
