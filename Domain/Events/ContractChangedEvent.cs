﻿using Domain.Enums;
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
    /// Contract made event (bet or pass).
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(ContractChangedEvent), typeof(CoincheContractMadeMappingConverter))]
    public class ContractChangedEvent : IDomainEvent, ITurnTimerBasedEvent
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
        /// Dictionnary of all the cards attributed to each players.
        /// </summary>
        public IDictionary<Guid, IEnumerable<ICards>> CardsDistribution { get; set; }

        /// <summary>
        /// Player number for the dealer.
        /// </summary>
        public int CurrentDealer { get; set; }

        /// <summary>
        /// Contract ower player's Id.
        /// </summary>
        public int? OwningTeamNumber { get; set; }

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
        /// Number of players who passed the current contract.
        /// </summary>
        public int PassCounter { get; set; }

        /// <summary>
        /// Time at wich the turn time for all players will start.
        /// Used to synchronise the time for each game player.
        /// </summary>
        public DateTimeOffset EndOfTurn { get; set; }

        /// <summary>
        /// Return true if the contract has been coinched.
        /// </summary>
        /// <returns></returns>
        public ContractCoincheStatesEnum CoincheState { get; set; }

        /// <summary>
        /// Current state of the contract.
        /// </summary>
        public ContractStatesEnum ContractState { get; set; }

        /// <summary>
        /// Version of the aggregate after the event was applyed.
        /// </summary>
        public int AggregateVersion { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="ContractChangedEvent"/>.
    /// </summary>
    public class CoincheContractMadeMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ContractChangedEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (ContractChangedEvent)value, options);
        }
    }
}
