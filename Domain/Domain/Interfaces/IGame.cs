using Domain.Domain.Implementations;
using Domain.Enums;
using Repositories.EventStoreEntities;
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

        bool IsPlaying { get; }

        void AddPlayers(IEnumerable<IPlayer> players);

        IEnumerable<CardsEnum> GetCardsForPlayer(Guid playerId);

        Task ShuffleCards();

        IEnumerable<CardsEnum> Cards { get; }
    }
}
