using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Player contract.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Player's id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Player's number.
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Player's cards.
        /// </summary>
        IEnumerable<CardsEnum> Cards { get; set; }
    }
}
