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
        public int LastPlayerNumber => (CurrentPlayerNumber + 3) % 4;

        /// <summary>
        /// Game's current contract.
        /// </summary>
        public IContract Contract { get; set; }

        private DateTimeOffset _CurrentTurnTimeout;
        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronise the time for each game player.
        /// </summary>
        public DateTimeOffset CurrentTurnTimeout => _CurrentTurnTimeout;

        /// <summary>
        /// Get current player.
        /// </summary>
        private IPlayer CurrentPlayer => Teams.SelectMany(t => t.Players).Single(p => p.Number == CurrentPlayerNumber);

        /// <summary>
        /// Current server configuration for the game.
        /// </summary>
        private CoincheConfiguration Configuration;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CoincheGame(CoincheConfiguration configuration)
        {
            Init(configuration);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        /// <param name="players">Game's players.</param>
        /// <param name="configuration">Game's server configuration.</param>
        public CoincheGame(Guid id, IEnumerable<IPlayer> players, CoincheConfiguration configuration)
        {
            Init(configuration);

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
            };

            RaiseEvent(createGameEvent);
        }

        /// <summary>
        /// Instanciate properties.
        /// </summary>
        private void Init(CoincheConfiguration configuration)
        {

            Configuration = configuration;
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
            _CurrentTurnTimeout = @event.EndOfTurn;
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
        /// <param name="coinched">True if the players has coinched.</param>
        /// <returns>True if the contract applyed correctly, false if it failed.</returns>
        public void SetGameContract(ColorEnum? color, int? value, Guid player, bool coinched)
        {
            //CheckTurnNotExpired();

            if (player != CurrentPlayer.Id)
            {
                throw new GameException("Not your turn to bet.");
            }

            var contractNextState = Contract.GetNextState(color, value, coinched);

            switch(contractNextState)
            {
                case ContractState.Failed:
                    RaiseContractFailedEvent();
                    break;

                case ContractState.Valid:
                    RaiseContractValidEvent(color, value, coinched);
                    break;

                case ContractState.Closed:
                    RaiseGameStartedEvent();
                    break;

                default:
                    throw new GameException($"Unknown contract state value : {contractNextState}.");
            }
        }

        private void RaiseGameStartedEvent()
        {
            // Todo: change contract state to closed.
            throw new NotImplementedException();
        }

        private void RaiseContractValidEvent(ColorEnum? color, int? value, bool coinched)
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

        private void RaiseContractFailedEvent()
        {
            var cardsDistribution = DealCards(Teams.SelectMany(t => t.Players));
            var nextDealer = GetPlayerRelative(CurrentDealer, 1);

            var contractFailedEvent = new ContractFailedEvent
            {
                Id = Guid.NewGuid(),
                GameId = Id,
                CardsDistribution = cardsDistribution,
                CurrentDealer = nextDealer,
                CurrentPlayerNumber = GetPlayerRelative(nextDealer, 1),
                ContractPassedCount = 0,
            };

            RaiseEvent(contractFailedEvent);
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
            _CurrentTurnTimeout = @event.EndOfTurn;

            Contract.Apply(@event);
        }

        /// <summary>
        /// Apply contract made event to current instance.
        /// </summary>
        /// <param name="event"></param>
        internal void Apply(ContractMadeEvent @event)
        {
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

        protected override void RaiseEvent(IDomainEvent @event)
        {
            if(@event is ITurnTimerBasedEvent turnTimeBasedEvent)
            {
                turnTimeBasedEvent.EndOfTurn = DateTimeOffset.Now.AddSeconds(Configuration.TimerLengthInSecond);
            }

            base.RaiseEvent(@event);
        }
    }
}
