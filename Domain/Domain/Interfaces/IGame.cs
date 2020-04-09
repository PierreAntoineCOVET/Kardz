using Domain.Domain.Implementations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Interfaces
{
    public interface IGame
    {
        Guid Id { get; }

        IEnumerable<ITeam> Teams { get; }

        void AddPlayers(IEnumerable<IPlayer> players);

        Task<IEnumerable<CardsEnum>> GetCardsForPlayer(Guid playerId);
    }
}
