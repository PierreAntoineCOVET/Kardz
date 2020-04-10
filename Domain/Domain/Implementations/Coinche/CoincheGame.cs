using Domain.Domain.Interfaces;
using Domain.Domain.Services;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Tools;
using Newtonsoft.Json;
using Repositories.EventStoreEntities;
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
    internal class CoincheGame : IGame
    {
        /// <summary>
        /// Game's Id.
        /// </summary>
        public Guid Id { get; private set; }

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
        /// Number of cards to give for each player.
        /// </summary>
        private const int NUMBER_OF_CARDS_PER_PLAYER = 8;

        /// <summary>
        /// Enshure that cards are shuffled only once.
        /// </summary>
        private SemaphoreSlim ShuffleCardsLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Constructor for JSON. 
        /// Use concrete <see cref="CoincheTeam"/> insted of <see cref="ITeam"/>.
        /// </summary>
        /// <param name="id">Id of the game.</param>
        /// <param name="teams">Teams for the game.</param>
        [JsonConstructor]
        public CoincheGame(Guid id, List<CoincheTeam> teams)
        {
            Id = id;
            _Teams = teams;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Game's Id.</param>
        public CoincheGame(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Add a list of players to the game.
        /// Split them into teams and give them number (id within the game).
        /// </summary>
        /// <param name="players">Players to add.</param>
        /// <remarks>Teams must already exists.</remarks>
        public void AddPlayers(IEnumerable<IPlayer> players)
        {
            if (_Teams.Count > 0)
                throw new GameException($"Game {Id} is already full");

            var playerCount = players.Count();
            if (playerCount != 4)
                throw new GameException($"Game {Enums.GamesEnum.Coinche} doesn't allow for {playerCount} player(s)");

            var coinchePlayers = players.Cast<CoinchePlayer>();
            var randomizedPlayers = RandomSorter.Randomize(coinchePlayers);
            AddTeam(0, randomizedPlayers.Take(2));
            AddTeam(1, randomizedPlayers.Skip(2).Take(2));
        }

        /// <summary>
        /// List of teams for the game.
        /// </summary>
        /// <param name="number">Team's number.</param>
        /// <param name="players">Team's players.</param>
        private void AddTeam(int number, IEnumerable<CoinchePlayer> players)
        {
            var list = players.ToList();

            var team = new CoincheTeam(number);
            team.AddPlayer(list[0]);
            team.AddPlayer(list[1]);

            _Teams.Add(team);
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
                throw new GameException($"Player {playerId} is not part of game {Id}");
            }

            int skip = NUMBER_OF_CARDS_PER_PLAYER * player.Number;
            int take = NUMBER_OF_CARDS_PER_PLAYER;
            return Cards.Skip(skip).Take(take);
        }

        /// <summary>
        /// If game is not playing the shuffle cards else do nothing.
        /// </summary>
        /// <returns></returns>
        public async Task ShuffleCards()
        {
            try
            {
                await ShuffleCardsLock.WaitAsync();

                if (!IsPlaying)
                {
                    var deck = CardsService.GetCardDeck(GamesEnum.Coinche);
                    Cards = deck.Shuffle();
                    IsPlaying = true;
                }

            }
            finally
            {
                ShuffleCardsLock.Release();
            }
        }
    }
}
