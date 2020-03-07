using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class GameAlreadyFullException : Exception
    {
        public GameAlreadyFullException(Guid id)
            : base($"Game {id} is already full")
        { }
    }
}
