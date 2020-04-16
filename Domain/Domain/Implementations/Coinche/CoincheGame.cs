using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Tools;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Domain.Implementations.Coinche
{
    /// <summary>
    /// Coinche game domain class.
    /// </summary>
    internal class CoincheGame : AggregateBase, IGame
    {
        private List<CoincheTeam> _Teams = new List<CoincheTeam>(2);
        /// <summary>
        /// List of teams for the game.
        /// </summary>
        public IEnumerable<ITeam> Teams => _Teams;

        /// <summary>
        /// True if the game is playing (cant shuffle cards).
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Liste of shuffled cards.
        /// </summary>
        public IEnumerable<CardsEnum> Cards { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        public CoincheGame(Guid id, IEnumerable<IPlayer> players)
        {
            if (id == default)
                throw new GameException($"Invalid game id {id}");

            if(players.Count() != Consts.NUMBER_OF_PLAYERS_FOR_A_GAME)
                throw new GameException($"Invalid number of players {players.Count()}");

            var createGameEvent = new GameCreatedEvent
            {
                Id = Guid.NewGuid(),
                GameId = id,
                Teams = CreateTeams(players)
            };

            RaiseEvent(createGameEvent);
        }

        /// <summary>
        /// Apply <see cref="GameCreatedEvent"/>.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(GameCreatedEvent @event)
        {
            Id = @event.GameId;
            _Teams = @event.Teams.Cast<CoincheTeam>().ToList();
        }

        /// <summary>
        /// Add a list of players to teams.
        /// </summary>
        /// <param name="players">Players to add.</param>
        private IEnumerable<CoincheTeam> CreateTeams(IEnumerable<IPlayer> players)
        {
            var teams = new List<CoincheTeam>(2);

            var coinchePlayers = players.Cast<CoinchePlayer>();
            var randomizedPlayers = RandomSorter.Randomize(coinchePlayers);

            teams.Add(CreateTeam(0, randomizedPlayers.Take(2)));
            teams.Add(CreateTeam(1, randomizedPlayers.Skip(2).Take(2)));

            return teams;
        }

        /// <summary>
        /// Create a team and add its players.
        /// </summary>
        /// <param name="number">Team's number.</param>
        /// <param name="players">Team's players.</param>
        private CoincheTeam CreateTeam(int number, IEnumerable<CoinchePlayer> players)
        {
            var list = players.ToList();

            var team = new CoincheTeam(number);
            team.AddPlayer(list[0]);
            team.AddPlayer(list[1]);

            return team;
        }

        /// <summary>
        /// Get cards from radomisez list for a player (based on the player number).
        /// </summary>
        /// <param name="playerId">Id of the player.</param>
        /// <returns>List of cards for the player.</returns>
        public IEnumerable<CardsEnum> GetCardsForPlayer(Guid playerId)
        {
            var player = _Teams.SelectMany(t => t.Players).SingleOrDefault(p => p.Id == playerId);
            if(player == null)
            {
                throw new GameException($"Player {playerId} is not part of game {Id}.");
            }

            int skip = Consts.NUMBER_OF_CARDS_PER_PLAYER * player.Number;
            int take = Consts.NUMBER_OF_CARDS_PER_PLAYER;
            return Cards.Skip(skip).Take(take);
        }

        /// <summary>
        /// If game is not playing the shuffle cards else do nothing.
        /// </summary>
        /// <returns></returns>
        public void ShuffleCards()
        {
            if (IsPlaying)
                throw new GameException($"Shuffle cards of game {Id} while it's playing.");

            var deck = new CoincheCardsDeck();
            var cards = deck.Shuffle();

            var shuffleCardsEvent = new ShuffledCardsEvent
            {
                Id = Guid.NewGuid(),
                ShuffledCards = cards
            };
            RaiseEvent(shuffleCardsEvent);
        }

        /// <summary>
        /// Apply <see cref="ShuffledCardsEvent"/>.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ShuffledCardsEvent @event)
        {
            Cards = @event.ShuffledCards;
        }
    }
}
