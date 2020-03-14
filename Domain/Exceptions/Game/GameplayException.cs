using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions.Game
{
    public class GameplayException : Exception
    {
        public GameplayException(string message)
            : base(message)
        { }
    }
}
