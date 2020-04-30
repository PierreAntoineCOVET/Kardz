using Domain.Domain.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.Interfaces
{
    public interface ITeam
    {
        IEnumerable<IPlayer> Players { get; }

        int Number { get; }

        void AddPlayer(IPlayer player);
    }
}
