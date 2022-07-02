using Domain.Events;
using Domain.Exceptions;
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
    /// Coinche team.
    /// </summary>
    [InterfaceResolver(typeof(CoincheGame), typeof(GameCreatedEvent), typeof(CoincheTeamMappingConverter))]
    internal class CoincheTeam : ITeam
    {
        public List<CoinchePlayer> Players { get; set; } = new List<CoinchePlayer>();
        /// <summary>
        /// List of players in the team.
        /// </summary>
        public IEnumerable<IPlayer> GetPlayers() => Players;

        /// <summary>
        /// Team's number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">Team's number.</param>
        public CoincheTeam(int number)
        {
            Number = number;
        }

        /// <summary>
        /// Add a player to the team and assign it's current number for the game.
        /// </summary>
        /// <param name="player">Player to add.</param>
        /// <remarks>Compute player's number based on the team's number and number of player in the team.</remarks>
        public void AddPlayer(IPlayer player)
        {
            if (Players.Count >= 2)
                throw new GameException($"Game {Enums.GamesEnum.Coinche} teams doesn't allow for {Players.Count} player(s)");

            if (!(player is CoinchePlayer))
                throw new InvalidCastException($"{nameof(player)} shoud be of type 'CoinchePlayer'");

            player.Number = Number + (Players.Count * 2);
            Players.Add((CoinchePlayer)player);
        }
    }

    /// <summary>
    /// Inteface mapping for serializing / deserializing <see cref="CoincheTeam"/>.
    /// </summary>
    public class CoincheTeamMappingConverter : JsonConverter<ITeam>
    {
        public override ITeam Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return System.Text.Json.JsonSerializer.Deserialize<CoincheTeam>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, ITeam value, JsonSerializerOptions options)
        {
            System.Text.Json.JsonSerializer.Serialize<CoincheTeam>(writer, (CoincheTeam)value, options);
        }
    }
}
