using Domain.Domain.Interfaces;
using Domain.Tools;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                Players = game.Teams
                    .SelectMany(t => t.Players)
                    .Select(p => Crypto.ComputeSha256Hash(p.Id.ToString()))
                    .ToList()
            };
        }
    }
}
