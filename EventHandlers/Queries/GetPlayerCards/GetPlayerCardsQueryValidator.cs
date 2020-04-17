using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHandlers.Queries.GetPlayerCards
{
    /// <summary>
    /// Validator for <see cref="GetPlayerCardsQuery"/>.
    /// </summary>
    public class GetPlayerCardsQueryValidator : AbstractValidator<GetPlayerCardsQuery>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GetPlayerCardsQueryValidator()
        {
            RuleFor(query => query.GameId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));
        }
    }
}
