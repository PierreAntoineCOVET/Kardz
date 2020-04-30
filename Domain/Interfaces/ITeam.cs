using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface ITeam
    {
        IEnumerable<IPlayer> Players { get; }

        int Number { get; }

        void AddPlayer(IPlayer player);
    }
}
