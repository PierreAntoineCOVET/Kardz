using Domain.Entities.ReadEntities;
using System;

namespace EventHandlers.Specifications
{
    public class CoincheGameDatasSpecification : BaseSpecification<CoincheGame>
    {
        public CoincheGameDatasSpecification(Guid gameId)
            : base(c => c.Id == gameId)
        {
            AddInclude($"{nameof(CoincheGame.Teams)}.{nameof(CoincheTeam.Players)}");
        }
    }
}
