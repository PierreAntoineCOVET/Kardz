using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public interface ILobby
    {
        GamesEnum Game { get; }

        void AddPlayer(Guid id);

        int NumberOfPlayers { get; }

        void AddPlayerLookingForGame(Guid id);

        bool CanStartGame();

        IGame CreateGame();
    }
}
