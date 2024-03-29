﻿using Domain.Entities.ReadEntities;
using System;

namespace EventHandlers.Specifications
{
    public class GetFullCoincheGameByIdSpecification : BaseSpecification<CoincheGame>
    {
        public GetFullCoincheGameByIdSpecification(Guid gameId)
            : base(g => g.Id == gameId)
        {
            AddInclude($"{nameof(CoincheGame.Teams)}.{nameof(CoincheTeam.Players)}");
        }
    }
}
