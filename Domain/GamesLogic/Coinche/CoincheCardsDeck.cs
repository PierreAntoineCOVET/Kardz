using Domain.Enums;
using Domain.Interfaces;
using Domain.Tools;
using System.Collections.Generic;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// List of classic 53 cards.
    /// </summary>
    internal class CoincheCardsDeck : ICardsDeck
    {
        /// <summary>
        /// List of cards that compose the deck.
        /// </summary>
        private IEnumerable<CardsEnum> Cards = new List<CardsEnum>
        {
            CardsEnum.AsSpade,
            CardsEnum.SevenSpade,
            CardsEnum.EightSpade,
            CardsEnum.NineSpade,
            CardsEnum.TenSpade,
            CardsEnum.JackSpade,
            CardsEnum.QueenSpade,
            CardsEnum.KingSpade,

            CardsEnum.AsClub,
            CardsEnum.SevenClub,
            CardsEnum.EightClub,
            CardsEnum.NineClub,
            CardsEnum.TenClub,
            CardsEnum.JackClub,
            CardsEnum.QueenClub,
            CardsEnum.KingClub,

            CardsEnum.AsDiamond,
            CardsEnum.SevenDiamond,
            CardsEnum.EightDiamond,
            CardsEnum.NineDiamond,
            CardsEnum.TenDiamond,
            CardsEnum.JackDiamond,
            CardsEnum.QueenDiamond,
            CardsEnum.KingDiamond,

            CardsEnum.AsHeart,
            CardsEnum.SevenHeart,
            CardsEnum.EightHeart,
            CardsEnum.NineHeart,
            CardsEnum.TenHeart,
            CardsEnum.JackHeart,
            CardsEnum.QueenHeart,
            CardsEnum.KingHeart,
        };

        /// <summary>
        /// Randomly shuffle the cards.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CardsEnum> Shuffle()
        {
            return RandomSorter.Randomize(Cards);
        }
    }
}
