using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class PlayerSearchGameWhileNotInLobbyException : Exception
    {
        public PlayerSearchGameWhileNotInLobbyException(Guid playerId)
            : base($"Player {playerId} search for of game while not being in lobby.")
        { }
    }
}
