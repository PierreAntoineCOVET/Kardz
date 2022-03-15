﻿using Domain.Enums;
using Domain.GamesLogic.Coinche;
using Domain.Tools.Serialization;
using MediatR;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Events
{
    /// <summary>
    /// Contract made event (bet or pass).
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(ContractMadeEvent), typeof(CoincheContractMadeMappingConverter))]
    public class ContractMadeEvent : IDomainEvent, INotification
    {
        /// <summary>
        /// Event's id.
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
        public ColorEnum? Color { get; set; }

        /// <summary>
        /// Contract succes threshold.
        /// </summary>
        public int? Value { get; set; }

        /// <summary>
        /// Number of players who passed the current contract.
        /// </summary>
        public int PassCounter { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="ContractMadeEvent"/>.
    /// </summary>
    public class CoincheContractMadeMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ContractMadeEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (ContractMadeEvent)value, options);
        }
    }
}
