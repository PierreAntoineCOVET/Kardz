using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class PlayerAlreadyInLobbyException : Exception
    {
        public PlayerAlreadyInLobbyException(Guid playerId)
            : base($"Player {playerId} is already in the lobby")
        { }
    }
}
