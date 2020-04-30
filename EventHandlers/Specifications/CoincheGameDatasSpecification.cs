using Repositories.ReadEntities;
using System;
using System.Collections.Generic;
using System.Text;

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
