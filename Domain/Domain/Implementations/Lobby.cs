using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations
{
    public static class Lobby
    {
        private static List<Player> Players = new List<Player>();

        public static void AddPlayer(Guid id)
        {
            if (Players.Any(p => p.Id == id))
                throw new PlayerAlreadyInLobbyException($"Player {id.ToString()} is already in the lobby");

            Players.Add(new Player(id));
        }

        public static int NumberOfPlayers => Players.Count;
    }
}
