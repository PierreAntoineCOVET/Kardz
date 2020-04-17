﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetNomberOfPlayersInLobby
{
    /// <summary>
    /// Get the total nomber of players in the lobby.
    /// </summary>
    public class GetNumberOfPlayersInLobbyQuery: IRequest<int>
    {
        /// <summary>
        /// Game's type of the Lobby.
        /// </summary>
        public int GameType { get; set; }
    }
}