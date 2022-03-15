﻿using Domain.Enums;
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
        /// Current player number.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Return the number of the player before current.
        /// </summary>
        public int LastPlayerNumber => CurrentPlayerNumber + 3 % 4;

        /// <summary>
        /// Game's current contract.
        /// </summary>
        public IContract Contract { get; set; }

        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronise the time for each game player.
        /// </summary>
        private DateTimeOffset TurnTimerBase;

        /// <summary>
        /// Get current player.
        /// </summary>
        private IPlayer CurrentPlayer => Teams.SelectMany(t => t.Players).Single(p => p.Number == CurrentPlayerNumber);

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CoincheGame()
        {
            Init();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        public CoincheGame(Guid id, IEnumerable<IPlayer> players)
        {
            Init();

            if (id == default)
                throw new GameException($"Invalid game id {id}");

            if (players.Count() != Consts.NUMBER_OF_PLAYERS_FOR_A_GAME)
                throw new GameException($"Invalid number of players {players.Count()}");

            var teams = CreateTeams(players);

            var cardsDistribution = DealCards(teams.SelectMany(t => t.Players));

            var createGameEvent = new GameCreatedEvent
            {
                Id = Guid.NewGuid(),
                GameId = id,
                Teams = teams,
                CardsDistribution = cardsDistribution,
                CurrentDealer = 3,
                CurrentPlayerNumber = 0,
                TurnTimerBase = DateTimeOffset.Now
            };

            RaiseEvent(createGameEvent);
        }

        /// <summary>
        /// Instanciate properties.
        /// </summary>
        private void Init()
        {
            Contract = new CoincheContract();
        }

        /// <summary>
        /// Apply <see cref="GameCreatedEvent"/>.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(GameCreatedEvent @event)
        {
            Id = @event.GameId;
            _Teams = @event.Teams.Cast<CoincheTeam>().ToList();
            ApplyCards(@event.CardsDistribution);
            CurrentDealer = @event.CurrentDealer;
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            TurnTimerBase = @event.TurnTimerBase;
        }

        /// <summary>
        /// Apply the cards distribution to each players.
        /// </summary>
        /// <param name="cards"></param>
        private void ApplyCards(IDictionary<Guid, IEnumerable<CardsEnum>> cards)
        {
            foreach (var player in Teams.SelectMany(t => t.Players))
            {
                player.Cards = cards[player.Id];
            }
        }

        /// <summary>
        /// Aplly a contract to the game.
        /// </summary>
        /// <param name="color">Contract color, if needed.</param>
        /// <param name="value">Contract value, if needed.</param>
        /// <param name="player">Contract maker.</param>
        /// <param name="game">Contract's game.</param>
        /// <param name="coinched">True if the players has coinched.</param>
        /// <returns>True if the contract applyed correctly, false if it failed.</returns>
        public bool SetGameContract(ColorEnum? color, int? value, Guid player, Guid game, bool coinched)
        {
            if (game != Id)
            {
                throw new GameException("Trying to apply contract to the wrong game.");
            }

            if (player != CurrentPlayer.Id)
            {
                throw new GameException("Not your turn to bet.");
            }

            if (Contract.IsContractFailed(color, value))
            {
                var cardsDistribution = DealCards(Teams.SelectMany(t => t.Players));
                var nextDealer = GetNext(CurrentDealer);

                var contractFailedEvent = new ContractFailedEvent
                {
                    Id = Guid.NewGuid(),
                    GameId = game,
                    CardsDistribution = cardsDistribution,
                    CurrentDealer = nextDealer,
                    CurrentPlayerNumber = GetNext(nextDealer),
                };

                RaiseEvent(contractFailedEvent);

                return false;
            }
            else
            {
                var contractMadeEvent = Contract.GetContractMadeEvent(color, value);

                contractMadeEvent.GameId = game;
                contractMadeEvent.CurrentPlayerNumber = GetNext(CurrentPlayerNumber);

                RaiseEvent(contractMadeEvent);

                return true;
            }
        }

        /// <summary>
        /// Apply contract failed event to current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ContractFailedEvent @event)
        {
            ApplyCards(@event.CardsDistribution);

            CurrentDealer = @event.CurrentDealer;
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
        }

        /// <summary>
        /// Apply contract made event to current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ContractMadeEvent @event)
        {
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            Contract.Apply(@event);
        }

        /// <summary>
        /// Return the next number modulo Consts.NUMBER_OF_PLAYERS_FOR_A_GAME;
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private int GetNext(int current)
        {
            return (current + 1) % Consts.NUMBER_OF_PLAYERS_FOR_A_GAME;
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
        /// <param name="players">List of players to deal.</param>
        private Dictionary<Guid, IEnumerable<CardsEnum>> DealCards(IEnumerable<IPlayer> players)
        {
            var deck = new CoincheCardsDeck();
            var cards = deck.Shuffle();

            var shuffledCards = new Dictionary<Guid, IEnumerable<CardsEnum>>();

            foreach (var player in players)
            {
                int skip = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER * player.Number;
                int take = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER;

                shuffledCards.Add(player.Id, cards.Skip(skip).Take(take));
            }

            return shuffledCards;
        }
    }
}
