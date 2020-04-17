using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public interface IPlayer
    {
        Guid Id { get; }

        int Number { get; set; }

        IEnumerable<CardsEnum> Cards { get; set; }
    }
}
