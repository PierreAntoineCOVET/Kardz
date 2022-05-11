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
        /// Transormf to a unique <see cref="CardsEnum"/> value.
        /// </summary>
        /// <returns></returns>
        CardsEnum ToCardEnum();
    }
}
