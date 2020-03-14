using Domain.Domain.Interfaces;
using Domain.Exceptions.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheTeam : ITeam
    {
        private List<Player> _Players = new List<Player>();
        public IEnumerable<Player> Players => _Players;

        public int Number { get; private set; }

        public CoincheTeam(int number)
        {
            Number = number;
        }

        public void AddPlayer(int number, Player player)
        {
            if (_Players.Count >= 2)
                throw new GameCreationException($"Game {Enums.GamesEnum.Coinche} teams doesn't allow for {_Players.Count} player(s)");

            player.Number = (2 * Number) + number;

            _Players.Add(player);
        }
    }
}
