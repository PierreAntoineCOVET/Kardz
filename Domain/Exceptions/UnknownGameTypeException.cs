using Domain.Enums;
using System;

namespace Domain.Exceptions
{
    public class UnknownGameTypeException : Exception
    {
        public UnknownGameTypeException(GamesEnum gamesEnum) 
            : base($"Game type {gamesEnum} is unknown")
        { }
    }
}
