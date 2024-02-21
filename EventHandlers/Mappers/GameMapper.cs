using Domain.Enums;
using Domain.Interfaces;
using DTOs;
using DTOs.Enums;
using DTOs.Shared;
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
                PlayersId = game.GetTeams()
                    .SelectMany(t => t.GetPlayers().Select(p => p.Id))
                    .ToList()
            };
        }

        /// <summary>
        /// From IGame to GameContractDto.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static GameContractDto ToContractDto(this IGame game, DTOs.Enums.CoincheCardColorsEnum? color, int? value)
        {
            var gameContract = game.GetContract();
            return new GameContractDto
            {
                Color = (int?)gameContract.Color,
                Value = gameContract.Value,
                LastPlayerNumber = game.LastPlayerNumber,
                CurrentPlayerNumber = game.CurrentPlayerNumber,
                ContractState = gameContract.CurrentState.ToDtoEnum(),
                IsContractCoinched = gameContract.CoincheState == Domain.Enums.ContractCoincheStatesEnum.Coinched,
                IsContractCounterCoinched = gameContract.CoincheState == Domain.Enums.ContractCoincheStatesEnum.CounterCoinched,
                TurnEndTime = game.CurrentTurnTimeout,
                LastColor = (int?)color,
                LastValue = value,
                OwningTeam = gameContract.OwningTeamNumber
            };
        }
    }
}
