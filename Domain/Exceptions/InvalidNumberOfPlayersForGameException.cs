using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class InvalidNumberOfPlayersForGameException : Exception
    {
        public InvalidNumberOfPlayersForGameException(int number, GamesEnum gamesEnum)
            : base($"Game {gamesEnum} doesn't allow for {number} player(s)")
        { }
    }
}
