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
    /// <summary>
    /// Lobby service.
    /// </summary>
    public class LobbiesService
    {
        private LobbyFactory LobbyFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LobbiesService(LobbyFactory lobbyFactory)
        {
            Lobbys = new Dictionary<GamesEnum, ILobby>();
            LobbyFactory = lobbyFactory;
        }

        public ILobby GetLobby(GamesEnum game)
        {
            if (!Lobbys.ContainsKey(game))
            {
                var lobby = LobbyFactory.CreateLobby(game);
                Lobbys.Add(game, LobbyFactory.CreateLobby(game));
            }

            return Lobbys[game];
        }

        /// <summary>
        /// Current active lobby.
        /// Lobby are not persisted on purpose.
        /// </summary>
        private Dictionary<GamesEnum, ILobby> Lobbys { get; set; }
    }
}
