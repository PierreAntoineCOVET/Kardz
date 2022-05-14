using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.GamesLogic.Coinche
{
    /// <summary>
    /// Coinche player.
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(GameCreatedEvent), typeof(CoinchePlayerMappingConverter))]
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

        [JsonIgnore]
        public List<CoincheCard> Cards { get; set; } = new List<CoincheCard>();
        /// <summary>
        /// List of cards.
        /// </summary>
        public IEnumerable<ICards> GetCards() => Cards;

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

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="CoinchePlayer"/>.
    /// </summary>
    public class CoinchePlayerMappingConverter : JsonConverter<IPlayer>
    {
        public override IPlayer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<CoinchePlayer>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IPlayer value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (CoinchePlayer)value, options);
        }
    }
}
