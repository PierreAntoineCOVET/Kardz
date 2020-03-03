using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;

namespace EventHandlers.Queries.GetShuffledCards
{
    public class GetShuffledCardsQueryValidator : AbstractValidator<GetShuffledCardsQuery>
    {
        public GetShuffledCardsQueryValidator()
        {
            RuleFor(query => query.GameType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
