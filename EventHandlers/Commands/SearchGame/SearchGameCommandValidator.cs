using Domain.Enums;
using FluentValidation;
using System;

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

            RuleFor(query => query.GameType).IsInEnum();
        }
    }
}
