using Domain.Interfaces;
using System.Linq;

namespace DTOs
{
    /// <summary>
    /// Map from model to DTO.
    /// </summary>
    public static class GameMapper
    {
        /// <summary>
        /// From IGame to GameDto.
        /// </summary>
        /// <param name="game"><see cref="IGame"/></param>
        /// <returns><see cref="GameDto"/></returns>
        public static GameDto ToGameDto(this IGame game)
        {
            return new GameDto
            {
                Id = game.Id,
                PlayersId = game.Teams
                    .SelectMany(t => t.Players.Select(p => p.Id))
                    .ToList()
            };
        }
    }
}
