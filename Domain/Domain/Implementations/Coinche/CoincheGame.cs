using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using Domain.Exceptions.Game;
using Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Implementations.Coinche
{
    public class CoincheGame : IGame
    {
        public Guid Id { get; private set; }

        private List<CoincheTeam> _Teams = new List<CoincheTeam>(2);
        public IEnumerable<ITeam> Teams => _Teams;

        private IEnumerable<CardsEnum> ShuffledCards;

        private const int NUMBER_OF_CARDS_PER_PLAYER = 8;

        public CoincheGame(Guid id)
        {
            Id = id;
        }

        public void AddPlayers(IEnumerable<Player> players)
        {
            if (_Teams.Count > 0)
                throw new GameCreationException($"Game {Id} is already full");

            var playerCount = players.Count();
            if (playerCount != 4)
                throw new GameCreationException($"Game {Enums.GamesEnum.Coinche} doesn't allow for {playerCount} player(s)");

            var randomizedPlayers = RandomSorter.Randomize(players);
            AddTeam(0, randomizedPlayers.Take(2));
            AddTeam(1, randomizedPlayers.Skip(2).Take(2));
        }

        private void AddTeam(int number, IEnumerable<Player> players)
        {
            var list = players.ToList();

            var team = new CoincheTeam(number);
            team.AddPlayer(0, list[0]);
            team.AddPlayer(1, list[1]);

            _Teams.Add(team);
        }

        public IEnumerable<CardsEnum> GetCardsForPlayer(Guid playerId)
        {
            var player = _Teams.SelectMany(t => t.Players).SingleOrDefault(p => p.Id == playerId);
            if(player == null)
            {
                throw new GameplayException($"Player {playerId} is not part of game {Id}");
            }

            if(ShuffledCards?.Any() ?? true)
            {
                var deck = CardsService.GetCardDeck(GamesEnum.Coinche);
                ShuffledCards = deck.Shuffle();
            }

            int skip = NUMBER_OF_CARDS_PER_PLAYER * player.Number;
            int take = NUMBER_OF_CARDS_PER_PLAYER;
            return ShuffledCards.Skip(skip).Take(take);
        }
    }
}
