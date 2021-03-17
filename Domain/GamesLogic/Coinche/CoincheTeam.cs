using Domain.Exceptions;
using Domain.Interfaces;
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
    internal class CoincheTeam : ITeam
    {
        private List<CoinchePlayer> _Players = new List<CoinchePlayer>();
        /// <summary>
        /// List of players in the team.
        /// </summary>
        public IEnumerable<IPlayer> Players => _Players;

        /// <summary>
        /// Team's number.
        /// </summary>
        public int Number { get; set; }

        public CoincheTeam()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="number">Team's number.</param>
        public CoincheTeam(int number)
        {
            Number = number;
        }

        /// <summary>
        /// Add a player to the team.
        /// </summary>
        /// <param name="player">Player to add.</param>
        /// <remarks>Compute player's number based on the team's number and number of player in the team.</remarks>
        public void AddPlayer(IPlayer player)
        {
            if (_Players.Count >= 2)
                throw new GameException($"Game {Enums.GamesEnum.Coinche} teams doesn't allow for {_Players.Count} player(s)");

            if (!(player is CoinchePlayer))
                throw new InvalidCastException($"{nameof(player)} shoud be of type 'CoinchePlayer'");

            player.Number = Number * 2 + _Players.Count;
            _Players.Add((CoinchePlayer)player);
        }
    }

    public class TeamMappingConverter : JsonConverter<ITeam>
    {
        public override ITeam Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var team = new CoincheTeam();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.GetString() == nameof(CoincheTeam.Number))
                    {
                        team.Number = GetNumber(ref reader);
                    }
                    else if (reader.GetString() == nameof(CoincheTeam.Players))
                    {
                        ((List<CoinchePlayer>)team.Players).AddRange(GetPlayers(ref reader, options).Cast<CoinchePlayer>());
                    }
                }
            }

            return team;
            //return JsonSerializer.Deserialize<CoincheTeam>(ref reader, options);
        }

        private IEnumerable<IPlayer> GetPlayers(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var players = new List<IPlayer>();

            while (reader.Read())
            {
                if(reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }
                else if(reader.TokenType == JsonTokenType.StartObject)
                {
                    players.Add(JsonSerializer.Deserialize<IPlayer>(ref reader, options));
                }
            }

            return players;
        }

        private int GetNumber(ref Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, ITeam value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
