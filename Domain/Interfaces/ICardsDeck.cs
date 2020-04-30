using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Contrats for all game's type cards deck.
    /// </summary>
    public interface ICardsDeck
    {
        IEnumerable<CardsEnum> Shuffle();
    }
}
