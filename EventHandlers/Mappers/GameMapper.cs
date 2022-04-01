using Domain.Enums;
using Domain.Interfaces;
using DTOs;
using DTOs.Shared;
using System;
using System.Linq;

namespace EventHandlers.Mappers
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

        /// <summary>
        /// From IGame to GameContractDto.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static GameContractDto ToContractDto(this IGame game, DateTimeOffset turnTimerEnd, ContractState contractState)
        {
            return new GameContractDto
            {
                Color = (int?)game.Contract.Color,
                Value = game.Contract.Value,
                LastPlayerNumber = game.LastPlayerNumber,
                CurrentPlayerNumber = game.CurrentPlayerNumber,
                HasContractFailed = contractState == ContractState.Failed,
                TurnEndTime = turnTimerEnd
            };
        }
    }
}
