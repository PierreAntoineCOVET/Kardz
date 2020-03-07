using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class UnknownGameTypeException : Exception
    {
        public UnknownGameTypeException(GamesEnum gamesEnum) 
            : base($"Game type {gamesEnum} is unknown")
        { }
    }
}
