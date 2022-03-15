using Domain.Enums;
using Domain.Events;

namespace Domain.Interfaces
{
    public interface IContract
    {
        /// <summary>
        /// Get the contract current color.
        /// </summary>
        /// <returns></returns>
        ColorEnum? Color { get; }

        /// <summary>
        /// Get the contract current value.
        /// </summary>
        /// <returns></returns>
        int? Value { get; }

        /// <summary>
        /// True if the last player has passed (regarding of contract color and  value).
        /// </summary>
        /// <returns></returns>
        bool HasLastPlayerPassed();

        /// <summary>
        /// Return true if the game need to redistribute the cards.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsContractFailed(ColorEnum? color, int? value);

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        /// <returns></returns>
        ContractMadeEvent GetContractMadeEvent(ColorEnum? color, int? value);

        /// <summary>
        /// Apply the event contract information to current contract instance.
        /// </summary>
        /// <param name="event"></param>
        void Apply(ContractMadeEvent @event);
    }
}
