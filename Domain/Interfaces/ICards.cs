using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICards
    {
        /// <summary>
        /// Get a card color and value <see cref="CardsEnum"/>.
        /// </summary>
        /// <returns></returns>
        CardsEnum Card { get; }
    }
}
