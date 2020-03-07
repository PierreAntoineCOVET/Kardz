using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class PlayerAlreadyLookingForAGameException : Exception
    {
        public PlayerAlreadyLookingForAGameException(Guid playerId) 
            :base($"Player {playerId} is already looking for a game")
        { }
    }
}
