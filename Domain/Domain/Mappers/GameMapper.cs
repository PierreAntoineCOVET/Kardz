using Domain.Domain.Interfaces;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public static class GameMapper
    {
        public static GameDto ToGameDto(this IGame game)
        {
            return new GameDto
            {
                Id = game.Id,
                // TODO: Hash player id to avoid cheating :
                // client need to see if the starting game is his, so he need to compare the players id. 
                // We will use hashed id so the client never realy know the id of the players with him.
                Players = game.Teams
                    .SelectMany(t => t.Players)
                    .Select(p => p.Id.ToString())
                    .ToList()
            };
        }
    }
}
