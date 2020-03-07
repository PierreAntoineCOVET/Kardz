using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.SearchGame
{
    public class SearchGameCommandValidator : AbstractValidator<SearchGameCommand>
    {
        public SearchGameCommandValidator()
        {
            RuleFor(query => query.PlayerId).NotNull();

            RuleFor(query => query.GamesType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
