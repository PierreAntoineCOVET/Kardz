using Domain.Domain.Interfaces;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheTeam : ITeam
    {
        private List<Player> _Players = new List<Player>();
        public IEnumerable<Player> Players => _Players;

        private int _Number;
        public int Number => _Number;

        public CoincheTeam(int number)
        {
            _Number = number;
        }

        public void AddPlayer(Player player)
        {
            if (_Players.Count >= 2)
                throw new InvalidNumberOfPlayersForGameTeamException(_Players.Count, Enums.GamesEnum.Coinche);

            _Players.Add(player);
        }
    }
}
