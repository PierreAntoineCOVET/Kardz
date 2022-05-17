using Domain.Enums;
using Domain.Events;
using System;

namespace Domain.Interfaces
{
    public interface IContract
    {
        /// <summary>
        /// Get the contract current color.
        /// </summary>
        /// <returns></returns>
        CoincheCardColorsEnum? Color { get; }

        /// <summary>
        /// Get the contract current value.
        /// </summary>
        /// <returns></returns>
        int? Value { get; }

        /// <summary>
        /// Get the contract current state.
        /// </summary>
        ContractStatesEnum CurrentState { get; }

        /// <summary>
        /// Get the coinche state.
        /// </summary>
        ContractCoincheStatesEnum CoincheState { get; }
    }
}
