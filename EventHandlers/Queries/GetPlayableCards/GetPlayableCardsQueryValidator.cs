using EventHandlers.Queries.GetPlayerCards;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlers.Queries.GetPlayableCards
{
    /// <summary>
    /// Validate <see cref="GetPlayableCardsQuery"/>.
    /// </summary>
    public class GetPlayableCardsQueryValidator: AbstractValidator<GetPlayableCardsQuery>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GetPlayableCardsQueryValidator()
        {
            RuleFor(query => query.GameId).NotNull().NotEqual(default(Guid));

            RuleFor(query => query.PlayerId).NotNull().NotEqual(default(Guid));
        }
    }
}
