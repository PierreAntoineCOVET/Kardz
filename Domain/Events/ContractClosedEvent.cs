using Domain.Entities.ReadEntities;
using Domain.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Events
{
    /// <summary>
    /// Contract closed event (game is stating, no more bet;
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(ContractClosedEvent), typeof(ContractClosedEventMappingConverter))]
    public class ContractClosedEvent : IDomainEvent
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event's game id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Player who needs to play.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Choosen color.
        /// </summary>
        public CoincheCardColorsEnum? Color { get; set; }

        /// <summary>
        /// Contract succes threshold.
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Contract ower player's Id.
        /// </summary>
        public int? OwningTeamNumber { get; set; }

        /// <summary>
        /// Return true if the contract has been coinched.
        /// </summary>
        /// <returns></returns>
        public ContractCoincheStatesEnum CoincheState { get; set; }

        /// <summary>
        /// Number of time the players passed the contract.
        /// </summary>
        public int PassCounter { get; set; }

        /// <summary>
        /// Aggregate version.
        /// </summary>
        public int AggregateVersion { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="CoincheGame"/>.
    /// </summary>
    public class ContractClosedEventMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ContractClosedEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (ContractClosedEvent)value, options);
        }
    }
}
