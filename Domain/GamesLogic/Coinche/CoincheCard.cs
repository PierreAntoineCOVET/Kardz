using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.GamesLogic.Coinche
{
    [InterfaceResolver(typeof(CoincheGame), typeof(GameCreatedEvent), typeof(CoincheCardMappingConverter))]
    internal record CoincheCard : ICards
    {
        public CoincheCardColorsEnum Color { get; init; }

        public CoincheCardValuesEnum Value { get; init; }

        private CardsEnum Card;

        public CoincheCard() { }

        public CoincheCard(CardsEnum card)
        {
            Value = GetCardValue(card);
            Color = GetCardColor(card);
            Card = card;
        }

        /// <summary>
        /// Get the value of the card.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        /// <exception cref="GameException">If unknown.</exception>
        private CoincheCardValuesEnum GetCardValue(CardsEnum card)
        {
            switch (card)
            {
                case CardsEnum.SevenSpade:
                case CardsEnum.SevenClub:
                case CardsEnum.SevenHeart:
                case CardsEnum.SevenDiamond:
                    return CoincheCardValuesEnum.Seven;

                case CardsEnum.EightSpade:
                case CardsEnum.EightClub:
                case CardsEnum.EightHeart:
                case CardsEnum.EightDiamond:
                    return CoincheCardValuesEnum.Eight;

                case CardsEnum.NineSpade:
                case CardsEnum.NineClub:
                case CardsEnum.NineHeart:
                case CardsEnum.NineDiamond:
                    return CoincheCardValuesEnum.Nine;

                case CardsEnum.JackSpade:
                case CardsEnum.JackClub:
                case CardsEnum.JackHeart:
                case CardsEnum.JackDiamond:
                    return CoincheCardValuesEnum.Jack;

                case CardsEnum.QueenSpade:
                case CardsEnum.QueenClub:
                case CardsEnum.QueenHeart:
                case CardsEnum.QueenDiamond:
                    return CoincheCardValuesEnum.Queen;

                case CardsEnum.KingSpade:
                case CardsEnum.KingClub:
                case CardsEnum.KingHeart:
                case CardsEnum.KingDiamond:
                    return CoincheCardValuesEnum.King;

                case CardsEnum.TenSpade:
                case CardsEnum.TenClub:
                case CardsEnum.TenHeart:
                case CardsEnum.TenDiamond:
                    return CoincheCardValuesEnum.Ten;

                case CardsEnum.AsSpade:
                case CardsEnum.AsClub:
                case CardsEnum.AsHeart:
                case CardsEnum.AsDiamond:
                    return CoincheCardValuesEnum.As;

                default:
                    throw new GameException($"Unknown card {card}");
            }
        }

        /// <summary>
        /// Return the color of the given card.
        /// </summary>
        /// <remarks>
        /// The order of the comparaison is base and the enum and cannot be changed.
        /// </remarks>
        /// <param name="card"></param>
        /// <returns></returns>
        private CoincheCardColorsEnum GetCardColor(CardsEnum card)
        {
            if (card <= CardsEnum.KingSpade)
            {
                return CoincheCardColorsEnum.Spade;
            }

            if (card <= CardsEnum.KingClub)
            {
                return CoincheCardColorsEnum.Club;
            }

            if (card <= CardsEnum.KingDiamond)
            {
                return CoincheCardColorsEnum.Diamond;
            }

            return CoincheCardColorsEnum.Heart;
        }

        /// <summary>
        /// Return true if the card is winning over the list of opponents cards.
        /// </summary>
        /// <param name="opponentsCards">Opponents cards to beat.</param>
        /// <param name="askedColor">Ascked color at the start of the turn.</param>
        /// <param name="trumpColor">Trump color for the turn.</param>
        /// <returns></returns>
        public bool IsStrongerThan(IEnumerable<CoincheCard> opponentsCards, CoincheCardColorsEnum askedColor,
            CoincheCardColorsEnum trumpColor)
        {
            var cardIsTrump = Color == trumpColor;
            var opponentTrumps = opponentsCards.Where(c => c.Color == trumpColor).ToList();

            if (cardIsTrump && !opponentTrumps.Any())
            {
                return true;
            }

            if (!cardIsTrump && opponentTrumps.Any())
            {
                return false;
            }

            if(cardIsTrump && opponentTrumps.Any())
            {
                return opponentTrumps
                    .Any(o => o.Value > Value);
            }

            var cardIsAskedColor = Color == askedColor;
            var opponentAskedColor = opponentsCards.Where(c => c.Color == askedColor).ToList();

            if (cardIsAskedColor && !opponentAskedColor.Any())
            {
                return true;
            }

            if (!cardIsAskedColor && opponentAskedColor.Any())
            {
                return false;
            }

            return !opponentAskedColor.Any(o => o.Value > Value);
        }

        /// <summary>
        /// Transormf to a unique <see cref="CardsEnum"/> value.
        /// </summary>
        /// <returns></returns>
        public CardsEnum ToCardEnum()
        {
            return Card;
        }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="CoincheCard"/>.
    /// </summary>
    public class CoincheCardMappingConverter : JsonConverter<ICards>
    {
        public override ICards Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<CoincheCard>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, ICards value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (CoincheCard)value, options);
        }
    }
}
