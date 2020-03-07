using Domain.Domain.Interfaces;
using Domain.Exceptions;
using Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheGame : IGame
    {
        private Guid _Id;
        public Guid Id => _Id;

        private List<CoincheTeam> _Teams = new List<CoincheTeam>(2);
        public IEnumerable<ITeam> Teams => _Teams;

        public CoincheGame(Guid id)
        {
            _Id = id;
        }

        public void AddPlayers(IEnumerable<Player> players)
        {
            if (_Teams.Count > 0)
                throw new GameAlreadyFullException(_Id);

            var playerCount = players.Count();
            if (playerCount != 4)
                throw new InvalidNumberOfPlayersForGameException(playerCount, Enums.GamesEnum.Coinche);

            var randomizedPlayers = RandomSorter.Randomize(players);
            AddTeam(1, randomizedPlayers.Take(2));
            AddTeam(2, randomizedPlayers.Skip(2).Take(2));
        }

        private void AddTeam(int number, IEnumerable<Player> players)
        {
            var list = players.ToList();

            var team = new CoincheTeam(number);
            team.AddPlayer(list[0]);
            team.AddPlayer(list[1]);

            _Teams.Add(team);
        }
    }
}
