using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class InvalidNumberOfPlayersForGameTeamException : Exception
    {
        public InvalidNumberOfPlayersForGameTeamException(int number, GamesEnum gamesEnum)
            : base($"Game {gamesEnum} teams doesn't allow for {number} player(s)")
        { }
    }
}
