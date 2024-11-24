using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Manage all operations for the current turn and its state.
    /// </summary>
    internal class CoincheTake
    {
        /// <summary>
        /// Return true if the turn is started.
        /// </summary>
        public bool IsStarted => CurrentFold.Any() || PreviousFold.Any();

        /// <summary>
        /// List of card played this turn.
        /// </summary>
        private List<CoincheCard> CurrentFold = new List<CoincheCard>();

        /// <summary>
        /// List of cards from the previous turns.
        /// </summary>
        private List<CoincheCard> PreviousFold = new List<CoincheCard>();

        public TakeChangedEvent StartNewTake(IEnumerable<CoincheCard> possibleCards, CoincheCardColorsEnum trumColor)
        {
            var currentFold = new List<CoincheCard>();

            return new TakeChangedEvent
            {
                Id = Guid.NewGuid(),
                CurrentFold = currentFold,
                PreviousFold = new List<CoincheCard>()
            };
        }

        /// <summary>
        /// Get the playable cards from the list given the current turn's state.
        /// </summary>
        /// <param name="possibleCards">List of possible cards.</param>
        /// <returns>List of playable cards.</returns>
        public IEnumerable<CoincheCard> GetPlayableCards(IEnumerable<CoincheCard> possibleCards, CoincheCardColorsEnum trumpColor)
        {
            if (!CurrentFold.Any())
            {
                return possibleCards.ToList();
            }

            var askedColor = CurrentFold.First().Color;
            var sameColorCards = possibleCards.Where(c => c.Color == askedColor).ToList();

            if (sameColorCards.Any())
            {
                if(askedColor == trumpColor)
                {
                    return GetBetterOrAll(possibleCards, CurrentFold, askedColor);
                }
                else
                {
                    return sameColorCards;
                }
            }

            if (IsCurrentPlayerTeamWinning(askedColor, trumpColor, CurrentFold))
            {
                return possibleCards.ToList();
            }

            var hasTrumps = possibleCards.Any(c => c.Color == trumpColor);
            if (hasTrumps)
            {
                return GetBetterOrAll(possibleCards, CurrentFold, trumpColor);
            }

            return possibleCards.ToList();
        }

        /// <summary>
        /// Get the cards of color <paramref name="color"/> from <paramref name="candidates"/> that can beat the best card of color <paramref name="color"/>
        /// from <paramref name="currentFold"/>.
        /// Return all <paramref name="candidates"/> if no higher values are found.
        /// </summary>
        /// <param name="candidates">Possible cards.</param>
        /// <param name="currentFold">Card to beat.</param>
        /// <param name="color">Filter color.</param>
        /// <returns></returns>
        private static IEnumerable<CoincheCard> GetBetterOrAll(IEnumerable<CoincheCard> candidates, IEnumerable<CoincheCard> currentFold, 
            CoincheCardColorsEnum color)
        {
            var tumpValueComparer = new CoincheCardTrumpOrderValueComparer();

            var bestOfColorQuery = currentFold
                .Where(c => c.Color == color);

            bestOfColorQuery = bestOfColorQuery.OrderByDescending(c => c.Value, tumpValueComparer);

            var bestOfColor = bestOfColorQuery.FirstOrDefault();

            var candidatesOfColor = candidates.Where(c => c.Color == color ).ToList();

            if (bestOfColor == null)
            {
                return candidatesOfColor;
            }

            var betterCandidates = candidatesOfColor
                .Where(c => tumpValueComparer.Compare(c.Value, bestOfColor.Value) > 0)
                .ToList();

            return betterCandidates.Any()
                ? betterCandidates
                : candidatesOfColor;
        }

        /// <summary>
        /// Return true if the current player's team is winning the turn (for the moment).
        /// </summary>
        /// <param name="askedColor">Ascked color at the start of the turn.</param>
        /// <returns></returns>
        private static bool IsCurrentPlayerTeamWinning(CoincheCardColorsEnum askedColor, CoincheCardColorsEnum trumColor, List<CoincheCard> currentFold)
        {
            var numberOfPlayedCards = currentFold.Count();

            if(numberOfPlayedCards == 1)
            {
                return false;
            }

            if(numberOfPlayedCards == 2)
            {
                return currentFold[0].IsStrongerThan(new List<CoincheCard> { currentFold[1] }, askedColor, trumColor);
            }

            return currentFold[1].IsStrongerThan(new List<CoincheCard> { currentFold[0], currentFold[2] }, askedColor, trumColor);
        }

        public TakeChangedEvent Play(CoincheCard playedCard, IEnumerable<CoincheCard> possibleCards)
        {
            if (!IsCardPlayable(playedCard, possibleCards))
            {
                throw new GameException($"Cannot play card {playedCard.Card}");
            }

            var takeChangedEvent = new TakeChangedEvent();

            if (CurrentFold.Count < 3)
            {
                var newCurrentFold = new List<CoincheCard>(CurrentFold)
                {
                    playedCard
                };

                takeChangedEvent.Id = Guid.NewGuid();
                takeChangedEvent.CurrentFold = newCurrentFold;
                takeChangedEvent.PreviousFold = PreviousFold;
            }
            else
            {
                // TODO: close the take, score points , ext.
            }

            return takeChangedEvent;
        }

        private bool IsCardPlayable(CoincheCard candidate, IEnumerable<CoincheCard> possibleCards)
        {
            var cardIsValid = possibleCards.Any(c => c == candidate);

            var cardHasNotBeenPlayed = !PreviousFold.Any(c => c == candidate);

            return cardIsValid && cardHasNotBeenPlayed;
        }

        internal void Apply(TakeChangedEvent @event)
        {
            CurrentFold = @event.CurrentFold.Cast<CoincheCard>().ToList();
            PreviousFold = @event.PreviousFold.Cast<CoincheCard>().ToList();
        }
    }
}
