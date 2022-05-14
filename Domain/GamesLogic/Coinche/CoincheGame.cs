using Domain.Configuration;
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
        private List<CoincheTeam> Teams = new List<CoincheTeam>(2);
        /// <summary>
        /// List of teams for the game.
        /// </summary>
        public IEnumerable<ITeam> GetTeams() => Teams;

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
        public int LastPlayerNumber => (CurrentPlayerNumber + 3) % 4;

        private CoincheContract Contract = new CoincheContract();
        /// <summary>
        /// Game's current contract.
        /// </summary>
        public IContract GetContract() => Contract;

        private DateTimeOffset _CurrentTurnTimeout;
        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronise the time for each game player.
        /// </summary>
        public DateTimeOffset CurrentTurnTimeout => _CurrentTurnTimeout;

        /// <summary>
        /// Get current player.
        /// </summary>
        private IPlayer CurrentPlayer => GetPlayerFromNumber(CurrentPlayerNumber);

        /// <summary>
        /// Current server configuration for the game.
        /// </summary>
        private CoincheConfiguration Configuration;

        /// <summary>
        /// Game current turn.
        /// </summary>
        private CoincheTurn CurrentTurn { get; init; } = new CoincheTurn();

        /// <summary>
        /// Reflexion construction for deserialization.
        /// </summary>
        public CoincheGame(CoincheConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        /// <param name="players">Game's players.</param>
        /// <param name="configuration">Game's server configuration.</param>
        public CoincheGame(Guid id, IEnumerable<IPlayer> players, CoincheConfiguration configuration)
        {
            Configuration = configuration;

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
                CardsDistribution = cardsDistribution.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Cast<ICards>()),
                CurrentDealer = 3,
                CurrentPlayerNumber = 0,
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
            Teams = @event.Teams.Cast<CoincheTeam>().ToList();
            ApplyCards(@event.CardsDistribution);
            CurrentDealer = @event.CurrentDealer;
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            _CurrentTurnTimeout = @event.EndOfTurn;
        }

        /// <summary>
        /// Apply the cards distribution to each players.
        /// </summary>
        /// <param name="cards"></param>
        private void ApplyCards(IDictionary<Guid, IEnumerable<ICards>> cards)
        {
            foreach (var player in Teams.SelectMany(t => t.Players))
            {
                player.Cards.AddRange(cards[player.Id].Cast<CoincheCard>());
            }
        }

        /// <summary>
        /// Aplly a contract to the game.
        /// </summary>
        /// <param name="color">Contract color, if needed.</param>
        /// <param name="value">Contract value, if needed.</param>
        /// <param name="player">Contract maker.</param>
        /// <param name="coinched">True if the players has coinched.</param>
        /// <returns>True if the contract applyed correctly, false if it failed.</returns>
        public void SetGameContract(CoincheCardColorsEnum? color, int? value, Guid player, bool coinched)
        {
            // TODO: CheckTurnNotExpired();

            if (player != CurrentPlayer.Id)
            {
                throw new GameException("Not your turn to bet.");
            }

            var contractNextState = Contract.GetNextState(color, value, coinched);

            switch(contractNextState)
            {
                case ContractStatesEnum.Failed:
                    RaiseContractFailedEvent();
                    break;

                case ContractStatesEnum.Valid:
                    RaiseContractValidEvent(color, value, coinched);
                    break;

                case ContractStatesEnum.Closed:
                    RaiseTurnStartedEvent();
                    break;

                default:
                    throw new GameException($"Unknown contract state value : {contractNextState}.");
            }
        }

        /// <summary>
        /// Raise a <see cref="TurnUpdatedEvent"/> to list the current player and the playable cards for this player.
        /// </summary>
        private void RaiseTurnStartedEvent()
        {
            var startingPlayer = GetPlayerRelative(CurrentDealer, 1);
            var currentPlayer = GetPlayerFromNumber(startingPlayer);
            
            var turnUpdatedEvent = new TurnUpdatedEvent
            {
                Id = Guid.NewGuid(),
                CurrentPlayerId = currentPlayer.Id,
                CurrentPlayerNumber = currentPlayer.Number,
                GameId = Id,
                PlayableCards = CurrentTurn.GetPlayableCards(currentPlayer.Cards, Contract.Color.Value)
            };

            RaiseEvent(turnUpdatedEvent);
        }

        /// <summary>
        /// Apply the <see cref="TurnUpdatedEvent"/> on the current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(TurnUpdatedEvent @event)
        {
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            _CurrentTurnTimeout = @event.EndOfTurn;
        }

        /// <summary>
        /// Raise a <see cref="ContractMadeEvent"/> to advange the game to the next stage of bets.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <param name="coinched"></param>
        private void RaiseContractValidEvent(CoincheCardColorsEnum? color, int? value, bool coinched)
        {
            var contractMadeEvent = Contract.GetContractMadeEvent(color, value, CurrentPlayer.Id, coinched);

            contractMadeEvent.GameId = Id;

            if (Contract.ShouldSkipNextPlayer(contractMadeEvent))
            {
                contractMadeEvent.CurrentPlayerNumber = GetPlayerRelative(CurrentPlayerNumber, 2);
            }
            else
            {
                contractMadeEvent.CurrentPlayerNumber = GetPlayerRelative(CurrentPlayerNumber, 1);
            }

            RaiseEvent(contractMadeEvent);
        }

        /// <summary>
        /// Apply <see cref="ContractMadeEvent"/> to current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ContractMadeEvent @event)
        {
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            _CurrentTurnTimeout = @event.EndOfTurn;

            Contract.Apply(@event);
        }

        /// <summary>
        /// Raise a <see cref="ContractFailedEvent"/> to reset the game cards and bets.
        /// </summary>
        private void RaiseContractFailedEvent()
        {
            var cardsDistribution = DealCards(Teams.SelectMany(t => t.Players));
            var nextDealer = GetPlayerRelative(CurrentDealer, 1);

            var contractFailedEvent = new ContractFailedEvent
            {
                Id = Guid.NewGuid(),
                GameId = Id,
                CardsDistribution = cardsDistribution.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Cast<ICards>()),
                CurrentDealer = nextDealer,
                CurrentPlayerNumber = GetPlayerRelative(nextDealer, 1),
                ContractPassedCount = 0,
            };

            RaiseEvent(contractFailedEvent);
        }

        /// <summary>
        /// Apply <see cref="ContractFailedEvent"/> to current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ContractFailedEvent @event)
        {
            ApplyCards(@event.CardsDistribution);

            CurrentDealer = @event.CurrentDealer;
            CurrentPlayerNumber = @event.CurrentPlayerNumber;
            _CurrentTurnTimeout = @event.EndOfTurn;

            Contract.Apply(@event);
        }

        /// <summary>
        /// Return the number of the player that is offset positions after the current.
        /// </summary>
        /// <param name="current">Current player number.</param>
        /// <param name="offset">Offset that is looked for.</param>
        /// <returns></returns>
        private int GetPlayerRelative(int current, int offset)
        {
            return (current + offset) % Consts.NUMBER_OF_PLAYERS_FOR_A_GAME;
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
        private Dictionary<Guid, IEnumerable<CoincheCard>> DealCards(IEnumerable<CoinchePlayer> players)
        {
            var deck = new CoincheCardsDeck();
            var cards = deck.Shuffle();

            var shuffledCards = new Dictionary<Guid, IEnumerable<CoincheCard>>();

            foreach (var player in players)
            {
                int skip = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER * player.Number;
                int take = Consts.COINCHE_NUMBER_OF_CARDS_PER_PLAYER;

                shuffledCards.Add(player.Id, cards.Skip(skip).Take(take));
            }

            return shuffledCards;
        }

        protected override void RaiseEvent(IDomainEvent @event)
        {
            if(@event is ITurnTimerBasedEvent turnTimeBasedEvent)
            {
                turnTimeBasedEvent.EndOfTurn = DateTimeOffset.Now.AddSeconds(Configuration.TimerLengthInSecond);
            }

            base.RaiseEvent(@event);
        }

        /// <summary>
        /// Return the <see cref="CoinchePlayer"/> object from the player number.
        /// </summary>
        /// <param name="playerNumber">Number.</param>
        /// <returns></returns>
        private CoinchePlayer GetPlayerFromNumber(int playerNumber) 
            => Teams.SelectMany(t => t.Players).Single(p => p.Number == playerNumber);
    }
}
