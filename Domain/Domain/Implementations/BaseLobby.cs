using Domain.Domain.Implementations;
using Domain.Domain.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Implementations
{
    public abstract class BaseLobby : ILobby
    {
        private protected ConcurrentDictionary<Guid, Player> PlayersInLobby = new ConcurrentDictionary<Guid, Player>();

        private protected ConcurrentDictionary<Guid, Player> PlayersLookingForGame = new ConcurrentDictionary<Guid, Player>();

        public abstract GamesEnum Game { get; }

        public void AddPlayer(Guid id)
        {
            if(!PlayersInLobby.TryAdd(id, new Player(id)))
                throw new PlayerAlreadyInLobbyException(id);
        }

        public int NumberOfPlayers => PlayersInLobby.Count;

        public void AddPlayerLookingForGame(Guid id)
        {
            if (!PlayersInLobby.Any(p => p.Key == id))
                throw new PlayerSearchGameWhileNotInLobbyException(id);

            if (!PlayersLookingForGame.TryAdd(id, PlayersInLobby[id]))
                throw new PlayerAlreadyLookingForAGameException(id);
        }

        public abstract bool CanStartGame();

        public abstract Task<IGame> CreateGame();
    }
}
