using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetPlayerCards
{
    public class GetPlayerCardsQueryValidator : AbstractValidator<GetPlayerCardsQuery>
    {
        public GetPlayerCardsQueryValidator()
        {
            RuleFor(query => query.GameId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));
        }
    }
}
