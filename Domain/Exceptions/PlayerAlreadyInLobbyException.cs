using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class PlayerAlreadyInLobbyException : Exception
    {
        public PlayerAlreadyInLobbyException(string message)
            : base(message)
        { }
    }
}
