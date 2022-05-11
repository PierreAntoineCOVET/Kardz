using Domain.Enums;
using Domain.GamesLogic.Coinche;
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
    /// Game created event.
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(GameCreatedEvent), typeof(CoincheGameCreatedMappingConverter))]
    public class GameCreatedEvent : IDomainEvent, INotification, ITurnTimerBasedEvent
    {
        /// <summary>
        /// Event's id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Game's id.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Game's teams.
        /// </summary>
        public IEnumerable<ITeam> Teams { get; set; }

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
        /// Used to synchronis the time for each game player.
        /// </summary>
        public DateTimeOffset EndOfTurn { get; set; }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="GameCreatedEvent"/>.
    /// </summary>
    public class CoincheGameCreatedMappingConverter : JsonConverter<IDomainEvent>
    {
        public override IDomainEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<GameCreatedEvent>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, IDomainEvent value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize(writer, (GameCreatedEvent)value, options);
        }
    }
}
