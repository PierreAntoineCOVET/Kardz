using System.Collections.Generic;

namespace Domain.Interfaces
{
    // TODO: realy usefull ? use directly CoincheTeam inside domain ?
    public interface ITeam
    {
        IEnumerable<IPlayer> GetPlayers();

        int Number { get; }

        void AddPlayer(IPlayer player);
    }
}
