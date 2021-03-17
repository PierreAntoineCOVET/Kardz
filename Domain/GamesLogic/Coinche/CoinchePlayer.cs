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
        public Guid Id { get; private set; }

        /// <summary>
        /// Player's number in the game.
        /// Used for cards distribution.
        /// </summary>
        /// <remarks>Can be replaced with id ?</remarks>
        public int Number { get; set; }

        /// <summary>
        /// List of cards.
        /// </summary>
        public IEnumerable<CardsEnum> Cards { get; set; }

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
            return JsonSerializer.Deserialize<CoinchePlayer>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IPlayer value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
