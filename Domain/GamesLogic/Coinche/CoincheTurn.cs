using Domain.Enums;
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
    internal class CoincheTurn
    {
        /// <summary>
        /// Return true if the turn is started.
        /// </summary>
        public bool IsStarted => Cards.Any();

        /// <summary>
        /// List of card played this turn.
        /// </summary>
        private IList<CoincheCard> Cards = new List<CoincheCard>();

        /// <summary>
        /// Get the playable cards from the list given the current turn's state.
        /// </summary>
        /// <param name="possibleCards">List of possible cards.</param>
        /// <returns>List of playable cards.</returns>
        public IEnumerable<CoincheCard> GetPlayableCards(IEnumerable<CoincheCard> possibleCards, CoincheCardColorsEnum trumColor)
        {
            if (!Cards.Any())
            {
                return possibleCards.ToList();
            }

            var askedColor = Cards.First().Color;
            var sameColorCards = possibleCards.Where(c => c.Color == askedColor).ToList();
            var trumps = possibleCards
                        .Where(c => c.Color == trumColor)
                        .ToList();

            if (sameColorCards.Any())
            {
                if(askedColor == trumColor)
                {
                    var trumpToBeat = Cards
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

            if (IsCurrentPlayerTeamWinning(askedColor, trumColor))
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
        private bool IsCurrentPlayerTeamWinning(CoincheCardColorsEnum askedColor, CoincheCardColorsEnum trumColor)
        {
            var numberOfPlayedCards = Cards.Count();

            if(numberOfPlayedCards == 1)
            {
                return false;
            }

            if(numberOfPlayedCards == 2)
            {
                return Cards[0].IsStrongerThan(new List<CoincheCard> { Cards[1] }, askedColor, trumColor);
            }

            return Cards[1].IsStrongerThan(new List<CoincheCard> { Cards[0], Cards[2] }, askedColor, trumColor);
        }
    }
}
