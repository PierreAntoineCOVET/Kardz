using Domain.GamesLogic.Coinche;
using Domain.Interfaces;
using Domain.Tools.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Events
{
    /// <summary>
    /// Contract has failed to be established event.
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(ContractFailedEvent), typeof(ContractFailedEventMappingConverter))]
    public class ContractFailedEvent : IDomainEvent, ITurnTimerBasedEvent
    {
        /// <summary>
        /// Event ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Game Id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Dictionnary of all the cards attributed to each players.
        /// </summary>
        public IDictionary<Guid, IEnumerable<ICards>> CardsDistribution { get; set; }

        /// <summary>
        /// Player number for the dealer.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Player who needs to play.
        /// </summary>
        public int CurrentPlayerNumber { get; set; }

        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronise the time for each game player.
        /// </summary>
        public DateTimeOffset EndOfTurn { get; set; }

        /// <summary>
        /// Number of time the players passed the contract.
        /// </summary>
        public int ContractPassedCount { get; set; }

        /// <summary>
        /// Version of the aggregate after the event was applyed.
        /// </summary>
        public int AggregateVersion { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="ContractFailedEvent"/>.
    /// </summary>
    public class ContractFailedEventMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ContractFailedEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (ContractFailedEvent)value, options);
        }
    }
}
