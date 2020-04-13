using Domain.Domain.Implementations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.Interfaces
{
    public interface IGame : IAggregate
    {
        new Guid Id { get; }

        bool IsPlaying { get; }

        IEnumerable<ITeam> Teams { get; }

        IEnumerable<CardsEnum> GetCardsForPlayer(Guid playerId);

        void ShuffleCards();
    }
}
