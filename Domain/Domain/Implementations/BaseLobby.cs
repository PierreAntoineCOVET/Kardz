using Domain.Domain.Implementations;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations
{
    public abstract class BaseLobby : ILobby
    {
        private protected List<Player> PlayersInLobby = new List<Player>();

        private protected List<Player> PlayersLookingForGame = new List<Player>();

        public abstract GamesEnum Game { get; }

        public void AddPlayer(Guid id)
        {
            if (PlayersInLobby.Any(p => p.Id == id))
                throw new PlayerAlreadyInLobbyException(id);

            PlayersInLobby.Add(new Player(id));
        }

        public int NumberOfPlayers => PlayersInLobby.Count;

        public void AddPlayerLookingForGame(Guid id)
        {
            if (!PlayersInLobby.Any(p => p.Id == id))
                throw new PlayerSearchGameWhileNotInLobbyException(id);

            if (PlayersLookingForGame.Any(p => p.Id == id))
                throw new PlayerAlreadyLookingForAGameException(id);

            PlayersLookingForGame.Add(PlayersInLobby.Single(p => p.Id == id));
        }

        public abstract bool CanStartGame();

        public abstract IGame CreateGame();
    }
}
