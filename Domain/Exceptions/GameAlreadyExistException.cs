using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class GameAlreadyExistException : Exception
    {
        public GameAlreadyExistException(Guid id)
            : base($"Game {id} already exist")
        { }
    }
}
