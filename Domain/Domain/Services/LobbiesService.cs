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
        /// <summary>
        /// Constructor.
        /// </summary>
        public LobbiesService()
        {
            Lobby = LobbyFactory.CreateLobby(GamesEnum.Coinche);
        }

        /// <summary>
        /// Current active lobby.
        /// Lobby are not persisted on purpose.
        /// </summary>
        public ILobby Lobby { get; private set; }
    }
}
