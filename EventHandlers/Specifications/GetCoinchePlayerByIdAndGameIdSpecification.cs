using Domain.Entities.ReadEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlers.Specifications
{
    internal class GetCoinchePlayerByIdAndGameIdSpecification : BaseSpecification<CoinchePlayer>
    {
        public GetCoinchePlayerByIdAndGameIdSpecification(Guid playerId, Guid gameId)
            : base(p => p.Id == playerId && p.Team.GameId == gameId)
        {
        }
    }
}
