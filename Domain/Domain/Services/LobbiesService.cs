using Domain.Domain.Factories;
using Domain.Domain.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Domain.Services
{
    public static class LobbiesService
    {
        private static readonly List<ILobby> Lobbies = new List<ILobby>();

        static LobbiesService()
        {
            Lobbies.Add(LobbyFactory.GetLobby(GamesEnum.Coinche));
        }

        public static ILobby GetLobby(GamesEnum gamesEnum)
        {
            var lobby = Lobbies.SingleOrDefault(l => l.Game == gamesEnum);

            return lobby;
        }
    }
}
