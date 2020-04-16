using Domain.Enums;
using EventHandlers.Validator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Commands.SearchGame
{
    /// <summary>
    /// Validator for <see cref="SearchGameCommand"/>
    /// </summary>
    public class SearchGameCommandValidator : AbstractValidator<SearchGameCommand>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchGameCommandValidator()
        {
            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.GameType).Custom(EnumValidator.Validate<GamesEnum>);
        }
    }
}
