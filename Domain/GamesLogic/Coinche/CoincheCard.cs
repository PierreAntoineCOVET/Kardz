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
    [InterfaceResolver(typeof(CoincheGame), typeof(ContractFailedEvent), typeof(CoincheCardMappingConverter))]
    internal class CoincheCard : ICards
    {
        public CoincheCardColorsEnum Color { get; init; }

        public CoincheCardValuesEnum Value { get; init; }

        public CoincheCard() { }

        public CoincheCard(CardsEnum card)
        {
            Value = GetCardValue(card);
            Color = GetCardColor(card);
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
        public bool IsStrongerThan(IEnumerable<CoincheCard> opponentsCards, CoincheCardColorsEnum askedColor, CoincheCardColorsEnum trumpColor)
        {
            var cardIsTrump = Color == trumpColor;
            var opponentIsTurm = opponentsCards.Any(c => c.Color == trumpColor);

            if (cardIsTrump && !opponentIsTurm)
            {
                return true;
            }

            if (!cardIsTrump && opponentIsTurm)
            {
                return false;
            }

            var cardIsAskedColor = Color == askedColor;
            var opponentIsAskedColor = opponentsCards.Any(c => c.Color == askedColor);

            if (cardIsAskedColor && !opponentIsAskedColor)
            {
                return true;
            }

            if (!cardIsAskedColor && opponentIsAskedColor)
            {
                return false;
            }

            return opponentsCards.Any(o => o.Value > Value);
        }

        /// <summary>
        /// Transormf to a unique <see cref="CardsEnum"/> value.
        /// </summary>
        /// <returns></returns>
        public CardsEnum ToCardEnum()
        {
            var colorSize = 13;

            var colorMultiplier = Color switch
            {
                CoincheCardColorsEnum.Spade => 0,
                CoincheCardColorsEnum.Club => 1,
                CoincheCardColorsEnum.Diamond => 2,
                CoincheCardColorsEnum.Heart => 3,
                _ => throw new GameException($"Invalid card color {Color}")
            };

            var value = Value switch
            {
                CoincheCardValuesEnum.As => 0,
                CoincheCardValuesEnum.Seven => 6,
                CoincheCardValuesEnum.Eight => 7,
                CoincheCardValuesEnum.Nine => 8,
                CoincheCardValuesEnum.Ten => 9,
                CoincheCardValuesEnum.Jack => 10,
                CoincheCardValuesEnum.Queen => 11,
                CoincheCardValuesEnum.King => 12,
                _ => throw new GameException($"Invalid card value {Value}")
            };

            return (CardsEnum)(colorSize * colorMultiplier) + value;
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
