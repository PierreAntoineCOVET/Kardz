using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Game
{
    public class GameCreationException: Exception
    {
        public GameCreationException(string message)
            : base(message)
        { }
    }
}
