using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.GamesLogic.Coinche
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
        /// Liste of shuffled cards.
        /// </summary>
        public IEnumerable<CardsEnum> Cards { get; private set; }

        /// <summary>
        /// Current dealer number.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Player whose turn it is.
        /// </summary>
        public int CurrentPlayer { get; set; }

        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronis the time for each game player.
        /// </summary>
        private DateTimeOffset TurnTimerBase { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CoincheGame()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        public CoincheGame(Guid id, IEnumerable<IPlayer> players)
        {
            if (id == default)
                throw new GameException($"Invalid game id {id}");

            if (players.Count() != Consts.NUMBER_OF_PLAYERS_FOR_A_GAME)
                throw new GameException($"Invalid number of players {players.Count()}");

            var teams = CreateTeams(players);

            DealCards(teams.SelectMany(t => t.Players));

            var createGameEvent = new GameCreatedEvent
            {
                Id = Guid.NewGuid(),
                GameId = id,
                Teams = teams,
                CurrentDealer = 3,
                CurrentPlayerNumber = 0,
                TurnTimerBase = DateTimeOffset.Now
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
            CurrentDealer = @event.CurrentDealer;
            CurrentPlayer = @event.CurrentPlayerNumber;
            TurnTimerBase = @event.TurnTimerBase;
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
        /// Deal random cards to the given players.
        /// </summary>
        /// <param name="gamePlayers">List of players to deal.</param>
        private void DealCards(IEnumerable<IPlayer> gamePlayers)
        {
            var deck = new CoincheCardsDeck();
            var cards = deck.Shuffle();

            //for (int i=0; i<gamePlayers.Count(); i++)
            foreach (var gamePlayer in gamePlayers)
            {
                int skip = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER * gamePlayer.Number;
                int take = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER;
                gamePlayer.Cards = cards.Skip(skip).Take(take);
            }
        }
    }
}
