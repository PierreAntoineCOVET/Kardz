using Domain.Entities.ReadEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlers.Specifications
{
    internal class GetCoinchePlayerByIdSpecification : BaseSpecification<CoinchePlayer>
    {
        public GetCoinchePlayerByIdSpecification(Guid playerId)
            : base(p => p.Id == playerId)
        {
        }
    }
}
