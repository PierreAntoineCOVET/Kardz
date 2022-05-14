using Domain.Entities.ReadEntities;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Events
{
    /// <summary>
    /// Raised each time the game's turn need to be updated (game started or player played).
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(TurnUpdatedEvent), typeof(TurnUpdatedMappingConverter))]
    public class TurnUpdatedEvent : IDomainEvent, INotification, ITurnTimerBasedEvent
    {
        /// <summary>
        /// Event Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Game Id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Current player Id.
        /// </summary>
        public Guid CurrentPlayerId { get; set; }

        /// <summary>
        /// Current player number.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// List of currrently playable cards.
        /// </summary>
        public IEnumerable<ICards> PlayableCards { get; set; }

        /// <summary>
        /// Current turn expiration date
        /// </summary>
        public DateTimeOffset EndOfTurn { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="TurnUpdatedEvent"/>.
    /// </summary>
    public class TurnUpdatedMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TurnUpdatedEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (TurnUpdatedEvent)value, options);
        }
    }
}
