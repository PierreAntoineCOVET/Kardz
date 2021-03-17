using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Coinche player.
    /// </summary>
    internal class CoinchePlayer : IPlayer
    {
        /// <summary>
        /// Player's Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Player's number in the game.
        /// Used for cards distribution.
        /// </summary>
        /// <remarks>Can be replaced with id ?</remarks>
        public int Number { get; set; }

        /// <summary>
        /// List of cards.
        /// </summary>
        public IEnumerable<CardsEnum> Cards { get; set; } = new List<CardsEnum>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Player's Id.</param>
        public CoinchePlayer(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CoinchePlayer()
        {
        }
    }

    public class PlayerMappingConverter : JsonConverter<IPlayer>
    {
        public override IPlayer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var player = new CoinchePlayer();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.GetString() == nameof(CoinchePlayer.Id))
                    {
                        player.Id = GetId(ref reader);
                    }
                    else if (reader.GetString() == nameof(CoinchePlayer.Number))
                    {
                        player.Number = GetNumber(ref reader);
                    }
                    else if(reader.GetString() == nameof(CoinchePlayer.Cards))
                    {
                        ((List<CardsEnum>)player.Cards).AddRange(GetCards(ref reader));
                    }
                }
            }

            return player;
        }

        public Guid GetId(ref Utf8JsonReader reader)
        {
            reader.Read();
            return Guid.Parse(reader.GetString());
        }

        public int GetNumber(ref Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetInt32();
        }

        public IEnumerable<CardsEnum> GetCards(ref Utf8JsonReader reader)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected StartArray token");

            var cards = new List<CardsEnum>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                cards.Add((CardsEnum)reader.GetInt32());
            }

            return cards;
        }

        public override void Write(Utf8JsonWriter writer, IPlayer value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
