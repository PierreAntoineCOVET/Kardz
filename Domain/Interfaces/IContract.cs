using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IContract
    {
        /// <summary>
        /// Get the contract current color.
        /// </summary>
        /// <returns></returns>
        ColorEnum? GetColor();

        /// <summary>
        /// Get the contract current value.
        /// </summary>
        /// <returns></returns>
        int? GetValue();

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
        bool ForceGameRedistribution(ColorEnum? color, int? value);

        /// <summary>
        /// Set color and value to the contract.
        /// </summary>
        /// <param name="color">Contract color</param>
        /// <param name="value">Contract value</param>
        void SetContract(ColorEnum? color, int? value);
    }
}
