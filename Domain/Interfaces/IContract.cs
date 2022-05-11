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
        /// Return the state of the contract if we apply the given values.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <param name="coinched"></param>
        /// <returns></returns>
        ContractStatesEnum GetNextState(CoincheCardColorsEnum? color, int? value, bool coinched);

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <param name="player">player submiting the contract</param>
        /// <returns></returns>
        ContractMadeEvent GetContractMadeEvent(CoincheCardColorsEnum? color, int? value, Guid player, bool coinched);

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        void Apply(ContractMadeEvent @event);

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        void Apply(ContractFailedEvent @event);

        /// <summary>
        /// Return true if the the next player in line should be skipped (his part just coinched).
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        bool ShouldSkipNextPlayer(ContractMadeEvent @event);
    }
}
