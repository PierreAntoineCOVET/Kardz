using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    /// <summary>
    /// Contrats for all game's type cards deck.
    /// </summary>
    public interface ICardsDeck
    {
        IEnumerable<CardsEnum> Shuffle();
    }
}
