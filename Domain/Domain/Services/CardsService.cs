using Domain.Domain.Implementations;
using Domain.Domain.Implementations.Coinche;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Services
{
    /// <summary>
    /// Cards sevice (factory, ...).
    /// </summary>
    public static class CardsService
    {
        /// <summary>
        /// Get a cards deck for the given game.
        /// </summary>
        /// <param name="gamesEnum"><see cref="GamesEnum"/></param>
        /// <returns><see cref="ICardsDeck"/></returns>
        public static ICardsDeck GetCardDeck(GamesEnum gamesEnum)
        {
            if (gamesEnum == GamesEnum.Coinche)
                return new CoincheCardsDeck();

            throw new UnknownGameTypeException(gamesEnum);
        }
    }
}
