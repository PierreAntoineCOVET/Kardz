using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
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

        private List<CoincheCard> PlayableCards = new List<CoincheCard>();

        public TakeChangedEvent StartNewTake(IEnumerable<CoincheCard> possibleCards, CoincheCardColorsEnum trumColor)
        {
            var currentFold = new List<CoincheCard>();

            return new TakeChangedEvent
            {
                CurrentFold = currentFold,
                PreviousFold = new List<CoincheCard>(),
                PlayableCards = GetPlayableCards(possibleCards, trumColor, currentFold)
            };
        }

        /// <summary>
        /// Get the playable cards from the list given the current turn's state.
        /// </summary>
        /// <param name="possibleCards">List of possible cards.</param>
        /// <returns>List of playable cards.</returns>
        private static IEnumerable<CoincheCard> GetPlayableCards(IEnumerable<CoincheCard> possibleCards, CoincheCardColorsEnum trumColor, List<CoincheCard> currentFold)
        {
            if (!currentFold.Any())
            {
                return possibleCards.ToList();
            }

            var askedColor = currentFold.First().Color;
            var sameColorCards = possibleCards.Where(c => c.Color == askedColor).ToList();
            var trumps = possibleCards
                        .Where(c => c.Color == trumColor)
                        .ToList();

            if (sameColorCards.Any())
            {
                if(askedColor == trumColor)
                {
                    var trumpToBeat = currentFold
                        .Where(c => c.Color == trumColor)
                        .OrderByDescending(c => c.Value)
                        .First();

                    var betterTrumps = trumps
                        .Where(c => c.Value > trumpToBeat.Value)
                        .ToList();

                    return betterTrumps.Any()
                        ? betterTrumps
                        : trumps;
                }
                else
                {
                    return sameColorCards;
                }
            }

            if (IsCurrentPlayerTeamWinning(askedColor, trumColor, currentFold))
            {
                return possibleCards.ToList();
            }

            if (trumps.Any())
            {
                return trumps;
            }

            return possibleCards.ToList();
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

        public TakeChangedEvent Play(CoincheCard playedCard, IEnumerable<CoincheCard> nextPlayerCandidate, CoincheCardColorsEnum trumColor)
        {
            if (!IsCardPlayable(playedCard))
            {
                throw new GameException($"Cannot play card {playedCard.ToCardEnum()}");
            }

            var takeChangedEvent = new TakeChangedEvent();

            if (CurrentFold.Count < 3)
            {
                var newCurrentFold = new List<CoincheCard>(CurrentFold);
                newCurrentFold.Add(playedCard);

                takeChangedEvent.PlayableCards = GetPlayableCards(nextPlayerCandidate, trumColor, newCurrentFold);
                takeChangedEvent.CurrentFold = newCurrentFold;
                takeChangedEvent.PreviousFold = PreviousFold;
            }
            else
            {
                // close the take, score points , ext.
            }

            return takeChangedEvent;
        }

        private bool IsCardPlayable(CoincheCard candidate)
        {
            var cardIsValid = PlayableCards.Any(c => c == candidate);

            var cardHasNotBeenPlayed = !PreviousFold.Any(c => c == candidate);

            return cardIsValid && cardHasNotBeenPlayed;
        }

        public void Apply(TakeChangedEvent @event)
        {
            CurrentFold = @event.CurrentFold.Cast<CoincheCard>().ToList();
            PreviousFold = @event.PreviousFold.Cast<CoincheCard>().ToList();
            PlayableCards = @event.PlayableCards.Cast<CoincheCard>().ToList();
        }
    }
}
