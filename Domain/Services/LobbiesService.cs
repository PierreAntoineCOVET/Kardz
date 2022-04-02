using Domain.Enums;
using Domain.Factories;
using Domain.Interfaces;
using System.Collections.Generic;

namespace Domain.Services
{
    /// <summary>
    /// Lobby service.
    /// </summary>
    public class LobbiesService
    {
        private readonly LobbyFactory LobbyFactory;

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
                Lobbys.Add(game, lobby);
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
